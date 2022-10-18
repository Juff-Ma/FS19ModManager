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

namespace FS19ModManager.Pages
{
    /// <summary>
    /// Interaktionslogik für Thanks.xaml
    /// </summary>
    public partial class Thanks : Page
    {
        public Thanks()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Navigate<MainPage>(true, new ModernWpf.Media.Animation.SlideNavigationTransitionInfo());
        }
    }
}
