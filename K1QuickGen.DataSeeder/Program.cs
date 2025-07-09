using K1QuickGen.Contracts.Dtos;
using System.Net.Http;
using System.Net.Http.Json;

namespace K1QuickGen.DataSeeder
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:64916"); // Update your port if needed

            await WaitForApiReady(client, "/swagger/index.html");

            var companyId = Guid.NewGuid();

            // Step 1: Create Form1065
            var formDto = new Form1065CreateDto
            {
                CompanyId = companyId,
                CompanyName = "Acme Software LLC",
                TaxYear = 2024,
                BusinessActivity = "Software Development",
                ProductOrService = "Web Applications",
                BusinessCode = "541511",
                Address = "123 Demo Street",
                City = "Los Angeles",
                State = "CA",
                ZipCode = "90001",
                Country = "USA",
                DateBusinessStarted = new DateTime(2018, 3, 15),
                IsFinalReturn = false,
                IsAmendedReturn = false
            };

            Console.WriteLine("Posting Form1065...");
            Console.WriteLine($"companyId: {companyId}");
            var formResponse = await client.PostAsJsonAsync("/api/form1065", formDto);
            formResponse.EnsureSuccessStatusCode();
            var createdForm = await formResponse.Content.ReadFromJsonAsync<Form1065OutputDto>();

            Console.WriteLine($"Form1065 created: {createdForm.Form1065Id}");

            // === Seed Partners ===
            var partners = new[]
            {
                new PartnerSubmissionCreateDto
                {
                    CompanyId = companyId,
                    Form1065Id = createdForm.Form1065Id,
                    PartnerName = "John Doe",
                    PartnerType = "Individual",
                    OwnershipPercentage = 40m,
                    CapitalContribution = 40000m,
                    Email = "john.doe@example.com",
                    SSNOrEIN = "123-45-6789",
                    Address = "456 Elm St",
                    City = "Los Angeles",
                    State = "CA",
                    ZipCode = "90001",
                    Country = "USA",
                    IsForeignPartner = false,
                    IsTaxExemptEntity = false,
                    BeginningCapitalAccount = 40000m,
                    EndingCapitalAccount = 42000m,
                    ShareOfIncome = 8000m,
                    ShareOfDeductions = 1000m
                },
                new PartnerSubmissionCreateDto
                {
                    CompanyId = companyId,
                    Form1065Id = createdForm.Form1065Id,
                    PartnerName = "Jane Smith",
                    PartnerType = "Individual",
                    OwnershipPercentage = 35m,
                    CapitalContribution = 35000m,
                    Email = "jane.smith@example.com",
                    SSNOrEIN = "234-56-7890",
                    Address = "789 Oak Ave",
                    City = "Los Angeles",
                    State = "CA",
                    ZipCode = "90001",
                    Country = "USA",
                    IsForeignPartner = false,
                    IsTaxExemptEntity = false,
                    BeginningCapitalAccount = 35000m,
                    EndingCapitalAccount = 37000m,
                    ShareOfIncome = 6000m,
                    ShareOfDeductions = 900m
                },
                new PartnerSubmissionCreateDto
                {
                    CompanyId = companyId,
                    Form1065Id = createdForm.Form1065Id,
                    PartnerName = "Bob Chen",
                    PartnerType = "Individual",
                    OwnershipPercentage = 25m,
                    CapitalContribution = 25000m,
                    Email = "bob.chen@example.com",
                    SSNOrEIN = "345-67-8901",
                    Address = "321 Pine Rd",
                    City = "Los Angeles",
                    State = "CA",
                    ZipCode = "90001",
                    Country = "USA",
                    IsForeignPartner = false,
                    IsTaxExemptEntity = false,
                    BeginningCapitalAccount = 25000m,
                    EndingCapitalAccount = 26500m,
                    ShareOfIncome = 4000m,
                    ShareOfDeductions = 500m
                }
            };

            Console.WriteLine("👥 Posting Partners...");
            foreach (var partner in partners)
            {
                Console.WriteLine($"Posting partner: {partner.PartnerName}, CompanyId: {partner.CompanyId}, Form1065Id: {partner.Form1065Id}");
                var response = await client.PostAsJsonAsync("/api/partners", partner);
                response.EnsureSuccessStatusCode();
                Console.WriteLine($"✅ Partner posted: {partner.PartnerName}");
            }

            Console.WriteLine("🎉 Data seeding complete!");
        }

        static async Task WaitForApiReady(HttpClient client, string testUrl)
        {
            const int maxRetries = 10;
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    var response = await client.GetAsync(testUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("✅ API is ready!");
                        return;
                    }
                }
                catch
                {
                    Console.WriteLine("⏳ Waiting for API...");
                }
                await Task.Delay(1000); // Wait 1 sec before retry
            }

            throw new Exception("❌ API not responding after retries.");
        }
    }
}
