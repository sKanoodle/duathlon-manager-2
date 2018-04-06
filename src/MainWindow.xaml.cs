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

namespace DuathlonManager2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            List<int> foo = new List<int>();
            foo.Insert(0, 3);
            foo.Insert(1, 2);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var resources = Application.Current.Resources.MergedDictionaries.ToArray();
            foreach (var resource in resources.Where(r => r.Source.OriginalString.StartsWith("Localization")))
                Application.Current.Resources.MergedDictionaries.Remove(resource);

            var dict = (ResourceDictionary)Application.LoadComponent(new Uri("Localization/de_DE.xaml", UriKind.Relative));
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
    }
}
