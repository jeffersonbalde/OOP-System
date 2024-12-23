using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace OOP_System
{
    public partial class mj_reporting : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;

        public mj_reporting()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());

        }

        private void mj_reporting_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();

            cboReportType.Items.AddRange(new string[] { "", "Orders", "Expenses", "Purchases", "Salary", "Down Payment", "Products" });
            cboReportType.SelectedIndex = 0; // Default to "All"

            dtFrom.ShowCheckBox = true;
            dtTo.ShowCheckBox = true;

            dtFrom.Checked = false;
            dtTo.Checked = false;

        }

        public void LoadProducts()
        {
            try
            {

                ReportDataSource rptDS;

                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\rpt_mj_products.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();

                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                string query1 = "SELECT * FROM tblProductMaster";
                da.SelectCommand = new SqlCommand(query1, cn);

                da.Fill(ds.Tables["dtProducts"]);
                cn.Close();

                ReportParameter pStoreName = new ReportParameter("pStoreName", "MJ SUBLIMEPRINT AND TAILORING SHOP");
                ReportParameter pTitle = new ReportParameter("pTitle", "PRODUCTS REPORT");

                reportViewer1.LocalReport.SetParameters(pStoreName);
                reportViewer1.LocalReport.SetParameters(pTitle);

                rptDS = new ReportDataSource("DataSet1", ds.Tables["dtProducts"]);
                reportViewer1.LocalReport.DataSources.Add(rptDS);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void LoadOrders()
        {
            //try
            //{

                ReportDataSource rptDS;

                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\rpt_mj_transactions.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();

                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                string query1 = "SELECT * FROM tblPurchased WHERE 1=1";


                if (dtFrom.Checked && dtTo.Checked)
                {
                    query1 += " AND date >= @start_date AND date < @end_date";
                }
                else if (dtFrom.Checked) // Only From date is set
                {
                    query1 += " AND date >= @start_date";
                }
                else if (dtTo.Checked) // Only To date is set
                {
                    query1 += " AND date < @end_date";
                }

                query1 += " ORDER BY date"; // Add order 

                da.SelectCommand = new SqlCommand(query1, cn);

                if (dtFrom.Checked)
                {
                    da.SelectCommand.Parameters.AddWithValue("@start_date", dtFrom.Value.Date);
                }
                if (dtTo.Checked)
                {
                    da.SelectCommand.Parameters.AddWithValue("@end_date", dtTo.Value.Date.AddDays(1));
                }


                da.Fill(ds.Tables["dtOrders"]);
                cn.Close();

                // Prepare default and dynamic report parameters
                string startDateParam = "Start Date: Not Specified";
                string endDateParam = "End Date: Not Specified";

                if (dtFrom.Checked)
                {
                    startDateParam = "Start Date: " + dtFrom.Value.ToString("MMMM dd, yyyy");
                }
                if (dtTo.Checked)
                {
                    endDateParam = "End Date: " + dtTo.Value.ToString("MMMM dd, yyyy");
                }

                List<ReportParameter> reportParameters = new List<ReportParameter>
                {
                    new ReportParameter("pStoreName", "MJ SUBLIMEPRINT AND TAILORING SHOP"),
                    new ReportParameter("pTitle", "ORDERS REPORT"),
                    new ReportParameter("dtFrom", startDateParam),
                    new ReportParameter("dtTo", endDateParam)
                };

                this.reportViewer1.LocalReport.SetParameters(reportParameters);

                rptDS = new ReportDataSource("DataSet1", ds.Tables["dtOrders"]);
                reportViewer1.LocalReport.DataSources.Add(rptDS);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;

            //}
            //catch (Exception ex)
            //{
            //        MessageBox.Show(ex.Message);
            //}
        }

        public void LoadExpenses()
        {
            try
            {

            ReportDataSource rptDS;

            this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\rpt_mj_expenses.rdlc";
            this.reportViewer1.LocalReport.DataSources.Clear();

            DataSet1 ds = new DataSet1();
            SqlDataAdapter da = new SqlDataAdapter();

            // Ensure the table is created
            if (!ds.Tables.Contains("dtExpenses"))
            {
                ds.Tables.Add("dtExpenses");
            }

            string query1 = "SELECT * FROM tblExpenses WHERE 1=1";

            if (dtFrom.Checked && dtTo.Checked)
            {
                query1 += " AND date >= @start_date AND date < @end_date";
            }
            else if (dtFrom.Checked) // Only From date is set
            {
                query1 += " AND date >= @start_date";
            }
            else if (dtTo.Checked) // Only To date is set
            {
                query1 += " AND date < @end_date";
            }

            query1 += " ORDER BY date"; // Add order 

            da.SelectCommand = new SqlCommand(query1, cn);

            if (dtFrom.Checked)
            {
                da.SelectCommand.Parameters.AddWithValue("@start_date", dtFrom.Value.Date);
            }
            if (dtTo.Checked)
            {
                da.SelectCommand.Parameters.AddWithValue("@end_date", dtTo.Value.Date.AddDays(1));
            }

            da.Fill(ds.Tables["dtExpenses"]);
            cn.Close();

            // Prepare default and dynamic report parameters
            string startDateParam = "Start Date: Not Specified";
            string endDateParam = "End Date: Not Specified";

            if (dtFrom.Checked)
            {
                startDateParam = "Start Date: " + dtFrom.Value.ToString("MMMM dd, yyyy");
            }
            if (dtTo.Checked)
            {
                endDateParam = "End Date: " + dtTo.Value.ToString("MMMM dd, yyyy");
            }

            List<ReportParameter> reportParameters = new List<ReportParameter>
            {
                new ReportParameter("pStoreName", "MJ SUBLIMEPRINT AND TAILORING SHOP"),
                new ReportParameter("pTitle", "EXPENSES REPORT"),
                new ReportParameter("dtFrom", startDateParam),
                new ReportParameter("dtTo", endDateParam)
            };

            this.reportViewer1.LocalReport.SetParameters(reportParameters);

            rptDS = new ReportDataSource("DataSet1", ds.Tables["dtExpenses"]);
            reportViewer1.LocalReport.DataSources.Add(rptDS);
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
            reportViewer1.ZoomMode = ZoomMode.Percent;
            reportViewer1.ZoomPercent = 100;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void LoadItemPurchase()
        {
            try
            {

                ReportDataSource rptDS;

                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\rpt_mj_itempurchase.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();

                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                // Ensure the table is created
                if (!ds.Tables.Contains("dtItemPurchase"))
                {
                    ds.Tables.Add("dtItemPurchase");
                }

                string query1 = "SELECT * FROM tblItemPurchase WHERE 1=1";


                if (dtFrom.Checked && dtTo.Checked)
                {
                    query1 += " AND purchase_date >= @start_date AND purchase_date < @end_date";
                }
                else if (dtFrom.Checked) // Only From date is set
                {
                    query1 += " AND purchase_date >= @start_date";
                }
                else if (dtTo.Checked) // Only To date is set
                {
                    query1 += " AND purchase_date < @end_date";
                }

                query1 += " ORDER BY purchase_date"; // Add order 

                da.SelectCommand = new SqlCommand(query1, cn);

                if (dtFrom.Checked)
                {
                    da.SelectCommand.Parameters.AddWithValue("@start_date", dtFrom.Value.Date);
                }
                if (dtTo.Checked)
                {
                    da.SelectCommand.Parameters.AddWithValue("@end_date", dtTo.Value.Date.AddDays(1));
                }

                da.Fill(ds.Tables["dtItemPurchase"]);
                cn.Close();

                string startDateParam = "Start Date: Not Specified";
                string endDateParam = "End Date: Not Specified";

                if (dtFrom.Checked)
                {
                    startDateParam = "Start Date: " + dtFrom.Value.ToString("MMMM dd, yyyy");
                }
                if (dtTo.Checked)
                {
                    endDateParam = "End Date: " + dtTo.Value.ToString("MMMM dd, yyyy");
                }

                List<ReportParameter> reportParameters = new List<ReportParameter>
                {
                new ReportParameter("pStoreName", "MJ SUBLIMEPRINT AND TAILORING SHOP"),
                new ReportParameter("pTitle", "PURCHASE REPORT"),
                new ReportParameter("dtFrom", startDateParam),
                new ReportParameter("dtTo", endDateParam)
                };

                this.reportViewer1.LocalReport.SetParameters(reportParameters);

                rptDS = new ReportDataSource("DataSet1", ds.Tables["dtItemPurchase"]);
                reportViewer1.LocalReport.DataSources.Add(rptDS);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void LoadSalary()
        {
            try
            {

                ReportDataSource rptDS;

                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\rpt_mj_salary.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();

                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                // Ensure the table is created
                if (!ds.Tables.Contains("dtSalary"))
                {
                    ds.Tables.Add("dtSalary");
                }

                string query1 = "SELECT * FROM tblSalary WHERE 1=1";


                if (dtFrom.Checked && dtTo.Checked)
                {
                    query1 += " AND start_date >= @start_date AND start_date < @end_date";
                }
                else if (dtFrom.Checked) // Only From date is set
                {
                    query1 += " AND start_date >= @start_date";
                }
                else if (dtTo.Checked) // Only To date is set
                {
                    query1 += " AND start_date < @end_date";
                }

                query1 += " ORDER BY start_date"; // Add order 

                da.SelectCommand = new SqlCommand(query1, cn);

                if (dtFrom.Checked)
                {
                    da.SelectCommand.Parameters.AddWithValue("@start_date", dtFrom.Value.Date);
                }
                if (dtTo.Checked)
                {
                    da.SelectCommand.Parameters.AddWithValue("@end_date", dtTo.Value.Date.AddDays(1));
                }

                da.Fill(ds.Tables["dtSalary"]);
                cn.Close();

                string startDateParam = "Start Date: Not Specified";
                string endDateParam = "End Date: Not Specified";

                if (dtFrom.Checked)
                {
                    startDateParam = "Start Date: " + dtFrom.Value.ToString("MMMM dd, yyyy");
                }
                if (dtTo.Checked)
                {
                    endDateParam = "End Date: " + dtTo.Value.ToString("MMMM dd, yyyy");
                }

                List<ReportParameter> reportParameters = new List<ReportParameter>
                {
                new ReportParameter("pStoreName", "MJ SUBLIMEPRINT AND TAILORING SHOP"),
                new ReportParameter("pTitle", "SALARY REPORT"),
                new ReportParameter("dtFrom", startDateParam),
                new ReportParameter("dtTo", endDateParam)
                };

                this.reportViewer1.LocalReport.SetParameters(reportParameters);

                rptDS = new ReportDataSource("DataSet1", ds.Tables["dtSalary"]);
                reportViewer1.LocalReport.DataSources.Add(rptDS);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void LoadDownPayment()
        {
            try
            {

                ReportDataSource rptDS;

                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\rpt_mj_downpayment.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();

                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                // Ensure the table is created
                if (!ds.Tables.Contains("dtDownPayment"))
                {
                    ds.Tables.Add("dtDownPayment");
                }

                string query1 = "SELECT * FROM tlbDownPayment WHERE 1=1";

                if (dtFrom.Checked && dtTo.Checked)
                {
                    query1 += " AND date_paid >= @start_date AND date_paid < @end_date";
                }
                else if (dtFrom.Checked) // Only From date is set
                {
                    query1 += " AND date_paid >= @start_date";
                }
                else if (dtTo.Checked) // Only To date is set
                {
                    query1 += " AND date_paid < @end_date";
                }

                query1 += " ORDER BY date_paid"; // Add order 

                da.SelectCommand = new SqlCommand(query1, cn);

                if (dtFrom.Checked)
                {
                    da.SelectCommand.Parameters.AddWithValue("@start_date", dtFrom.Value.Date);
                }
                if (dtTo.Checked)
                {
                    da.SelectCommand.Parameters.AddWithValue("@end_date", dtTo.Value.Date.AddDays(1));
                }

                da.Fill(ds.Tables["dtDownPayment"]);
                cn.Close();

                string startDateParam = "Start Date: Not Specified";
                string endDateParam = "End Date: Not Specified";

                if (dtFrom.Checked)
                {
                    startDateParam = "Start Date: " + dtFrom.Value.ToString("MMMM dd, yyyy");
                }
                if (dtTo.Checked)
                {
                    endDateParam = "End Date: " + dtTo.Value.ToString("MMMM dd, yyyy");
                }

                List<ReportParameter> reportParameters = new List<ReportParameter>
                {
                new ReportParameter("pStoreName", "MJ SUBLIMEPRINT AND TAILORING SHOP"),
                new ReportParameter("pTitle", "DOWN PAYMENT REPORT"),
                new ReportParameter("dtFrom", startDateParam),
                new ReportParameter("dtTo", endDateParam)
                };

                this.reportViewer1.LocalReport.SetParameters(reportParameters);

                rptDS = new ReportDataSource("DataSet1", ds.Tables["dtDownPayment"]);
                reportViewer1.LocalReport.DataSources.Add(rptDS);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear previous data sources and reset the report viewer
            reportViewer1.LocalReport.DataSources.Clear();

            // Load the selected report
            switch (cboReportType.Text)
            {
                case "Products":
                    LoadProducts();
                    break;
                case "Orders":
                    LoadOrders();
                    break;
                case "Expenses":
                    LoadExpenses();
                    break;
                case "Purchases":
                    LoadItemPurchase();
                    break;
                case "Salary":
                    LoadSalary();
                    break;
                case "Down Payment":
                    LoadDownPayment();
                    break;
                default:
                    // No report type selected, clear the report
                    reportViewer1.LocalReport.DataSources.Clear();
                    break;
            }

            // Refresh the report viewer to display the selected report
            reportViewer1.RefreshReport();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {

            if (dtFrom.Checked)
            {
                // Refresh based on the selected report when checked
                cboStatus_SelectedIndexChanged(sender, e);
            }
            else
            {
                // Reset to default when unchecked
                cboStatus_SelectedIndexChanged(sender, e);
            }
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {

            if (dtTo.Checked)
            {
                // Refresh based on the selected report when checked
                cboStatus_SelectedIndexChanged(sender, e);
            }
            else
            {
                // Reset to default when unchecked
                cboStatus_SelectedIndexChanged(sender, e);
            }
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void btnConvertToPDF_Click(object sender, EventArgs e)
        {

            if (reportViewer1.LocalReport.DataSources.Count == 0)
            {
                MessageBox.Show("Please load the report first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string reportType = cboReportType.Text + "Report"; // Set default value

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            saveFileDialog.FileName = reportType + ".pdf";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Warning[] warnings;
                    string[] streamIds;
                    string mimeType = "application/pdf";
                    string encoding = "utf-8";
                    string extension = "pdf";

                    byte[] bytes = reportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

                    using (var fs = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        fs.Write(bytes, 0, bytes.Length);
                    }

                    MessageBox.Show("PDF saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure the report is loaded
                if (reportViewer1.LocalReport.DataSources.Count == 0)
                {
                    MessageBox.Show("Please load the report first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string reportType = cboReportType.Text + "Report"; // Get report type dynamically

                byte[] bytes = reportViewer1.LocalReport.Render(
                    format: "Excel",
                    deviceInfo: null,
                    out string mimeType,
                    out string encoding,
                    out string fileNameExtension,
                    out string[] streamIds,
                    out Warning[] warnings
                );

                // Open a SaveFileDialog for user to choose the save location
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Excel files (*.xls)|*.xls";
                    saveFileDialog.Title = "Save the Report as Excel";
                    saveFileDialog.FileName = reportType + ".xls";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Save the file to the selected path
                        using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create))
                        {
                            fs.Write(bytes, 0, bytes.Length);
                        }

                        MessageBox.Show($"Report successfully exported to Excel!\nLocation: {saveFileDialog.FileName}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to Excel: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConvertToWord_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure the report is loaded
                if (reportViewer1.LocalReport.DataSources.Count == 0)
                {
                    MessageBox.Show("Please load the report first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string reportType = cboReportType.Text + "Report"; // Get report type dynamically

                byte[] bytes = reportViewer1.LocalReport.Render(
                    format: "Word",
                    deviceInfo: null,
                    out string mimeType,
                    out string encoding,
                    out string fileNameExtension,
                    out string[] warnings, // Correct type here
                    out Warning[] streams
                );

                // Open a SaveFileDialog for user to choose the save location
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Word documents (*.doc)|*.doc";
                    saveFileDialog.Title = "Save the Report as Word";
                    saveFileDialog.FileName = reportType + ".doc";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Save the file to the selected path
                        using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create))
                        {
                            fs.Write(bytes, 0, bytes.Length);
                        }

                        MessageBox.Show($"Report successfully exported to Word!\nLocation: {saveFileDialog.FileName}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to Word: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if the report data is loaded
                if (reportViewer1.LocalReport.DataSources.Count == 0)
                {
                    MessageBox.Show("Please load the report first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Directly trigger the PrintDialog from the ReportViewer
                reportViewer1.PrintDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
