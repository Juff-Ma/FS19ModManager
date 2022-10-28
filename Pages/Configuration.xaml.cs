using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaktionslogik für Configuration.xaml
    /// </summary>
    public partial class Configuration : Page, IDataPageHandle
    {
        public Configuration()
        {
            InitializeComponent();
            DataContext = data;
        }

        bool first = true;

        public void handleData(object? data)
        {
            object[]? datas = data as object[];
            if (datas != null)
            {
                this.data.TitleText = datas[0] != null ? datas[0] as string : "Config";

                if (datas[1] != null)
                {
                    first = (bool)datas[1];
                }
            }
        }

        private Data data = new();

        public class Data : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler? PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string? name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }

            private string? titleText;
            public string? TitleText
            {
                get { return titleText; }
                set
                {
                    titleText = value;
                    
                    OnPropertyChanged();
                }

            }

            public string? GamePath { get; set; } = "C:\\Users\\" + Environment.UserName + "\\Documents\\My Games\\FarmingSimulator2019\\";
            public string? ModsPath { get; set; } = "C:\\Users\\" + Environment.UserName + "\\Documents\\My Games\\FarmingSimulator2019\\mods";
            public string? AllModsPath { get; set; } = "C:\\Users\\" + Environment.UserName + "\\Documents\\My Games\\FarmingSimulator2019\\all_mods";

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (System.IO.Directory.Exists(GameBox.Text) &&
                System.IO.Directory.Exists(ModsBox.Text))
            {
                if (App.Instance?.Config != null && App.Instance.Config.Folders != null)
                {
                    App.Instance.Config.Initialised = true;

                    App.Instance.Config.Folders.GameRoot = GameBox.Text;
                    App.Instance.Config.Folders.Mods = ModsBox.Text;
                    App.Instance.Config.Folders.Active = AllModsBox.Text;
                }
               
                if (System.IO.Directory.Exists(AllModsBox.Text))
                {
                    var result = MessageBox.Show(MainWindow.Instance,
                        "The new mods folder already contains files/mods. Do you want to index them? If you press no all files alredy contained will be deleted!",
                        "Index existing Mods?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                    if (result == MessageBoxResult.No)
                        System.IO.Directory.Delete(AllModsBox.Text);
                }
                System.IO.Directory.CreateDirectory(AllModsBox.Text);

                if (first)
                {
                    MainWindow.Navigate<Thanks>(null, new ModernWpf.Media.Animation.DrillInNavigationTransitionInfo());
                }
                else if (this.Parent is Window)
                {
                    (this.Parent as Window)?.Close();
                }
            }
            else
            {
                MessageBox.Show(MainWindow.Instance, "Either the Game or Mods folder don't exist!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Game_Click(object sender, RoutedEventArgs e)
        {
            using var dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "Select Savegame folder",
                UseDescriptionForTitle = true,
                InitialDirectory = "C:\\Users\\" + Environment.UserName + "\\Documents\\My Games\\FarmingSimulator2019\\",
                ShowNewFolderButton = true
            };

            if (dialog.ShowDialog(MainWindow.Instance?.GetIWin32Window()) == System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show(dialog.SelectedPath);
            }
        }

        private void Mods_Click(object sender, RoutedEventArgs e)
        {
            using var dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "Select Mods folder",
                UseDescriptionForTitle = true,
                InitialDirectory = "C:\\Users\\" + Environment.UserName + "\\Documents\\My Games\\FarmingSimulator2019\\",
                SelectedPath = "mods",
                ShowNewFolderButton = true
            };

            if (dialog.ShowDialog(MainWindow.Instance?.GetIWin32Window()) == System.Windows.Forms.DialogResult.OK)
            {

            }
        }

        private void AllMods_Click(object sender, RoutedEventArgs e)
        {
            using var dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "Select New Mods folder",
                UseDescriptionForTitle = true,
                InitialDirectory = "C:\\Users\\" + Environment.UserName + "\\Documents\\My Games\\FarmingSimulator2019\\",
                SelectedPath = "all_mods",
                ShowNewFolderButton = true
            };

            if (dialog.ShowDialog(MainWindow.Instance?.GetIWin32Window()) == System.Windows.Forms.DialogResult.OK)
            {

            }
        }
    }
}
