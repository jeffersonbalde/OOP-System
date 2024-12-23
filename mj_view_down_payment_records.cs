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
    public partial class mj_view_down_payment_records : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;


        public mj_view_down_payment_records()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        private void mj_view_down_payment_records_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtSearch;

            dtFrom.ShowCheckBox = true;
            dtTo.ShowCheckBox = true;

            dtFrom.Checked = false;
            dtTo.Checked = false;

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
            lblTotalOrders.Text = "₱" + totalExpense.ToString("#,##0.00");
        }

        public void LoadRecords()
        {



            try
            {
                int i = 0;
                dataGridView1.Rows.Clear();
                string query = "SELECT * FROM tlbDownPayment WHERE 1=1"; // Base query

                cn.Open();

                // Build the query dynamically
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    query += " AND customer_name LIKE @customer_name";
                }
                //if (dtFrom.Value != null && dtTo.Value != null)
                //{
                //    query += " AND date >= @DateFrom AND date < @DateTo";
                //}

                // Add date filtering logic only if dates are selected
                if (dtFrom.Checked && dtTo.Checked)
                {
                    query += " AND date_paid >= @start_date AND date_paid < @end_date";
                }
                else if (dtFrom.Checked) // Only From date is set
                {
                    query += " AND date_paid >= @start_date";
                }
                else if (dtTo.Checked) // Only To date is set
                {
                    query += " AND date_paid < @end_date";
                }

                query += " ORDER BY date_paid"; // Add order 

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
                    i++;

                    //// Ensure safe retrieval of data from database
                    //string pricePerPc = dr["price_per_pc"] != DBNull.Value
                    //    ? "₱" + Double.Parse(dr["price_per_pc"].ToString()).ToString("#,##0.00")
                    //    : "N/A";

                    string amount_paid = dr["amount_paid"] != DBNull.Value
                        ? "₱" + Double.Parse(dr["amount_paid"].ToString()).ToString("#,##0.00")
                        : "N/A";

                    //string status = dr["status"]?.ToString() ?? "N/A";

                    //string mobileNumber = dr["contact_number"].ToString();

                    // Format dates
                    //string formattedDate = dr["date"] != DBNull.Value && DateTime.TryParse(dr["date"].ToString(), out DateTime parsedDate)
                    //    ? parsedDate.ToString("MM/dd/yyyy")
                    //    : "N/A";

                    string date_paid = dr["date_paid"] != DBNull.Value && DateTime.TryParse(dr["date_paid"].ToString(), out DateTime parsedDateOfPayment)
                        ? parsedDateOfPayment.ToString("MM/dd/yyyy")
                        : " ";

                    //string formattedQty = int.Parse(dr["qty"].ToString()).ToString("N0");

                    // Add data to the DataGridView
                    int rowIndex = dataGridView1.Rows.Add(
                        i,
                        dr["customer_name"].ToString(),
                        date_paid,
                        amount_paid,
                        dr["remarks"]?.ToString() ?? "",
                        dr["id"].ToString()
                    );

                    // Change row color if status is 'Voided'
                    //if (status.Equals("Voided", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightPink;
                    //    dataGridView1.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.DarkRed;
                    //}
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
            CalculateTotalAmountPaid();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadRecords();
            CalculateTotalAmountPaid();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            LoadRecords();
            CalculateTotalAmountPaid();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            string status = "";

            if(colName == "Update")
            {
                try
                {
                    // Check if the DataGridView2 has any rows

                    int selectedRowIndex = dataGridView1.SelectedRows[0].Index;
                    string id = dataGridView1.Rows[selectedRowIndex].Cells["id"].Value.ToString();

                    cn.Open();

                    string query = @"SELECT * FROM tlbDownPayment WHERE id LIKE @id";
                    cm = new SqlCommand(query, cn);
                    cm.Parameters.AddWithValue("@id", int.Parse(id));
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        status = dr["status"].ToString();
                    }
                    dr.Close();
                    cn.Close();

                    if(status == "Fully Paid")
                    {
                        MessageBox.Show("This record cannot be updated because it is fully paid.",
                                        "Action Not Allowed",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);    
                        return;
                    }

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

                        // Retrieve the data from the selected ro

                        String customer = dataGridView1.Rows[selectedRowIndex].Cells["customer_name"].Value.ToString();
                        //DateTime date = DateTime.Parse(dataGridView1.Rows[selectedRowIndex].Cells[1].Value.ToString());
                        DateTime date_paid = DateTime.Parse(dataGridView1.Rows[selectedRowIndex].Cells["date_paid"].Value.ToString());

                        string amount_paid = dataGridView1.Rows[selectedRowIndex].Cells["amount_paid"].Value.ToString();
                        string remarks = dataGridView1.Rows[selectedRowIndex].Cells["remarks"].Value.ToString();
                        

                        // Remove '₱' and '.' from the price and trim '00' if present
                        amount_paid = amount_paid.Replace("₱", "")   // Remove currency symbol
                                               .Replace(".", ""); // Remove dot
                        if (amount_paid.EndsWith("00"))            // Remove trailing '00'
                        {
                            amount_paid = amount_paid.Substring(0, amount_paid.Length - 2);
                        }

                        // Create an instance of the update form and pass the data including the optional fields
                        mj_updateDownPayment2 frm = new mj_updateDownPayment2(this, customer, date_paid, amount_paid, remarks, id);
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
    }
}
