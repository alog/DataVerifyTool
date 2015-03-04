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
            this.btnVerify = new System.Windows.Forms.Button();
            this.result = new System.Windows.Forms.TextBox();
            this.closeMe = new System.Windows.Forms.Button();
            this.tableName = new System.Windows.Forms.TextBox();
            this.clearScreen = new System.Windows.Forms.Button();
            this.connect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.masDb = new System.Windows.Forms.TextBox();
            this.sasDb = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.SASConnect = new System.Windows.Forms.CheckBox();
            this.MASConnect = new System.Windows.Forms.CheckBox();
            this.deleteObject = new System.Windows.Forms.Button();
            this.objectId = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnVerify
            // 
            this.btnVerify.Location = new System.Drawing.Point(84, 96);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(69, 23);
            this.btnVerify.TabIndex = 5;
            this.btnVerify.Text = "Verify";
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.verifyClick);
            // 
            // result
            // 
            this.result.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.result.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.result.Location = new System.Drawing.Point(0, 177);
            this.result.Multiline = true;
            this.result.Name = "result";
            this.result.ReadOnly = true;
            this.result.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.result.Size = new System.Drawing.Size(1088, 336);
            this.result.TabIndex = 7;
            // 
            // closeMe
            // 
            this.closeMe.Location = new System.Drawing.Point(133, 148);
            this.closeMe.Name = "closeMe";
            this.closeMe.Size = new System.Drawing.Size(75, 23);
            this.closeMe.TabIndex = 9;
            this.closeMe.Text = "Exit";
            this.closeMe.UseVisualStyleBackColor = true;
            this.closeMe.Click += new System.EventHandler(this.closeMe_Click);
            // 
            // tableName
            // 
            this.tableName.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableName.Location = new System.Drawing.Point(81, 63);
            this.tableName.Name = "tableName";
            this.tableName.Size = new System.Drawing.Size(230, 25);
            this.tableName.TabIndex = 13;
            // 
            // clearScreen
            // 
            this.clearScreen.Location = new System.Drawing.Point(12, 148);
            this.clearScreen.Name = "clearScreen";
            this.clearScreen.Size = new System.Drawing.Size(75, 23);
            this.clearScreen.TabIndex = 15;
            this.clearScreen.Text = "ClearScreen";
            this.clearScreen.UseVisualStyleBackColor = true;
            this.clearScreen.Click += new System.EventHandler(this.ClearResut);
            // 
            // connect
            // 
            this.connect.Location = new System.Drawing.Point(46, 96);
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
            this.label1.Location = new System.Drawing.Point(10, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "MAS/Standalone";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "SAS";
            // 
            // masDb
            // 
            this.masDb.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masDb.Location = new System.Drawing.Point(105, 23);
            this.masDb.Name = "masDb";
            this.masDb.Size = new System.Drawing.Size(298, 25);
            this.masDb.TabIndex = 0;
            // 
            // sasDb
            // 
            this.sasDb.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sasDb.Location = new System.Drawing.Point(105, 55);
            this.sasDb.Name = "sasDb";
            this.sasDb.Size = new System.Drawing.Size(298, 25);
            this.sasDb.TabIndex = 12;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.sasDb);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.masDb);
            this.groupBox1.Controls.Add(this.connect);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(418, 130);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.SASConnect);
            this.groupBox2.Controls.Add(this.MASConnect);
            this.groupBox2.Controls.Add(this.deleteObject);
            this.groupBox2.Controls.Add(this.objectId);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.tableName);
            this.groupBox2.Controls.Add(this.btnVerify);
            this.groupBox2.Location = new System.Drawing.Point(464, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(613, 130);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Action on table";
            this.groupBox2.Visible = false;
            // 
            // SASConnect
            // 
            this.SASConnect.AutoSize = true;
            this.SASConnect.Location = new System.Drawing.Point(161, 27);
            this.SASConnect.Name = "SASConnect";
            this.SASConnect.Size = new System.Drawing.Size(47, 17);
            this.SASConnect.TabIndex = 19;
            this.SASConnect.Text = "SAS";
            this.SASConnect.UseVisualStyleBackColor = true;
            this.SASConnect.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // MASConnect
            // 
            this.MASConnect.AutoSize = true;
            this.MASConnect.Location = new System.Drawing.Point(15, 27);
            this.MASConnect.Name = "MASConnect";
            this.MASConnect.Size = new System.Drawing.Size(118, 17);
            this.MASConnect.TabIndex = 18;
            this.MASConnect.Text = "MAS or Standalone";
            this.MASConnect.UseVisualStyleBackColor = true;
            this.MASConnect.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.MASConnect.EnabledChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // deleteObject
            // 
            this.deleteObject.Location = new System.Drawing.Point(389, 96);
            this.deleteObject.Name = "deleteObject";
            this.deleteObject.Size = new System.Drawing.Size(111, 23);
            this.deleteObject.TabIndex = 17;
            this.deleteObject.Text = "Delete object";
            this.deleteObject.UseVisualStyleBackColor = true;
            this.deleteObject.Click += new System.EventHandler(this.deleteObject_Click);
            // 
            // objectId
            // 
            this.objectId.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectId.Location = new System.Drawing.Point(389, 61);
            this.objectId.Name = "objectId";
            this.objectId.Size = new System.Drawing.Size(201, 25);
            this.objectId.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(334, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Object id";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Table name";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(208, 96);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(103, 23);
            this.button1.TabIndex = 18;
            this.button1.Text = "Similar table";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1088, 513);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.clearScreen);
            this.Controls.Add(this.closeMe);
            this.Controls.Add(this.result);
            this.Name = "MainForm";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.TextBox result;
        private System.Windows.Forms.Button closeMe;
        private System.Windows.Forms.TextBox tableName;
        private System.Windows.Forms.Button clearScreen;
        private System.Windows.Forms.Button connect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox masDb;
        private System.Windows.Forms.TextBox sasDb;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox objectId;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button deleteObject;
        private System.Windows.Forms.CheckBox SASConnect;
        private System.Windows.Forms.CheckBox MASConnect;
        private System.Windows.Forms.Button button1;
    }
}

