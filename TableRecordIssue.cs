using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace muweili
{
    public class TableRecordIssue
    {
        private string tableName;
        private int objectId;
        private string issue;


        public TableRecordIssue(string n, int id, string text)
        {
            tableName = n;
            objectId = id;
            issue = text;
        }
        public string toString()
        {
            return(String.Format("Issue found: {0}, objectId={1}, {2}", tableName, objectId.ToString(),issue));
        }

    }
}
