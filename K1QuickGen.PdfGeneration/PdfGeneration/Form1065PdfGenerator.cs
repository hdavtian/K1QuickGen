using K1QuickGen.Contracts.Dtos;
using K1QuickGen.PdfGeneration.Interfaces;
using K1QuickGen.PdfGeneration.PdfGeneration;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace K1QuickGen.PdfGeneration
{
    public class Form1065PdfGenerator : IForm1065PdfGenerator
    {
        public byte[] GeneratePdf(Form1065OutputDto data)
        {
            var document = new Form1065PdfDocument(data);
            return document.GeneratePdf();
        }
    }
}
