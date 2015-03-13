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
using System.Diagnostics;
using System.Security.Principal;



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
            masDb.Text = "srw\\sqlexpress";  //
            sasDb.Text = "localhost\\sqlexpress";
            tableName.Text = "operator";
        } 
        private void connectClick(object sender, EventArgs e)
        {
            IntPtr accountToken = WindowsIdentity.GetCurrent().Token;
            LogToScreenAndFile(" Current credential : " + WindowsIdentity.GetCurrent().Name);
            mas = null;
            sas = null;  //  initialize the mas or sas string;
            MASConnect.Enabled = false;
            SASConnect.Enabled = false;
            groupBox2.Visible = false;
            
            if (masDb.Text.Trim().Length == 0)
            { 
                LogToScreenAndFile("Please enter the SQL server and instance name.");
                return;
            }
            try
            {
                mas = new CcureDatabaseServer(masDb.Text);
            }
            catch (Exception ex) {
                LogToScreenAndFile("Error ,Check connection again : " + masDb.Text);
                LogToScreenAndFile(ex.Message);
                return;
            }
            groupBox2.Visible = true;
             
            MASConnect.Enabled  = true; // enable the check box "MASConnect"
            MASConnect.Checked = true;
            LogToScreenAndFile(mas.getInfo());
            LogToScreenAndFile(mas.getRangeInfo());

            if (sasDb.Text.Trim().Length != 0)
            {
                try
                {
                    sas = new CcureDatabaseServer(sasDb.Text);
                    SASConnect.Enabled = true;
                    SASConnect.Checked = true;
                    LogToScreenAndFile(sas.getInfo());
                    
                }
                catch (Exception ex)
                {
                    LogToScreenAndFile("Error ,Check connection again : " + sasDb.Text);
                    LogToScreenAndFile(ex.Message);
                    SASConnect.Enabled = false;
                    //checkBox1.Checked = true;
                 }
            }
            
            if (  sas != null)
            {
                masSasConfig(mas, sas);
                LogToScreenAndFile(mas.getRangeInfo());
                LogToScreenAndFile(sas.getRangeInfo());
                //LogToScreenAndFile(" sas not null");
                 
            }
             
        }
         

        private void verifyClick(object sender, EventArgs e)
        {
            tableName.Text = tableName.Text.Trim();
            if (tableName.Text.Length == 0  )
            {
                LogToScreenAndFile(" No SAS available or enter your table name.");
                return;  // 表的名字是空，或者没有SAS， 直接返回
            }
            CcureDatabase masAcvscore = new CcureDatabase(mas, Ccure9000.ACVSCORE );
            if (mas.getTableAndColumn(tableName.Text) == null)
            {
                LogToScreenAndFile(" No such table : " + tableName.Text);
                return;  //获取需要比较检查的表名和表的列， 没有返回则直接退出
            }
            if (sas == null)
            {
                //如果只有MAS 或独立服务器，则只显示该服务器
                DataTable dt = mas.getDataTableToCompare(tableName.Text); 
                DualTableForm df = new DualTableForm(dt,null);
                df.Show();
            }
            else
            {   
                //如果MAS和SAS都有
                CcureDatabase sasAcvscore = new CcureDatabase(sas, Ccure9000.ACVSCORE); 
                LogToScreenAndFile(string.Format("Pair table comparing : {0}", tableName.Text));
                LogToScreenAndFile(string.Join(".", mas.getTableAndColumn(tableName.Text))); 
                DataTable dt = mas.getDataTableToCompare(tableName.Text);
                DataTable sasDt = sas.getDataTableToCompare(tableName.Text);
                List<TableRecordIssue> issues = new List<TableRecordIssue>();
                issues.AddRange(masSasCompare(mas, sas, tableName.Text));
                if (issues.Count != 0)
                {
                    foreach (TableRecordIssue sth in issues) { LogToScreenAndFile(sth.toString()); }
                }
                else
                {
                    LogToScreenAndFile(string.Format("Pair table comparing  : no issue found."));
                }

                DualTableForm df = new DualTableForm(dt,sasDt);
                df.Show();
            
            }


            

         }


        private List<TableRecordIssue> masSasCompare (CcureDatabaseServer mas, CcureDatabaseServer sas, string tbl)
        {   //List<TableRecordIssue>
            //SAS中的记录一定会在MAS中， 而且其objectid应该在其ID值范围或在MASID值范围(global记录),且各列的值都一样
            //MAS中 objectID属于SAS ID值范围，必须出现在SAS中，且各列的值都一样
            //
            List<TableRecordIssue> result = new List<TableRecordIssue>();
            DataTable masDT = mas.getDataTableToCompare(tbl);

            string[] columnNames = masDT.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray(); 
            result.Add(new TableRecordIssue(null, 0, "comparing " + tbl + " fields:" + string.Join(",", columnNames)));

            DataColumn chkColumn = new DataColumn("checked",typeof(System.Boolean));
            chkColumn.DefaultValue=false ;
            masDT.Columns.Add(chkColumn );

            DataTable sasDT = sas.getDataTableToCompare(tbl);
             
            StringBuilder rowCheckResult= new StringBuilder() ;
            
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
            // XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
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
            if (mas.ccureVersion == Ccure9000.Ccure2_3VersionSignature)
            {
                CcureDatabase acvscore = new CcureDatabase(mas, Ccure9000.ACVSCORE);
                List<CcureTable> tables = acvscore.getTableWhoseNameLike(shortTableName);

                foreach (CcureTable table in tables)
                {
                    List<TableForeignKey> fks = table.getFKListDependOnMe();
                    foreach (TableForeignKey fk in fks)
                    { LogToScreenAndFile(fk.toString()); 
                    }
                    LogToScreenAndFile("----------");
                    //LogToScreenAndFile("--" + table.getFKIDependon(new CcureTable(table.parentDatabase, "acvscore.access.partition")).toString());
                }

                LogToScreenAndFile("--");
                LogToScreenAndFile("--");
                string condition = "WHERE objectid=" + objId;
                 
                foreach (CcureTable table in tables)
                {   
                     killObject(   table, condition );
 
                } 
            } 
        }

        private void killObject(CcureTable table, String  queryCondition)
        {   
            /* queryCondition example :  "WHERE objectid=1234"
            // fks 列出所有对当前表有外键依赖的表， 其格式如下
            //acvscore.Access.Personnel PartitionID --> acvscore.Access.Partition ObjectID foreign key: FK_Personnel_PartitionID
            // 这个例子中，personnel表，列PartitionID是外键 指向 Partition表的ObjectID这个列
            */

            List<TableForeignKey> fks = table.getFKListDependOnMe();
            foreach (TableForeignKey fk in fks)
            {
                
                if (string.Equals(fk.tableName, fk.referenceTableName))
                {   //如果这个外键 是一个表某列外键指向本表， 例如Area这个表
                    //则要特别声明如此处理
                    CcureTable currentTbl = new CcureTable(table.parentDatabase, fk.tableName);
                    TableForeignKey currentFK=currentTbl.getFKIDependon(table);

                    string sqlText = string.Format(
                    "DELETE FROM {0} WHERE {1} IN (SELECT {2} FROM {3} {4});",
                                 currentFK.tableName, currentFK.columnName, 
                                 currentFK.referenceColName, table.toString(), queryCondition);
                    LogToScreenAndFile(sqlText);
                }
                else
                {
                    //如果当前表有被其他表外键引用，那么对于每一个引用的表，做迭代，删除其记录 
                    //例如对上面那个例子，要删除partition记录，就必须先删除personnel记录，
                    //生成的命令就是：
                    // DELETE FROM personnel WHERE PartitionID in （SELECT ObjectID FROM partition 
                    //        WHERE  objectid=1234);
                    String condition = string.Format("WHERE {0} in (SELECT {1} from {2} {3})",
                        fk.columnName, fk.referenceColName, fk.referenceTableName, queryCondition);
                    killObject(new CcureTable(table.parentDatabase, fk.tableName), condition);
                }
                
            }

            // 如果当前表没有被其他表外键引用，那么就直接可以删除记录
            // 生成的命令就是 :  DELETE FROM table WHERE objectid=1234
            LogToScreenAndFile("DELETE FROM " + table.toString() + " " + queryCondition + ";");
             
        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxReaction();
        }
        private void checkBoxReaction()
        {
            if (!MASConnect.Checked && !SASConnect.Checked)
            {
                btnVerify.Enabled = false;
                deleteObject.Enabled = false;
            }
            else
            {
                btnVerify.Enabled = true;
                deleteObject.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // to create 100 istar doors
            tableName.Text = tableName.Text.Trim();
             
            if (mas == null || tableName.Text.Length == 0 ) 
            { return; }
            if (mas.ccureVersion == Ccure9000.Ccure2_3)
            {
                CcureDatabase acvscore = new CcureDatabase(mas, Ccure9000.ACVSCORE);
                List<string> result=acvscore.getTableNameListWhoseNameContains(tableName.Text );
                if (result.Count > 0)
                {
                    LogToScreenAndFile("The talbe whose name contains " + tableName.Text);
                    foreach (string s in result) { LogToScreenAndFile(s); }
                }
                else
                { LogToScreenAndFile( "No tale name like "+tableName.Text ); }
         
            }

            
         
             

        }
   
    }
}
