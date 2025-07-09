using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K1QuickGen.PdfGeneration.FieldMappings
{
    public static class Form1065FieldNames
    {
        /*
        public const string temp1 = "topmostSubform[0]";
        public const string temp2 = "topmostSubform[0].Page1[0]";
        public const string temp3 = "topmostSubform[0].Page1[0].PgHeader[0]";
        public const string temp4 = "topmostSubform[0].Page1[0].PgHeader[0].f1_1[0]";
        public const string temp5 = "topmostSubform[0].Page1[0].PgHeader[0].f1_2[0]";
        public const string temp6 = "topmostSubform[0].Page1[0].PgHeader[0].f1_3[0]";
        public const string temp7 = "topmostSubform[0].Page1[0].TypeOrPrint[0]";
        public const string temp8 = "topmostSubform[0].Page1[0].TypeOrPrint[0].f1_4[0]";
        public const string temp9 = "topmostSubform[0].Page1[0].TypeOrPrint[0].f1_5[0]";
        public const string temp10 = "topmostSubform[0].Page1[0].TypeOrPrint[0].f1_6[0]";
        public const string temp11 = "topmostSubform[0].Page1[0].LinesABC[0]";
        public const string temp12 = "topmostSubform[0].Page1[0].LinesABC[0].f1_7[0]";
        public const string temp13 = "topmostSubform[0].Page1[0].LinesABC[0].f1_8[0]";
        public const string temp14 = "topmostSubform[0].Page1[0].LinesABC[0].f1_9[0]";
        public const string temp15 = "topmostSubform[0].Page1[0].f1_10[0]";
        public const string temp16 = "topmostSubform[0].Page1[0].f1_11[0]";
        public const string temp17 = "topmostSubform[0].Page1[0].f1_12[0]";
        public const string temp18 = "topmostSubform[0].Page1[0].c1_1[0]";
        public const string temp19 = "topmostSubform[0].Page1[0].c1_2[0]";
        public const string temp20 = "topmostSubform[0].Page1[0].c1_3[0]";
        public const string temp21 = "topmostSubform[0].Page1[0].c1_4[0]";
        public const string temp22 = "topmostSubform[0].Page1[0].c1_5[0]";
        public const string temp23 = "topmostSubform[0].Page1[0].c1_6[0]";
        public const string temp24 = "topmostSubform[0].Page1[0].c1_6[1]";
        public const string temp25 = "topmostSubform[0].Page1[0].c1_6[2]";
        public const string temp26 = "topmostSubform[0].Page1[0].f1_13[0]";
        public const string temp27 = "topmostSubform[0].Page1[0].f1_14[0]";
        public const string temp28 = "topmostSubform[0].Page1[0].c1_9[0]";
        public const string temp29 = "topmostSubform[0].Page1[0].c1_10[0]";
        public const string temp30 = "topmostSubform[0].Page1[0].c1_11[0]";
        public const string temp31 = "topmostSubform[0].Page1[0].f1_15[0]";
        public const string temp32 = "topmostSubform[0].Page1[0].f1_16[0]";
        public const string temp33 = "topmostSubform[0].Page1[0].f1_17[0]";
        public const string temp34 = "topmostSubform[0].Page1[0].f1_18[0]";
        public const string temp35 = "topmostSubform[0].Page1[0].f1_19[0]";
        public const string temp36 = "topmostSubform[0].Page1[0].f1_20[0]";
        public const string temp37 = "topmostSubform[0].Page1[0].f1_21[0]";
        public const string temp38 = "topmostSubform[0].Page1[0].f1_22[0]";
        public const string temp39 = "topmostSubform[0].Page1[0].f1_23[0]";
        public const string temp40 = "topmostSubform[0].Page1[0].f1_24[0]";
        public const string temp41 = "topmostSubform[0].Page1[0].f1_25[0]";
        public const string temp42 = "topmostSubform[0].Page1[0].f1_26[0]";
        public const string temp43 = "topmostSubform[0].Page1[0].f1_27[0]";
        public const string temp44 = "topmostSubform[0].Page1[0].f1_28[0]";
        public const string temp45 = "topmostSubform[0].Page1[0].f1_29[0]";
        public const string temp46 = "topmostSubform[0].Page1[0].f1_30[0]";
        public const string temp47 = "topmostSubform[0].Page1[0].f1_31[0]";
        public const string temp48 = "topmostSubform[0].Page1[0].f1_32[0]";
        public const string temp49 = "topmostSubform[0].Page1[0].f1_33[0]";
        public const string temp50 = "topmostSubform[0].Page1[0].f1_34[0]";
        public const string temp51 = "topmostSubform[0].Page1[0].f1_35[0]";
        public const string temp52 = "topmostSubform[0].Page1[0].f1_36[0]";
        public const string temp53 = "topmostSubform[0].Page1[0].f1_37[0]";
        public const string temp54 = "topmostSubform[0].Page1[0].f1_38[0]";
        public const string temp55 = "topmostSubform[0].Page1[0].f1_39[0]";
        public const string temp56 = "topmostSubform[0].Page1[0].f1_40[0]";
        public const string temp57 = "topmostSubform[0].Page1[0].f1_41[0]";
        public const string temp58 = "topmostSubform[0].Page1[0].f1_42[0]";
        public const string temp59 = "topmostSubform[0].Page1[0].f1_43[0]";
        public const string temp60 = "topmostSubform[0].Page1[0].f1_44[0]";
        public const string temp61 = "topmostSubform[0].Page1[0].f1_45[0]";
        public const string temp62 = "topmostSubform[0].Page1[0].f1_46[0]";
        public const string temp63 = "topmostSubform[0].Page1[0].f1_47[0]";
        public const string temp64 = "topmostSubform[0].Page1[0].f1_48[0]";
        public const string temp65 = "topmostSubform[0].Page1[0].f1_49[0]";
        public const string temp66 = "topmostSubform[0].Page1[0].f1_50[0]";
        public const string temp67 = "topmostSubform[0].Page1[0].c1_12[0]";
        public const string temp68 = "topmostSubform[0].Page1[0].c1_12[1]";
        public const string temp69 = "topmostSubform[0].Page1[0].f1_49[1]";
        public const string temp70 = "topmostSubform[0].Page1[0].c1_13[0]";
        public const string temp71 = "topmostSubform[0].Page1[0].f1_51[0]";
        public const string temp72 = "topmostSubform[0].Page1[0].f1_52[0]";
        public const string temp73 = "topmostSubform[0].Page1[0].f1_53[0]";
        public const string temp74 = "topmostSubform[0].Page1[0].f1_54[0]";
        public const string temp75 = "topmostSubform[0].Page1[0].f1_55[0]";
        */

        // Header: Tax year range
        public const string TaxYearStart = "topmostSubform[0].Page1[0].PgHeader[0].f1_1[0]";   // MM/DD
        public const string TaxYearEnd = "topmostSubform[0].Page1[0].PgHeader[0].f1_2[0]";     // MM/DD
        public const string TaxYearSuffix = "topmostSubform[0].Page1[0].PgHeader[0].f1_3[0]";  // YY

        // Line A, B, C: Business Info
        public const string BusinessActivity = "topmostSubform[0].Page1[0].LinesABC[0].f1_7[0]";
        public const string ProductOrService = "topmostSubform[0].Page1[0].LinesABC[0].f1_8[0]";
        public const string BusinessCode = "topmostSubform[0].Page1[0].LinesABC[0].f1_9[0]";

        public const string PartnershipName = "topmostSubform[0].Page1[0].TypeOrPrint[0].f1_4[0]";
        public const string PartnershipAddress = "topmostSubform[0].Page1[0].TypeOrPrint[0].f1_5[0]";
        public const string PartnershipCityStateZip = "";
        public const string PartnershipCountry = "";
        public const string CityZipCountry = "topmostSubform[0].Page1[0].TypeOrPrint[0].f1_6[0]";

        public const string Ein = "topmostSubform[0].Page1[0].f1_10[0]";
        public const string DateBusinessStarted = "topmostSubform[0].Page1[0].f1_11[0]";
        public const string TotalAssets = "topmostSubform[0].Page1[0].f1_12[0]";
    }
}
