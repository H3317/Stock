using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Input;
using System.Threading;
using System.Threading.Tasks;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Stock.Models;
using Stock.ReportView;

namespace Stock
{
    public class AccentColorMenuData
    {
        public string Name { get; set; }
        public Brush BorderColorBrush { get; set; }
        public Brush ColorBrush { get; set; }

        private ICommand _changeAccentCommand;

        public ICommand ChangeAccentCommand
        {
            get { return this._changeAccentCommand ?? (_changeAccentCommand = new SimpleCommand { CanExecuteDelegate = x => true, ExecuteDelegate = x => this.DoChangeTheme(x) }); }
        }

        protected virtual void DoChangeTheme(object sender)
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            var accent = ThemeManager.GetAccent(this.Name);
            ThemeManager.ChangeAppStyle(Application.Current, accent, theme.Item1);
            Properties.Settings.Default.skin = this.Name;
            Properties.Settings.Default.Save();
        }
    }

    public class AppThemeMenuData : AccentColorMenuData
    {
        protected override void DoChangeTheme(object sender)
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            var appTheme = ThemeManager.GetAppTheme(this.Name);
            ThemeManager.ChangeAppStyle(Application.Current, theme.Item2, appTheme);
        }
    }


    public class MainWindowViewModel : INotifyPropertyChanged, IDataErrorInfo, IDisposable
    {
        private readonly IDialogCoordinator _dialogCoordinator;
        public List<AccentColorMenuData> AccentColors { get; set; }

        public MainWindowViewModel(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;
            this.AccentColors = ThemeManager.Accents
                                            .Select(a => new AccentColorMenuData() { 
                                                Name = a.Name, 
                                                ColorBrush = a.Resources["AccentColorBrush"] as Brush })
                                            .ToList();
            if (!string.IsNullOrEmpty(Properties.Settings.Default.skin))
            {
                var theme = ThemeManager.DetectAppStyle(Application.Current);
                var accent = ThemeManager.GetAccent(Properties.Settings.Default.skin);
                ThemeManager.ChangeAppStyle(Application.Current, accent, theme.Item1);
            }
         }

        public void Dispose()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "IntegerGreater10Property" )
                {
                    return "Number is not greater than 10!";
                }

                if (columnName == "DatePickerDate")
                {
                    return "No date given!";
                }

                return null;
            }
        }

        public string Error { get { return string.Empty; } }


        private ICommand _showCustomDialogCommand;

        public ICommand ShowCustomDialogCommand
        {
            get
            {
                return this._showCustomDialogCommand ?? (this._showCustomDialogCommand = new SimpleCommand
                {
                    CanExecuteDelegate = x => true,
                    ExecuteDelegate = x => RunCustomFromVm()
                });
            }
        }
        private async void RunCustomFromVm()
        {
            var customDialog = new CustomDialog();
            var customDialogExampleContent = new ReportViewModel(instance =>
            {
                _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
            });
            customDialog.Content = new ReportView.ReportView { DataContext = customDialogExampleContent };
            customDialog.Title = ((ReportView.ReportView)customDialog.Content).Tag.ToString();
            await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
        }

        public class RandomDataTemplateSelector : DataTemplateSelector
        {
            public DataTemplate TemplateOne { get; set; }

            public override DataTemplate SelectTemplate(object item, DependencyObject container)
            {
                return TemplateOne;
            }
        }

    }
}
