using MahApps.Metro.Controls;

namespace Stock.GetBaseData
{
    /// <summary>
    /// GetDataView.xaml 的交互逻辑
    /// </summary>
    public partial class GetBaseDataView : MetroWindow
    {

        public GetBaseDataView()
        {
            InitializeComponent();
            this.DataContext = new GetBasedataViewModel();
        }

    }
}
