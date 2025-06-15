using Microsoft.Win32;
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

using Microsoft.Web.WebView2.Wpf;
using Microsoft.Win32;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for RotaPage.xaml
    /// </summary>
    public partial class RotaPage : Page
    {
        private TabControl pdfTabControl;
        private String pdfFilePath;
        public RotaPage()
        {
            InitializeComponent();
        }

        public void Menu_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem item)
            {
                Rotangle.Header = $"Selected: {item.Header}";
                Rotangle.Tag = item.Tag; // Store the rotation angle in the Tag property
            }

        }

        private void Rotalpdf_Click(object sender, RoutedEventArgs e)
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

        private void rotaclpdf_Click(object sender, RoutedEventArgs e)
        {
            if (pdfTabControl.SelectedItem != null)
            {

                pdfTabControl.Items.Remove(pdfTabControl.SelectedItem);
            }
        }

        private void rotatepdf_Click(object sender, RoutedEventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                Title = "Save Rotated PDF",
                FileName = "Rotated Doc.pdf"
            };
            string outputFilePath = string.Empty;
            if (saveFileDialog.ShowDialog() == true)
            {
                outputFilePath = saveFileDialog.FileName;
            }
            int rotationAngle = 0;
            rotationAngle = Convert.ToInt32(Rotangle.Tag?.ToString());


            using (PdfDocument document = PdfReader.Open(pdfFilePath, PdfDocumentOpenMode.Modify))
            {
                foreach (PdfPage page in document.Pages)
                {
                    // Apply rotation
                    page.Rotate = (page.Rotate + rotationAngle) % 360;
                }

                document.Save(outputFilePath);
            }
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
