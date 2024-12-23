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
    public partial class mj_updatePurchasedItemcs : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;

        mj_Purchased mj_purchased;

        private DateTime? dateOfPayment;

        public mj_updatePurchasedItemcs(mj_Purchased frm, String item_description, Double price_per_pc, String client, int qty, String total_cost, int id, String typeOfPurchased, String contact_number)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            //this.dateOfPayment = dateOfPayment;

            mj_purchased = frm;
            //currentDate.Value = DateTime.Parse(date_time.ToString());
            txtItemDescription.Text = item_description;
            txtPricePerPc.Text = price_per_pc.ToString();
            txtClient.Text = client;
            txtQty.Text = qty.ToString();
            txtTotalCost.Text = total_cost.ToString();
            txtID.Text = id.ToString();
            txtContactNumber.Text = contact_number.ToString();

            //// Check if typeOfPurchased is null or empty, and enable/disable txtTypeOfPayment
            //if (string.IsNullOrEmpty(typeOfPurchased))
            //{
            //    txtTypeOfPurchased.Enabled = false; // Disable if empty or null
            //}
            //else
            //{
            //    txtTypeOfPurchased.Enabled = true;  // Enable if has value
            //    txtTypeOfPurchased.Text = typeOfPurchased; // Optionally, set the text
            //}

            //if (dateOfPayment.HasValue)
            //{
            //    txtDateOfPayment.Value = dateOfPayment.Value;
            //}
            //else
            //{
            //    // Set DateTimePicker to show as blank (nullable)
            //    txtDateOfPayment.CustomFormat = " ";
            //    txtDateOfPayment.Format = DateTimePickerFormat.Custom;
            //    txtDateOfPayment.Enabled = false;
            //}
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

                while (dr.Read())
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
                //MessageBox.Show("Please select a valid item first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //txtQty.Text = ""; // Clear the quantity on error
            }
        }

        private void mj_updatePurchasedItemcs_Load(object sender, EventArgs e)
        {
            loadSKUItem();
            AutoComplete();
            //button1.Focus();
            this.ActiveControl = button1;
            
        }

        private void txtPricePerPc_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtItemDescription.Text == "" || txtClient.Text == "" || txtQty.Text == "")
                {
                    MessageBox.Show("Please fill up all fields", "UPDATE ITEM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Do you want to confirm updating this item?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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


                    // Remove peso sign and commas before parsing
                    string pricePerPc = txtPricePerPc.Text.Replace("₱", "").Replace(",", "").Trim();

                    double parsedPricePerPc = double.Parse(pricePerPc);

                    string totalCostPerPc = txtTotalCost.Text.Replace("₱", "").Replace(",", "").Trim();

                    double parsedTotalCostPerPc = double.Parse(totalCostPerPc);

                    cn.Open();
                    string query2 = "UPDATE tblPurchased SET date = @date, item_description=@item_description, qty=@qty, total_cost=@total_cost, price_per_pc=@price_per_pc, client=@client, type_of_purchased=@type_of_purchased, date_of_payment=@date_of_payment WHERE id LIKE @id";
                    cm = new SqlCommand(query2, cn);
                    //cm.Parameters.AddWithValue("@date", currentDate.Value);
                    cm.Parameters.AddWithValue("@item_description", txtItemDescription.Text);
                    cm.Parameters.AddWithValue("@qty", int.Parse(txtQty.Text));
                    cm.Parameters.AddWithValue("@total_cost", parsedTotalCostPerPc);
                    cm.Parameters.AddWithValue("@price_per_pc", parsedPricePerPc);
                    cm.Parameters.AddWithValue("@client", txtClient.Text);
                    cm.Parameters.AddWithValue("@id", int.Parse(txtID.Text));
                    //cm.Parameters.AddWithValue("@type_of_purchased", txtTypeOfPurchased.Text);
                    //cm.Parameters.AddWithValue("@date_of_payment", txtDateOfPayment.Value);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    MessageBox.Show("Item has been successfully updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    mj_purchased.LoadRecords();
                    this.Dispose();
                }

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

        private void txtSKU_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadRecords();
        }

        public void loadRecords()
        {
            if (string.IsNullOrWhiteSpace(txtItemDescription.Text))
            {
                txtPricePerPc.Text = "";
                txtQty.Clear();
                txtTotalCost.Clear();
            }

            txtPricePerPc.Clear();

            try
            {
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

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtDateOfPayment_ValueChanged(object sender, EventArgs e)
        {
            //txtDateOfPayment.CustomFormat = "MM/dd/yyyy";
            //txtDateOfPayment.Format = DateTimePickerFormat.Custom;
        }

        private void txtItemDescription_TextChanged(object sender, EventArgs e)
        {
            generateTotalCost();
            loadRecords();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int db_qty = 0;
            string item_description = "";
            int id = 0;

            try
            {

                if (string.IsNullOrWhiteSpace(txtItemDescription.Text) || string.IsNullOrWhiteSpace(txtClient.Text) || string.IsNullOrWhiteSpace(txtQty.Text) || string.IsNullOrWhiteSpace(txtContactNumber.Text))
                {
                    MessageBox.Show("Please fill up all fields", "UPDATE ITEM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate the contact number length
                if (txtContactNumber.Text.Length != 11 || !long.TryParse(txtContactNumber.Text, out _))
                {
                    MessageBox.Show("Contact number must be an 11-digit number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Do you want to confirm updating this item?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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

                    if (db_qty < int.Parse(txtQty.Text))
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
                    string query3 = "UPDATE tblProductMaster SET qty = (qty - " + int.Parse(txtQty.Text) + ") WHERE id = '" + id + "'";
                    cm = new SqlCommand(query3, cn);
                    cm.ExecuteNonQuery();
                    cn.Close();




                    // Remove peso sign and commas before parsing
                    string pricePerPc = txtPricePerPc.Text.Replace("₱", "").Replace(",", "").Trim();

                    double parsedPricePerPc = double.Parse(pricePerPc);

                    string totalCostPerPc = txtTotalCost.Text.Replace("₱", "").Replace(",", "").Trim();

                    double parsedTotalCostPerPc = double.Parse(totalCostPerPc);

                    cn.Open();
                    string query2 = "UPDATE tblPurchased SET item_description=@item_description, qty=@qty, total_cost=@total_cost, price_per_pc=@price_per_pc, client=@client, contact_number=@contact_number WHERE id LIKE @id";
                    cm = new SqlCommand(query2, cn);
                    //cm.Parameters.AddWithValue("@date", currentDate.Value);
                    cm.Parameters.AddWithValue("@item_description", txtItemDescription.Text);
                    cm.Parameters.AddWithValue("@qty", int.Parse(txtQty.Text));
                    cm.Parameters.AddWithValue("@total_cost", parsedTotalCostPerPc);
                    cm.Parameters.AddWithValue("@price_per_pc", parsedPricePerPc);
                    cm.Parameters.AddWithValue("@client", txtClient.Text);
                    cm.Parameters.AddWithValue("@id", int.Parse(txtID.Text));
                    cm.Parameters.AddWithValue("@contact_number", txtContactNumber.Text);
                    //cm.Parameters.AddWithValue("@type_of_purchased", txtTypeOfPurchased.Text);
                    //cm.Parameters.AddWithValue("@date_of_payment", txtDateOfPayment.Value);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    MessageBox.Show("Item has been successfully updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    mj_purchased.LoadRecords();
                    this.Dispose();
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
