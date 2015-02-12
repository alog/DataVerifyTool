using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using muweili.Logging;
using muweili.Database;

namespace muweili.SqlServer
{
    class CcureDatabaseServer
    {

        public Boolean connectOK = false;
        public string connectionString { get; set; }
        public string ccureVersion { get; set; }
        public string applicationServerName { get; set; }
        public int rangeStart { get; set; }
        public int rangeEnd { get; set; }
        public int applicationServerObjectID { get; set; } 
        private LogWriter myLogger = LogWriter.Instance; 
        private LogWriter logger = LogWriter.Instance; 

        public CcureDatabaseServer(String cnnString){
             connectionString = cnnString;
        }

        // this one is to open connection & return error message for troubleshooting.
        public String OpenConnectionWithErrorReturn()
        {
            SqlConnection cnn = new SqlConnection(connectionString);
            try
            {
                cnn.Open();
                connectOK = true;
                if (getDBServerVersion() == Ccure9000.Ccure2_3)
                {

                }


            }
            catch (Exception ex)
            {
                //LogToScreenAndFile(ex.Message);
                connectOK = false;
                return ex.Message;
            }
            finally
            {
                cnn.Close();
            }
            return null;
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
        public string getCcureVersion()
        { 
            // 0.0.640.0024  is 2.2
            // 2.30.25 is 2.3
             
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
            if (result==null )
            {
                return null;
            }


            if (result.StartsWith(Ccure9000.Ccure2_3))
            {
                return Ccure9000.Ccure2_3;
            }
            if (result.StartsWith(Ccure9000.Ccure2_2))
            {
                return Ccure9000.Ccure2_2;
            }

            return null;


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

    }
}
