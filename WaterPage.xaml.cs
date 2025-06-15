using Microsoft.Web.WebView2.Wpf;
using Microsoft.Win32;
using Microsoft.Win32;
using PdfSharp.Drawing;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for WaterPage.xaml
    /// </summary>
    public partial class WaterPage : Page
    {

        private TabControl pdfTabControl;
        private String pdfFilePath;
        public WaterPage()
        {
            InitializeComponent();
        }

        private void waterlpdf_Click(object sender, RoutedEventArgs e)
        {
            if (pdfTabControl == null)
            {
                pdfTabControl = new TabControl();
                TabArea.Children.Add(pdfTabControl);
            }
            // Open file dialog to pick PDFs
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                Multiselect = false // Allow only single file selection for splitting
            };
            if (openFileDialog.ShowDialog() == true)
            {
                pdfFilePath = openFileDialog.FileName;
                string fileName = System.IO.Path.GetFileName(pdfFilePath);
                var tab = new TabItem
                {
                    Header = fileName,
                    Content = new WebView2
                    {
                        Source = new Uri(pdfFilePath),
                    }
                };

                pdfTabControl.Items.Add(tab);
                pdfTabControl.SelectedItem = tab;

            }

        }

        private void waterclpdf_Click(object sender, RoutedEventArgs e)
        {
            if (pdfTabControl != null && pdfTabControl.SelectedItem != null)
            {
                pdfTabControl.Items.Remove(pdfTabControl.SelectedItem);
            }

        }

        private void Waterpdfbut_Click(object sender, RoutedEventArgs e)
        {
            string watermarkText = watertex.Text.Trim();
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                Title = "MaterMark PDF",
                FileName = "WaterMark.pdf"
            };
            string outputFilePath = string.Empty;
            if (saveFileDialog.ShowDialog() == true)
            {
                outputFilePath = saveFileDialog.FileName;
            }
            GlobalFontSettings.UseWindowsFontsUnderWindows = true;
            using (PdfDocument document = PdfReader.Open(pdfFilePath, PdfDocumentOpenMode.Modify))
            {
                // Fix: Replace 'XFontStyle.Regular' with 'XFontStyle.Normal'
                XFont font = new XFont("Verdana", 40, XFontStyleEx.Bold);
                XBrush brush = new XSolidBrush(XColor.FromArgb(128, 200, 200, 200)); // Semi-transparent gray

                foreach (PdfPage page in document.Pages)
                {
                    using (XGraphics gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Append))
                    {
                        gfx.TranslateTransform(page.Width / 2, page.Height / 2);
                        gfx.RotateTransform(-45);
                        gfx.DrawString(watermarkText, font, brush, new XPoint(0, 0), XStringFormats.Center);
                    }
                }

                document.Save(outputFilePath);
            }


            MessageBox.Show($"PDF WaterMark successfully into.\nSaved to: {outputFilePath}", "WaterMark Complete", MessageBoxButton.OK, MessageBoxImage.Information);

            string fileName = System.IO.Path.GetFileName(outputFilePath);
            var tab = new TabItem
            {
                Header = fileName,
                Content = new WebView2
                {
                    Source = new Uri(outputFilePath),
                }
            };

            pdfTabControl?.Items.Clear();
            pdfTabControl.Items.Add(tab);
            pdfTabControl.SelectedItem = tab;
        }
    }
}
