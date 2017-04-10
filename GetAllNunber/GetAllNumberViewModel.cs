using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Stock.Models;

namespace Stock.GetAllNunber
{
    public class Allnumber : ViewModel
    {
        private string _number;
        public string Number
        {
            get { return _number; }
            set
            {
                if (value == _number) return;
                _number = value;
                OnPropertyChanged();
            }
        }

        private string _no;
        public string No
        {
            get { return _no; }
            set
            {
                if (value == _no) return;
                _no = value;
                OnPropertyChanged();
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }
    }

    public class GetAllNumberViewModel:ViewModel
    {
        public ObservableCollection<Allnumber> Allnumber { get; set; }

        public GetAllNumberViewModel()
        {
            Allnumber = new ObservableCollection<Allnumber>();
        }
        private bool _enable = true;
        public bool Enable
        {
            get { return _enable; }
            set
            {
                if (value == _enable) return;
                _enable = value;
                OnPropertyChanged();
            }
        }

        private ICommand _getdata;
        public ICommand Getdata
        {
            get
            {
                return this._getdata ?? (_getdata = new SimpleCommand
                {
                    CanExecuteDelegate = x => true,
                    ExecuteDelegate = x => this.OnGetdata(x)
                });
            }
        }
        private void OnGetdata(object obj)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    Enable = false;
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        Allnumber.Clear();
                    }));

                    string url = "http://quote.eastmoney.com/stocklist.html";
                    WebClient myWebClient = new WebClient();
                    Byte[] pageData = myWebClient.DownloadData(url);
                    string pageHtml = Encoding.Default.GetString(pageData);

                    Regex reg = new Regex(@"<a target=""_blank"" href=""http://quote.eastmoney.com/s(h|z)(6|0|3)([^<])+</a>", RegexOptions.IgnoreCase);
                    Match ml = reg.Match(pageHtml);
                    ArrayList list = new ArrayList();
                    if (ml.Success)
                    {
                        DbHelperSqLite.ExecuteSql("delete from gp_all;delete from sqlite_sequence where name = 'gp_all' ");
                    }

                    while (ml.Success)
                    {
                        string a = ml.Result("$0");
                        a = a.Replace("<a target=\"_blank\" href=\"http://quote.eastmoney.com/", "");
                        a = a.Replace(".html\">", "(");
                        a = a.Replace(")</a>", "");
                        string[] b = a.Split('(');
                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            Allnumber.Add(new Allnumber
                            {
                                Number = b[0],
                                No = b[2],
                                Name = b[1]
                            });
                        }));
                        ml = ml.NextMatch();
                        list.Add("insert into gp_all(name,number,no) values ('" + b[1] + "','" + b[0] + "','" + b[2] + "')");
                    }
                    if (list.Count > 0)
                        DbHelperSqLite.ExecuteSqlTran(list);
                }
                catch(Exception ex)
                {
                    Log.WriteLog(ex.Message);
                }
                finally
                {
                    Enable = true;
                }
            });
        }
    }
}
