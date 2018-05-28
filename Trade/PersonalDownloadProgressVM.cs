using CommonFrontEnd.Common;
using System.Windows.Input;

namespace CommonFrontEnd.ViewModel.Trade
{
    public class PersonalDownloadProgressVM : BaseViewModel
    {
        #region Properties

        #endregion

        #region RelayCommand
        //private RelayCommand _PersonalDownload_KeyDown;

        //public RelayCommand PersonalDownload_KeyDown
        //{
        //    get { return _PersonalDownload_KeyDown??(_PersonalDownload_KeyDown=new RelayCommand()); }
        //}

        private RelayCommand<KeyEventArgs> _PersonalDownload_KeyDown;

        public RelayCommand<KeyEventArgs> PersonalDownload_KeyDown
        {
            get
            {
                return _PersonalDownload_KeyDown ?? (_PersonalDownload_KeyDown = new RelayCommand<KeyEventArgs>(PersonalDownload_KeyDown_Click));
            }
            set { _PersonalDownload_KeyDown = value; }
        }

        private void PersonalDownload_KeyDown_Click(KeyEventArgs e)
        {
            //if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt)
            //{
            //    AltDown = true;

            if (e != null)
            {
                if (e.SystemKey == Key.F4 && (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt))
                {
                    e.Handled = true;
                }
                else if (e.SystemKey == Key.Space && (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt))
                {
                    e.Handled = true;
                }
                else if (e.SystemKey == Key.F4)
                {
                    e.Handled = true;
                }
            }
        }



        #endregion

        public PersonalDownloadProgressVM()
        {

        }
       
    }
}
