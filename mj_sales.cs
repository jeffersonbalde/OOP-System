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
    public partial class mj_sales : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;

        public mj_sales()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        private void mj_sales_Load(object sender, EventArgs e)
        {
            // Set DatePickers to optional state with CheckBox
            this.ActiveControl = txtSearch;
            dtFrom.ShowCheckBox = true;
            dtTo.ShowCheckBox = true;

            dtFrom.Checked = false;
            dtTo.Checked = false;

            LoadRecords();
            CalculateTotalGrossSales();
            CalculateTotalExpense();
            CalculateTotalPurchase();
            CalculateTotalNetIncome();
            CalculateTotalTotalProfit();
        }


        public void LoadRecords()
        {
            try
            {
                int i = 0;
                dataGridView1.Rows.Clear();

                // Base query without date filtering
                string query = @"
WITH ExpensesSummary AS (
    SELECT
        customer,
        SUM(amount) AS TotalExpenses
    FROM
        tblExpenses
    GROUP BY
        customer
),
ItemPurchasesSummary AS (
    SELECT
        purchase_under,
        SUM(total_amount) AS TotalItemPurchases
    FROM
        tblItemPurchase
    GROUP BY
        purchase_under
),
CustomerSummary AS (
    SELECT
        p.client AS CustomerName,
        p.date AS PurchaseDate,
        SUM(p.total_cost) AS GrossSales,
        ISNULL(e.TotalExpenses, 0) AS TotalExpenses,
        ISNULL(i.TotalItemPurchases, 0) AS TotalItemPurchases,
        SUM(p.total_cost) - ISNULL(e.TotalExpenses, 0) - ISNULL(i.TotalItemPurchases, 0) AS NetIncome,
        SUM(p.total_cost) - ISNULL(e.TotalExpenses, 0) AS NetProfit
    FROM
        tblPurchased p
    LEFT JOIN
        ExpensesSummary e ON p.client = e.customer
    LEFT JOIN
        ItemPurchasesSummary i ON p.client = i.purchase_under
    WHERE
        p.status != 'Voided'";

                // Add date filtering logic only if dates are selected
                // Add date filtering logic only if dates are selected

                // Add parameters to prevent SQL injection
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    query += " AND p.client LIKE @search";
                }

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

                query += @"
    GROUP BY
        p.client, p.date, e.TotalExpenses, i.TotalItemPurchases
)
SELECT 
    CustomerName,
    FORMAT(PurchaseDate, 'MMMM dd, yyyy') AS PurchaseDate,
    GrossSales,
    TotalExpenses,
    TotalItemPurchases,
    NetIncome,
    NetProfit
FROM
    CustomerSummary
ORDER BY
    PurchaseDate;";

                cn.Open();
                cm = new SqlCommand(query, cn);

                // Add parameters for date filtering if necessary
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    cm.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");
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
                    // Parse NetIncome and NetProfit values
                    decimal netIncome = Convert.ToDecimal(dr["NetIncome"]);
                    decimal netProfit = Convert.ToDecimal(dr["NetProfit"]);

                    // Add rows to DataGridView
                    int rowIndex = dataGridView1.Rows.Add(
                        i,
                        dr["PurchaseDate"] != DBNull.Value ? dr["PurchaseDate"].ToString() : "N/A",
                        dr["CustomerName"].ToString(),
                        Convert.ToDecimal(dr["GrossSales"]).ToString("₱#,##0.00"),
                        Convert.ToDecimal(dr["TotalExpenses"]).ToString("₱#,##0.00"),
                        Convert.ToDecimal(dr["TotalItemPurchases"]).ToString("₱#,##0.00"),
                        netIncome.ToString("₱#,##0.00"),
                        netProfit.ToString("₱#,##0.00")
                    );

                    // Change the background color of 'Net Income' column
                    DataGridViewCell netIncomeCell = dataGridView1.Rows[rowIndex].Cells["net_income"];
                    netIncomeCell.Style.BackColor = Color.LightYellow; // Apply desired background color
                    netIncomeCell.Style.ForeColor = Color.Black;

                    // Change the background color of 'Net Profit' column
                    DataGridViewCell netProfitCell = dataGridView1.Rows[rowIndex].Cells["total_profit"];
                    netProfitCell.Style.BackColor = Color.LightYellow; // Apply desired background color
                    netProfitCell.Style.ForeColor = Color.Black;

                    // Change row color if NetIncome or NetProfit is negative
                    if (netIncome < 0 || netProfit < 0)
                    {
                        dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightPink;
                        dataGridView1.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    }


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

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadRecords();
            CalculateTotalGrossSales();
            CalculateTotalExpense();
            CalculateTotalPurchase();
            CalculateTotalNetIncome();
            CalculateTotalTotalProfit();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            LoadRecords();
            CalculateTotalGrossSales();
            CalculateTotalExpense();
            CalculateTotalPurchase();
            CalculateTotalNetIncome();
            CalculateTotalTotalProfit();
        }

        public void CalculateTotalGrossSales()
        {
            decimal totalGrossSales = 0;

            try
            {
                // Iterate through each row in the DataGridView
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["gross_sales"].Value != null)
                    {
                        string amountString = row.Cells["gross_sales"].Value.ToString();

                        // Remove the peso sign (₱) if it exists
                        if (amountString.Contains("₱"))
                        {
                            amountString = amountString.Replace("₱", "").Trim();
                        }

                        // Parse the amount to a decimal
                        if (decimal.TryParse(amountString, out decimal value))
                        {
                            totalGrossSales += value; // Add the parsed value to totalExpense
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating total gross sales: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Display the total expense in the label with proper formatting
            lblTotalGrossSales.Text = "₱" + totalGrossSales.ToString("#,##0.00");
        }

        public void CalculateTotalExpense()
        {
            decimal totalGrossSales = 0;

            try
            {
                // Iterate through each row in the DataGridView
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["expenses"].Value != null)
                    {
                        string amountString = row.Cells["expenses"].Value.ToString();

                        // Remove the peso sign (₱) if it exists
                        if (amountString.Contains("₱"))
                        {
                            amountString = amountString.Replace("₱", "").Trim();
                        }

                        // Parse the amount to a decimal
                        if (decimal.TryParse(amountString, out decimal value))
                        {
                            totalGrossSales += value; // Add the parsed value to totalExpense
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating total expense: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Display the total expense in the label with proper formatting
            lblTotalExpenses.Text = "₱" + totalGrossSales.ToString("#,##0.00");
        }

        public void CalculateTotalPurchase()
        {
            decimal totalGrossSales = 0;

            try
            {
                // Iterate through each row in the DataGridView
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["purchase"].Value != null)
                    {
                        string amountString = row.Cells["purchase"].Value.ToString();

                        // Remove the peso sign (₱) if it exists
                        if (amountString.Contains("₱"))
                        {
                            amountString = amountString.Replace("₱", "").Trim();
                        }

                        // Parse the amount to a decimal
                        if (decimal.TryParse(amountString, out decimal value))
                        {
                            totalGrossSales += value; // Add the parsed value to totalExpense
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating total expense: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Display the total expense in the label with proper formatting
            lblPurchase.Text = "₱" + totalGrossSales.ToString("#,##0.00");
        }

        public void CalculateTotalNetIncome()
        {
            decimal totalGrossSales = 0;

            try
            {
                // Iterate through each row in the DataGridView
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["net_income"].Value != null)
                    {
                        string amountString = row.Cells["net_income"].Value.ToString();

                        // Remove the peso sign (₱) if it exists
                        if (amountString.Contains("₱"))
                        {
                            amountString = amountString.Replace("₱", "").Trim();
                        }

                        // Parse the amount to a decimal
                        if (decimal.TryParse(amountString, out decimal value))
                        {
                            totalGrossSales += value; // Add the parsed value to totalExpense
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating total expense: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Display the total expense in the label with proper formatting
            lblNetIncome.Text = "₱" + totalGrossSales.ToString("#,##0.00");
        }


        public void CalculateTotalTotalProfit()
        {
            decimal totalGrossSales = 0;

            try
            {
                // Iterate through each row in the DataGridView
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["total_profit"].Value != null)
                    {
                        string amountString = row.Cells["total_profit"].Value.ToString();

                        // Remove the peso sign (₱) if it exists
                        if (amountString.Contains("₱"))
                        {
                            amountString = amountString.Replace("₱", "").Trim();
                        }

                        // Parse the amount to a decimal
                        if (decimal.TryParse(amountString, out decimal value))
                        {
                            totalGrossSales += value; // Add the parsed value to totalExpense
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating total expense: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Display the total expense in the label with proper formatting
            lblTotalProfit.Text = "₱" + totalGrossSales.ToString("#,##0.00");
        }


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void panel12_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
