using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace muweili
{
    public class TableRecordIssue
    {
        public string tableName { get; set; }
        public int objectId { get; set; }
        public string desc { get; set; } 
         
        public TableRecordIssue(string n, int id, string text)
        {
            tableName = n;
            objectId = id;
            desc = text;
        }
        public string toString()
        {
            if (tableName != null)
            {
                return (String.Format("Table: {0},(objectId){1}, {2}", tableName, objectId.ToString(), desc));
            }
            else
            {
                return (String.Format("{0}", desc));
            }

        }

    }
}
