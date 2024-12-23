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
    public partial class mj_addExpense : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;

        mj_Expenses mj_expenses;

        public mj_addExpense(mj_Expenses frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            mj_expenses = frm;
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

        private void btnAddExpense_Click(object sender, EventArgs e)
        {
  
            int purchased_id = 0;
            Double total_cost = 0;
            String customer = "";

            try
            {
                if (string.IsNullOrWhiteSpace(txtDate.Text) || string.IsNullOrWhiteSpace(txtCustomer.Text) ||
                    string.IsNullOrWhiteSpace(txtExpense.Text) || string.IsNullOrWhiteSpace(txtAmount.Text))
                {
                    MessageBox.Show("Please fill up all fields", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if(Double.Parse(txtAmount.Text) <= 0)
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

                    if (count >= 1 || txtCustomer.Text == "Business Expense")
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
                        string query2 = "INSERT INTO tblExpenses (purchased_id, customer, date, expense, amount, total_cost) VALUES(@purchased_id, @customer, @date, @expense, @amount, @total_cost)";
                        cm = new SqlCommand(query2, cn);
                        cm.Parameters.AddWithValue("@purchased_id", purchased_id);
                        cm.Parameters.AddWithValue("@customer", txtCustomer.Text);
                        cm.Parameters.AddWithValue("@date", txtDate.Value);
                        cm.Parameters.AddWithValue("@expense", txtExpense.Text);
                        cm.Parameters.AddWithValue("@amount", double.Parse(txtAmount.Text));
                        cm.Parameters.AddWithValue("@total_cost", total_cost);
                        cm.ExecuteNonQuery();
                        cn.Close();

                        txtCustomer.Text = "";
                        txtExpense.Clear();
                        txtAmount.Clear();
                        txtCustomer.Focus();

                        mj_expenses.calculateExpenses();
                        mj_expenses.LoadRecords();
                        MessageBox.Show("Item has been successfully added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
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

        private void mj_addExpense_Load(object sender, EventArgs e)
        {
            LoadCustomer();
            AutoComplete();
        }

        private void txtCustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            //e.Handled = true;
        }

        private void txtDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtCustomer_TextUpdate(object sender, EventArgs e)
        {

        }

        private void txtCustomer_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int purchased_id = 0;
            Double total_cost = 0;
            String customer = "";

            try
            {
                if (string.IsNullOrWhiteSpace(txtDate.Text) || string.IsNullOrWhiteSpace(txtCustomer.Text) ||
                    string.IsNullOrWhiteSpace(txtExpense.Text) || string.IsNullOrWhiteSpace(txtAmount.Text))
                {
                    MessageBox.Show("Please fill up all fields", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (Double.Parse(txtAmount.Text) <= 0)
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

                    if (count >= 1 || txtCustomer.Text == "Business Expense")
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
                        string query2 = "INSERT INTO tblExpenses (purchased_id, customer, date, expense, amount, total_cost) VALUES(@purchased_id, @customer, @date, @expense, @amount, @total_cost)";
                        cm = new SqlCommand(query2, cn);
                        cm.Parameters.AddWithValue("@purchased_id", purchased_id);
                        cm.Parameters.AddWithValue("@customer", txtCustomer.Text);
                        cm.Parameters.AddWithValue("@date", txtDate.Value);
                        cm.Parameters.AddWithValue("@expense", txtExpense.Text);
                        cm.Parameters.AddWithValue("@amount", double.Parse(txtAmount.Text));
                        cm.Parameters.AddWithValue("@total_cost", total_cost);
                        cm.ExecuteNonQuery();
                        cn.Close();

                        txtCustomer.Text = "";
                        txtExpense.Clear();
                        txtAmount.Clear();
                        txtCustomer.Focus();

                        //mj_expenses.calculateExpenses();
                        mj_expenses.LoadRecords();
                        mj_expenses.CalculateTotalExpense();

                        MessageBox.Show("Item has been successfully added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Expense under customer not exists", "Expense Under Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void txtAmount_KeyUp(object sender, KeyEventArgs e)
        {

        }
    }
}
