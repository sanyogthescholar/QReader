using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Drawing;

using ZXing;

using BarcodeReader = ZXing.Presentation.BarcodeReader;
using System.Drawing.Imaging;
using System.IO;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.Windows.Media;

namespace QReader
{
    public partial class Form1 : Form
    {
        private BarcodeReader reader = new BarcodeReader();

        private System.Drawing.Bitmap BitmapFromSource(BitmapSource bitmapsource) //taken from https://stackoverflow.com/a/3751751
        {
            System.Drawing.Bitmap bitmap;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new System.Drawing.Bitmap(outStream);
            }
            return bitmap;
        }

        private Bitmap takeScreenshot()
        {
            Rectangle bounds = Screen.GetBounds(System.Drawing.Point.Empty);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(System.Drawing.Point.Empty, System.Drawing.Point.Empty, bounds.Size);
                }
                return bitmap;
            }
        }
        public void handleShortcut()
        {
            //System.Windows.Forms.MessageBox.Show("hotkey works 2");
            Console.WriteLine("inside handleShortcut");
            //this.takeScreenshot();
            //scanQR(new Bitmap(ScreenCapture.CaptureDesktop()));
            if (System.Windows.Clipboard.ContainsImage())
            {
                System.Drawing.Image clipboardImage = BitmapFromSource(System.Windows.Clipboard.GetImage());
                var result = reader.Decode(System.Windows.Clipboard.GetImage());
                if (result == null)
                {
                    System.Windows.Clipboard.SetText("No QR found");
                }
                else
                {
                    //System.Windows.Forms.MessageBox.Show(result.Text);
                    System.Windows.Clipboard.SetText(result.Text);
                }
            }
        }

        private void scanQR(Bitmap screenShot) //pass in screenshot as a bitmap
        {
            var reader = new ZXing.Presentation.BarcodeReader();
            Console.WriteLine(screenShot.GetType());
            var result = reader.Decode(Convert(screenShot));
            //pictureBox1.Image = screenShot;
            if (result == null)
            {
                System.Windows.Clipboard.SetText("No QR found");
            }
            else
            {
                //System.Windows.Forms.MessageBox.Show(result.Text);
                System.Windows.Clipboard.SetText(result.Text);
            }
        }

        public static BitmapSource Convert(System.Drawing.Bitmap bitmap) //https://stackoverflow.com/a/30729291
        {
            var bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height,
                bitmap.HorizontalResolution, bitmap.VerticalResolution,
                PixelFormats.Bgr24, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);

            return bitmapSource;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Event is fired");
            this.Hide();
            notifyIcon1.Visible = true;
            ShowInTaskbar = false;
            e.Cancel = true;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            //notifyIcon1.Visible = false;
            this.WindowState = FormWindowState.Normal;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipTitle = "Minimize to Tray App";
            notifyIcon1.BalloonTipText = "You have successfully minimized your form.";

            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(500);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = false;
            }
        }
    }
}