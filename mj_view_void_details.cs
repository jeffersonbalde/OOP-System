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
    public partial class mj_view_void_details : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;

        mj_Purchased mj_purchased;

        public mj_view_void_details()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        private void mj_view_void_details_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtSearch;

            // Set DatePickers to optional state with CheckBox
            dtFrom.ShowCheckBox = true;
            dtTo.ShowCheckBox = true;

            dtFrom.Checked = false;
            dtTo.Checked = false;

            LoadRecords();
            getTotalProductsGrid();


        }

        public void getTotalProductsGrid()
        {
            try
            {
                //dataGridView1.Rows.Clear();

                // Count the rows in the DataGridView
                int totalOrders = dataGridView1.Rows.Count;

                // Display the total in the label
                lblTotalOrders.Text = totalOrders.ToString();
            }
            catch (Exception ex)
            {
                // Handle any potential errors
                MessageBox.Show("An error occurred while counting total products: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadRecords()
        {
            try
            {
                int i = 0;
                dataGridView1.Rows.Clear();
                string query = "SELECT * FROM tblVoidProduct WHERE 1=1"; // Base query

                cn.Open();

                // Build the query dynamically
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    query += " AND customer_name LIKE @customer_name";
                }
                if (dtFrom.Checked && dtTo.Checked)
                {
                    query += " AND void_date >= @start_date AND void_date < @end_date";
                }
                else if (dtFrom.Checked) // Only From date is set
                {
                    query += " AND void_date >= @start_date";
                }
                else if (dtTo.Checked) // Only To date is set
                {
                    query += " AND void_date < @end_date";
                }

                query += " ORDER BY void_date"; // Add order 


                cm = new SqlCommand(query, cn);

                // Add parameters to prevent SQL injection
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    cm.Parameters.AddWithValue("@customer_name", txtSearch.Text + "%");
                }

                if (dtFrom.Checked)
                {
                    cm.Parameters.AddWithValue("@start_date", dtFrom.Value.Date);
                }
                if (dtTo.Checked)
                {
                    cm.Parameters.AddWithValue("@end_date", dtTo.Value.Date.AddDays(1));
                }

                dr = cm.ExecuteReader();

                while (dr.Read())
                {
                    string formattedDate = dr["void_date"] != DBNull.Value && DateTime.TryParse(dr["void_date"].ToString(), out DateTime parsedDate)
                        ? parsedDate.ToString("MM/dd/yyyy")
                        : "N/A";

                    i++;
                    dataGridView1.Rows.Add(i, formattedDate, dr["customer_name"].ToString(), dr["item_description"].ToString(),
                        dr["reason_for_void"].ToString(), dr["voided_by"].ToString(), dr["void_condition"].ToString(),
                        dr["payment_amount"].ToString(), dr["void_id"].ToString());
                }

                dr.Close();
                cn.Close();

                // Show/hide the DataGridView and label based on the number of rows
                if (dataGridView1.Rows.Count == 0)
                {
                    lblNoLowStocks.Visible = true;
                    dataGridView1.Visible = false;
                }
                else
                {
                    lblNoLowStocks.Visible = false;
                    dataGridView1.Visible = true;
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;

            string date = "";
            string item_description = "";
            string price_per_pc = "";
            string qty = "";
            string total_cost = "";
            string customer_name = "";
            string contact_number = "";
            string void_date = "";
            string reason_for_void = "";
            string voided_by = "";
            string void_condition = "";
            string payment_amount = "";

            if (colName == "View")
            {

                int selectedRowIndex = dataGridView1.SelectedRows[0].Index;

                string void_id = dataGridView1.Rows[selectedRowIndex].Cells["void_id"].Value.ToString();

                try
                {

                    if (dataGridView1.SelectedRows.Count == 0)
                    {
                        MessageBox.Show("No item has been specified.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {

                        cn.Open();
                        string query1 = "SELECT * FROM tblVoidProduct WHERE void_id = @void_id";
                        cm = new SqlCommand(query1, cn);
                        cm.Parameters.AddWithValue("@void_id", int.Parse(void_id));
                        dr = cm.ExecuteReader();
                        dr.Read();

                        if (dr.HasRows)
                        {

                            date = dr["order_date"].ToString();
                            item_description = dr["item_description"].ToString();

                            price_per_pc = dr["price_per_pc"].ToString();
                            price_per_pc = price_per_pc.Replace("?", "₱");

                            qty = dr["qty"].ToString();

                            total_cost = dr["total_cost"].ToString();
                            total_cost = total_cost.Replace("?", "₱");

                            customer_name = dr["customer_name"].ToString();
                            contact_number = dr["contact_number"].ToString();

                            void_date = dr["void_date"].ToString();
                            if (DateTime.TryParse(void_date, out DateTime parsedVoidDate))
                            {

                                void_date = parsedVoidDate.ToString("MM/dd/yyyy"); // Format as "Year-Month-Day"
                            }

                            reason_for_void = dr["reason_for_void"].ToString();
                            voided_by = dr["voided_by"].ToString();
                            void_condition = dr["void_condition"].ToString();
                            payment_amount = dr["payment_amount"].ToString();

                        }

                        dr.Close();
                        cn.Close();

                        view_void frm = new view_void(this, date, item_description, price_per_pc, qty, total_cost, customer_name, contact_number, void_date, reason_for_void, voided_by, void_condition, payment_amount);
                        frm.ShowDialog();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadRecords();
            getTotalProductsGrid();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            LoadRecords();
            getTotalProductsGrid();
        }
    }
}
