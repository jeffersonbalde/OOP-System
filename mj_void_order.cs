using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net.Sockets;

namespace OOP_System
{
    public partial class mj_void_order : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;

        mj_Purchased mj_purchased;

        public mj_void_order(mj_Purchased frm, string order_date_v, string item_description, string price_per, string qty, string total_cost, string customer_name, string contact_number, int id)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            mj_purchased = frm;

            order_date.Text = order_date_v;
            text_item_description.Text = item_description;
            txtPricePerPc.Text = price_per;
            txtQty.Text = qty;
            txtTotalCost.Text = total_cost;
            txt_customer_name.Text = customer_name;
            txt_contact_number.Text = contact_number;
            txtID.Text = id.ToString();
        }

        private void mj_void_order_Load(object sender, EventArgs e)
        {
            this.ActiveControl = voidReason;
            cboVoidCondition.Items.AddRange(new string[] { " ", "No Payment (Return to Stock)", "With Payment (Added to Sales)" });
            cboVoidCondition.SelectedIndex = 0; // Default to "All"
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int item_description_id = 0;

            try
            {
                if (string.IsNullOrWhiteSpace(voidDate.Text) || string.IsNullOrWhiteSpace(voidReason.Text) || string.IsNullOrWhiteSpace(voidedBy.Text) || string.IsNullOrWhiteSpace(cboVoidCondition.Text))
                {
                    MessageBox.Show("Please fill up all fields", "Void Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string selectedStatus = cboVoidCondition.SelectedItem.ToString();

                if (selectedStatus == "With Payment (Added to Sales)" && string.IsNullOrWhiteSpace(txtTotalPayment.Text))
                {
                    MessageBox.Show("Please fill up payment amount field", "Void Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Do you want to confirm saving this item?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string cleanedBalance = txtQty.Text.Replace(",", "").Trim();
                    int qty = int.Parse(cleanedBalance);


                    cn.Open();
                    string query3 = "UPDATE tblPurchased SET status=@status WHERE id=@id";
                    cm = new SqlCommand(query3, cn);
                    cm.Parameters.AddWithValue("@status", "Voided");
                    cm.Parameters.AddWithValue("@id", txtID.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();


                    if (selectedStatus == "No Payment (Return to Stock)")
                    {
                        string parsedQty = txtQty.Text.Replace(",", "").Trim();
                        int qty_input = int.Parse(parsedQty);

                        cn.Open();
                        string query1 = "SELECT * FROM tblProductMaster WHERE description = @description";
                        cm = new SqlCommand(query1, cn);
                        cm.Parameters.AddWithValue("@description", text_item_description.Text);
                        dr = cm.ExecuteReader();
                        dr.Read();

                        if (dr.HasRows)
                        {
                            item_description_id = int.Parse(dr["id"].ToString());
                        }
                        dr.Close();
                        cn.Close();

                        cn.Open();
                        string query4= "UPDATE tblProductMaster SET qty = (qty + " + qty_input + ") WHERE id = '" + item_description_id + "'";
                        cm = new SqlCommand(query4,cn);
                        cm.ExecuteNonQuery();
                        cn.Close();

                        cn.Open();
                        string query5 = "UPDATE tblProductMaster SET sold = (sold - " + qty_input + ") WHERE id = '" + item_description_id + "'";
                        cm = new SqlCommand(query5, cn);
                        cm.ExecuteNonQuery();
                        cn.Close();

                        cn.Open();

                        string query2 = "INSERT INTO tblVoidProduct (transaction_id, order_date, item_description, price_per_pc, qty, total_cost, customer_name, contact_number, void_date, reason_for_void, voided_by, void_condition) VALUES(@transaction_id, @order_date, @item_description, @price_per_pc, @qty, @total_cost, @customer_name, @contact_number, @void_date, @reason_for_void, @voided_by, @void_condition)";
                        cm = new SqlCommand(query2, cn);
                        cm.Parameters.AddWithValue("@transaction_id", txtID.Text);
                        cm.Parameters.AddWithValue("@order_date", order_date.Text);
                        cm.Parameters.AddWithValue("@item_description", text_item_description.Text);
                        cm.Parameters.AddWithValue("@price_per_pc", txtPricePerPc.Text);
                        cm.Parameters.AddWithValue("@qty", txtQty.Text);
                        cm.Parameters.AddWithValue("@total_cost", txtTotalCost.Text);
                        cm.Parameters.AddWithValue("@customer_name", txt_customer_name.Text);
                        cm.Parameters.AddWithValue("@contact_number", txt_contact_number.Text);

                        cm.Parameters.AddWithValue("@void_date", voidDate.Value);
                        cm.Parameters.AddWithValue("@reason_for_void", voidReason.Text);
                        cm.Parameters.AddWithValue("@voided_by", voidedBy.Text);
                        cm.Parameters.AddWithValue("@void_condition", cboVoidCondition.Text);

                        cm.ExecuteNonQuery();
                        mj_purchased.LoadRecords();
                        mj_purchased.getTotalOrders();
                        cn.Close();

                        mj_purchased.LoadRecords();
                        MessageBox.Show($"The order for {text_item_description.Text} has been successfully voided.\n\nTotal quantity back to inventory: {txtQty.Text}.\nAll changes have been saved.", "Void Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Dispose();
                    }

                    if (selectedStatus == "With Payment (Added to Sales)")
                    {

                        cn.Open();

                        string query2 = "INSERT INTO tblVoidProduct (transaction_id, order_date, item_description, price_per_pc, qty, total_cost, customer_name, contact_number, void_date, reason_for_void, voided_by, void_condition, payment_amount) VALUES(@transaction_id, @order_date, @item_description, @price_per_pc, @qty, @total_cost, @customer_name, @contact_number, @void_date, @reason_for_void, @voided_by, @void_condition, @payment_amount)";
                        cm = new SqlCommand(query2, cn);
                        cm.Parameters.AddWithValue("@transaction_id", txtID.Text);
                        cm.Parameters.AddWithValue("@order_date", order_date.Text);
                        cm.Parameters.AddWithValue("@item_description", text_item_description.Text);
                        cm.Parameters.AddWithValue("@price_per_pc", txtPricePerPc.Text);
                        cm.Parameters.AddWithValue("@qty", txtQty.Text);
                        cm.Parameters.AddWithValue("@total_cost", txtTotalCost.Text);
                        cm.Parameters.AddWithValue("@customer_name", txt_customer_name.Text);
                        cm.Parameters.AddWithValue("@contact_number", txt_contact_number.Text);

                        cm.Parameters.AddWithValue("@void_date", voidDate.Value);
                        cm.Parameters.AddWithValue("@reason_for_void", voidReason.Text);
                        cm.Parameters.AddWithValue("@voided_by", voidedBy.Text); 
                        cm.Parameters.AddWithValue("@void_condition", cboVoidCondition.Text);
                        cm.Parameters.AddWithValue("@payment_amount", Double.Parse(txtTotalPayment.Text.Replace(",", "").Trim()));

                        cm.ExecuteNonQuery();
                        mj_purchased.LoadRecords();
                        mj_purchased.getTotalOrders();
                        cn.Close();

                        mj_purchased.LoadRecords();
                        MessageBox.Show($"The order for {text_item_description.Text} has been successfully voided.\n\nTotal payment added to sales: ₱{txtTotalPayment.Text}.\nAll changes have been saved.", "Void Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Dispose();
                    }
                }
        }   
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
}

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            txtTotalPayment.TextChanged -= textBox1_TextChanged;

            try
            {
                if (!string.IsNullOrWhiteSpace(txtTotalPayment.Text))
                {
                    string rawText = txtTotalPayment.Text.Replace(",", "");

                    // Skip formatting if the last character is a dot (allow intermediate input)
                    if (rawText.EndsWith("."))
                    {
                        txtTotalPayment.TextChanged += textBox1_TextChanged;
                        return;
                    }

                    // Parse the input as a double if valid (without commas)
                    double number;
                    if (double.TryParse(rawText, out number))
                    {
                        // Format the number with commas and preserve decimals
                        txtTotalPayment.Text = number.ToString("#,##0.###");

                        // Preserve the cursor position at the end
                        txtTotalPayment.SelectionStart = txtTotalPayment.Text.Length;
                    }
                }
            }
            catch (Exception ex)
            {
                txtTotalPayment.Text = "";
            }

            txtTotalPayment.TextChanged += textBox1_TextChanged;
        }

        private void cboVoidCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboVoidCondition.SelectedItem != null)
            {
                string selectedStatus = cboVoidCondition.SelectedItem.ToString();

                if (selectedStatus == "No Payment (Return to Stock)")
                {
                    lblTotalPayment.Visible = false;
                    txtTotalPayment.Visible = false;
                }
                else if (selectedStatus == "With Payment (Added to Sales)")
                {
                    lblTotalPayment.Visible = true;
                    txtTotalPayment.Visible = true;
                }
                else
                {
                    lblTotalPayment.Visible = false;
                    txtTotalPayment.Visible = false;
                }
            }
        }

        private void txtTotalPayment_KeyPress(object sender, KeyPressEventArgs e)
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
    }
}
