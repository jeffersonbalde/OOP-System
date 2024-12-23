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

namespace OOP_System
{
    public partial class frmUserAccount : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;

        public frmUserAccount()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());

            this.KeyPreview = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        //private void Clear()
        //{
        //    txtName.Clear();
        //    txtPass.Clear();
        //    txtRetype.Clear();
        //    txtPin.Clear();
        //    cboUserType.Text = "";
        //    txtPin.Focus();
        //}


        private void Clear()
        {
            txtPin.Clear();
            cboUserType.Text = "";
            txtName.Clear();
            txtPin.Focus();
        }

        private void metroTabControl1_Resize(object sender, EventArgs e)
        {

        }

        private void frmUserAccount_Resize(object sender, EventArgs e)
        {
            //metroTabControl1.Left = (this.Width - metroTabControl1.Width) / 2;
            //metroTabControl1.Top = (this.Height - metroTabControl1.Height) / 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        //private void btnSave_Click(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        if (txtName.Text == "" || txtPass.Text == "" || txtRetype.Text == "" || txtPin.Text == "" || cboUserType.Text == "")
        //        {
        //            MessageBox.Show("Please fill up all fields", "ADD ITEM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }

        //        if (txtPass.Text != txtRetype.Text)
        //        {
        //            MessageBox.Show("Please make sure your passwords match.", "ALL J SHOP GENERAL MERCHANDISE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }

        //        cn.Open();
        //        string query = "INSERT INTO tblUser(username, password, role, name) VALUES(@username, @password, @role, @name)";
        //        cm = new SqlCommand(query, cn);
        //        cm.Parameters.AddWithValue("@username", txtPin.Text);
        //        cm.Parameters.AddWithValue("@password", txtPass.Text);
        //        cm.Parameters.AddWithValue("@role", cboUserType.Text);
        //        cm.Parameters.AddWithValue("@name", txtName.Text);
        //        cm.ExecuteNonQuery();
        //        cn.Close();

        //        LoadUsername();
        //        LoadUsernameDelete();
        //        Clear();
        //        MessageBox.Show("Account saved.", "ALL J SHOP GENERAL MERCHANDISE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
        //    }catch(Exception ex)
        //    {
        //        cn.Close();
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void metroTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void LoadUsername()
        {
            try
            {
                txtPin2.Items.Clear();
                cn.Open();
                string query = "SELECT * FROM tblUser";
                cm = new SqlCommand(query, cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    txtPin2.Items.Add(dr["username"].ToString());
                }
                dr.Close();
                cn.Close();

            }
            catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }



        public void LoadUsernamePin()
        {
            try
            {
                txtPin2.Items.Clear();
                cn.Open();
                string query = "SELECT * FROM Pin";
                cm = new SqlCommand(query, cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    txtPin2.Items.Add(dr["Name"].ToString());
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


        public void LoadUsernameDelete()
        {
            try
            {
                cboDeletePin.Items.Clear();
                cn.Open();
                string query = "SELECT * FROM tblUser";
                cm = new SqlCommand(query, cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    cboDeletePin.Items.Add(dr["username"].ToString());
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

        public void LoadPinDelete()
        {
            try
            {
                cboDeletePin.Items.Clear();
                cn.Open();
                string query = "SELECT * FROM Pin";
                cm = new SqlCommand(query, cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    cboDeletePin.Items.Add(dr["Name"].ToString());
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

        public void DeleteUsername()
        {
            try
            {
                cboDeletePin.Items.Clear();
                LoadUsernameDelete();

                if (MessageBox.Show("Deleting your account will remove your access to the system. This can’t be undone.", "DELETE ACCOUNT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    cn.Open();
                    string query = "DELETE FROM tblUser WHERE username LIKE '" + cboDeletePin.Text + "'";
                    cm = new SqlCommand(query, cn);
                    cm.ExecuteNonQuery();
                    dr.Close();
                    cn.Close();

                    LoadUsernameDelete();
                    MessageBox.Show("Account " + cboDeletePin.Text + " has been deleted.", "ALL J SHOP GENERAL MERCHANDISE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboDeletePin.Text = "";
                    cboDeletePin.Focus();

                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        public void DeletePin()
        {
            try
            {
                cboDeletePin.Items.Clear();
                //LoadUsernameDelete();
                LoadPinDelete();

                if (MessageBox.Show("Deleting your pin will remove your access to the system. This can’t be undone.", "DELETE PIN", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    cn.Open();
                    string query = "DELETE FROM Pin WHERE Name LIKE '" + cboDeletePin.Text + "'";
                    cm = new SqlCommand(query, cn);
                    cm.ExecuteNonQuery();
                    dr.Close();
                    cn.Close();

                    //LoadUsernameDelete();
                    LoadPinDelete();
                    LoadUsernamePin();
                    MessageBox.Show("Pin " + cboDeletePin.Text + " has been deleted.", "PIN DELETED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboDeletePin.Text = "";
                    cboDeletePin.Focus();

                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        public void ValidateOldPassword()
        {
            try
            {
                cn.Open();
                string query = "SELECT password FROM tblUser WHERE username LIKE '" + txtPin2.Text + "'";
                cm = new SqlCommand(query, cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    //oldPassword = dr["password"].ToString();
                }
                dr.Close();
                cn.Close();

            }
            catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        public void UpdatePassword()
        {
            try
            {
                string oldPass = "";
                cn.Open();
                string query = "SELECT password FROM tblUser WHERE username LIKE '" + txtPin2.Text + "'";
                cm = new SqlCommand(query, cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    oldPass = dr["password"].ToString();
                }
                dr.Close();
                cn.Close();

                if (txtOldPin.Text != oldPass)
                {
                    MessageBox.Show("Old password don't match.");
                    return;
                }

                if (txtNewPin.Text != txtConfirmPin.Text)
                {
                    MessageBox.Show("Please make sure your passwords match.", "ALL J SHOP GENERAL MERCHANDISE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                txtPin2.Items.Clear();
                LoadUsername();
                cn.Open();
                string query1 = "UPDATE tblUser SET password = '" + txtNewPin.Text + "' WHERE username LIKE '" + txtPin2.Text + "'";
                cm = new SqlCommand(query1, cn);
                cm.ExecuteNonQuery();
                cn.Close();

                LoadUsername();
                MessageBox.Show("Password to " + txtPin2.Text + " has been changed.", "ALL J SHOP GENERAL MERCHANDISE", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtPin2.Text = "";
                txtOldPin.Text = "";
                txtNewPin.Text = "";
                txtConfirmPin.Text = "";
                txtPin2.Focus();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        public void UpdatePinPassword()
        {
            try
            {
                string oldPass = "";
                cn.Open();
                string query = "SELECT Pin FROM Pin WHERE Name LIKE '" + txtPin2.Text + "'";
                cm = new SqlCommand(query, cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    oldPass = dr["Pin"].ToString();
                }
                dr.Close();
                cn.Close();

                if (txtOldPin.Text != oldPass)
                {
                    MessageBox.Show("Old pin don't match.", "CHANGE PIN", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (txtNewPin.Text != txtConfirmPin.Text)
                {
                    MessageBox.Show("Please make sure your pin match.", "CHANGE PIN", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                txtPin2.Items.Clear();
                LoadUsernamePin();
                //LoadUsername();
                cn.Open();
                string query1 = "UPDATE Pin SET Pin = '" + txtNewPin.Text + "' WHERE Name LIKE '" + txtPin2.Text + "'";
                cm = new SqlCommand(query1, cn);
                cm.ExecuteNonQuery();
                cn.Close();

                LoadUsernamePin();
                //LoadUsername();
                MessageBox.Show("Pin " + txtPin2.Text + " has been changed.", "CHANGE PIN", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtPin2.Text = "";
                txtOldPin.Text = "";
                txtNewPin.Text = "";
                txtConfirmPin.Text = "";
                txtPin2.Focus();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(cboDeletePin.Text == "")
            {
                MessageBox.Show("Please select username", "ADD ITEM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DeleteUsername();
        }

        private void buttonSaveNewPassword_Click(object sender, EventArgs e)
        {

            if (txtConfirmPin.Text == "" || txtNewPin.Text == "" || txtOldPin.Text == "" || txtPin2.Text == "")
            {
                MessageBox.Show("Please fill up all fields", "ADD ITEM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            UpdatePassword();

        }

        private void tabPageCreate_Click(object sender, EventArgs e)
        {

        }

        private void cboRole_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void frmUserAccount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                button1_Click(sender, e);
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            try
            {

                if (txtName.Text == "" || txtPin.Text == "" || cboUserType.Text == "")
                {
                    MessageBox.Show("Please fill up all fields", "CREATE PIN", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //if (txtPass.Text != txtRetype.Text)
                //{
                //    MessageBox.Show("Please make sure your passwords match.", "ALL J SHOP GENERAL MERCHANDISE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}

                cn.Open();
                string query = "INSERT INTO Pin(Pin, Type, Name) VALUES(@Pin, @Type, @Name)";
                cm = new SqlCommand(query, cn);
                cm.Parameters.AddWithValue("@Pin", txtPin.Text);
                cm.Parameters.AddWithValue("@Type", cboUserType.Text);
                cm.Parameters.AddWithValue("@Name", txtName.Text);
                cm.ExecuteNonQuery();
                cn.Close();

                //LoadUsername();
                //LoadUsernameDelete();
                LoadUsernamePin();
                LoadPinDelete();
                Clear();
                LoadAccounts();
                MessageBox.Show("Pin saved.", "CREATE PIN", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void frmUserAccount_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtPin;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txtConfirmPin.Text == "" || txtNewPin.Text == "" || txtOldPin.Text == "" || txtPin2.Text == "")
            {
                MessageBox.Show("Please fill up all fields", "CHANGE PIN", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LoadAccounts();
            //UpdatePassword();
            UpdatePinPassword();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (cboDeletePin.Text == "")
            {
                MessageBox.Show("Please select username", "ADD ITEM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DeleteUsername();
            LoadAccounts();
            LoadUsernameDelete();
            LoadUsername();
        }

        private void cboRole_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBoxUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBoxDeleteUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        public void LoadAccounts()
        {
            try
            {

                cn.Open();
                int i = 0;
                dataGridView1.Rows.Clear();
                string query = "SELECT * FROM tblUser";

                cm = new SqlCommand(query, cn);
                dr = cm.ExecuteReader();

                while (dr.Read())
                {
                    i++;
                    dataGridView1.Rows.Add(i, dr["name"].ToString(), dr["username"].ToString(), dr["password"].ToString(), dr["role"].ToString());
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

        public void LoadPinAccounts()
        {
            try
            {

                cn.Open();
                int i = 0;
                dataGridView1.Rows.Clear();
                string query = "SELECT * FROM Pin";

                cm = new SqlCommand(query, cn);
                dr = cm.ExecuteReader();

                while (dr.Read())
                {
                    i++;
                    dataGridView1.Rows.Add(i, dr["Name"].ToString(), dr["Pin"].ToString(), dr["Type"].ToString());
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

        private void button6_Click(object sender, EventArgs e)
        {
            if (cboDeletePin.Text == "")
            {
                MessageBox.Show("Please select Name in the combobox", "DELETE PIN", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //DeleteUsername();
            //LoadAccounts();
            //LoadUsernameDelete();
            //LoadUsername();
            DeletePin();
            LoadUsernamePin();
            LoadPinDelete();
            LoadPinAccounts();
        }

        

        private void button5_Click_1(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtPin_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtOldPin_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtNewPin_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtConfirmPin_KeyPress(object sender, KeyPressEventArgs e)
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
