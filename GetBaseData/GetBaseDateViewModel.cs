using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Stock.Models;
using Application = System.Windows.Application;
using sy = System.Windows.Forms;
using System.Collections;

namespace Stock.GetBaseData
{
    public class BaseDetail : ViewModel
    {
        private string _time;
        public string Time
        {
            get { return _time; }
            set
            {
                if (value == _time) return;
                _time = value;
                OnPropertyChanged();
            }
        }

        private double _kp;
        public double Kp
        {
            get { return _kp; }
            set 
            {
                if (Equals(value, _kp)) return;
                _kp = value;
                OnPropertyChanged();
            }
        }

        private double _zg;
        public double Zg
        {
            get { return _zg; }
            set
            {
                if (Equals(value, _zg)) return;
                _zg = value;
                OnPropertyChanged();
            }
        }

        private double _zd;
        public double Zd
        {
            get { return _zd; }
            set
            {
                if (Equals(value, _zd)) return;
                _zd = value;
                OnPropertyChanged();
            }
        }

        private double _sp;
        public double Sp
        {
            get { return _sp; }
            set
            {
                if (Equals(value, _sp)) return;
                _sp = value;
                OnPropertyChanged();
            }
        }

        private double _cjl;
        public double Cjl
        {
            get { return _cjl; }
            set
            {
                if (Equals(value, _cjl)) return;
                _cjl = value;
                OnPropertyChanged();
            }
        }

        private double _cjje;
        public double Cjje
        {
            get { return _cjje; }
            set
            {
                if (Equals(value, _cjje)) return;
                _cjje = value;
                OnPropertyChanged();
            }
        }

        private double _sdl;
        public double Sdl
        {
            get { return _sdl; }
            set
            {
                if (Equals(value, _sdl)) return;
                _sdl = value;
                OnPropertyChanged();
            }
        }

        private double _sdbl;
        public double Sdbl
        {
            get { return _sdbl; }
            set
            {
                if (Equals(value, _sdbl)) return;
                _sdbl = value;
                OnPropertyChanged();
            }
        }

        private int _s;
        public int S
        {
            get { return _s; }
            set
            {
                if (value == _s) return;
                _s = value;
                OnPropertyChanged();
            }
        }

        private double _gdc;
        public double Gdc
        {
            get { return _gdc; }
            set
            {
                if (Equals(value, _gdc)) return;
                _gdc = value;
                OnPropertyChanged();
            }
        }

        private double _sz;
        public double Sz
        {
            get { return _sz; }
            set
            {
                if (Equals(value, _sz)) return;
                _sz = value;
                OnPropertyChanged();
            }
        }

        private double _szl;
        public double Szl
        {
            get { return _szl; }
            set
            {
                if (Equals(value, _szl)) return;
                _szl = value;
                OnPropertyChanged();
            }
        }


    }

    public class GetBasedataViewModel:ViewModel
    {
        public ObservableCollection<BaseDetail> Detail { get; set; }
        public GetBasedataViewModel()
        {
            Detail = new ObservableCollection<BaseDetail>();
        } 
        private string _name = "600900";
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
                    //Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    //{
                    //}));

                    string tableName = Name;
                    if (DbHelperSqLite.Exists("select count(*) from sqlite_master where type='table' and name='BaseDataReceived_" + tableName + "'"))
                    {
                        if (DbHelperSqLite.Exists("select count(*) from BaseDataReceived_" + tableName + " where status = 1"))
                        {
                            Log.WriteLog("股票【" + tableName + "】的数据已获取，不必重新获取");
                            return;
                        }
                    }

                    string url = "http://www.aigaogao.com/tools/history.html?s=" + Name;
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        Detail.Clear();
                        sy.WebBrowser webBrowser = new sy.WebBrowser();
                        webBrowser.Navigate(url);
                        webBrowser.DocumentCompleted += new sy.WebBrowserDocumentCompletedEventHandler(web_DocumentCompleted);

                    }));
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

        private void web_DocumentCompleted(object sender,sy.WebBrowserDocumentCompletedEventArgs e)
        {
            sy.WebBrowser web = (sy.WebBrowser)sender;
            if (web.Document != null)
            {
                sy.HtmlElementCollection tbs = web.Document.GetElementsByTagName("TABLE");
                if (tbs.Count == 8)
                {
                    sy.HtmlElementCollection trs = tbs[7].GetElementsByTagName("TR");

                    ArrayList list = new ArrayList();
                    for (int j = 1; j < trs.Count; j++)
                    {
                        sy.HtmlElementCollection tds = trs[j].GetElementsByTagName("TD");
                        if (tds.Count == 13)
                        {
                            var bd = new BaseDetail
                            {
                                Time = Pfun.StringtoDatetime1(tds[0].InnerText.Trim(), "MM/dd/yyyy").ToString("yyyyMMdd"),
                                Kp = Pfun.Stringtodouble1(tds[1].InnerText.Trim()),
                                Zg = Pfun.Stringtodouble1(tds[2].InnerText.Trim()),
                                Zd = Pfun.Stringtodouble1(tds[3].InnerText.Trim()),
                                Sp = Pfun.Stringtodouble1(tds[4].InnerText.Trim()),
                                Cjl = Pfun.Stringtodouble1(tds[5].InnerText.Replace(",", "").Trim()),
                                Cjje = Pfun.Stringtodouble1(tds[6].InnerText.Replace(",", "").Trim()),
                                Sdl = Pfun.Stringtodouble1(tds[7].InnerText.Trim()),
                                Sdbl = Pfun.Stringtodouble1(tds[8].InnerText.Replace("%", "").Trim()),
                                S = Pfun.Stringtoint1(tds[9].InnerText),
                                Gdc = Pfun.Stringtodouble1(tds[10].InnerText.Replace("%", "").Trim()),
                                Sz = Pfun.Stringtodouble1(tds[11].InnerText.Trim()),
                                Szl = Pfun.Stringtodouble1(tds[12].InnerText.Replace("%", "").Trim()),
                            };
                            Detail.Add(bd);

                            list.Add("insert into BaseData_" + Name + "(tradetime,kpjg,zgjg,zdjg,spjg,cjl,cjje,sd,sdbl) " +
                                    "values ('" + bd.Time + "'," + bd.Kp +
                                    "," + bd.Zg + "," + bd.Zd +
                                    "," + bd.Sp + "," + bd.Cjl + "," + bd.Cjje + "," + bd.Sdl + "," + bd.Sdbl + ")");

                        }
                    }

                    if (list.Count > 0)
                    {
                        if (!DbHelperSqLite.Exists("select count(*) from sqlite_master where type='table' and name='BaseData_" + Name + "'"))
                        {
                            string tableText = (string)DbHelperSqLite.GetSingle("select table_text from need_createtable where Table_name = 'BaseData' Limit 1");
                            if (tableText == null)
                            {
                                Log.WriteLog("need_createtable表中没有BaseData_表的创建代码");
                                return;
                            }
                            tableText = tableText.Replace("XX", Name);
                            DbHelperSqLite.ExecuteSql(tableText);
                        }
                        DbHelperSqLite.ExecuteSqlTran(list);

                        if (!DbHelperSqLite.Exists("select count(*) from sqlite_master where type='table' and name='BaseDataReceived_" + Name + "'"))
                        {
                            string tableText = (string)DbHelperSqLite.GetSingle("select table_text from need_createtable where Table_name = 'BaseDataReceived' Limit 1");
                            if (tableText == null)
                            {
                                Log.WriteLog("need_createtable表中没有BaseDataReceived表的创建代码");
                                return;
                            }
                            tableText = tableText.Replace("XX", Name);
                            DbHelperSqLite.ExecuteSql(tableText);
                        }
                        if (DbHelperSqLite.Exists("select count(*) from BaseDataReceived_" + Name))
                        {
                            DbHelperSqLite.ExecuteSql("update BaseDataReceived_" + Name + " set status = 1 where tradedate  ='" + DateTime.Now.ToString("yyyyMMdd") + "'");
                        }
                        else
                        {
                            DbHelperSqLite.ExecuteSql("insert into BaseDatareceived_" + Name + "(tradedate,status) values('" + DateTime.Now.ToString("yyyyMMdd") + "',1) ");
                        }
                        Log.WriteLog("股票【" + Name + "】的数据获取成功");
                    }
                }
            }
        }
    }
}
