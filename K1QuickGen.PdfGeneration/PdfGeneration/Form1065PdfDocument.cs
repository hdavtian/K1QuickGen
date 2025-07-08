using K1QuickGen.Contracts.Dtos;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K1QuickGen.PdfGeneration.PdfGeneration
{
    public class Form1065PdfDocument : IDocument
    {
        private readonly Form1065OutputDto _data;

        public Form1065PdfDocument(Form1065OutputDto data)
        {
            _data = data;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Content().Column(col =>
                {
                    col.Item().Text($"Form 1065 – {_data.CompanyName}").FontSize(20).Bold();

                    // Fix: Wrap the Text() call in a container to apply PaddingBottom
                    col.Item().PaddingBottom(10).Text($"Tax Year: {_data.TaxYear}").FontSize(14);

                    col.Item().Text("Partners").FontSize(16).Bold().Underline();

                    foreach (var partner in _data.PartnerK1s)
                    {
                        col.Item().PaddingTop(10).Text(txt =>
                        {
                            txt.Span($"Name: ").SemiBold(); txt.Span(partner.PartnerName);
                            txt.Line(""); // Fix: Provide an empty string as the required 'text' parameter
                            txt.Span($"Type: ").SemiBold(); txt.Span(partner.PartnerType);
                            txt.Line(""); // Fix: Provide an empty string as the required 'text' parameter
                            txt.Span($"Ownership: ").SemiBold(); txt.Span($"{partner.OwnershipPercentage}%");
                            txt.Line(""); // Fix: Provide an empty string as the required 'text' parameter
                            txt.Span($"Capital: ").SemiBold(); txt.Span($"${partner.CapitalContribution}");
                            txt.Line(""); // Fix: Provide an empty string as the required 'text' parameter
                            txt.Span($"Email: ").SemiBold(); txt.Span(partner.Email ?? "(N/A)");
                        });
                    }
                });
            });
        }
    }
}
