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
    public partial class mj_updateDownPayment : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;

        mj_downPayment mj_downpayment;

        public mj_updateDownPayment(mj_downPayment frm, String Customer, String total_cost, DateTime date, String mop, Double amount, int id)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());

            mj_downpayment = frm;
            txtCustomer.Text = Customer;
            txtTotalCost.Text = total_cost;
            txtDateOfDownPayment.Value = date;
            txtModeOfPayment.Text = mop;
            txtAmount.Text = amount.ToString();
            txtID.Text = id.ToString();
            
        }

        private void mj_updateDownPayment_Load(object sender, EventArgs e)
        {

        }

        private void txtModeOfPayment_TextChanged(object sender, EventArgs e)
        {

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
                if (txtAmount.Text.Contains(".") || txtAmount.SelectionStart == 0)
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
            txtAmount.TextChanged -= txtAmount_TextChanged;

            try
            {
                if (!string.IsNullOrWhiteSpace(txtAmount.Text))
                {
                    string rawText = txtAmount.Text.Replace(",", "");

                    // Skip formatting if the last character is a dot (allow intermediate input)
                    if (rawText.EndsWith("."))
                    {
                        txtAmount.TextChanged += txtAmount_TextChanged;
                        return;
                    }

                    // Parse the input as a double if valid (without commas)
                    double number;
                    if (double.TryParse(rawText, out number))
                    {
                        // Format the number with commas and preserve decimals
                        txtAmount.Text = number.ToString("#,##0.###");

                        // Preserve the cursor position at the end
                        txtAmount.SelectionStart = txtAmount.Text.Length;
                    }
                }
            }
            catch (Exception ex)
            {
                txtAmount.Text = "";
            }

            txtAmount.TextChanged += txtAmount_TextChanged;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Double t_amount = 0;
            Double total_cost = 0;
            Double u_balance = 0;
            Double e_balance = 0;
            Double n_balance = 0;
            Double check_balance = 0;
            int purchased_id = 0;
            String customer = "";
            String paid = "PAID";

            string amount = txtAmount.Text.Replace(",", "").Trim();

            double parsedAmount = double.Parse(amount);

            if (parsedAmount == 0)
            {
                MessageBox.Show("Amount cannot be 0.", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(txtCustomer.Text) || string.IsNullOrWhiteSpace(txtTotalCost.Text) || string.IsNullOrWhiteSpace(txtDateOfDownPayment.Text) || string.IsNullOrWhiteSpace(txtModeOfPayment.Text) || string.IsNullOrWhiteSpace(txtAmount.Text))
                {
                    MessageBox.Show("Please fill up all fields", "UPDATE ITEM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Do you want to confirm updating this item?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    string query4 = "SELECT * FROM tlbDownPayment WHERE id = @id";
                    cm = new SqlCommand(query4, cn);
                    cm.Parameters.AddWithValue("@id", txtID.Text); // Add the ID parameter
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        purchased_id = int.Parse(dr["purchased_id"].ToString()); // Sum up down payments
                        total_cost = Double.Parse(dr["total_cost"].ToString()); // Sum up down paymentsus
                        customer = dr["customer"].ToString();
                    }
                    dr.Close();
                    cn.Close();

                    cn.Open();
                    string query6 = "SELECT * FROM tblBalance WHERE purchased_id = @id";
                    cm = new SqlCommand(query6, cn);
                    cm.Parameters.AddWithValue("@id", purchased_id); // Add the ID parameter
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        e_balance = Double.Parse(dr["balance"].ToString()); // Sum up down payments
                    }
                    dr.Close();
                    cn.Close();

                    if (parsedAmount > total_cost)
                    {
                            MessageBox.Show("Overpayment detected! Adjust the amount.", "Invalid Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    cn.Open();
                    string query2 = "UPDATE tlbDownPayment SET date_of_down_payment = @date_of_down_payment, mode_of_payment=@mode_of_payment, amount=@amount WHERE id LIKE @id";
                    cm = new SqlCommand(query2, cn);
                    cm.Parameters.AddWithValue("@date_of_down_payment", txtDateOfDownPayment.Value);
                    cm.Parameters.AddWithValue("@mode_of_payment", txtModeOfPayment.Text);
                    cm.Parameters.AddWithValue("@amount", parsedAmount);
                    cm.Parameters.AddWithValue("@id", txtID.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    cn.Open();
                    string query5 = "SELECT * FROM tlbDownPayment WHERE purchased_id = @id";
                    cm = new SqlCommand(query5, cn);
                    cm.Parameters.AddWithValue("@id", purchased_id); // Add the ID parameter
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        t_amount += Double.Parse(dr["amount"].ToString()); // Sum up down payments
                    }
                    dr.Close();
                    cn.Close();

                    u_balance = total_cost - t_amount;

                    // Update existing balance
                    cn.Open();
                    string updateBalanceQuery = "UPDATE tblBalance SET balance = @balance WHERE purchased_id = @purchased_id";
                    cm = new SqlCommand(updateBalanceQuery, cn);
                    cm.Parameters.AddWithValue("@purchased_id", purchased_id);
                    cm.Parameters.AddWithValue("@balance", u_balance);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    // Update the status to "DOWN PAYMENT"
                    cn.Open();
                    string query7 = "UPDATE tblPurchased SET balance=@balance WHERE id LIKE @id";
                    cm = new SqlCommand(query7, cn);
                    cm.Parameters.AddWithValue("@balance", u_balance);
                    cm.Parameters.AddWithValue("@id", purchased_id);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    cn.Open();
                    string query8 = "SELECT * FROM tblBalance WHERE purchased_id = @id";
                    cm = new SqlCommand(query8, cn);
                    cm.Parameters.AddWithValue("@id", purchased_id); // Add the ID parameter
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        check_balance = Double.Parse(dr["balance"].ToString()); // Sum up down payments
                    }
                    dr.Close();
                    cn.Close();

                    if (check_balance == 0.00)
                    {   // Update the status to "DOWN PAYMENT"
                        cn.Open();
                        string query9 = "UPDATE tblPurchased SET status=@status, type_of_purchased=@type_of_purchased, date_of_payment=@date_of_payment WHERE id LIKE @id";
                        cm = new SqlCommand(query9, cn);
                        cm.Parameters.AddWithValue("@type_of_purchased", txtModeOfPayment.Text);
                        cm.Parameters.AddWithValue("@status", paid);
                        cm.Parameters.AddWithValue("@date_of_payment", txtDateOfDownPayment.Value);
                        cm.Parameters.AddWithValue("@id", purchased_id);
                        cm.ExecuteNonQuery();
                        cn.Close();
                    }

                    MessageBox.Show("Item has been successfully updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    mj_downpayment.LoadDownPaymentRecords();  
                    mj_downpayment.LoadRecords();
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
