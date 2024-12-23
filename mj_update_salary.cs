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
    public partial class mj_update_salary : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;

        private bool isLoading = true; // Flag to skip validations during load

        mj_salary mj_sa;

        public mj_update_salary(mj_salary frm, string name, string position, string salary_rate, string salary_rate_type, string days_hours_worked, DateTime start_date, DateTime end_date, string total_salary, int id)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            mj_sa = frm;

            Decimal total_salary2 = Decimal.Parse(total_salary);

            txtName.Text = name;
            txtPosition.Text = position;
            txtSalaryRateAmount.Text = salary_rate;
            cboSalaryRateType.Text = salary_rate_type;
            txtDaysHoursWorked.Text = days_hours_worked;
            dtStartDate.Value = start_date;
            dtEndDate.Value = end_date;
            lblTotalSalary.Text = total_salary2.ToString("₱0,0.00");
            txtID.Text = id.ToString();

        }

        private void UpdateSalary()
        {

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
                }
                else if (salaryRateType == "Weekly")
                {
                    lblWeekly.Visible = true;
                    lblReadOnly.Visible = false;
                }
                else
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

        private void mj_update_salary_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtName;

            cboSalaryRateType.Items.AddRange(new string[] { "", "Monthly", "Weekly", "Daily", "Hourly" });
            //cboSalaryRateType.SelectedIndex = 0; // Default to "All"

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
            if (string.IsNullOrWhiteSpace(txtSalaryRateAmount.Text))
            {
                {
                    MessageBox.Show("Please fill up Salary Rate Amount first.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
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

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
