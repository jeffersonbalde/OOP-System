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
using System.Windows.Forms.DataVisualization.Charting;

namespace OOP_System
{
    public partial class mj_Dashboard : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();        
        SqlDataReader dr;

        public mj_Dashboard()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        public void FillChartByYear()
        {
            try
            {
                cn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT YEAR(date) AS year, ISNULL (SUM(total_cost), 0.00) AS total FROM tblPurchased WHERE status LIKE 'PAID' GROUP BY YEAR(date)", cn);
                DataSet ds = new DataSet();
                da.Fill(ds, "Sales");
                chart1.DataSource = ds.Tables["Sales"];
                Series series1 = chart1.Series["Sales"];
                series1.ChartType = SeriesChartType.Doughnut;

                series1.Name = "SALES CHART";

                var chart = chart1;
                chart.Series[series1.Name].XValueMember = "year";
                chart.Series[series1.Name].YValueMembers = "total";
                chart.Series[0].IsValueShownAsLabel = true;


                chart1.Titles.Add("Sales Chart");
                chart1.Titles[0].Font = new Font("Arial", 20, FontStyle.Bold);
                chart1.Titles[0].ForeColor = Color.FromArgb(192, 0, 0);
                chart1.Titles[0].Alignment = ContentAlignment.MiddleCenter;

                chart1.BorderlineDashStyle = ChartDashStyle.Solid;
                chart1.BorderlineColor = Color.DimGray;
                chart1.BorderlineWidth = 1;
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }
        public void FillChart()
        {
            try
            {
                DataTable dt = new DataTable();
                cn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT FORMAT(date, 'yyyy-MM') AS Month, SUM(total_cost) AS total_cost FROM tblPurchased GROUP BY FORMAT(date, 'yyyy-MM')", cn);
                da.Fill(dt);
                chart1.DataSource = dt;
                cn.Close();

                chart1.Series["Sales"].XValueMember = "Month";
                chart1.Series["Sales"].YValueMembers = "total_cost";

                chart1.Titles.Add("Sales Chart");
                chart1.Titles[0].Font = new Font("Arial", 20, FontStyle.Bold);
                chart1.Titles[0].ForeColor = Color.FromArgb(192, 0, 0);
                chart1.Titles[0].Alignment = ContentAlignment.MiddleCenter;

                chart1.BorderlineDashStyle = ChartDashStyle.Solid;
                chart1.BorderlineColor = Color.DimGray;
                chart1.BorderlineWidth = 1;
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void mj_Dashboard_Load(object sender, EventArgs e)
        {
            CalculateSales();
            //FillChart();
            getItemSales();
            FillChartByYear();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {

        }

        public void CalculateSales()
        {
            //Double totalSales = 0;
            //Double totalExpenses = 0;
            //Double totalPurchase = 0;
            //int totalOrders = 0;
            //int totalProducts = 0;
            //try
            //{

            //    //total Sales
            //    cn.Open();
            //    string query = "SELECT ISNULL(SUM(total_cost), 0.00) AS total FROM tblPurchased WHERE date BETWEEN '" + txtDate1.Value.ToString("yyyy-MM-dd") + "' AND '" + txtDate2.Value.ToString("yyyy-MM-dd") + "' AND status LIKE 'PAID'";
            //    cm = new SqlCommand(query, cn);
            //    dr = cm.ExecuteReader();
            //    while (dr.Read())
            //    {
            //        totalSales += double.Parse(dr["total"].ToString());
            //        lblTotalSales.Text = "₱" + totalSales.ToString("#,##0.00");
            //    }
            //    dr.Close();
            //    cn.Close();

            //    //total expenses
            //    cn.Open();
            //    string query2 = "SELECT ISNULL(SUM(amount), 0.00) AS total FROM tblExpenses WHERE date BETWEEN '" + txtDate1.Value.ToString("yyyy-MM-dd") + "' AND '" + txtDate2.Value.ToString("yyyy-MM-dd") + "'";
            //    cm = new SqlCommand(query2, cn);
            //    dr = cm.ExecuteReader();
            //    while (dr.Read())
            //    {
            //        totalExpenses += double.Parse(dr["total"].ToString());
            //        lblTotalExpense.Text = "₱" + totalExpenses.ToString("#,##0.00");
            //    }
            //    dr.Close();
            //    cn.Close();

            //    //total purchase
            //    cn.Open();
            //    string query3 = "SELECT ISNULL(SUM(amount), 0.00) AS total FROM tblItemPurchase WHERE date BETWEEN '" + txtDate1.Value.ToString("yyyy-MM-dd") + "' AND '" + txtDate2.Value.ToString("yyyy-MM-dd") + "'";
            //    cm = new SqlCommand(query3, cn);
            //    dr = cm.ExecuteReader();
            //    while (dr.Read())
            //    {
            //        totalPurchase += double.Parse(dr["total"].ToString());
            //        lblTotalPurchase.Text = "₱" + totalPurchase.ToString("#,##0.00");
            //    }
            //    dr.Close();
            //    cn.Close();

            //    //total orders
            //    cn.Open();
            //    string query4 = "SELECT COUNT(*) FROM tblPurchased WHERE date BETWEEN '" + txtDate1.Value.ToString("yyyy-MM-dd") + "' AND '" + txtDate2.Value.ToString("yyyy-MM-dd") + "' AND status LIKE 'PAID'";
            //    cm = new SqlCommand(query4, cn);
            //    totalOrders = int.Parse(cm.ExecuteScalar().ToString());
            //    lbllTotalOrders.Text = totalOrders.ToString();          
            //    cn.Close();

            //    //total products
            //    cn.Open();
            //    string query5 = "SELECT COUNT(*) FROM tblProductMaster";    
            //    cm = new SqlCommand(query5, cn);
            //    totalProducts = int.Parse(cm.ExecuteScalar().ToString());
            //    lblTotalProducts.Text = totalProducts.ToString();
            //    cn.Close();

            //}
            //catch (Exception ex)
            //{
            //    cn.Close();
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void txtDate1_ValueChanged(object sender, EventArgs e)
        {
            CalculateSales();
        }

        private void txtDate2_ValueChanged(object sender, EventArgs e)
        {
            CalculateSales();
        }

        public void getItemSales()
        {
            try
            {
                int i = 0;
                dataGridView1.Rows.Clear();

                cn.Open();
                // Query to sum up total_cost for each unique item that has a non-zero total cost
                string query = "SELECT item_description, SUM(total_cost) AS total_sales FROM tblPurchased WHERE status = 'PAID' AND total_cost > 0 GROUP BY item_description";
                cm = new SqlCommand(query, cn);
                dr = cm.ExecuteReader();

                // Loop through each result and add it to the DataGridView
                while (dr.Read())
                {
                    i++;
                    // Check if total_sales is not null beforeparsing
                    string totalSales = dr["total_sales"] !=  DBNull.Value && !string.IsNullOrWhiteSpace(dr["total_sales"].ToString())
                        ? "₱" + Double.Parse(dr["total_sales"].ToString()).ToString("#,##0.00")
                        : "₱0.00";

                    // Add the row manually
                    dataGridView1.Rows.Add(i, dr["item_description"].ToString(), totalSales);
                }

                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }
    }
}