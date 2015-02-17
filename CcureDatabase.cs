using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using muweili.SqlServer;
using muweili;
using System.Data.SqlClient;

namespace muweili.Database
{
    class CcureDatabase
    {
        public CcureDatabaseServer parentSqlServer;
        private string name;
        public List<TableForeignKey> foreignKeylist ; 
        public CcureDatabase(CcureDatabaseServer sqlServer, string name) 
        {
            parentSqlServer = sqlServer;
            this.name = name;
            foreignKeylist = generateFKList();
        }
         

        // return a list which contain all base teble type table from current database
        // table name format  dbname.schema.tablename , sample: acvscore.access.operator
        public List<string> getAllTableNameList()
        {
            StringBuilder result = new StringBuilder();
            List<string> allTableNames = new List<string>();  

            string sqlGetAlltables=@"select t.TABLE_CATALOG+'.'+t.TABLE_SCHEMA+'.'+t.TABLE_NAME as TABLE_NAME 
from information_schema.tables t  where t.TABLE_TYPE like 'Base Table'
order by t.TABLE_CATALOG,t.TABLE_SCHEMA, t.TABLE_NAME " ;
               
            using (SqlConnection connection = new SqlConnection(parentSqlServer.connectionString))
            {
                connection.Open();
                String sqlUseDb= string.Format("use {0};", name);
                SqlCommand sqlCmd = new SqlCommand(sqlUseDb, connection);
                sqlCmd.ExecuteNonQuery();

                sqlCmd.CommandText = sqlGetAlltables;
                using (SqlDataReader rdrr = sqlCmd.ExecuteReader())
                {
                    while (rdrr.Read())
                    {
                        allTableNames.Add(rdrr.GetString(0).ToLower()); //The 0 stands for "the 0'th column", so the first column of the result.
                    }
                }
                
            }
            return allTableNames;
        }

        public List<CcureTable> getAllTable()
        {
            List<CcureTable> result = new List<CcureTable>();
            List<string> allTable = getAllTableNameList();
            foreach(string tbl in allTable)
            {
                result.Add(new CcureTable(this,tbl));
            }
            return result;
        }


        // return a string with all table name, seperatre by ","  ;
        public string getAllTableNameString()
        {
            return string.Join(",", getAllTableNameList().ToArray());
        }

        public Boolean hasTable(string tblName)
        {
            return (getAllTableNameString().Contains(tblName));
        }

        // return a name list<string> 
        public List<string> getTableNameListWhoseNameLike(string tbl)
        {
            List<string> allTable = getAllTableNameList();
            List<string> result=new List<string>();
            foreach ( string tmp in allTable)
            {   //   
                //   compare the last part......db.schema.name.....
                string s = (tbl.StartsWith(".") ? tbl.ToLower() : "." + tbl.ToLower());
                if (tmp.ToLower().EndsWith(s))
                {
                    result.Add(tmp.ToLower());
                }
            }
            return result;
        }

        // return a name list<CcureTable> 
        public List<CcureTable> getTableWhoseNameLike(string tbl)
        {
            List<string> allTable = getAllTableNameList();
            List<CcureTable> result = new List<CcureTable>();
            string s = (tbl.Contains(".") ? tbl.ToLower() : "." + tbl.ToLower());
            foreach (string tmp in allTable)
            {
                if (tmp.ToLower().EndsWith(s))
                {
                    result.Add(new CcureTable(this, tmp.ToLower()));
                }
            }
            return result;
        }


        // find out all forgei key relationship for all base type table in current database
        // generate the TableForeignKey object list
        //  
        public List<TableForeignKey> generateFKList()
        {
            List<TableForeignKey> fKlist = new List<TableForeignKey>();
            string sqlGetAllFK = @"SELECT 
db_name()+'.'+OBJECT_SCHEMA_NAME(f.parent_object_id)+'.'+OBJECT_NAME(f.parent_object_id) AS TableName,
COL_NAME(fc.parent_object_id, fc.parent_column_id) AS ColumnName,
db_name()+'.'+OBJECT_SCHEMA_NAME(f.referenced_object_id)+'.'+OBJECT_NAME (f.referenced_object_id) AS ReferencedTableName,
COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS ReferencedColName,
f.name AS ForeignKey
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc
ON f.OBJECT_ID = fc.constraint_object_id
order by TableName";

            using (SqlConnection connection = new SqlConnection(parentSqlServer.connectionString))
            {
                connection.Open();
                useCurrentDB(connection);
                SqlCommand sqlCmd = new SqlCommand(sqlGetAllFK, connection);
                using (SqlDataReader rdrr = sqlCmd.ExecuteReader())
                {
                    while (rdrr.Read())
                    {
                        fKlist.Add(new TableForeignKey(rdrr.GetString(0), rdrr.GetString(1),
                            rdrr.GetString(2),rdrr.GetString(3),rdrr.GetString(4))); 
                    }
                }

            }
            return fKlist;

        }

        //before running query, point to use the current DB, use current connection, don't close it.
        private void useCurrentDB(SqlConnection connection)
        {
            String sqlUseDb = string.Format("use {0};", this.name);
            SqlCommand sqlCmd = new SqlCommand(sqlUseDb, connection);
            sqlCmd.ExecuteNonQuery();
        }

        // giving a name like "operator", for ccure 2.3 compare dbo.operator & access.operator\
        // for 2.2 or lower, will be swhsystem.dbo.operator  and acvscore.dbo.operator
        public List<TableRecordIssue> pairTableCompare(string tableName)
        {
            List<TableRecordIssue> result = new List<TableRecordIssue>();
            List<CcureTable> tables = getTableWhoseNameLike(tableName);
            string srv = this.parentSqlServer.applicationServerName;
            if (tables.Count == 2)
            {
                List<int> list0 = tables[0].getObjectId();
                List<int> list1 = tables[1].getObjectId();
                
                foreach (int i in list0)
                {
                    if (!list1.Contains(i))
                    {
                        result.Add(new TableRecordIssue(srv + " " + tables[0].name, i, "Not found in " + tables[1].name));
                    }
                }
                foreach (int i in list1)
                {
                    if (!list0.Contains(i))
                    {
                        result.Add(new TableRecordIssue(srv + " " + tables[1].name, i, "Not found in " + tables[0].name));
                    }
                }

            }
            else
            {
                result.Add( new TableRecordIssue(null,0,string.Format("{0}, no pair talbe for {1}",srv, tableName)));
            }

            return result;
        }



    }
}
