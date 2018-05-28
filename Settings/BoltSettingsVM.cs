using CommonFrontEnd;
using CommonFrontEnd.Common;
using CommonFrontEnd.Model;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.View.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace CommonFrontEnd.ViewModel.Settings
{
#if TWS
    partial class BoltSettingsVM
    {
        #region Properties

        private string _LeftPosition = "345";

        public string LeftPosition
        {
            get { return _LeftPosition; }
            set { _LeftPosition = value; NotifyStaticPropertyChanged("LeftPosition"); }
        }

        private string _TopPosition = "200";

        public string TopPosition
        {
            get { return _TopPosition; }
            set { _TopPosition = value; NotifyStaticPropertyChanged("TopPosition"); }
        }

        private string _Width = "642";

        public string Width
        {
            get { return _Width; }
            set { _Width = value; NotifyStaticPropertyChanged("Width"); }
        }


        private string _Height = "680";

        public string Height
        {
            get { return _Height; }
            set { _Height = value; NotifyStaticPropertyChanged("Height"); }
        }


        //public static DirectoryInfo BoltINIPath = new DirectoryInfo(
        //  Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"BOLT.ini")));

        private static string _EquityIP1Prod;
        private static string _EquityIP2Prod;
        private static string _EquityIP3Prod;
        private static string _EquityIP4Prod;
        private static string _EquityPort1Prod;
        private static string _EquityPort2Prod;
        private static string _EquityPort3Prod;
        private static string _EquityPort4Prod;

        private static bool _ScripIdEquity;

        public static bool ScripIdEquity
        {
            get { return _ScripIdEquity; }
            set { _ScripIdEquity = value; NotifyStaticPropertyChanged("ScripIdEquity"); }
        }

        private static bool _ScripNameEquity;

        public static bool ScripNameEquity
        {
            get { return _ScripNameEquity; }
            set { _ScripNameEquity = value; NotifyStaticPropertyChanged("ScripNameEquity"); }
        }

        private static bool _ScripNameDerivative;

        public static bool ScripNameDerivative
        {
            get { return _ScripNameDerivative; }
            set { _ScripNameDerivative = value; NotifyStaticPropertyChanged("ScripNameDerivative"); }
        }

        private static bool _ScripIdDerivative;

        public static bool ScripIdDerivative
        {
            get { return _ScripIdDerivative; }
            set { _ScripIdDerivative = value; NotifyStaticPropertyChanged("ScripIdDerivative"); }
        }

        private static bool _ScripIdCurrency;

        public static bool ScripIdCurrency
        {
            get { return _ScripIdCurrency; }
            set { _ScripIdCurrency = value; NotifyStaticPropertyChanged("ScripIdCurrency"); }
        }

        private static bool _ScripNameCurrency;

        public static bool ScripNameCurrency
        {
            get { return _ScripNameCurrency; }
            set { _ScripNameCurrency = value; NotifyStaticPropertyChanged("ScripNameCurrency"); }
        }

        private static string _EquityIP1BCastProd;
        private static string _EquityIP2BCastProd;


        private static string _EquityPort1BCastProd;
        private static string _EquityPort2BCastProd;

        private static string _DerivativeIP1Prod;
        private static string _DerivativeIP2Prod;
        private static string _DerivativeIP3Prod;
        private static string _DerivativeIP4Prod;
        private static string _DerivativePort1Prod;
        private static string _DerivativePort2Prod;
        private static string _DerivativePort3Prod;
        private static string _DerivativePort4Prod;
        private static string _DerivativeIP1BCastProd;
        private static string _DerivativePort1BCastProd;
        private static string _CurrencyIP1Prod;
        private static string _CurrencyIP2Prod;
        private static string _CurrencyIP3Prod;
        private static string _CurrencyIP4Prod;
        private static string _CurrencyPort1Prod;
        private static string _CurrencyPort2Prod;
        private static string _CurrencyPort3Prod;
        private static string _CurrencyPort4Prod;
        private static string _CurrencyIP1BCastProd;
        private static string _CurrencyPort1BCastProd;
        private static string _BoltIp1;
        private static string _BoltPort1;
        private static string _BoltIp2;
        private static string _BoltPort2;
        private static string _InterfaceIP;
        private static string _IMLPort;
        private static string _PrimaryExtranetIP;
        private static string _SecondaryExtranetIP;
        private static string _CtrEnable;

        public static string CtrEnable
        {
            get { return _CtrEnable; }
            set { _CtrEnable = value; NotifyStaticPropertyChanged("CtrEnable"); }
        }

        private static string _BtnModifyEnable;

        public static string BtnModifyEnable
        {
            get { return _BtnModifyEnable; }
            set { _BtnModifyEnable = value; NotifyStaticPropertyChanged("BtnModifyEnable"); }
        }

        public static string EquityIP1Prod
        {
            get { return _EquityIP1Prod; }
            set { _EquityIP1Prod = value; NotifyStaticPropertyChanged("EquityIP1Prod"); }
        }

        public static string EquityIP2Prod
        {
            get { return _EquityIP2Prod; }
            set { _EquityIP2Prod = value; NotifyStaticPropertyChanged("EquityIP2Prod"); }
        }



        public static string EquityIP3Prod
        {
            get { return _EquityIP3Prod; }
            set { _EquityIP3Prod = value; NotifyStaticPropertyChanged("EquityIP3Prod"); }
        }



        public static string EquityIP4Prod
        {
            get { return _EquityIP4Prod; }
            set { _EquityIP4Prod = value; NotifyStaticPropertyChanged("EquityIP4Prod"); }
        }


        public static string EquityPort1Prod
        {
            get { return _EquityPort1Prod; }
            set { _EquityPort1Prod = value; NotifyStaticPropertyChanged("EquityPort1Prod"); }
        }


        public static string EquityPort2Prod
        {
            get { return _EquityPort2Prod; }
            set { _EquityPort2Prod = value; NotifyStaticPropertyChanged("EquityPort2Prod"); }
        }



        public static string EquityPort3Prod
        {
            get { return _EquityPort3Prod; }
            set { _EquityPort3Prod = value; NotifyStaticPropertyChanged("EquityPort3Prod"); }
        }



        public static string EquityPort4Prod
        {
            get { return _EquityPort4Prod; }
            set { _EquityPort4Prod = value; NotifyStaticPropertyChanged("EquityPort4Prod"); }
        }



        public static string EquityIP1BCastProd
        {
            get { return _EquityIP1BCastProd; }
            set { _EquityIP1BCastProd = value; NotifyStaticPropertyChanged("EquityIP1BCastProd"); }
        }

        public static string EquityIP2BCastProd
        {
            get { return _EquityIP2BCastProd; }
            set { _EquityIP2BCastProd = value; NotifyStaticPropertyChanged("EquityIP2BCastProd"); }
        }


        public static string EquityPort1BCastProd
        {
            get { return _EquityPort1BCastProd; }
            set { _EquityPort1BCastProd = value; NotifyStaticPropertyChanged("EquityPort1BCastProd"); }
        }



        public static string EquityPort2BCastProd
        {
            get { return _EquityPort2BCastProd; }
            set { _EquityPort2BCastProd = value; NotifyStaticPropertyChanged("EquityPort2BCastProd"); }
        }



        public static string DerivativeIP1Prod
        {
            get { return _DerivativeIP1Prod; }
            set { _DerivativeIP1Prod = value; NotifyStaticPropertyChanged("DerivativeIP1Prod"); }
        }

        public static string DerivativeIP2Prod
        {
            get { return _DerivativeIP2Prod; }
            set { _DerivativeIP2Prod = value; NotifyStaticPropertyChanged("DerivativeIP2Prod"); }
        }



        public static string DerivativeIP3Prod
        {
            get { return _DerivativeIP3Prod; }
            set { _DerivativeIP3Prod = value; NotifyStaticPropertyChanged("DerivativeIP3Prod"); }
        }



        public static string DerivativeIP4Prod
        {
            get { return _DerivativeIP4Prod; }
            set { _DerivativeIP4Prod = value; NotifyStaticPropertyChanged("DerivativeIP4Prod"); }
        }



        public static string DerivativePort1Prod
        {
            get { return _DerivativePort1Prod; }
            set { _DerivativePort1Prod = value; NotifyStaticPropertyChanged("DerivativePort1Prod"); }
        }


        public static string DerivativePort2Prod
        {
            get { return _DerivativePort2Prod; }
            set { _DerivativePort2Prod = value; NotifyStaticPropertyChanged("DerivativePort2Prod"); }
        }



        public static string DerivativePort3Prod
        {
            get { return _DerivativePort3Prod; }
            set { _DerivativePort3Prod = value; NotifyStaticPropertyChanged("DerivativePort3Prod"); }
        }



        public static string DerivativePort4Prod
        {
            get { return _DerivativePort4Prod; }
            set { _DerivativePort4Prod = value; NotifyStaticPropertyChanged("DerivativePort4Prod"); }
        }


        public static string DerivativeIP1BCastProd
        {
            get { return _DerivativeIP1BCastProd; }
            set { _DerivativeIP1BCastProd = value; NotifyStaticPropertyChanged("DerivativeIP1BCastProd"); }
        }


        public static string DerivativePort1BCastProd
        {
            get { return _DerivativePort1BCastProd; }
            set { _DerivativePort1BCastProd = value; NotifyStaticPropertyChanged("DerivativePort1BCastProd"); }
        }

        public static string CurrencyIP1Prod
        {
            get { return _CurrencyIP1Prod; }
            set { _CurrencyIP1Prod = value; NotifyStaticPropertyChanged("CurrencyIP1Prod"); }
        }

        public static string CurrencyIP2Prod
        {
            get { return _CurrencyIP2Prod; }
            set { _CurrencyIP2Prod = value; NotifyStaticPropertyChanged("CurrencyIP2Prod"); }
        }



        public static string CurrencyIP3Prod
        {
            get { return _CurrencyIP3Prod; }
            set { _CurrencyIP3Prod = value; NotifyStaticPropertyChanged("CurrencyIP3Prod"); }
        }



        public static string CurrencyIP4Prod
        {
            get { return _CurrencyIP4Prod; }
            set { _CurrencyIP4Prod = value; NotifyStaticPropertyChanged("CurrencyIP4Prod"); }
        }



        public static string CurrencyPort1Prod
        {
            get { return _CurrencyPort1Prod; }
            set { _CurrencyPort1Prod = value; NotifyStaticPropertyChanged("CurrencyPort1Prod"); }
        }


        public static string CurrencyPort2Prod
        {
            get { return _CurrencyPort2Prod; }
            set { _CurrencyPort2Prod = value; NotifyStaticPropertyChanged("CurrencyPort2Prod"); }
        }



        public static string CurrencyPort3Prod
        {
            get { return _CurrencyPort3Prod; }
            set { _CurrencyPort3Prod = value; NotifyStaticPropertyChanged("CurrencyPort3Prod"); }
        }



        public static string CurrencyPort4Prod
        {
            get { return _CurrencyPort4Prod; }
            set { _CurrencyPort4Prod = value; NotifyStaticPropertyChanged("CurrencyPort4Prod"); }
        }


        public static string CurrencyIP1BCastProd
        {
            get { return _CurrencyIP1BCastProd; }
            set { _CurrencyIP1BCastProd = value; NotifyStaticPropertyChanged("CurrencyIP1BCastProd"); }
        }


        public static string CurrencyPort1BCastProd
        {
            get { return _CurrencyPort1BCastProd; }
            set { _CurrencyPort1BCastProd = value; NotifyStaticPropertyChanged("CurrencyPort1BCastProd"); }
        }



        public static string BoltIp1
        {
            get { return _BoltIp1; }
            set { _BoltIp1 = value; NotifyStaticPropertyChanged("BoltIp1"); }
        }





        public static string BoltPort1
        {
            get { return _BoltPort1; }
            set { _BoltPort1 = value; NotifyStaticPropertyChanged("BoltPort1"); }
        }



        public static string BoltIp2
        {
            get { return _BoltIp2; }
            set { _BoltIp2 = value; NotifyStaticPropertyChanged("BoltIp2"); }
        }



        public static string BoltPort2
        {
            get { return _BoltPort2; }
            set { _BoltPort2 = value; NotifyStaticPropertyChanged("BoltPort2"); }
        }



        public static string InterfaceIP
        {
            get { return _InterfaceIP; }
            set { _InterfaceIP = value; NotifyStaticPropertyChanged("InterfaceIP"); }
        }



        public static string IMLPort
        {
            get { return _IMLPort; }
            set { _IMLPort = value; NotifyStaticPropertyChanged("IMLPort"); }
        }



        public static string PrimaryExtranetIP
        {
            get { return _PrimaryExtranetIP; }
            set { _PrimaryExtranetIP = value; NotifyStaticPropertyChanged("PrimaryExtranetIP"); }
        }



        public static string SecondaryExtranetIP
        {
            get { return _SecondaryExtranetIP; }
            set { _SecondaryExtranetIP = value; NotifyStaticPropertyChanged("SecondaryExtranetIP"); }
        }



        #endregion

        #region RelayCommand

        private RelayCommand _BoltSettingClosing;

        public RelayCommand BoltSettingClosing
        {

            get { return _BoltSettingClosing ?? (_BoltSettingClosing = new RelayCommand((object e) => BoltSetting_Closing(e))); }

        }

        private void BoltSetting_Closing(object e)
        {
            Windows_BoltSettingsLocationChanged(e);
            ProfileSettings oBolt = System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault();
            if (oBolt != null)
            {
                oBolt.Close();
            }
        }


        private static RelayCommand _SettingScreenLoaded;

        public static RelayCommand SettingScreenLoaded
        {
            get
            {
                return _SettingScreenLoaded ?? (_SettingScreenLoaded = new RelayCommand((object e) => SettingScreenLoaded_Load(e)));

            }

        }

        private RelayCommand _myLocationChanged;

        public RelayCommand myLocationChanged
        {
            get
            {
                return _myLocationChanged ?? (_myLocationChanged = new RelayCommand(
                    (object e) => Windows_BoltSettingsLocationChanged(e)));

            }
        }



        private static RelayCommand _TxtEquityPort1_TextChanged;

        public static RelayCommand TxtEquityPort1_TextChanged
        {
            get
            {
                return _TxtEquityPort1_TextChanged ?? (_TxtEquityPort1_TextChanged = new RelayCommand((object e) => TxtEquityPort1_TextChangedevent(e)));
            }

        }

        private static void TxtEquityPort1_TextChangedevent(object e)
        {

            EquityPort1Prod = Regex.Replace(EquityPort1Prod, "[^0-9]+", "");

        }



        private static RelayCommand _TxtEquityPort2_TextChanged;

        public static RelayCommand TxtEquityPort2_TextChanged
        {
            get
            {
                return _TxtEquityPort2_TextChanged ?? (_TxtEquityPort2_TextChanged = new RelayCommand((object e) => TxtEquityPort2_TextChangedevent(e)));
            }

        }

        private static void TxtEquityPort2_TextChangedevent(object e)
        {

            EquityPort2Prod = Regex.Replace(EquityPort2Prod, "[^0-9]+", "");

        }


        private static RelayCommand _TxtBoltPort1_TextChanged;

        public static RelayCommand TxtBoltPort1_TextChanged
        {
            get
            {
                return _TxtBoltPort1_TextChanged ?? (_TxtBoltPort1_TextChanged = new RelayCommand((object e) => TxtBoltPort1_TextChangedevent(e)));
            }

        }

        private static void TxtBoltPort1_TextChangedevent(object e)
        {

            BoltPort1 = Regex.Replace(BoltPort1, "[^0-9]+", "");

        }

        private static RelayCommand _TxtEquityPort1BCastProd_TextChanged;

        public static RelayCommand TxtEquityPort1BCastProd_TextChanged
        {
            get
            {
                return _TxtEquityPort1BCastProd_TextChanged ?? (_TxtEquityPort1BCastProd_TextChanged = new RelayCommand((object e) => TxtEquityPort1BCastProd_TextChangedevent(e)));
            }

        }

        private static void TxtEquityPort1BCastProd_TextChangedevent(object e)
        {

            EquityPort1BCastProd = Regex.Replace(EquityPort1BCastProd, "[^0-9]+", "");

        }


        private static RelayCommand _TxtEquityPort2BCastProd_TextChanged;

        public static RelayCommand TxtEquityPort2BCastProd_TextChanged
        {
            get
            {
                return _TxtEquityPort2BCastProd_TextChanged ?? (_TxtEquityPort2BCastProd_TextChanged = new RelayCommand((object e) => TxtEquityPort2BCastProd_TextChangedevent(e)));
            }

        }

        private static void TxtEquityPort2BCastProd_TextChangedevent(object e)
        {

            EquityPort2BCastProd = Regex.Replace(EquityPort2BCastProd, "[^0-9]+", "");

        }

        private static RelayCommand _TxtDerivativePort1_TextChanged;

        public static RelayCommand TxtDerivativePort1_TextChanged
        {
            get
            {
                return _TxtDerivativePort1_TextChanged ?? (_TxtDerivativePort1_TextChanged = new RelayCommand((object e) => TxtDerivativePort1_TextChangedevent(e)));
            }

        }

        private static void TxtDerivativePort1_TextChangedevent(object e)
        {

            DerivativePort1Prod = Regex.Replace(DerivativePort1Prod, "[^0-9]+", "");

        }

        private static RelayCommand _TxtDerivativePort2_TextChanged;

        public static RelayCommand TxtDerivativePort2_TextChanged
        {
            get
            {
                return _TxtDerivativePort2_TextChanged ?? (_TxtDerivativePort2_TextChanged = new RelayCommand((object e) => TxtDerivativePort2_TextChangedevent(e)));
            }

        }

        private static void TxtDerivativePort2_TextChangedevent(object e)
        {

            DerivativePort2Prod = Regex.Replace(DerivativePort2Prod, "[^0-9]+", "");

        }


        private static RelayCommand _TxtDerivativePort1BCastProd_TextChanged;

        public static RelayCommand TxtDerivativePort1BCastProd_TextChanged
        {
            get
            {
                return _TxtDerivativePort1BCastProd_TextChanged ?? (_TxtDerivativePort1BCastProd_TextChanged = new RelayCommand((object e) => TxtDerivativePort1BCastProd_TextChangedevent(e)));
            }

        }

        private static void TxtDerivativePort1BCastProd_TextChangedevent(object e)
        {

            DerivativePort1BCastProd = Regex.Replace(DerivativePort1BCastProd, "[^0-9]+", "");

        }

        private static RelayCommand _TxtCurrencyPort1Prod_TextChanged;

        public static RelayCommand TxtCurrencyPort1Prod_TextChanged
        {
            get
            {
                return _TxtCurrencyPort1Prod_TextChanged ?? (_TxtCurrencyPort1Prod_TextChanged = new RelayCommand((object e) => TxtCurrencyPort1Prod_TextChangedevent(e)));
            }

        }

        private static void TxtCurrencyPort1Prod_TextChangedevent(object e)
        {

            CurrencyPort1Prod = Regex.Replace(CurrencyPort1Prod, "[^0-9]+", "");

        }

        private static RelayCommand _TxtCurrencyPort2Prod_TextChanged;

        public static RelayCommand TxtCurrencyPort2Prod_TextChanged
        {
            get
            {
                return _TxtCurrencyPort2Prod_TextChanged ?? (_TxtCurrencyPort2Prod_TextChanged = new RelayCommand((object e) => TxtCurrencyPort2Prod_TextChangedevent(e)));
            }

        }

        private static void TxtCurrencyPort2Prod_TextChangedevent(object e)
        {

            CurrencyPort2Prod = Regex.Replace(CurrencyPort2Prod, "[^0-9]+", "");

        }


        private static RelayCommand _TxtCurrencyPort1BCastProd_TextChanged;

        public static RelayCommand TxtCurrencyPort1BCastProd_TextChanged
        {
            get
            {
                return _TxtCurrencyPort1BCastProd_TextChanged ?? (_TxtCurrencyPort1BCastProd_TextChanged = new RelayCommand((object e) => TxtCurrencyPort1BCastProd_TextChangedevent(e)));
            }

        }

        private static void TxtCurrencyPort1BCastProd_TextChangedevent(object e)
        {

            CurrencyPort1BCastProd = Regex.Replace(CurrencyPort1BCastProd, "[^0-9]+", "");

        }


        private static RelayCommand _TxtEquityPort3_TextChanged;

        public static RelayCommand TxtEquityPort3_TextChanged
        {
            get
            {
                return _TxtEquityPort3_TextChanged ?? (_TxtEquityPort3_TextChanged = new RelayCommand((object e) => TxtEquityPort3_TextChangedevent(e)));
            }

        }

        private static void TxtEquityPort3_TextChangedevent(object e)
        {

            EquityPort3Prod = Regex.Replace(EquityPort3Prod, "[^0-9]+", "");

        }

        private static RelayCommand _TxtEquityPort4_TextChanged;

        public static RelayCommand TxtEquityPort4_TextChanged
        {
            get
            {
                return _TxtEquityPort4_TextChanged ?? (_TxtEquityPort4_TextChanged = new RelayCommand((object e) => TxtEquityPort4_TextChangedevent(e)));
            }

        }

        private static void TxtEquityPort4_TextChangedevent(object e)
        {

            EquityPort4Prod = Regex.Replace(EquityPort4Prod, "[^0-9]+", "");

        }







        private static RelayCommand _TxtBoltPort2_TextChanged;

        public static RelayCommand TxtBoltPort2_TextChanged
        {
            get
            {
                return _TxtBoltPort2_TextChanged ?? (_TxtBoltPort2_TextChanged = new RelayCommand((object e) => TxtBoltPort2_TextChangedevent(e)));
            }

        }

        private static void TxtBoltPort2_TextChangedevent(object e)
        {

            BoltPort2 = Regex.Replace(BoltPort2, "[^0-9]+", "");

        }

        private static RelayCommand _TxtDerivativePort3_TextChanged;

        public static RelayCommand TxtDerivativePort3_TextChanged
        {
            get
            {
                return _TxtDerivativePort3_TextChanged ?? (_TxtDerivativePort3_TextChanged = new RelayCommand((object e) => TxtDerivativePort3_TextChangedevent(e)));
            }

        }

        private static void TxtDerivativePort3_TextChangedevent(object e)
        {

            DerivativePort3Prod = Regex.Replace(DerivativePort3Prod, "[^0-9]+", "");

        }


        private static RelayCommand _TxtDerivativePort4Prod_TextChanged;

        public static RelayCommand TxtDerivativePort4Prod_TextChanged
        {
            get
            {
                return _TxtDerivativePort4Prod_TextChanged ?? (_TxtDerivativePort4Prod_TextChanged = new RelayCommand((object e) => TxtDerivativePort4Prod_TextChangedevent(e)));
            }

        }

        private static void TxtDerivativePort4Prod_TextChangedevent(object e)
        {

            DerivativePort4Prod = Regex.Replace(DerivativePort4Prod, "[^0-9]+", "");

        }


        private static RelayCommand _TxtCurrencyPort3Prod_TextChanged;

        public static RelayCommand TxtCurrencyPort3Prod_TextChanged
        {
            get
            {
                return _TxtCurrencyPort3Prod_TextChanged ?? (_TxtCurrencyPort3Prod_TextChanged = new RelayCommand((object e) => TxtCurrencyPort3Prod_TextChangedevent(e)));
            }

        }

        private static void TxtCurrencyPort3Prod_TextChangedevent(object e)
        {

            CurrencyPort3Prod = Regex.Replace(CurrencyPort3Prod, "[^0-9]+", "");

        }

        private static RelayCommand _TxtCurrencyPort4Prod_TextChanged;

        public static RelayCommand TxtCurrencyPort4Prod_TextChanged
        {
            get
            {
                return _TxtCurrencyPort4Prod_TextChanged ?? (_TxtCurrencyPort3Prod_TextChanged = new RelayCommand((object e) => TxtCurrencyPort4Prod_TextChangedevent(e)));
            }

        }

        private static void TxtCurrencyPort4Prod_TextChangedevent(object e)
        {

            CurrencyPort4Prod = Regex.Replace(CurrencyPort4Prod, "[^0-9]+", "");

        }



        private static RelayCommand _TxtIMLPort_TextChanged;

        public static RelayCommand TxtIMLPort_TextChanged
        {
            get
            {
                return _TxtIMLPort_TextChanged ?? (_TxtIMLPort_TextChanged = new RelayCommand((object e) => TxtIMLPort_TextChangedevent(e)));
            }
        }

        private static void TxtIMLPort_TextChangedevent(object e)
        {

            IMLPort = Regex.Replace(IMLPort, "[^0-9]+", "");

        }


        private static RelayCommand _btnSave_Click;

        public static RelayCommand btnSave_Click
        {
            get
            {
                return _btnSave_Click ?? (_btnSave_Click = new RelayCommand(
                    (object e) => SaveChanges(e)));

            }

        }


        private static RelayCommand _btnCancel_Click;

        public static RelayCommand btnCancel_Click
        {
            get
            {
                return _btnCancel_Click ?? (_btnCancel_Click = new RelayCommand(
                    (object e) => CancelChanges(e)));

            }

        }

        private static RelayCommand _btnModify_Click;

        public static RelayCommand btnModify_Click
        {
            get
            {
                return _btnModify_Click ?? (_btnModify_Click = new RelayCommand(
                    (object e) => ModifyChanges(e)));


            }

        }

        //public object LeftPosition { get; private set; }
        //public object TopPosition { get; private set; }
        //public object Width { get; private set; }
        //public object Height { get; private set; }

        private static void ModifyChanges(object e)
        {
            BtnModifyEnable = Boolean.FalseString;
            if (CtrEnable == Boolean.TrueString)
            {
                CtrEnable = Boolean.FalseString;
            }
            else { CtrEnable = Boolean.TrueString; }

        }

        private static void CancelChanges(object e)
        {

            if (CtrEnable == Boolean.TrueString)
            {
                CtrEnable = Boolean.FalseString;
                BtnModifyEnable = Boolean.TrueString;

                //    PopulateIpEnvironment();

                SettingScreenLoaded_Load(e);
                //    PopulateIpEnvironment();


            }

            else
            {
                CtrEnable = Boolean.FalseString;
                BtnModifyEnable = Boolean.TrueString;

                //  PopulateIpEnvironment();
                SettingScreenLoaded_Load(e);
                //  PopulateIpEnvironment();

            }
            ProfileSettings oBolt = System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault();
            if (oBolt != null)
            {
                oBolt.Close();
            }
        }

        private static void SettingScreenLoaded_Load(object e)
        {
            // BtnModifyEnable = Boolean.TrueString;
            ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).EquityIP1Prod.Text = EquityIP1Prod;
            ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).EquityIP2Prod.Text = EquityIP2Prod;
            ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).EquityIP3Prod.Text = EquityIP3Prod;
            ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).EquityIP4Prod.Text = EquityIP4Prod;
            ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).EquityIP1BCastProd.Text = EquityIP1BCastProd;
            // ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).EquityIP2BCastProd.Text = EquityIP2BCastProd;
            ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).DerivativeIP1Prod.Text = DerivativeIP1Prod;
            ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).DerivativeIP2Prod.Text = DerivativeIP2Prod;
            ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).DerivativeIP3Prod.Text = DerivativeIP3Prod;
            ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).DerivativeIP4Prod.Text = DerivativeIP4Prod;
            ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).DerivativeIP1BCastProd.Text = DerivativeIP1BCastProd;
            ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).CurrencyIP1Prod.Text = CurrencyIP1Prod;
            ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).CurrencyIP2Prod.Text = CurrencyIP2Prod;
            ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).CurrencyIP3Prod.Text = CurrencyIP3Prod;
            ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).CurrencyIP4Prod.Text = CurrencyIP4Prod;
            ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).CurrencyIP1BCastProd.Text = CurrencyIP1BCastProd;
            // ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).BoltIp1.Text = BoltIp1;
            // ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).BoltIp2.Text = BoltIp2;
            //  ((AdvancedTWS.View.Settings.BoltSettings)(System.Windows.Application.Current.Windows.OfType<MainSettings>().FirstOrDefault().SettingsWindow.Text)).DerivativeIP1BCastProd.Text = DerivativeIP1BCastProd;
            ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).InterfaceIP.Text = InterfaceIP;
            ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).PrimaryExtranetIP.Text = PrimaryExtranetIP;
            ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).SecondaryExtranetIP.Text = SecondaryExtranetIP;
            //ModifyChanges(e);

        }

        private static void SaveChanges(object e)
        {

            EquityIP1Prod = ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).EquityIP1Prod.Text;
            EquityIP2Prod = ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).EquityIP2Prod.Text;
            EquityIP3Prod = ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).EquityIP3Prod.Text;
            EquityIP4Prod = ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).EquityIP4Prod.Text;
            EquityIP1BCastProd = ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).EquityIP1BCastProd.Text;
            // EquityIP2BCastProd = ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).EquityIP2BCastProd.Text;
            DerivativeIP1Prod = ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).DerivativeIP1Prod.Text;
            DerivativeIP2Prod = ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).DerivativeIP2Prod.Text;
            DerivativeIP3Prod = ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).DerivativeIP3Prod.Text;
            DerivativeIP4Prod = ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).DerivativeIP4Prod.Text;
            DerivativeIP1BCastProd = ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).DerivativeIP1BCastProd.Text;
            CurrencyIP1Prod = ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).CurrencyIP1Prod.Text;
            CurrencyIP1Prod = ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).CurrencyIP1Prod.Text;
            CurrencyIP1Prod = ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).CurrencyIP1Prod.Text;
            CurrencyIP1Prod = ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).CurrencyIP1Prod.Text;
            CurrencyIP1BCastProd = ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).CurrencyIP1BCastProd.Text;
            // BoltIp1 = ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).BoltIp1.Text;
            // BoltIp2 = ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).BoltIp2.Text;
            InterfaceIP = ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).InterfaceIP.Text;
            PrimaryExtranetIP = ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).PrimaryExtranetIP.Text;
            SecondaryExtranetIP = ((BoltSettings)(System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault().BoltSettingWindow.Content)).SecondaryExtranetIP.Text;

            BtnModifyEnable = Boolean.TrueString;
            CtrEnable = Boolean.FalseString;
            //BtnCancelEnable = Boolean.FalseString;
            //BtnSaveEnable = Boolean.FalseString;
            //IniParser parser = new IniParser(BoltINIPath.ToString());
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "EQTHOST1", EquityIP1Prod);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "EQTPORT1", EquityPort1Prod);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "EQTHOST2", EquityIP2Prod);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "EQTPORT2", EquityPort2Prod);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "EQTHOST3", EquityIP3Prod);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "EQTPORT3", EquityPort3Prod);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "EQTHOST4", EquityIP4Prod);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "EQTHPORT4", EquityPort4Prod);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "EQTMultiCastAdd", EquityIP1BCastProd);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "EQTMultiCastAdd2", EquityIP2BCastProd);


            MainWindowVM.ParserBoltIni.AddSetting("RSC", "EQTUDPPort", EquityPort1BCastProd);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "EQTUDPPort2", EquityPort2BCastProd);


            MainWindowVM.ParserBoltIni.AddSetting("RSC", "DERHOST1", DerivativeIP1Prod);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "DERPORT1", DerivativePort1Prod);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "DERHOST2", DerivativeIP2Prod);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "DERPORT2", DerivativePort2Prod);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "DERHOST3", DerivativeIP3Prod);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "DERPORT3", DerivativePort3Prod);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "DERHOST4", DerivativeIP4Prod);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "DERPORT4", DerivativePort4Prod);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "DERMultiCastAdd", DerivativeIP1BCastProd);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "DERUDPPort", DerivativePort1BCastProd);

            MainWindowVM.ParserBoltIni.AddSetting("RSC", "NTAHost1", CurrencyIP1Prod);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "NTAPort1", CurrencyPort1Prod);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "NTAHost2", CurrencyIP2Prod);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "NTAPort2", CurrencyPort2Prod);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "NTAHOST3", CurrencyIP3Prod);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "NTAPORT3", CurrencyPort3Prod);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "NTAHOST4", CurrencyIP4Prod);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "NTAPORT4", CurrencyPort4Prod);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "NTAMultiCastAdd", CurrencyIP1BCastProd);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "NTAUDPPort", CurrencyPort1BCastProd);

            MainWindowVM.ParserBoltIni.AddSetting("RSC", "Primary", BoltIp1);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "Secondary", BoltIp2);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "PortNo1", BoltPort1);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "PortNo2", BoltPort2);

            MainWindowVM.ParserBoltIni.AddSetting("RSC", "MultiCastAdd", InterfaceIP);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "IMLPort", IMLPort);

            MainWindowVM.ParserBoltIni.AddSetting("RSC", "PrimaryEXTRANET", PrimaryExtranetIP);
            MainWindowVM.ParserBoltIni.AddSetting("RSC", "SecondaryEXTRANET", SecondaryExtranetIP);

            MainWindowVM.ParserBoltIni.SaveSettings(MainWindowVM.BoltINIPath.ToString());
            System.Windows.MessageBox.Show("Configuration Settings has been Changed. To bring the changes in to effect, Please Restart the BOLT again.", "Message", MessageBoxButton.OK);

        }

        #endregion

        //#region Collection Properties
        //    private static List<string> _IPEnvironmentList;

        //    public static List<string> IPEnvironmentList
        //    {
        //        get { return _IPEnvironmentList; }
        //        set { _IPEnvironmentList = value; NotifyStaticPropertyChanged("IPEnvironmentList"); }
        //    }

        //    private static string _SelectedEnvironment;
        //    public static string SelectedEnvironment
        //    {
        //        get { return _SelectedEnvironment; }
        //        set
        //        {
        //            _SelectedEnvironment = value; NotifyStaticPropertyChanged("SelectedEnvironment");
        //            //SelectionChanged();

        //            //PopulateIpEnvironment();
        //        }
        //    }


        //#endregion

        #region Constructor 
        public BoltSettingsVM()
        {
            BindData();
            // BindDataTWS();
            CtrEnable = Boolean.FalseString;
            BtnModifyEnable = Boolean.TrueString;
        }


        #endregion

        #region Methods
        //    private static void PopulateIpEnvironment()
        //    {
        //        try
        //        {

        //            SelectedEnvironment =   Enumerations.Environment.Live.ToString();
        //            IPEnvironmentList.Add(SelectedEnvironment);
        //            }
        //        catch (Exception e)
        //        {
        //            ExceptionUtility.LogError(e);
        //        }

        //}
        //private void BindDataTWS()
        //{
        //    IniParser parser = new IniParser(MainWindowVM.TwsINIPath.ToString());
        //    ScripIdEquity = Convert.ToBoolean(MainWindowVM.parser.GetSetting("Login Settings", "EQTSCR").ToString());
        //    ScripNameEquity = Convert.ToBoolean(MainWindowVM.parser.GetSetting("Login Settings", "EQTSCRNAME").ToString());
        //    ScripIdDerivative = Convert.ToBoolean(MainWindowVM.parser.GetSetting("Login Settings", "DEVSCR").ToString());
        //    ScripNameDerivative = Convert.ToBoolean(MainWindowVM.parser.GetSetting("Login Settings", "DEVSCRNAME").ToString());
        //    ScripIdCurrency = Convert.ToBoolean(MainWindowVM.parser.GetSetting("Login Settings", "CURSCR").ToString());
        //    ScripNameCurrency = Convert.ToBoolean(MainWindowVM.parser.GetSetting("Login Settings", "CURSCRNAME").ToString());
        //}

        private static void BindData()
        {
            IniParser parser = new IniParser(MainWindowVM.BoltINIPath.ToString());
            EquityIP1Prod = parser.GetSetting("RSC", "EQTHOST1");
            EquityPort1Prod = parser.GetSetting("RSC", "EQTPORT1");
            EquityIP2Prod = parser.GetSetting("RSC", "EQTHOST2");
            EquityPort2Prod = parser.GetSetting("RSC", "EQTPORT2");
            EquityIP3Prod = parser.GetSetting("RSC", "EQTHOST3");
            EquityPort3Prod = parser.GetSetting("RSC", "EQTPORT3");
            EquityIP4Prod = parser.GetSetting("RSC", "EQTHOST4");
            EquityPort4Prod = parser.GetSetting("RSC", "EQTPORT4");
            EquityIP1BCastProd = parser.GetSetting("RSC", "EQTMultiCastAdd");
            EquityPort1BCastProd = parser.GetSetting("RSC", "EQTUDPPort");
            EquityIP2BCastProd = parser.GetSetting("RSC", "EQTMultiCastAdd2");
            EquityPort2BCastProd = parser.GetSetting("RSC", "EQTUDPPort2");


            DerivativeIP1Prod = parser.GetSetting("RSC", "DERHOST1");
            DerivativePort1Prod = parser.GetSetting("RSC", "DERPORT1");
            DerivativeIP2Prod = parser.GetSetting("RSC", "DERHOST2");
            DerivativePort2Prod = parser.GetSetting("RSC", "DERPORT2");
            DerivativeIP3Prod = parser.GetSetting("RSC", "DERHOST3");
            DerivativePort3Prod = parser.GetSetting("RSC", "DERPORT3");
            DerivativeIP4Prod = parser.GetSetting("RSC", "DERHOST4");
            DerivativePort4Prod = parser.GetSetting("RSC", "DERPORT4");
            DerivativeIP1BCastProd = parser.GetSetting("RSC", "DERMultiCastAdd");
            DerivativePort1BCastProd = parser.GetSetting("RSC", "DERUDPPort");


            CurrencyIP1Prod = parser.GetSetting("RSC", "NTAHost1");
            CurrencyPort1Prod = parser.GetSetting("RSC", "NTAPort1");
            CurrencyIP2Prod = parser.GetSetting("RSC", "NTAHost2");
            CurrencyPort2Prod = parser.GetSetting("RSC", "NTAPort2");
            CurrencyIP3Prod = parser.GetSetting("RSC", "NTAHOST3");
            CurrencyPort3Prod = parser.GetSetting("RSC", "NTAPORT3");
            CurrencyIP4Prod = parser.GetSetting("RSC", "NTAHOST4");
            CurrencyPort4Prod = parser.GetSetting("RSC", "NTAPORT4");
            CurrencyIP1BCastProd = parser.GetSetting("RSC", "NTAMultiCastAdd");
            CurrencyPort1BCastProd = parser.GetSetting("RSC", "NTAUDPPort");


            BoltIp1 = parser.GetSetting("RSC", "Primary");
            BoltIp2 = parser.GetSetting("RSC", "Secondary");
            BoltPort1 = parser.GetSetting("RSC", "PortNo1");
            BoltPort2 = parser.GetSetting("RSC", "PortNo2");


            InterfaceIP = parser.GetSetting("RSC", "MultiCastAdd");
            IMLPort = parser.GetSetting("RSC", "IMLPort");

            PrimaryExtranetIP = parser.GetSetting("RSC", "PrimaryEXTRANET");
            SecondaryExtranetIP = parser.GetSetting("RSC", "SecondaryEXTRANET");


        }

        private void Windows_BoltSettingsLocationChanged(object e)
        {
            if (ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = (BoltAppSettingsWindowsPosition)ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.SETTINGS != null && oBoltAppSettingsWindowsPosition.SETTINGS.WNDPOSITION != null)
                {
                    oBoltAppSettingsWindowsPosition.SETTINGS.WNDPOSITION.Left = Convert.ToInt32(LeftPosition);
                    oBoltAppSettingsWindowsPosition.SETTINGS.WNDPOSITION.Top = Convert.ToInt32(TopPosition);
                    oBoltAppSettingsWindowsPosition.SETTINGS.WNDPOSITION.Right = Convert.ToInt32(Width);
                    oBoltAppSettingsWindowsPosition.SETTINGS.WNDPOSITION.Down = Convert.ToInt32(Height);
                }
                //CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(@"D:\TWS_DotNetNewStructure\TWS_DOTNETT\CommonFrontEnd\bin\Debug\xml\Users\AppSettings\920200000.xml", "WindowsPosition");
                SaveConfiguration.SaveUserConfiguration(SettingsManager.AppSettingsXmlPath, "WindowsPosition");
            }
        }





        #endregion

        #region StaticNotifyStaticPropertyChangedEvent
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged
                 = delegate { };
        private static void NotifyStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region NotifyStaticPropertyChanged
        //public event PropertyChangedEventHandler PropertyChanged;
        //private void NotifyPropertyChanged(String propertyName = "")
        //{
        //    PropertyChangedEventHandler handler = this.PropertyChanged;
        //    if (handler != null)
        //    {
        //        var e = new PropertyChangedEventArgs(propertyName);
        //        this.PropertyChanged(this, e);
        //    }

        //}
        #endregion

    }
#endif


}
