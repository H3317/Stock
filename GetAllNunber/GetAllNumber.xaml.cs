using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;

namespace Stock.GetAllNunber
{
    /// <summary>
    /// GetAllNumber.xaml 的交互逻辑
    /// </summary>
    public partial class GetAllNumber : MetroWindow
    {
        public GetAllNumber()
        {
            InitializeComponent();
            this.DataContext = new GetAllNumberViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Dg.Visibility == System.Windows.Visibility.Visible)
            {
                Dg.Visibility = System.Windows.Visibility.Collapsed;

                var button = sender as Button;
                if (button != null)
                    button.Content = new Image
                    {
                        Source = new BitmapImage(new Uri("pack://application:,,,/Images/arrow-down-o.png")),
                        Stretch = Stretch.Uniform
                    };
            }
            else
            {
                Dg.Visibility = System.Windows.Visibility.Visible;
                var button = sender as Button;
                if (button != null)
                    button.Content = new Image
                    {
                        Source = new BitmapImage(new Uri("pack://application:,,,/Images/arrow-top-o.png")),
                        Stretch = Stretch.Uniform
                    };
            }
        }
    }
}
