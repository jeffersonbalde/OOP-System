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
    public partial class mj_salary : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;

        public mj_salary()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        private void mj_salary_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtSearch;
            cboStatus.Items.AddRange(new string[] { "All", "Paid", "Unpaid" });
            cboStatus.SelectedIndex = 0; // Default to "All"

            // Set DatePickers to optional state with CheckBox
            dtFrom.ShowCheckBox = true;
            dtTo.ShowCheckBox = true;

            dtFrom.Checked = false;
            dtTo.Checked = false;

            LoadRecords();
            CalculateTotalExpense();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mj_add_salarycs frm = new mj_add_salarycs(this);
            frm.ShowDialog();
        }

        public void LoadRecords()
        {
            try
            {
                int i = 0;
                dataGridView1.Rows.Clear();
                string query = "SELECT * FROM tblSalary WHERE 1=1"; // Base query

                cn.Open();

                // Build the query dynamically
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    query += " AND name LIKE @name";
                }
                if (cboStatus.SelectedIndex != -1 && cboStatus.Text != "All")
                {
                    query += " AND status LIKE @status";
                }


                // Add date filtering logic only if dates are selected
                if (dtFrom.Checked && dtTo.Checked)
                {
                    query += " AND start_date >= @start_date AND end_date < @end_date";
                }
                else if (dtFrom.Checked) // Only From date is set
                {
                    query += " AND start_date >= @start_date";
                }
                else if (dtTo.Checked) // Only To date is set
                {
                    query += " AND end_date < @end_date";
                }

                query += " ORDER BY start_date"; // Add order 

                cm = new SqlCommand(query, cn);

                // Add parameters to prevent SQL injection
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    cm.Parameters.AddWithValue("@name", "%" + txtSearch.Text + "%");
                }
                if (cboStatus.SelectedIndex != -1 && cboStatus.Text != "All")
                {
                    cm.Parameters.AddWithValue("@status", cboStatus.SelectedItem.ToString() + "%");
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


                    string salary_rate = dr["salary_rate"] != DBNull.Value
                        ? "₱" + Double.Parse(dr["salary_rate"].ToString()).ToString("#,##0.00")
                        : "";

                    string total_salary = dr["total_salary"] != DBNull.Value
                        ? "₱" + Double.Parse(dr["total_salary"].ToString()).ToString("#,##0.00")
                        : "";


                    // Format start_date
                    string start_date = dr["start_date"] != DBNull.Value && DateTime.TryParse(dr["start_date"].ToString(), out DateTime parsedStartDate)
                        ? parsedStartDate.ToString("MMMM d, yyyy")
                        : "";

                    // Format end_date
                    string end_date = dr["end_date"] != DBNull.Value && DateTime.TryParse(dr["end_date"].ToString(), out DateTime parsedEndDate)
                        ? parsedEndDate.ToString("MMMM d, yyyy")
                        : "";

                    // Format payment_date
                    string payment_date = dr["payment_date"] != DBNull.Value && DateTime.TryParse(dr["payment_date"].ToString(), out DateTime parsedPaymentDate)
                        ? parsedPaymentDate.ToString("MMMM d, yyyy")
                        : "";

                    string days_hours_worked = int.Parse(dr["days_hours_worked"].ToString()).ToString("N0");

                    // Add data to the DataGridView
                    int rowIndex = dataGridView1.Rows.Add(
                        i,
                        dr["name"].ToString(),
                        dr["position"].ToString(),
                        salary_rate,
                        dr["salary_rate_type"].ToString(),
                        days_hours_worked,
                        start_date,
                        end_date,
                        total_salary,
                        dr["status"].ToString(),
                        payment_date,
                        dr["id"].ToString()
                    );
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
        }

        private void lblTotalExpenses_Click(object sender, EventArgs e)
        {

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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (colName == "delete")
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
                        string query = "DELETE FROM tblSalary WHERE id LIKE '" + dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString() + "'";
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
        }

        public void CalculateTotalExpense()
        {
            decimal totalExpense = 0;

            try
            {
                // Iterate through each row in the DataGridView
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["total_salary"].Value != null)
                    {
                        string amountString = row.Cells["total_salary"].Value.ToString();

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

        private void cboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRecords();
            CalculateTotalExpense();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            // Check if the DataGridView has any rows
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No salary records available to process.", "No Data Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if any row is selected
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a salary record to proceed.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            int selectedRowIndex = dataGridView1.SelectedRows[0].Index;

            string currentStatus = dataGridView1.Rows[selectedRowIndex].Cells["status"].Value.ToString();
            int id = int.Parse(dataGridView1.Rows[selectedRowIndex].Cells["id"].Value.ToString());

            if (currentStatus == "Paid")
            {
                // Show a message if the item is already voided
                MessageBox.Show("This salary record is already paid.", "Data Already Paid",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Do you want to confirm payment for this salary?", "Confirm Payment", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {

                    // Update the selected item in the database
                    cn.Open();
                    string query = "UPDATE tblSalary SET payment_date = @payment_date, status = @status WHERE id = @id";
                    cm = new SqlCommand(query, cn);

                    // Pass the new values from the form controls
                    cm.Parameters.AddWithValue("@payment_date", DateTime.Now);
                    cm.Parameters.AddWithValue("@status", "Paid");
                    cm.Parameters.AddWithValue("@id", id);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    LoadRecords();
                    CalculateTotalExpense();

                    // Notify user of success
                    MessageBox.Show("Salary has been successfully paid!", "Paid Salary", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if the DataGridView2 has any rows
                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("There are no salary records to update. Please ensure there is data available.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if any row is selected
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("No salary record has been specified.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    // Get the selected row index
                    int selectedRowIndex = dataGridView1.SelectedRows[0].Index;

                    // Retrieve the data from the selected ro

                    string name = dataGridView1.Rows[selectedRowIndex].Cells["name"].Value.ToString();
                    string position = dataGridView1.Rows[selectedRowIndex].Cells["position"].Value.ToString();

                    string salary_rate = dataGridView1.Rows[selectedRowIndex].Cells["salary_rate"].Value.ToString();

                    // Remove '₱' and '.' from the price and trim '00' if present
                    salary_rate = salary_rate.Replace("₱", "")   // Remove currency symbol
                                           .Replace(".", ""); // Remove dot
                    if (salary_rate.EndsWith("00"))            // Remove trailing '00'
                    {
                        salary_rate = salary_rate.Substring(0, salary_rate.Length - 2);
                    }

                    string salary_rate_type = dataGridView1.Rows[selectedRowIndex].Cells["salary_rate_type"].Value.ToString(); 
                    string days_hours_worked = dataGridView1.Rows[selectedRowIndex].Cells["days_hours_worked"].Value.ToString();

                    DateTime start_date = DateTime.Parse(dataGridView1.Rows[selectedRowIndex].Cells["start_date"].Value.ToString());
                    DateTime end_date = DateTime.Parse(dataGridView1.Rows[selectedRowIndex].Cells["end_date"].Value.ToString());

                    string total_salary = dataGridView1.Rows[selectedRowIndex].Cells["total_salary"].Value.ToString();

                    // Remove '₱' and '.' from the price and trim '00' if present
                    total_salary = total_salary.Replace("₱", "")   // Remove currency symbol
                                           .Replace(".", ""); // Remove dot
                    if (total_salary.EndsWith("00"))            // Remove trailing '00'
                    {
                        total_salary = total_salary.Substring(0, total_salary.Length - 2);
                    }


                    int id = int.Parse(dataGridView1.Rows[selectedRowIndex].Cells["id"].Value.ToString());

                    // Create an instance of the update form and pass the data including the optional fields
                    mj_update_salary frm = new mj_update_salary(this, name, position, salary_rate, salary_rate_type, days_hours_worked, start_date, end_date, total_salary, id);
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lblNoLowStocks_Click(object sender, EventArgs e)
        {

        }
    }
}
