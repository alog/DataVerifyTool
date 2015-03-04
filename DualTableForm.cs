using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace muweili
{
    public partial class DualTableForm : Form
    {
        public DataGridView masDGV,sasDGV;
        public DualTableForm()
        {
            InitializeComponent();
            masDGV = masDataGridView;
            sasDGV = sasDataGridView;
        }

        public DualTableForm(DataTable masDT)
        {
            InitializeComponent();
            masDGV = masDataGridView;
            masDGV.RowHeadersVisible = false;
            //masDGV.AutoResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader);
            masDGV.DataSource = masDT;
            sasDGV = sasDataGridView;
        }

        private void sasDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void masDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


    }
}
