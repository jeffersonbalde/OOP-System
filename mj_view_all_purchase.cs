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
    public partial class mj_view_all_purchase : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;

        mj_itemPurchase mj_item_purchase;

        public mj_view_all_purchase(mj_itemPurchase frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            mj_item_purchase = frm;
        }

        public void AutoComplete()
        {
            try
            {
                cn.Open();
                String query = "SELECT * FROM tblPurchased WHERE status <> 'Voided'";
                cm = new SqlCommand(query, cn);
                dr = cm.ExecuteReader();
                AutoCompleteStringCollection cl = new AutoCompleteStringCollection();

                //cl.Add("Business Expense"); 

                while (dr.Read())
                {
                    cl.Add(dr["client"].ToString());
                }

                txtFilterByCustomer.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtFilterByCustomer.AutoCompleteSource = AutoCompleteSource.CustomSource;

                txtFilterByCustomer.AutoCompleteCustomSource = cl;
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        public void LoadRecords()
        {

            try
            {
                int i = 0;
                dataGridView1.Rows.Clear();
                string query = "SELECT * FROM tblItemPurchase WHERE 1=1"; // Base query

                cn.Open();

                // Build the query dynamically
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    query += " AND purchase_description LIKE @purchase_description";
                }
                if (txtFilterByCustomer.SelectedIndex != -1 && txtFilterByCustomer.Text != "All")
                {
                    query += " AND purchase_under LIKE @purchase_under";
                }

                query += " ORDER BY purchase_date"; // Add order 

                cm = new SqlCommand(query, cn);

                // Add parameters to prevent SQL injection
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    cm.Parameters.AddWithValue("@purchase_description", "%" + txtSearch.Text + "%");
                }
                if (txtFilterByCustomer.SelectedIndex != -1 && txtFilterByCustomer.Text != "All")
                {
                    cm.Parameters.AddWithValue("@purchase_under", "%" + txtFilterByCustomer.SelectedItem.ToString() + "%");
                }

                //cm = new SqlCommand("SELECT * FROM tblExpenses", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    // Check if amount and total_cost are null or empty before parsing
                    string total_amount = dr["total_amount"] != DBNull.Value && !string.IsNullOrWhiteSpace(dr["total_amount"].ToString())
                        ? "₱" + Double.Parse(dr["total_amount"].ToString()).ToString("#,##0.00")
                        : "₱0.00";

                    string purchase_price = dr["purchase_price"] != DBNull.Value && !string.IsNullOrWhiteSpace(dr["purchase_price"].ToString())
                        ? "₱" + Double.Parse(dr["purchase_price"].ToString()).ToString("#,##0.00")
                        : "₱0.00";

                    //string formattedDate = dr["date"] != DBNull.Value && DateTime.TryParse(dr["date"].ToString(), out DateTime parsedDate)
                    //    ? parsedDate.ToString("MM/dd/yyyy")
                    //    : "N/A";

                    string purchase_qty = int.Parse(dr["purchase_qty"].ToString()).ToString("N0");

                    dataGridView1.Rows.Add(i, dr["purchase_date"].ToString(), dr["purchase_under"].ToString(), dr["purchase_description"].ToString(), purchase_qty, purchase_price, total_amount, dr["id"].ToString());
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

        public void CalculateTotalExpense()
        {
            decimal totalExpense = 0;

            try
            {
                // Iterate through each row in the DataGridView
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["total"].Value != null)
                    {
                        string amountString = row.Cells["total"].Value.ToString();

                        // Remove the peso sign (₱) if it exists
                        if (amountString.Contains("₱"))
                        {
                            amountString = amountString.Replace("₱", "").Trim();
                        }

                        // Parse the amount to a decimal
                        if (decimal.TryParse(amountString, out decimal value))
                        {
                            totalExpense += value; // Add the parsed value to totalExpense
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating total expense: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Display the total expense in the label with proper formatting
            lblTotalOrders.Text = "₱" + totalExpense.ToString("#,##0.00");
        }

        public void LoadFilterByCategory()
        {
            try
            {
                txtFilterByCustomer.Items.Clear();
                txtFilterByCustomer.Items.Add("All");
                txtFilterByCustomer.SelectedIndex = 0; // Default to "All"
                cn.Open();
                string query = "SELECT * FROM tblPurchased WHERE status <> 'Voided'";
                cm = new SqlCommand(query, cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    txtFilterByCustomer.Items.Add(dr["client"].ToString());
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

        private void mj_view_all_purchase_Load(object sender, EventArgs e)
        {
            LoadRecords();
            CalculateTotalExpense();
            LoadFilterByCategory();
            AutoComplete();

            this.ActiveControl = txtSearch;
        }

        private void txtFilterByCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRecords();
            CalculateTotalExpense();
        }

        private void txtFilterByCustomer_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
            CalculateTotalExpense();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (colName == "Delete")
            {
                // Show a confirmation prompt before deletion
                DialogResult result = MessageBox.Show("Are you sure you want to permanently delete this item? This action cannot be undone.",
                                                      "Confirm Deletion",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        cn.Open();
                        string query = "DELETE FROM tblItemPurchase WHERE id LIKE '" + dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString() + "'";
                        cm = new SqlCommand(query, cn);
                        cm.ExecuteNonQuery();
                        cn.Close();
                        LoadRecords();
                        //calculateExpenses();
                        CalculateTotalExpense();

                        mj_item_purchase.LoadRecords();
                        mj_item_purchase.CalculateTotalExpense();

                        // Show success message after deletion
                        MessageBox.Show("The item has been successfully deleted from the system.",
                                        "Item Deleted",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        cn.Close();
                        MessageBox.Show("An error occurred while trying to delete the item. Please try again.",
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
