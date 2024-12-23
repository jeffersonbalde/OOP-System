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
    public partial class ResumeTransaction : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;

        frmPOS fpos;

        public ResumeTransaction(frmPOS form)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());

            fpos = form;

            this.KeyPreview = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void LoadPauseTransaction()
        {
            try
            {

                cn.Open();
                int i = 0;
                dataGridView1.Rows.Clear();
                //string query = "SELECT c.transno, c.id, c.pcode, p.pdesc, c.price, c.qty, c.disc, c.total FROM tblcart AS c INNER JOIN tblproduct AS p ON c.pcode = p.pcode WHERE status LIKE 'Pause'";
                string query = "SELECT transno from tblCart WHERE status LIKE 'Pause' GROUP BY transno;";
                cm = new SqlCommand(query, cn);
                dr = cm.ExecuteReader();
                while (dr.Read())

                {
                    i++;
                    dataGridView1.Rows.Add(i, dr["transno"].ToString());
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                string colName = dataGridView1.Columns[e.ColumnIndex].Name;

                if (colName == "Select")
                {
                    if(fpos.dataGridView1.Rows.Count > 0)
                    {
                        MessageBox.Show("You have pending items in your current transaction", "RESUME FAILED", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (MessageBox.Show("Are you sure you want to resume this transaction?", "RESUME TRANSACTION", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {

                        cn.Open();
                        //string query = "UPDATE tblCart SET status = 'Pending', transno = " + fpos.lblTransno.Text + " WHERE transno = " + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() + "";
                        string query = "UPDATE tblCart SET status = 'Pending', transno = " + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() + " WHERE transno = " + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() + "";
                        cm = new SqlCommand(query, cn);
                        cm.ExecuteNonQuery();

                        cn.Close();
                        fpos.lblTransno.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                        fpos.LoadCart();
                        this.Dispose();

                    }
                }

                if(colName == "ViewItems")
                {
                    PauseItems frm = new PauseItems(this);
                    frm.getTransno.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    frm.LoadPauseItems();
                    frm.ShowDialog();
                }

            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void ResumeTransaction_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
