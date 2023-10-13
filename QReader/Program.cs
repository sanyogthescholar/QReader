using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace QReader
{
    internal static class QReader
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //InitializeComponent();
            var myForm = new Form1();
            GlobalHotKey.RegisterHotKey("Ctrl + Shift + Q", () => myForm.handleShortcut());
            Application.Run(myForm);
        }
        static System.Drawing.Bitmap BitmapFromSource(BitmapSource bitmapsource) //taken from https://stackoverflow.com/a/3751751
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
        static void ScanCode()
        {
            /*var reader = new ZXing.Presentation.BarcodeReader();
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
            }*/
            //Try Screenshot method as described here https://www.codeproject.com/Articles/485883/Create-your-own-Snipping-Tool

        }
    }
}
