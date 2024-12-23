using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace OOP_System
{
    public partial class mj_Purchased : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;

        public mj_Purchased()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        private void mj_Purchased_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtSearch;
            cboStatus.Items.AddRange(new string[] { "All", "Paid", "Down Payment", "Unpaid", "Voided" });
            cboStatus.SelectedIndex = 0; // Default to "All"

            // Set DatePickers to optional state with CheckBox
            dtFrom.ShowCheckBox = true;
            dtTo.ShowCheckBox = true;

            dtFrom.Checked = false;
            dtTo.Checked = false;

            LoadRecords();
            getTotalOrders();
        }

        public void getTotalOrders()
        {

            //int totalOrders = 0;

            //try
            //{
            //    cn.Open();
            //    string query4 = "SELECT COUNT(*) FROM tblPurchased";
            //    cm = new SqlCommand(query4, cn);
            //    totalOrders = int.Parse(cm.ExecuteScalar().ToString());
            //    lblTotalOrders.Text = totalOrders.ToString();
            //    cn.Close();
            //}
            //catch (Exception ex)
            //{

            //}

            try
            {
                //dataGridView1.Rows.Clear();

                // Count the rows in the DataGridView
                int totalOrders = dataGridView1.Rows.Count;

                // Display the total in the label
                lblTotalOrders.Text = totalOrders.ToString();
            }
            catch (Exception ex)
            {
                // Handle any potential errors
                MessageBox.Show("An error occurred while counting total orders: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            mj_addPurchasedItem frm = new mj_addPurchasedItem(this);
            frm.ShowDialog();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
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
            //        //DateTime date = DateTime.Parse(dataGridView1.Rows[selectedRowIndex].Cells[1].Value.ToString());
            //        string date = dataGridView1.Rows[selectedRowIndex].Cells[1].Value.ToString();
            //        string sku = dataGridView1.Rows[selectedRowIndex].Cells[2].Value.ToString();
            //        string description = dataGridView1.Rows[selectedRowIndex].Cells[3].Value.ToString();
            //        int qty = int.Parse(dataGridView1.Rows[selectedRowIndex].Cells[4].Value.ToString());
            //        string total_cost = dataGridView1.Rows[selectedRowIndex].Cells[5].Value.ToString();
            //        double price_per_pc = double.Parse(dataGridView1.Rows[selectedRowIndex].Cells[6].Value.ToString().Replace("₱", "").Trim());
            //        string client = dataGridView1.Rows[selectedRowIndex].Cells[7].Value.ToString();
            //        int id = int.Parse(dataGridView1.Rows[selectedRowIndex].Cells[11].Value.ToString());

            //        // Extract type_of_purchased and date_of_payment from the selected row
            //        string typeOfPurchased = dataGridView1.Rows[selectedRowIndex].Cells[9].Value?.ToString();
            //        DateTime? dateOfPayment = dataGridView1.Rows[selectedRowIndex].Cells[10].Value == DBNull.Value
            //            ? (DateTime?)null : DateTime.Parse(dataGridView1.Rows[selectedRowIndex].Cells[10].Value.ToString());

            //        // Create an instance of the update form and pass the data including the optional fields
            //        mj_updatePurchasedItemcs frm = new mj_updatePurchasedItemcs(this, description, price_per_pc, client, qty, total_cost, id, typeOfPurchased, contact_number);

            //        // Show the form
            //        frm.loadRecords();
            //        frm.ShowDialog();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }


        public void LoadRecords()
        {



            try
            {
                int i = 0;
                dataGridView1.Rows.Clear();
                string query = "SELECT * FROM tblPurchased WHERE 1=1"; // Base query

                cn.Open();

                // Build the query dynamically
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    query += " AND item_description LIKE @ItemDescription";
                }
                if (cboStatus.SelectedIndex != -1 && cboStatus.Text != "All")
                {
                    query += " AND status LIKE @Status";
                }
                //if (dtFrom.Value != null && dtTo.Value != null)
                //{
                //    query += " AND date >= @DateFrom AND date < @DateTo";
                //}

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
                    cm.Parameters.AddWithValue("@ItemDescription", txtSearch.Text + "%");
                }
                if (cboStatus.SelectedIndex != -1 && cboStatus.Text != "All")
                {
                    cm.Parameters.AddWithValue("@Status", cboStatus.SelectedItem.ToString() + "%");
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

                    // Ensure safe retrieval of data from database
                    string pricePerPc = dr["price_per_pc"] != DBNull.Value
                        ? "₱" + Double.Parse(dr["price_per_pc"].ToString()).ToString("#,##0.00")
                        : "N/A";

                    string totalCost = dr["total_cost"] != DBNull.Value
                        ? "₱" + Double.Parse(dr["total_cost"].ToString()).ToString("#,##0.00")
                        : "N/A";

                    string status = dr["status"]?.ToString() ?? "N/A";

                    string mobileNumber = dr["contact_number"].ToString();

                    // Format dates
                    string formattedDate = dr["date"] != DBNull.Value && DateTime.TryParse(dr["date"].ToString(), out DateTime parsedDate)
                        ? parsedDate.ToString("MM/dd/yyyy")
                        : "N/A";

                    string formattedDateOfPayment = dr["date_of_payment"] != DBNull.Value && DateTime.TryParse(dr["date_of_payment"].ToString(), out DateTime parsedDateOfPayment)
                        ? parsedDateOfPayment.ToString("MM/dd/yyyy")
                        : " ";

                    string formattedQty = int.Parse(dr["qty"].ToString()).ToString("N0");

                    // Add data to the DataGridView
                    int rowIndex = dataGridView1.Rows.Add(
                        i,
                        formattedDate,
                        dr["client"].ToString() ?? "N/A",
                        mobileNumber,
                        dr["SKU"]?.ToString() ?? "N/A",
                        dr["item_description"]?.ToString() ?? "N/A",
                        formattedQty,
                        totalCost,
                        pricePerPc,
                        status,
                        dr["type_of_purchased"]?.ToString() ?? "N/A",
                        formattedDateOfPayment,
                        dr["id"] != DBNull.Value ? int.Parse(dr["id"].ToString()) : 0
                    );

                    // Change row color if status is 'Voided'
                    if (status.Equals("Voided", StringComparison.OrdinalIgnoreCase))
                    {
                        dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightPink;
                        dataGridView1.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.DarkRed;
                    }
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






        private void btnCompleteItem_Click(object sender, EventArgs e)
        {
            mj_completePaymentPurchased frm = new mj_completePaymentPurchased(this);
            frm.LoadRecords();
            frm.ShowDialog();
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
                    //try
                    //{
                        cn.Open();
                        string query = "DELETE FROM tblPurchased WHERE id LIKE '" + int.Parse(dataGridView1.Rows[e.RowIndex].Cells[12].Value.ToString()) + "'";
                        cm = new SqlCommand(query, cn);
                        cm.ExecuteNonQuery();
                        cn.Close();

                        cn.Open();
                        string query2 = "DELETE FROM tlbDownPayment WHERE purchased_id LIKE '" + int.Parse(dataGridView1.Rows[e.RowIndex].Cells[12].Value.ToString()) + "'";
                        cm = new SqlCommand(query2, cn);
                        cm.ExecuteNonQuery();
                        cn.Close();

                        
                        
                        LoadRecords();
                        getTotalOrders();
                            

                    // Show success message after deletion
                    MessageBox.Show("The item has been successfully deleted from the system.",
                                        "Item Deleted",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);

                    //}
                    //catch (Exception ex)
                    //{
                    //    cn.Close();
                    //    MessageBox.Show("An error occurred while trying to delete the item. Please try again.",
                    //                    "Error",
                    //                    MessageBoxButtons.OK,
                    //                    MessageBoxIcon.Error);
                    //}
                }
            }

            //if(colName == "Edit")
            //{
            //    try
            //    {
            //        // Check if any row is selected
            //        if (dataGridView1.SelectedRows.Count == 0)
            //        {
            //            MessageBox.Show("No item has been specified.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //            return;
            //        }
            //        else
            //        {
            //            // Get the selected row index
            //            int selectedRowIndex = dataGridView1.SelectedRows[0].Index;

            //            // Retrieve the data from the selected row
            //            //DateTime date = DateTime.Parse(dataGridView1.Rows[selectedRowIndex].Cells[1].Value.ToString());
            //            string date = dataGridView1.Rows[selectedRowIndex].Cells[1].Value.ToString();
            //            string sku = dataGridView1.Rows[selectedRowIndex].Cells[2].Value.ToString();
            //            string description = dataGridView1.Rows[selectedRowIndex].Cells[4].Value.ToString();
            //            int qty = int.Parse(dataGridView1.Rows[selectedRowIndex].Cells[5].Value.ToString());
            //            string total_cost = dataGridView1.Rows[selectedRowIndex].Cells[6].Value.ToString();
            //            double price_per_pc = double.Parse(dataGridView1.Rows[selectedRowIndex].Cells[7].Value.ToString().Replace("₱", "").Trim());
            //            string client = dataGridView1.Rows[selectedRowIndex].Cells[2].Value.ToString();
            //            int id = int.Parse(dataGridView1.Rows[selectedRowIndex].Cells[11].Value.ToString());

            //            // Extract type_of_purchased and date_of_payment from the selected row
            //            string typeOfPurchased = dataGridView1.Rows[selectedRowIndex].Cells[9].Value?.ToString();
            //            //DateTime? dateOfPayment = dataGridView1.Rows[selectedRowIndex].Cells[10].Value == DBNull.Value
            //            //    ? (DateTime?)null : DateTime.Parse(dataGridView1.Rows[selectedRowIndex].Cells[10].Value.ToString());

            //            // Create an instance of the update form and pass the data including the optional fields
            //            mj_updatePurchasedItemcs frm = new mj_updatePurchasedItemcs(this, description, price_per_pc, client, qty, total_cost, id, typeOfPurchased, contact_number);

            //            // Show the form
            //            frm.loadRecords();
            //            frm.ShowDialog();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //}

            if(colName == "View")
            {
                try
                {

                    if (dataGridView1.SelectedRows.Count == 0)
                    {
                        MessageBox.Show("No item has been specified.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {

                        int selectedRowIndex = dataGridView1.SelectedRows[0].Index;

                        string date  = dataGridView1.Rows[selectedRowIndex].Cells["order_date"].Value.ToString();
                        string item_description = dataGridView1.Rows[selectedRowIndex].Cells["item"].Value.ToString();
                        string price_per_pc = dataGridView1.Rows[selectedRowIndex].Cells["price"].Value.ToString();
                        string qty = dataGridView1.Rows[selectedRowIndex].Cells["qty"].Value.ToString();
                        string total_cost = dataGridView1.Rows[selectedRowIndex].Cells["total"].Value.ToString();
                        string order_status = dataGridView1.Rows[selectedRowIndex].Cells["status"].Value.ToString();
                        string payment_method = dataGridView1.Rows[selectedRowIndex].Cells["mop"].Value.ToString();
                        string payment_date = dataGridView1.Rows[selectedRowIndex].Cells["payment_date"].Value.ToString();
                        string customer_name = dataGridView1.Rows[selectedRowIndex].Cells["customer"].Value.ToString();
                        string contact_number = dataGridView1.Rows[selectedRowIndex].Cells["contact"].Value.ToString();

                    mj_view_order frm = new mj_view_order(this, date, item_description, price_per_pc, qty, total_cost, order_status, payment_method, payment_date, customer_name, contact_number);
                        frm.ShowDialog();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
            getTotalOrders();
        }

        private void btnPrintReceipt_Click(object sender, EventArgs e)
        {
            mj_printReceiptAddOrder frm = new mj_printReceiptAddOrder(this);
            frm.LoadReport();
            frm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mj_addPurchasedItem frm = new mj_addPurchasedItem(this);
            frm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mj_completePaymentPurchased frm = new mj_completePaymentPurchased(this);
            frm.getTotalProductsGrid();
            frm.LoadRecords();
            frm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            mj_printReceiptAddOrder frm = new mj_printReceiptAddOrder(this);
            frm.LoadReport();
            frm.ShowDialog();
        }

        private void cboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRecords();
            getTotalOrders();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadRecords();
            getTotalOrders();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            LoadRecords();
            getTotalOrders();
        }

        private void lblTotalOrders_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
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

                    // Retrieve the data from the selected row
                    //DateTime date = DateTime.Parse(dataGridView1.Rows[selectedRowIndex].Cells[1].Value.ToString());
                    string date = dataGridView1.Rows[selectedRowIndex].Cells[1].Value.ToString();
                    string sku = dataGridView1.Rows[selectedRowIndex].Cells[2].Value.ToString();
                    string description = dataGridView1.Rows[selectedRowIndex].Cells[5].Value.ToString();
                    int qty = int.Parse(dataGridView1.Rows[selectedRowIndex].Cells[6].Value.ToString());
                    string total_cost = dataGridView1.Rows[selectedRowIndex].Cells[7].Value.ToString();
                    double price_per_pc = double.Parse(dataGridView1.Rows[selectedRowIndex].Cells[8].Value.ToString().Replace("₱", "").Trim());
                    string client = dataGridView1.Rows[selectedRowIndex].Cells[2].Value.ToString();
                    int id = int.Parse(dataGridView1.Rows[selectedRowIndex].Cells[12].Value.ToString());
                    string contact_number = dataGridView1.Rows[selectedRowIndex].Cells[3].Value.ToString();

                    // Extract type_of_purchased and date_of_payment from the selected row
                    string typeOfPurchased = dataGridView1.Rows[selectedRowIndex].Cells[9].Value?.ToString();
                    //DateTime? dateOfPayment = dataGridView1.Rows[selectedRowIndex].Cells[10].Value == DBNull.Value
                    //    ? (DateTime?)null : DateTime.Parse(dataGridView1.Rows[selectedRowIndex].Cells[10].Value.ToString());

                    // Create an instance of the update form and pass the data including the optional fields
                    mj_updatePurchasedItemcs frm = new mj_updatePurchasedItemcs(this, description, price_per_pc, client, qty, total_cost, id, typeOfPurchased, contact_number);

                    // Show the form
                    frm.loadRecords();
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {

                // Check if the DataGridView has any rows
                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("No transactions available to process.", "No Data Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if any row is selected
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a transaction to proceed.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                int selectedRowIndex = dataGridView1.SelectedRows[0].Index;

                string currentStatus = dataGridView1.Rows[selectedRowIndex].Cells["status"].Value.ToString();

                if (currentStatus == "Voided")
                {
                    // Show a message if the item is already voided
                    MessageBox.Show("This transaction is already voided.", "Transaction Already Voided",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (currentStatus == "Paid")
                {
                    // Show a message if the item is already paid
                    MessageBox.Show("This transaction cannot be voided because it is already paid.", "Transaction Paid",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (currentStatus == "Down Payment")
                {
                    // Show a message if the transaction is in the Down Payment process
                    MessageBox.Show("This transaction cannot be voided because it is currently in the Down Payment process.",
                                    "Transaction in Down Payment Process",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    if (dataGridView1.SelectedRows.Count == 0)
                    {
                        MessageBox.Show("No item has been specified.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {

                        string date = dataGridView1.Rows[selectedRowIndex].Cells["order_date"].Value.ToString();
                        string item_description = dataGridView1.Rows[selectedRowIndex].Cells["item"].Value.ToString();
                        string price_per_pc = dataGridView1.Rows[selectedRowIndex].Cells["price"].Value.ToString();
                        string qty = dataGridView1.Rows[selectedRowIndex].Cells["qty"].Value.ToString();
                        string total_cost = dataGridView1.Rows[selectedRowIndex].Cells["total"].Value.ToString();
                        string order_status = dataGridView1.Rows[selectedRowIndex].Cells["status"].Value.ToString();
                        string payment_method = dataGridView1.Rows[selectedRowIndex].Cells["mop"].Value.ToString();
                        string payment_date = dataGridView1.Rows[selectedRowIndex].Cells["payment_date"].Value.ToString();
                        string customer_name = dataGridView1.Rows[selectedRowIndex].Cells["customer"].Value.ToString();
                        string contact_number = dataGridView1.Rows[selectedRowIndex].Cells["contact"].Value.ToString();
                        int id = int.Parse(dataGridView1.Rows[selectedRowIndex].Cells["id"].Value.ToString());

                        mj_void_order frm = new mj_void_order(this, date, item_description, price_per_pc, qty, total_cost, customer_name, contact_number, id);
                        frm.ShowDialog();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            mj_view_void_details frm = new mj_view_void_details();
            frm.LoadRecords();
            frm.getTotalProductsGrid();
            frm.ShowDialog();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
