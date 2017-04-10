using GalaSoft.MvvmLight;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace Stock.MViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the LoginViewModel class.
        /// </summary>
        public LoginViewModel()
        {
            //Struser = DateTime.Now.ToString("yyMMdd");
            //TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            //TimeSpan ts1 = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            //TimeSpan ts2 = ts1 - ts;
            OK_Command = new RelayCommand(btok_Click);
            //Struser = Properties.Settings.Default.Loginuser;            
        }

        private string struser = string.Empty;
        public string Struser
        {
            get { return struser; }
            set { Set(ref struser, value); }
        }

        private string strpwd = string.Empty;

        public string Strpwd
        {
            get { return strpwd; }
            set { Set(ref strpwd, value); }
        }

        private void btok_Click()
        {
            Properties.Settings.Default.Loginuser = Properties.Settings.Default.chkissaveuser ? Struser : "";
            Properties.Settings.Default.Save();


            if (Struser.Equals("123") && Strpwd.Equals("123"))
            {            
                Messenger.Default.Send<string>("LoginOK");
            }
            else
            {
                Messenger.Default.Send<string>("LoginError");
            }
        }

        public ICommand OK_Command { get; set; }
    }

}