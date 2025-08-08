<#
.SYNOPSIS
  Generates PlantUML class diagrams for each C# project in the solution,
  then renders them to PNG and SVG images.

.DESCRIPTION
  1. Finds all `.csproj` files in the solution (optionally skipping test projects).
  2. Runs the `puml-gen` CLI tool (PlantUmlClassDiagramGenerator) for each project:
     - Generates `.puml` files for each class in the project.
     - Creates an `include.puml` that aggregates all classes in that project.
     - Applies custom diagram settings from a `header.puml` file.
  3. Uses the local `plantuml.jar` to render each `.puml` file to:
     - PNG (bitmap, for quick viewing in image viewers)
     - SVG (vector, scalable without loss of quality)
  4. Stores all generated files in a `uml` folder inside each project directory.

.PARAMETER OutputFolderName
  Name of the folder inside each project where UML output will be stored (default: "uml").

.PARAMETER SkipTests
  Skips any project whose `.csproj` filename matches `*.Tests.csproj`.

.PARAMETER PlantUmlJarPath
  Full path to the `plantuml.jar` file used for rendering diagrams.

.PARAMETER HeaderFile
  Path to a `.puml` file containing shared PlantUML `skinparam` or style settings
  to be injected into all generated diagrams.

.NOTES
  Requirements:
    - .NET SDK and `puml-gen` installed globally:
        dotnet tool install --global PlantUmlClassDiagramGenerator
    - Java installed and available on PATH.
    - `plantuml.jar` downloaded from the official PlantUML releases.
    - (Optional) Graphviz installed for improved diagram layouts.

  Example run:
    pwsh -File .\tools\generate-uml.ps1 -SkipTests

  Output:
    Per-project UML diagrams located in:
      <ProjectFolder>\uml\*.puml
      <ProjectFolder>\uml\*.png
      <ProjectFolder>\uml\*.svg
#>

param(
  [string]$OutputFolderName = "uml",
  [switch]$SkipTests,
  [string]$PlantUmlJarPath = ".\plantuml.jar",
  [string]$HeaderFile = ".\header.puml"
)

# Repo root = parent of the /tools folder (two levels up from this script)
$RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path

$ErrorActionPreference = "Stop"

# Locate puml-gen even if PATH isn't updated
$toolCmd = Get-Command puml-gen -ErrorAction SilentlyContinue
$tool = if ($toolCmd) { $toolCmd.Source } else { "$env:USERPROFILE\.dotnet\tools\puml-gen.exe" }

if (-not (Test-Path $tool)) {
  Write-Error "❌ puml-gen not found. Install with: dotnet tool install --global PlantUmlClassDiagramGenerator"
}

if (-not (Test-Path $PlantUmlJarPath)) {
  Write-Error "❌ plantuml.jar not found at: $PlantUmlJarPath"
}

if (-not (Test-Path $HeaderFile)) {
  Write-Error "❌ header.puml not found at: $HeaderFile"
}

# Get all .csproj files (optional test filter)
$projects = Get-ChildItem -Path $RepoRoot -Recurse -Filter *.csproj -File | Where-Object {
  if ($SkipTests) { $_.Name -notmatch '\.Tests\.csproj$' } else { $true }
}

if (-not $projects) {
  Write-Warning "No .csproj files found."
  exit 0
}

foreach ($proj in $projects) {
  $projDir = Split-Path $proj.FullName -Parent
  $outDir  = Join-Path $projDir $OutputFolderName
  if (-not (Test-Path $outDir)) { New-Item -ItemType Directory -Path $outDir | Out-Null }

  Write-Host "`n🔄 Generating UML for $($proj.Name) -> $outDir"
  & "$tool" $projDir $outDir `
    -dir `
    -excludePaths "bin,obj,Properties,.vs,.git,packages,node_modules" `
    -allInOne `
    -headerFile "$(Resolve-Path $HeaderFile)"

  if ($LASTEXITCODE -ne 0) {
    Write-Warning "⚠️ puml-gen failed for $($proj.FullName) (exit $LASTEXITCODE)"
    continue
  }

  # Render all .puml files to PNG and SVG
  $pumlFiles = Get-ChildItem -Path $outDir -Filter *.puml
  foreach ($puml in $pumlFiles) {
    Write-Host "🖼️ Rendering: $($puml.Name)"
    java -jar $PlantUmlJarPath -tpng $puml.FullName
    java -jar $PlantUmlJarPath -tsvg $puml.FullName
  }
}

Write-Host "`n✅ Done generating and rendering all project UML diagrams."
