using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.CurveView
{
    public class CurveViewModel : ViewModelBase
    {
        private CurveModel _CurveData = new CurveModel();
        public CurveModel CurveData
        {
            get
            {
                return _CurveData;
            }
            set
            {
                Set(ref _CurveData, value);
            }
        }
        public CurveViewModel()
        {

        }
    }
}
