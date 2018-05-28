using CommonFrontEnd.Common;
using CommonFrontEnd.Processor;
using CommonFrontEnd.Processor.Order;
using CommonFrontEnd.View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CommonFrontEnd.ViewModel
{
    public class CustomMessageBoxVM : BaseViewModel
    {
        #region Properties
        RegistryKey regKey = null;
        private string _Title;

        public string Title
        {
            get { return _Title; }
            set { _Title = value; NotifyPropertyChanged(nameof(Title)); }
        }
        private string _CapitalInfoMsg;

        public string CapitalInfoMsg
        {
            get { return _CapitalInfoMsg; }
            set { _CapitalInfoMsg = value; NotifyPropertyChanged(nameof(CapitalInfoMsg)); }
        }

        private bool _chkDontShow = false;

        public bool chkDontShow
        {
            get { return _chkDontShow; }
            set { _chkDontShow = value; NotifyPropertyChanged(nameof(chkDontShow)); }
        }

        private static string _screenID;

        public static string ScreenID
        {
            get { return _screenID; }
            set { _screenID = value; }
        }

        #endregion

        #region RelayCommands

        private RelayCommand _btnOkClick;
        public RelayCommand btnOkClick
        {
            get { return _btnOkClick ?? (_btnOkClick = new RelayCommand((object e) => btnOkClick_Click(e))); }
        }

        private static CustomMessageBoxVM _getinstance;

        public static CustomMessageBoxVM GetInstance
        {
            get
            {
                if (_getinstance == null)
                {
                    _getinstance = new CustomMessageBoxVM();
                }
                return _getinstance;
            }
        }

        #endregion

        #region methods
        private void btnOkClick_Click(object e)
        {
            if (chkDontShow)
            {
                if (ScreenID == "i070")
                    UMSProcessor.i070 = 1;
                else if (ScreenID == "i7080")
                    UMSProcessor.i7080 = 1;
                else if (ScreenID == "i8090")
                    UMSProcessor.i8090 = 1;
                else if (ScreenID == "i90100")
                    UMSProcessor.i90100 = 1;
                else if (ScreenID == "iFlag")
                    PositionLimitNotification.ShowFlag = 1;
            }
            CustomMessageBox ocustomwindow = e as CustomMessageBox;
            ocustomwindow.Hide();
        }

        public void ShowDialog(string title, string Message, string screenID)
        {

            ScreenID = screenID;
            chkDontShow = false;
            CommonMessagingWindowVM.ProcessMiscellaneousMessages(Message, "Alert");

            CustomMessageBox objcustwindow = Application.Current.Windows.OfType<CustomMessageBox>().FirstOrDefault();
            if (objcustwindow != null)
            {
                Title = title;
                CapitalInfoMsg = Message;
                objcustwindow.Focus();
                objcustwindow.Activate();
                objcustwindow.ShowDialog();
            }
            else
            {
                objcustwindow = new CustomMessageBox();
                Title = title;
                CapitalInfoMsg = Message;
                objcustwindow.ShowDialog();
            }
        }
        #endregion

        #region Constructor
        public CustomMessageBoxVM()
        {
            // regKey = Registry.CurrentUser.CreateSubKey(Environment.CurrentDirectory);
        }
        #endregion
    }

}
