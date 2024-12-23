using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing;

namespace OOP_System
{
    public partial class mj_view_low_stocks : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;


        public mj_view_low_stocks()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        public void getTotalOrders()
        {


            try
            {
                //dataGridView1.Rows.Clear();

                // Count the rows in the DataGridView
                int totalOrders = dataGridView1.Rows.Count;

                // Display the total in the label
                lblTotalLowStocks.Text = totalOrders.ToString();
            }
            catch (Exception ex)
            {
                // Handle any potential errors
                MessageBox.Show("An error occurred while counting total low stocks: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadRecords()
        {
            try
            {
                int i = 0;
                dataGridView1.Rows.Clear(); // Clear existing data in DataGridView
                lblNoLowStocks.Visible = false; // Hide the label by default
                dataGridView1.Visible = true; // Ensure DataGridView is visible by default

                cn.Open();
                string query = "SELECT * FROM tblProductMaster WHERE qty <= 10";
                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    query += " AND description LIKE @search";
                }

                cm = new SqlCommand(query, cn);
                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    cm.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");
                }

                dr = cm.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        i++;
                        dataGridView1.Rows.Add(i, dr["description"].ToString(), dr["qty"].ToString());
                    }
                }
                else
                {
                    // No data to show, hide DataGridView and display the label
                    dataGridView1.Visible = false;
                    //lblNoLowStocks.Text = "No low stock items found.";
                    lblNoLowStocks.Visible = true;
                    txtSearch.Visible = false;
                }

                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }



        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lblTotalLowStocks_Click(object sender, EventArgs e)
        {

        }

        private void mj_view_low_stocks_Load(object sender, EventArgs e)
        {
            LoadRecords();
            getTotalOrders();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }
    }
}
