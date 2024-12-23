using System;
using System.Collections;
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
using ZXing.QrCode.Internal;

namespace OOP_System
{
    public partial class mj_addPurchasedItem : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;

        //String costPerPc;
        //String pricePerPc;

        mj_Purchased mj_purchased;

        public mj_addPurchasedItem(mj_Purchased frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            mj_purchased = frm;
        }

        private void mj_addPurchasedItem_Load(object sender, EventArgs e)
        {
            loadSKUItem();
            AutoComplete();
        }
        public void checkQty()
        {
            int db_qty = 0;
            string item_description = "";

            try
            {
                cn.Open();
                string query1 = "SELECT * FROM tblProductMaster WHERE description = @description";
                cm = new SqlCommand(query1, cn);
                cm.Parameters.AddWithValue("@description", txtItemDescription.Text);
                dr = cm.ExecuteReader();
                dr.Read();

                if (dr.HasRows)
                {
                    db_qty = int.Parse(dr["qty"].ToString());
                    item_description = dr["description"].ToString();

                }

                if (db_qty < int.Parse(txtQty.Text))
                {
                    MessageBox.Show(
                        $"Unable to proceed. The item '{item_description}' has only {db_qty} item(s) remaining in stock.",
                        "Stock Warning",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                dr.Close();
                cn.Close();

            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        public void AutoComplete()
        {
            try
            {
                cn.Open();
                String query = "SELECT * FROM tblProductMaster WHERE status = 'Active'";
                cm = new SqlCommand(query, cn);
                dr = cm.ExecuteReader();
                AutoCompleteStringCollection cl = new AutoCompleteStringCollection();

                while(dr.Read())
                {
                    cl.Add(dr["description"].ToString());
                }

                txtItemDescription.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtItemDescription.AutoCompleteSource = AutoCompleteSource.CustomSource;

                txtItemDescription.AutoCompleteCustomSource = cl;
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void txtPricePerPc_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadRecords();
            generateTotalCost();
        }

        private void comboBoxCategoryAddItem_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dt2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        public void loadRecords()
        {
            if(string.IsNullOrWhiteSpace(txtItemDescription.Text)) {
                txtPricePerPc.Text = "";
                txtQty.Clear();
                txtTotalCost.Clear();
            }

            txtPricePerPc.Clear();

            try
            {
                //cn.Open();
                //string query = "SELECT * FROM tblProductMaster WHERE description = '" + txtItemDescription.Text + "'";
                //cm = new SqlCommand(query, cn);
                //dr = cm.ExecuteReader();
                //while (dr.Read())
                //{
                //    string pricePerPc = "₱" + Double.Parse(dr["price_per_pc"].ToString()).ToString("#,##0.00");
                //    txtPricePerPc.Text = pricePerPc;
                //}
                //dr.Close();
                //cn.Close();

                cn.Open();
                string query = "SELECT * FROM tblProductMaster WHERE description = @description AND status = @status";
                cm = new SqlCommand(query, cn);

                // Use parameters to prevent SQL injection
                cm.Parameters.AddWithValue("@description", txtItemDescription.Text);
                cm.Parameters.AddWithValue("@status", "Active");

                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    string pricePerPc = "₱" + Double.Parse(dr["price_per_pc"].ToString()).ToString("#,##0.00");
                    txtPricePerPc.Text = pricePerPc;
                }
                dr.Close();
                cn.Close();


            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        public void loadSKUItem()
        {
            try
            {
                txtItemDescription.Items.Clear();
                cn.Open();
                string query = "SELECT * FROM tblProductMaster WHERE status = 'Active'";
                cm = new SqlCommand(query, cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    txtItemDescription.Items.Add(dr["description"].ToString());
                }
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void txtSKU_KeyPress(object sender, KeyPressEventArgs e)
        {
            //e.Handled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //mj_printReceiptAddOrder frm = new mj_printReceiptAddOrder(this);
            //frm.ShowDialog(); 
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtQty_TextChanged(object sender, EventArgs e)
        {

            // Remove the SKU check here and only calculate total cost.
            generateTotalCost();

            txtQty.TextChanged -= txtQty_TextChanged;

            try
            {
                if (!string.IsNullOrWhiteSpace(txtQty.Text))
                {
                    string rawText = txtQty.Text.Replace(",", "");

                    // Skip formatting if the last character is a dot (allow intermediate input)
                    if (rawText.EndsWith("."))
                    {
                        txtQty.TextChanged += txtQty_TextChanged;
                        return;
                    }

                    // Parse the input as a double if valid (without commas)
                    double number;
                    if (double.TryParse(rawText, out number))
                    {
                        // Format the number with commas and preserve decimals
                        txtQty.Text = number.ToString("#,##0.###");

                        // Preserve the cursor position at the end
                        txtQty.SelectionStart = txtQty.Text.Length;
                    }
                }
            }
            catch (Exception ex)
            {
                txtQty.Text = "";
            }

            txtQty.TextChanged += txtQty_TextChanged;
        }

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits, one '.' character, and backspace
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8) // Allow digits and backspace
            {
                e.Handled = false;
            }
            else if (e.KeyChar == 46) // '.' character
            {
                // Allow only one '.' and not at the start of the input
                if (txtQty.Text.Contains(".") || txtQty.SelectionStart == 0)
                {
                    e.Handled = true;
                }
                else
                {
                    e.Handled = false; // Allow the first dot
                }
            }
            else
            {
                e.Handled = true; // Reject all other characters
            }
        }
            
        public void Clear()
        {
            txtItemDescription.Text = "";
            txtPricePerPc.Clear();
            txtClient.Clear();
            txtQty.Clear();
            txtTotalCost.Clear();
            txtContactNumber.Clear();
            txtItemDescription.Focus();

        }

        public void generateTotalCost()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtQty.Text) || string.IsNullOrWhiteSpace(txtItemDescription.Text))
                {
                    txtTotalCost.Text = "";
                    //txtPricePerPc.Text = "";
                }
                else
                {
                    // Parse the price per piece (removing the peso sign and trimming spaces)
                    double pricePerPc = double.Parse(txtPricePerPc.Text.Replace("₱", "").Trim());

                    // Parse the quantity from txtQty
                    double qty = double.Parse(txtQty.Text.Replace(",", "").Trim());

                    // Calculate the total cost
                    double total = pricePerPc * qty;

                    // Format total cost with peso sign and two decimal places
                    txtTotalCost.Text = "₱" + total.ToString("#,##0.00");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please select a valid item first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQty.Text = ""; // Clear the quantity on error
                //txtPricePerPc.Text = "";
                txtItemDescription.Focus();

            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtItemDescription.Text == "" || txtClient.Text == "" || txtQty.Text == "")
            {
                MessageBox.Show("Please fill up all fields", "ADD ITEM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Do you want to confirm saving this item?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                cn.Open();
                // Use parameterized queries to check for existing SKU and description
                string query = "SELECT COUNT(*) FROM tblProductMaster WHERE description = @description";
                cm = new SqlCommand(query, cn);
                cm.Parameters.AddWithValue("@description", txtItemDescription.Text);
                int count = (int)cm.ExecuteScalar(); // Get the count of matching items
                cn.Close();

                if (count <= 0)
                {
                    MessageBox.Show("Item with this description not exists", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                    string cleanedBalance = txtQty.Text.Replace(",", "").Trim();
                int qty = int.Parse(cleanedBalance);

                cn.Open();

                string query2 = "INSERT INTO tblPurchased (date, item_description, qty, total_cost, price_per_pc, client, balance) VALUES(@date, @item_description, @qty, @total_cost, @price_per_pc, @client, @balance)";
                cm = new SqlCommand(query2, cn);
                cm.Parameters.AddWithValue("@date", currentDate.Value);
                cm.Parameters.AddWithValue("@item_description", txtItemDescription.Text);
                cm.Parameters.AddWithValue("@qty", qty);
                cm.Parameters.AddWithValue("@total_cost", double.Parse(txtTotalCost.Text.Replace("₱", "").Trim()));
                cm.Parameters.AddWithValue("@price_per_pc", double.Parse(txtPricePerPc.Text.Replace("₱", "").Trim()));
                cm.Parameters.AddWithValue("@client", txtClient.Text);
                cm.Parameters.AddWithValue("@balance", double.Parse(txtTotalCost.Text.Replace("₱", "").Trim()));
                cm.ExecuteNonQuery();
                mj_purchased.LoadRecords();
                cn.Close();
                Clear();
                MessageBox.Show("Item has been successfully added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
}

        private void txtClient_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtItemDescription_TextChanged(object sender, EventArgs e)
        {
            loadRecords();
            generateTotalCost();

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int db_qty = 0;
            string item_description = "";
            int id = 0;

            try
            {
                if (string.IsNullOrWhiteSpace(txtItemDescription.Text) || string.IsNullOrWhiteSpace(txtClient.Text) || string.IsNullOrWhiteSpace(txtQty.Text) || string.IsNullOrWhiteSpace(txtContactNumber.Text))
                {
                    MessageBox.Show("Please fill up all fields", "ADD ITEM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int qty_input = int.Parse(txtQty.Text, NumberStyles.AllowThousands, CultureInfo.InvariantCulture);

                if (qty_input <= 0)
                {
                    MessageBox.Show("Quantity cannot be 0 or negative.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate the contact number length
                if (txtContactNumber.Text.Length != 11 || !long.TryParse(txtContactNumber.Text, out _))
                {
                    MessageBox.Show("Contact number must be an 11-digit number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Do you want to confirm saving this item?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    // Use parameterized queries to check for existing SKU and description
                    string query = "SELECT COUNT(*) FROM tblProductMaster WHERE description = @description";
                    cm = new SqlCommand(query, cn);
                    cm.Parameters.AddWithValue("@description", txtItemDescription.Text);
                    int count = (int)cm.ExecuteScalar(); // Get the count of matching items
                    cn.Close();

                    if (count <= 0)
                    {
                        MessageBox.Show("Item with this description not exists", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    cn.Open();
                    // Use parameterized queries to check for existing SKU and description
                    string query2 = "SELECT COUNT(*) FROM tblPurchased WHERE client = @client";
                    cm = new SqlCommand(query2, cn);
                    cm.Parameters.AddWithValue("@client", txtClient.Text);
                    int count2 = (int)cm.ExecuteScalar(); // Get the count of matching items
                    cn.Close();

                    if (count2 >= 1)
                    {
                        MessageBox.Show($"The customer '{txtClient.Text}' already exists. Consider using a unique name like '{txtClient.Text} (Shirt)'.",
                                        "Duplicate Customer",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        return;
                    }



                    //checkQty();

                    cn.Open();
                    string query1 = "SELECT * FROM tblProductMaster WHERE description = @description";
                    cm = new SqlCommand(query1, cn);
                    cm.Parameters.AddWithValue("@description", txtItemDescription.Text);
                    dr = cm.ExecuteReader();
                    dr.Read();

                    if (dr.HasRows)
                    {
                        db_qty = int.Parse(dr["qty"].ToString());
                        item_description = dr["description"].ToString();
                        id = int.Parse(dr["id"].ToString());

                    }

                    string qtyInput = txtQty.Text.Replace(",", "");

                    if (db_qty < int.Parse(qtyInput))
                        {
                        MessageBox.Show(
                            $"Unable to proceed. The item '{item_description}' has only {db_qty} item(s) remaining in stock.",
                            "Stock Warning",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        dr.Close();
                        cn.Close();
                        return;

                    }

                    dr.Close();
                    cn.Close();

                    cn.Open();
                    string query3 = "UPDATE tblProductMaster SET qty = (qty - " + int.Parse(qtyInput) + ") WHERE id = '" + id + "'";
                    cm = new SqlCommand(query3, cn);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    cn.Open();
                    string query4 = "UPDATE tblProductMaster SET sold = (sold + " + int.Parse(qtyInput) + ") WHERE id = '" + id + "'";
                    cm = new SqlCommand(query4, cn);
                    cm.ExecuteNonQuery();
                    cn.Close();

                //cn.Open();
                //string query2 = "UPDATE tblProductMaster SET description=@description, price_per_pc=@price_per_pc, qty=@qty WHERE id=@id";
                //cm = new SqlCommand(query2, cn);
                //cm.Parameters.AddWithValue("@description", txtDescription.Text);
                //cm.Parameters.AddWithValue("@price_per_pc", parsedPricePerPc); // Use the parsed decimal value
                //cm.Parameters.AddWithValue("@qty", qty);
                //cm.Parameters.AddWithValue("@id", int.Parse(txtID.Text));
                //cm.ExecuteNonQuery();
                //cn.Close();

                    string cleanedBalance = txtQty.Text.Replace(",", "").Trim();
                    int qty = int.Parse(cleanedBalance);

                    cn.Open();

                    string query5 = "INSERT INTO tblPurchased (date, item_description, qty, total_cost, price_per_pc, client, balance, contact_number) VALUES(@date, @item_description, @qty, @total_cost, @price_per_pc, @client, @balance, @contact_number)";
                    cm = new SqlCommand(query5, cn);
                    cm.Parameters.AddWithValue("@date", currentDate.Value);
                    cm.Parameters.AddWithValue("@item_description", txtItemDescription.Text);
                    cm.Parameters.AddWithValue("@qty", qty);

                    cm.Parameters.AddWithValue("@total_cost", double.Parse(txtTotalCost.Text.Replace("₱", "").Trim()));
                    cm.Parameters.AddWithValue("@price_per_pc", double.Parse(txtPricePerPc.Text.Replace("₱", "").Trim()));
                    cm.Parameters.AddWithValue("@client", txtClient.Text);
                    cm.Parameters.AddWithValue("@balance", double.Parse(txtTotalCost.Text.Replace("₱", "").Trim()));
                    cm.Parameters.AddWithValue("@contact_number", txtContactNumber.Text);
                    cm.ExecuteNonQuery();
                    mj_purchased.LoadRecords();
                    mj_purchased.getTotalOrders();
                    cn.Close();
                    Clear();
                    MessageBox.Show($"Item has been successfully added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void txtContactNumber_KeyPress(object sender, KeyPressEventArgs e)
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

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void txtItemDescription_Click(object sender, EventArgs e)
        {

        }

        private void txtQty_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtItemDescription.Text))
            {
                {
                    MessageBox.Show("Please fill up item field first.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
        }
    }
}
