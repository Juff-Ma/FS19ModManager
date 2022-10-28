using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.IO;
using System.IO.Compression;

namespace FS19ModManager.Pages
{
    /// <summary>
    /// Interaktionslogik für MainPage.xaml
    /// </summary>
    public partial class MainPage : Page, IDataPageHandle
    {

        public static MainPage? Instance { get; private set; }

        private UserControl? Current { get; set; }
        public Dictionary<object, UserControl> DataSave { get; set; } = new Dictionary<object, UserControl>();

        bool firstload = false;
        public MainPage()
        {
            InitializeComponent();
            DataContext = this;
            Instance = this;
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
            var worker = MainWindow.Instance != null ? new Worker(MainWindow.Instance) : new Worker();
            worker.JobDone += Worker_JobDone;
            if (MainWindow.Instance != null)
                MainWindow.Instance.Closable = false;
            worker.Show();

            Worker_JobDone(null, "first", null, worker);
        }

        private void Worker_JobDone(object? result, string name, Exception? e, Worker worker)
        {
            if (e != null)
            {
                MessageBox.Show(e.Message, "Error loading", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            Worker.Job job = new();

            switch (name)
            {
                case "first":
                    job = new()
                    {
                        name = "Discovering Mods",
                        description = "Discovering new or existing LS Mods",
                        prefix = "",
                        suffix = " Mods",
                        hasCount = true,
                        hasProgress = false,
                        maxValue = null,
                        data = new object?[] {App.Instance?.Config, App.Instance?.Mods, App.Instance?.ModInfos },
                        job = (object? sender, DoWorkEventArgs e) =>
                        {
                            BackgroundWorker? w = sender as BackgroundWorker;

                            var appConf = (e.Argument as object?[])?[0] as FS19ModManager.config.Config;
                            var mods = (e.Argument as object?[])?[1] as FS19ModManager.config.Mods;
                            var modIcons = (e.Argument as object?[])?[2] as Dictionary<FS19ModManager.config.Mod, App.ModStruct>;

                            string? allModsPath = appConf?.Folders?.Mods;
                            if (allModsPath != null)
                            {
                                int progress = 0;
                                Hash hash = new();
                                foreach (var file in Directory.GetFiles(allModsPath))
                                {
                                    w?.ReportProgress(++progress);
                                    var mod = new FS19ModManager.config.Mod();
                                    mod.Name = file;
                                    mod.Filehash = hash.hash(file);
                                    mod.Hash = hash.hash(file, System.IO.Path.GetFileName(file));
                                    if (mods?.ModList?.Contains(mod) == false)
                                    {
                                        mods?.ModList?.Add(mod);
                                    }
                                }

                                w?.ReportProgress(0, "Checking Active Mods folder");
                                string? activeModsPath = appConf?.Folders?.Active;

                                if (activeModsPath != null)
                                {
                                    foreach (var file in Directory.GetFiles(activeModsPath))
                                    {
                                        w?.ReportProgress(++progress);
                                        var mod = new FS19ModManager.config.Mod();
                                        mod.Name = file;
                                        mod.Filehash = hash.hash(file);
                                        mod.Hash = hash.hash(file, System.IO.Path.GetFileName(file));
                                        if (mods?.ModList?.Contains(mod) == false)
                                        {
                                            if (mods?.ModList?.AsQueryable().Where((x) => x.Filehash == mod.Filehash).AsEnumerable().ToArray().Length > 0)
                                            {
                                                var result = MessageBox.Show("Same File with different name", "You got a file with a different name but exact same contents do you want to replace the one already known to the program or not? (do you wan't to replace)",
                                                    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No); 

                                                
                                            }
                                            else if (Directory.GetFiles(allModsPath).AsEnumerable().Contains(file))
                                            {
                                                
                                            }
                                        }
                                    }
                                }
                            }

                            e.Result = new object?[] { appConf, mods, modIcons };
                        }
                    }; break;
                default: worker.Quit(); if (MainWindow.Instance != null) MainWindow.Instance.Closable = true; Loading = false; NavView_SelectionChanged(NavView); return;
            }
            worker.RunJobAsync(job);
        }

        bool Loading = true;

        private void NavView_SelectionChanged(ModernWpf.Controls.NavigationView sender, ModernWpf.Controls.NavigationViewSelectionChangedEventArgs args)
            => NavView_SelectionChanged(sender);

        private async void NavView_SelectionChanged(ModernWpf.Controls.NavigationView sender)
        {
            if (Loading)
            {
                NavContent.Content = new MainPageViews.Loading();
            }
            else
            {
                if (NavContent.Content is UserControl)
                {
                    (NavContent.Content as UserControl)?.BeginAnimation(UserControl.OpacityProperty, new DoubleAnimation(1.0, 0.0, TimeSpan.FromSeconds(0.2)));
                    await Task.Delay(TimeSpan.FromSeconds(0.2));

                    UserControl view;

                    if (DataSave.ContainsKey(sender.SelectedItem))
                    {
                        view = DataSave[sender.SelectedItem];
                    }
                    else
                    {
                        switch (sender.SelectedItem)
                        {
                            default: view = new UserControl(); break;
                        }
                    }

                    view.Opacity = 0.0;
                    NavContent.Content = view;
                    (NavContent.Content as UserControl)?.BeginAnimation(UserControl.OpacityProperty, new DoubleAnimation(0.0, 1.0, TimeSpan.FromSeconds(0.2)));
                }
                else
                {
                    NavContent.Content = new UserControl() { Opacity = 0.0 };
                    (NavContent.Content as UserControl)?.BeginAnimation(UserControl.OpacityProperty, new DoubleAnimation(0.0, 1.0, TimeSpan.FromSeconds(0.2)));
                }
            }
        }
    }
}
