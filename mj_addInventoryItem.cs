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
    public partial class mj_addInventoryItem : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;

        public mj_addInventoryItem()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        private void mj_addInventoryItem_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtSKU;
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
                if (txtQtyReceived.Text.Contains(".") || txtQtyReceived.SelectionStart == 0)
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
            txtQtyReceived.TextChanged -= txtPricePerPc_TextChanged;

            try
            {
                if (!string.IsNullOrWhiteSpace(txtQtyReceived.Text))
                {
                    string rawText = txtQtyReceived.Text.Replace(",", "");

                    // Skip formatting if the last character is a dot (allow intermediate input)
                    if (rawText.EndsWith("."))
                    {
                        txtQtyReceived.TextChanged += txtPricePerPc_TextChanged;
                        return;
                    }

                    // Parse the input as a double if valid (without commas)
                    double number;
                    if (double.TryParse(rawText, out number))
                    {
                        // Format the number with commas and preserve decimals
                        txtQtyReceived.Text = number.ToString("#,##0.###");

                        // Preserve the cursor position at the end
                        txtQtyReceived.SelectionStart = txtQtyReceived.Text.Length;
                    }
                }
            }
            catch (Exception ex)
            {
                txtQtyReceived.Text = "";
            }

            txtQtyReceived.TextChanged += txtPricePerPc_TextChanged;
        }

        private void txtCurrentCost_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits, one '.' character, and backspace
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8) // Allow digits and backspace
            {
                e.Handled = false;
            }
            else if (e.KeyChar == 46) // '.' character
            {
                // Allow only one '.' and not at the start of the input
                if (txtCurrentCost.Text.Contains(".") || txtCurrentCost.SelectionStart == 0)
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

        private void txtCurrentCost_TextChanged(object sender, EventArgs e)
        {
            txtCurrentCost.TextChanged -= txtCurrentCost_TextChanged;

            try
            {
                if (!string.IsNullOrWhiteSpace(txtCurrentCost.Text))
                {
                    string rawText = txtCurrentCost.Text.Replace(",", "");

                    // Skip formatting if the last character is a dot (allow intermediate input)
                    if (rawText.EndsWith("."))
                    {
                        txtCurrentCost.TextChanged += txtCurrentCost_TextChanged;
                        return;
                    }

                    // Parse the input as a double if valid (without commas)
                    double number;
                    if (double.TryParse(rawText, out number))
                    {
                        // Format the number with commas and preserve decimals
                        txtCurrentCost.Text = number.ToString("#,##0.###");

                        // Preserve the cursor position at the end
                        txtCurrentCost.SelectionStart = txtCurrentCost.Text.Length;
                    }
                }
            }
            catch (Exception ex)
            {
                txtCurrentCost.Text = "";
            }

            txtCurrentCost.TextChanged += txtCurrentCost_TextChanged;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtSKU.Text) || string.IsNullOrWhiteSpace(txtDescription.Text) ||
                    string.IsNullOrWhiteSpace(txtQtyReceived.Text) || string.IsNullOrWhiteSpace(txtCurrentCost.Text))
                {
                    MessageBox.Show("Please fill up all fields", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                cn.Open();

                // First, check if the SKU already exists
                string skuQuery = "SELECT COUNT(*) FROM tblInventory WHERE SKU = @sku";
                cm = new SqlCommand(skuQuery, cn);
                cm.Parameters.AddWithValue("@sku", txtSKU.Text);
                int skuCount = (int)cm.ExecuteScalar();

                // Check if the description already exists
                string descriptionQuery = "SELECT COUNT(*) FROM tblInventory WHERE description = @description";
                cm = new SqlCommand(descriptionQuery, cn);
                cm.Parameters.AddWithValue("@description", txtDescription.Text);
                int descriptionCount = (int)cm.ExecuteScalar();

                cn.Close();

                if (skuCount > 0)
                {
                    MessageBox.Show("An item with this SKU already exists", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (descriptionCount > 0)
                {
                    MessageBox.Show("An item with this description already exists", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (MessageBox.Show("Do you want to confirm saving this item?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    string query2 = "INSERT INTO tblInventory (SKU, description, received, current_cost) VALUES(@sku, @description, @received, @current_cost)";
                    cm = new SqlCommand(query2, cn);
                    cm.Parameters.AddWithValue("@sku", txtSKU.Text);
                    cm.Parameters.AddWithValue("@description", txtDescription.Text);
                    cm.Parameters.AddWithValue("@received", double.Parse(txtQtyReceived.Text));
                    cm.Parameters.AddWithValue("@current_cost", double.Parse(txtCurrentCost.Text));
                    cm.ExecuteNonQuery();
                    cn.Close();

                    txtSKU.Clear();
                    txtDescription.Clear();
                    txtCurrentCost.Clear();
                    txtQtyReceived.Clear();
                    txtSKU.Focus();

                    //mj_productmaster.LoadRecords();
                    MessageBox.Show("Item has been successfully added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
