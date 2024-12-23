using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_System
{
    public partial class mj_printReceiptAddOrder : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;


        mj_Purchased mj_purchased;
        public mj_printReceiptAddOrder(mj_Purchased frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());

            mj_purchased = frm;
        }

        private void mj_printReceiptAddOrder_Load(object sender, EventArgs e)
        {

            //this.reportViewer1.RefreshReport();
            this.reportViewer2.RefreshReport();
        }

        public void LoadReport()
        {
            ReportDataSource rptDataSource;
            try
            {
                this.reportViewer2.ProcessingMode = ProcessingMode.Local;
                this.reportViewer2.LocalReport.ReportPath = Application.StartupPath + @"\Reports\mj_reportItemOrder.rdlc";
                this.reportViewer2.LocalReport.DataSources.Clear();

                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                int selectedRowIndex = mj_purchased.dataGridView1.SelectedRows[0].Index;
                int id = int.Parse(mj_purchased.dataGridView1.Rows[selectedRowIndex].Cells["id"].Value.ToString());

                cn.Open();
                string query = "SELECT * FROM tblPurchased WHERE id LIKE '" + id + "'";
                da.SelectCommand = new SqlCommand(query, cn);
                da.Fill(ds.Tables["dtItemOrder"]);
                cn.Close();

                rptDataSource = new ReportDataSource("DataSet1", ds.Tables["dtItemOrder"]);
                reportViewer2.LocalReport.DataSources.Add(rptDataSource);
                reportViewer2.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer2.ZoomMode = ZoomMode.Percent;
                reportViewer2.ZoomPercent = 100;
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }
    }
}
