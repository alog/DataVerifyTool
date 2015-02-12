namespace muweili
{
    partial class DualTableForm
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
            this.masDataGridView = new System.Windows.Forms.DataGridView();
            this.masLabel = new System.Windows.Forms.Label();
            this.sasLabel = new System.Windows.Forms.Label();
            this.sasDataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.masDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sasDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // masDataGridView
            // 
            this.masDataGridView.AllowUserToOrderColumns = true;
            this.masDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.masDataGridView.Location = new System.Drawing.Point(12, 26);
            this.masDataGridView.Name = "masDataGridView";
            this.masDataGridView.ReadOnly = true;
            this.masDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.masDataGridView.Size = new System.Drawing.Size(560, 385);
            this.masDataGridView.TabIndex = 0;
            // 
            // masLabel
            // 
            this.masLabel.AutoSize = true;
            this.masLabel.Location = new System.Drawing.Point(12, 7);
            this.masLabel.Name = "masLabel";
            this.masLabel.Size = new System.Drawing.Size(30, 13);
            this.masLabel.TabIndex = 1;
            this.masLabel.Text = "MAS";
            // 
            // sasLabel
            // 
            this.sasLabel.AutoSize = true;
            this.sasLabel.Location = new System.Drawing.Point(575, 7);
            this.sasLabel.Name = "sasLabel";
            this.sasLabel.Size = new System.Drawing.Size(28, 13);
            this.sasLabel.TabIndex = 3;
            this.sasLabel.Text = "SAS";
            // 
            // sasDataGridView
            // 
            this.sasDataGridView.AllowUserToOrderColumns = true;
            this.sasDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sasDataGridView.Location = new System.Drawing.Point(578, 26);
            this.sasDataGridView.Name = "sasDataGridView";
            this.sasDataGridView.ReadOnly = true;
            this.sasDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.sasDataGridView.Size = new System.Drawing.Size(560, 385);
            this.sasDataGridView.TabIndex = 2;
            // 
            // TableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1152, 423);
            this.Controls.Add(this.sasLabel);
            this.Controls.Add(this.sasDataGridView);
            this.Controls.Add(this.masLabel);
            this.Controls.Add(this.masDataGridView);
            this.Name = "TableForm";
            this.Text = "Table";
            ((System.ComponentModel.ISupportInitialize)(this.masDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sasDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView masDataGridView;
        private System.Windows.Forms.Label masLabel;
        private System.Windows.Forms.Label sasLabel;
        private System.Windows.Forms.DataGridView sasDataGridView;
    }
}