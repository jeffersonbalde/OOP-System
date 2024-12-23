using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_System
{
    public partial class mj_itemPurchase : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;

        public mj_itemPurchase()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            mj_addtemPurchase frm = new mj_addtemPurchase(this);
            frm.LoadCustomer();
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
            //        string query2 = "SELECT * FROM tblItemPurchase";
            //        //string query1 = "SELECT c.transno, c.pcode, p.pdesc, c.price, c.qty, c.disc, SUM(c.total) AS total FROM tblCart as c INNER JOIN tblProduct as p ON c.pcode = p.pcode WHERE status LIKE 'Sold' AND sdate BETWEEN '" + dt1.Value.ToString("yyyy-MM-dd") + "' AND '" + dt2.Value.ToString("yyyy-MM-dd") + "' GROUP BY c.transno ORDER BY total DESC";
            //        cm = new SqlCommand(query2, cn);
            //    }
            //    else
            //    {

            //        string query = "SELECT * FROM tblItemPurchase WHERE customer LIKE '" + txtFilterByCustomer.Text + "'";
            //        //string query = "SELECT c.id, c.transno, c.pcode, p.pdesc, c.price, c.qty, c.disc, c.total FROM tblCart as c INNER JOIN tblProduct as p ON c.pcode = p.pcode WHERE status LIKE 'Sold' AND sdate BETWEEN '" + dt1.Value.ToString("yyyy-MM-dd") + "' AND '" + dt2.Value.ToString("yyyy-MM-dd") + "' AND cashier LIKE '" + cboCashier.Text + "'";
            //        cm = new SqlCommand(query, cn);
            //    }

            //    //cm = new SqlCommand("SELECT * FROM tblExpenses", cn);
            //    dr = cm.ExecuteReader();
            //    while (dr.Read())
            //    {
            //        i++;
            //        // Check if amount and total_cost are null or empty before parsing
            //        string amount = dr["amount"] != DBNull.Value && !string.IsNullOrWhiteSpace(dr["amount"].ToString())
            //            ? "₱" + Double.Parse(dr["amount"].ToString()).ToString("#,##0.00")
            //            : "₱0.00";

            //        string totalcost = dr["total_cost"] != DBNull.Value && !string.IsNullOrWhiteSpace(dr["total_cost"].ToString())
            //            ? "₱" + Double.Parse(dr["total_cost"].ToString()).ToString("#,##0.00")
            //            : "₱0.00";

            //        dataGridView1.Rows.Add(i, DateTime.Parse(dr["date"].ToString()), dr["customer"].ToString(), totalcost, dr["purchase"].ToString(), amount, int.Parse(dr["id"].ToString()));
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

                // Add date filtering logic only if dates are selected
                if (dtFrom.Checked && dtTo.Checked)
                {
                    query += " AND purchase_date >= @start_date AND purchase_date < @end_date";
                }
                else if (dtFrom.Checked) // Only From date is set
                {
                    query += " AND purchase_date >= @start_date";
                }
                else if (dtTo.Checked) // Only To date is set
                {
                    query += " AND purchase_date < @end_date";
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
                    //// Check if amount and total_cost are null or empty before parsing
                    //string amount = dr["amount"] != DBNull.Value && !string.IsNullOrWhiteSpace(dr["amount"].ToString())
                    //    ? "₱" + Double.Parse(dr["amount"].ToString()).ToString("#,##0.00")
                    //    : "₱0.00";

                    //string totalcost = dr["total_cost"] != DBNull.Value && !string.IsNullOrWhiteSpace(dr["total_cost"].ToString())
                    //    ? "₱" + Double.Parse(dr["total_cost"].ToString()).ToString("#,##0.00")
                    //    : "₱0.00";

                    //string formattedDate = dr["date"] != DBNull.Value && DateTime.TryParse(dr["date"].ToString(), out DateTime parsedDate)
                    //    ? parsedDate.ToString("MM/dd/yyyy")
                    //    : "N/A";

                    string purchase_qty = int.Parse(dr["purchase_qty"].ToString()).ToString("N0");

                    string purchase_price = dr["purchase_price"] != DBNull.Value
                    ? "₱" + Double.Parse(dr["purchase_price"].ToString()).ToString("#,##0.00")
                    : "";

                    string total_amount = dr["total_amount"] != DBNull.Value
                    ? "₱" + Double.Parse(dr["total_amount"].ToString()).ToString("#,##0.00")
                    : "";

                    string date = dr["purchase_date"] != DBNull.Value && DateTime.TryParse(dr["purchase_date"].ToString(), out DateTime parsedPaymentDate)
                    ? parsedPaymentDate.ToString("MMMM d, yyyy")
                    : "";

                    dataGridView1.Rows.Add(i, date, dr["purchase_under"].ToString(), dr["purchase_description"].ToString(), purchase_qty, purchase_price, total_amount, dr["id"].ToString());
                }
                dr.Close();
                cn.Close();

                // Show/hide the DataGridView and label based on the number of rows
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
            lblTotalExpenses.Text = "₱" + totalExpense.ToString("#,##0.00");
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

        public void calculateExpenses()
        {
            //Double totalItem = 0;

            //try
            //{
            //    cn.Open();

            //    if (txtFilterByCustomer.Text == "")
            //    {
            //        string query2 = "SELECT * FROM tblItemPurchase";
            //        //string query1 = "SELECT c.transno, c.pcode, p.pdesc, c.price, c.qty, c.disc, SUM(c.total) AS total FROM tblCart as c INNER JOIN tblProduct as p ON c.pcode = p.pcode WHERE status LIKE 'Sold' AND sdate BETWEEN '" + dt1.Value.ToString("yyyy-MM-dd") + "' AND '" + dt2.Value.ToString("yyyy-MM-dd") + "' GROUP BY c.transno ORDER BY total DESC";
            //        cm = new SqlCommand(query2, cn);
            //    }
            //    else
            //    {

            //        string query = "SELECT * FROM tblItemPurchase WHERE customer LIKE '" + txtFilterByCustomer.Text + "'";
            //        //string query = "SELECT c.id, c.transno, c.pcode, p.pdesc, c.price, c.qty, c.disc, c.total FROM tblCart as c INNER JOIN tblProduct as p ON c.pcode = p.pcode WHERE status LIKE 'Sold' AND sdate BETWEEN '" + dt1.Value.ToString("yyyy-MM-dd") + "' AND '" + dt2.Value.ToString("yyyy-MM-dd") + "' AND cashier LIKE '" + cboCashier.Text + "'";
            //        cm = new SqlCommand(query, cn);
            //    }

            //    //string query = "SELECT * FROM tblExpenses";
            //    //cm = new SqlCommand(query, cn);
            //    dr = cm.ExecuteReader();

            //    while (dr.Read())
            //    {
            //        totalItem += Double.Parse(dr["amount"].ToString());
            //    }
            //    dr.Close();
            //    cn.Close();

            //    String total = "₱" + totalItem.ToString("#,##0.00");

            //    lblTotalExpenses.Text = total;

            //}
            //catch (Exception ex)
            //{
            //    cn.Close();
            //    MessageBox.Show(ex.Message);
            //}
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

        private void mj_itemPurchase_Load(object sender, EventArgs e)
        {
            // Set DatePickers to optional state with CheckBox
            dtFrom.ShowCheckBox = true;
            dtTo.ShowCheckBox = true;

            dtFrom.Checked = false;
            dtTo.Checked = false;

            LoadFilterByCategory();
            LoadRecords();
            AutoComplete();
            CalculateTotalExpense();

            this.ActiveControl = txtSearch;
        }

        private void txtFilterByCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRecords();
            calculateExpenses();
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
            //        string purchase = dataGridView1.Rows[selectedRowIndex].Cells[4].Value.ToString();
            //        string amount = dataGridView1.Rows[selectedRowIndex].Cells[5].Value.ToString();
            //        int id = int.Parse(dataGridView1.Rows[selectedRowIndex].Cells[6].Value.ToString());

            //        // Create an instance of the update form and pass the data including the optional fields
            //        mj_updateItemPurchase frm = new mj_updateItemPurchase(this, customer, date, purchase, amount, id);
            //        frm.LoadCustomer();
            //        frm.ShowDialog();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
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
                        calculateExpenses();

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

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            mj_addtemPurchase frm = new mj_addtemPurchase(this);
            frm.ShowDialog();
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
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
                        CalculateTotalExpense();
                        //calculateExpenses();

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

                        int selectedRowIndex = dataGridView1.SelectedRows[0].Index;

                        DateTime purchase_date = DateTime.Parse(dataGridView1.Rows[selectedRowIndex].Cells["purchase_date"].Value.ToString());
                        string purchase_under = dataGridView1.Rows[selectedRowIndex].Cells["purchase_under"].Value.ToString();
                        string purchase_description = dataGridView1.Rows[selectedRowIndex].Cells["purchase_description"].Value.ToString();

                        string price = dataGridView1.Rows[selectedRowIndex].Cells["price"].Value.ToString();

                        // Remove '₱' and '.' from the price and trim '00' if present
                        price = price.Replace("₱", "")   // Remove currency symbol
                                               .Replace(".", ""); // Remove dot
                        if (price.EndsWith("00"))            // Remove trailing '00'
                        {
                            price = price.Substring(0, price.Length - 2);
                        }

                        string qty = dataGridView1.Rows[selectedRowIndex].Cells["qty"].Value.ToString();

                        string total = dataGridView1.Rows[selectedRowIndex].Cells["total"].Value.ToString();

                        //// Remove '₱' and '.' from the price and trim '00' if present
                        //total = total.Replace("₱", "")   // Remove currency symbol
                        //                       .Replace(".", ""); // Remove dot
                        //if (total.EndsWith("00"))            // Remove trailing '00'
                        //{
                        //    total = total.Substring(0, total.Length - 2);
                        //}


                        int id = int.Parse(dataGridView1.Rows[selectedRowIndex].Cells["id"].Value.ToString());

                        mj_updateItemPurchase frm = new mj_updateItemPurchase(this, purchase_date, purchase_under, purchase_description, price, qty, total, id);
                        frm.ShowDialog();
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
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

                    int selectedRowIndex = dataGridView1.SelectedRows[0].Index;

                    DateTime purchase_date = DateTime.Parse(dataGridView1.Rows[selectedRowIndex].Cells["purchase_date"].Value.ToString());
                    string purchase_under = dataGridView1.Rows[selectedRowIndex].Cells["purchase_under"].Value.ToString();
                    string purchase_description = dataGridView1.Rows[selectedRowIndex].Cells["purchase_description"].Value.ToString();

                    string price = dataGridView1.Rows[selectedRowIndex].Cells["price"].Value.ToString();

                    // Remove '₱' and '.' from the price and trim '00' if present
                    price = price.Replace("₱", "")   // Remove currency symbol
                                           .Replace(".", ""); // Remove dot
                    if (price.EndsWith("00"))            // Remove trailing '00'
                    {
                        price = price.Substring(0, price.Length - 2);
                    }

                    string qty = dataGridView1.Rows[selectedRowIndex].Cells["qty"].Value.ToString();

                    string total = dataGridView1.Rows[selectedRowIndex].Cells["total"].Value.ToString();

                    //// Remove '₱' and '.' from the price and trim '00' if present
                    //total = total.Replace("₱", "")   // Remove currency symbol
                    //                       .Replace(".", ""); // Remove dot
                    //if (total.EndsWith("00"))            // Remove trailing '00'
                    //{
                    //    total = total.Substring(0, total.Length - 2);
                    //}


                    int id = int.Parse(dataGridView1.Rows[selectedRowIndex].Cells["id"].Value.ToString());

                    mj_updateItemPurchase frm = new mj_updateItemPurchase(this, purchase_date, purchase_under, purchase_description, price, qty, total, id);
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void txtFilterByCustomer_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            LoadRecords();
            CalculateTotalExpense();
        }

        private void txtFilterByCustomer_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
            CalculateTotalExpense();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mj_view_all_purchase frm = new mj_view_all_purchase(this);
            frm.LoadRecords();
            frm.CalculateTotalExpense();
            frm.ShowDialog();
        }
    }
}
