using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using muweili.Logging;
using muweili.Database;
using System.Data;

namespace muweili.SqlServer
{
    class CcureDatabaseServer
    {

        public Boolean connectOK = false;
        public string connectionString { get; set; }
        public string ccureVersion { get; set; }
        public string applicationServerName { get; set; }    // application server, MAS, SAS, or Standalone
        public int rangeStart { get; set; }    
        public int rangeEnd { get; set; }
        public int applicationServerObjectID { get; set; } 
        private LogWriter myLogger = LogWriter.Instance; 
        private LogWriter logger = LogWriter.Instance; 

        public CcureDatabaseServer(string serverName){
             
            this.connectionString = makeConnectionString(serverName);
            SqlConnection cnn = new SqlConnection(connectionString);
            try {
                cnn.Open();
                connectOK = true;
            }
            catch (Exception ex)
            {
                
                connectOK = false;
                throw ex ;
            }
            finally
            {
                cnn.Close();
            }
            detectCcureVersion(); // detect the ccure version// XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX 这里要加入错误处理
            detectApplicationServer();
            ;
        }

        // this one is to open connection & return error message for troubleshooting.
        public String OpenConnectionWithErrorReturn()
        {  
            SqlConnection cnn = new SqlConnection(connectionString);
             
                cnn.Open();
                connectOK = true;
   
            return null;
        }

        // by given a servername/instaneName, make a valid connection string, use windows auth
        // example: "Server=srw\\sqlexpress; database=acvscore;Trusted_Connection=True;"
        private string makeConnectionString(string serverName)
        {
            return string.Format("server={0};database=acvscore;Trusted_Connection=True;", serverName);
        }

        // this method has to be redisgned,  XXXXX
        public Boolean IsConnected()
        {
            return false;
        }

        public string getDBServerVersion()
        {
            string version;
            // SqlConnection is closed automatically by using block.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var sqlCmd = new SqlCommand("Select @@version", connection))
                {
                    var dr = sqlCmd.ExecuteReader();
                    version = dr.Read() ? dr.GetString(0) : null;
                    dr.Close();

                }
            }
            return version;
        }



        public string getDBServerName()
        {
            string serverName;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var sqlCmd = new SqlCommand("Select @@SERVERNAME", connection))
                {
                    var dr = sqlCmd.ExecuteReader();
                    serverName = dr.Read() ? dr.GetString(0) : null;
                    dr.Close();
                }
            }
            return serverName;
        }

        // check databaseVersion table 
        private void detectCcureVersion()
        { 
            // 0.0.640.0024  is 2.2
            // 2.30.25 is 2.3
            // 2.40.8 is 2.4
             
            string result=null  ;
            CcureDatabase acvscore = new CcureDatabase(this, Ccure9000.ACVSCORE); 
            if (acvscore.hasTable(Ccure9000.ACCESS_DATABASEVERSION))
            {   // it's 2.3
                string sqlText = string.Format("SELECT PlatformObjectVersion  FROM  {0}", Ccure9000.ACCESS_DATABASEVERSION);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var sqlCmd = new SqlCommand(sqlText, connection))
                    {
                        result = (string)((sqlCmd.ExecuteScalar() != null) ? sqlCmd.ExecuteScalar() : null);
                    } 
                }
            }
            else
            {   
                if (hasDb(Ccure9000.SWHSYSTEM))
                {
                    CcureDatabase swhsystem = new CcureDatabase(this, Ccure9000.SWHSYSTEM);
                    string sqlText = string.Format("SELECT PlatformObjectVersion  FROM  {0}", Ccure9000.DBO_DATABASEVERSION);
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (var sqlCmd = new SqlCommand(sqlText, connection))
                        {
                            result = (string)((sqlCmd.ExecuteScalar() != null) ? sqlCmd.ExecuteScalar() : null);
                        }
                    }
                }

            }
  

            if (result.StartsWith(Ccure9000.Ccure2_2VersionSignature))
            {
                ccureVersion = Ccure9000.Ccure_Version_Dict[Ccure9000.Ccure2_2VersionSignature]; 
            }
            if (result.StartsWith(Ccure9000.Ccure2_3VersionSignature))
            {
                ccureVersion = Ccure9000.Ccure_Version_Dict[Ccure9000.Ccure2_3VersionSignature];
            }
            if (result.StartsWith(Ccure9000.Ccure2_4VersionSignature))
            {
                ccureVersion = Ccure9000.Ccure_Version_Dict[Ccure9000.Ccure2_4VersionSignature];
            } 


        }

        private void detectApplicationServer()
        {
            //查询 DBO_APPLICATIONSERVERREGION这个表，获得在此SQL服务器上的Ccure服务器
            // 

            if (ccureVersion == Ccure9000.Ccure2_3 || ccureVersion == Ccure9000.Ccure2_4)
            {
                string sqlText=@"select * from  dbo.ApplicationServer where objectid in (
select appserverid from  dbo.AppServerPartitions where PartitionOwnedID in (
select partitionid  from  dbo.applicationserverregion) )";
                CcureDatabase acvscore=new CcureDatabase(this, Ccure9000.ACVSCORE);

                DataTable dt = acvscore.getDataTableFromQuery(sqlText);
                //this dt should only have one row record , that's the application server in this SQL server
                applicationServerName = dt.Rows[0]["name"].ToString();
                applicationServerObjectID = Convert.ToInt32(dt.Rows[0]["objectid"]);
                rangeStart = Convert.ToInt32(dt.Rows[0]["rangeStart"]);
                rangeEnd = Convert.ToInt32(dt.Rows[0]["rangeEnd"]);
            } 
        }

        public bool hasDb(string dbName)
        {
            bool result = false;
            string sqlCreateDBQuery = string.Format("SELECT database_id FROM sys.databases WHERE Name='{0}'", dbName);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var sqlCmd = new SqlCommand(sqlCreateDBQuery, connection))
                {
                    int databaseID = (int)((sqlCmd.ExecuteScalar()!=null)?sqlCmd.ExecuteScalar():0);
                    result = (databaseID > 0);
                }
            }
            return result;
        }

        public string[] getTableAndColumn(string shortTableName)
        {
            string[] array = null;
            string k = shortTableName + this.ccureVersion;
            if (Ccure9000.TABLE_COLUMN_TO_COMPARE.ContainsKey(k))
            {
                array = Ccure9000.TABLE_COLUMN_TO_COMPARE[k];
            }
            else
            {
                // 对于一些表如果没有预先定义的列，就只比较id即可
                // {"dbo.partition","objectid", "name"}
                if (ccureVersion == Ccure9000.Ccure2_3 || ccureVersion == Ccure9000.Ccure2_4)
                {
                    CcureDatabase acvscore = new CcureDatabase(this, Ccure9000.ACVSCORE);
                    List<string> tables = acvscore.getTableNameListWhoseNameLike(shortTableName);
                    if (tables.Count >= 1)
                    {
                        array = new string[] { tables[0], Ccure9000.objectID };
                        //获得第一个表即可，排序上 access.operator 先于 dbo.operator
                    }
                }
            } 
            return (array!=null?array:null);
        }

        public DataTable getDataTableToCompare(  string shortTableName)
        {
            DataTable dt = new DataTable();
            string[] array = getTableAndColumn(shortTableName);
            if (array == null) { return dt; }   //没有找到同名的表， 返回空值； 
            string table = array[0];
            int n=array.Count()-1 ;
            string[] columnArray = new string[n] ;
            Array.Copy(array, 1, columnArray, 0, n );
            string fiedsForQuery = string.Join(",", columnArray );
            string sqlText = string.Format(@"select {0} from {1} order by {2} asc", fiedsForQuery, table, columnArray[0]);
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand sqlCmd = new SqlCommand(sqlText, connection);
                SqlDataReader rdr = sqlCmd.ExecuteReader();
                dt.Load(rdr);
            }
            dt.TableName = table;
            dt.PrimaryKey = new DataColumn[] { dt.Columns[columnArray[0]] };
            return dt;
        }

        public Boolean fitRange(int id)
        {
            return ((id >= this.rangeStart) && (id <= rangeEnd));
        }
        public string getRangeInfo()
        {
            return string.Format("{0} objectID range:{1} - {2}", this.applicationServerName, this.rangeStart, this.rangeEnd);
        }

        public string getInfo()
        {
            StringBuilder result=new StringBuilder();
            result.Append(this.getDBServerName() + (this.connectOK ? " is connected." : " not connected."));
            result.Append(System.Environment.NewLine + "\t"+this.connectionString);
            result.Append(System.Environment.NewLine + "\t" + this.getDBServerVersion());
            result.Append(System.Environment.NewLine + "\tCcure 9000 version: " + this.ccureVersion);
            return result.ToString();
        }

        public List<TableRecordIssue> objectLinked(string shortTableName, int objectId)
        {   
            // here, the t
            List<TableRecordIssue> result = new List<TableRecordIssue>();
            if (ccureVersion == Ccure9000.Ccure2_3VersionSignature || ccureVersion == Ccure9000.Ccure2_4VersionSignature)
            {
                CcureDatabase acvscore = new CcureDatabase(this, Ccure9000.ACVSCORE);
                List<CcureTable> tables = acvscore.getTableWhoseNameLike(shortTableName); 
            }
             
            return result;
        }

        


    }
}
