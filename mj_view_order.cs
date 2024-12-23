using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_System
{
    public partial class mj_view_order : Form
    {
        mj_Purchased mj_purchased;

        //string item_description, string price_per_pc, string qty, string total_cost
        public mj_view_order(mj_Purchased frm, String order_date_v, string item_description, string price_per, string qty, string total_cost, string order_status, string payment_method, string payment_date, string customer_name, string contact_number)
        {
            InitializeComponent();
            mj_purchased = frm;
            
            order_date.Text = order_date_v;
            text_item_description.Text = item_description;
            txtPricePerPc.Text = price_per;
            txtQty.Text = qty;
            txtTotalCost.Text = total_cost;
            txt_order_status.Text = order_status;
            txt_payment_method.Text = payment_method;
            txt_payment_date.Text = payment_date;
            txt_customer_name.Text = customer_name;
            txt_contact_number.Text = contact_number;
        }

        private void mj_view_order_Load(object sender, EventArgs e)
        {

        }
    }
}
