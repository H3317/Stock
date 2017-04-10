using MahApps.Metro.Controls;

namespace Stock.GetData
{
    /// <summary>
    /// GetDataView.xaml 的交互逻辑
    /// </summary>
    public partial class GetDataView : MetroWindow
    {

        public GetDataView()
        {
            InitializeComponent();
            this.DataContext = new GetdataViewModel();
        }

    }
}
