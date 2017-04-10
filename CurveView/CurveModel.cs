
using GalaSoft.MvvmLight;

namespace Stock.CurveView
{
    public class CurveModel : ObservableObject
    {
        public CurveModel()
        {

        }

		private string _Title =  "hellow";
        public string Title
        {
            get{ return _Title;}
            set{ Set(ref _Title, value);}
        }

		private string _Msg =  "show";
        public string Msg
        {
            get{ return _Msg;}
            set{ Set(ref _Msg, value);}
        }

		private int _sss =  1;
        public int sss
        {
            get{ return _sss;}
            set{ Set(ref _sss, value);}
        }

		private int _ddd = 0;
        public int ddd
        {
            get{ return _ddd;}
            set{ Set(ref _ddd, value);}
        }

		private object _tttt = null;
        public object tttt
        {
            get{ return _tttt;}
            set{ Set(ref _tttt, value);}
        }

		private object _sr =  null;
        public object sr
        {
            get{ return _sr;}
            set{ Set(ref _sr, value);}
        }

		private bool _bols = false;//fff
        public bool bols
        {
            get{ return _bols;}
            set{ Set(ref _bols, value);}
        }

		//fffff
		private bool _bolt =  true;//gggg
        public bool bolt
        {
            get{ return _bolt;}
            set{ Set(ref _bolt, value);}
        }

	}
}