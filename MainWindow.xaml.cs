using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new MergePage());
        }

        private async void LoadPdf()
        {

            
            // Uncomment if you want to use a different WebView2 control for PDF viewing
            /* await merpdf.EnsureCoreWebView2Async(null);

             // Path to your local PDF
             string pdfPath = @"C:\Path\To\Your\File.pdf";

             // Convert to URI
             string pdfUri = new Uri(pdfPath).AbsoluteUri;

             // Load in WebView2
             pdfWebView.Source = new Uri(pdfUri);*/
        }



        private void Nav_Merge(object sender, RoutedEventArgs e)
        {
           NavigateToPage("Merge");
        }
        private void Nav_Split(object sender, RoutedEventArgs e)
        {
           /* MessageBox.Show("This is a sample WPF application demonstrating navigation between pages.", "About", MessageBoxButton.OK, MessageBoxImage.Information);*/
            NavigateToPage("Split");
        }

        private void Nav_Water(object sender, RoutedEventArgs e)
        {
            NavigateToPage("Water");
        }

        private void Nav_Rota(object sender, RoutedEventArgs e)
        {
           NavigateToPage("Rota");
        }


        private void NavigateToPage(string page)
        {
            // Load Page
            switch (page)
            {
                case "Merge":
                    MainFrame.Navigate(new MergePage());
                    break;
                case "Split":
                    MainFrame.Navigate(new SplitPage());
                    break;
                case "Water":
                    MainFrame.Navigate(new WaterPage());
                    break;
                case "Rota":
                    MainFrame.Navigate(new RotaPage());
                    break;
              
            }
            var color1 = (Color)ColorConverter.ConvertFromString("#201f1e");
            var color2 = (Color)ColorConverter.ConvertFromString("#282828");
            // Reset All Button Backgrounds
            foreach (var child in Nav.Children)
            {
                if (child is Button btn)
                    btn.Background = Brushes.Transparent;
            }

           
            // Highlight selected
            switch (page)
            {
                case "Merge":
                    merbut.Background = new SolidColorBrush(color2);
                    break;
                case "Split":
                    splitbut.Background = new SolidColorBrush(color2);
                    break;
                case "Water":
                    waterbut.Background= new SolidColorBrush(color2);
                    break;
                case "Rota":
                    rotabut.Background = new SolidColorBrush(color2);
                    break;  
            }
        }



    }
}