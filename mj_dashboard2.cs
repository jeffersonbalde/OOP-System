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
using System.Windows.Forms.DataVisualization.Charting;

namespace OOP_System
{
    public partial class mj_dashboard2 : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;

        public mj_dashboard2()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        private void UpdateTotalExpenses()
        {
            try
            {
                // SQL query to calculate the total of the 'amount' column
                string query = "SELECT ISNULL(SUM(amount), 0) AS total_expenses FROM tblExpenses";

                // Open database connection
                cn.Open();
                cm = new SqlCommand(query, cn);

                // Execute the query and get the total expenses
                object result = cm.ExecuteScalar();
                double totalExpenses = Convert.ToDouble(result);

                // Format and update the label
                lblTotalExpenses.Text = "₱" + totalExpenses.ToString("#,##0.00");

                // Close the database connection
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close(); // Ensure the connection is closed on error
                MessageBox.Show("Error updating total expenses: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mj_dashboard2_Load(object sender, EventArgs e)
        {
            LoadYears();

            UpdateTotalExpenses();
            UpdateTotalPurchase();
            LoadRecords();

            UpdateTotalOrdersCount();
            UpdateTotalVoidedOrdersCount();
            UpdateTotalOrdersAmount();
            UpdateVoidAmount();

            TopProducts();
            LoadProductStatusChart();

            if (cmbYear.SelectedIndex != -1)
            {
                FillChartByYear();
            }
        }

        private void UpdateTotalPurchase()
        {
            try
            {
                // SQL query to calculate the total of the 'amount' column
                string query = "SELECT ISNULL(SUM(total_amount), 0) AS total_amount FROM tblItemPurchase";

                // Open database connection
                cn.Open();
                cm = new SqlCommand(query, cn);

                // Execute the query and get the total expenses
                object result = cm.ExecuteScalar();
                double totalExpenses = Convert.ToDouble(result);

                // Format and update the label
                lblTotalPurchase.Text = "₱" + totalExpenses.ToString("#,##0.00");

                // Close the database connection
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close(); // Ensure the connection is closed on error
                MessageBox.Show("Error updating total expenses: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void LoadRecords()
        {
            double totalProfit = 0;
            double netIncome = 0;

            try
            {
                int i = 0;
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
                dr = cm.ExecuteReader();

                while (dr.Read())
                {
                    i++;
                    // Parse NetIncome and NetProfit values
                    netIncome += Convert.ToDouble(dr["NetIncome"]);
                    totalProfit += Convert.ToDouble(dr["NetProfit"]);
                }

                dr.Close();
                cn.Close();

                // Format and update the label
                lblTotalNetIncome.Text = "₱" + netIncome.ToString("#,##0.00");

                // Format and update the label
                lblTotalProfit.Text = "₱" + totalProfit.ToString("#,##0.00");
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateTotalOrdersCount()
        {
            try
            {
                // SQL query to count the total number of rows in 'tblPurchased'
                string query = "SELECT COUNT(*) AS total_orders FROM tblPurchased";

                // Open database connection
                cn.Open();
                cm = new SqlCommand(query, cn);

                // Execute the query and get the total orders count
                object result = cm.ExecuteScalar();
                int totalOrdersCount = Convert.ToInt32(result);

                // Update the label with the count
                lblTotalOrders.Text = totalOrdersCount.ToString();

                // Close the database connection
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close(); // Ensure the connection is closed on error
                MessageBox.Show("Error updating total orders count: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateTotalVoidedOrdersCount()
        {
            try
            {
                // SQL query to count the number of records where status is 'Voided'
                string query = "SELECT COUNT(*) FROM tblPurchased WHERE status = @status";

                // Open the database connection
                cn.Open();
                cm = new SqlCommand(query, cn);

                // Add the parameter for the query
                cm.Parameters.AddWithValue("@status", "Voided");

                // Execute the query and get the result
                int totalVoidedOrders = Convert.ToInt32(cm.ExecuteScalar());

                // Update the label with the count
                lblTotalVoid.Text = totalVoidedOrders.ToString();

                // Close the database connection
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close(); // Ensure the connection is closed in case of an exception
                MessageBox.Show("Error fetching voided orders count: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void UpdateTotalOrdersAmount()
        {
            try
            {
                // SQL query to sum the total_cost column, excluding records where status is 'Voided'
                string query = "SELECT ISNULL(SUM(total_cost), 0) FROM tblPurchased WHERE status != 'Voided'";

                // Open the database connection
                cn.Open();
                cm = new SqlCommand(query, cn);

                // Execute the query and get the result
                double totalOrdersAmount = Convert.ToDouble(cm.ExecuteScalar());

                // Update the label with the formatted total cost
                lblOrdersAmount.Text = "₱" + totalOrdersAmount.ToString("#,##0.00");

                // Close the database connection
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close(); // Ensure the connection is closed in case of an exception
                MessageBox.Show("Error calculating total orders amount: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateVoidAmount()
        {
            try
            {
                // SQL query to sum the payment_amount column
                string query = "SELECT ISNULL(SUM(payment_amount), 0) FROM tblVoidProduct";

                // Open the database connection
                cn.Open();
                cm = new SqlCommand(query, cn);

                // Execute the query and get the result
                double totalVoidAmount = Convert.ToDouble(cm.ExecuteScalar());

                // Update the label with the formatted total amount
                lblVoidAmount.Text = "₱" + totalVoidAmount.ToString("#,##0.00");

                // Close the database connection
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close(); // Ensure the connection is closed in case of an exception
                MessageBox.Show("Error calculating void amount: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //public void FillChartByYear()
        //{
        //    try
        //    {
        //        cn.Open();
        //        SqlDataAdapter da = new SqlDataAdapter("SELECT YEAR(date) AS year, ISNULL (SUM(total_cost), 0.00) AS total FROM tblPurchased WHERE status LIKE 'Paid' GROUP BY YEAR(date)", cn);
        //        DataSet ds = new DataSet();
        //        da.Fill(ds, "Sales");
        //        chart1.DataSource = ds.Tables["Sales"];
        //        Series series1 = chart1.Series["Sales"];
        //        series1.ChartType = SeriesChartType.Doughnut;

        //        series1.Name = "SALES CHART";

        //        var chart = chart1;
        //        chart.Series[series1.Name].XValueMember = "year";
        //        chart.Series[series1.Name].YValueMembers = "total";
        //        chart.Series[0].IsValueShownAsLabel = true;


        //        chart1.Titles.Add("Sales Chart");
        //        chart1.Titles[0].Font = new Font("Arial", 20, FontStyle.Bold);
        //        chart1.Titles[0].ForeColor = Color.FromArgb(192, 0, 0);
        //        chart1.Titles[0].Alignment = ContentAlignment.MiddleCenter;

        //        chart1.BorderlineDashStyle = ChartDashStyle.Solid;
        //        chart1.BorderlineColor = Color.DimGray;
        //        chart1.BorderlineWidth = 1;
        //        cn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        cn.Close();
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //public void FillChartByYear()
        //{
        //    try
        //    {
        //        cn.Open();

        //        // Updated SQL query for grouping by year and day
        //        string query = @"
        //    SELECT 
        //        YEAR(date) AS year, 
        //        CONVERT(VARCHAR(10), date, 120) AS day,
        //        ISNULL(SUM(total_cost), 0.00) AS total
        //    FROM 
        //        tblPurchased
        //    WHERE 
        //        status = 'Paid'
        //    GROUP BY 
        //        YEAR(date), CONVERT(VARCHAR(10), date, 120)
        //    ORDER BY 
        //        YEAR(date), CONVERT(VARCHAR(10), date, 120)";

        //        SqlDataAdapter da = new SqlDataAdapter(query, cn);
        //        DataSet ds = new DataSet();
        //        da.Fill(ds, "Sales");

        //        // Bind the data to the chart
        //        chart1.DataSource = ds.Tables["Sales"];
        //        Series series1 = chart1.Series["Sales"];
        //        series1.ChartType = SeriesChartType.Line; // Change to Line or Bar for better readability
        //        series1.BorderWidth = 2;

        //        // Set chart data bindings
        //        series1.Name = "Sales Trend";
        //        chart1.Series[series1.Name].XValueMember = "day"; // Use day for X-Axis
        //        chart1.Series[series1.Name].YValueMembers = "total"; // Use total sales for Y-Axis

        //        // Display values on the chart
        //        chart1.Series[0].IsValueShownAsLabel = true;
        //        chart1.Series[0].LabelFormat = "₱{#,##0.00}";

        //        // Customize chart appearance
        //        chart1.Titles.Clear();
        //        chart1.Titles.Add("Daily Sales Trend");
        //        chart1.Titles[0].Font = new Font("Arial", 18, FontStyle.Bold);
        //        chart1.Titles[0].ForeColor = Color.FromArgb(0, 122, 204);
        //        chart1.Titles[0].Alignment = ContentAlignment.MiddleCenter;

        //        chart1.ChartAreas[0].AxisX.Title = "Day";
        //        chart1.ChartAreas[0].AxisY.Title = "Total Sales (₱)";
        //        chart1.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 12, FontStyle.Bold);
        //        chart1.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 12, FontStyle.Bold);
        //        chart1.ChartAreas[0].AxisX.Interval = 1; // Ensure each day is visible
        //        chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -45; // Rotate X-Axis labels for readability
        //        chart1.ChartAreas[0].AxisY.LabelStyle.Format = "₱{#,##0.00}";

        //        // Style the chart border
        //        chart1.BorderlineDashStyle = ChartDashStyle.Solid;
        //        chart1.BorderlineColor = Color.Gray;
        //        chart1.BorderlineWidth = 1;

        //        cn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        cn.Close();
        //        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        public void LoadYears()
        {
            try
            {
                cn.Open();
                string query = "SELECT DISTINCT YEAR(date) AS year FROM tblPurchased ORDER BY year DESC";
                SqlCommand cmd = new SqlCommand(query, cn);
                SqlDataReader dr = cmd.ExecuteReader();

                cmbYear.Items.Clear();
                while (dr.Read())
                {
                    cmbYear.Items.Add(dr["year"].ToString());
                }
                dr.Close();
                cn.Close();

                if (cmbYear.Items.Count > 0)
                {
                    cmbYear.SelectedIndex = 0; // Select the first year by default
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void FillChartByYear()
        {
            try
            {
                if (cmbYear.SelectedIndex == -1) // Ensure a year is selected
                {
                    MessageBox.Show("Please select a year to filter the data.", "Year Not Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cn.Open();

                // SQL query to sum total_cost by month, filtered by the selected year
                string query = @"
        SELECT 
            DATENAME(MONTH, DATEFROMPARTS(YEAR(date), MONTH(date), 1)) AS month_name,
            MONTH(date) AS month_number,
            ISNULL(SUM(total_cost), 0.00) AS total
        FROM 
            tblPurchased
        WHERE 
            status = 'Paid' AND 
            YEAR(date) = @selectedYear
        GROUP BY 
            MONTH(date), DATENAME(MONTH, DATEFROMPARTS(YEAR(date), MONTH(date), 1))
        ORDER BY 
            month_number";

                SqlDataAdapter da = new SqlDataAdapter(query, cn);
                da.SelectCommand.Parameters.AddWithValue("@selectedYear", cmbYear.SelectedItem.ToString());
                DataSet ds = new DataSet();
                da.Fill(ds, "Sales");

                // Bind data to the chart
                chart1.DataSource = ds.Tables["Sales"];
                Series series1 = chart1.Series[0];
                series1.ChartType = SeriesChartType.Column; // Use Column for monthly analysis
                series1.BorderWidth = 2;

                // Configure chart series
                series1.Name = "Monthly Sales";
                chart1.Series[series1.Name].XValueMember = "month_name"; // Use month names for X-Axis
                chart1.Series[series1.Name].YValueMembers = "total";

                // Format chart labels
                chart1.Series[0].IsValueShownAsLabel = true;
                chart1.Series[0].LabelFormat = "₱{#,##0.00}";

                // Customize chart appearance
                chart1.Titles.Clear();
                chart1.Titles.Add($"Monthly Sales for {cmbYear.SelectedItem}");
                chart1.Titles[0].Font = new Font("Arial", 14, FontStyle.Bold);
                chart1.Titles[0].ForeColor = Color.FromArgb(0, 48, 79);
                chart1.Titles[0].Alignment = ContentAlignment.MiddleCenter;

                chart1.ChartAreas[0].AxisX.Title = "Month";
                chart1.ChartAreas[0].AxisY.Title = "Total Sales (₱)";
                chart1.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 12, FontStyle.Bold);
                chart1.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 12, FontStyle.Bold);
                chart1.ChartAreas[0].AxisX.Interval = 1; // Ensure each month is visible
                chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -45; // Rotate X-Axis labels for readability
                chart1.ChartAreas[0].AxisY.LabelStyle.Format = "₱{#,##0.00}";

                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        public void TopProducts()
        {
            try
            {

                cn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT TOP 10 description,sold FROM tblProductMaster WHERE sold > 0 ORDER BY sold DESC;", cn);
                DataSet ds = new DataSet();
                da.Fill(ds, "Top Products");
                chart2.DataSource = ds.Tables["Top Products"];
                Series series1 = chart2.Series["Series1"];
                series1.ChartType = SeriesChartType.Doughnut;

                series1.Name = "Top Products";

                var chart = chart2;
                chart.Series[0].XValueMember = "description";
                chart.Series[0].YValueMembers = "sold";
                chart.Series[0].IsValueShownAsLabel = true;
                chart.Series[0].LabelFormat = "{#,##0}";


                chart2.Titles.Add("Top Products");
                chart2.Titles[0].Font = new Font("Arial", 14, FontStyle.Bold);
                chart2.Titles[0].ForeColor = Color.FromArgb(0, 48, 79);
                chart2.Titles[0].Alignment = ContentAlignment.MiddleLeft;

                //chart2.BorderlineDashStyle = ChartDashStyle.Solid;
                //chart2.BorderlineColor = Color.DimGray;
                //chart2.BorderlineWidth = 1;

                cn.Close();

            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void LoadProductStatusChart()
        {
            try
            {
                // Open the database connection
                cn.Open();

                // Query to count products by status
                string query = @"
            SELECT 
                status, 
                COUNT(*) AS product_count
            FROM 
                tblProductMaster
            GROUP BY 
                status";

                // Execute the query and fetch the data
                SqlDataAdapter da = new SqlDataAdapter(query, cn);
                DataSet ds = new DataSet();
                da.Fill(ds, "ProductStatus");

                // Set the chart's data source
                chart3.DataSource = ds.Tables["ProductStatus"];
                Series series1 = chart3.Series["Series1"];
                series1.ChartType = SeriesChartType.Pie; // You can use Doughnut, Bar, etc.

                // Set the chart series properties
                series1.Name = "Product Status";
                series1.XValueMember = "status";
                series1.YValueMembers = "product_count";
                series1.IsValueShownAsLabel = true;
                series1.LabelFormat = "{#,##0}";

                // Customize chart title
                chart3.Titles.Clear();
                chart3.Titles.Add("Products Overview");
                chart3.Titles[0].Font = new Font("Arial", 14, FontStyle.Bold);
                chart3.Titles[0].ForeColor = Color.FromArgb(0, 48, 79);
                chart3.Titles[0].Alignment = ContentAlignment.MiddleLeft;

                // Close the database connection
                cn.Close();
            }
            catch (Exception ex)
            {
                // Handle any errors
                cn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillChartByYear();
        }
    }
}
