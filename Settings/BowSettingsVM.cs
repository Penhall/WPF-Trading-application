using CommonFrontEnd.Common;
using CommonFrontEnd.View.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommonFrontEnd.ViewModel.Settings
{
#if BOW

    class BowSettingsVM : INotifyPropertyChanged
    {


#region Properties

        private string _MainServerIpAddresstxt;

        public string MainServerIpAddresstxt
        {
            get { return _MainServerIpAddresstxt; }
            set { _MainServerIpAddresstxt = value; NotifyPropertyChanged("MainServerIpAddresstxt"); }
        }

        private string _txtInteractiveIPLan;

        public string txtInteractiveIPLan
        {
            get { return _txtInteractiveIPLan; }
            set { _txtInteractiveIPLan = value; NotifyPropertyChanged("txtInteractiveIPLan"); }
        }

        private string _InteractivePortLan;

        public string InteractivePortLan
        {
            get { return _InteractivePortLan; }
            set { _InteractivePortLan = value; NotifyPropertyChanged("InteractivePortLan"); }
        }

        private string _txtBroadcastIP;

        public string txtBroadcastIP
        {
            get { return _txtBroadcastIP; }
            set { _txtBroadcastIP = value; NotifyPropertyChanged("txtBroadcastIP"); }
        }

        private string _BroadcastPort;

        public string BroadcastPort
        {
            get { return _BroadcastPort; }
            set { _BroadcastPort = value; NotifyPropertyChanged("BroadcastPort"); }
        }

        private string _txtEquityIP1;

        public string txtEquityIP1
        {
            get { return _txtEquityIP1; }
            set { _txtEquityIP1 = value; NotifyPropertyChanged("txtEquityIP1"); }
        }

        private string _EquityPort1;

        public string EquityPort1
        {
            get { return _EquityPort1; }
            set { _EquityPort1 = value; NotifyPropertyChanged("EquityPort1"); }
        }

        private string _txtDerivativeIP1;

        public string txtDerivativeIP1
        {
            get { return _txtDerivativeIP1; }
            set { _txtDerivativeIP1 = value; NotifyPropertyChanged("txtDerivativeIP1"); }
        }

        private string _txtCurrencyIP1;

        public string txtCurrencyIP1
        {
            get { return _txtCurrencyIP1; }
            set { _txtCurrencyIP1 = value; NotifyPropertyChanged("txtCurrencyIP1"); }
        }

        private string _txtCommodityIP1;

        public string txtCommodityIP1
        {
            get { return _txtCommodityIP1; }
            set { _txtCommodityIP1 = value; NotifyPropertyChanged("txtCommodityIP1"); }
        }

        private string _DerivativePort1;

        public string DerivativePort1
        {
            get { return _DerivativePort1; }
            set { _DerivativePort1 = value; NotifyPropertyChanged("DerivativePort1"); }
        }

        private string _CurrencyPort1;

        public string CurrencyPort1
        {
            get { return _CurrencyPort1; }
            set { _CurrencyPort1 = value; NotifyPropertyChanged("CurrencyPort1"); }
        }

        private string _CommodityPort1;

        public string CommodityPort1
        {
            get { return _CommodityPort1; }
            set { _CommodityPort1 = value; NotifyPropertyChanged("CommodityPort1"); }
        }

        private string _txtFTPServerIP;

        public string txtFTPServerIP
        {
            get { return _txtFTPServerIP; }
            set { _txtFTPServerIP = value; NotifyPropertyChanged("txtFTPServerIP"); }
        }

        private string _txtInterfaceIP;

        public string txtInterfaceIP
        {
            get { return _txtInterfaceIP; }
            set { _txtInterfaceIP = value; NotifyPropertyChanged("txtInterfaceIP"); }
        }

        private string _txtClientListenIP;

        public string txtClientListenIP
        {
            get { return _txtClientListenIP; }
            set { _txtClientListenIP = value; NotifyPropertyChanged("txtClientListenIP"); }
        }

        private string _ClientListenPort;

        public string ClientListenPort
        {
            get { return _ClientListenPort; }
            set { _ClientListenPort = value; NotifyPropertyChanged("ClientListenPort"); }
        }

        //private string _BroadcastOverHTTPChecked;

        //public string BroadcastOverHTTPChecked
        //{
        //    get { return _BroadcastOverHTTPChecked; }
        //    set { _BroadcastOverHTTPChecked = value; NotifyPropertyChanged("BroadcastOverHTTPChecked"); }
        //}

        private string _MainServerIpAddressDR;

        public string MainServerIpAddressDR
        {
            get { return _MainServerIpAddressDR; }
            set { _MainServerIpAddressDR = value; NotifyPropertyChanged("MainServerIpAddressDR"); }
        }

        private string _txtInteractiveIPDR;

        public string txtInteractiveIPDR
        {
            get { return _txtInteractiveIPDR; }
            set { _txtInteractiveIPDR = value; NotifyPropertyChanged("txtInteractiveIPDR"); }
        }

        private string _InteractivePortDR;

        public string InteractivePortDR
        {
            get { return _InteractivePortDR; }
            set { _InteractivePortDR = value; NotifyPropertyChanged("InteractivePortDR"); }
        }

       
        private string _txtBroadcastIPDR;

        public string txtBroadcastIPDR
        {
            get { return _txtBroadcastIPDR; }
            set
            {
                _txtBroadcastIPDR = value;
                NotifyPropertyChanged("txtBroadcastIPDR");
            }
        }

        private string _BroadcastPortDR;

        public string BroadcastPortDR
        {
            get { return _BroadcastPortDR; }
            set
            {
                _BroadcastPortDR = value;
                NotifyPropertyChanged("BroadcastPortDR");
            }
        }

        private string _SelectedConnectionType;
        public string SelectedConnectionType
        {
            get { return _SelectedConnectionType; }
            set { _SelectedConnectionType = value; NotifyPropertyChanged("SelectedConnectionType");
                OnChangeOfConnectionType();
              
            }
        }

        private List<string> _ConnectionType;
        public List<string> ConnectionType
        {
            get { return _ConnectionType; }
            set { _ConnectionType = value; NotifyPropertyChanged("ConnectionType");
                BindData();

            }
        }

        private string _SelectedSocketCompression;
        public string SelectedSocketCompression
        {
            get { return _SelectedSocketCompression; }
            set
            {
                _SelectedSocketCompression = value;
                NotifyPropertyChanged("SelectedSocketCompression");
               
            }
        }

        private List<string> _SocketCompression;
        public List<string> SocketCompression
        {
            get { return _SocketCompression; }
            set
            {
                _SocketCompression = value;
                NotifyPropertyChanged("SocketCompression");
             
            }
        }

        private string _BtnCancelEnable;

        public string BtnCancelEnable
        {
            get { return _BtnCancelEnable; }
            set { _BtnCancelEnable = value; NotifyPropertyChanged(nameof(BtnCancelEnable)); }
        }


        private string _BtnSaveEnable;

        public string BtnSaveEnable
        {
            get { return _BtnSaveEnable; }
            set { _BtnSaveEnable = value; NotifyPropertyChanged(nameof(BtnSaveEnable)); }
        }

        private string _BtnEditEnable;

        public string BtnEditEnable
        {
            get { return _BtnEditEnable; }
            set { _BtnEditEnable = value; NotifyPropertyChanged(nameof(BtnEditEnable)); }
        }

        private string _txtEquityIPDR;

        public string txtEquityIPDR
        {
            get { return _txtEquityIPDR; }
            set { _txtEquityIPDR = value; NotifyPropertyChanged(nameof(txtEquityIPDR)); }
        }

        private static string _CtrEnable;

        public static string CtrEnable
        {
            get { return _CtrEnable; }
            set { _CtrEnable = value; NotifyStaticPropertyChanged("CtrEnable"); }
        }

      

        private string _LanVisibility;

        public string LanVisibility
        {
            get { return _LanVisibility; }
            set
            {
                _LanVisibility = value; NotifyPropertyChanged("LanVisibility");
                
            }
        }

        private string _InternetVisibility;

        public string InternetVisibility
        {
            get { return _InternetVisibility; }
            set
            {
                _InternetVisibility = value; NotifyPropertyChanged("InternetVisibility");
                
            }
        }

     

        private string _ConnectionTypeEnabled;

        public string ConnectionTypeEnabled
        {
            get { return _ConnectionTypeEnabled; }
            set { _ConnectionTypeEnabled = value; NotifyPropertyChanged("ConnectionTypeEnabled"); }
        }

        private string _EquityPortDR;

        public string EquityPortDR
        {
            get { return _EquityPortDR; }
            set { _EquityPortDR = value; NotifyPropertyChanged("EquityPortDR"); }
        }

        private string _txtDerivativeIPDR;

        public string txtDerivativeIPDR
        {
            get { return _txtDerivativeIPDR; }
            set { _txtDerivativeIPDR = value; NotifyPropertyChanged("txtDerivativeIPDR"); }
        }

        private string _ReceiverPortVisibility;

        public string ReceiverPortVisibility
        {
            get { return _ReceiverPortVisibility; }
            set { _ReceiverPortVisibility = value; NotifyPropertyChanged("ReceiverPortVisibility"); }
        }

        private string _txtCurrencyIPDR;

        public string txtCurrencyIPDR
        {
            get { return _txtCurrencyIPDR; }
            set { _txtCurrencyIPDR = value; NotifyPropertyChanged("txtCurrencyIPDR"); }
        }

        private string _txtCommodityIPDR;

        public string txtCommodityIPDR
        {
            get { return _txtCommodityIPDR; }
            set { _txtCommodityIPDR = value; NotifyPropertyChanged("txtCommodityIPDR"); }
        }

        private string _DerivativePortDR;

        public string DerivativePortDR
        {
            get { return _DerivativePortDR; }
            set { _DerivativePortDR = value; NotifyPropertyChanged("DerivativePortDR"); }
        }

        private string _CurrencyPortDR;

        public string CurrencyPortDR
        {
            get { return _CurrencyPortDR; }
            set { _CurrencyPortDR = value; NotifyPropertyChanged("CurrencyPortDR"); }
        }

        private string _CommodityPortDR;

        public string CommodityPortDR
        {
            get { return _CommodityPortDR; }
            set { _CommodityPortDR = value; NotifyPropertyChanged("CommodityPortDR"); }
        }

        private string _txtFTPServerDR;

        public string txtFTPServerDR
        {
            get { return _txtFTPServerDR; }
            set { _txtFTPServerDR = value; NotifyPropertyChanged("txtFTPServerDR"); }
        }
   
        private string _MainServerIPAddressInternet;

        public string MainServerIPAddressInternet
        {
            get { return _MainServerIPAddressInternet; }
            set { _MainServerIPAddressInternet = value; NotifyPropertyChanged("MainServerIPAddressInternet"); }
        }

        private string _InteractiveIPInternet;

        public string InteractiveIPInternet
        {
            get { return _InteractiveIPInternet; }
            set { _InteractiveIPInternet = value; NotifyPropertyChanged("InteractiveIPInternet"); }
        }

        private string _BroadcastIPInternet;

        public string BroadcastIPInternet
        {
            get { return _BroadcastIPInternet; }
            set { _BroadcastIPInternet = value; NotifyPropertyChanged("BroadcastIPInternet"); }
        }

        private string _txtProxyIpInternet;

        public string txtProxyIpInternet
        {
            get { return _txtProxyIpInternet; }
            set { _txtProxyIpInternet = value; NotifyPropertyChanged("txtProxyIpInternet"); }
        }

        private string _ProxyPortInternet;

        public string ProxyPortInternet
        {
            get { return _ProxyPortInternet; }
            set { _ProxyPortInternet = value; NotifyPropertyChanged("ProxyPortInternet"); }
        }

        private string _ProxyUserInternet;

        public string ProxyUserInternet
        {
            get { return _ProxyUserInternet; }
            set { _ProxyUserInternet = value; NotifyPropertyChanged("ProxyUserInternet"); }
        }

        private string _PasswordInternet;

        public string PasswordInternet
        {
            get { return _PasswordInternet; }
            set { _PasswordInternet = value; NotifyPropertyChanged("PasswordInternet"); }
        }

        private string _InteractivePortInternet;

        public string InteractivePortInternet
        {
            get { return _InteractivePortInternet; }
            set { _InteractivePortInternet = value; NotifyPropertyChanged("InteractivePortInternet"); }
        }

        private string _BroadcastPortInternet;

        public string BroadcastPortInternet
        {
            get { return _BroadcastPortInternet; }
            set { _BroadcastPortInternet = value; NotifyPropertyChanged("BroadcastPortInternet"); }
        }

        private string _FTPServerInternet;

        public string FTPServerInternet
        {
            get { return _FTPServerInternet; }
            set { _FTPServerInternet = value; NotifyPropertyChanged("FTPServerInternet"); }
        }

        private string _MainServerIpAddressIntDR;

        public string MainServerIpAddressIntDR
        {
            get { return _MainServerIpAddressIntDR; }
            set { _MainServerIpAddressIntDR = value; NotifyPropertyChanged("MainServerIpAddressIntDR"); }
        }

        private string _InteractiveIpIntDR;

        public string InteractiveIpIntDR
        {
            get { return _InteractiveIpIntDR; }
            set { _InteractiveIpIntDR = value; NotifyPropertyChanged("InteractiveIpIntDR"); }
        }

        private string _BroadcastIpIntDR;

        public string BroadcastIpIntDR
        {
            get { return _BroadcastIpIntDR; }
            set { _BroadcastIpIntDR = value; NotifyPropertyChanged("BroadcastIpIntDR"); }
        }

        private string _txtProxyIpIntDR;

        public string txtProxyIpIntDR
        {
            get { return _txtProxyIpIntDR; }
            set { _txtProxyIpIntDR = value; NotifyPropertyChanged("txtProxyIpIntDR"); }
        }

        private string _ProxyPortIntDR;

        public string ProxyPortIntDR
        {
            get { return _ProxyPortIntDR; }
            set { _ProxyPortIntDR = value; NotifyPropertyChanged("ProxyPortIntDR"); }
        }

        private string _ProxyUserIntDR;

        public string ProxyUserIntDR
        {
            get { return _ProxyUserIntDR; }
            set { _ProxyUserIntDR = value; NotifyPropertyChanged("ProxyUserIntDR"); }
        }

        private string _PasswordIntDR;

        public string PasswordIntDR
        {
            get { return _PasswordIntDR; }
            set { _PasswordIntDR = value; NotifyPropertyChanged("PasswordIntDR"); }
        }

        private string _InteractivePortIntDR;

        public string InteractivePortIntDR
        {
            get { return _InteractivePortIntDR; }
            set { _InteractivePortIntDR = value; NotifyPropertyChanged("InteractivePortIntDR"); }
        }

        private string _BroadcastPortIntDR;

        public string BroadcastPortIntDR
        {
            get { return _BroadcastPortIntDR; }
            set { _BroadcastPortIntDR = value; NotifyPropertyChanged("BroadcastPortIntDR"); }
        }

        private string _FTPServerIntDR;

        public string FTPServerIntDR
        {
            get { return _FTPServerIntDR; }
            set { _FTPServerIntDR = value; NotifyPropertyChanged("FTPServerIntDR"); }
        }

     
#endregion


#region Relay Commands

        private RelayCommand _btnSave_Click;

        public RelayCommand btnSave_Click
        {
            get
            {
                return _btnSave_Click ?? (_btnSave_Click = new RelayCommand(
                    (object e) => SaveChanges(e)));
                
            }

        }

        
        private RelayCommand _btnEdit_Click;

        public RelayCommand btnEdit_Click
        {

            get
            {
                return _btnEdit_Click ?? (_btnEdit_Click = new RelayCommand(
                    (object e) => EditChanges(e)));

            }

        }

        private RelayCommand _btnCancel_Click;

        public RelayCommand btnCancel_Click
        {
            get
            {
                return _btnCancel_Click ?? (_btnCancel_Click = new RelayCommand(
                    (object e) => CancelChanges(e)));

            }

        }

        private RelayCommand _txtMainServerIpAddress_TextChanged;

        public RelayCommand txtMainServerIpAddress_TextChanged
        {
            get
            {
                return _txtMainServerIpAddress_TextChanged ?? (_txtMainServerIpAddress_TextChanged = new RelayCommand((object e) => _txtMainServerIpAddress_TextChangedevent(e)));
            }

        }

        private void _txtMainServerIpAddress_TextChangedevent(object e)
        {
            
        }

        private RelayCommand _txtBroadcastPort_TextChanged;

        public RelayCommand txtBroadcastPort_TextChanged
        {
            get
            {
                return _txtBroadcastPort_TextChanged ?? (_txtBroadcastPort_TextChanged = new RelayCommand((object e) => _txtBroadcastPort_TextChangedevent(e)));
            }

        }

        private void _txtBroadcastPort_TextChangedevent(object e)
        {
            BroadcastPort = Regex.Replace(BroadcastPort, "[^0-9]+", "");
        }

        private RelayCommand _txtInteractivePort_TextChanged;

        public RelayCommand txtInteractivePort_TextChanged
        {
            get
            {
                return _txtInteractivePort_TextChanged ?? (_txtInteractivePort_TextChanged = new RelayCommand((object e) => _txtInteractivePort_TextChangedevent(e)));
            }

        }

        private void _txtInteractivePort_TextChangedevent(object e)
        {
            InteractivePortLan = Regex.Replace(InteractivePortLan, "[^0-9]+", "");
        }

        private RelayCommand _txtProxyServerPassword_TextChanged;

        public RelayCommand txtProxyServerPassword_TextChanged
        {
            get
            {
                return _txtProxyServerPassword_TextChanged ?? (_txtProxyServerPassword_TextChanged = new RelayCommand((object e) => _txtProxyServerPassword_TextChangedevent(e)));
            }

        }
        private void _txtProxyServerPassword_TextChangedevent(object e)
        {

        }

        private RelayCommand _txtInteractiveIpAddress_TextChanged;

        public RelayCommand txtInteractiveIpAddress_TextChanged
        {
            get
            {
                return _txtInteractiveIpAddress_TextChanged ?? (_txtInteractiveIpAddress_TextChanged = new RelayCommand((object e) => _txtInteractiveIpAddress_TextChangedevent(e)));
            }

        }
        private void _txtInteractiveIpAddress_TextChangedevent(object e)
        {

        }

        private RelayCommand _txtEquityPort1_TextChanged;

        public RelayCommand txtEquityPort1_TextChanged
        {
            get
            {
                return _txtEquityPort1_TextChanged ?? (_txtEquityPort1_TextChanged = new RelayCommand((object e) => _txtEquityPort1_TextChangedevent(e)));
            }

        }
        private void _txtEquityPort1_TextChangedevent(object e)
        {
            EquityPort1 = Regex.Replace(EquityPort1, "[^0-9]+", "");
        }

        private RelayCommand _txtDerivativePort1_TextChanged;

        public RelayCommand txtDerivativePort1_TextChanged
        {
            get
            {
                return _txtDerivativePort1_TextChanged ?? (_txtDerivativePort1_TextChanged = new RelayCommand((object e) => _txtDerivativePort1_TextChangedevent(e)));
            }

        }
        private void _txtDerivativePort1_TextChangedevent(object e)
        {
            DerivativePort1 = Regex.Replace(DerivativePort1, "[^0-9]+", "");
        }

        private RelayCommand _txtCurrencyPort1_TextChanged;

        public RelayCommand txtCurrencyPort1_TextChanged
        {
            get
            {
                return _txtCurrencyPort1_TextChanged ?? (_txtCurrencyPort1_TextChanged = new RelayCommand((object e) => _txtCurrencyPort1_TextChangedevent(e)));
            }

        }
        private void _txtCurrencyPort1_TextChangedevent(object e)
        {
            CurrencyPort1 = Regex.Replace(CurrencyPort1, "[^0-9]+", "");
        }

        private RelayCommand _txtCommodityPort1_TextChanged;

        public RelayCommand txtCommodityPort1_TextChanged
        {
            get
            {
                return _txtCommodityPort1_TextChanged ?? (_txtCommodityPort1_TextChanged = new RelayCommand((object e) => _txtCommodityPort1_TextChangedevent(e)));
            }

        }
        private void _txtCommodityPort1_TextChangedevent(object e)
        {
            CommodityPort1 = Regex.Replace(CommodityPort1, "[^0-9]+", "");
        }

        private RelayCommand _txtClientListenPort_TextChanged;

        public RelayCommand txtClientListenPort_TextChanged
        {
            get
            {
                return _txtClientListenPort_TextChanged ?? (_txtClientListenPort_TextChanged = new RelayCommand((object e) => _txtClientListenPort_TextChangedevent(e)));
            }

        }
        private void _txtClientListenPort_TextChangedevent(object e)
        {
            ClientListenPort = Regex.Replace(ClientListenPort, "[^0-9]+", "");
        }

        private RelayCommand _txtBroadcastPortDR_TextChanged;

        public RelayCommand txtBroadcastPortDR_TextChanged
        {
            get
            {
                return _txtBroadcastPortDR_TextChanged ?? (_txtBroadcastPortDR_TextChanged = new RelayCommand((object e) => _txtBroadcastPortDR_TextChangedevent(e)));
            }

        }
        private void _txtBroadcastPortDR_TextChangedevent(object e)
        {
            BroadcastPortDR = Regex.Replace(BroadcastPortDR, "[^0-9]+", "");
        }

        private RelayCommand _txtMainServerIpAddressDR_TextChanged;

        public RelayCommand txtMainServerIpAddressDR_TextChanged
        {
            get
            {
                return _txtMainServerIpAddressDR_TextChanged ?? (_txtMainServerIpAddressDR_TextChanged = new RelayCommand((object e) => _txtMainServerIpAddressDR_TextChangedevent(e)));
            }

        }

        private void _txtMainServerIpAddressDR_TextChangedevent(object e)
        {

        }

        private RelayCommand _txtInteractivePortDR_TextChanged;

        public RelayCommand txtInteractivePortDR_TextChanged
        {
            get
            {
                return _txtInteractivePortDR_TextChanged ?? (_txtInteractivePortDR_TextChanged = new RelayCommand((object e) => _txtInteractivePortDR_TextChangedevent(e)));
            }

        }

        private void _txtInteractivePortDR_TextChangedevent(object e)
        {
            InteractivePortDR = Regex.Replace(InteractivePortDR, "[^0-9]+", "");
        }

        private RelayCommand _txtEquityPortDR_TextChanged;

        public RelayCommand txtEquityPortDR_TextChanged
        {
            get
            {
                return _txtEquityPortDR_TextChanged ?? (_txtEquityPortDR_TextChanged = new RelayCommand((object e) => _txtEquityPortDR_TextChangedevent(e)));
            }

        }
        private void _txtEquityPortDR_TextChangedevent(object e)
        {
            EquityPortDR = Regex.Replace(EquityPortDR, "[^0-9]+", "");
        }

        private RelayCommand _txtDerivativePortDR_TextChanged;

        public RelayCommand txtDerivativePortDR_TextChanged
        {
            get
            {
                return _txtDerivativePortDR_TextChanged ?? (_txtDerivativePortDR_TextChanged = new RelayCommand((object e) => _txtDerivativePortDR_TextChangedevent(e)));
            }

        }
        private void _txtDerivativePortDR_TextChangedevent(object e)
        {
            DerivativePortDR = Regex.Replace(DerivativePortDR, "[^0-9]+", "");
        }

        private RelayCommand _txtCommodityPortDR_TextChanged;

        public RelayCommand txtCommodityPortDR_TextChanged
        {
            get
            {
                return _txtCommodityPortDR_TextChanged ?? (_txtCommodityPortDR_TextChanged = new RelayCommand((object e) => _txtCommodityPortDR_TextChangedevent(e)));
            }

        }
        private void _txtCommodityPortDR_TextChangedevent(object e)
        {
            CommodityPortDR = Regex.Replace(CommodityPortDR, "[^0-9]+", "");
        }

        private RelayCommand _txtCurrencyPortDR_TextChanged;

        public RelayCommand txtCurrencyPortDR_TextChanged
        {
            get
            {
                return _txtCurrencyPortDR_TextChanged ?? (_txtCurrencyPortDR_TextChanged = new RelayCommand((object e) => _txtCurrencyPortDR_TextChangedevent(e)));
            }

        }
        private void _txtCurrencyPortDR_TextChangedevent(object e)
        {
            CurrencyPortDR = Regex.Replace(CurrencyPortDR, "[^0-9]+", "");
        }

        private RelayCommand _txtMainServerIPAddressInternet_TextChanged;

        public RelayCommand txtMainServerIPAddressInternet_TextChanged
        {
            get
            {
                return _txtMainServerIPAddressInternet_TextChanged ?? (_txtMainServerIPAddressInternet_TextChanged = new RelayCommand((object e) => _txtMainServerIPAddressInternet_TextChangedevent(e)));
            }

        }
        private void _txtMainServerIPAddressInternet_TextChangedevent(object e)
        {

        }

        private RelayCommand _txtInteractiveIPInternet_TextChanged;

        public RelayCommand txtInteractiveIPInternet_TextChanged
        {
            get
            {
                return _txtInteractiveIPInternet_TextChanged ?? (_txtInteractiveIPInternet_TextChanged = new RelayCommand((object e) => _txtInteractiveIPInternet_TextChangedevent(e)));
            }

        }
        private void _txtInteractiveIPInternet_TextChangedevent(object e)
        {

        }

        private RelayCommand _txtBroadcastIPInternet_TextChanged;

        public RelayCommand txtBroadcastIPInternet_TextChanged
        {
            get
            {
                return _txtBroadcastIPInternet_TextChanged ?? (_txtBroadcastIPInternet_TextChanged = new RelayCommand((object e) => _txtBroadcastIPInternet_TextChangedevent(e)));
            }

        }
        private void _txtBroadcastIPInternet_TextChangedevent(object e)
        {

        }

        private RelayCommand _txtProxyPortInternet_TextChanged;

        public RelayCommand txtProxyPortInternet_TextChanged
        {
            get
            {
                return _txtProxyPortInternet_TextChanged ?? (_txtProxyPortInternet_TextChanged = new RelayCommand((object e) => _txtProxyPortInternet_TextChangedevent(e)));
            }

        }
        private void _txtProxyPortInternet_TextChangedevent(object e)
        {
            ProxyPortInternet = Regex.Replace(ProxyPortInternet, "[^0-9]+", "");
        }

        private RelayCommand _txtProxyUserInternet_TextChanged;

        public RelayCommand txtProxyUserInternet_TextChanged
        {
            get
            {
                return _txtProxyUserInternet_TextChanged ?? (_txtProxyUserInternet_TextChanged = new RelayCommand((object e) => _txtProxyUserInternet_TextChangedevent(e)));
            }

        }
        private void _txtProxyUserInternet_TextChangedevent(object e)
        {

        }

        private RelayCommand _txtPasswordInternet_TextChanged;

        public RelayCommand txtPasswordInternet_TextChanged
        {
            get
            {
                return _txtPasswordInternet_TextChanged ?? (_txtPasswordInternet_TextChanged = new RelayCommand((object e) => _txtPasswordInternet_TextChangedevent(e)));
            }

        }
         
        private void _txtPasswordInternet_TextChangedevent(object e)
        {

        }

        private RelayCommand _txtInteractivePortInternet_TextChanged;

        public RelayCommand txtInteractivePortInternet_TextChanged
        {
            get
            {
                return _txtInteractivePortInternet_TextChanged ?? (_txtInteractivePortInternet_TextChanged = new RelayCommand((object e) => _txtInteractivePortInternet_TextChangedevent(e)));
            }

        }
        private void _txtInteractivePortInternet_TextChangedevent(object e)
        {
            InteractivePortInternet = Regex.Replace(InteractivePortInternet, "[^0-9]+", "");
        }

        private RelayCommand _txtBroadcastPortInternet_TextChanged;

        public RelayCommand txtBroadcastPortInternet_TextChanged
        {
            get
            {
                return _txtBroadcastPortInternet_TextChanged ?? (_txtBroadcastPortInternet_TextChanged = new RelayCommand((object e) => _txtBroadcastPortInternet_TextChangedevent(e)));
            }

        }

        private void _txtBroadcastPortInternet_TextChangedevent(object e)
        {
            BroadcastPortInternet = Regex.Replace(BroadcastPortInternet, "[^0-9]+", "");
        }

        private RelayCommand _FTPServerInternet_TextChanged;

        public RelayCommand FTPServerInternet_TextChanged
        {
            get
            {
                return _FTPServerInternet_TextChanged ?? (_FTPServerInternet_TextChanged = new RelayCommand((object e) => _FTPServerInternet_TextChangedevent(e)));
            }

        }
        private void _FTPServerInternet_TextChangedevent(object e)
        {

        }

        private RelayCommand _txtMainServerIpAddressIntDR_TextChanged;

        public RelayCommand txtMainServerIpAddressIntDR_TextChanged
        {
            get
            {
                return _txtMainServerIpAddressIntDR_TextChanged ?? (_txtMainServerIpAddressIntDR_TextChanged = new RelayCommand((object e) => _txtMainServerIpAddressIntDR_TextChangedevent(e)));
            }

        }
        private void _txtMainServerIpAddressIntDR_TextChangedevent(object e)
        {

        }

        private RelayCommand _txtInteractiveIpIntDR_TextChanged;

        public RelayCommand txtInteractiveIpIntDR_TextChanged
        {
            get
            {
                return _txtInteractiveIpIntDR_TextChanged ?? (_txtInteractiveIpIntDR_TextChanged = new RelayCommand((object e) => _txtInteractiveIpIntDR_TextChangedevent(e)));
            }

        }
        private void _txtInteractiveIpIntDR_TextChangedevent(object e)
        {

        }

        private RelayCommand _txtBroadcastIpIntDR_TextChanged;

        public RelayCommand txtBroadcastIpIntDR_TextChanged
        {
            get
            {
                return _txtBroadcastIpIntDR_TextChanged ?? (_txtBroadcastIpIntDR_TextChanged = new RelayCommand((object e) => _txtBroadcastIpIntDR_TextChangedevent(e)));
            }

        }
        private void _txtBroadcastIpIntDR_TextChangedevent(object e)
        {

        }

        private RelayCommand _txtProxyPortIntDR_TextChanged;

        public RelayCommand txtProxyPortIntDR_TextChanged
        {
            get
            {
                return _txtProxyPortIntDR_TextChanged ?? (_txtProxyPortIntDR_TextChanged = new RelayCommand((object e) => _txtProxyPortIntDR_TextChangedevent(e)));
            }

        }
        private void _txtProxyPortIntDR_TextChangedevent(object e)
        {

        }

        private RelayCommand _txtProxyUserIntDR_TextChanged;

        public RelayCommand txtProxyUserIntDR_TextChanged
        {
            get
            {
                return _txtProxyUserIntDR_TextChanged ?? (_txtProxyUserIntDR_TextChanged = new RelayCommand((object e) => _txtProxyUserIntDR_TextChangedevent(e)));
            }

        }
        private void _txtProxyUserIntDR_TextChangedevent(object e)
        {

        }

        private RelayCommand _ProxyPasswordIntDR_TextChanged;

        public RelayCommand ProxyPasswordIntDR_TextChanged
        {
            get
            {
                return _ProxyPasswordIntDR_TextChanged ?? (_ProxyPasswordIntDR_TextChanged = new RelayCommand((object e) => _ProxyPasswordIntDR_TextChangedevent(e)));
            }

        }
        private void _ProxyPasswordIntDR_TextChangedevent(object e)
        {

        }

        private RelayCommand _txtInteractivePortIntDR_TextChanged;

        public RelayCommand txtInteractivePortIntDR_TextChanged
        {
            get
            {
                return _txtInteractivePortIntDR_TextChanged ?? (_txtInteractivePortIntDR_TextChanged = new RelayCommand((object e) => _InteractivePortIntDR_TextChangedevent(e)));
            }

        }
        private void _InteractivePortIntDR_TextChangedevent(object e)
        {
            InteractivePortIntDR = Regex.Replace(InteractivePortIntDR, "[^0-9]+", "");
        }

        private RelayCommand _txtBroadcastPortIntDR_TextChanged;

        public RelayCommand txtBroadcastPortIntDR_TextChanged
        {
            get
            {
                return _txtBroadcastPortIntDR_TextChanged ?? (_txtBroadcastPortIntDR_TextChanged = new RelayCommand((object e) => _txtBroadcastPortIntDR_TextChangedevent(e)));
            }

        }
        private void _txtBroadcastPortIntDR_TextChangedevent(object e)
        {
            BroadcastPortIntDR = Regex.Replace(BroadcastPortIntDR, "[^0-9]+", "");
        }



        private RelayCommand _txtFTPServerIntDR_TextChanged;

        public RelayCommand txtFTPServerIntDR_TextChanged
        {
            get
            {
                return _txtFTPServerIntDR_TextChanged ?? (_txtFTPServerIntDR_TextChanged = new RelayCommand((object e) => txtFTPServerIntDR_TextChangedevent(e)));
            }

        }
        private void txtFTPServerIntDR_TextChangedevent(object e)
        {

        }

        private RelayCommand _BowSettingsLoaded;

        public RelayCommand BowSettingsLoaded
        {
            get
            {
                return _BowSettingsLoaded ?? (_BowSettingsLoaded = new RelayCommand((object e) => BowSettingsLoaded_Load(e)));

            }

        }




        private void BowSettingsLoaded_Load(object e)
        {
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtMainServerIpAddress.Text = MainServerIpAddresstxt;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtInteractiveIP.Text = txtInteractiveIPLan;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtInteractivePort.Text = InteractivePortLan;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtBroadcastIP.Text = txtBroadcastIP;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtBroadcastPort.Text = BroadcastPort;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtEquityIP1.Text = txtEquityIP1;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtEquityPort1.Text = EquityPort1;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtDerivativeIP1.Text = txtDerivativeIP1;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtCurrencyIP1.Text = txtCurrencyIP1;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtCommodityIP1.Text = txtCommodityIP1;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtDerivativePort1.Text = DerivativePort1;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtCurrencyPort1.Text = CurrencyPort1;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtCommodityPort1.Text = CommodityPort1;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtFTPServerIP.Text = txtFTPServerIP;
           
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtMainServerIpAddressDR.Text = MainServerIpAddressDR;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtInteractiveIPDR.Text = txtInteractiveIPDR;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtInteractivePortDR.Text = InteractivePortDR;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtBroadcastIPDR.Text = txtBroadcastIPDR;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtBroadcastPortDR.Text = BroadcastPortDR;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtEquityIPDR.Text = txtEquityIPDR;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtEquityPortDR.Text = EquityPortDR;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtDerivativeIPDR.Text = txtDerivativeIPDR;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtCurrencyIPDR.Text = txtCurrencyIPDR;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtCommodityIPDR.Text = txtCommodityIPDR;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtDerivativePortDR.Text = DerivativePortDR;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtCurrencyPortDR.Text = CurrencyPortDR;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtCommodityPortDR.Text = CommodityPortDR;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtFTPServerDR.Text = txtFTPServerDR;
           
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtMainServerIPAddressInternet.Text = MainServerIPAddressInternet;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtInteractiveIPInternet.Text = InteractiveIPInternet;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtBroadcastIPInternet.Text = BroadcastIPInternet;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtBroadcastPortInternet.Text = BroadcastPortInternet;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).FTPServerInternet.Text = FTPServerInternet;

            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtMainServerIpAddressIntDR.Text = MainServerIpAddressIntDR;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtInteractiveIpIntDR.Text = InteractiveIpIntDR;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtBroadcastIpIntDR.Text = BroadcastIpIntDR;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtBroadcastPortIntDR.Text = BroadcastPortIntDR;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtFTPServerIntDR.Text = FTPServerIntDR;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtInterfaceIP.Text = txtInterfaceIP;
            ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtClientListenIP.Text = txtClientListenIP;

             ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtProxyIpInternet.Text = txtProxyIpInternet;

        }

        private void SaveChanges(object e)
        {
            BtnEditEnable = Boolean.TrueString;
            CtrEnable = Boolean.FalseString;
            BtnCancelEnable = Boolean.TrueString;
            BtnSaveEnable = Boolean.FalseString;
            ConnectionTypeEnabled = Boolean.FalseString;
          
            MainServerIpAddresstxt = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtMainServerIpAddress.Text;
            txtInteractiveIPLan = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtInteractiveIP.Text;
            InteractivePortLan = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtInteractivePort.Text;
            txtBroadcastIP = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtBroadcastIP.Text;
            BroadcastPort = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtBroadcastPort.Text;
           
            txtEquityIP1 = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtEquityIP1.Text;
            EquityPort1 = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtEquityPort1.Text;
            txtDerivativeIP1 = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtDerivativeIP1.Text;
            txtCurrencyIP1 = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtCurrencyIP1.Text;
            txtCommodityIP1 = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtCommodityIP1.Text;
            DerivativePort1 = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtDerivativePort1.Text;
            CurrencyPort1 = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtCurrencyPort1.Text;
            CommodityPort1 = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtCommodityPort1.Text;
            txtFTPServerIP = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtFTPServerIP.Text;
            
            MainServerIpAddressDR = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtMainServerIpAddressDR.Text;
            txtInteractiveIPDR = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtInteractiveIPDR.Text;
            InteractivePortDR = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtInteractivePortDR.Text;
            txtBroadcastIPDR = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtBroadcastIPDR.Text;
            BroadcastPortDR = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtBroadcastPortDR.Text;

            txtEquityIPDR = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtEquityIPDR.Text;
            EquityPortDR = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtEquityPortDR.Text;
            txtDerivativeIPDR = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtDerivativeIPDR.Text;
            txtCurrencyIPDR = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtCurrencyIPDR.Text;
            txtCommodityIPDR = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtCommodityIPDR.Text;
            DerivativePortDR = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtDerivativePortDR.Text;
            CurrencyPortDR = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtCurrencyPortDR.Text;
            CommodityPortDR = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtCommodityPortDR.Text;
            txtFTPServerDR = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtFTPServerDR.Text;
          
            MainServerIPAddressInternet = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtMainServerIPAddressInternet.Text;
            InteractiveIPInternet = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtInteractiveIPInternet.Text;
            BroadcastIPInternet = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtBroadcastIPInternet.Text;
            InteractivePortInternet = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtInteractivePortInternet.Text;
            BroadcastPortInternet = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtBroadcastPortInternet.Text;
            FTPServerInternet = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).FTPServerInternet.Text;

            MainServerIpAddressIntDR = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtMainServerIpAddressIntDR.Text;
            InteractiveIpIntDR = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtInteractiveIpIntDR.Text;
            BroadcastIpIntDR = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtBroadcastIpIntDR.Text;
            InteractivePortIntDR = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtInteractivePortIntDR.Text;
            BroadcastPortIntDR = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtBroadcastPortIntDR.Text;
            FTPServerIntDR = ((BowSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BowSettingWindow.Content)).txtFTPServerIntDR.Text;

            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line PRODUCTION", "MAIN SERVER IP", MainServerIpAddresstxt);
            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line PRODUCTION", "INTERACTIVE IP", txtInteractiveIPLan);
            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line PRODUCTION", "INTERACTIVE PORT", InteractivePortLan);
            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line PRODUCTION", "BROADCAST IP", txtBroadcastIP);
            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line PRODUCTION", "BROADCAST PORT", BroadcastPort);
          
            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line PRODUCTION", "EQUITY IP(BCast)", txtEquityIP1);
            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line PRODUCTION", "EQUITY PORT(BCast)", EquityPort1);
            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line PRODUCTION", "DERIVATIVE IP(BCast)", txtDerivativeIP1);
            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line PRODUCTION", "CURRENCY IP(BCast)", txtCurrencyIP1);
            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line PRODUCTION", "COMMODITY IP(BCast)", txtCommodityIP1);
            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line PRODUCTION", "DERIVATIVE PORT(BCast)", DerivativePort1);
            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line PRODUCTION", "CURRENCY PORT(BCast)", CurrencyPort1);
            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line PRODUCTION", "COMMODITY PORT(BCast)", CommodityPort1);
            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line PRODUCTION", "FTP SERVER IP", txtFTPServerIP);

            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line DR", "MAIN SERVER IP", MainServerIpAddressDR);
            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line DR", "DR INTERACTIVE IP", txtInteractiveIPDR);
            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line DR", "DR INTERACTIVE PORT", InteractivePortDR);
            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line DR", "DR BROADCAST IP", txtBroadcastIPDR);
            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line DR", "DR BROADCAST PORT", BroadcastPortDR);

            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line DR", "DR EQUITY IP(BCast)", txtEquityIPDR);
            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line DR", "DR EQUITY PORT(BCast)", EquityPortDR);
            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line DR", "DR DERIVATIVE IP(BCast)", txtDerivativeIPDR);
            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line DR", "DR CURRENCY IP(BCast)", txtCurrencyIPDR);
            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line DR", "DR COMMODITY IP(BCast)", txtCommodityIPDR);
            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line DR", "DR DERIVATIVE PORT(BCast)", DerivativePortDR);
            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line DR", "DR CURRENCY PORT(BCast)", CurrencyPortDR);
            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line DR", "DR COMMODITY PORT(BCast)", CommodityPortDR);
            MainWindowVM.parser1OS.AddSetting("LAN/Leased Line DR", "DR FTP SERVER", txtFTPServerDR);

            MainWindowVM.parser1OS.AddSetting("INTERNET PRODUCTION", "MAIN SERVER IP", MainServerIPAddressInternet);
            MainWindowVM.parser1OS.AddSetting("INTERNET PRODUCTION", "INTERACTIVE IP", InteractiveIPInternet);
            MainWindowVM.parser1OS.AddSetting("INTERNET PRODUCTION", "BROADCAST IP", BroadcastIPInternet);
           
            MainWindowVM.parser1OS.AddSetting("INTERNET PRODUCTION", "INTERACTIVE PORT", InteractivePortInternet);
            MainWindowVM.parser1OS.AddSetting("INTERNET PRODUCTION", "BROADCAST PORT", BroadcastPortInternet);
            MainWindowVM.parser1OS.AddSetting("INTERNET PRODUCTION", "FTP SERVER", FTPServerInternet);

            MainWindowVM.parser1OS.AddSetting("INTERNET DR", "MAIN SERVER IP", MainServerIpAddressIntDR);
            MainWindowVM.parser1OS.AddSetting("INTERNET DR", "INTERACTIVE IP", InteractiveIpIntDR);
            MainWindowVM.parser1OS.AddSetting("INTERNET DR", "INTERACTIVE PORT", InteractivePortIntDR);
            MainWindowVM.parser1OS.AddSetting("INTERNET DR", "BROADCAST PORT", BroadcastPortIntDR);
            MainWindowVM.parser1OS.AddSetting("INTERNET DR", "FTP SERVER", FTPServerIntDR);

            MainWindowVM.parser.AddSetting("BOW CONNECTION SETTINGS", "INTERFACE IP", txtInterfaceIP);
            MainWindowVM.parser.AddSetting("BOW CONNECTION SETTINGS", "CLIENT LISTEN PORT", ClientListenPort);
            MainWindowVM.parser.AddSetting("BOW CONNECTION SETTINGS", "CLIENT LISTEN IP", txtClientListenIP);

            MainWindowVM.parser.AddSetting("BOW CONNECTION SETTINGS", "PROXY SERVER", txtProxyIpInternet);
            MainWindowVM.parser.AddSetting("BOW CONNECTION SETTINGS", "PROXY PORT", ProxyPortInternet);
            MainWindowVM.parser.AddSetting("BOW CONNECTION SETTINGS", "PROXY USER", ProxyUserInternet);
            MainWindowVM.parser.AddSetting("BOW CONNECTION SETTINGS", "PASSWORD", PasswordInternet);

            MainWindowVM.parser.SaveSettings(MainWindowVM.TwsINIPath.ToString());

            MainWindowVM.parser1OS.SaveSettings(MainWindowVM.BOWSettingsINIPath.ToString());
    
        }

      

#endregion

#region Constructor

        public BowSettingsVM()
        {
                       
            CtrEnable = Boolean.FalseString;
            BtnEditEnable = Boolean.TrueString;
            Visibility();
            PopulatingConnectionType();
            OnChangeOfConnectionType();
            PopulatingConnectionType();
        }


#endregion

#region Methods

        private void OnChangeOfConnectionType()
        {

           
            if (SelectedConnectionType == "LAN")
            {
                InternetVisibility = "Hidden";
                LanVisibility = "Visible";

            }

            else if (SelectedConnectionType == "Internet")
            {

                LanVisibility = "Hidden";
                InternetVisibility = "Visible";

            }
     }


        private void EditChanges(object e)
        {
            BtnEditEnable = Boolean.FalseString;
            BtnSaveEnable = Boolean.TrueString;
            BtnCancelEnable = Boolean.TrueString;
            ConnectionTypeEnabled = Boolean.TrueString;
            if (CtrEnable == Boolean.TrueString)
            {
                CtrEnable = Boolean.FalseString;
            }
            else { CtrEnable = Boolean.TrueString; }

        }


        private void CancelChanges(object e)
        {
          
            if (CtrEnable == Boolean.TrueString)
            {
                CtrEnable = Boolean.FalseString;
                BtnEditEnable = Boolean.TrueString;
                BtnSaveEnable = Boolean.FalseString;
                BowSettingsLoaded_Load(e);
                BtnCancelEnable = Boolean.FalseString;
                ConnectionTypeEnabled = Boolean.FalseString;
              
            }

            else
            {
                CtrEnable = Boolean.FalseString;
                BtnEditEnable = Boolean.TrueString;
                BtnSaveEnable = Boolean.FalseString;
                BowSettingsLoaded_Load(e);
                BtnCancelEnable = Boolean.FalseString;
            }

        }

       
        private void PopulatingConnectionType()
        {
            ConnectionType = new List<string>();
            if (string.IsNullOrEmpty(SelectedConnectionType))
                SelectedConnectionType = (Enumerations.ConnectionType.LAN.ToString());


            {
                ConnectionType.Add(Enumerations.ConnectionType.LAN.ToString());

                ConnectionType.Add(Enumerations.ConnectionType.Internet.ToString());

                if (SelectedConnectionType == "LAN")
                {
                    InternetVisibility = "Hidden";
                    LanVisibility = "Visible";

                }

                else if (SelectedConnectionType == "Internet")
                {

                    LanVisibility = "Hidden";
                    InternetVisibility = "Visible";

                }

            }

        }


        private void Visibility()
        {
           
            if (LanVisibility == Boolean.TrueString)
            {
                InternetVisibility = Boolean.FalseString;
            }
            else

            {
                InternetVisibility = Boolean.TrueString;
            }
        }

       
        private void BindData()
        {
            IniParser parser1OS = new IniParser(MainWindowVM.BOWSettingsINIPath.ToString());
           
            MainServerIpAddresstxt = MainWindowVM.parser1OS.GetSetting("LAN/Leased Line PRODUCTION", "MAIN SERVER IP");
            txtInteractiveIPLan = MainWindowVM.parser1OS.GetSetting("LAN/Leased Line PRODUCTION", "INTERACTIVE IP");
            InteractivePortLan = MainWindowVM.parser1OS.GetSetting("LAN/Leased Line PRODUCTION", "INTERACTIVE PORT");
            txtBroadcastIP = MainWindowVM.parser1OS.GetSetting("LAN/Leased Line PRODUCTION", "BROADCAST IP");
            BroadcastPort = MainWindowVM.parser1OS.GetSetting("LAN/Leased Line PRODUCTION", "BROADCAST PORT");
          
            txtEquityIP1 = MainWindowVM.parser1OS.GetSetting("LAN/Leased Line PRODUCTION", "EQUITY IP(BCast)");
            EquityPort1 = MainWindowVM.parser1OS.GetSetting("LAN/Leased Line PRODUCTION", "EQUITY PORT(BCast)");
            txtDerivativeIP1 = MainWindowVM.parser1OS.GetSetting("LAN/Leased Line PRODUCTION", "DERIVATIVE IP(BCast)");
            txtCurrencyIP1 = MainWindowVM.parser1OS.GetSetting("LAN/Leased Line PRODUCTION", "CURRENCY IP(BCast)");
            txtCommodityIP1 = MainWindowVM.parser1OS.GetSetting("LAN/Leased Line PRODUCTION", "COMMODITY IP(BCast)");
            DerivativePort1 = MainWindowVM.parser1OS.GetSetting("LAN/Leased Line PRODUCTION", "DERIVATIVE PORT(BCast)");
            CurrencyPort1 = MainWindowVM.parser1OS.GetSetting("LAN/Leased Line PRODUCTION", "CURRENCY PORT(BCast)");
            CommodityPort1 = MainWindowVM.parser1OS.GetSetting("LAN/Leased Line PRODUCTION", "COMMODITY PORT(BCast)");
            txtFTPServerIP = MainWindowVM.parser1OS.GetSetting("LAN/Leased Line PRODUCTION", "FTP SERVER IP");

            MainServerIpAddressDR = MainWindowVM.parser1OS.GetSetting("LAN/Leased Line DR", "MAIN SERVER IP");
            txtInteractiveIPDR = MainWindowVM.parser1OS.GetSetting("LAN/Leased Line DR", "DR INTERACTIVE IP");
            InteractivePortDR = MainWindowVM.parser1OS.GetSetting("LAN/Leased Line DR", "DR INTERACTIVE PORT");
            txtBroadcastIPDR = MainWindowVM.parser1OS.GetSetting("LAN/Leased Line DR", "DR BROADCAST IP");
            BroadcastPortDR = MainWindowVM.parser1OS.GetSetting("LAN/Leased Line DR", "DR BROADCAST PORT");
            txtEquityIPDR = MainWindowVM.parser1OS.GetSetting("LAN/Leased Line DR", "DR EQUITY IP(BCast)");
            EquityPortDR = MainWindowVM.parser1OS.GetSetting("LAN/Leased Line DR", "DR EQUITY PORT(BCast)");
            txtDerivativeIPDR = MainWindowVM.parser1OS.GetSetting("LAN/Leased Line DR", "DR DERIVATIVE IP(BCast)");
            txtCurrencyIPDR = MainWindowVM.parser1OS.GetSetting("LAN/Leased Line DR", "DR CURRENCY IP(BCast)");
            txtCommodityIPDR = MainWindowVM.parser1OS.GetSetting("LAN/Leased Line DR", "DR COMMODITY IP(BCast)");
            DerivativePortDR = MainWindowVM.parser1OS.GetSetting("LAN/Leased Line DR", "DR DERIVATIVE PORT(BCast)");
            CurrencyPortDR =  MainWindowVM.parser1OS.GetSetting("LAN/Leased Line DR", "DR CURRENCY PORT(BCast)");
            CommodityPortDR = MainWindowVM.parser1OS.GetSetting("LAN/Leased Line DR", "DR COMMODITY PORT(BCast)");
            txtFTPServerDR = MainWindowVM.parser1OS.GetSetting("LAN/Leased Line DR", "DR FTP SERVER");
            MainServerIPAddressInternet = MainWindowVM.parser1OS.GetSetting("INTERNET PRODUCTION", "MAIN SERVER IP");
            InteractiveIPInternet = MainWindowVM.parser1OS.GetSetting("INTERNET PRODUCTION", "INTERACTIVE IP");
            BroadcastIPInternet = MainWindowVM.parser1OS.GetSetting("INTERNET PRODUCTION", "BROADCAST IP");
      
            InteractivePortInternet = MainWindowVM.parser1OS.GetSetting("INTERNET PRODUCTION", "INTERACTIVE PORT");
            BroadcastPortInternet = MainWindowVM.parser1OS.GetSetting("INTERNET PRODUCTION", "BROADCAST PORT");
            FTPServerInternet = MainWindowVM.parser1OS.GetSetting("INTERNET PRODUCTION", "FTP SERVER");

            MainServerIpAddressIntDR = MainWindowVM.parser1OS.GetSetting("INTERNET DR", "MAIN SERVER IP");
            InteractiveIpIntDR = MainWindowVM.parser1OS.GetSetting("INTERNET DR", "INTERACTIVE IP");
            BroadcastIpIntDR = MainWindowVM.parser1OS.GetSetting("INTERNET DR", "BROADCAST IP");

            InteractivePortIntDR = MainWindowVM.parser1OS.GetSetting("INTERNET DR", "INTERACTIVE PORT");
            BroadcastPortIntDR = MainWindowVM.parser1OS.GetSetting("INTERNET DR", "BROADCAST PORT");
            FTPServerIntDR = MainWindowVM.parser1OS.GetSetting("INTERNET DR", "FTP SERVER");

            IniParser parser = new IniParser(MainWindowVM.TwsINIPath.ToString());

            txtInterfaceIP = MainWindowVM.parser.GetSetting("BOW CONNECTION SETTINGS", "INTERFACE IP");
            ClientListenPort = MainWindowVM.parser.GetSetting("BOW CONNECTION SETTINGS", "CLIENT LISTEN PORT");
            txtClientListenIP = MainWindowVM.parser.GetSetting("BOW CONNECTION SETTINGS", "CLIENT LISTEN IP");

            txtProxyIpInternet = MainWindowVM.parser.GetSetting("BOW CONNECTION SETTINGS", "PROXY SERVER");
            ProxyPortInternet = MainWindowVM.parser.GetSetting("BOW CONNECTION SETTINGS", "PROXY PORT");
            ProxyUserInternet = MainWindowVM.parser.GetSetting("BOW CONNECTION SETTINGS", "PROXY USER");
            PasswordInternet = MainWindowVM.parser.GetSetting("BOW CONNECTION SETTINGS", "PASSWORD");
           
        }

#endregion

#region StaticNotifyPropertyChangedEvent
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged
                 = delegate { };
        private static void NotifyStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }
#endregion

#region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, e);
            }

        }
#endregion
       

        //#region Constructor

        //public BowSettingsVM()
        //{
        //    Visibility();
            

        //    PopulatingConnectionType();
        //    OnChangeOfConnectionType();
        //    BindData();
        //    PopulatingConnectionType();
        //}

       
       
    }
#endif

}
