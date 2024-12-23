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
    public partial class mj_ProductMaster : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;


        public mj_ProductMaster()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }
        
        public void getTotalProducts()
        {

            int totalProducts = 0;

            try
            {
                cn.Open();
                string query4 = "SELECT COUNT(*) FROM tblProductMaster";
                cm = new SqlCommand(query4, cn);
                totalProducts = int.Parse(cm.ExecuteScalar().ToString());
                lblTotalProducts.Text = totalProducts.ToString();
                cn.Close();
            }
            catch (Exception ex)
            {

            }
        }
        public void button2_Click(object sender, EventArgs e)
        {
            mj_addProdductMasterItem frm = new mj_addProdductMasterItem(this);
            //String text = "";

            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    cn.Open();
            //    text += dataGridView1.Rows[i].Cells[3].Value.ToString();
            //    cn.Close();
            //}
            //qRCodeGenerator.txtInput.Text = text;
            frm.ShowDialog();
        }

        private void mj_ProductMaster_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtSearch;
            cboStatus.Items.AddRange(new string[] { "All", "Active", "Voided"});
            cboStatus.SelectedIndex = 0; // Default to "All"
            LoadRecords();
        }

        public void getTotalProductsGrid()
        {
            try
            {
                //dataGridView1.Rows.Clear();

                // Count the rows in the DataGridView
                int totalOrders = dataGridView1.Rows.Count;

                // Display the total in the label
                lblTotalProducts.Text = totalOrders.ToString();
            }
            catch (Exception ex)
            {
                // Handle any potential errors
                MessageBox.Show("An error occurred while counting total products: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadRecords()
        {
            try
            {
                int i = 0;
                dataGridView1.Rows.Clear();

                // Determine filter based on ComboBox selection
                string statusFilter = cboStatus.SelectedItem?.ToString(); // Assuming your ComboBox is named cmbFilter
                string query = "SELECT * FROM tblProductMaster WHERE description LIKE @search";

                if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All")
                {
                    query += " AND status = @status"; // Add status filter if not "All"
                }

                query += " ORDER BY description"; // Order by description

                // Prepare and execute the query
                cn.Open();
                cm = new SqlCommand(query, cn);
                cm.Parameters.AddWithValue("@search", txtSearch.Text + "%");

                if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All")
                {
                    cm.Parameters.AddWithValue("@status", statusFilter);
                }

                dr = cm.ExecuteReader();

                // Populate the DataGridView
                while (dr.Read())
                {
                    i++;

                    // Format price per piece
                    string pricePerPc = "₱" + double.Parse(dr["price_per_pc"].ToString()).ToString("#,##0.00");

                    // Format quantity with commas
                    string formattedQty = int.Parse(dr["qty"].ToString()).ToString("N0");
                    string formattedSold = int.Parse(dr["sold"].ToString()).ToString("N0");

                    // Add row to DataGridView
                    int rowIndex = dataGridView1.Rows.Add(
                        i,
                        dr["SKU"].ToString(),
                        dr["description"].ToString(),
                        dr["status"].ToString(),
                        formattedQty,
                        formattedSold,
                        pricePerPc,
                        dr["id"].ToString()
                    );

                    // Change the row color if the status is "Voided"
                    if (dr["status"].ToString().Equals("Voided", StringComparison.OrdinalIgnoreCase))
                    {
                        dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightPink; // Background color
                        dataGridView1.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.DarkRed;      // Text color
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




        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the click was on a valid row (not on the header row)
            //if (e.RowIndex >= 0)
            //{
            //    btn_Update.Enabled = true; // Enable the button if a row is clicked
            //}

            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (colName == "Delete")
            {
                // Check if the selected item is already voided
                string currentStatus = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString(); // Assuming the "status" column is at index 3

                if (currentStatus == "Voided")
                {
                    // Show a message if the item is already voided
                    MessageBox.Show("This item is already voided.", "Item Already Voided",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }


                // Show a confirmation prompt before deletion
                DialogResult result = MessageBox.Show("Are you sure you want to void this item? This action will disable the item, it will no longer appear in orders, and it cannot be undone.", "Confirm Void", MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {

                        cn.Open();
                        string query = "UPDATE tblProductMaster SET status=@status WHERE id LIKE '" + dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString() + "'";
                        cm = new SqlCommand(query, cn);
                        cm.Parameters.AddWithValue("@status", "Voided");
                        cm.ExecuteNonQuery();
                        cn.Close();
                        getTotalProducts();
                        LoadRecords();

                        // Show success message after deletion
                        MessageBox.Show("Item has been successfully voided. It will no longer appear in orders", "Item Voided",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);

               
                    }
                    catch (Exception ex)
                    {
                        cn.Close();
                        MessageBox.Show("An error occurred while voiding the item: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            if (colName == "Edit")  
            {
                // Check if any row is selected
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("No item has been specified.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {        // Get the selected row index
                    int selectedRowIndex = dataGridView1.SelectedRows[0].Index;


                    // Check the status of the selected row
                    string status = dataGridView1.Rows[selectedRowIndex].Cells["Status"].Value.ToString();
                    if (status == "Voided")
                    {
                        MessageBox.Show("This item cannot be updated because its status is 'Voided'.", "Update Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    string description = dataGridView1.Rows[selectedRowIndex].Cells[2].Value.ToString();
                    int onHand = int.Parse(dataGridView1.Rows[selectedRowIndex].Cells[4].Value.ToString().Replace(",", ""));
                    string pricePerPc = dataGridView1.Rows[selectedRowIndex].Cells[6].Value.ToString();
                    int id = int.Parse(dataGridView1.Rows[selectedRowIndex].Cells[7].Value.ToString());

                    // Remove '₱' and '.' from the price and trim '00' if present
                    pricePerPc = pricePerPc.Replace("₱", "")   // Remove currency symbol
                                           .Replace(".", ""); // Remove dot
                    if (pricePerPc.EndsWith("00"))            // Remove trailing '00'
                    {
                        pricePerPc = pricePerPc.Substring(0, pricePerPc.Length - 2);
                    }

                    // Create an instance of the update form and pass the data
                    mj_updateProductMasterItem frm = new mj_updateProductMasterItem(this, id, description, onHand, pricePerPc);

                    // Show the form
                    frm.ShowDialog();
                }
            }
        }


        private void btn_Search_Click(object sender, EventArgs e)
        {
            mj_searchProductMasterItem frm = new mj_searchProductMasterItem(this);
            frm.ShowDialog();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void btnPrintReports_Click(object sender, EventArgs e)
        {
            mj_printProductMaster frm = new mj_printProductMaster();
            frm.ShowDialog();       
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            mj_addProdductMasterItem frm = new mj_addProdductMasterItem(this);
            //String text = "";

            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    cn.Open();
            //    text += dataGridView1.Rows[i].Cells[3].Value.ToString();
            //    cn.Close();
            //}
            //qRCodeGenerator.txtInput.Text = text;
            frm.ShowDialog();
        }

        private void lblTotalProducts_Click(object sender, EventArgs e)
        {
                
        }

        private void cboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRecords();
            getTotalProductsGrid();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            mj_view_low_stocks frm = new mj_view_low_stocks();
            frm.getTotalOrders();
            frm.LoadRecords();
            frm.ShowDialog();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
