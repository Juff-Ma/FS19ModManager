using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FS19ModManager.Pages
{
    /// <summary>
    /// Interaktionslogik für MainPage.xaml
    /// </summary>
    public partial class MainPage : Page, IDataPageHandle
    {
        bool firstload = false;
        public MainPage()
        {
            InitializeComponent();
        }

        public void handleData(object? data)
        {
            if (data != null)
            {
                firstload = (bool)data;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (firstload)
            {
                DoubleAnimation widthAnimation = new DoubleAnimation(700, TimeSpan.FromSeconds(0.7));
                DoubleAnimation heightAnimation = new DoubleAnimation(550, TimeSpan.FromSeconds(1.2));
                MainWindow.Instance?.BeginAnimation(Window.WidthProperty, widthAnimation);
                MainWindow.Instance?.BeginAnimation(Window.HeightProperty, heightAnimation);
            }
        }
    }
}
