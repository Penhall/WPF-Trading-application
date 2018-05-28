using CommonFrontEnd.Common;
using CommonFrontEnd.Global;
using CommonFrontEnd.Model;
using CommonFrontEnd.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace CommonFrontEnd.ViewModel
{
    partial class LocationIDVM : BaseViewModel
    {

        #region Properties

        private string _SenderLocationID;

        public string SenderLocationID
        {
            get { return _SenderLocationID; }
            set { _SenderLocationID = value; NotifyPropertyChanged(nameof(SenderLocationID)); }
        }

        public string CtrEnable { get; private set; }
        public string BtnOKEnable { get; private set; }
        public string BtnCloseEnable { get; private set; }
        #endregion

        #region RelayCommand

        private RelayCommand _OK_Click;

        public RelayCommand OK_Click
        {
            get
            {
                return _OK_Click ?? (_OK_Click = new RelayCommand(
                    (object e) => OKChanges(e)));

            }

        }

        private RelayCommand _Close_Click;

        public RelayCommand Close_Click
        {
            get
            {
                return _Close_Click ?? (_Close_Click = new RelayCommand(
                    (object e) => CloseChanges(e)));

            }

        }


        //private RelayCommand _LocationIDtxt_TextChanged;

        //public RelayCommand LocationIDtxt_TextChanged
        //{
        //    get
        //    {
        //        return _LocationIDtxt_TextChanged ?? (_LocationIDtxt_TextChanged = new RelayCommand((object e) => LocationIDtxt_TextChangedevent(e)));
        //    }

        //}

        //private void LocationIDtxt_TextChangedevent(object e)
        //{

        //    if (SenderLocationID != null)
        //    {
        //        SenderLocationID = Regex.Replace(SenderLocationID, "[^0-9]+", "");
        //    }
        //}

        #endregion


        private void OKChanges(object e)
        {

            bool validate = ValidateInput();
            if (!validate)
            {
                return;
            }

            string merge = UtilityLoginDetails.GETInstance.MemberId.ToString() + UtilityLoginDetails.GETInstance.TraderId.ToString();
            MainWindowVM.parser.AddSetting("Login Settings", "LOCID" + merge, SenderLocationID);
            UtilityLoginDetails.GETInstance.SenderLocationId = SenderLocationID;
            //string merge = UtilityLoginDetails.GETInstance.MemberId.ToString() + UtilityLoginDetails.GETInstance.TraderId.ToString();
            //MainWindowVM.parser.AddSetting("Login Settings","LOCID"+merge, );
            MainWindowVM.parser.SaveSettings(MainWindowVM.TwsINIPath.ToString());

            LocationID oLocationID = System.Windows.Application.Current.Windows.OfType<LocationID>().FirstOrDefault();
            if (oLocationID != null)
                oLocationID.Close();
        }

        private bool ValidateInput()
        {
            if ((String.IsNullOrWhiteSpace(SenderLocationID)))
            {
                BtnOKEnable = Boolean.FalseString;
                MessageBox.Show("Please Enter Location ID", "Warning");
                return false;
            }
            //else if()
            //{

            //}
            else if (SenderLocationID != null && SenderLocationID.Length != 16)
            {
                BtnOKEnable = Boolean.FalseString;
                // SenderLocationID = Regex.Replace(SenderLocationID, "[^0-9]+", "");
                MessageBox.Show("Please Enter Valid Location ID", "Warning");
                return false;
            }
            else
            {
                BtnOKEnable = Boolean.TrueString;
                return true;
            }
        }



        private void CloseChanges(object e)
        {
            try
            {
                Process process = Process.GetProcessesByName("imlPro").FirstOrDefault();
                if (process != null)
                    process.Kill();

                Process.GetCurrentProcess().Kill();
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        #region constructor

        public LocationIDVM()
        {


        }

        #endregion

    }
}
