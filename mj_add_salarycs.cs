using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_System
{
    public partial class mj_add_salarycs : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;

        private bool isLoading = true; // Flag to skip validations during load

        mj_salary  mj_sa;


        public mj_add_salarycs(mj_salary frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            mj_sa = frm;
        }

        private void mj_add_salarycs_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtName;

            cboSalaryRateType.Items.AddRange(new string[] { "", "Monthly", "Weekly", "Daily", "Hourly" });
            cboSalaryRateType.SelectedIndex = 0; // Default to "All"

            // Set default dates
            dtStartDate.Value = DateTime.Now; // Default to today
            dtEndDate.Value = DateTime.Now.AddDays(1); // Default to tomorrow

            isLoading = false; // Form load complete, enable validations
        }

        private void txtSalaryRateAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits, one '.' character, and backspace
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8) // Allow digits and backspace
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true; // Reject all other characters
            }
        }

        private void txtSalaryRateAmount_TextChanged(object sender, EventArgs e)
        {
            UpdateSalary();

            txtSalaryRateAmount.TextChanged -= txtSalaryRateAmount_TextChanged;

            try
            {
                if (!string.IsNullOrWhiteSpace(txtSalaryRateAmount.Text))
                {
                    string rawText = txtSalaryRateAmount.Text.Replace(",", "");

                    // Skip formatting if the last character is a dot (allow intermediate input)
                    if (rawText.EndsWith("."))
                    {
                        txtSalaryRateAmount.TextChanged += txtSalaryRateAmount_TextChanged;
                        return;
                    }

                    // Parse the input as a double if valid (without commas)
                    double number;
                    if (double.TryParse(rawText, out number))
                    {
                        // Format the number with commas and preserve decimals
                        txtSalaryRateAmount.Text = number.ToString("#,##0.###");

                        // Preserve the cursor position at the end
                        txtSalaryRateAmount.SelectionStart = txtSalaryRateAmount.Text.Length;
                    }
                }
            }
            catch (Exception ex)
            {
                txtSalaryRateAmount.Text = "";
            }

            txtSalaryRateAmount.TextChanged += txtSalaryRateAmount_TextChanged;
        }

        private void txtDaysHoursWorked_TextChanged(object sender, EventArgs e)
        {
            UpdateSalary();

            txtDaysHoursWorked.TextChanged -= txtDaysHoursWorked_TextChanged;

            try
            {
                if (!string.IsNullOrWhiteSpace(txtDaysHoursWorked.Text))
                {
                    string rawText = txtDaysHoursWorked.Text.Replace(",", "");

                    // Skip formatting if the last character is a dot (allow intermediate input)
                    if (rawText.EndsWith("."))
                    {
                        txtDaysHoursWorked.TextChanged += txtDaysHoursWorked_TextChanged;
                        return;
                    }

                    // Parse the input as a double if valid (without commas)
                    double number;
                    if (double.TryParse(rawText, out number))
                    {
                        // Format the number with commas and preserve decimals
                        txtDaysHoursWorked.Text = number.ToString("#,##0.###");

                        // Preserve the cursor position at the end
                        txtDaysHoursWorked.SelectionStart = txtDaysHoursWorked.Text.Length;
                    }
                }
            }
            catch (Exception ex)
            {
                txtDaysHoursWorked.Text = "";
            }

            txtDaysHoursWorked.TextChanged += txtDaysHoursWorked_TextChanged;
        }

        private void txtDaysHoursWorked_KeyPress(object sender, KeyPressEventArgs e)
        {

            // Allow digits, one '.' character, and backspace
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8) // Allow digits and backspace
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true; // Reject all other characters
            }
        }

        private void UpdateSalary()
        {
            if (isLoading) return; // Skip calculation during form load

            try
            {

                if (string.IsNullOrWhiteSpace(txtSalaryRateAmount.Text))
                {
                    {
                        lblTotalSalary.Text = "₱00.00";
                    }
                }


                // Get values from UI
                string salaryRateType = cboSalaryRateType.SelectedItem?.ToString();
                decimal salaryRateAmount = string.IsNullOrWhiteSpace(txtSalaryRateAmount.Text) ? 0 : Convert.ToDecimal(txtSalaryRateAmount.Text);
                DateTime startDate = dtStartDate.Value;
                DateTime endDate = dtEndDate.Value;

                // Variables for calculation
                decimal totalSalary = 0;
                int totalDays = (int)(endDate - startDate).TotalDays + 1; // Include end date

                if (endDate < startDate)
                {
                    MessageBox.Show("End date cannot be earlier than start date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dtEndDate.Value = startDate; // Reset End Date to match Start Date
                    return;
                }

                if (salaryRateType == "Monthly")
                {
                    lblReadOnly.Visible = true;
                    lblWeekly.Visible = false;
                }else if (salaryRateType == "Weekly")
                {
                    lblWeekly.Visible = true;
                    lblReadOnly.Visible = false;
                } else
                {
                    lblWeekly.Visible = false;
                    lblReadOnly.Visible = false;
                }

                if (salaryRateType == "Monthly")
                {
                    // Disable Days/Hours Worked field
                    txtDaysHoursWorked.ReadOnly = true;

                    // Compute total salary (monthly rate remains the same regardless of days worked)
                    totalSalary = salaryRateAmount;
                    txtDaysHoursWorked.Text = totalDays.ToString(); // Display total days
                    lblTotalSalary.Text = totalSalary.ToString("₱0,0.00");
                }
                else if (salaryRateType == "Weekly")
                {
                    txtDaysHoursWorked.ReadOnly = true;

                    // Calculate weeks in the range (7 days = 1 week)
                    int totalWeeks = (int)Math.Ceiling(totalDays / 7.0);
                    totalSalary = salaryRateAmount * totalWeeks;

                    txtDaysHoursWorked.Text = totalWeeks.ToString(); // Display total weeks
                }
                else if (salaryRateType == "Daily")
                {
                    txtDaysHoursWorked.ReadOnly = false;

                    string daysWorkedText = txtDaysHoursWorked.Text.Replace(",", "");

                    // Parse the value as long to avoid overflow issues
                    long daysWorked = string.IsNullOrWhiteSpace(daysWorkedText) ? 0 : long.Parse(daysWorkedText);

                    // Check if the value exceeds the Int32 range
                    if (daysWorked > int.MaxValue)
                    {
                        MessageBox.Show("The value for Days Worked is too large. Please enter a smaller number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtDaysHoursWorked.Text = "0";
                        return;
                    }

                    // Convert to int after validation
                    int daysWorkedInt = (int)daysWorked;

                    totalSalary = salaryRateAmount * daysWorkedInt;
                }
                else if (salaryRateType == "Hourly")
                {
                    txtDaysHoursWorked.ReadOnly = false;

                    // Remove commas before parsing
                    string daysWorkedText = txtDaysHoursWorked.Text.Replace(",", "");

                    // Parse the value as long to avoid overflow issues
                    long daysWorked = string.IsNullOrWhiteSpace(daysWorkedText) ? 0 : long.Parse(daysWorkedText);

                    // Check if the value exceeds the Int32 range
                    if (daysWorked > int.MaxValue)
                    {
                        MessageBox.Show("The value for Hours Worked is too large. Please enter a smaller number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtDaysHoursWorked.Text = "0";
                        return;
                    }

                    // Convert to int after validation
                    int daysWorkedInt = (int)daysWorked;

                    totalSalary = salaryRateAmount * daysWorkedInt;
                }

                // Update Total Salary field
                lblTotalSalary.Text = totalSalary.ToString("₱0,0.00");
        }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in calculation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
}

        private void cboSalaryRateType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSalary();
        }

        private void dtStartDate_ValueChanged(object sender, EventArgs e)
        {
            UpdateSalary();
        }

        private void dtEndDate_ValueChanged(object sender, EventArgs e)
        {
            UpdateSalary();
        }

        private void cboSalaryRateType_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtSalaryRateAmount.Text))
            {
                {
                    MessageBox.Show("Please fill up Salary Rate Amount first.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPosition.Text) || string.IsNullOrWhiteSpace(txtSalaryRateAmount.Text) || string.IsNullOrWhiteSpace(cboSalaryRateType.Text) || string.IsNullOrWhiteSpace(txtDaysHoursWorked.Text))
                {
                    MessageBox.Show("Please fill up all fields", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Parse price (if required)
                double salary_amount = double.Parse(txtSalaryRateAmount.Text.Replace(",", ""), CultureInfo.InvariantCulture);

                if (salary_amount <= 0)
                {
                    MessageBox.Show("Salary Rate Amount cannot be 0 or negative.", "Invalid Salary Rate Amount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                double daysHoursWorked = double.Parse(txtDaysHoursWorked.Text.Replace(",", ""), CultureInfo.InvariantCulture);

                if (daysHoursWorked <= 0)
                {
                    MessageBox.Show("Days/Hours Worked cannot be 0 or negative.", "Days/Hours Worked cannot be 0 or negative.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Do you want to confirm saving this item?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    cn.Open();
                    string query2 = "INSERT INTO tblSalary (name, position, salary_rate, salary_rate_type, days_hours_worked, start_date, end_date, total_salary) VALUES(@name, @position, @salary_rate, @salary_rate_type, @days_hours_worked, @start_date, @end_date, @total_salary)";
                    cm = new SqlCommand(query2, cn);
                    cm.Parameters.AddWithValue("@name", txtName.Text);
                    cm.Parameters.AddWithValue("@position", txtPosition.Text);
                    cm.Parameters.AddWithValue("@salary_rate", Double.Parse(txtSalaryRateAmount.Text.Replace(",", "").Trim()));
                    cm.Parameters.AddWithValue("@salary_rate_type", cboSalaryRateType.Text);
                    cm.Parameters.AddWithValue("@days_hours_worked", int.Parse(txtDaysHoursWorked.Text.Replace(",", "").Trim()));
                    cm.Parameters.AddWithValue("@start_date", dtStartDate.Value);
                    cm.Parameters.AddWithValue("@end_date", dtEndDate.Value);
                    cm.Parameters.AddWithValue("@total_salary", Double.Parse(lblTotalSalary.Text.Replace("₱", "").Trim()));
                    cm.ExecuteNonQuery();
                    cn.Close();

                    txtName.Clear();
                    txtPosition.Clear();
                    txtSalaryRateAmount.Clear();
                    cboSalaryRateType.SelectedIndex = 0; // Default to "All"
                    txtDaysHoursWorked.Clear();
                    lblTotalSalary.Text = "₱00.00";
                    txtName.Focus();

                    mj_sa.LoadRecords();
                    mj_sa.CalculateTotalExpense();


                    MessageBox.Show("Salary has been successfully saved!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }
    }
}
