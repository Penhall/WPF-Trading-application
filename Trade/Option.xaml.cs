using CommonFrontEnd.Common;
using CommonFrontEnd.ViewModel.Trade;

namespace CommonFrontEnd.View.Trade
{
    /// <summary>
    /// Interaction logic for Option.xaml
    /// </summary>
    public partial class Option : TitleBarHelperClass
    {
        public Option()
        {
            InitializeComponent();
#if TWS
            this.DataContext = new OptionVM();
#endif
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Hide();
        }
    }
}
