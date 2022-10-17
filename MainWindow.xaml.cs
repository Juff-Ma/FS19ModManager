using ModernWpf.Media.Animation;
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

namespace FS19ModManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ModernWpf.Controls.Frame? navigator { get; private set; }
        public static void Navigate<T>(object data, NavigationTransitionInfo animation) where T: Page
        {
            navigator?.Navigate(typeof(T), data, new DrillInNavigationTransitionInfo());
        }
        public static void Navigate<T>() where T : Page
        {
            navigator?.Navigate(typeof(T));
        }
        public static void Navigate<T>(object data) where T : Page
        {
            navigator?.Navigate(typeof(T), data);
        }
        public MainWindow()
        {
            InitializeComponent();
            navigator = ContentFrame;
        }

        private void ContentFrame_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
