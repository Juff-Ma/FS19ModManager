using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FS19ModManager
{
    /// <summary>
    /// Interaktionslogik für Worker.xaml
    /// </summary>
    public partial class Worker : Window, INotifyPropertyChanged
    {

        private BackgroundWorker? worker;

        public delegate void JobDoneEventHandler(object? result, string name, Exception? e, Worker self);
        public event JobDoneEventHandler? JobDone;

        public void Quit()
        {
            if (running)
            {
                throw new Exception("still having jobs running. Are you subscribed to Worker.JobDone?");
            }
            else
            {
                closable = true;
                Close();
            }

        }

        bool closable = false;
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = !closable;
        }

        public Worker()
        {
            InitializeComponent();
            DataContext = this;
            running = false;
        }
        public Worker(Window parent) : this()
        {
            Owner = parent;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        bool running_;
        bool running { get { return running_; } set { running_ = value; OnPropertyChanged("Paused"); } }
        public bool Paused { get { return !running; } }

        string? prefix;
        string? suffix;
        public string CountText { get { return prefix + (hasCount ? Count : "") + (Bar.IsIndeterminate ? "" : ("/" + Bar.Maximum)) + suffix; } }
        public double Count { get { return Bar.Value; } set { Bar.Value = value; OnPropertyChanged("CountText"); } }
        bool hasCount = true;

        public struct Job
        {
            public DoWorkEventHandler job { get; set; }
            public bool hasProgress { get; set; }
            public bool hasCount { get; set; }
            public int? maxValue { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string prefix { get; set; }
            public string suffix { get; set; }
            public object? data { get; set; }
        }
        
        public void RunJobAsync(Job job)
        {
            if (!running)
            {
                running = true;
                Bar.ShowError = false;
                hasCount = job.hasCount;

                Title = job.name;
                Description.Content = job.description;

                Bar.IsIndeterminate = !job.hasProgress;
                if (job.maxValue != null)
                {
                    Bar.Maximum = (int)job.maxValue;
                }

                prefix = job.prefix;
                suffix = job.suffix;

                worker = new();
                worker.WorkerSupportsCancellation = false;
                worker.WorkerReportsProgress = job.hasCount || job.hasProgress;
                worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
                worker.ProgressChanged += Worker_ProgressChanged;

                worker.DoWork += job.job;

                worker.RunWorkerAsync(job.data);
            }
        }

        private void Worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            Count = e.ProgressPercentage;
            if (e.UserState is string)
            {
                Description.Content = e.UserState as string;
            }
        }

        private void Worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            running = false;
            
            if (e.Error != null)
            {
                Bar.ShowError = true;
            }

            JobDone?.Invoke(e.Result, Title, e.Error, this);
        }
    }
}
