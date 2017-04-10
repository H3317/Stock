using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Stock.Models;

namespace Stock.GetData
{
    public class Detail : ViewModel
    {
        private DateTime _time;
        public DateTime Time
        {
            get { return _time; }
            set
            {
                if (value == _time) return;
                _time = value;
                OnPropertyChanged();
            }
        }

        private double _price;
        public double Price
        {
            get { return _price; }
            set 
            {
                if (Equals(value, _price)) return;
                _price = value;
                OnPropertyChanged();
            }
        }

        private double _pricechg;
        public double Pricechg
        {
            get { return _pricechg; }
            set
            {
                if (Equals(value, _pricechg)) return;
                _pricechg = value;
                OnPropertyChanged();
            }
        }

        private int _volume;
        public int Volume
        {
            get { return _volume; }
            set
            {
                if (value == _volume) return;
                _volume = value;
                OnPropertyChanged();
            }
        }

        private int _amount;
        public int Amount
        {
            get { return _amount; }
            set
            {
                if (value == _amount) return;
                _amount = value;
                OnPropertyChanged();
            }
        }

        private string _type;
        public string Type
        {
            get { return _type; }
            set
            {
                if (value == _type) return;
                _type = value;
                OnPropertyChanged();
            }
        }       
    }

    public class GetdataViewModel:ViewModel
    {
        public ObservableCollection<Detail> Detail { get; set; }
        public GetdataViewModel()
        {
            Detail = new ObservableCollection<Detail>();
        }  
        private string _date;
        public string Date
        {
            get { return _date; }
            set
            {
                if (value == _date) return;
                _date = value;
                OnPropertyChanged();
            }
        }

        private string _name = "sh600900";
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
                return this._getdata ?? (_getdata = new SimpleCommand{ 
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
                        Detail.Clear();
                    }));
                    string tableName = Name.Substring(2);
                    if (DbHelperSqLite.Exists("select count(*) from sqlite_master where type='table' and name='DataReceived_" + tableName + "'"))
                    {
                        if (DbHelperSqLite.Exists("select count(*) from DataReceived_" + tableName + " where tradedate = '" + Date + "' and status =1"))
                        {
                            Log.WriteLog("股票【" + tableName + "】【" + Date + "】的数据已获取，不必重新获取");
                            return;
                        }
                    }

                    string url = "http://market.finance.sina.com.cn/downxls.php?date=" + Date + "&symbol=" + Name;
                    WebClient myWebClient = new WebClient();
                    Byte[] pageData = myWebClient.DownloadData(url);
                    string pageHtml = Encoding.Default.GetString(pageData);
                    string[] list1 = pageHtml.Split('\n');
                    if (list1.Length <=5)
                    {
                        Log.WriteLog("股票【" + tableName + "】【" + Date + "】没有数据");
                        return;

                    }

                    ArrayList list = new ArrayList();
                    foreach (string m in list1)
                    {
                        if (m == list1[0])
                        {
                            continue;
                        }
                        string[] n = m.Split('\t');
                        if (n.Length >= 5)
                        {

                            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                            {
                                Detail.Add(new Detail
                                {
                                    Time = Pfun.StringtoDatetime1(Date + " " + n[0], "yyyy-MM-dd HH:mm:ss"),
                                    Price = Pfun.Stringtodouble1(n[1]),
                                    Pricechg = Pfun.Stringtodouble1(n[2]),
                                    Volume = Pfun.Stringtoint1(n[3]),
                                    Amount = Pfun.Stringtoint1(n[4]),
                                    Type = n[5]
                                });

                                list.Add("insert into Detail_" + tableName + "(tradetime,price,pricechg,volume,amount,type) " +
                                    "values ('" + Date + " " + n[0] + "'," + Pfun.Stringtodouble1(n[1]).ToString(CultureInfo.InvariantCulture) +
                                    "," + Pfun.Stringtodouble1(n[2]).ToString(CultureInfo.InvariantCulture) + "," + Pfun.Stringtoint1(n[3]).ToString() +
                                    "," + Pfun.Stringtoint1(n[4]).ToString() + ",'" + n[5] + "')");

                            }));
                        }
                    }

                    if (list.Count > 0)
                    {
                        if (!DbHelperSqLite.Exists("select count(*) from sqlite_master where type='table' and name='Detail_" + tableName + "'"))
                        {
                            string tableText = (string)DbHelperSqLite.GetSingle("select table_text from need_createtable where Table_name = 'Detail' Limit 1");
                            if (tableText == null)
                            {
                                Log.WriteLog("need_createtable表中没有Detail表的创建代码");
                                return;
                            }
                            tableText = tableText.Replace("XX", tableName);
                            DbHelperSqLite.ExecuteSql(tableText);
                        }
                        DbHelperSqLite.ExecuteSqlTran(list);

                        if (!DbHelperSqLite.Exists("select count(*) from sqlite_master where type='table' and name='DataReceived_" + tableName + "'"))
                        {
                            string tableText = (string)DbHelperSqLite.GetSingle("select table_text from need_createtable where Table_name = 'DataReceived' Limit 1");
                            if (tableText == null)
                            {
                                Log.WriteLog("need_createtable表中没有DataReceived表的创建代码");
                                return;
                            }
                            tableText = tableText.Replace("XX", tableName);
                            DbHelperSqLite.ExecuteSql(tableText);
                        }
                        if (DbHelperSqLite.Exists("select count(*) from DataReceived_" + tableName + " where tradedate  ='" + Date + "'"))
                        {
                            DbHelperSqLite.ExecuteSql("update DataReceived_" + tableName + " set status = 1 where tradedate  ='" + Date + "'");
                        }
                        else
                        {
                            DbHelperSqLite.ExecuteSql("insert into Datareceived_" + tableName + "(tradedate,status) values('" + Date + "',1) ");
                        }
                        Log.WriteLog("股票【" + tableName + "】【" + Date + "】的数据获取成功");
                    }
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
