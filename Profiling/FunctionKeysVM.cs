using CommonFrontEnd.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CommonFrontEnd.ViewModel.Profiling
{
    class FunctionKeysVM : BaseViewModel
    {

        #region Properties
        public static bool SaveSetting = false;
        public static string temp;
      
        private static bool _ChkAlternate;

        public static bool ChkAlternate
        {
            get { return _ChkAlternate; }
            set { _ChkAlternate = value;  NotifyStaticPropertyChanged(nameof(ChkAlternate)); }
        }

        

        private static bool _ChkExisting;

        public static bool ChkExisting
        {
            get { return _ChkExisting; }
            set { _ChkExisting = value; NotifyStaticPropertyChanged(nameof(ChkExisting)); }
        }

        private RelayCommand _Window_Closing;

        public RelayCommand Window_Closing
        {
            get { return _Window_Closing ?? (_Window_Closing = new RelayCommand((object e) => Window_ClosingClick(e))); }

        }

        
        #endregion

        #region Constructor

        public FunctionKeysVM()
        {
            temp = MainWindowVM.ShortCutKeysFlag;
            if (temp == "0")
            {
                ChkAlternate = true;
                ChkExisting = false;
            }
            else if(temp == "1")
            {
                ChkExisting = true;
                ChkAlternate = false;
            }
        }


        #endregion

        #region Methods
        
        private void Window_ClosingClick(object e)
        {
           
        }
        #endregion
    }
}
