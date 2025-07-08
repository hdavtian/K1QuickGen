using K1QuickGen.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K1QuickGen.PdfGeneration.Interfaces
{
    public interface IForm1065PdfGenerator
    {
        byte[] GeneratePdf(Form1065OutputDto data);
    }
}
