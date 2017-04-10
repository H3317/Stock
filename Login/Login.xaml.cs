using System;
using System.Windows;
using MahApps.Metro.Controls;
using GalaSoft.MvvmLight.Messaging;

namespace Stock.Login
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : MetroWindow
    {
        public Login()
        {
            InitializeComponent();
            //Messenger.Default.Register<string>(this,(msg) =>Msghandle(msg));
        }

        private void Msghandle(string msg)
        {
            switch (msg)
            {
                case "LoginOK":
                    {
                        MessageBox.Show("登录成功！\n" + tbuser.Text + "---" + pbpwd.Password);
                        DialogResult = true;
                    }
                    break;
                case "LoginError":
                    {
                        MessageBox.Show("登录失败！\n" + tbuser.Text + "---" + pbpwd.Password);
                    }
                    break;
            }

        }

        private void btok_Click(object sender, RoutedEventArgs e)
        {
            var isChecked = this.cbissaveuser.IsChecked;
            if (isChecked != null)
            {
                Properties.Settings.Default.chkissaveuser = (bool)isChecked;
                Properties.Settings.Default.Loginuser = (bool)isChecked?this.tbuser.Text:"";
            }
            Properties.Settings.Default.Save();
            DialogResult = true;
        }

        private void tbuser_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (this.pbpwd != null)
                this.pbpwd.Password = "";
        }

        private void MetroWindow_ContentRendered(object sender, EventArgs e)
        {
            this.tbuser.Text = Properties.Settings.Default.Loginuser;
            this.cbissaveuser.IsChecked = Properties.Settings.Default.chkissaveuser;
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Messenger.Default.Unregister<string>(this, (msg) => Msghandle(msg));
        }
    }
}
