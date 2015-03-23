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
        static public string DBO_APPLICATIONSERVERREGION = "acvscore.dbo.applicationserverregion";
            // table acvscore.dbo.applicationserverregion to identify the ccure server name

        
        static public string Ccure2_2VersionSignature = "0.0.640";
        static public string Ccure2_3VersionSignature = "2.30";
        static public string Ccure2_4VersionSignature = "2.40";// 2.40.8 is 2.4
        
        static public string Ccure2_2 = "2.20";
        static public string Ccure2_3 = "2.30";
        static public string Ccure2_4 = "2.40";

        static public string objectID = "objectid";
        

  
        static public Dictionary<string, string> Ccure_Version_Dict = new Dictionary<string, string>()
        {
            {Ccure2_2VersionSignature,Ccure2_2},
            {Ccure2_3VersionSignature,Ccure2_3},
            {Ccure2_4VersionSignature,Ccure2_4}
        };

 

        static public Dictionary<string, string[]> TABLE_COLUMN_TO_COMPARE = new Dictionary<string, string[]>()
        {   
            // table name use full name , like acvscore.Access.Personnel
            //这个字典用于检索对应的表，应该检查哪些我们感兴趣的列，
            {"operator"+Ccure2_3VersionSignature, new string[] {"dbo.operator",objectID, "name", "PartitionID","WindowsPrincipal","Description" } },
            {"personnel"+Ccure2_3VersionSignature, new string[] {"Access.Personnel",objectID, "name", "PartitionID" } },
            {"partition"+Ccure2_3VersionSignature, new string[] {"dbo.partition",objectID, "name"}},
            {"credential"+Ccure2_3VersionSignature, new string[] {"access.credential",objectID, "name","PartitionID","cardnumber","personnelid","chuid"}},
            {"ApplicationServer"+Ccure2_3VersionSignature, new string[] {"dbo.ApplicationServer",objectID, "name","RangeStart","RangeEnd"}},

            {"operator"+Ccure2_4VersionSignature, new string[] {"dbo.operator",objectID, "name", "PartitionID","WindowsPrincipal","Description" } },
            {"personnel"+Ccure2_4VersionSignature, new string[] {"Access.Personnel",objectID, "name", "PartitionID" } },
            {"partition"+Ccure2_4VersionSignature, new string[] {"dbo.partition",objectID, "name"}},
            {"credential"+Ccure2_4VersionSignature, new string[] {"access.credential",objectID, "name","PartitionID","cardnumber","personnelid","chuid"}},
            {"ApplicationServer"+Ccure2_4VersionSignature, new string[] {"dbo.ApplicationServer",objectID, "name","RangeStart","RangeEnd"}} 


         };

     

    }
}
