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
using FS19ModManager.Pages;
using ModernWpf.Navigation;
using System.Threading;
using System.ComponentModel;

namespace FS19ModManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ModernWpf.Controls.Frame? Navigator { get; set; }

        public static MainWindow? Instance { get; private set; }

        public static void Navigate<T>(object? data, NavigationTransitionInfo animation) where T: Page
        {
            Instance?.Navigator?.Navigate(typeof(T), data, animation);
        }
        public static void Navigate<T>() where T : Page
        {
            Instance?.Navigator?.Navigate(typeof(T));
        }
        public static void Navigate<T>(object? data) where T : Page
        {
            Instance?.Navigator?.Navigate(typeof(T), data);
        }
        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            Navigator = ContentFrame;

            if (App.Instance?.Config?.Window != null)
            {
                Width = App.Instance.Config.Window.Width;
                Height = App.Instance.Config.Window.Height;
            }
        }

        bool closable = true;
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = !closable;
        }

        private void ContentFrame_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.Instance?.Config?.Initialised == false)
            {
                Navigate<Configuration>(new object[] { "Initial Config", true });
            }
            else
            {
                Navigate<MainPage>();
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (App.Instance?.Config?.Window != null)
            {
                App.Instance.Config.Window.Width = Convert.ToInt32(Width);
                App.Instance.Config.Window.Height = Convert.ToInt32(Height);
            }
        }

        private void ContentFrame_LoadCompleted(object sender, NavigationEventArgs e)
        {
            Title = "FS19 Mod Manager: " + (e.Content as Page)?.Title;

            if ((e.Content as Page) is IDataPageHandle)
            {
                (e.Content as IDataPageHandle)?.handleData(e.ExtraData);
            }
        }
    }
}

public static class MyWpfExtensions
{
    public static System.Windows.Forms.IWin32Window? GetIWin32Window(this System.Windows.Media.Visual visual)
    {
        var source = System.Windows.PresentationSource.FromVisual(visual) as System.Windows.Interop.HwndSource;
        System.Windows.Forms.IWin32Window win;
        if (source != null)
        {
            win = new OldWindow(source.Handle);
            return win;
        }
        return null;
    }

    private class OldWindow : System.Windows.Forms.IWin32Window
    {
        private readonly System.IntPtr _handle;
        public OldWindow(System.IntPtr handle)
        {
            _handle = handle;
        }

        #region IWin32Window Members
        System.IntPtr System.Windows.Forms.IWin32Window.Handle
        {
            get { return _handle; }
        }
        #endregion
    }

    public static ModernWpf.Controls.NavigationViewItem? GetSelectedItem(this ModernWpf.Controls.NavigationView self)
    {
        return self.SelectedItem as ModernWpf.Controls.NavigationViewItem;
    }
}