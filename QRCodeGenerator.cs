using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;
using ZXing.QrCode.Internal;
using ZXing.Rendering;

namespace OOP_System
{
    public partial class QRCodeGenerator : Form
    {
        public QRCodeGenerator()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string inputText = txtInput.Text;

            int qrSize = inputText.Length > 100 ? 500 : 300; 
            ErrorCorrectionLevel errorCorrectionLevel = inputText.Length > 100 ? ErrorCorrectionLevel.M : ErrorCorrectionLevel.H;

            BarcodeWriter bw = new BarcodeWriter();
            EncodingOptions encodingOptions = new EncodingOptions()
            {
                Width = qrSize,
                Height = qrSize,
                Margin = 0,
                PureBarcode = false
            };

            encodingOptions.Hints.Add(EncodeHintType.ERROR_CORRECTION, errorCorrectionLevel);
            bw.Renderer = new BitmapRenderer();
            bw.Options = encodingOptions;
            bw.Format = BarcodeFormat.QR_CODE;

            try
            {
                Bitmap bitmap = bw.Write(inputText);

                Bitmap logo = new Bitmap($"{Application.StartupPath}/MJ_SHOP_LOGO.png");
                Bitmap resize_logo = new Bitmap(logo, new Size(80, 80));

                Bitmap logo_with_border = new Bitmap(100, 100);
                using (Graphics graphics = Graphics.FromImage(logo_with_border))
                {
                    graphics.Clear(Color.White);
                    graphics.DrawImage(resize_logo, new Point(10, 10));
                }

                Graphics g = Graphics.FromImage(bitmap);
                g.DrawImage(logo_with_border, new Point((bitmap.Width - logo_with_border.Width) / 2, (bitmap.Height - logo_with_border.Height) / 2));

                pictureBox1.Image = bitmap;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BarcodeReader barcodeReader = new BarcodeReader();
            var result = barcodeReader.Decode(new Bitmap(pictureBox1.Image));
            if (result != null)
            {
                txtOutput.Text = result.Text;
            }
            else
            {
                txtOutput.Text = "no output";
            }
        }
    }
}
