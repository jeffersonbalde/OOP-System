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
    public partial class mj_downPayment : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;

        public mj_downPayment()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        private void mj_downPayment_Load(object sender, EventArgs e)
        {
            cboStatus.Items.AddRange(new string[] { "All", "Pending", "Fully Paid"});
            cboStatus.SelectedIndex = 0; // Default to "All"

            this.ActiveControl = txtSearch;


            //dataGridView1.ClearSelection();
            LoadRecords();
            CalculateTotalAmountPaid();
        }

        public void CalculateTotalAmountPaid()
        {
            decimal totalExpense = 0;

            try
            {
                // Iterate through each row in the DataGridView
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["amount_paid"].Value != null)
                    {
                        string amountString = row.Cells["amount_paid"].Value.ToString();

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

        public void LoadDownPaymentRecords()
        {
            //try
            //{
            //    int i = 0;
            //    dataGridView2.Rows.Clear();

            //    cn.Open();
            //    // Update the SQL query to fetch records where type__of__purchased or date_of_payment is NULL
            //    cm = new SqlCommand("SELECT * FROM tlbDownPayment WHERE customer LIKE '" + txtSearch.Text + "%' ORDER BY customer", cn);
            //    dr = cm.ExecuteReader();

            //    while (dr.Read())
            //    {
            //        i++;
            //        string totalCost = "₱" + Double.Parse(dr["total_cost"].ToString()).ToString("#,##0.00");
            //        string amount = "₱" + Double.Parse(dr["amount"].ToString()).ToString("#,##0.00");
            //        int id = int.Parse(dr["id"].ToString());

            //        dataGridView2.Rows.Add(i, dr["customer"].ToString(), totalCost, DateTime.Parse(dr["date_of_down_payment"].ToString()), dr["mode_of_payment"].ToString(), amount, id);
            //    }

            //    dr.Close();
            //    cn.Close();
            //}
            //catch (Exception ex)
            //{
            //    cn.Close();
            //    MessageBox.Show(ex.Message);
            //}
        }

        //public void LoadRecords()
        //{
        //    try
        //    {
        //        int i = 0;
        //        dataGridView1.Rows.Clear();
        //        dataGridView1.ClearSelection();

        //        cn.Open();
        //        // Update the SQL query to fetch records where type__of__purchased or date_of_payment is NULL
        //        cm = new SqlCommand("SELECT * FROM tblPurchased WHERE ISNULL(type_of_purchased, '') = '' OR ISNULL(date_of_payment, '') = ''", cn);
        //        dr = cm.ExecuteReader();

        //        while (dr.Read())
        //        {
        //            i++;
        //            string totalCost = "₱" + Double.Parse(dr["total_cost"].ToString()).ToString("#,##0.00");
        //            string balance = "₱" + Double.Parse(dr["balance"].ToString()).ToString("#,##0.00");

        //            dataGridView1.Rows.Add(i, dr["client"].ToString(), totalCost, balance);
        //        }

        //        dataGridView1.ClearSelection();

        //        dr.Close();
        //        cn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        cn.Close();
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //string colName = dataGridView2.Columns[e.ColumnIndex].Name;

            //int purchased_id = 0;
            //String status = "";

            //if (colName == "delete")
            //{
            //    // Show a confirmation prompt before deletion
            //    DialogResult result = MessageBox.Show("Are you sure you want to permanently delete this item? This action cannot be undone.",
            //                                          "Confirm Deletion",
            //                                          MessageBoxButtons.YesNo,
            //                                          MessageBoxIcon.Warning);

            //    if (result == DialogResult.Yes)
            //    {
            //        try
            //        {
            //            int selectedRowIndex = dataGridView2.SelectedRows[0].Index;
            //            int id = int.Parse(dataGridView2.Rows[selectedRowIndex].Cells[6].Value.ToString());

            //            cn.Open();
            //            string query4 = "SELECT * FROM tlbDownPayment WHERE id = @id";
            //            cm = new SqlCommand(query4, cn);
            //            cm.Parameters.AddWithValue("@id", id); // Add the ID parameter
            //            dr = cm.ExecuteReader();
            //            while (dr.Read())
            //            {
            //                purchased_id = int.Parse(dr["purchased_id"].ToString());
            //            }
            //            dr.Close();
            //            cn.Close();

            //            cn.Open();
            //            string query5 = "SELECT * FROM tblPurchased WHERE id = @id";
            //            cm = new SqlCommand(query5, cn);
            //            cm.Parameters.AddWithValue("@id", purchased_id); // Add the ID parameter
            //            dr = cm.ExecuteReader();
            //            while (dr.Read())
            //            {
            //                status = dr["status"].ToString();
            //            }
            //            dr.Close();
            //            cn.Close();

            //            if(status == "PAID")
            //            {
            //                cn.Open();
            //                string query = "DELETE FROM tlbDownPayment WHERE id LIKE '" + dataGridView2.Rows[e.RowIndex].Cells[6].Value.ToString() + "'";
            //                cm = new SqlCommand(query, cn);
            //                cm.ExecuteNonQuery();
            //                cn.Close();

            //                // Show success message after deletion
            //                MessageBox.Show("The item has been successfully deleted from the system.",
            //                                "Item Deleted",
            //                                MessageBoxButtons.OK,
            //                                MessageBoxIcon.Information);
            //                LoadDownPaymentRecords();
            //            }else
            //            {
            //                MessageBox.Show("The item cannot be deleted because it is still in the down payment process. Complete the payment first before attempting to delete.",
            //                                "Deletion Not Allowed",
            //                                MessageBoxButtons.OK,
            //                                MessageBoxIcon.Warning);
            //            }
            //    }
            //        catch (Exception ex)
            //        {
            //        cn.Close();
            //        MessageBox.Show("An error occurred while trying to delete the item. Please try again.",
            //                        "Error",
            //                        MessageBoxButtons.OK,
            //                        MessageBoxIcon.Error);
            //    }
            //}
            //}
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            mj_addDownPayment frm = new mj_addDownPayment(this);
            //frm.LoadRecords();
            frm.ShowDialog();
        }

        private void btnCompleteItem_Click(object sender, EventArgs e)
        {   
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //// Set the selection mode to FullRowSelect or CellSelect (depending on your needs)
            //dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //// Disable the selection by handling the CellEnter event
            //dataGridView1.CellEnter += (s, args) =>
            //{
            //    dataGridView1.ClearSelection(); // Clears any selection when the user tries to select a cell or row
            //};

            //// Optional: Disable multi-selection (if needed)
            //dataGridView1.MultiSelect = false;

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //String check_status = "";
            //int p_id = 0;

            //try
            //{
            //    // Check if the DataGridView2 has any rows
            //    if (dataGridView2.Rows.Count == 0)
            //    {
            //        MessageBox.Show("There are no items to update. Please ensure there is data available.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return;
            //    }

            //    // Check if any row is selected
            //    if (dataGridView2.SelectedRows.Count == 0)
            //    {
            //        MessageBox.Show("No item has been specified.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return;
            //    }

            //    int selectedRowIndex = dataGridView2.SelectedRows[0].Index;
            //    int id = int.Parse(dataGridView2.Rows[selectedRowIndex].Cells[6].Value.ToString());

            //    // Retrieve down payments and compute total paid amount
            //    cn.Open();
            //    string query4 = "SELECT * FROM tlbDownPayment WHERE id = @id";
            //    cm = new SqlCommand(query4, cn);
            //    cm.Parameters.AddWithValue("@id", id); // Add the ID parameter
            //    dr = cm.ExecuteReader();
            //    while (dr.Read())
            //    {
            //        p_id = int.Parse(dr["purchased_id"].ToString()); // Sum up down payments
            //    }
            //    dr.Close();
            //    cn.Close();


            //    // Retrieve down payments and compute total paid amount
            //    cn.Open();
            //    string query5 = "SELECT * FROM tblPurchased WHERE id = @id";
            //    cm = new SqlCommand(query5, cn);
            //    cm.Parameters.AddWithValue("@id", p_id); // Add the ID parameter
            //    dr = cm.ExecuteReader();
            //    while (dr.Read())
            //    {
            //        check_status = dr["status"].ToString();
            //    }
            //    dr.Close();
            //    cn.Close();

            //    if (check_status == "PAID")
            //    {
            //        MessageBox.Show("This item has already been fully paid. No further updates are allowed for completed transactions.",
            //    "Update Restricted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }   


            //    // Check if any row is selected                 
            //    if (dataGridView2.SelectedRows.Count == 0)
            //    {
            //        MessageBox.Show("No item has been specified.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return;
            //    }
            //    else
            //    {
            //        // Get the selected row index

            //        // Retrieve the data from the selected row
            //        string customer = dataGridView2.Rows[selectedRowIndex].Cells[1].Value.ToString();
            //        String total_cost = dataGridView2.Rows[selectedRowIndex].Cells[2].Value.ToString();
            //        DateTime date = DateTime.Parse(dataGridView2.Rows[selectedRowIndex].Cells[3].Value.ToString());
            //        string mop = dataGridView2.Rows[selectedRowIndex].Cells[4].Value.ToString();
            //        double amount = double.Parse(dataGridView2.Rows[selectedRowIndex].Cells[5].Value.ToString().Replace("₱", "").Trim());
            //        int idd = int.Parse(dataGridView2.Rows[selectedRowIndex].Cells[6].Value.ToString());

            //        // Create an instance of the update form and pass the data including the optional fields
            //        mj_updateDownPayment frm = new mj_updateDownPayment(this, customer, total_cost, date, mop, amount, idd);

            //        frm.ShowDialog();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //LoadDownPaymentRecords();
            LoadRecords();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mj_addDownPayment frm = new mj_addDownPayment(this);
            //frm.LoadRecords();
            frm.ShowDialog();
        }

        public void LoadRecords()
        {
            try
            {
                int i = 0;
                dataGridView1.Rows.Clear();

                // Fetch grouped down payment data
                DataTable downPaymentData = GetGroupedDownPaymentData();

                foreach (DataRow row in downPaymentData.Rows)
                {
                    string customerName = row["customer_name"].ToString();
                    string totalAmountPaid = "₱" + Convert.ToDouble(row["total_amount_paid"]).ToString("#,##0.00");

                    // Fetch the balance for the current customer
                    double balance = GetCustomerBalance(customerName);
                    string formattedBalance = "₱" + balance.ToString("#,##0.00");

                    string datePaid = row["latest_date_paid"] != DBNull.Value
                        ? Convert.ToDateTime(row["latest_date_paid"]).ToString("MMMM d, yyyy")
                        : "N/A";
                    string remarks = row["combined_remarks"] != DBNull.Value ? row["combined_remarks"].ToString() : "N/A";
                    string status = row["latest_status"] != DBNull.Value ? row["latest_status"].ToString() : "N/A";

                    // Add row to DataGridView
                    dataGridView1.Rows.Add(++i, customerName, totalAmountPaid, formattedBalance, status);
                }

                lblNoLowStocks.Visible = dataGridView1.Rows.Count == 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading records: " + ex.Message);
            }
        }



        public DataTable GetGroupedDownPaymentData()
        {
            DataTable dt = new DataTable();
            try
            {
                string query = @"
            SELECT 
                dp.customer_name,
                ISNULL(SUM(dp.amount_paid), 0) AS total_amount_paid,
                MAX(dp.date_paid) AS latest_date_paid,
                STRING_AGG(dp.remarks, ', ') AS combined_remarks,
                MAX(dp.status) AS latest_status
            FROM 
                tlbDownPayment dp
            WHERE 
                1=1";

                // Add filters dynamically
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    query += " AND dp.customer_name LIKE @customer_name";
                }
                if (cboStatus.SelectedIndex != -1 && cboStatus.Text != "All")
                {
                    query += " AND dp.status LIKE @status";
                }
                //if (dtFrom.Checked && dtTo.Checked)
                //{
                //    query += " AND dp.date_paid >= @start_date AND dp.date_paid < @end_date";
                //}

                query += @"
            GROUP BY 
                dp.customer_name
            ORDER BY 
                MAX(dp.date_paid) DESC";

                cn.Open();
                cm = new SqlCommand(query, cn);

                // Add parameters for filtering
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    cm.Parameters.AddWithValue("@customer_name", "%" + txtSearch.Text.Trim() + "%");
                }
                if (cboStatus.SelectedIndex != -1 && cboStatus.Text != "All")
                {
                    cm.Parameters.AddWithValue("@status", "%" + cboStatus.SelectedItem.ToString() + "%");
                }
                //if (dtFrom.Checked)
                //{
                //    cm.Parameters.AddWithValue("@start_date", dtFrom.Value.Date);
                //}
                //if (dtTo.Checked)
                //{
                //    cm.Parameters.AddWithValue("@end_date", dtTo.Value.Date.AddDays(1));
                //}

                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(dt);
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show("Error fetching grouped down payment data: " + ex.Message);
            }
            return dt;
        }

        public double GetCustomerBalance(string customerName)
        {
            double balance = 0;
            try
            {
                string query = "SELECT ISNULL(balance, 0) AS balance FROM tblPurchased WHERE client = @customer_name";

                cn.Open();
                cm = new SqlCommand(query, cn);
                cm.Parameters.AddWithValue("@customer_name", customerName);
                object result = cm.ExecuteScalar();
                balance = result != null ? Convert.ToDouble(result) : 0;
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show("Error fetching customer balance: " + ex.Message);
            }
            return balance;
        }

        private void cboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRecords();
            CalculateTotalAmountPaid();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            mj_view_down_payment_records frm = new mj_view_down_payment_records();
            frm.LoadRecords();
            frm.CalculateTotalAmountPaid();
            frm.ShowDialog();
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
