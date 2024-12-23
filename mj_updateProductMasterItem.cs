using System;
using System.Collections;
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
    public partial class mj_updateProductMasterItem : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;

        mj_ProductMaster mj_product_master;

        public mj_updateProductMasterItem(mj_ProductMaster frm, int id, string description, int onHand, string pricePerPc)    
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            mj_product_master = frm;
            txtDescription.Text = description;
            txtQty.Text = onHand.ToString();
            txtPricePerPc.Text = pricePerPc;
            txtID.Text = id.ToString(); 
            
        }

        public void Clear()
        {
            txtDescription.Clear();
            txtPricePerPc.Clear();
            txtDescription.Focus();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // Clean the input price by removing unwanted characters
                string cleanedPrice = txtPricePerPc.Text.Replace("₱", "").Replace(",", "").Trim();

                // Parse the cleaned price to a numeric value
                if (!decimal.TryParse(cleanedPrice, out decimal parsedPricePerPc))
                {
                    MessageBox.Show("Please enter a valid price", "UPDATE ITEM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate input fields
                if (string.IsNullOrWhiteSpace(txtDescription.Text) || string.IsNullOrWhiteSpace(txtPricePerPc.Text))
                {
                    MessageBox.Show("Please fill up all fields", "UPDATE ITEM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Ensure the price is greater than zero
                if (parsedPricePerPc <= 0)
                {
                    MessageBox.Show("Price cannot be 0 or negative.", "Invalid Price", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Confirm update
                if (MessageBox.Show("Do you want to confirm updating this item?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    string query2 = "UPDATE tblProductMaster SET description=@description, price_per_pc=@price_per_pc WHERE id=@id";
                    cm = new SqlCommand(query2, cn);
                    cm.Parameters.AddWithValue("@description", txtDescription.Text);
                    cm.Parameters.AddWithValue("@price_per_pc", parsedPricePerPc); // Use the parsed decimal value
                    cm.Parameters.AddWithValue("@id", int.Parse(txtID.Text));
                    cm.ExecuteNonQuery();
                    cn.Close();

                    MessageBox.Show("Item has been successfully updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
                    mj_product_master.LoadRecords();
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }


        private void mj_updateProductMasterItem_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtDescription;
        }

        private void txtPricePerPc_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits, one '.' character, and backspace
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8) // Allow digits and backspace
            {
                e.Handled = false;
            }
            else if (e.KeyChar == 46) // '.' character
            {
                // Allow only one '.' and not at the start of the input
                if (txtPricePerPc.Text.Contains(".") || txtPricePerPc.SelectionStart == 0)
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

        private void txtPricePerPc_TextChanged(object sender, EventArgs e)
        {
            txtPricePerPc.TextChanged -= txtPricePerPc_TextChanged;

            try
            {
                if (!string.IsNullOrWhiteSpace(txtPricePerPc.Text))
                {
                    string rawText = txtPricePerPc.Text.Replace(",", "");

                    // Skip formatting if the last character is a dot (allow intermediate input)
                    if (rawText.EndsWith("."))
                    {
                        txtPricePerPc.TextChanged += txtPricePerPc_TextChanged;
                        return;
                    }

                    // Parse the input as a double if valid (without commas)
                    double number;
                    if (double.TryParse(rawText, out number))
                    {
                        // Format the number with commas and preserve decimals
                        txtPricePerPc.Text = number.ToString("#,##0.###");

                        // Preserve the cursor position at the end
                        txtPricePerPc.SelectionStart = txtPricePerPc.Text.Length;
                    }
                }
            }
            catch (Exception ex)
            {
                txtPricePerPc.Text = "";
            }

            txtPricePerPc.TextChanged += txtPricePerPc_TextChanged;
        }

        private void txtID_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Clean the input price by removing unwanted characters
                string cleanedPrice = txtPricePerPc.Text.Replace("₱", "").Replace(",", "").Trim();

                // Parse the cleaned price to a numeric value
                if (!decimal.TryParse(cleanedPrice, out decimal parsedPricePerPc))
                {
                    MessageBox.Show("Please enter a valid price", "UPDATE ITEM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate input fields
                if (string.IsNullOrWhiteSpace(txtDescription.Text) || string.IsNullOrWhiteSpace(txtPricePerPc.Text) || string.IsNullOrWhiteSpace(txtQty.Text))
                {
                    MessageBox.Show("Please fill up all fields", "UPDATE ITEM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int qty = int.Parse(txtQty.Text.Replace(",", "").Trim());

            // Ensure the price is greater than zero
                if (parsedPricePerPc <= 0)
                    {
                        MessageBox.Show("Price cannot be 0 or negative.", "Invalid Price", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                if (qty <= 0)
                {
                    MessageBox.Show("Quantity cannot be 0 or negative.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Confirm update
                if (MessageBox.Show("Do you want to confirm updating this item?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    string query2 = "UPDATE tblProductMaster SET description=@description, price_per_pc=@price_per_pc, qty=@qty WHERE id=@id";
                    cm = new SqlCommand(query2, cn);
                    cm.Parameters.AddWithValue("@description", txtDescription.Text);
                    cm.Parameters.AddWithValue("@price_per_pc", parsedPricePerPc); // Use the parsed decimal value
                    cm.Parameters.AddWithValue("@qty", qty);
                    cm.Parameters.AddWithValue("@id", int.Parse(txtID.Text));
                    cm.ExecuteNonQuery();
                    cn.Close();

                    MessageBox.Show("Item has been successfully updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
                    mj_product_master.LoadRecords();
                    this.Dispose();
                }
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
            else
            {
                e.Handled = true; // Reject all other characters
            }
        }

        private void txtQty_TextChanged(object sender, EventArgs e)
        {
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
    }
}
