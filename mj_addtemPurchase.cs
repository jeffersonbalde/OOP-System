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
    public partial class mj_addtemPurchase : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;

        mj_itemPurchase mj_item_purchase;

        public mj_addtemPurchase(mj_itemPurchase frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            mj_item_purchase = frm;
        }

        public void AutoComplete()
        {
            try
            {
                cn.Open();
                String query = "SELECT * FROM tblPurchased WHERE status <> 'Voided'";
                cm = new SqlCommand(query, cn);
                dr = cm.ExecuteReader();
                AutoCompleteStringCollection cl = new AutoCompleteStringCollection();

                //cl.Add("Business Expense");

                while (dr.Read())
                {
                    cl.Add(dr["client"].ToString());
                }

                txtCustomer.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtCustomer.AutoCompleteSource = AutoCompleteSource.CustomSource;

                txtCustomer.AutoCompleteCustomSource = cl;
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        public void LoadCustomer()
        {
            try
            {
                txtCustomer.Items.Clear();
                //txtCustomer.Items.Add("Business Expense");
                cn.Open();
                string query = "SELECT * FROM tblPurchased WHERE status <> 'Voided'";
                cm = new SqlCommand(query, cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    txtCustomer.Items.Add(dr["client"].ToString());
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

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits, one '.' character, and backspace
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8) // Allow digits and backspace
            {
                e.Handled = false;
            }
            else if (e.KeyChar == 46) // '.' character
            {
                // Allow only one '.' and not at the start of the input
                if (txtPrice.Text.Contains(".") || txtPrice.SelectionStart == 0)
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

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            txtPrice.TextChanged -= txtAmount_TextChanged;

            try
            {
                if (!string.IsNullOrWhiteSpace(txtPrice.Text))
                {
                    string rawText = txtPrice.Text.Replace(",", "");

                    // Skip formatting if the last character is a dot (allow intermediate input)
                    if (rawText.EndsWith("."))
                    {
                        txtPrice.TextChanged += txtAmount_TextChanged;
                        return;
                    }

                    // Parse the input as a double if valid (without commas)
                    double number;
                    if (double.TryParse(rawText, out number))
                    {
                        // Format the number with commas and preserve decimals
                        txtPrice.Text = number.ToString("#,##0.###");

                        // Preserve the cursor position at the end
                        txtPrice.SelectionStart = txtPrice.Text.Length;
                    }
                }
            }
            catch (Exception ex)
            {
                txtPrice.Text = "";
            }

            txtPrice.TextChanged += txtAmount_TextChanged;
        }


        private void mj_addtemPurchase_Load(object sender, EventArgs e)
        {
            AutoComplete();
            LoadCustomer();
        }

        private void txtCustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            //e.Handled = true;
        }

        private void btnAddExpense_Click(object sender, EventArgs e)
        {

            int purchased_id = 0;
            Double total_cost = 0;
            String customer = "";

            try
            {
                if (string.IsNullOrWhiteSpace(txtDate.Text) || string.IsNullOrWhiteSpace(txtCustomer.Text) 
                     || string.IsNullOrWhiteSpace(txtPrice.Text))
                {
                    MessageBox.Show("Please fill up all fields", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (Double.Parse(txtPrice.Text) <= 0)
                {
                    MessageBox.Show("Amount cannot be 0.", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Do you want to confirm saving this item?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    // Use parameterized queries to check for existing SKU and description
                    string query1 = "SELECT COUNT(*) FROM tblPurchased WHERE client = @client";
                    cm = new SqlCommand(query1, cn);
                    cm.Parameters.AddWithValue("@client", txtCustomer.Text);
                    int count = (int)cm.ExecuteScalar(); // Get the count of matching items
                    cn.Close();

                    if (count >= 1 || txtCustomer.Text == "Business Use")
                    {
                        cn.Open();
                        string query = "SELECT * FROM tblPurchased WHERE client = @client";
                        cm = new SqlCommand(query, cn);
                        cm.Parameters.AddWithValue("@client", txtCustomer.Text); // Add the ID parameter
                        dr = cm.ExecuteReader();
                        while (dr.Read())
                        {
                            purchased_id = int.Parse(dr["id"].ToString());
                            total_cost = Double.Parse(dr["total_cost"].ToString());
                            customer = dr["client"].ToString();
                        }
                        dr.Close();
                        cn.Close();

                        cn.Open();
                        string query2 = "INSERT INTO tblItemPurchase (purchased_id, customer, date, purchase, amount, total_cost) VALUES(@purchased_id, @customer, @date, @purchase, @amount, @total_cost)";
                        cm = new SqlCommand(query2, cn);
                        cm.Parameters.AddWithValue("@purchased_id", purchased_id);
                        cm.Parameters.AddWithValue("@customer", txtCustomer.Text);
                        cm.Parameters.AddWithValue("@date", txtDate.Value);
                        cm.Parameters.AddWithValue("@amount", double.Parse(txtPrice.Text));
                        cm.Parameters.AddWithValue("@total_cost", total_cost);
                        cm.ExecuteNonQuery();
                        cn.Close();

                        txtCustomer.Text = "";
                        txtPrice.Clear();
                        txtCustomer.Focus();

                        mj_item_purchase.calculateExpenses();
                        mj_item_purchase.LoadRecords();
                        MessageBox.Show("Item has been successfully added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }else
                    {
                        MessageBox.Show("Customer not exists", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
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

                if (string.IsNullOrWhiteSpace(txtPrice.Text))
                {
                    txtQty.Clear();
                    txtTotal.Clear();
                }

                if (string.IsNullOrWhiteSpace(txtQty.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
                {
                    txtTotal.Text = "";
                    //txtPricePerPc.Text = "";
                }
                else
                {
                    // Parse the price per piece (removing the peso sign and trimming spaces)
                    double pricePerPc = double.Parse(txtPrice.Text.Replace(",", "").Trim());

                    // Parse the quantity from txtQty
                    double qty = double.Parse(txtQty.Text.Replace(",", "").Trim());

                    // Calculate the total cost
                    double total = pricePerPc * qty;

                    // Format total cost with peso sign and two decimal places
                    txtTotal.Text = "₱" + total.ToString("#,##0.00");
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Please select a valid item first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQty.Text = ""; // Clear the quantity on error
                //txtPricePerPc.Text = "";
                //txtItemDescription.Focus();

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int purchased_id = 0;
            Double total_cost = 0;
            String customer = "";

            try
            {
                if (string.IsNullOrWhiteSpace(txtDate.Text) || string.IsNullOrWhiteSpace(txtCustomer.Text) || string.IsNullOrWhiteSpace(txtPurchaseDescription.Text) || string.IsNullOrWhiteSpace(txtPrice.Text) || string.IsNullOrWhiteSpace(txtTotal.Text) || string.IsNullOrWhiteSpace(txtCustomer.Text)
                     || string.IsNullOrWhiteSpace(txtPrice.Text))
                {
                    MessageBox.Show("Please fill up all fields", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Parse price (if required)
                double price = double.Parse(txtPrice.Text.Replace(",", ""), CultureInfo.InvariantCulture);

                if (price <= 0)
                {
                    MessageBox.Show("Price cannot be 0 or negative.", "Invalid Price", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Parse quantity and remove commas
                int qty = int.Parse(txtQty.Text, NumberStyles.AllowThousands, CultureInfo.InvariantCulture);

                if (qty <= 0)
                {
                    MessageBox.Show("Quantity cannot be 0 or negative.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Do you want to confirm saving this item?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    // Use parameterized queries to check for existing SKU and description
                    string query1 = "SELECT COUNT(*) FROM tblPurchased WHERE client = @client";
                    cm = new SqlCommand(query1, cn);
                    cm.Parameters.AddWithValue("@client", txtCustomer.Text);
                    int count = (int)cm.ExecuteScalar(); // Get the count of matching items
                    cn.Close();

                    if (count >= 1)
                    {

                        cn.Open();
                        string query2 = "INSERT INTO tblItemPurchase (purchase_date, purchase_under, purchase_description, purchase_qty, purchase_price, total_amount) VALUES(@purchase_date, @purchase_under, @purchase_description, @purchase_qty, @purchase_price, @total_amount)";
                        cm = new SqlCommand(query2, cn);
                        cm.Parameters.AddWithValue("@purchase_date", txtDate.Value);
                        cm.Parameters.AddWithValue("@purchase_under", txtCustomer.Text);
                        cm.Parameters.AddWithValue("@purchase_description", txtPurchaseDescription.Text);
                        cm.Parameters.AddWithValue("@purchase_qty", int.Parse(txtQty.Text.Replace(",", "").Trim()));
                        cm.Parameters.AddWithValue("@purchase_price", Double.Parse(txtPrice.Text.Replace(",", "").Trim()));
                        cm.Parameters.AddWithValue("@total_amount", Double.Parse(txtTotal.Text.Replace("₱", "").Trim()));
                        cm.ExecuteNonQuery();
                        cn.Close();

                        txtCustomer.Text = "";
                        txtPurchaseDescription.Clear();
                        txtQty.Clear();
                        txtPrice.Clear();
                        txtTotal.Clear();
                        txtCustomer.Focus();

                        

                        mj_item_purchase.calculateExpenses();
                        
                        mj_item_purchase.LoadRecords();
                        mj_item_purchase.CalculateTotalExpense();
                        MessageBox.Show("Item has been successfully added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Purchase under customer not exists", "Purchase Under Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
        }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
}

        private void txtAmount_TextChanged_1(object sender, EventArgs e)
        {

            generateTotalCost();

            txtPrice.TextChanged -= txtAmount_TextChanged;

            try
            {
                if (!string.IsNullOrWhiteSpace(txtPrice.Text))
                {
                    string rawText = txtPrice.Text.Replace(",", "");

                    // Skip formatting if the last character is a dot (allow intermediate input)
                    if (rawText.EndsWith("."))
                    {
                        txtPrice.TextChanged += txtAmount_TextChanged;
                        return;
                    }

                    // Parse the input as a double if valid (without commas)
                    double number;
                    if (double.TryParse(rawText, out number))
                    {
                        // Format the number with commas and preserve decimals
                        txtPrice.Text = number.ToString("#,##0.###");

                        // Preserve the cursor position at the end
                        txtPrice.SelectionStart = txtPrice.Text.Length;
                    }
                }
            }
            catch (Exception ex)
            {
                txtPrice.Text = "";
            }

            txtPrice.TextChanged += txtAmount_TextChanged;
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtQty_TextChanged(object sender, EventArgs e)
        {

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
            else
            {
                e.Handled = true; // Reject all other characters
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
