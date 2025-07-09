using iText.Forms;
using iText.Kernel.Pdf;
using System.Reflection.PortableExecutable;

//var templatePath = Path.Combine(AppContext.BaseDirectory, "2024" , "f1065.pdf");
var templatePath = @"C:\Sites\K1QuickGen\K1QuickGen.PdfGeneration\PdfTemplates\2024\f1065.pdf";

if (!File.Exists(templatePath))
{
    Console.WriteLine("PDF file not found: " + templatePath);
    return;
}

using var pdfReader = new PdfReader(templatePath);
using var pdfDoc = new PdfDocument(pdfReader);

var form = PdfAcroForm.GetAcroForm(pdfDoc, false);
var fields = form.GetAllFormFields();

Console.WriteLine("📄 Fields in the PDF:");
foreach (var field in fields)
{
    Console.WriteLine($"- {field.Key}");
}
