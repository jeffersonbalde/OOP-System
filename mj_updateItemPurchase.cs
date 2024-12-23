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
    public partial class mj_updateItemPurchase : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;

        mj_itemPurchase mj_item_purchase;

        public mj_updateItemPurchase(mj_itemPurchase frm, DateTime purchase_date, String purchase_under, String purchase_description, String price, String qty, String total, int id)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());

            mj_item_purchase = frm;

            txtDate.Value = purchase_date;
            txtCustomer.Text = purchase_under;
            txtPurchaseDescription.Text = purchase_description;
            txtPrice.Text = price.ToString();
            txtQty.Text = qty.ToString();
            txtTotal.Text = total.ToString();
            txtId.Text = id.ToString();
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

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            //txtAmount.TextChanged -= txtAmount_TextChanged;

            //try
            //{
            //    if (!string.IsNullOrWhiteSpace(txtAmount.Text))
            //    {
            //        string rawText = txtAmount.Text.Replace(",", "");

            //        // Skip formatting if the last character is a dot (allow intermediate input)
            //        if (rawText.EndsWith("."))
            //        {
            //            txtAmount.TextChanged += txtAmount_TextChanged;
            //            return;
            //        }

            //        // Parse the input as a double if valid (without commas)
            //        double number;
            //        if (double.TryParse(rawText, out number))
            //        {
            //            // Format the number with commas and preserve decimals
            //            txtAmount.Text = number.ToString("#,##0.###");

            //            // Preserve the cursor position at the end
            //            txtAmount.SelectionStart = txtAmount.Text.Length;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    txtAmount.Text = "";
            //}

            //txtAmount.TextChanged += txtAmount_TextChanged;
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits, one '.' character, and backspace
            //if (char.IsDigit(e.KeyChar) || e.KeyChar == 8) // Allow digits and backspace
            //{
            //    e.Handled = false;
            //}
            //else if (e.KeyChar == 46) // '.' character
            //{
            //    // Allow only one '.' and not at the start of the input
            //    if (txtAmount.Text.Contains(".") || txtAmount.SelectionStart == 0)
            //    {
            //        e.Handled = true;
            //    }
            //    else
            //    {
            //        e.Handled = false; // Allow the first dot
            //    }
            //}
            //else
            //{
            //    e.Handled = true; // Reject all other characters
            //}
        }

        private void btnUpdatePurchase_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    // Remove peso sign and commas before parsing
            //    string amount = txtAmount.Text.Replace("₱", "").Replace(",", "").Trim();

            //    double parsedAmount = double.Parse(amount);

            //    if (string.IsNullOrWhiteSpace(txtDate.Text) || string.IsNullOrWhiteSpace(txtCustomer.Text) || string.IsNullOrWhiteSpace(txtPurchase.Text) || string.IsNullOrWhiteSpace(txtAmount.Text))
            //    {
            //        MessageBox.Show("Please fill up all fields", "UPDATE ITEM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return;
            //    }

            //    if (parsedAmount <= 0)
            //    {
            //        MessageBox.Show("Amount cannot be 0.", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return;
            //    }


            //    if (MessageBox.Show("Do you want to confirm updating this item?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //    {
            //        cn.Open();
            //        // Use parameterized queries to check for existing SKU and description
            //        string query1 = "SELECT COUNT(*) FROM tblPurchased WHERE client = @client";
            //        cm = new SqlCommand(query1, cn);
            //        cm.Parameters.AddWithValue("@client", txtCustomer.Text);
            //        int count = (int)cm.ExecuteScalar(); // Get the count of matching items
            //        cn.Close();

            //        if (count >= 1 || txtCustomer.Text == "Business Use")
            //        {
            //            if (txtCustomer.Text == "Business Use")
            //            {
            //                cn.Open();
            //                string query3 = "UPDATE tblItemPurchase SET customer = @customer, date = @date, purchase=@purchase, amount=@amount, total_cost=@total_cost WHERE id LIKE @id";
            //                cm = new SqlCommand(query3, cn);
            //                cm.Parameters.AddWithValue("@customer", txtCustomer.Text);
            //                cm.Parameters.AddWithValue("@date", txtDate.Value);
            //                cm.Parameters.AddWithValue("@purchase", txtPurchase.Text);
            //                cm.Parameters.AddWithValue("@amount", amount);
            //                cm.Parameters.AddWithValue("@total_cost", 0.00);
            //                cm.Parameters.AddWithValue("@id", txtID.Text);
            //                cm.ExecuteNonQuery();
            //                cn.Close();

            //                MessageBox.Show("Item has been successfully updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                mj_item_purchase.LoadRecords();
            //                mj_item_purchase.calculateExpenses();
            //                this.Dispose();
            //                return;
            //            }
            //            cn.Open();
            //            string query2 = "UPDATE tblItemPurchase SET customer = @customer, date = @date, purchase=@purchase, amount=@amount WHERE id LIKE @id";
            //            cm = new SqlCommand(query2, cn);
            //            cm.Parameters.AddWithValue("@customer", txtCustomer.Text);
            //            cm.Parameters.AddWithValue("@date", txtDate.Value);
            //            cm.Parameters.AddWithValue("@purchase", txtPurchase.Text);
            //            cm.Parameters.AddWithValue("@amount", amount);
            //            cm.Parameters.AddWithValue("@id", txtID.Text);
            //            cm.ExecuteNonQuery();
            //            cn.Close();

            //            MessageBox.Show("Item has been successfully updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            mj_item_purchase.LoadRecords();
            //            mj_item_purchase.calculateExpenses();
            //            this.Dispose();
            //        }
            //        else
            //        {
            //            MessageBox.Show("Customer not exists", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            return;
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{
            //    cn.Close();
            //    MessageBox.Show(ex.Message);
            //}
        }
            
        private void txtCustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            //e.Handled = true;
        }

        private void mj_updateItemPurchase_Load(object sender, EventArgs e)
        {
            AutoComplete();
            LoadCustomer();
        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {
            generateTotalCost();

            txtPrice.TextChanged -= txtPrice_TextChanged;

            try
            {
                if (!string.IsNullOrWhiteSpace(txtPrice.Text))
                {
                    string rawText = txtPrice.Text.Replace(",", "");

                    // Skip formatting if the last character is a dot (allow intermediate input)
                    if (rawText.EndsWith("."))
                    {
                        txtPrice.TextChanged += txtPrice_TextChanged;
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

            txtPrice.TextChanged += txtPrice_TextChanged;
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Remove peso sign and commas before parsing
                string total = txtTotal.Text.Replace("₱", "").Replace(",", "").Trim();

                double parsedAmount = double.Parse(total);

                if (string.IsNullOrWhiteSpace(txtDate.Text) || string.IsNullOrWhiteSpace(txtCustomer.Text) || string.IsNullOrWhiteSpace(txtPurchaseDescription.Text) || string.IsNullOrWhiteSpace(txtPrice.Text) || string.IsNullOrWhiteSpace(txtTotal.Text) || string.IsNullOrWhiteSpace(txtCustomer.Text)
                     || string.IsNullOrWhiteSpace(txtPrice.Text))
                {
                    MessageBox.Show("Please fill up all fields", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

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


                if (MessageBox.Show("Do you want to confirm updating this item?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                        string query2 = "UPDATE tblItemPurchase SET purchase_date = @purchase_date, purchase_under = @purchase_under, purchase_description=@purchase_description, purchase_qty=@purchase_qty, purchase_price=@purchase_price, total_amount=@total_amount WHERE id LIKE @id";
                        cm = new SqlCommand(query2, cn);
                        cm.Parameters.AddWithValue("@purchase_date", txtDate.Value);
                        cm.Parameters.AddWithValue("@purchase_under", txtCustomer.Text);
                        cm.Parameters.AddWithValue("@purchase_description", txtPurchaseDescription.Text);
                        cm.Parameters.AddWithValue("@purchase_qty", int.Parse(txtQty.Text.Replace(",", "").Trim()));
                        cm.Parameters.AddWithValue("@purchase_price", Double.Parse(txtPrice.Text.Replace(",", "").Trim()));
                        cm.Parameters.AddWithValue("@total_amount", Double.Parse(txtTotal.Text.Replace("₱", "").Trim()));
                        cm.Parameters.AddWithValue("@id", int.Parse(txtId.Text));
                        cm.ExecuteNonQuery();
                        cn.Close();

                        MessageBox.Show("Item has been successfully updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        mj_item_purchase.LoadRecords();
                        mj_item_purchase.CalculateTotalExpense();
                        //mj_item_purchase.calculateExpenses();
                        this.Dispose();
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
    }
}
