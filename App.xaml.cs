using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shell;
using FS19ModManager.config;

namespace FS19ModManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public struct ModStruct
        {
            public Dictionary<string, string> titles;
            public Dictionary<string, string> descriptions;
            public string version;
            public string author;

            public Image icon;
        }
        
        public static App? Instance { get; private set; }
        public Config? Config { get; private set; }
        public Mods? Mods { get; set; }
        public Dictionary<Mod, ModStruct>? ModInfos { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Instance = this;
            try
            {
                Config = new(true);
                Mods = new(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error loading config", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                
                Config = new(false);
                Mods = new(false);
            }
            ModInfos = new();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                Config?.Save();
                Mods?.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error saving config", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            }
        }
    }
}
