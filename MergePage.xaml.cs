using Microsoft.Web.WebView2;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using Microsoft.Win32;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for MergePage.xaml
    /// </summary>
    public partial class MergePage : Page
    {

        private TabControl pdfTabControl;
        private List <string> pdfFiles = new List<string>();
        public MergePage()
        {
            InitializeComponent();

            //LoadPdf();
            

        }
        private async void AddPdfs_Click(object sender, RoutedEventArgs e)
        {
            // If the TabControl doesn't exist, create and add it
            if (pdfTabControl == null)
            {
                pdfTabControl = new TabControl();
                TabArea.Children.Add(pdfTabControl);
            }
            
            // Open file dialog to pick PDFs
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var pdfPath in openFileDialog.FileNames)
                {
                    string fileName = System.IO.Path.GetFileName(pdfPath);

                   

                    var tab = new TabItem
                    {
                        Header = fileName,
                        Content = new WebView2
                        {
                            Source = new Uri(pdfPath),
                        }
                    };
                    

                    pdfFiles.Add(pdfPath);

                    pdfTabControl.Items.Add(tab);
                    pdfTabControl.SelectedItem = tab; // Auto-switch to the new tab
                }
                
            }
        }


        private void TabsRem_Click(object sender, RoutedEventArgs e )

        {
           

            if (pdfTabControl.SelectedItem != null)
            {
                pdfFiles.Remove(pdfTabControl.SelectedItem.ToString());
                pdfTabControl.Items.Remove(pdfTabControl.SelectedItem);
            }
        }


        private void ClearTabs_Click(object sender, RoutedEventArgs e)
        {
            pdfTabControl?.Items.Clear();
            pdfFiles.Clear(); // Clear the list of PDF files as well
        }

        private void Merge_Click(object sender, RoutedEventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                Title = "Save Merged PDF",
                FileName = "MergedDocument.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string outputFilePath = saveFileDialog.FileName;

                using (PdfDocument outputDocument = new PdfDocument())
                {
                    foreach (string file in pdfFiles)
                    {
                        if (!File.Exists(file))
                            continue;

                        using (PdfDocument inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import))
                        {
                            for (int i = 0; i < inputDocument.PageCount; i++)
                            {
                                PdfPage page = inputDocument.Pages[i];
                                outputDocument.AddPage(page);
                            }
                        }
                    }

                    outputDocument.Save(outputFilePath);
                    MessageBox.Show($"PDFs merged successfully:\n{outputFilePath}", "Merge Complete", MessageBoxButton.OK, MessageBoxImage.Information);
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
            else
            {
                MessageBox.Show("Merge canceled.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }



        }



        //private async  void AddTabs_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog dialog = new OpenFileDialog
        //    {
        //        Filter = "PDF Files (*.pdf)|*.pdf",
        //        Multiselect = true
        //    };

        //    if (dialog.ShowDialog() == true)
        //    {
        //        foreach (var file in dialog.FileNames)
        //        {
        //            if (!File.Exists(file)) continue;

        //            var webView = new WebView2
        //            {
        //                Source= new Uri(file),
        //                HorizontalAlignment = HorizontalAlignment.Stretch,
        //                VerticalAlignment = VerticalAlignment.Stretch
        //            };

        //            await webView.EnsureCoreWebView2Async();
        //            //webView.Source = new Uri(file);

        //            var tab = new TabItem
        //            {
        //                Header = System.IO.Path.GetFileName(file),
        //                Content = webView
        //            };

        //            PdfTabControl.Items.Add(tab);
        //            PdfTabControl.SelectedItem = tab;
        //        }
        //    }
        //}



        //private async void LoadPdf()
        //{
        //    await mergpdf.EnsureCoreWebView2Async(null);

        //    string pdfPath = @"C:\Users\pedad\Downloads\Real-World Bug Hunting.pdf";

        //    // Convert to URI
        //    string pdfUri = new Uri(pdfPath).AbsoluteUri;

        //    // Load in WebView2
        //    mergpdf.Source = new Uri(pdfUri);

        //}

        //private void Browse_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";

        //    if (openFileDialog.ShowDialog() == true)
        //    {
        //        string selectedFilePath = openFileDialog.FileName;
        //        MessageBox.Show($"Selected file: {selectedFilePath}");

        //        // Example: load in WebView2 (assuming you have one named Merpdf)
        //        mergpdf.Source = new Uri(selectedFilePath);
        //    }
        //}

    }
}
