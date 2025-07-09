using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K1QuickGen.PdfGeneration.FieldMappings
{
    public static class Form1065FieldNames
    {
        // Header: Tax year range
        public const string TaxYearStart = "topmostSubform[0].Page1[0].PgHeader[0].f1_1[0]";   // MM/DD
        public const string TaxYearEnd = "topmostSubform[0].Page1[0].PgHeader[0].f1_2[0]";     // MM/DD
        public const string TaxYearSuffix = "topmostSubform[0].Page1[0].PgHeader[0].f1_3[0]";  // YY

        // Line A, B, C: Business Info
        public const string BusinessActivity = "topmostSubform[0].Page1[0].f1_4[0]";         // Box A
        public const string ProductOrService = "topmostSubform[0].Page1[0].f1_5[0]";         // Box B
        public const string BusinessCode = "topmostSubform[0].Page1[0].f1_6[0]";


























        // Header section
        public const string CompanyName = "topmostSubform[0].Page1[0].PgHeader[0].f1_1[0]";
        public const string TaxYear = "topmostSubform[0].Page1[0].PgHeader[0].f1_3[0]";

        // Line A – Business activity
        //public const string BusinessActivity = "topmostSubform[0].Page1[0].f1_4[0]";

        // Line B – Product or service
        //public const string ProductOrService = "topmostSubform[0].Page1[0].f1_5[0]";

        // Line C – Business code number
        //public const string BusinessCode = "topmostSubform[0].Page1[0].f1_6[0]";

        // Address
        public const string Address = "topmostSubform[0].Page1[0].f1_7[0]";
        public const string CityStateZip = "topmostSubform[0].Page1[0].f1_8[0]";
        public const string Country = "topmostSubform[0].Page1[0].f1_9[0]";

        // Date business started – Line E
        public const string DateBusinessStarted = "topmostSubform[0].Page1[0].f1_10[0]";

        // Final return checkbox – Line G
        public const string IsFinalReturnCheckbox = "topmostSubform[0].Page1[0].c1_1[0]";

        // Amended return checkbox – Line H
        public const string IsAmendedReturnCheckbox = "topmostSubform[0].Page1[0].c1_2[0]";

        // Partner data (used in separate K-1 generator typically, but listing here for reference)
        public const string PartnerName = "topmostSubform[0].Page5[0].f5_2[0]";
        public const string PartnerSSNOrEIN = "topmostSubform[0].Page5[0].f5_3[0]";
        public const string PartnerAddress = "topmostSubform[0].Page5[0].f5_4[0]";
        public const string PartnerCityStateZip = "topmostSubform[0].Page5[0].f5_5[0]";
        public const string PartnerIsForeign = "topmostSubform[0].Page5[0].c5_2[0]";
        public const string PartnerIsTaxExempt = "topmostSubform[0].Page5[0].c5_3[0]";
        public const string CapitalContribution = "topmostSubform[0].Page5[0].f5_6[0]";
        public const string BeginningCapitalAccount = "topmostSubform[0].Page5[0].f5_7[0]";
        public const string EndingCapitalAccount = "topmostSubform[0].Page5[0].f5_8[0]";
        public const string ShareOfIncome = "topmostSubform[0].Page5[0].f5_9[0]";
        public const string ShareOfDeductions = "topmostSubform[0].Page5[0].f5_10[0]";
    }
}
