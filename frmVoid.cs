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
    public partial class frmVoid : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();

        frmCancelDetails f;

        string adminUsername = "";
        string adminPassword = "";

        string adminPin;

        public frmVoid(frmCancelDetails frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            f = frm;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void frmVoid_Load(object sender, EventArgs e)
        {
            panel2.BackColor = Color.FromArgb(100, 0, 0, 0);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if ((txtUser.Text != String.Empty) && (txtPass.Text != String.Empty))
            {
                try
                {
                    cn.Open();
                    string query1 = "SELECT * FROM Pin WHERE role LIKE 'Admin'";
                    cm = new SqlCommand(query1, cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        //adminUsername = dr["username"].ToString();
                        //adminPassword = dr["password"].ToString();

                        adminPin = dr["Pin"].ToString();
                    }
                    dr.Close();
                    cn.Close();
                }
                catch (Exception ex)
                {
                    cn.Close();
                    MessageBox.Show(ex.Message);
                }

                if(adminUsername != txtUser.Text)
                {
                    MessageBox.Show("Incorrect username");
                    return;
                }

                if (adminPassword != txtPass.Text)
                {
                    MessageBox.Show("Incorrect password");
                    return;
                }

                if(adminPin != textBoxPassword.Text)
                {
                    MessageBox.Show("The PIN is incorrect. Try again.");
                    return;
                }

                SaveCancelOrder();
                if (f.cboAction.Text == "Yes")
                {
                    UpdateData("UPDATE tblproduct SET qty = qty + " + int.Parse(f.txtCancelQty.Text) + " WHERE pcode = '" + f.txtPCode.Text + "'");
                }

                //UpdateData("UPDATE tblcart SET qty = qty - " + int.Parse(f.txtCancelQty.Text) + " WHERE id LIKE '" + f.txtID.Text + "'");
                UpdateData("UPDATE tblcart SET qty = qty - " + int.Parse(f.txtCancelQty.Text) + " WHERE pcode LIKE '" + f.txtPCode.Text + "'");


                MessageBox.Show("Return Successfully", "ALL J SHOP GENERAL MERCHANDISE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Dispose();

                f.RefreshList();
                f.Dispose();

            }
            else
            {
                MessageBox.Show("Fill up all form");
            }
        }

        public void SaveCancelOrder()
        {
            try
            {
                cn.Open();
                string query1 = "INSERT INTO tblcancel (transno, pcode, price, qty, sdate, cancelledby, action) VALUES (@transno, @pcode, @price, @qty, @sdate, @cancelledby, @action)";
                cm = new SqlCommand(query1, cn);
                cm.Parameters.AddWithValue("@transno", f.txtTransnoNo.Text);
                cm.Parameters.AddWithValue("@pcode", f.txtPCode.Text);
                cm.Parameters.AddWithValue("@price", double.Parse(f.txtPrice.Text));
                cm.Parameters.AddWithValue("@qty", int.Parse(f.txtCancelQty.Text));
                cm.Parameters.AddWithValue("@sdate", DateTime.Now);
                cm.Parameters.AddWithValue("@cancelledby", f.txtCancel.Text);
                cm.Parameters.AddWithValue("@action", f.cboAction.Text);
                cm.ExecuteNonQuery();
                cn.Close();

            }catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "ALL J SHOP GENERAL MERCHANDISE", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void UpdateData(string sql)
        {
            try
            {
                cn.Open();
                cm = new SqlCommand(sql, cn);
                cm.ExecuteNonQuery();
                cn.Close();

            }catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "ALL J SHOP GENERAL MERCHANDISE", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmVoid_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }else if (e.KeyCode == Keys.Escape)
            {
                button1_Click(sender, e);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            if (textBoxPassword.Text != "")
            {
                try
                {
                    cn.Open();
                    string query1 = "SELECT * FROM Pin WHERE Type LIKE 'Admin'";
                    cm = new SqlCommand(query1, cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        //adminUsername = dr["username"].ToString();
                        //adminPassword = dr["password"].ToString();

                        adminPin = dr["Pin"].ToString();
                    }
                    dr.Close();
                    cn.Close();
                }
                catch (Exception ex)
                {
                    cn.Close();
                    MessageBox.Show(ex.Message);
                }

                if (adminPin != textBoxPassword.Text)
                {
                    MessageBox.Show("The PIN is incorrect. Try again.", "VOID FAILED", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxPassword.Clear();
                    return;
                }

                SaveCancelOrder();
                if (f.cboAction.Text == "Yes")
                {
                    UpdateData("UPDATE tblproduct SET qty = qty + " + int.Parse(f.txtCancelQty.Text) + " WHERE pcode = '" + f.txtPCode.Text + "'");
                }

                //UpdateData("UPDATE tblcart SET qty = qty - " + int.Parse(f.txtCancelQty.Text) + " WHERE id LIKE '" + f.txtID.Text + "'");
                UpdateData("UPDATE tblcart SET qty = qty - " + int.Parse(f.txtCancelQty.Text) + " WHERE pcode LIKE '" + f.txtPCode.Text + "'");


                MessageBox.Show("Return Successfully", "ALL J SHOP GENERAL MERCHANDISE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Dispose();

                f.RefreshCart2();
                f.RefreshList();
                f.Dispose();

            }
            else
            {
                MessageBox.Show("Please enter admin pin", "VOID FAILED", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
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

        private void buttonx_Click(object sender, EventArgs e)
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

        private void button4_Click(object sender, EventArgs e)
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

        private void button0_Click(object sender, EventArgs e)
        {
            textBoxPassword.Text += button0.Text;
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

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
