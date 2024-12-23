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
using Tulpep.NotificationWindow;

namespace OOP_System
{
    public partial class frmPOS : Form
    {

        String id;
        String price;
        String cart_qty;
        String cart_total;
        String item;

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr; 
        DBConnection dbcon = new DBConnection();
        frmSoldItems frmSales;

        int qty;
        string userType = "";

        frmRecords frmrecords;
        frmAddDebt frmAdd;
        frmAddCustomer frmAddC;

        public frmPOS(frmSoldItems form, frmAddDebt frm)
        {
            InitializeComponent();
            //lblDate.Text = DateTime.Now.ToLongDateString();
            cn = new SqlConnection(dbcon.MyConnection());
            this.KeyPreview = true;
            frmSales = form;
            frmAdd = frm;

            NotifyCriticalItems();
        }

        public double GetTotalItem()
        {
            Double totalItem = 0;

            try
            {
                cn.Open();
                string query = "SELECT c.id, c.pcode, p.pdesc, c.price, c.qty, c.disc, c.total FROM tblcart AS c INNER JOIN tblproduct AS p ON c.pcode = p.pcode WHERE transno LIKE '" + lblTransno.Text + "' AND status LIKE 'Pending'";
                cm = new SqlCommand(query, cn);
                dr = cm.ExecuteReader();

                while (dr.Read())
                {
                    totalItem += Double.Parse(dr["total"].ToString());
                }
                dr.Close();
                cn.Close();

                return totalItem;

            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }

            return totalItem;
        }
    

        public void NotifyCriticalItems()
        {
            string critical = "";
            int i = 0;

            cn.Open();
            cm = new SqlCommand("SELECT COUNT(*) FROM vwCriticalItems", cn);
            string count = cm.ExecuteScalar().ToString();
            cn.Close();

            cn.Open();
            string query = "SELECT * FROM vwCriticalItems";
            cm = new SqlCommand(query, cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                critical += i + ". " + dr["pdesc"].ToString() + Environment.NewLine;
            }
            dr.Close();
            cn.Close();

            PopupNotifier popup = new PopupNotifier();
            popup.Image = Properties.Resources.x;
            popup.TitleText = count + " LOW STOCK ITEMS";
            popup.ContentText = critical;
            popup.Popup();
        }

        private void frmPOS_Load(object sender, EventArgs e)
        {
            LoadRecords();
            GetTransNo();
            this.ActiveControl = txtSearch;
        }

        public void LoadRecords()
        {
            int i = 0;
            dataGridView2.Rows.Clear();
            cn.Open();
            //string query = "SELECT p.pcode, p.barcode, p.pdesc, b.brand, c.category, p.price, p.qty FROM tblProduct as p INNER JOIN tblBrand AS b ON b.id = p.bid INNER JOIN tblCategory AS c ON c.id = p.cid WHERE p.pdesc LIKE '" + txtSearchProduct.Text + "%' ORDER BY p.pdesc";
            //string query1 = "SELECT pcode, barcode, pdesc, price, qty FROM tblProduct WHERE pdesc LIKE '%" + txtSearchProduct.Text + "%' ORDER BY pdesc";

            if(txtSearch.Text == "")
            {
                cm = new SqlCommand("SELECT pcode, barcode, pdesc, price, qty FROM tblProduct WHERE pdesc LIKE '%" + txtSearchProduct.Text + "%' ORDER BY pdesc", cn);
            }

            if(txtSearch.Text != "")
            {
                cm = new SqlCommand("SELECT pcode, barcode, pdesc, price, qty FROM tblProduct WHERE barcode LIKE '%" + txtSearch.Text + "%' ORDER BY pdesc", cn);
            }

            if(txtSearchProduct.Text != "")
            {
                if(txtSearch.Text != "")
                {
                    cm = new SqlCommand("SELECT pcode, barcode, pdesc, price, qty FROM tblProduct WHERE pdesc LIKE '%" + txtSearchProduct.Text + "%' AND barcode LIKE '" + txtSearch.Text + "' ORDER BY pdesc", cn);
                }
                else
                {
                    cm = new SqlCommand("SELECT pcode, barcode, pdesc, price, qty FROM tblProduct WHERE pdesc LIKE '%" + txtSearchProduct.Text + "%' ORDER BY pdesc", cn);
                }
            }

            //cm = new SqlCommand(query1, cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                //dataGridView2.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
                dataGridView2.Rows.Add(i, dr["pcode"].ToString(), dr["barcode"].ToString(), dr["pdesc"].ToString(), dr["price"].ToString(), dr["qty"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if(dataGridView1.Rows.Count > 0)
            //{
            //    MessageBox.Show("You have pending items in your current transaction", "ALL J SHOP GENERAL MERCHANDISE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            //GetTransNo();
            //txtSearch.Enabled = true;
            //txtSearch.Focus();

            ResumeTransaction frm = new ResumeTransaction(this);
            frm.LoadPauseTransaction();
            frm.ShowDialog();
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        public void GetCartTotal()
        {
            double discount = Double.Parse(labelDiscount.Text);
            double sales = Double.Parse(lblTotal.Text);
            //double vat = sales * dbcon.GetVal();
            //double vatable = sales - vat;

            //lblVat.Text = vat.ToString("#,##0.00");
            //lblVatable.Text = vatable.ToString("#,##0.00");z
            labelTotal.Text = sales.ToString("#,##0.00");
        }

        public void GetTransNo()
        {
            try
            {
                string sdate = DateTime.Now.ToString("yyyyMMdd");
                string transno;
                int count;
                cn.Open();
                string query = "SELECT TOP 1 transno FROM tblcart WHERE transno LIKE '" + sdate + "%' ORDER BY id DESC";
                cm = new SqlCommand(query, cn);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    transno = dr[0].ToString();
                    count = int.Parse(transno.Substring(8, 4));
                    lblTransno.Text = sdate + (count + 1);
                }else
                {
                    transno = sdate + "1001";
                    lblTransno.Text = transno;
                }
                dr.Close();
                cn.Close();

            }catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "ALL J GENERAL MERCHANDISE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtSearch_Click(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
            try
            {
                if(txtSearch.Text == String.Empty) { return; }
                else
                {

                    String _pcode;
                    double _price;
                    int _qty;

                    cn.Open();
                    string query = "SELECT * FROM tblproduct WHERE barcode LIKE '" + txtSearch.Text + "'";
                    cm = new SqlCommand(query, cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                     
                    if (dr.HasRows)
                    {
                        qty = int.Parse(dr["qty"].ToString());
                        //frmQty frm = new frmQty(this);
                        //frm.ProductDetails(dr["pcode"].ToString(), double.Parse(dr["price"].ToString()), lblTransno.Text, qty);

                        _pcode = dr["pcode"].ToString();
                        _price = double.Parse(dr["price"].ToString());
                        _qty = int.Parse(txtQty.Text);

                        dr.Close();
                        cn.Close();

                        //frm.ShowDialog();

                        AddToCart(_pcode, _price, _qty);

                    } else
                    {
                        dr.Close();
                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "ALL J GENERAL MERCHANDISE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void AddToCart(String _pcode, double _price, int _qty)
        {

            String id = "";
            bool found = false;
            int cart_qty = 0;

            cn.Open();
            string query1 = "SELECT * FROM tblCart WHERE pcode = @pcode AND transno = @transno";
            cm = new SqlCommand(query1, cn);
            cm.Parameters.AddWithValue("@pcode", _pcode);
            cm.Parameters.AddWithValue("@transno", lblTransno.Text);
            dr = cm.ExecuteReader();
            dr.Read();

            if (dr.HasRows)
            {
                found = true;
                id = dr["id"].ToString();
                cart_qty = int.Parse(dr["qty"].ToString());
            }
            else
            {
                found = false;
            }
            dr.Close();
            cn.Close();

            if (found)
            {

                if (qty < (int.Parse(txtQty.Text) + cart_qty))
                {
                    MessageBox.Show("Insufficient remaing stock, Remaing item is " + qty, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                cn.Open();
                string query = "UPDATE tblCart SET qty = (qty + " + _qty  + ") WHERE id = '" + id + "'";
                cm = new SqlCommand(query, cn);
                cm.ExecuteNonQuery();
                cn.Close();

                txtSearch.SelectionStart = 0;
                txtSearch.SelectionLength = txtSearch.Text.Length;
                LoadCart();

            }
            else
            {

                if (qty < int.Parse(txtQty.Text))
                {
                    MessageBox.Show("Insufficient remaing stock, Remaing item is " + qty, "ALL J SHOP GENERAL MERCHANDISE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                cn.Open();
                string query = "INSERT INTO tblcart (transno, pcode, price, qty, sdate, cashier)VALUES(@transno, @pcode, @price, @qty, @sdate, @cashier)";
                cm = new SqlCommand(query, cn);
                cm.Parameters.AddWithValue("@transno", lblTransno.Text);
                cm.Parameters.AddWithValue("@pcode", _pcode);
                cm.Parameters.AddWithValue("@price", _price);
                cm.Parameters.AddWithValue("@qty", _qty);
                cm.Parameters.AddWithValue("@sdate", DateTime.Now);
                cm.Parameters.AddWithValue("@cashier", lblUser.Text);
                cm.ExecuteNonQuery();
                cn.Close();

                txtSearch.SelectionStart = 0;
                txtSearch.SelectionLength = txtSearch.Text.Length;
                LoadCart();
            }

        }

        public void LoadCart()
        {
            try
            {
                Boolean hasrecord = false;
                dataGridView1.Rows.Clear();
                int i = 0;
                double total = 0;
                double discount = 0;
                cn.Open();
                string query = "SELECT c.id, c.pcode, p.pdesc, c.price, c.qty, c.disc, c.total FROM tblcart AS c INNER JOIN tblproduct AS p ON c.pcode = p.pcode WHERE transno LIKE '" + lblTransno.Text + "' AND status LIKE 'Pending'";
                cm = new SqlCommand(query, cn);
                dr = cm.ExecuteReader();

                while (dr.Read())
                {
                    i++;
                    total += Double.Parse(dr["total"].ToString());
                    discount += Double.Parse(dr["disc"].ToString());
                    dataGridView1.Rows.Add(i, dr["id"].ToString(), dr["pcode"].ToString(), dr["pdesc"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["disc"].ToString(), Double.Parse(dr["total"].ToString()).ToString("#,##0.00"));
                    hasrecord = true;
                }
                dr.Close(); 
                cn.Close();
                lblTotal.Text = total.ToString("#,##0.00");
                labelDiscount.Text = discount.ToString("#,##0.00");
                GetCartTotal();
                if(hasrecord == true)
                {
                    //btnPayment.Enabled = true;
                    buttonAddDiscount.Enabled = true;
                    buttonClearItems.Enabled = true;
                }else
                {
                    //btnPayment.Enabled = false;
                    buttonAddDiscount.Enabled = false;
                    buttonClearItems.Enabled = false;
                }

            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ALL J GENERAL MERCHANDISE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cn.Close();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(lblTransno.Text == "000000000000000")
            {
                return;
            }

            frmLookUp frm = new frmLookUp(this);
            frm.LoadRecords();
            frm.ShowDialog();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            int i = dataGridView1.CurrentRow.Index;
            id = dataGridView1[1, i].Value.ToString();
            price = dataGridView1[4, i].Value.ToString();
            cart_qty = dataGridView1[5, i].Value.ToString();
            cart_total = dataGridView1[7, i].Value.ToString();
            item = dataGridView1[3, i].Value.ToString();
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            frmDiscount frm = new frmDiscount(this);
            frm.lblID.Text = id;
            frm.txtPrice.Text = price;
            frm.txtQty.Text = cart_qty;
            frm.txtTotal.Text = cart_total;
            frm.txtItem.Text = item;
            frm.ShowDialog();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;

            if(colName == "Delete")
            {
                if(MessageBox.Show("Remove this item?", "ALL J GENERAL MERCHANDISE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    string query = "DELETE FROM tblcart WHERE id LIKE '" + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() + "'";
                    cm = new SqlCommand(query, cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Item has been removed", "ALL J GENERAL MERCHANDISE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCart();
                }
            }
        }

        private void dataGridView12_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;

            if (colName == "Delete")
            {
                if (MessageBox.Show("Remove this item?", "ITEM REMOVE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    string query = "DELETE FROM tblcart WHERE id LIKE '" + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() + "'";
                    cm = new SqlCommand(query, cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    LoadCart();
                }
            }
            else if (colName == "colAdd")
            {
                int i = 0;
                cn.Open();
                string query = "SELECT sum(qty) AS qty FROM tblproduct WHERE pcode LIKE '" + dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() + "' GROUP BY pcode";
                cm = new SqlCommand(query, cn);
                i = int.Parse(cm.ExecuteScalar().ToString());
                cn.Close();

                if (int.Parse(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString()) < i)
                {

                    cn.Open();
                    string query1 = "UPDATE tblCart SET qty  = qty + " + int.Parse(txtQty.Text) + " WHERE transno LIKE '" + lblTransno.Text + "' AND pcode LIKE '" + dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() + "'";
                    cm = new SqlCommand(query1, cn);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    LoadCart();
                }
                else
                {
                    MessageBox.Show("Insufficient remaing stock, Remaining item is " + i + " ", "ALL J SHOP GENERAL MERCHANDISE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if(colName == "colRemove")
            {
                int i = 0;
                cn.Open();
                string query = "SELECT sum(qty) AS qty FROM tblCart WHERE pcode LIKE '" + dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() + "' AND transno LIKE '" + lblTransno.Text + "' GROUP BY transno, pcode";
                cm = new SqlCommand(query, cn);
                i = int.Parse(cm.ExecuteScalar().ToString());
                cn.Close();

                if (i > 1)
                {

                    cn.Open();
                    string query1 = "UPDATE tblCart SET qty  = qty - " + int.Parse(txtQty.Text) + " WHERE transno LIKE '" + lblTransno.Text + "' AND pcode LIKE '" + dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() + "'";
                    cm = new SqlCommand(query1, cn);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    LoadCart();
                }
                else
                {
                    MessageBox.Show("Insufficient item, Remaining item is " + i + " ", "ITEM INVALID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
            lblDate1.Text = DateTime.Now.ToLongDateString();
        }

        private void lblTime_Click(object sender, EventArgs e)
        {

        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            //frmSettle frm = new frmSettle(this, frmAdd);
            //frm.txtSale.Text = lblDisplayTotal.Text;
            //frm.lblDiscount.Text = labelDiscount.Text;    
            //frm.comboBoxCategory.Text = "CASH";
            //frm.txtSale.Text = GetTotalItem().ToString("#,##0.00");
            //frm.ShowDialog();
            //frm.txtCash.Focus();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSale_Click(object sender, EventArgs e)
        {
            CashierItemSales frm = new CashierItemSales(frmSales);
            frm.dt1.Enabled = false;
            frm.dt2.Enabled = false;
            frm.cboCashier.Enabled = false;
            frm.cboCashier.Text = lblUser.Text;
            frm.LoadRecord();
            frm.ShowDialog();
        }

        private void lblUser_Click(object sender, EventArgs e)
        {

        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {

            if (lblUserType.Text == "Admin")
            {

                if (dataGridView1.Rows.Count > 0)
                {
                    MessageBox.Show("You have pending items in your current transaction", "QUIT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    Form1 frm = new Form1();
                    frm.lblName.Text = lblUser.Text;

                    this.Dispose();
                    frm.ShowDialog();
                }
            }
            else
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    MessageBox.Show("You have pending items in your current transaction", "QUIT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    if (MessageBox.Show("Are you sure you want to log out and close the application?", "QUIT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.Dispose();
                        frmSecurity frmLogin = new frmSecurity();
                        frmLogin.ShowDialog();
                    }
                }
            }

            //if(dataGridView1.Rows.Count > 0)
            //{
            //    MessageBox.Show("You have pending items in your current transaction", "ALL J SHOP GENERAL MERCHANDISE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}else
            //{
            //    if (MessageBox.Show("Are you sure you want to log out and close the application?", "ALL J SHOP GENERAL MERCHANDISE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //    {
            //        this.Dispose();
            //        frmSecurity frm = new frmSecurity();
            //        frm.ShowDialog();
            //    }
            //}
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }
            
        private void frmPOS_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F1)
            {
                btnSale_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F2)
            {
                //if(buttonAddDiscount.Enabled == true)
                //{
                //    btnDiscount_Click(sender, e);
                //}
                //else
                //{
                //    MessageBox.Show("You have no transaction", "ALL J SHOP GENERAL MERCHANDISE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //}
                buttonPrintReciept_Click(sender, e);
            }
            //else if (e.KeyCode == Keys.F3)
            //{
            //    if (btnPayment.Enabled == true)
            //    {
            //        btnPayment_Click(sender, e);
            //    }
            //    else
            //    {
            //        MessageBox.Show("You have no transaction", "ALL J SHOP GENERAL MERCHANDISE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }
            //}
            else if(e.KeyCode == Keys.F3)
            {
                if(dataGridView1.Rows.Count <= 0)
                {
                    MessageBox.Show("You have no transaction", "CLEAR ITEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    btnCancel_Click(sender, e);
                }
            }
            else if (e.KeyCode == Keys.F4)
            {
                if (dataGridView1.Rows.Count <= 0)
                {
                    MessageBox.Show("You have no transaction", "ADD DISCOUNT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    btnDiscount_Click(sender, e);
                }
            }
            else if (e.KeyCode == Keys.F5)
            {
                button12_Click(sender, e);
            }
            else if(e.KeyCode == Keys.Escape)
            {
                button11_Click(sender, e);
            }
            else if(e.KeyCode == Keys.F6)
            {
                button13_Click(sender, e);
            }
            else if(e.KeyCode == Keys.F7)
            {
                button14_Click(sender, e);
            }
            else if(e.KeyCode == Keys.F8)
            {
                button1_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F9)
            {
                txtSearch.SelectionStart = 0;
                txtSearch.SelectionLength = txtSearch.Text.Length;
            }
            else if(e.KeyCode == Keys.Enter)
            {
                buttonPay_Click(sender, e);
            }
        }

        private void btnChangePass_Click(object sender, EventArgs e)
        {
            //frmChangePassword frm = new frmChangePassword(this);
            //frm.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to clear items? ", "CLEAR ITEMS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                cn.Open();
                string query = "DELETE FROM tblcart WHERE transno LIKE '" + lblTransno.Text + "'";
                cm = new SqlCommand(query, cn);
                cm.ExecuteNonQuery();
                cn.Close();
                LoadCart();
            }
        }

        private void txtQty_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void lblTotal_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void txtQty_Click_1(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (lblTransno.Text == "000000000000000")
                {
                    MessageBox.Show("Generate transaction number first", "ALL J SHOP GENERAL MERCHANDISE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string colName = dataGridView2.Columns[e.ColumnIndex].Name;


                if (colName == "Select")
                {
                    frmQty frm = new frmQty(this);
                    frm.ProductDetails(dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString(), Double.Parse(dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString()), lblTransno.Text, int.Parse(dataGridView2.Rows[e.RowIndex].Cells[5].Value.ToString()));
                    frm.ShowDialog();

                    //frmAddDebt frmDebt = new frmAddDebt(this, null, frmAddC);
                    //frmDebt.DebtProductDetails(dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString(), Double.Parse(dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString()), lblTransno.Text, int.Parse(dataGridView2.Rows[e.RowIndex].Cells[5].Value.ToString()), double.Parse(dataGridView2.Rows[e.RowIndex].Cells[7].Value.ToString()));

                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void metroTextBox1_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btn7_Click(object sender, EventArgs e)
        {
           textBoxCash.Text += button7.Text;
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            textBoxCash.Text += button0.Text;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBoxCash.Text += button8.Text;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBoxCash.Text += button9.Text;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBoxCash.Text += button4.Text;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBoxCash.Text += button5.Text;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBoxCash.Text += button6.Text;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            textBoxCash.Text += button1.Text;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            textBoxCash.Text += button2.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBoxCash.Text += button3.Text;
        }

        private void button00_Click(object sender, EventArgs e)
        {
            textBoxCash.Text += button00.Text;
        }

        private void buttonDot_Click(object sender, EventArgs e)
        {
            textBoxCash.Text += buttonDot.Text;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            int length = textBoxCash.Text.Length;


            if (textBoxCash.Text == "")
            {
                return;
            }
            else
            {
                textBoxCash.Text = textBoxCash.Text.Substring(0, length - 1);
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxCash.Clear();
            textBoxCash.Focus();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBoxCash.Text = button10.Text;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            textBoxCash.Text = button20.Text;
        }

        private void button50_Click(object sender, EventArgs e)
        {
            textBoxCash.Text = button50.Text;
        }

        private void button100_Click(object sender, EventArgs e)
        {
            textBoxCash.Text = button100.Text;
        }

        private void button200_Click(object sender, EventArgs e)
        {
            textBoxCash.Text = button200.Text;
        }

        private void button300_Click(object sender, EventArgs e)
        {
            textBoxCash.Text = button300.Text;
        }

        private void button400_Click(object sender, EventArgs e)
        {
            textBoxCash.Text = button400.Text;
        }

        private void button500_Click(object sender, EventArgs e)
        {
            textBoxCash.Text = button500.Text;
        }

        private void button1000_Click(object sender, EventArgs e)
        {
            textBoxCash.Text = button1000.Text;
        }

        private void textBoxCash_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double total = double.Parse(labelTotal.Text);
                double cash = double.Parse(textBoxCash.Text);
                double change = cash - total;
                labelChange.Text = change.ToString("#,##0.00");

            }
            catch (Exception ex)
            {
                labelChange.Text = "0.00";
            }
        }

        private void textBoxCash_KeyPress(object sender, KeyPressEventArgs e)
        {
            //accept only numbers
            if (e.KeyChar == 46)
            {
                //accept . character
            }
            else if (e.KeyChar == 8)
            {
                //accept backspace
            }
            else if ((e.KeyChar < 48) || (e.KeyChar > 57)) //accept code 48-57 between 0-9
            {
                e.Handled = true;
            }
        }


        public void UpdateQuantity()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                cn.Open();
                string query = "UPDATE tblproduct SET qty = qty - " + int.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString()) + " WHERE pcode = '" + dataGridView1.Rows[i].Cells[2].Value.ToString() + "'";
                cm = new SqlCommand(query, cn);
                cm.ExecuteNonQuery();
                cn.Close();

                cn.Open();
                string query1 = "UPDATE tblcart SET status = 'Sold', MOP = 'CASH' WHERE id = '" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "'";
                cm = new SqlCommand(query1, cn);
                cm.ExecuteNonQuery();
                cn.Close();
            }
        }


        public void SavePauseTransaction()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                cn.Open();
                string query1 = "UPDATE tblcart SET status = 'Pause', MOP = 'CASH' WHERE id = '" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "'";
                cm = new SqlCommand(query1, cn);
                cm.ExecuteNonQuery();
                cn.Close();
            }
        }

        private void buttonPay_Click(object sender, EventArgs e)
        {
            try
            {
                if(dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("You have no transaction, please select an item", "PAYMENT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxCash.Clear();
                    return;
                }

                if ((double.Parse(labelChange.Text) < 0) || (textBoxCash.Text == String.Empty) || (double.Parse(labelTotal.Text) > (double.Parse(textBoxCash.Text))))
                {
                    MessageBox.Show("Insufficient Amount!", "PAYMENT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    //if (comboBoxCategory.Text == "CASH")
                    //{
                    //    UpdateQuantity();
                    //}
                    //else
                    //{
                    //    GCASHUpdateQuantity();
                    //}

                    UpdateQuantity();

                    frmReceipt frm = new frmReceipt(this);
                    frm.LoadReport(labelTotal.Text, labelChange.Text);

                    frm.ShowDialog();

                    MessageBox.Show("Payment successfully saved!", "PAYMENT SAVED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBoxCash.Clear();
                    GetTransNo();
                    LoadCart();
                    LoadRecords();
                    txtSearch.Focus();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "PAYMENT", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            frmAddCustomer frm = new frmAddCustomer(null);
            frm.ShowDialog();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            CustomerDetails frm = new CustomerDetails();
            frm.LoadCustomerInformation();
            frm.ShowDialog();
        }

        private void button14_Click(object sender, EventArgs e)
        {

            try
            {
                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("You have no transaction to pause", "PAUSE FAILED", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxCash.Clear();
                    return;
                }

                if (MessageBox.Show("Are you sure you want to pause this transaction?", "PAUSE TRANSACTION", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    SavePauseTransaction();

                    //MessageBox.Show("Transaction paused. You may resume it through resume transaction menu", "CONVENIENCE STORE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBoxCash.Clear();
                    GetTransNo();
                    LoadCart();
                    LoadRecords();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "CONVENIENCE STORE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonPrintReciept_Click(object sender, EventArgs e)
        {
            CashierReturnItems frm = new CashierReturnItems(this);
            frm.cboCashier.Text = lblUser.Text;
            frm.LoadRecord();
            frm.ShowDialog();
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label6_Click_1(object sender, EventArgs e)
        {

        }
    }
}
