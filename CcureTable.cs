using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using muweili.Database;
using System.Data.SqlClient;

namespace muweili
{
    class CcureTable
    {   
        //// table name format  dbname.schema.tablename , sample: acvscore.access.operator
        public string name;   
        public List<TableForeignKey> foreignKeylist;  // foreign key list for this table
        public CcureDatabase parentDatabase;
        public CcureTable(CcureDatabase db,string tableName)
        {
            parentDatabase = db;
            name = tableName.ToLower();
            foreignKeylist = getFKListIDependOn();
        }


        public List<TableForeignKey> getFKListIDependOn( )
        {
            List<TableForeignKey> fKlist = new List<TableForeignKey>();
            foreach (TableForeignKey fk in parentDatabase.foreignKeylist)
            {
                if (this.name == fk.tableName.ToLower())
                {
                    fKlist.Add(fk);
                }
            }
            return fKlist;
        }

        public List<TableForeignKey> getFKListDependOnMe()
        {
            List<TableForeignKey> fKlist = new List<TableForeignKey>();
            foreach (TableForeignKey fk in parentDatabase.foreignKeylist)
            {
                if (this.name == fk.referenceTableName.ToLower())
                {
                    fKlist.Add(fk);
                }
            }
            return fKlist;
        }


        public List<CcureTable> tableDependOnMe()
        {
            List<CcureTable> result = new List<CcureTable>();
            foreach (TableForeignKey fk in this.getFKListDependOnMe())
            {
                result.Add(new CcureTable(this.parentDatabase, fk.tableName));
            } 
            return result;
        }

        public List<TableRecordIssue> checkFK()
        {   
            // this one, might not needed, since foreign key integrity is forced applied by database.

            List < TableRecordIssue > tableRecordIssueList= new List<TableRecordIssue>();
            return tableRecordIssueList;
        }

        public List<int> getObjectId()
        {
            List<int> result = new List<int>();
            string sqlText = string.Format(@"select objectID from {0} order by objectId",this.name);

            using (SqlConnection connection = new SqlConnection(parentDatabase.parentSqlServer.connectionString))
            {
                connection.Open();

                SqlCommand sqlCmd = new SqlCommand(sqlText, connection);
                using (SqlDataReader rdr = sqlCmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        result.Add(rdr.GetInt32(0)); //The 0 stands for "the 0'th column", so the first column of the result.
                    }
                }
            }
            return result;
        }

        public string toString()
        {
            return string.Format("{0}", this.name);
        }

    }
}
