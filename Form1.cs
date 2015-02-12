using System;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic; // For generic collections like List.
using System.Data.SqlClient;      // For the database connections and objects.
using muweili.Logging;
using muweili.SqlServer;
using muweili.Database;
using muweili;



namespace muweili
{
    public partial class MainForm : Form
    {
        string initMessage= @"connection string, sample:  
For SQL 2012,   Server=localhost\sqlexpress; Trusted_Connection=True; (using Windows Authentication)
For SQL 2008,   Server=myServerName\myInstanceName; User Id=myUsername;Password=myPassword; 
                Server=serverName; User Id=sa;Password=xxxxx;    (using SQL authentication)
--------------------------------------------------------------";
        SqlConnection  masCnn=null,sasCnn=null;
        CcureDatabaseServer mas=null, sas=null;
        SqlCommand myCommand = new SqlCommand();
        SqlCommand masCmd=new SqlCommand(), sasCmd=new SqlCommand(); 
        LogWriter logger = LogWriter.Instance;
        

        public MainForm()
        {
            InitializeComponent();
            logger.ClearExistLogFile();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            result.Text = initMessage;
            masDb.Text = "Server=localhost\\sqlexpress; database=acvscore;Trusted_Connection=True;";
            sasDb.Text = "Server=localhost\\sqlexpress; database=sacvscore;Trusted_Connection=True;";
            tableName.Text = "operator";
        }

 
        private void connectClick(object sender, EventArgs e)
        {
            if (!checkMasDbString()) {
                
                LogToScreenAndFile("Check your connection again.");
                return;
            }
            LogToScreenAndFile("Connecting MAS...  " + masDb.Text);

            mas = new CcureDatabaseServer(masDb.Text);
            String result=mas.OpenConnectionWithErrorReturn(); 
          
            if (result != null)
            {
                LogToScreenAndFile(result);
            }
            if (mas.connectOK)
            {
                LogToScreenAndFile(mas.getDBServerName() + " connect OK!");
                LogToScreenAndFile(mas.getDBServerVersion());

            }

            LogToScreenAndFile("Connecting SAS...  " + sasDb.Text);
            sas = new CcureDatabaseServer(sasDb.Text);
            result = sas.OpenConnectionWithErrorReturn();
            if (result != null)
            {
                LogToScreenAndFile(result);
            }
            if (sas.connectOK)
            {
                LogToScreenAndFile(sas.getDBServerName() + " connect OK!");
             }
        }
        private void getInfoClick(object sender, EventArgs e)
        {
            if (masCnn == null || sasCnn==null)
            {
                return;
            }
            string queryString = " select objectid, name,rangestart, rangeend from  dbo.applicationserver;";
            masCmd.CommandText = queryString;
            
            LogToScreenAndFile( "MAS, executing..." + queryString);
            SqlDataReader sqlReader = null;
            try
            {

                sqlReader = masCmd.ExecuteReader();
                while (sqlReader.Read())
                {
                    int objectid = sqlReader.GetInt32(0);
                    string name = sqlReader.GetString(1);
                    int rangeStart = sqlReader.GetInt32(2);
                    int rangeEnd = sqlReader.GetInt32(3);
                    LogToScreenAndFile( "ObjectId:" + objectid.ToString() + " Name:" + name
                        + " range:" + rangeStart + " - " + rangeEnd);

                }
            }
            catch (Exception ex)
            {
                LogToScreenAndFile( ex.Message);
            }
            finally
            {
                sqlReader.Close();
            }
            queryString = "  select objectid, name,rangestart, rangeend from  dbo.applicationserver;";
            sasCmd.CommandText = queryString;
            LogToScreenAndFile( "SAS, executing..." + queryString);
            try
            {

                sqlReader = sasCmd.ExecuteReader();
                while (sqlReader.Read())
                {
                    int objectid = sqlReader.GetInt32(0);
                    string name = sqlReader.GetString(1);
                    int rangeStart = sqlReader.GetInt32(2);
                    int rangeEnd = sqlReader.GetInt32(3);
                    LogToScreenAndFile( "ObjectId:" + objectid.ToString() + " Name:" + name
                        + " range:" + rangeStart + " - " + rangeEnd);

                }
            }
            catch (Exception ex)
            {
                LogToScreenAndFile(ex.Message);
            }
            finally
            {
                sqlReader.Close();
            }
        }

        private void verifyClick(object sender, EventArgs e)
        {
            tableName.Text = tableName.Text.Trim();
            CcureDatabase masAcvscore = new CcureDatabase(mas, "acvscore");
            CcureDatabase sasAcvscore = new CcureDatabase(sas, "sacvscore");
            if (!mas.connectOK)
            {
                return;
            }
            LogToScreenAndFile(string.Format("something like {0} in mas :", tableName.Text));
            List<TableRecordIssue> issues = masAcvscore.pairTableCompare(tableName.Text);
            foreach (TableRecordIssue sth in issues)
            {
                    LogToScreenAndFile(sth.toString());
                    
            }
            LogToScreenAndFile("--------------");
            LogToScreenAndFile("--------mas Ver:" + mas.getCcureVersion());
            LogToScreenAndFile("--------sas Ver:" + sas.getCcureVersion());
            masSasCompare(mas, sas, tableName.Text);

            
        }


        private void  masSasCompare (CcureDatabaseServer mas, CcureDatabaseServer sas, string tbl)
        {   //List<TableRecordIssue>
            List<TableRecordIssue> result = new List<TableRecordIssue>();
            string k=tbl + mas.getCcureVersion();
            if (!Ccure9000.TABLE_KEY.ContainsKey(k))
            {
                return;
            }
            LogToScreenAndFile(" YES, we have key item for " + tbl);
            string v = string.Join(",", Ccure9000.TABLE_KEY[k].ToArray());
            LogToScreenAndFile(v);
            string[] array = Ccure9000.TABLE_KEY[k];
            
            string table = array[0];
            LogToScreenAndFile("table name : " + tbl);

            int n=array.Count()-1 ;
            string[] columnArray = new string[n] ;
            string[] sasResult = new string[n];
            Array.Copy(array, 1, columnArray, 0, n );
            string fiedsForQuery = string.Join(",", columnArray );
            LogToScreenAndFile(fiedsForQuery+"----------------");

            string sqlText = string.Format(@"select {0} from {1} order by {2}", fiedsForQuery, table, columnArray[0]);
            LogToScreenAndFile(sqlText + "--------");

            using (SqlConnection connection = new SqlConnection(sas.connectionString))
            {
                connection.Open();
                SqlCommand sqlCmd = new SqlCommand(sqlText, connection);
                using (SqlDataReader rdr = sqlCmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                       // conver all table column into string value;
                       //   
                        for (int i = 0; i < n; i++)
                        {   
                             
                            sasResult[i] = rdr.IsDBNull(i) ? "Null" : rdr[i].ToString();
                        }

                        using (SqlConnection masConnection = new SqlConnection(mas.connectionString))
                        { 
                            connection.Open();
                            string sqlText = string.Format(@"select {0} from {1} where {2}=={3} ", 
                                fiedsForQuery, table, columnArray[0], sasResult[0]);
                            SqlCommand masSqlCmd = new SqlCommand(sqlText, connection);
                            using (SqlDataReader masRdr = masSqlCmd.ExecuteReader())
                            XXXXXXXXXXXXXXXXX
                        
                        
                        
                        }





                        LogToScreenAndFile(string.Join(",", sasResult ));
                    }
                }

            }




            // return result;

        }
         
        private Boolean checkMasDbString()
        {
            masDb.Text = masDb.Text.Trim();
            if ( masDb.Text.Length==0 ){
                return false;
            }
            return true;
        }

        private void runSQLQuery_Click(object sender, EventArgs e)
        {
        }
         

        private void closeMe_Click(object sender, EventArgs e)
        {
            if (masCnn != null)
            {
                masCnn.Close();
            }
            Application.Exit();
        }
        private void LogToScreenAndFile(string messsage)
        {
            logger.WriteToLog(messsage);
            result.AppendText(System.Environment.NewLine+messsage);
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ClearResut(object sender, EventArgs e)
        {
            result.Clear();
        }
         
    }
}
