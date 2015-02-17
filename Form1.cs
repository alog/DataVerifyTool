﻿using System;

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
using System.Diagnostics;



namespace muweili
{
    public partial class MainForm : Form
    {
        string initMessage= @"Enter server name and SQL server instance name, sample:  
localhost\sqlexpress;  or YourHostName\SQLInstanceName
make sure you current logged on user has privile to access it.
--------------------------------------------------------------";

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
            masDb.Text = "srw\\sqlexpress";
            sasDb.Text = "localhost\\sqlexpress";
            tableName.Text = "operator";
        }

 
        private void connectClick(object sender, EventArgs e)
        {
            if (!checkMasDbString()) {
                
                LogToScreenAndFile("Check your connection again.");
                return;
            }
            mas = new CcureDatabaseServer(masDb.Text);
            String result=mas.OpenConnectionWithErrorReturn(); 
          
            if (result != null)
            {
                LogToScreenAndFile(result);
            }
            if (mas.connectOK)
            { 
                LogToScreenAndFile(mas.getInfo());

            }
             
            sas = new CcureDatabaseServer(sasDb.Text);
            result = sas.OpenConnectionWithErrorReturn();
            if (result != null)
            {
                LogToScreenAndFile(result);
            }
            if (sas.connectOK)
            {
                LogToScreenAndFile(sas.getInfo());
                masSasConfig(mas, sas);
                LogToScreenAndFile(mas.applicationServerName + " Range " + mas.rangeStart.ToString() + " " + mas.rangeEnd.ToString());
                LogToScreenAndFile(sas.applicationServerName + " Range " + sas.rangeStart.ToString() + " " + sas.rangeEnd.ToString());
             }
            if (mas.connectOK && sas.connectOK)
            {
                groupBox2.Visible = true;
            }


        }

        private void getInfoClick(object sender, EventArgs e)
        {  
        }

        private void verifyClick(object sender, EventArgs e)
        {
            tableName.Text = tableName.Text.Trim();
            CcureDatabase masAcvscore = new CcureDatabase(mas, "acvscore");
            CcureDatabase sasAcvscore = new CcureDatabase(sas, "acvscore");
            if (!mas.connectOK)
            {
                return;
            }
            LogToScreenAndFile(string.Format("Pair table comparing : {0}", tableName.Text));
            List<TableRecordIssue> issues = new List<TableRecordIssue>();
            //issues.AddRange(masAcvscore.pairTableCompare(tableName.Text));
            //issues.AddRange(sasAcvscore.pairTableCompare(tableName.Text));

            issues.AddRange(masSasCompare(mas, sas, tableName.Text));
            if (issues.Count != 0)
            {
                foreach (TableRecordIssue sth in issues) { LogToScreenAndFile(sth.toString()); }
            }
            else {
                LogToScreenAndFile(string.Format("Pair table comparing : no issue found."));
            }

        }


        private List<TableRecordIssue> masSasCompare (CcureDatabaseServer mas, CcureDatabaseServer sas, string tbl)
        {   //List<TableRecordIssue>
            //SAS中的记录一定会在MAS中， 而且其objectid应该在其ID值范围或在MASID值范围(global记录),且各列的值都一样
            //MAS中 objectID属于SAS ID值范围，必须出现在SAS中，且各列的值都一样
            //
            List<TableRecordIssue> result = new List<TableRecordIssue>();
            DataTable masDT = mas.getDataTableToCompare(tbl);
            DataColumn chkColumn = new DataColumn("checked",typeof(System.Boolean));
            chkColumn.DefaultValue=false ;
            masDT.Columns.Add(chkColumn );

            DataTable sasDT = sas.getDataTableToCompare(tbl);
            string[] columnNames = masDT.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray();  
            StringBuilder rowCheckResult= new StringBuilder() ;
            result.Add(new TableRecordIssue(null,0,"comparing "+tbl+" fields:"+string.Join(",", columnNames)));
            Stopwatch sw = new Stopwatch();
            sw.Start();
            foreach (DataRow sasRow in sasDT.Rows)
            { 
                int objID=Convert.ToInt32(sasRow["objectid"]);
 
                // 检查是否在MAS或SAS的 ID值范围
                if ((!sas.fitRange(objID)) && (! mas.fitRange(objID)) )
                { 
                    rowCheckResult.Clear();
                    rowCheckResult.Append(string.Format(" NOT in {0} or {1}", mas.getRangeInfo(), sas.getRangeInfo()));
                    result.Add(new TableRecordIssue(sasDT.TableName, objID, rowCheckResult.ToString()));
                }

                //检查SAS表里的记录是否在MAS , 
                DataRow foundRow = masDT.Rows.Find(sasRow[0]);
                if (foundRow == null)
                {
                    rowCheckResult.Clear();
                    rowCheckResult.Append(string.Format(" NOT found in {0} {1}", mas.applicationServerName,masDT.TableName ));
                    result.Add(new TableRecordIssue(sasDT.TableName, objID, rowCheckResult.ToString())); 
                }
                else
                { //因为数据库对ObjectID的主键限制，id肯定是拥有唯一值的，所以记录肯定只有一个， 比较所有列的值
                    rowCheckResult.Clear();
                    foreach (DataColumn col in sasDT.Columns)
                    { 
                        if (!Equals(foundRow[col.ColumnName] , sasRow[col.ColumnName]))
                        {
                            rowCheckResult.Append(string.Format("{0} NOT equal between Sas & MAS. ", col.ColumnName));
                        }
                    }
                    if (rowCheckResult.Length != 0) 
                    {
                        result.Add(new TableRecordIssue(sasDT.TableName, objID, rowCheckResult.ToString()));
                    } 
                    foundRow["checked"]=true; // 标记一下，该记录已经检查过了;
                }
            }
            //检查MAS表里剩余的记录，是否还有属于SAS ID Range里但又不在SAS表里的记录
            DataRow[] rows = masDT.Select(string.Format(" checked=false AND objectid >={0} AND objectid<={1}", sas.rangeStart, sas.rangeEnd));
            foreach (DataRow row in rows)
            {
                rowCheckResult.Clear();
                rowCheckResult.Append(string.Format(" NOT found in {0} {1} ", sas.applicationServerName, sasDT.TableName));
                result.Add(new TableRecordIssue(masDT.TableName, Convert.ToInt32(row["objectid"]), rowCheckResult.ToString()));
            }
            sw.Stop();
            result.Add(new TableRecordIssue(null,0,"Compare finish. time used:" + sw.Elapsed));
            return result;
        }


        private void masSasConfig(CcureDatabaseServer mas, CcureDatabaseServer sas)
        {
             
            DataTable sasDT = sas.getDataTableToCompare("ApplicationServer");
             
            // XXXXX Here might need to double check, XXXXXXXXXXXXXXXXXXX
            // assume the lowest ID it's the MAS.
            DataView dv = sasDT.DefaultView;
            dv.Sort = "objectid asc";
            DataTable sortedDT = dv.ToTable();
            mas.applicationServerName = sortedDT.Rows[0]["name"].ToString();
            mas.applicationServerObjectID = Convert.ToInt32(sortedDT.Rows[0]["objectid"]);
            mas.rangeStart = Convert.ToInt32(sortedDT.Rows[0]["rangeStart"]);
            mas.rangeEnd = Convert.ToInt32(sortedDT.Rows[0]["rangeEnd"]);
            sas.applicationServerName = sortedDT.Rows[1]["name"].ToString();
            sas.applicationServerObjectID = Convert.ToInt32(sortedDT.Rows[1]["objectid"]);
            sas.rangeStart = Convert.ToInt32(sortedDT.Rows[1]["rangeStart"]);
            sas.rangeEnd = Convert.ToInt32(sortedDT.Rows[1]["rangeEnd"]);
              
        }

         

        private Boolean checkMasDbString()
        {
            masDb.Text = masDb.Text.Trim();
            if ( masDb.Text.Length==0 ){
                return false;
            }
            return true;
        } 

        private void closeMe_Click(object sender, EventArgs e)
        {
 
            Application.Exit();
        }
        private void LogToScreenAndFile(string messsage)
        {
            logger.WriteToLog(messsage);
            result.AppendText(System.Environment.NewLine+messsage);
        }
 

        private void ClearResut(object sender, EventArgs e)
        {
            result.Clear();
        }

        private void deleteObject_Click(object sender, EventArgs e)
        {
            objectId.Text = objectId.Text.Trim();
            int objId;
            if (!(int.TryParse(objectId.Text, out objId))) { return; }
             
            objId= int.Parse(objectId.Text);
            string shortTableName = tableName.Text.Trim();


            List<TableRecordIssue> result = new List<TableRecordIssue>();
            if (mas.getCcureVersion() == Ccure9000.Ccure2_3)
            {
                CcureDatabase acvscore = new CcureDatabase(mas, Ccure9000.ACVSCORE);
                List<CcureTable> tables = acvscore.getTableWhoseNameLike(shortTableName);

                foreach (CcureTable table in tables)
                {
                    LogToScreenAndFile("-- "+table.toString());
                    //killObject(table, objId);
                    List<TableForeignKey> fks = table.getFKListDependOnMe();
                    foreach (TableForeignKey fk in fks)
                    {
                        LogToScreenAndFile(fk.toString());
                        //killObject( new CcureTable( table.parentDatabase, fk.tableName ), id);
                    }
                     
                } 
            } 
        }

        private void killObject(CcureTable table, int id)
        {
             
            List<TableForeignKey> fks = table.getFKListDependOnMe();
            if (fks.Count != 0)
            {
                foreach (TableForeignKey fk in fks)
                {
                    LogToScreenAndFile(fk.toString());
                    //killObject( new CcureTable( table.parentDatabase, fk.tableName ), id);
                }
            }

            LogToScreenAndFile(" delete " + table.toString() + " where objectid= " + id);
            
             
        }


     
   
         
    }
}
