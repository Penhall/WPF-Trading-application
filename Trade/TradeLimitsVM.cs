using CommonFrontEnd.Common;
using CommonFrontEnd.Global;
using CommonFrontEnd.Model.Trade;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.View.Trade;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.ViewModel.Trade
{
    class TradeLimitsVM : INotifyPropertyChanged
    {
        #region RelayCommand
        static TradeLimits mWindow = null;
        private string _txtMemberId;

        public string txtMemberId
        {
            get { return _txtMemberId; }
            set { _txtMemberId = value; NotifyPropertyChanged(nameof(txtMemberId)); }
        }

        private string _txtEqGrossBuy;

        public string txtEqGrossBuy
        {
            get { return _txtEqGrossBuy; }
            set { _txtEqGrossBuy = value; NotifyPropertyChanged(nameof(txtEqGrossBuy)); }
        }

        private string _txtEqGrossSell;

        public string txtEqGrossSell
        {
            get { return _txtEqGrossSell; }
            set { _txtEqGrossSell = value; NotifyPropertyChanged(nameof(txtEqGrossSell)); }
        }

        private string _txtEqTotalNetValue;

        public string txtEqTotalNetValue
        {
            get { return _txtEqTotalNetValue; }
            set { _txtEqTotalNetValue = value; NotifyPropertyChanged(nameof(txtEqTotalNetValue)); }
        }

        private string _txtEqCurrentNetValue;

        public string txtEqCurrentNetValue
        {
            get { return _txtEqCurrentNetValue; }
            set { _txtEqCurrentNetValue = value; NotifyPropertyChanged(nameof(txtEqCurrentNetValue)); }
        }

        private bool _chkUnrestrictedGrpLimit;

        public bool chkUnrestrictedGrpLimit
        {
            get { return _chkUnrestrictedGrpLimit; }
            set { _chkUnrestrictedGrpLimit = value; NotifyPropertyChanged(nameof(chkUnrestrictedGrpLimit)); }
        }

        private string _txtEqBuyNetQtyLimit;

        public string txtEqBuyNetQtyLimit
        {
            get { return _txtEqBuyNetQtyLimit; }
            set { _txtEqBuyNetQtyLimit = value; NotifyPropertyChanged(nameof(txtEqBuyNetQtyLimit)); }
        }

        private string _txtEqSellNetQtyLimit;

        public string txtEqSellNetQtyLimit
        {
            get { return _txtEqSellNetQtyLimit; }
            set { _txtEqSellNetQtyLimit = value; NotifyPropertyChanged(nameof(txtEqSellNetQtyLimit)); }
        }

        private bool _chk4L;

        public bool chk4L
        {
            get { return _chk4L; }
            set { _chk4L = value; NotifyPropertyChanged(nameof(chk4L)); }
        }

        private bool _chk6L;

        public bool chk6L
        {
            get { return _chk6L; }
            set { _chk6L = value; NotifyPropertyChanged(nameof(chk6L)); }
        }

        private bool _chkAuction;

        public bool chkAuction
        {
            get { return _chkAuction; }
            set { _chkAuction = value; NotifyPropertyChanged(nameof(chkAuction)); }
        }

        private bool _chkOddLot;

        public bool chkOddLot
        {
            get { return _chkOddLot; }
            set { _chkOddLot = value; NotifyPropertyChanged(nameof(chkOddLot)); }
        }

        private bool _chkBlockDeal;

        public bool chkBlockDeal
        {
            get { return _chkBlockDeal; }
            set { _chkBlockDeal = value; NotifyPropertyChanged(nameof(chkBlockDeal)); }
        }

        private bool _chkInstTrading;

        public bool chkInstTrading
        {
            get { return _chkInstTrading; }
            set { _chkInstTrading = value; NotifyPropertyChanged(nameof(chkInstTrading)); }
        }

        private string _txtDerGrossBuy;

        public string txtDerGrossBuy
        {
            get { return _txtDerGrossBuy; }
            set { _txtDerGrossBuy = value; NotifyPropertyChanged(nameof(txtDerGrossBuy)); }
        }

        private string _txtDerGrossSell;

        public string txtDerGrossSell
        {
            get { return _txtDerGrossSell; }
            set { _txtDerGrossSell = value; NotifyPropertyChanged(nameof(txtDerGrossBuy)); }
        }

        private string _txtDerTotlNetValue;

        public string txtDerTotlNetValue
        {
            get { return _txtDerTotlNetValue; }
            set { _txtDerTotlNetValue = value; NotifyPropertyChanged(nameof(txtDerTotlNetValue)); }
        }

        private string _txtDerCurNetValue;

        public string txtDerCurNetValue
        {
            get { return _txtDerCurNetValue; }
            set { _txtDerCurNetValue = value; NotifyPropertyChanged(nameof(txtDerCurNetValue)); }
        }

        private string _txtDerBuy;

        public string txtDerBuy
        {
            get { return _txtDerBuy; }
            set { _txtDerBuy = value; NotifyPropertyChanged(nameof(txtDerBuy)); }
        }

        private string _txtDerSell;

        public string txtDerSell
        {
            get { return _txtDerSell; }
            set { _txtDerSell = value; NotifyPropertyChanged(nameof(txtDerSell)); }
        }

        private string _txtCurrGrossBuy;

        public string txtCurrGrossBuy
        {
            get { return _txtCurrGrossBuy; }
            set { _txtCurrGrossBuy = value; NotifyPropertyChanged(nameof(txtCurrGrossBuy)); }
        }

        private string _txtCurrGrossSell;

        public string txtCurrGrossSell
        {
            get { return _txtCurrGrossSell; }
            set { _txtCurrGrossSell = value; NotifyPropertyChanged(nameof(txtCurrGrossSell)); }
        }

        private string _txtCurrTotlNetValue;

        public string txtCurrTotlNetValue
        {
            get { return _txtCurrTotlNetValue; }
            set { _txtCurrTotlNetValue = value; NotifyPropertyChanged(nameof(txtCurrTotlNetValue)); }
        }

        private string _txtCurrCurentNetValue;

        public string txtCurrCurentNetValue
        {
            get { return _txtCurrCurentNetValue; }
            set { _txtCurrCurentNetValue = value; NotifyPropertyChanged(nameof(txtCurrCurentNetValue)); }
        }

        private string _txtCurrBuy;

        public string txtCurrBuy
        {
            get { return _txtCurrBuy; }
            set { _txtCurrBuy = value; NotifyPropertyChanged(nameof(txtCurrBuy)); }
        }

        private string _txtCurrSell;

        public string txtCurrSell
        {
            get { return  _txtCurrSell; }
            set {  _txtCurrSell = value; NotifyPropertyChanged(nameof(txtCurrSell)); }
        }


        private string _txtTraderId;

        public string txtTraderId
        {
            get { return _txtTraderId; }
            set { _txtTraderId = value; NotifyPropertyChanged(nameof(txtTraderId)); }
        }
        private RelayCommand _InvokeGroupWiseLimits;
        public RelayCommand InvokeGroupWiseLimits
        {
            get
            {

                return _InvokeGroupWiseLimits ?? (_InvokeGroupWiseLimits = new RelayCommand(
                    (object e) => InvokeGroupWiseLimitsWindow()
                        ));
            }
        }
        private RelayCommand _CloseWindowsOnEscape;

        public RelayCommand CloseWindowsOnEscape
        {
            get
            {
                return _CloseWindowsOnEscape ?? (_CloseWindowsOnEscape = new RelayCommand(
                    (object e) => CloseWindowsOnEscape_Click()
                        ));
            }
        }

        
        private bool _isEnableAllControl = false;

        public bool IsInableAllControl
        {
            get { return _isEnableAllControl; }
            set { _isEnableAllControl = value; }
        }
        #endregion


        #region Cunstroctor

        public TradeLimitsVM()
        {
            FillData();
            mWindow = System.Windows.Application.Current.Windows.OfType<TradeLimits>().FirstOrDefault();
        }



        #endregion


        #region Methods
        private void FillData()
        { 
            txtMemberId = Convert.ToString(UtilityLoginDetails.GETInstance.MemberId);
            txtTraderId = Convert.ToString(UtilityLoginDetails.GETInstance.TraderId);
            txtEqGrossBuy = Convert.ToString(Limit.g_Limit[(int)Limit.ExchangeNum.BSE_EQT].GrossBuyLimit/100000);
            txtEqGrossSell = Convert.ToString(Limit.g_Limit[(int)Limit.ExchangeNum.BSE_EQT].GrossSellLimit/100000);
            txtEqTotalNetValue = Convert.ToString(Limit.g_Limit[(int)Limit.ExchangeNum.BSE_EQT].NetValue / 100000);
            txtEqCurrentNetValue = Convert.ToString(Limit.g_Limit[(int)Limit.ExchangeNum.BSE_EQT].CurrNetValue / 100000);
            //Equity
            chkUnrestrictedGrpLimit = !Limit.g_Limit[(int)Limit.ExchangeNum.BSE_EQT].UnrestGrpLimit;
            var item = Limit.g_Limit[(int)Limit.ExchangeNum.BSE_EQT].g_NetQtyLimit.FirstOrDefault();
            txtEqBuyNetQtyLimit = Convert.ToString(Limit.g_Limit[(int)Limit.ExchangeNum.BSE_EQT].g_NetQtyLimit[item.Key].BuyLimit);
            txtEqSellNetQtyLimit = Convert.ToString(Limit.g_Limit[(int)Limit.ExchangeNum.BSE_EQT].g_NetQtyLimit[item.Key].SellLimit);

            //Derivative
            txtDerGrossBuy = Convert.ToString(Limit.g_Limit[(int)Limit.ExchangeNum.BSE_EDRV].GrossBuyLimit / 100000);
            txtDerGrossSell = Convert.ToString(Limit.g_Limit[(int)Limit.ExchangeNum.BSE_EDRV].GrossSellLimit / 100000);
            txtDerTotlNetValue = Convert.ToString(Limit.g_Limit[(int)Limit.ExchangeNum.BSE_EDRV].NetValue / 100000);
            txtDerCurNetValue = Convert.ToString(Limit.g_Limit[(int)Limit.ExchangeNum.BSE_EDRV].CurrNetValue / 100000);

           
            var item1 = Limit.g_Limit[(int)Limit.ExchangeNum.BSE_EDRV].g_NetQtyLimit.FirstOrDefault();
            txtDerBuy = Convert.ToString(Limit.g_Limit[(int)Limit.ExchangeNum.BSE_EDRV].g_NetQtyLimit[item1.Key].BuyLimit);
            txtDerSell = Convert.ToString(Limit.g_Limit[(int)Limit.ExchangeNum.BSE_EDRV].g_NetQtyLimit[item1.Key].SellLimit);

            //Currency
            txtCurrGrossBuy = Convert.ToString(Limit.g_Limit[(int)Limit.ExchangeNum.BSE_CDRV].GrossBuyLimit / 100000);
            txtCurrGrossSell = Convert.ToString(Limit.g_Limit[(int)Limit.ExchangeNum.BSE_CDRV].GrossSellLimit / 100000);
            txtCurrTotlNetValue = Convert.ToString(Limit.g_Limit[(int)Limit.ExchangeNum.BSE_CDRV].NetValue / 100000);
            txtCurrCurentNetValue = Convert.ToString(Limit.g_Limit[(int)Limit.ExchangeNum.BSE_CDRV].CurrNetValue / 100000);


            item1 = Limit.g_Limit[(int)Limit.ExchangeNum.BSE_CDRV].g_NetQtyLimit.FirstOrDefault();
            txtCurrBuy = Convert.ToString(Limit.g_Limit[(int)Limit.ExchangeNum.BSE_CDRV].g_NetQtyLimit[item1.Key].BuyLimit);
            txtCurrSell = Convert.ToString(Limit.g_Limit[(int)Limit.ExchangeNum.BSE_CDRV].g_NetQtyLimit[item1.Key].SellLimit);

        }

        private void CloseWindowsOnEscape_Click()
        {
            mWindow?.Close();
            //throw new NotImplementedException();
        }

        private void InvokeGroupWiseLimitsWindow()
        {
            try
            {
                GroupWiseLimits oGroupWiseLimits = System.Windows.Application.Current.Windows.OfType<GroupWiseLimits>().FirstOrDefault();
                if (oGroupWiseLimits != null)
                {
                    oGroupWiseLimits.Show();
                    oGroupWiseLimits.Focus();
                }
                else
                {
                    oGroupWiseLimits = new GroupWiseLimits();
                    oGroupWiseLimits.Owner = System.Windows.Application.Current.MainWindow;
                    oGroupWiseLimits.Activate();
                    oGroupWiseLimits.Show();
                }

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        #endregion
        #region NotifyPropertyChangedEvent
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {

                var e = new PropertyChangedEventArgs(propertyName);

                this.PropertyChanged(this, e);

            }

        }

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged
                = delegate { };
        public static void NotifyStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
