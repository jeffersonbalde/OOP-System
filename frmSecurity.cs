using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;

namespace OOP_System
{
    public partial class frmSecurity : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;

        frmSoldItems formSales;
        frmAddDebt frmAdd;
        
        public frmSecurity()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());

            this.KeyPreview = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void frmSecurity_Load(object sender, EventArgs e)
        {
            //txtUser.Focus();
            //this.ActiveControl = txtUser;
            this.ActiveControl = textBoxPassword;
            panel1.BackColor = Color.FromArgb(100, 0, 0, 0);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to close the application? ", "ALL J SHOP GENERAL MERCHANDISE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)

        {
            //try
            //{
            //    bool found = false;
            //    string name = "";
            //    string role = "";

            //    cn.Open();
            //    string query = "SELECT * FROM tblUser WHERE username = @username AND password = @password";
            //    cm = new SqlCommand(query, cn);
            //    cm.Parameters.AddWithValue("@username", txtUser.Text);
            //    cm.Parameters.AddWithValue("@password", txtPass.Text);
            //    dr = cm.ExecuteReader();
            //    dr.Read();

            //    if (dr.HasRows)
            //    {
            //        found = true;
            //        name = dr["name"].ToString();
            //        role = dr["role"].ToString();
            //    }

            //    cn.Close();
            //    dr.Close();

            //    if ((found) && (role == "Admin"))
            //    {
            //        MessageBox.Show("WELCOME " + name + " ", "ALL J SHOP GENERAL MERCHANDISE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        Form1 frm = new Form1();
            //        frm.lblName.Text = name;
            //        frm.lblRole.Text = role;
            //        frm.ShowDialog();
            //        this.Dispose();

            //    }
            //    else if ((found) && (role == "Cashier"))
            //    {
            //        MessageBox.Show("WELCOME " + name + " ", "ALL J SHOP GENERAL MERCHANDISE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        frmPOS frm = new frmPOS(formSales, frmAdd);
            //        frm.lblUser.Text = name;
            //        frm.ShowDialog();
            //        this.Dispose();
            //    }
            //    else
            //    {
            //        MessageBox.Show("Username and password do not match", "ALL J SHOP GENERAL MERCHANDISE", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    cn.Close();
            //    MessageBox.Show(ex.Message, "ALL J GENERAL MERCHANDISE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}

            try
            {
                bool found = false;
                string name = "";
                string role = "";

                cn.Open();
                string query = "SELECT * FROM Pin WHERE Pin = @Pin";
                cm = new SqlCommand(query, cn);
                cm.Parameters.AddWithValue("@Pin", textBoxPassword.Text);
                dr = cm.ExecuteReader();
                dr.Read();

                if (dr.HasRows)
                {
                    found = true;
                    name = dr["Name"].ToString();
                    role = dr["Type"].ToString();
                }

                cn.Close();
                dr.Close();

                if ((found) && (role == "Admin"))
                {
                    MessageBox.Show("WELCOME " + name + " ");
                    Form1 frm = new Form1();
                    frm.lblName.Text = name;
                    frm.lblRole.Text = role;
                    frm.ShowDialog();
                    this.Dispose();

                }
                else if ((found) && (role == "Cashier"))
                {
                    MessageBox.Show("WELCOME " + name + " ");
                    frmPOS frm = new frmPOS(formSales, frmAdd);
                    frm.lblUser.Text = name;
                    frm.ShowDialog();
                    this.Dispose();
                }
                else
                {
                    MessageBox.Show("Invalid pin");
                    textBoxPassword.Clear();
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void frmSecurity_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
            else if(e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to close the application? ", "ALL J SHOP GENERAL MERCHANDISE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            labelTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
            labelDate.Text = DateTime.Now.ToLongDateString();    
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn7_Click(object sender, EventArgs e)
        {
            textBoxPassword.Text += button1.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBoxPassword.Text += button2.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBoxPassword.Text += button3.Text;
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            textBoxPassword.Text += button4.Text;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBoxPassword.Text += button5.Text;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBoxPassword.Text += button6.Text;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBoxPassword.Text += button7.Text;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBoxPassword.Text += button8.Text;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBoxPassword.Text += button9.Text;
        }

        private void button0_Click(object sender, EventArgs e)
        {
            textBoxPassword.Text += button0.Text;
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            try
            {
                bool found = false;
                string name = "";
                string role = "";

                cn.Open();
                string query = "SELECT * FROM Pin WHERE Pin = @Pin";
                cm = new SqlCommand(query, cn);
                cm.Parameters.AddWithValue("@Pin", textBoxPassword.Text);
                dr = cm.ExecuteReader();
                dr.Read();

                if (dr.HasRows)
                {
                    found = true;
                    name = dr["Name"].ToString();
                    role = dr["Type"].ToString();
                }

                cn.Close();
                dr.Close();

                if ((found) && (role == "Admin"))
                {
                    MessageBox.Show("WELCOME " + name + " ");
                    Form1 frm = new Form1();
                    frm.lblName.Text = name;
                    frm.lblRole.Text = role;
                    frm.ShowDialog();
                    this.Dispose();

                }
                else if ((found) && (role == "Cashier"))
                {
                    MessageBox.Show("WELCOME " + name + " ");
                    frmPOS frm = new frmPOS(formSales, frmAdd);
                    frm.lblUser.Text = name;
                    frm.ShowDialog();
                    this.Dispose();
                }
                else
                {
                    MessageBox.Show("Invalid pin");
                    textBoxPassword.Clear();
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            int length = textBoxPassword.Text.Length;


            if (textBoxPassword.Text == "")
            {
                return;
            }
            else
            {
                textBoxPassword.Text = textBoxPassword.Text.Substring(0, length - 1);
            }
        }

        private void textBoxPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8)
            {
                //accept backspace
            }
            else if ((e.KeyChar < 48) || (e.KeyChar > 57)) //accept code 48-57 between 0-9
            {
                e.Handled = true;
            }
        }
    }
}
