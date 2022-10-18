using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FS19ModManager.config;

namespace FS19ModManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static App? Instance { get; private set; }
        public Config? Config { get; private set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                Instance = this;
                Config = new Config(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error loading config", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                
                Instance = this;
                Config = new Config(false);
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                Config?.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error saving config", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            }
        }
    }
}
