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
    public partial class mj_completePaymentPurchased : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;

        mj_Purchased mj_purchased;

        public mj_completePaymentPurchased(mj_Purchased frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            mj_purchased = frm;
        }

        private void txtPricePerPc_TextChanged(object sender, EventArgs e)
        {   

        }

        public void getTotalProductsGrid()
        {
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
                MessageBox.Show("An error occurred while counting total products: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void currentDate_ValueChanged(object sender, EventArgs e)
        {
            txtDateOfPayment.CustomFormat = "yyyy-MM-dd";
        }

        private void mj_completePaymentPurchased_Load(object sender, EventArgs e)
        {
            //txtDateOfPayment.CustomFormat = " ";
            //txtDateOfPayment.Format = DateTimePickerFormat.Custom;
            this.ActiveControl = txtSearch; 
            getTotalProductsGrid();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        //public void UpdateSoldQty()
        //{
        //    int selectedRowIndex = dataGridView1.SelectedRows[0].Index;
        //    int qty = int.Parse(dataGridView1.Rows[selectedRowIndex].Cells["qty"].Value.ToString());
        //    int sku = int.Parse(dataGridView1.Rows[selectedRowIndex].Cells["sku"].Value.ToString());

        //    cn.Open();
        //    string query = "UPDATE tblInventory SET qty_sold = qty_sold + " + qty + " WHERE SKU = '" + sku + "'";
        //    cm = new SqlCommand(query, cn);
        //    cm.ExecuteNonQuery();
        //    cn.Close();
        //}

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            String status = "Paid";
            try
            {
                // Ensure all fields are filled
                if (txtTypeOfPurchased.Text == "" || string.IsNullOrWhiteSpace(txtDateOfPayment.Text))                {
                    MessageBox.Show("Please fill up all fields", "PAY ITEM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if a row is selected in the DataGridView
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("No item has been selected.", "PAY ITEM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get the selected row index and the id of the selected item
                int selectedRowIndex = dataGridView1.SelectedRows[0].Index;
                int id = int.Parse(dataGridView1.Rows[selectedRowIndex].Cells["id"].Value.ToString());

                // Confirm update
                if (MessageBox.Show("Do you want to confirm payment for this item?", "Confirm Payment", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Update the selected item in the database
                    cn.Open();
                    string query = "UPDATE tblPurchased SET type_of_purchased = @type_of_purchased, date_of_payment = @date_of_payment, status = @status WHERE id = @id";
                    cm = new SqlCommand(query, cn);

                    // Pass the new values from the form controls
                    cm.Parameters.AddWithValue("@type_of_purchased", txtTypeOfPurchased.Text);
                    cm.Parameters.AddWithValue("@status", status);
                    

                    // Check if date_of_payment is valid, otherwise set it to NULL
                    if (txtDateOfPayment.CustomFormat == " ")
                    {
                        cm.Parameters.AddWithValue("@date_of_payment", DBNull.Value); // Set to NULL if empty
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@date_of_payment", txtDateOfPayment.Value);
                    }

                    cm.Parameters.AddWithValue("@id", id);  // Use the selected item's id for updating
                    cm.ExecuteNonQuery();
                    cn.Close();

                    // Notify user of success
                    MessageBox.Show("Item has been successfully paid!", "Payment Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear the input fields and reload the records in DataGridView
                    txtTypeOfPurchased.Clear();
                    txtDateOfPayment.CustomFormat = " "; // Reset DateTimePicker to blank state
                    txtDateOfPayment.Format = DateTimePickerFormat.Custom;
                    txtTypeOfPurchased.Focus();
                    LoadRecords();  // Reload DataGridView records after update
                    mj_purchased.LoadRecords();
                    //UpdateSoldQty();
                }
        }
            catch (Exception ex)
            {
                // Handle any errors
                cn.Close();
                MessageBox.Show(ex.Message);
            }
}


        public void LoadRecords()
        {
            try
            {
                int i = 0;
                dataGridView1.Rows.Clear();

                cn.Open();

                // Use a parameterized query to search only by the client field
                cm = new SqlCommand("SELECT * FROM tblPurchased WHERE client LIKE @search AND (type_of_purchased IS NULL OR date_of_payment IS NULL) AND status != 'Voided'", cn);
                cm.Parameters.AddWithValue("@search", "%" + txtSearch.Text.Trim() + "%");

                dr = cm.ExecuteReader();

                while (dr.Read())
                {
                    string formattedQty = int.Parse(dr["qty"].ToString()).ToString("N0");

                    i++;
                    string pricePerPc = "₱" + Double.Parse(dr["price_per_pc"].ToString()).ToString("#,##0.00");
                    string totalCost = "₱" + Double.Parse(dr["total_cost"].ToString()).ToString("#,##0.00");
                    string balance = "₱" + Double.Parse(dr["balance"].ToString()).ToString("#,##0.00");
                    string formattedDate = DateTime.TryParse(dr["date"].ToString(), out DateTime parsedDate) ? parsedDate.ToString("MM/dd/yyyy") : "N/A";

                    dataGridView1.Rows.Add(i, formattedDate, dr["client"].ToString(), dr["item_description"].ToString(), dr["status"].ToString(), balance, pricePerPc,
                        formattedQty, totalCost, dr["SKU"].ToString(), int.Parse(dr["id"].ToString()));
                }

                dr.Close();
                cn.Close();

                // Show or hide the label and DataGridView based on the row count
                if (dataGridView1.Rows.Count == 0)
                {
                    lblNoData.Visible = true;
                    dataGridView1.Visible = false;
                }
                else
                {
                    lblNoData.Visible = false;
                    dataGridView1.Visible = true;
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }



        private void printReceipt_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String status = "Paid";
            try
            {
                // Ensure all fields are filled
                if (string.IsNullOrWhiteSpace(txtTypeOfPurchased.Text) || string.IsNullOrWhiteSpace(txtDateOfPayment.Text))
                {
                    MessageBox.Show("Please fill up all fields", "PAY ITEM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if a row is selected in the DataGridView
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("No item has been selected.", "PAY ITEM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get the selected row index and the id of the selected item
                int selectedRowIndex = dataGridView1.SelectedRows[0].Index;
                int id = int.Parse(dataGridView1.Rows[selectedRowIndex].Cells["id"].Value.ToString());

                // Confirm update
                if (MessageBox.Show("Do you want to confirm payment for this item?", "Confirm Payment", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Update the selected item in the database
                    cn.Open();
                    string query = "UPDATE tblPurchased SET type_of_purchased = @type_of_purchased, date_of_payment = @date_of_payment, status = @status WHERE id = @id";
                    cm = new SqlCommand(query, cn);

                    // Pass the new values from the form controls
                    cm.Parameters.AddWithValue("@type_of_purchased", txtTypeOfPurchased.Text);
                    cm.Parameters.AddWithValue("@status", status);


                    // Check if date_of_payment is valid, otherwise set it to NULL
                    if (txtDateOfPayment.CustomFormat == " ")
                    {
                        cm.Parameters.AddWithValue("@date_of_payment", DBNull.Value); // Set to NULL if empty
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@date_of_payment", txtDateOfPayment.Value);
                    }

                    cm.Parameters.AddWithValue("@id", id);  // Use the selected item's id for updating
                    cm.ExecuteNonQuery();
                    cn.Close();

                    // Notify user of success
                    MessageBox.Show("Item has been successfully paid!", "Payment Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear the input fields and reload the records in DataGridView
                    txtTypeOfPurchased.Clear();
                    //txtDateOfPayment.CustomFormat = " "; // Reset DateTimePicker to blank state
                    //txtDateOfPayment.Format = DateTimePickerFormat.Custom;
                    txtTypeOfPurchased.Focus();
                    LoadRecords();  // Reload DataGridView records after update
                    getTotalProductsGrid();
                    mj_purchased.LoadRecords();
                    //UpdateSoldQty();
                }
            }
            catch (Exception ex)
            {
                // Handle any errors
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }
    }
}
