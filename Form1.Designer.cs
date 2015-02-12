namespace muweili
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.connect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.verify = new System.Windows.Forms.Button();
            this.result = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.masDb = new System.Windows.Forms.TextBox();
            this.closeMe = new System.Windows.Forms.Button();
            this.cmdString = new System.Windows.Forms.TextBox();
            this.runSQLQuery = new System.Windows.Forms.Button();
            this.sasDb = new System.Windows.Forms.TextBox();
            this.tableName = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.clearScreen = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // connect
            // 
            this.connect.Location = new System.Drawing.Point(25, 22);
            this.connect.Name = "connect";
            this.connect.Size = new System.Drawing.Size(75, 23);
            this.connect.TabIndex = 2;
            this.connect.Text = "Connect";
            this.connect.UseVisualStyleBackColor = true;
            this.connect.Click += new System.EventHandler(this.connectClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(117, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "MAS";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // verify
            // 
            this.verify.Location = new System.Drawing.Point(153, 88);
            this.verify.Name = "verify";
            this.verify.Size = new System.Drawing.Size(111, 23);
            this.verify.TabIndex = 5;
            this.verify.Text = "Verify";
            this.verify.UseVisualStyleBackColor = true;
            this.verify.Click += new System.EventHandler(this.verifyClick);
            // 
            // result
            // 
            this.result.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.result.Location = new System.Drawing.Point(25, 166);
            this.result.Multiline = true;
            this.result.Name = "result";
            this.result.ReadOnly = true;
            this.result.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.result.Size = new System.Drawing.Size(948, 335);
            this.result.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(119, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "SAS";
            this.label2.Click += new System.EventHandler(this.ClearResut);
            // 
            // masDb
            // 
            this.masDb.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masDb.Location = new System.Drawing.Point(153, 25);
            this.masDb.Name = "masDb";
            this.masDb.Size = new System.Drawing.Size(820, 25);
            this.masDb.TabIndex = 0;
            // 
            // closeMe
            // 
            this.closeMe.Location = new System.Drawing.Point(25, 86);
            this.closeMe.Name = "closeMe";
            this.closeMe.Size = new System.Drawing.Size(75, 23);
            this.closeMe.TabIndex = 9;
            this.closeMe.Text = "Exit";
            this.closeMe.UseVisualStyleBackColor = true;
            this.closeMe.Click += new System.EventHandler(this.closeMe_Click);
            // 
            // cmdString
            // 
            this.cmdString.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdString.Location = new System.Drawing.Point(284, 117);
            this.cmdString.Name = "cmdString";
            this.cmdString.Size = new System.Drawing.Size(689, 25);
            this.cmdString.TabIndex = 10;
            // 
            // runSQLQuery
            // 
            this.runSQLQuery.Location = new System.Drawing.Point(153, 117);
            this.runSQLQuery.Name = "runSQLQuery";
            this.runSQLQuery.Size = new System.Drawing.Size(111, 23);
            this.runSQLQuery.TabIndex = 11;
            this.runSQLQuery.Text = "Run this SQL query";
            this.runSQLQuery.UseVisualStyleBackColor = true;
            this.runSQLQuery.Click += new System.EventHandler(this.runSQLQuery_Click);
            // 
            // sasDb
            // 
            this.sasDb.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sasDb.Location = new System.Drawing.Point(153, 57);
            this.sasDb.Name = "sasDb";
            this.sasDb.Size = new System.Drawing.Size(820, 25);
            this.sasDb.TabIndex = 12;
            // 
            // tableName
            // 
            this.tableName.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableName.Location = new System.Drawing.Point(284, 86);
            this.tableName.Name = "tableName";
            this.tableName.Size = new System.Drawing.Size(689, 25);
            this.tableName.TabIndex = 13;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(25, 51);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "Get Info";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.getInfoClick);
            // 
            // clearScreen
            // 
            this.clearScreen.Location = new System.Drawing.Point(25, 137);
            this.clearScreen.Name = "clearScreen";
            this.clearScreen.Size = new System.Drawing.Size(75, 23);
            this.clearScreen.TabIndex = 15;
            this.clearScreen.Text = "ClearScreen";
            this.clearScreen.UseVisualStyleBackColor = true;
            this.clearScreen.Click += new System.EventHandler(this.ClearResut);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1003, 513);
            this.Controls.Add(this.clearScreen);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tableName);
            this.Controls.Add(this.sasDb);
            this.Controls.Add(this.runSQLQuery);
            this.Controls.Add(this.cmdString);
            this.Controls.Add(this.closeMe);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.result);
            this.Controls.Add(this.verify);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.connect);
            this.Controls.Add(this.masDb);
            this.Name = "MainForm";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button verify;
        private System.Windows.Forms.TextBox result;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox masDb;
        private System.Windows.Forms.Button closeMe;
        private System.Windows.Forms.TextBox cmdString;
        private System.Windows.Forms.Button runSQLQuery;
        private System.Windows.Forms.TextBox sasDb;
        private System.Windows.Forms.TextBox tableName;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button clearScreen;
    }
}

