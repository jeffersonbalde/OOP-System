using System;
using System.Data.SqlClient;
using System.Net.Sockets;
using System.Windows.Forms;

namespace OOP_System
{
    public partial class mj_addDownPayment : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;

        mj_downPayment mj_downpayment;

        Double check_balance = 0;
        String paid = "PAID";

        public mj_addDownPayment(mj_downPayment frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            mj_downpayment = frm;
        }

        public void AutoComplete()
        {
            try
            {
                cn.Open();
                string query = @"SELECT * FROM tblPurchased WHERE status NOT IN ('Voided', 'Paid') AND downpayment_status = 'Pending';";
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
                string query = @"SELECT * FROM tblPurchased WHERE status NOT IN ('Voided', 'Paid') AND downpayment_status = 'Pending';";
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

        public void calculateCurrentBalances()
        {
            double balances = 0;

            try
            {
                // Open the connection
                cn.Open();

                // Check if the customer has pending purchases
                string query1 = @"SELECT COUNT(*) 
                          FROM tblPurchased 
                          WHERE status NOT IN ('Voided', 'Paid') 
                            AND downpayment_status = 'Pending'
                            AND client = @client;"; // Use '=' for exact match
                cm = new SqlCommand(query1, cn);
                cm.Parameters.AddWithValue("@client", txtCustomer.Text.Trim());

                int count = (int)cm.ExecuteScalar();
                cn.Close();

                // If purchases exist for this customer
                if (count > 0)
                {
                    txtAmountPid.Enabled = true;
                    cboRemarks.Enabled = true;
                    dtDatePaid.Enabled = true;

                    cn.Open();

                    // Fetch the balances for the specific customer
                    string query = @"SELECT balance 
                             FROM tblPurchased 
                             WHERE client = @client 
                               AND status NOT IN ('Voided', 'Paid') 
                               AND downpayment_status = 'Pending';";
                    cm = new SqlCommand(query, cn);
                    cm.Parameters.AddWithValue("@client", txtCustomer.Text.Trim());
                    dr = cm.ExecuteReader();

                    while (dr.Read())
                    {
                        balances += dr["balance"] != DBNull.Value ? Convert.ToDouble(dr["balance"]) : 0;
                    }
                    dr.Close();
                    cn.Close();

                    // Format and display the total balance
                    txtCurrentBalances.Text = "₱" + balances.ToString("#,##0.00");
                }
                else
                {
                    // Reset UI components if no purchases found
                    txtCurrentBalances.Text = "";
                    txtAmountPid.Enabled = false;
                    cboRemarks.Enabled = false;
                    dtDatePaid.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }


        public void checkCustomerExists()
        {

            try
            {
                cn.Open();
                // Use parameterized queries to check for existing SKU and description
                string query1 = "SELECT COUNT(*) FROM tblPurchased WHERE client = @client";
                cm = new SqlCommand(query1, cn);
                cm.Parameters.AddWithValue("@client", txtCustomer.Text);
                int count = (int)cm.ExecuteScalar(); // Get the count of matching items
                cn.Close();

                if (count <= 0)
                {
                    MessageBox.Show("Customer not exists", "Customer Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCustomer.Text = "";
                    return;
                }

            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }


        public void generateRemainingBalance()
        {
            try
            {
                string balance = txtCurrentBalances.Text;

                // Remove '₱' and '.' from the price and trim '00' if present
                balance = balance.Replace("₱", "")   // Remove currency symbol
                                       .Replace(".", ""); // Remove dot
                if (balance.EndsWith("00"))            // Remove trailing '00'
                {
                    balance = balance.Substring(0, balance.Length - 2);
                }

                // Parse the price per piece (removing the peso sign and trimming spaces)
                double balanceParsed = double.Parse(balance);

                // Parse the quantity from txtQty
                double amount_paid = double.Parse(txtAmountPid.Text.Replace(",", "").Trim());

                // Calculate the total cost
                double remain_balance = balanceParsed - amount_paid;

                // Format total cost with peso sign and two decimal places
                txtBalanceRemaining.Text = "₱" + remain_balance.ToString("#,##0.00");
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Please select a valid item first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //txtQty.Text = ""; // Clear the quantity on error
                ////txtPricePerPc.Text = "";
                //txtItemDescription.Focus();
                txtBalanceRemaining.Text = "";

            }
        }


        private void txtTypeOfPurchased_TextChanged(object sender, EventArgs e)
        {

        }


        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void mj_addDownPayment_Load(object sender, EventArgs e)
        {
            AutoComplete();
            LoadCustomer();

            cboRemarks.Items.AddRange(new string[] { "", "Initial down payment", "Paid in full" });
            cboRemarks.SelectedIndex = 0; // Default to "All"
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            generateRemainingBalance();

            txtAmountPid.TextChanged -= textBox1_TextChanged;

            try
            {
                if (!string.IsNullOrWhiteSpace(txtAmountPid.Text))
                {
                    string rawText = txtAmountPid.Text.Replace(",", "");

                    // Skip formatting if the last character is a dot (allow intermediate input)
                    if (rawText.EndsWith("."))
                    {
                        txtAmountPid.TextChanged += textBox1_TextChanged;
                        return;
                    }

                    // Parse the input as a double if valid (without commas)
                    double number;
                    if (double.TryParse(rawText, out number))
                    {
                        // Format the number with commas and preserve decimals
                        txtAmountPid.Text = number.ToString("#,##0.###");

                        // Preserve the cursor position at the end
                        txtAmountPid.SelectionStart = txtAmountPid.Text.Length;
                    }
                }
            }
            catch (Exception ex)
            {
                txtAmountPid.Text = "";
            }

            txtAmountPid.TextChanged += textBox1_TextChanged;
        }

        private void txtAmountPid_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8) // Allow digits and backspace
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true; // Reject all other characters
            }

        }

        private void txtCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            calculateCurrentBalances();
        }

        private void txtCustomer_TextChanged(object sender, EventArgs e)
        {
            calculateCurrentBalances();
        }

        private void txtCustomer_MouseLeave(object sender, EventArgs e)
        {
            //checkCustomerExists();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //checkCustomerExists();

                cn.Open();

                // Check if the customer has pending purchases
                string query1 = @"SELECT COUNT(*) 
                          FROM tblPurchased 
                          WHERE status NOT IN ('Voided', 'Paid') 
                            AND downpayment_status = 'Pending'
                            AND client = @client;"; // Use '=' for exact match
                cm = new SqlCommand(query1, cn);
                cm.Parameters.AddWithValue("@client", txtCustomer.Text.Trim());

                int count = (int)cm.ExecuteScalar();
                cn.Close();

                if (count <= 0)
                {
                    MessageBox.Show("Customer not exists", "Customer Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCustomer.Text = "";
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtCustomer.Text) || string.IsNullOrWhiteSpace(txtCurrentBalances.Text) || string.IsNullOrWhiteSpace(txtAmountPid.Text) || string.IsNullOrWhiteSpace(txtBalanceRemaining.Text) || string.IsNullOrWhiteSpace(cboRemarks.Text))
                {
                    MessageBox.Show("Please fill up all fields", "Add Down Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                string amount_paid = txtAmountPid.Text.Replace("₱", "").Replace(",", "").Trim();
                double amount = Double.Parse(amount_paid);

                string remaining_balance = txtBalanceRemaining.Text.Replace("₱", "").Replace(",", "").Trim();
                double balance = Double.Parse(remaining_balance);

                if (amount == 0)
                {
                    MessageBox.Show("Amount cannot be 0.", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if(balance < 0)
                {
                    MessageBox.Show("Amount exceeds the balance.", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Do you want to confirm saving this item?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    double amount_paid2 = double.Parse(txtAmountPid.Text.Replace(",", "").Trim());
                    double remain_balance = Double.Parse(txtBalanceRemaining.Text.Replace("₱", "").Replace(",", "").Trim());

                    cn.Open();
                    string query2 = "INSERT INTO tlbDownPayment (customer_name, date_paid, amount_paid, remarks) VALUES(@customer_name,  @date_paid, @amount_paid, @remarks)";
                    cm = new SqlCommand(query2, cn);
                    cm.Parameters.AddWithValue("@customer_name", txtCustomer.Text);
                    cm.Parameters.AddWithValue("@date_paid", dtDatePaid.Value);
                    cm.Parameters.AddWithValue("@amount_paid", amount_paid2);
                    cm.Parameters.AddWithValue("@remarks", cboRemarks.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();


                    cn.Open();
                    string query6 = "UPDATE tblPurchased SET balance=@balance WHERE client LIKE @client";
                    cm = new SqlCommand(query6, cn);
                    cm.Parameters.AddWithValue("@balance", remain_balance);
                    cm.Parameters.AddWithValue("@client", txtCustomer.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    cn.Open();
                    string query9 = "UPDATE tblPurchased SET status=@status WHERE client LIKE @client";
                    cm = new SqlCommand(query9, cn);
                    cm.Parameters.AddWithValue("@status", "Down Payment");
                    cm.Parameters.AddWithValue("@client", txtCustomer.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    if (remain_balance <= 0)
                    {
                        cn.Open();
                        string query7 = "UPDATE tblPurchased SET downpayment_status=@downpayment_status, status=@status, type_of_purchased=@type_of_purchased, date_of_payment=@date_of_payment WHERE client LIKE @client";
                        cm = new SqlCommand(query7, cn);
                        cm.Parameters.AddWithValue("@downpayment_status", "Fully Paid");
                        cm.Parameters.AddWithValue("@status", "Paid");
                        cm.Parameters.AddWithValue("@type_of_purchased", "Down Payment");
                        cm.Parameters.AddWithValue("@date_of_payment", DateTime.Now);
                        cm.Parameters.AddWithValue("@client", txtCustomer.Text);
                        cm.ExecuteNonQuery();
                        cn.Close();

                        cn.Open();
                        string query8 = "UPDATE tlbDownPayment SET status=@status WHERE customer_name LIKE @customer_name";
                        cm = new SqlCommand(query8, cn);
                        cm.Parameters.AddWithValue("@status", "Fully Paid");
                        cm.Parameters.AddWithValue("@customer_name", txtCustomer.Text);
                        cm.ExecuteNonQuery();
                        cn.Close();

                    }

                    // Build the message
                    string message = $"Amount Paid: ₱{amount_paid2:#,##0.00}\nRemaining Balance: ₱{remain_balance:#,##0.00}";
                    if (remain_balance <= 0)
                    {
                        message += "\n\nThe balance is now fully paid.";
                    }

                    MessageBox.Show(message, "Payment Recorded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AutoComplete();
                    LoadCustomer();
                    mj_downpayment.LoadRecords();

                    txtCustomer.Text = "";
                    txtCurrentBalances.Clear();
                    txtAmountPid.Clear();
                    txtBalanceRemaining.Clear();
                    cboRemarks.Text = "";
                    txtCustomer.Focus();

                }

        }
            catch (Exception ex) {

                MessageBox.Show($"{ex.Message}");
            }
}
    }
}
