using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.IO;
using System.Threading;
using System.Globalization;
using Stock.GetAllNunber;
using Stock.GetBaseData;
using Stock.GetData;
using System.Threading.Tasks;

namespace Stock
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        const string fileNameen = "pack://siteoforigin:,,,/Resources/en.xaml";
        const string fileNamezh = "pack://siteoforigin:,,,/Resources/zh.xaml";

        public MainWindow()
        {
            Thread.CurrentThread.CurrentCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();

            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            InitializeComponent();
            var viewModel = new MainWindowViewModel(DialogCoordinator.Instance);
            DataContext = viewModel;

            

        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void MetroWindow_ContentRendered(object sender, EventArgs e)
        {
            Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary()
            {
                Source = new Uri(Properties.Settings.Default.lang == "en" ? fileNameen : fileNamezh, UriKind.Absolute)
            };
            SignIn(true);
        }
        
        private void SignIn(bool firstopen)
        {
            Login.Login login = new Login.Login{Owner = this};
            Opacity = 0.5;
            var showDialog = login.ShowDialog();
            if (showDialog != null && (bool)showDialog)
            {
                Opacity = 1.0;
            }
            else
            {
                Opacity = 1.0;
                if (firstopen)
                    Close();
            }

            login = null;
            GC.Collect();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SignIn(false);
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            //contentcontrol1.Content = new DataView();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            //var customDialog = new CustomDialog() { Title = "ReportView" };
            //customDialog.Content = new ReportView();
            //await _dialogCoordinator.ShowMetroDialogAsync(this.DataContext, customDialog);
        }

        private void MenuItem_Click_2(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var windowFlg = false;

            GetDataView getdataview = new GetDataView();

            foreach (Window m in Application.Current.Windows)
            {
                if (m is GetDataView)
                {
                    windowFlg = true;
                    getdataview = m as GetDataView;
                    getdataview.Visibility = System.Windows.Visibility.Visible;
                    getdataview.Show();
                    getdataview.Activate();
                    break;
                }
            }
            if (!windowFlg)
            {
                getdataview = new GetDataView();
                getdataview.Show();
            }
            //getdataview.Owner = this;
            getdataview.Show();
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            CancellationToken token;
            TaskScheduler uiSched = TaskScheduler.FromCurrentSynchronizationContext();
            Task.Factory.StartNew(DialogsBeforeExit, token, TaskCreationOptions.None, uiSched);
        }
        private async void DialogsBeforeExit()
        {
            MessageDialogResult mresult = await this.ShowMessageAsync(
                Application.Current.Resources["Coninfo"].ToString(),
                Application.Current.Resources["IsExit"].ToString(),
                MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings
                {
                    AffirmativeButtonText = Application.Current.Resources["Yes"].ToString(),
                    NegativeButtonText = Application.Current.Resources["No"].ToString(),
                    ColorScheme = MetroDialogColorScheme.Accented
                });
            if (mresult == MessageDialogResult.Negative)
            {
                return;
            }
            else
            {
                foreach (Window m in Application.Current.Windows)
                {
                    if (!(m is MainWindow))
                        m.Close();
                }
                Application.Current.Shutdown();
            }
        }

        private void MenuItem_Click_3(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            //Boolean Window_flg;
            //Window_flg = false;

            GetAllNumber view = new GetAllNumber();
            this.Opacity = 0.7;
            view.Owner = this;
            view.ShowDialog();
            Opacity = 1.0;
            GC.Collect();
            //foreach (Window m in App.Current.Windows)
            //{
            //    if (m is GetAllNumber)
            //    {
            //        Window_flg = true;
            //        view = m as GetAllNumber;
            //        view.Visibility = System.Windows.Visibility.Visible;
            //        view.Show();
            //        view.Activate();
            //        break;
            //    }
            //}
            //if (!Window_flg)
            //{
            //    view = new GetAllNumber();
            //    view.Show();
            //}
            ////getdataview.Owner = this;
            //view.Show();

        }

        private void MenuItem_Click_4(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var windowFlg = false;

            GetBaseDataView getdataview = new GetBaseDataView();

            foreach (Window m in Application.Current.Windows)
            {
                if (m is GetBaseDataView)
                {
                    windowFlg = true;
                    getdataview = m as GetBaseDataView;
                    getdataview.Visibility = System.Windows.Visibility.Visible;
                    getdataview.Show();
                    getdataview.Activate();
                    break;
                }
            }
            if (!windowFlg)
            {
                getdataview = new GetBaseDataView();
                getdataview.Show();
            }
            //getdataview.Owner = this;
            getdataview.Show();

        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.lang = Properties.Settings.Default.lang == "zh" ? "en" : "zh";
            Properties.Settings.Default.Save();
            Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary()
            {
                Source = new Uri(Properties.Settings.Default.lang == "en" ? fileNameen : fileNamezh, UriKind.Absolute)
            };
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {            
            Close();
        }
    }

    public class Log
    {
        public static void WriteLog(string msg)
        {
            try
            {
                DirectoryInfo d = Directory.CreateDirectory(Directory.GetCurrentDirectory() + "//Log");
                FileStream fs = new FileStream(Directory.GetCurrentDirectory() + "//Log/" + DateTime.Now.ToString("yyyy-MM-dd") + ".Log", System.IO.FileMode.Append);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "\r\n" + msg);
                sw.Close();
                fs.Close();
            }
            catch (Exception)
            {
                //
            }
        }
    } 
}
