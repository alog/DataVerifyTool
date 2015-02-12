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

        public DualTableForm(SqlConnection cnn1, SqlConnection cnn2, string tablename)
        {
            InitializeComponent();
            masDGV = masDataGridView;
            sasDGV = sasDataGridView;
        }


    }
}
