using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using K1QuickGen.Contracts.Dtos;
using K1QuickGen.PdfGeneration.FieldMappings;
using K1QuickGen.PdfGeneration.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace K1QuickGen.PdfGeneration
{
    public class Form1065PdfGenerator : IForm1065PdfGenerator
    {
        public byte[] GeneratePdf(Form1065OutputDto data)
        {
            var templatePath = Path.Combine(AppContext.BaseDirectory, "PdfTemplates", "2024", "f1065.pdf");

            using var ms = new MemoryStream();
            using var pdfReader = new PdfReader(templatePath);
            using var pdfWriter = new PdfWriter(ms);
            using var pdfDoc = new PdfDocument(pdfReader, pdfWriter);

            var form = PdfAcroForm.GetAcroForm(pdfDoc, true);
            var fields = form.GetAllFormFields();

            // Top row: tax year range
            var start = new DateTime(data.TaxYear, 1, 1);
            var end = new DateTime(data.TaxYear, 12, 31);

            Set(fields, Form1065FieldNames.TaxYearStart, start.ToString("MM/dd"));
            Set(fields, Form1065FieldNames.TaxYearEnd, end.ToString("MM/dd"));
            Set(fields, Form1065FieldNames.TaxYearSuffix, data.TaxYear.ToString("yy"));

            Set(fields, Form1065FieldNames.BusinessActivity, data.BusinessActivity);
            Set(fields, Form1065FieldNames.ProductOrService, data.ProductOrService);
            Set(fields, Form1065FieldNames.BusinessCode, data.BusinessCode);


            // === Form1065 Fields ===
            //Set(fields, Form1065FieldNames.CompanyName, data.CompanyName);
            //Set(fields, Form1065FieldNames.TaxYear, data.TaxYear.ToString());
            //Set(fields, Form1065FieldNames.BusinessActivity, data.BusinessActivity);
            //Set(fields, Form1065FieldNames.ProductOrService, data.ProductOrService);
            //Set(fields, Form1065FieldNames.BusinessCode, data.BusinessCode);
            //Set(fields, Form1065FieldNames.Address, data.Address);
            //Set(fields, Form1065FieldNames.CityStateZip, $"{data.City}, {data.State} {data.ZipCode}");
            //Set(fields, Form1065FieldNames.Country, data.Country);
            //Set(fields, Form1065FieldNames.DateBusinessStarted, data.DateBusinessStarted?.ToString("MM/dd/yyyy"));

            //SetCheckbox(fields, Form1065FieldNames.IsFinalReturnCheckbox, data.IsFinalReturn);
            //SetCheckbox(fields, Form1065FieldNames.IsAmendedReturnCheckbox, data.IsAmendedReturn);

            // === Partner Fields (optional preview – real K-1 generator will handle these) ===
            /*
            if (data.PartnerK1s?.Any() == true)
            {
                var firstPartner = data.PartnerK1s.First();
                Set(fields, Form1065FieldNames.PartnerName, firstPartner.PartnerName);
                Set(fields, Form1065FieldNames.CapitalContribution, firstPartner.CapitalContribution.ToString("F2"));
                // Add more if needed
            }
            */

            form.FlattenFields();
            pdfDoc.Close();

            return ms.ToArray();
        }

        private void Set(IDictionary<string, PdfFormField> fields, string key, string? value)
        {
            if (value != null && fields.ContainsKey(key))
                fields[key].SetValue(value);
        }

        private void SetCheckbox(IDictionary<string, PdfFormField> fields, string key, bool isChecked)
        {
            if (fields.ContainsKey(key))
                fields[key].SetValue(isChecked ? "Yes" : "Off");
        }
    }
}


