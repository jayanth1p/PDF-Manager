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
    /// Interaction logic for SplitPage.xaml
    /// </summary>
    public partial class SplitPage : Page
    {
        private TabControl pdfTabControl;
        private String pdfFilePath;
        private List<int> splitPoints = new List<int>();
        public SplitPage()
        {
            InitializeComponent();
        }

        private void Splitlbut_Click(object sender, RoutedEventArgs e)
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
                Header = fileName ,
                Content = new WebView2
                {
                    Source = new Uri(pdfFilePath),
                }
            };

            pdfTabControl.Items.Add(tab);
            pdfTabControl.SelectedItem = tab;

            }


            
        }

        private void AddSpoint_Click(object sender, RoutedEventArgs e)
        {
            splitPoints.Add(int.Parse(Spointbox.Text));
            SplitList.Items.Add($"Split at page: {Spointbox.Text}");
            splitPoints.Sort(); // Ensure split points are sorted
            Spointbox.Clear(); // Clear the input box after adding

        }

     

        private void Splitpdf_Click(object sender, RoutedEventArgs e)
        {
            
            string fileName = string.Empty;
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                Title = "Save Merged PDF",
                FileName = "SplitDoc.pdf"
            };
            string outputFilePath = string.Empty;
            if (saveFileDialog.ShowDialog() == true)
            {
                 outputFilePath = saveFileDialog.FileName;
            }

                using (PdfDocument inputDocument = PdfReader.Open(pdfFilePath, PdfDocumentOpenMode.Import))
                {
                    int totalPages = inputDocument.PageCount;
                    splitPoints.Sort();

                    List<(int start, int end)> ranges = new List<(int, int)>();
                    int lastIndex = 0;

                    foreach (int point in splitPoints)
                    {
                        if (point <= lastIndex || point > totalPages)
                            continue;

                        ranges.Add((lastIndex, point - 1));
                        lastIndex = point;
                    }

                    // Add final range
                    if (lastIndex < totalPages)
                        ranges.Add((lastIndex, totalPages - 1));

                    // Create PDFs for each range
                    int partNumber = 1;
                pdfTabControl?.Items.Clear();
                

                foreach (var (start, end) in ranges)
                    {
                        PdfDocument outputDoc = new PdfDocument();

                        for (int i = start; i <= end; i++)
                        {
                            outputDoc.AddPage(inputDocument.Pages[i]);
                        }

                    string outputPath = outputFilePath.Replace(".pdf",string.Empty) + $"@Part-{partNumber}.pdf";
                        outputDoc.Save(outputPath);
                        partNumber++;

                     fileName = System.IO.Path.GetFileName(outputPath);
                    var newtab = new TabItem
                    {
                        Header = fileName,
                        Content = new WebView2
                        {
                            Source = new Uri(outputPath),
                        }
                    };


                    pdfTabControl.Items.Add(newtab);
                    pdfTabControl.SelectedItem = newtab;

                }
                }
                MessageBox.Show($"PDF split successfully into {splitPoints.Count + 1} parts.\nSaved to: {outputFilePath}", "Split Complete", MessageBoxButton.OK, MessageBoxImage.Information);

            

        }

        private void Splitclbut_Click(object sender, RoutedEventArgs e)
        {
            if (pdfTabControl.SelectedItem != null)
            {
                
                pdfTabControl.Items.Remove(pdfTabControl.SelectedItem);
            }
        }
    }
}
