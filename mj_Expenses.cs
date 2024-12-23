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
    public partial class mj_Expenses : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        public mj_Expenses()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
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

            if(colName == "Edit")
            {
                try
                {
                    // Check if the DataGridView2 has any rows
                    if (dataGridView1.Rows.Count == 0)
                    {
                        MessageBox.Show("There are no items to update. Please ensure there is data available.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Check if any row is selected
                    if (dataGridView1.SelectedRows.Count == 0)
                    {
                        MessageBox.Show("No item has been specified.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        // Get the selected row index
                        int selectedRowIndex = dataGridView1.SelectedRows[0].Index;

                        // Retrieve the data from the selected ro

                        String customer = dataGridView1.Rows[selectedRowIndex].Cells[2].Value.ToString();
                        //DateTime date = DateTime.Parse(dataGridView1.Rows[selectedRowIndex].Cells[1].Value.ToString());
                        string expenses = dataGridView1.Rows[selectedRowIndex].Cells[3].Value.ToString();
                        string amount = dataGridView1.Rows[selectedRowIndex].Cells[4].Value.ToString();
                        int id = int.Parse(dataGridView1.Rows[selectedRowIndex].Cells[5].Value.ToString());
                        string pricePerPc = dataGridView1.Rows[selectedRowIndex].Cells[4].Value.ToString();
                        DateTime expense_date = DateTime.Parse(dataGridView1.Rows[selectedRowIndex].Cells["date"].Value.ToString());

                        // Remove '₱' and '.' from the price and trim '00' if present
                        pricePerPc = pricePerPc.Replace("₱", "")   // Remove currency symbol
                                               .Replace(".", ""); // Remove dot
                        if (pricePerPc.EndsWith("00"))            // Remove trailing '00'
                        {
                            pricePerPc = pricePerPc.Substring(0, pricePerPc.Length - 2);
                        }

                        // Create an instance of the update form and pass the data including the optional fields
                        mj_updateExpense frm = new mj_updateExpense(this, expense_date, customer, expenses, pricePerPc, id);
                        frm.LoadCustomer();
                        frm.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            mj_addExpense frm = new mj_addExpense(this);
            frm.ShowDialog();
        }

        public void LoadRecords()
        {

            //try
            //{
            //    int i = 0;
            //    dataGridView1.Rows.Clear();

            //    cn.Open();

            //    if (txtFilterByCustomer.Text == "")
            //    {
            //    string query2 = "SELECT * FROM tblExpenses";
            //        //string query1 = "SELECT c.transno, c.pcode, p.pdesc, c.price, c.qty, c.disc, SUM(c.total) AS total FROM tblCart as c INNER JOIN tblProduct as p ON c.pcode = p.pcode WHERE status LIKE 'Sold' AND sdate BETWEEN '" + dt1.Value.ToString("yyyy-MM-dd") + "' AND '" + dt2.Value.ToString("yyyy-MM-dd") + "' GROUP BY c.transno ORDER BY total DESC";
            //        cm = new SqlCommand(query2, cn);
            //    }
            //    else
            //    {

            //    string query = "SELECT * FROM tblExpenses WHERE customer LIKE '" + txtFilterByCustomer.Text + "'";
            //        //string query = "SELECT c.id, c.transno, c.pcode, p.pdesc, c.price, c.qty, c.disc, c.total FROM tblCart as c INNER JOIN tblProduct as p ON c.pcode = p.pcode WHERE status LIKE 'Sold' AND sdate BETWEEN '" + dt1.Value.ToString("yyyy-MM-dd") + "' AND '" + dt2.Value.ToString("yyyy-MM-dd") + "' AND cashier LIKE '" + cboCashier.Text + "'";
            //        cm = new SqlCommand(query, cn);
            //    }

            //    //cm = new SqlCommand("SELECT * FROM tblExpenses", cn);
            //    dr = cm.ExecuteReader();
            //    while (dr.Read())
            //    {
            //        i++;
            //    // Check if amount and total_cost are null or empty before parsing
            //    string amount = dr["amount"] != DBNull.Value && !string.IsNullOrWhiteSpace(dr["amount"].ToString())
            //        ? "₱" + Double.Parse(dr["amount"].ToString()).ToString("#,##0.00")
            //        : "₱0.00";

            //    string totalcost = dr["total_cost"] != DBNull.Value && !string.IsNullOrWhiteSpace(dr["total_cost"].ToString())
            //        ? "₱" + Double.Parse(dr["total_cost"].ToString()).ToString("#,##0.00")
            //        : "₱0.00";

            //    string formattedDate = dr["date"] != DBNull.Value && DateTime.TryParse(dr["date"].ToString(), out DateTime parsedDate)
            //        ? parsedDate.ToString("MM/dd/yyyy")
            //        : "N/A";

            //        dataGridView1.Rows.Add(i, formattedDate, dr["customer"].ToString(), dr["expense"].ToString(), amount, int.Parse(dr["id"].ToString()));
            //    }
            //    dr.Close();
            //    cn.Close();
            //}
            //catch (Exception ex)
            //{
            //    cn.Close();
            //    MessageBox.Show(ex.Message);

            //}

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

                // Add date filtering logic only if dates are selected
                if (dtFrom.Checked && dtTo.Checked)
                {
                    query += " AND date >= @start_date AND date < @end_date";
                }
                else if (dtFrom.Checked) // Only From date is set
                {
                    query += " AND date >= @start_date";
                }
                else if (dtTo.Checked) // Only To date is set
                {
                    query += " AND date < @end_date";
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

                if (dtFrom.Checked)
                {
                    cm.Parameters.AddWithValue("@start_date", dtFrom.Value.Date);
                }
                if (dtTo.Checked)
                {
                    cm.Parameters.AddWithValue("@end_date", dtTo.Value.Date.AddDays(1));
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

                    string date = dr["date"] != DBNull.Value && DateTime.TryParse(dr["date"].ToString(), out DateTime parsedPaymentDate)
                    ? parsedPaymentDate.ToString("MMMM d, yyyy")
                    : "";

                    dataGridView1.Rows.Add(i, date, dr["customer"].ToString(), dr["expense"].ToString(), amount, int.Parse(dr["id"].ToString()));
                }
                dr.Close();
                cn.Close();

                if (dataGridView1.Rows.Count == 0)
                {
                    lblNoLowStocks.Visible = true;
                    //dataGridView1.Visible = false;
                }
                else
                {
                    lblNoLowStocks.Visible = false;
                    //dataGridView1.Visible = true;
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);

            }

            //private void mj_Expenses_Load(object sender, EventArgs e)
            //{

            //}

            //private void btnUpdate_Click(object sender, EventArgs e)
            //{
            //try
            //{
            //    // Check if any row is selected
            //    if (dataGridView1.SelectedRows.Count == 0)
            //    {
            //        MessageBox.Show("No item has been specified.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return;
            //    }
            //    else
            //    {
            //        // Get the selected row index
            //        int selectedRowIndex = dataGridView1.SelectedRows[0].Index;

            //        // Retrieve the data from the selected row
            //        DateTime date = DateTime.Parse(dataGridView1.Rows[selectedRowIndex].Cells[1].Value.ToString());
            //        string expenses = dataGridView1.Rows[selectedRowIndex].Cells[2].Value.ToString();
            //        string category = dataGridView1.Rows[selectedRowIndex].Cells[3].Value.ToString();
            //        string amount = dataGridView1.Rows[selectedRowIndex].Cells[4].Value.ToString();
            //        int id = int.Parse(dataGridView1.Rows[selectedRowIndex].Cells[5].Value.ToString());

            //        // Create an instance of the update form and pass the data including the optional fields
            //        mj_updateExpense frm = new mj_updateExpense(this, date, expenses, category, amount, id);
            //        frm.ShowDialog();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //}
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
            lblTotalExpenses.Text = "₱" + totalExpense.ToString("#,##0.00");
        }



        public void calculateExpenses()
        {
            Double totalItem = 0;

            try
            {
                cn.Open();

                if (txtFilterByCustomer.Text == "")
                {
                    string query2 = "SELECT * FROM tblExpenses";
                    //string query1 = "SELECT c.transno, c.pcode, p.pdesc, c.price, c.qty, c.disc, SUM(c.total) AS total FROM tblCart as c INNER JOIN tblProduct as p ON c.pcode = p.pcode WHERE status LIKE 'Sold' AND sdate BETWEEN '" + dt1.Value.ToString("yyyy-MM-dd") + "' AND '" + dt2.Value.ToString("yyyy-MM-dd") + "' GROUP BY c.transno ORDER BY total DESC";
                    cm = new SqlCommand(query2, cn);
                }
                else
                {

                    string query = "SELECT * FROM tblExpenses WHERE customer LIKE '" + txtFilterByCustomer.Text + "'";
                    //string query = "SELECT c.id, c.transno, c.pcode, p.pdesc, c.price, c.qty, c.disc, c.total FROM tblCart as c INNER JOIN tblProduct as p ON c.pcode = p.pcode WHERE status LIKE 'Sold' AND sdate BETWEEN '" + dt1.Value.ToString("yyyy-MM-dd") + "' AND '" + dt2.Value.ToString("yyyy-MM-dd") + "' AND cashier LIKE '" + cboCashier.Text + "'";
                    cm = new SqlCommand(query, cn);
                }

                //string query = "SELECT * FROM tblExpenses";
                //cm = new SqlCommand(query, cn);
                dr = cm.ExecuteReader();

                while (dr.Read())
                {
                    totalItem += Double.Parse(dr["amount"].ToString());
                }
                dr.Close();
                cn.Close();

                String total = "₱" + totalItem.ToString("#,##0.00");

                lblTotalExpenses.Text = total;

            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void mj_Expenses_Load(object sender, EventArgs e)
        {
            LoadFilterByCategory();
            this.ActiveControl = txtSearch;

            // Set DatePickers to optional state with CheckBox
            dtFrom.ShowCheckBox = true;
            dtTo.ShowCheckBox = true;

            dtFrom.Checked = false;
            dtTo.Checked = false;

            AutoComplete();
            LoadRecords();

            CalculateTotalExpense();

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
            calculateExpenses();
        }

        private void txtFilterByCustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    // Check if the DataGridView2 has any rows
            //    if (dataGridView1.Rows.Count == 0)
            //    {
            //        MessageBox.Show("There are no items to update. Please ensure there is data available.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return;
            //    }

            //    // Check if any row is selected
            //    if (dataGridView1.SelectedRows.Count == 0)
            //    {
            //        MessageBox.Show("No item has been specified.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return;
            //    }
            //    else
            //    {
            //        // Get the selected row index
            //        int selectedRowIndex = dataGridView1.SelectedRows[0].Index;

            //        // Retrieve the data from the selected ro

            //        String customer = dataGridView1.Rows[selectedRowIndex].Cells[2].Value.ToString();
            //        DateTime date = DateTime.Parse(dataGridView1.Rows[selectedRowIndex].Cells[1].Value.ToString());
            //        string expenses = dataGridView1.Rows[selectedRowIndex].Cells[4].Value.ToString();
            //        string amount = dataGridView1.Rows[selectedRowIndex].Cells[5].Value.ToString();
            //        int id = int.Parse(dataGridView1.Rows[selectedRowIndex].Cells[6].Value.ToString());

            //        // Create an instance of the update form and pass the data including the optional fields
            //        mj_updateExpense frm = new mj_updateExpense(this, customer, expenses, amount, id);
            //        frm.LoadCustomer();
            //        frm.ShowDialog();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mj_addExpense frm = new mj_addExpense(this);
            frm.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if the DataGridView2 has any rows
                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("There are no items to update. Please ensure there is data available.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if any row is selected
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("No item has been specified.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    // Get the selected row index
                    int selectedRowIndex = dataGridView1.SelectedRows[0].Index;

                    // Retrieve the data from the selected ro

                    String customer = dataGridView1.Rows[selectedRowIndex].Cells[2].Value.ToString();
                    //DateTime date = DateTime.Parse(dataGridView1.Rows[selectedRowIndex].Cells[1].Value.ToString());
                    string expenses = dataGridView1.Rows[selectedRowIndex].Cells[3].Value.ToString();
                    string amount = dataGridView1.Rows[selectedRowIndex].Cells[4].Value.ToString();
                    int id = int.Parse(dataGridView1.Rows[selectedRowIndex].Cells[5].Value.ToString());
                    string pricePerPc = dataGridView1.Rows[selectedRowIndex].Cells[4].Value.ToString();
                    DateTime expense_date = DateTime.Parse(dataGridView1.Rows[selectedRowIndex].Cells["date"].Value.ToString());

                    // Remove '₱' and '.' from the price and trim '00' if present
                    pricePerPc = pricePerPc.Replace("₱", "")   // Remove currency symbol
                                           .Replace(".", ""); // Remove dot
                    if (pricePerPc.EndsWith("00"))            // Remove trailing '00'
                    {
                        pricePerPc = pricePerPc.Substring(0, pricePerPc.Length - 2);
                    }

                    // Create an instance of the update form and pass the data including the optional fields
                    mj_updateExpense frm = new mj_updateExpense(this, expense_date, customer, expenses, pricePerPc, id);
                    frm.LoadCustomer();
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadRecords();
            CalculateTotalExpense();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            LoadRecords();
            CalculateTotalExpense();
        }

        private void txtFilterByCustomer_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            LoadRecords();
            CalculateTotalExpense();
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

        private void lblTotalExpenses_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            mj_view_all_expenses frm = new mj_view_all_expenses(this);
            frm.LoadRecords();
            frm.CalculateTotalExpense();
            frm.ShowDialog();
        }
    }
    }
