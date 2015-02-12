using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace muweili
{
    class Ccure9000
    {
        static public string ACVSCORE = "acvscore";
        static public string SWHSYSTEM = "swhsystem";
        static public string ACCESS_DATABASEVERSION = "acvscore.access.databaseversion";
        static public string DBO_DATABASEVERSION = "swhsystem.dbo.databaseversion";
        static public string DBO_APPLICATIONSERVER = "acvscore.dbo.ApplicationServer";
        static public string Ccure2_3 = "2.30";
        static public string Ccure2_2 = "0.0.640";

 

        static public Dictionary<string, string[]> TABLE_KEY = new Dictionary<string, string[]>()
        {   
            // table name use full name , like acvscore.Access.Personnel
            {"operator"+Ccure2_3, new string[] {"dbo.operator","objectid", "name", "PartitionID","WindowsPrincipal","Description" } },
            {"personnel"+Ccure2_3, new string[] {"Access.Personnel","objectid", "name", "PartitionID" } }
        };

     

    }
}
