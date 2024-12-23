using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace OOP_System
{
    public partial class mj_mainForm : Form
    {
        private Button currentButton;
        private Random random;
        private int tempIndex;

        private Boolean showPanelOrders = false;
        private Boolean showPanelExpense = false;
        private Boolean showPanelPurchase = false;
        private Boolean showPanelDownPayment = false;
        private Boolean showPanelProduct = false;

        private DBConnection dbConnection;

        private Timer networkMonitorTimer;


        public mj_mainForm()
        {
            InitializeComponent();
            random = new Random();

            dbConnection = new DBConnection();
            //MonitorNetworkConnection();
        }

        //public void togglePanels()
        //{
        //    if(showPanelOrders)
        //    {
        //        panelOrders.Height = 107;
        //    }
        //    else
        //    {
        //        panelOrders.Height = 0;
        //    }

        //    if (showPanelExpense)
        //    {
        //        panelExpense.Height = 54;
        //    }
        //    else
        //    {
        //        panelExpense.Height = 0;
        //    }

        //    if (showPanelPurchase)
        //    {
        //        panelPurchase.Height = 51;
        //    }
        //    else
        //    {
        //        panelPurchase.Height = 0;
        //    }

        //    if (showPanelDownPayment)
        //    {
        //        panelDown_Payment.Height = 98;
        //    }
        //    else
        //    {
        //        panelDown_Payment.Height = 0;
        //    }

        //    if (showPanelProduct)
        //    {
        //        panelProduct.Height = 51;
        //    }
        //    else
        //    {
        //        panelProduct.Height = 0;
        //    }
        //}

        private Color SelectThemeColor()
        {
            int index = random.Next(ThemeColor.ColorList.Count);
            while (tempIndex == index)
            {
                index = random.Next(ThemeColor.ColorList.Count);
            }
            tempIndex = index;
            string color = ThemeColor.ColorList[index];
            return ColorTranslator.FromHtml(color);
        }

        private void ActivateButton(object btnSender)
        {
            if (btnSender != null)
            {
                if (currentButton != (Button)btnSender)
                {
                    DisableButton();
                    string colorr = "#cdfec2";
                    Color color = ColorTranslator.FromHtml(colorr);
                    currentButton = (Button)btnSender;
                    currentButton.BackColor = Color.FromArgb(109, 207, 246);
                    //currentButton.ForeColor = Color.White;
                    //currentButton.Font = new System.Drawing.Font("Arial", 12.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    //panelTitleBar.BackColor = color;
                    //panelLogo.BackColor = ThemeColor.ChangeColorBrightness(color, -0.3);
                    //ThemeColor.PrimaryColor = color;
                    //ThemeColor.SecondaryColor = ThemeColor.ChangeColorBrightness(color, -0.3);
                    //btnCloseChildForm.Visible = true;
                }
            }
        }
        //public static class NetworkHelper
        //{
        //    /// <summary>
        //    /// Checks if the computer is connected to the internet.
        //    /// </summary>
        //    public static bool IsConnectedToInternet()
        //    {
        //        try
        //        {
        //            // Ping a reliable external host to verify connectivity
        //            using (var ping = new Ping())
        //            {
        //                var reply = ping.Send("8.8.8.8", 3000); // Google DNS
        //                return reply != null && reply.Status == IPStatus.Success;
        //            }
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //}

        //private async void MonitorNetworkConnection()
        //{
        //    while (true)
        //    {
        //        if (!NetworkHelper.IsConnectedToInternet())
        //        {
        //            MessageBox.Show("Internet connection lost. The application will now close.",
        //                "Connection Lost", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        //            Application.Exit();
        //        }

        //        await Task.Delay(5000); // Check every 5 seconds
        //    }
        //}


        private void DisableButton()
        {
            foreach (Control previousBtn in panel1.Controls)
            {
                if (previousBtn.GetType() == typeof(Button))
                {
                    previousBtn.BackColor = Color.FromArgb(239, 251, 255);
                    //previousBtn.ForeColor = Color.FromArgb(51, 54, 63);
                    //previousBtn.Font = new System.Drawing.Font("Arial", 12.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
        }
    
        public void GetDashboard()
        {

            //ActivateButton(sender);
            main_panel.Controls.Clear();
            mj_dashboard2 frm = new mj_dashboard2();


            frm.TopLevel = false;
            main_panel.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void mj_mainForm_Load(object sender, EventArgs e)
        {

            
            main_panel.Controls.Clear();
            GetDashboard();
            //togglePanels();
            //MonitorNetworkConnection();
        }

        private void btn_Inventory_Click(object sender, EventArgs e)
        {
            main_panel.Controls.Clear();
            mj_inventoryForm frm = new mj_inventoryForm();
            frm.TopLevel = false;
            main_panel.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void main_panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //showPanelOrders = false;
            //showPanelExpense = false;
            //showPanelPurchase = false;
            //showPanelDownPayment = false;
            //showPanelProduct = false;
            //togglePanels();

            ActivateButton(sender);
            main_panel.Controls.Clear();
            mj_dashboard2 frm = new mj_dashboard2();


            frm.TopLevel = false;
            main_panel.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //showPanelOrders = false;
            //showPanelExpense = false;
            //showPanelPurchase = false;
            //showPanelDownPayment = false;

            //showPanelProduct = !showPanelProduct;
            //togglePanels();

            ActivateButton(sender);
            main_panel.Controls.Clear();
            mj_ProductMaster frm = new mj_ProductMaster();
            frm.LoadRecords();
            frm.getTotalProducts();
            frm.getTotalProductsGrid();
            frm.TopLevel = false;
            main_panel.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            main_panel.Controls.Clear();
            mj_Purchased frm = new mj_Purchased();
            frm.LoadRecords();
            frm.TopLevel = false;
            main_panel.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //showPanelPurchase = false;
            //showPanelDownPayment = false;
            //showPanelOrders = false;
            //showPanelProduct = false;

            //showPanelExpense = !showPanelExpense;
            //togglePanels();

            ActivateButton(sender);
            main_panel.Controls.Clear();
            mj_Expenses frm = new mj_Expenses();
            frm.CalculateTotalExpense();
            frm.LoadRecords();
            frm.calculateExpenses();
            frm.LoadFilterByCategory();
            frm.TopLevel = false;
            main_panel.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void btnDownPayment_Click(object sender, EventArgs e)
        {
            //showPanelOrders = false;
            //showPanelExpense = false;
            //showPanelPurchase = false;
            //showPanelProduct = false;

            //showPanelDownPayment = !showPanelDownPayment;
            //togglePanels();

            ActivateButton(sender);
            main_panel.Controls.Clear();
            mj_downPayment frm = new mj_downPayment();
            frm.LoadRecords();
            frm.CalculateTotalAmountPaid();
            //frm.LoadDownPaymentRecords();
            frm.TopLevel = false;
            main_panel.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //showPanelOrders = false;
            //showPanelExpense = false;
            //showPanelDownPayment = false;
            //showPanelProduct = false;

            //showPanelPurchase = !showPanelPurchase;
            //togglePanels();

            ActivateButton(sender);
            main_panel.Controls.Clear();
            mj_itemPurchase frm = new mj_itemPurchase();
            frm.CalculateTotalExpense();
            frm.LoadRecords();
            //frm.calculateExpenses();
            frm.TopLevel = false;
            main_panel.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnPurchased_Click(object sender, EventArgs e)
        {

            //showPanelExpense = false;
            //showPanelPurchase = false;
            //showPanelDownPayment = false;
            //showPanelProduct = false;

            //showPanelOrders = !showPanelOrders;
            //togglePanels();

            ActivateButton(sender);
            main_panel.Controls.Clear();
            mj_Purchased frm = new mj_Purchased();
            frm.getTotalOrders();
            frm.LoadRecords();
            frm.TopLevel = false;
            main_panel.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
                
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            //mj_ProductMaster frm2 = new mj_ProductMaster();
            //mj_addProdductMasterItem frm = new mj_addProdductMasterItem();
            //String text = "";

            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    cn.Open();
            //    text += dataGridView1.Rows[i].Cells[3].Value.ToString();
            //    cn.Close();
            //}
            //qRCodeGenerator.txtInput.Text = text;

            mj_ProductMaster frm = new mj_ProductMaster();
            frm.button2_Click(sender, e);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ActivateButton(sender);
            main_panel.Controls.Clear();
            mj_salary frm = new mj_salary();
            frm.LoadRecords();
            frm.CalculateTotalExpense();
            frm.TopLevel = false;
            main_panel.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            ActivateButton(sender);
            main_panel.Controls.Clear();
            mj_sales frm = new mj_sales();
            frm.LoadRecords();
            frm.CalculateTotalGrossSales();
            frm.CalculateTotalExpense(); 
            frm.CalculateTotalPurchase();
            frm.CalculateTotalNetIncome();
            frm.CalculateTotalTotalProfit();
            frm.TopLevel = false;
            main_panel.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            ActivateButton(sender);
            main_panel.Controls.Clear();
            mj_reporting frm = new mj_reporting();
            frm.TopLevel = false;
            main_panel.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }
    }
}
