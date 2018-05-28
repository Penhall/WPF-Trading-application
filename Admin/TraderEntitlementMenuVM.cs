using CommonFrontEnd.Common;
using CommonFrontEnd.View.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CommonFrontEnd.ViewModel.Admin
{
    public partial class TraderEntitlementMenuVM
    {

        public TraderEntitlementMenuVM()
        {

        }

    }

    public partial class TraderEntitlementMenuVM
    {
#if TWS

        #region RelayCommands

        private RelayCommand _TraderRightsWindow;

        public RelayCommand TraderRightsWindow
        {
            get
            {
                return _TraderRightsWindow ?? (_TraderRightsWindow = new RelayCommand((object e) => TraderRightsWindow_Click(e)));
            }
        }


        private RelayCommand _ClientMasterWindow;

        public RelayCommand ClientMasterWindow
        {
            get
            {
                return _ClientMasterWindow ?? (_ClientMasterWindow = new RelayCommand((object e) => ClientMasterWindow_Click(e)));
            }
        }


        private RelayCommand _ScripBlockUnblock;

        public RelayCommand ScripBlockUnblock
        {
            get
            {
                return _ScripBlockUnblock ?? (_ScripBlockUnblock = new RelayCommand((object e) => ScripBlockUnblock_Click(e)));
            }
        }


        private RelayCommand _UploadGenerate;

        public RelayCommand UploadGenerate
        {
            get
            {
                return _UploadGenerate ?? (_UploadGenerate = new RelayCommand((object e) => UploadGenerate_Click(e)));
            }
        }

        #endregion


        public static void TraderRightsWindow_Click(object e)
        {
            try
            {
                TraderEntitlementMenu oTraderEntitlementMenu = System.Windows.Application.Current.Windows.OfType<TraderEntitlementMenu>().FirstOrDefault();
                TraderEntitlement oTraderEntitlement = System.Windows.Application.Current.Windows.OfType<TraderEntitlement>().FirstOrDefault();
                if (oTraderEntitlement != null)
                {
                    oTraderEntitlement.Show();
                    oTraderEntitlement.Focus();
                    oTraderEntitlementMenu.Hide();
                }
                else
                {
                    oTraderEntitlement = new TraderEntitlement();                    
                    oTraderEntitlement.Activate();
                    oTraderEntitlement.Owner = Application.Current.MainWindow;
                    oTraderEntitlement.Show();
                    oTraderEntitlementMenu.Hide();
                }
            }
            catch (Exception ex)
            {

                /*throw*/
                ;
            }

        }


        public static void ClientMasterWindow_Click(object e)
        {
            try
            {
                TraderEntitlementMenu oTraderEntitlementMenu = System.Windows.Application.Current.Windows.OfType<TraderEntitlementMenu>().FirstOrDefault();
                ClientMaster oClientMaster = System.Windows.Application.Current.Windows.OfType<ClientMaster>().FirstOrDefault();
                if (oClientMaster != null)
                {
                    oClientMaster.Show();
                    oClientMaster.Focus();
                    oTraderEntitlementMenu.Hide();
                }
                else
                {
                    oClientMaster = new ClientMaster();
                    oClientMaster.Owner = Application.Current.MainWindow;
                    oClientMaster.Activate();
                    oClientMaster.Show();
                    oTraderEntitlementMenu.Hide();
                }
            }
            catch (Exception ex)
            {

                /*throw*/
                ;
            }

        }


        public static void ScripBlockUnblock_Click(object e)
        {
            try
            {
                TraderEntitlementMenu oTraderEntitlementMenu = System.Windows.Application.Current.Windows.OfType<TraderEntitlementMenu>().FirstOrDefault();
                Scrip_Block_Unblock oScripBlockUnblock = System.Windows.Application.Current.Windows.OfType<Scrip_Block_Unblock>().FirstOrDefault();
                if (oScripBlockUnblock != null)
                {
                    oScripBlockUnblock.Show();
                    oScripBlockUnblock.Focus();
                    oTraderEntitlementMenu.Hide();
                }
                else
                {
                    oScripBlockUnblock = new Scrip_Block_Unblock();
                    oScripBlockUnblock.Owner = Application.Current.MainWindow;
                    oScripBlockUnblock.Activate();
                    oScripBlockUnblock.Show();
                    oTraderEntitlementMenu.Hide();
                }
            }
            catch (Exception ex)
            {

                /*throw*/
                ;
            }

        }


        public static void UploadGenerate_Click(object e)
        {
            try
            {
                TraderEntitlementMenu oTraderEntitlementMenu = System.Windows.Application.Current.Windows.OfType<TraderEntitlementMenu>().FirstOrDefault();
                Upload_Generate oUploadGenerate = System.Windows.Application.Current.Windows.OfType<Upload_Generate>().FirstOrDefault();
                if (oUploadGenerate != null)
                {
                    oUploadGenerate.Show();
                    oUploadGenerate.Focus();
                    oTraderEntitlementMenu.Hide();
                }
                else
                {
                    oUploadGenerate = new Upload_Generate();
                    oUploadGenerate.Owner = Application.Current.MainWindow;
                    oUploadGenerate.Activate();
                    oUploadGenerate.Show();
                    oTraderEntitlementMenu.Hide();
                }
            }
            catch (Exception ex)
            {

                /*throw*/
                ;
            }

        }


#endif
    }

    public partial class TraderEntitlementMenuVM
    {

    }
}
