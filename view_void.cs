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
    public partial class view_void : Form
    {
        mj_view_void_details mj_view;

        public view_void(mj_view_void_details frm, String order_date_v, string item_description, string price_per, string qty, string total_cost, string customer_name, string contact_number, string void_date, string reason_for_void, string voided_by, string void_condition_, string payment_amount)
        {
            InitializeComponent();

            mj_view = frm;


            order_date.Text = order_date_v;
            text_item_description.Text = item_description;
            txtPricePerPc.Text = price_per;
            txtQty.Text = qty;
            txtTotalCost.Text = total_cost;
            txt_customer_name.Text = customer_name;
            txt_contact_number.Text = contact_number;
            void_Date.Text = void_date;
            voidReason.Text = reason_for_void;
            voidedBy.Text = voided_by;
            cboVoidCondition.Text = void_condition_;
            txtTotalPayment.Text = payment_amount;

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void view_void_Load(object sender, EventArgs e)
        {

        }
    }
}
