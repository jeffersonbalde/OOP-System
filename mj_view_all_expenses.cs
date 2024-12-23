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
    public partial class mj_view_all_expenses : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;

        mj_Expenses mj_expenses;

        public mj_view_all_expenses(mj_Expenses frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            mj_expenses = frm;
        }

        private void mj_view_all_expenses_Load(object sender, EventArgs e)
        {
            LoadRecords();
            CalculateTotalExpense();
            LoadFilterByCategory();
            AutoComplete();

            this.ActiveControl = txtSearch;
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
                string query = "SELECT * FROM tblExpenses WHERE 1=1"; // Base query

                cn.Open();

                // Build the query dynamically
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    query += " AND expense LIKE @expense";
                }
                if (txtFilterByCustomer.SelectedIndex != -1 && txtFilterByCustomer.Text != "All")
                {
                    query += " AND customer LIKE @customer";
                }

                query += " ORDER BY date"; // Add order 

                cm = new SqlCommand(query, cn);

                // Add parameters to prevent SQL injection
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    cm.Parameters.AddWithValue("@expense", "%" + txtSearch.Text + "%");
                }
                if (txtFilterByCustomer.SelectedIndex != -1 && txtFilterByCustomer.Text != "All")
                {
                    cm.Parameters.AddWithValue("@customer", "%" + txtFilterByCustomer.SelectedItem.ToString() + "%");
                }

                //cm = new SqlCommand("SELECT * FROM tblExpenses", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    // Check if amount and total_cost are null or empty before parsing
                    string amount = dr["amount"] != DBNull.Value && !string.IsNullOrWhiteSpace(dr["amount"].ToString())
                        ? "₱" + Double.Parse(dr["amount"].ToString()).ToString("#,##0.00")
                        : "₱0.00";

                    string totalcost = dr["total_cost"] != DBNull.Value && !string.IsNullOrWhiteSpace(dr["total_cost"].ToString())
                        ? "₱" + Double.Parse(dr["total_cost"].ToString()).ToString("#,##0.00")
                        : "₱0.00";

                    //string formattedDate = dr["date"] != DBNull.Value && DateTime.TryParse(dr["date"].ToString(), out DateTime parsedDate)
                    //    ? parsedDate.ToString("MM/dd/yyyy")
                    //    : "N/A";

                    dataGridView1.Rows.Add(i, dr["date"].ToString(), dr["customer"].ToString(), dr["expense"].ToString(), amount, int.Parse(dr["id"].ToString()));
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
                    if (row.Cells["amount"].Value != null)
                    {
                        string amountString = row.Cells["amount"].Value.ToString();

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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void txtFilterByCustomer_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
            CalculateTotalExpense();
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

        private void txtFilterByCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRecords();
            CalculateTotalExpense();
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
                        string query = "DELETE FROM tblExpenses WHERE id LIKE '" + dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString() + "'";
                        cm = new SqlCommand(query, cn);
                        cm.ExecuteNonQuery();
                        cn.Close();
                        LoadRecords();
                        //calculateExpenses();
                        CalculateTotalExpense();

                        mj_expenses.LoadRecords();
                        mj_expenses.CalculateTotalExpense();

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
