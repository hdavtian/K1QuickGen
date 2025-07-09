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

            //Set(fields, Form1065FieldNames.temp1, "1");
            //Set(fields, Form1065FieldNames.temp2, "2");
            //Set(fields, Form1065FieldNames.temp3, "3");
            //Set(fields, Form1065FieldNames.temp4, "4");
            //Set(fields, Form1065FieldNames.temp5, "5");
            //Set(fields, Form1065FieldNames.temp6, "6");
            //Set(fields, Form1065FieldNames.temp7, "7");
            //Set(fields, Form1065FieldNames.temp8, "8");
            //Set(fields, Form1065FieldNames.temp9, "9");
            //Set(fields, Form1065FieldNames.temp10, "10");
            //Set(fields, Form1065FieldNames.temp11, "11");
            //Set(fields, Form1065FieldNames.temp12, "12");
            //Set(fields, Form1065FieldNames.temp13, "13");
            //Set(fields, Form1065FieldNames.temp14, "14");
            //Set(fields, Form1065FieldNames.temp15, "15");
            //Set(fields, Form1065FieldNames.temp16, "16");
            //Set(fields, Form1065FieldNames.temp17, "17");
            //Set(fields, Form1065FieldNames.temp18, "18");
            //Set(fields, Form1065FieldNames.temp19, "19");
            //Set(fields, Form1065FieldNames.temp20, "20");
            //Set(fields, Form1065FieldNames.temp21, "21");
            //Set(fields, Form1065FieldNames.temp22, "22");
            //Set(fields, Form1065FieldNames.temp23, "23");
            //Set(fields, Form1065FieldNames.temp24, "24");
            //Set(fields, Form1065FieldNames.temp25, "25");
            //Set(fields, Form1065FieldNames.temp26, "26");
            //Set(fields, Form1065FieldNames.temp27, "27");
            //Set(fields, Form1065FieldNames.temp28, "28");
            //Set(fields, Form1065FieldNames.temp29, "29");
            //Set(fields, Form1065FieldNames.temp30, "30");
            //Set(fields, Form1065FieldNames.temp31, "31");
            //Set(fields, Form1065FieldNames.temp32, "32");
            //Set(fields, Form1065FieldNames.temp33, "33");
            //Set(fields, Form1065FieldNames.temp34, "34");
            //Set(fields, Form1065FieldNames.temp35, "35");
            //Set(fields, Form1065FieldNames.temp36, "36");
            //Set(fields, Form1065FieldNames.temp37, "37");
            //Set(fields, Form1065FieldNames.temp38, "38");
            //Set(fields, Form1065FieldNames.temp39, "39");
            //Set(fields, Form1065FieldNames.temp40, "40");
            //Set(fields, Form1065FieldNames.temp41, "41");
            //Set(fields, Form1065FieldNames.temp42, "42");
            //Set(fields, Form1065FieldNames.temp43, "43");
            //Set(fields, Form1065FieldNames.temp44, "44");
            //Set(fields, Form1065FieldNames.temp45, "45");
            //Set(fields, Form1065FieldNames.temp46, "46");
            //Set(fields, Form1065FieldNames.temp47, "47");
            //Set(fields, Form1065FieldNames.temp48, "48");
            //Set(fields, Form1065FieldNames.temp49, "49");
            //Set(fields, Form1065FieldNames.temp50, "50");
            //Set(fields, Form1065FieldNames.temp51, "51");
            //Set(fields, Form1065FieldNames.temp52, "52");
            //Set(fields, Form1065FieldNames.temp53, "53");
            //Set(fields, Form1065FieldNames.temp54, "54");
            //Set(fields, Form1065FieldNames.temp55, "55");
            //Set(fields, Form1065FieldNames.temp56, "56");
            //Set(fields, Form1065FieldNames.temp57, "57");
            //Set(fields, Form1065FieldNames.temp58, "58");
            //Set(fields, Form1065FieldNames.temp59, "59");
            //Set(fields, Form1065FieldNames.temp60, "60");
            //Set(fields, Form1065FieldNames.temp61, "61");
            //Set(fields, Form1065FieldNames.temp62, "62");
            //Set(fields, Form1065FieldNames.temp63, "63");
            //Set(fields, Form1065FieldNames.temp64, "64");
            //Set(fields, Form1065FieldNames.temp65, "65");
            //Set(fields, Form1065FieldNames.temp66, "66");
            //Set(fields, Form1065FieldNames.temp67, "67");
            //Set(fields, Form1065FieldNames.temp68, "68");
            //Set(fields, Form1065FieldNames.temp69, "69");
            //Set(fields, Form1065FieldNames.temp70, "70");
            //Set(fields, Form1065FieldNames.temp71, "71");
            //Set(fields, Form1065FieldNames.temp72, "72");
            //Set(fields, Form1065FieldNames.temp73, "73");
            //Set(fields, Form1065FieldNames.temp74, "74");
            //Set(fields, Form1065FieldNames.temp75, "75");

            // Top row: tax year range
            var start = new DateTime(data.TaxYear, 1, 1);
            var end = new DateTime(data.TaxYear, 12, 31);

            Set(fields, Form1065FieldNames.TaxYearStart, start.ToString("MM/dd"));
            Set(fields, Form1065FieldNames.TaxYearEnd, end.ToString("MM/dd"));
            Set(fields, Form1065FieldNames.TaxYearSuffix, new DateTime(data.TaxYear, 1, 1).ToString("yy"));

            Set(fields, Form1065FieldNames.BusinessActivity, data.BusinessActivity);
            Set(fields, Form1065FieldNames.ProductOrService, data.ProductOrService);
            Set(fields, Form1065FieldNames.BusinessCode, data.BusinessCode);

            Set(fields, Form1065FieldNames.PartnershipName, data.CompanyName);
            Set(fields, Form1065FieldNames.PartnershipAddress, data.Address);
            var cityZipCountry = $"{data.City} {data.State} {data.ZipCode} {data.Country}";
            Set(fields, Form1065FieldNames.CityZipCountry, cityZipCountry);

            //Set(fields, Form1065FieldNames.Ein, data.E);
            //Set(fields, Form1065FieldNames.DateBusinessStarted, cityZipCountry);
            //Set(fields, Form1065FieldNames.TotalAssets, cityZipCountry);









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


