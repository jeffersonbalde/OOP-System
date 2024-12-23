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
using ZXing.QrCode.Internal;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Globalization; // Add this namespace for parsing with culture info

namespace OOP_System
{
    public partial class mj_addProdductMasterItem : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;

        mj_ProductMaster mj_productmaster;

        string item = "";

        public mj_addProdductMasterItem(mj_ProductMaster frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            mj_productmaster = frm;
        }
            
        public void Clear()
        {
            txtDescription.Clear();
            txtPricePerPc.Clear();
            txtQty.Clear();
            txtDescription.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtDescription.Text) ||
                    string.IsNullOrWhiteSpace(txtPricePerPc.Text) ||
                    string.IsNullOrWhiteSpace(txtQty.Text))
                {
                    MessageBox.Show("Please fill up all fields", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Parse quantity and remove commas
                int qty = int.Parse(txtQty.Text, NumberStyles.AllowThousands, CultureInfo.InvariantCulture);

                if (qty <= 0)
                {
                    MessageBox.Show("Quantity cannot be 0 or negative.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Parse price (if required)
                double pricePerPc = double.Parse(txtPricePerPc.Text.Replace(",", ""), CultureInfo.InvariantCulture);

                if (pricePerPc <= 0)
                {
                    MessageBox.Show("Price cannot be 0 or negative.", "Invalid Price", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cn.Open();

                // Check for existing description
                string query = "SELECT COUNT(*) FROM tblProductMaster WHERE description = @description";
                cm = new SqlCommand(query, cn);
                cm.Parameters.AddWithValue("@description", txtDescription.Text);
                int count = (int)cm.ExecuteScalar();
                cn.Close();

                if (count > 0)
                {
                    MessageBox.Show("Item with this description already exists", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (MessageBox.Show("Do you want to confirm saving this item?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    string query2 = "INSERT INTO tblProductMaster (description, price_per_pc, qty) VALUES(@description, @price_per_pc, @qty)";
                    cm = new SqlCommand(query2, cn);
                    cm.Parameters.AddWithValue("@description", txtDescription.Text);
                    cm.Parameters.AddWithValue("@price_per_pc", pricePerPc);
                    cm.Parameters.AddWithValue("@qty", qty);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    Clear();
                    mj_productmaster.getTotalProducts();
                    mj_productmaster.LoadRecords();
                    MessageBox.Show("Item has been successfully added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid number in the Quantity or Price fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void mj_addProdductMasterItem_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtDescription;
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

        private void txtCostPerPc_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void mj_addProdductMasterItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAdd_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            else if (e.KeyCode == Keys.F1)
            {
                MessageBox.Show("test");
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
