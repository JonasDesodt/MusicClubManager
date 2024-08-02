using System.Windows;
using System.Xml.Linq;
using Publisher = Microsoft.Office.Interop.Publisher;

namespace MusicClubManager.Cms.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var publisherApp = new Publisher.Application();                          

            var doc = publisherApp.Open("C:\\Users\\jonas\\OneDrive\\Documents\\Publication2.pub");

            Publisher.TextFrame nameFrame = doc.Pages[1].Shapes["TextBox1"].TextFrame;

                foreach(Publisher.Shape tet in doc.Pages[1].Shapes)
            {
                var text = tet.TextFrame.TextRange.Text;

            }
            nameFrame.TextRange.Text = "Koekoek2";

            string outputPath = @"C:\Users\jonas\OneDrive\Documents\test2.pub";
            doc.SaveAs(outputPath);

            doc.Close();
            publisherApp.Quit();
        }
    }
}