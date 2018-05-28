using CommonFrontEnd.Common;
using CommonFrontEnd.Common.DataGridHelperClasses;
using CommonFrontEnd.Global;
using CommonFrontEnd.Model;
using CommonFrontEnd.Model.ETIMessageStructure;
using CommonFrontEnd.Model.Order;
using CommonFrontEnd.Model.Trade;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace CommonFrontEnd.ViewModel
{
    class CommonMessagingWindowVM : BaseViewModel
    {
#if TWS
        #region Properties
        private static SynchronizationContext uiContext;
        public static AutoResetEvent waitHandle = new AutoResetEvent(false);
        static CommonMessagingWindow mWindow = null;

        private string _SearchBoxTxt;

        public string SearchBoxTxt
        {
            get { return _SearchBoxTxt; }
            set
            {
                _SearchBoxTxt = value;
                OnChangeofSearchBoxTxt();
                NotifyPropertyChanged(nameof(SearchBoxTxt));
            }
        }

        private string _SearchBoxVisibility = "Hidden";

        public string SearchBoxVisibility
        {
            get { return _SearchBoxVisibility; }
            set { _SearchBoxVisibility = value; NotifyPropertyChanged(nameof(SearchBoxVisibility)); }
        }

        private string _FilterSelectContent = "DeselectAll";

        public string FilterSelectContent
        {
            get { return _FilterSelectContent; }
            set { _FilterSelectContent = value; NotifyPropertyChanged(nameof(FilterSelectContent)); }
        }

        #endregion

        #region Collections
        private static ObservableCollection<CommonMessagingWindowModel> _CollectionCMW;

        public static ObservableCollection<CommonMessagingWindowModel> CollectionCMW
        {
            get { return _CollectionCMW; }
            set { _CollectionCMW = value; }
        }

        private static ObservableCollection<CheckedListItem<string>> _customerFilters;

        public static ObservableCollection<CheckedListItem<string>> customerFilters
        {
            get { return _customerFilters; }
            set { _customerFilters = value; }
        }

        //private ObservableCollection<Customer> customers = new ObservableCollection<Customer>();
        //private ObservableCollection<CheckedListItem<string>> customerFilters = null;
        public CollectionViewSource viewSource = null;
        #endregion

        #region Relay Command
        private RelayCommand _btnCountryFilter_Click;
        public RelayCommand btnCountryFilter_Click
        {
            get
            {
                return _btnCountryFilter_Click ?? (_btnCountryFilter_Click = new RelayCommand(
                    (object e) => btnCountryFilter_Click_Click(e, null)
                        ));
            }
        }

        private RelayCommand _btnSelectAll_Click;
        public RelayCommand btnSelectAll_Click
        {
            get
            {
                return _btnSelectAll_Click ?? (_btnSelectAll_Click = new RelayCommand(
                    (object e) => btnSelectAll_Click_Click(e, null)
                        ));
            }
        }


        //private RelayCommand _btnUnselectAll_Click;
        //public RelayCommand btnUnselectAll_Click
        //{
        //    get
        //    {
        //        return _btnUnselectAll_Click ?? (_btnUnselectAll_Click = new RelayCommand(
        //            (object e) => btnUnselectAll_Click_Click(e, null)
        //                ));
        //    }
        //}

        private RelayCommand _btnSearchClick;
        public RelayCommand btnSearchClick
        {
            get
            {
                return _btnSearchClick ?? (_btnSearchClick = new RelayCommand(
                    (object e) => btnSearchClick_Click(e, null)
                        ));
            }
        }

        #endregion

        public CommonMessagingWindowVM()
        {
            uiContext = SynchronizationContext.Current;
            CollectionCMW = new AsyncObservableCollection<CommonMessagingWindowModel>();
            customerFilters = new AsyncObservableCollection<CheckedListItem<string>>();
            mWindow = System.Windows.Application.Current.Windows.OfType<CommonMessagingWindow>().FirstOrDefault();
            viewSource = ((System.Windows.Data.CollectionViewSource)(mWindow.FindResource("MyItemsViewSource")));
            //viewSource.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Descending));

            FilterFill();
        }

#elif BOW
        public static void AddMessages(string MessageString)
        {
            //TODO: Process Messages accordingly 
            RecordSplitter lobjRecordHelper = new RecordSplitter(MessageString);
            int lintMessageIdentifier = 0;
            string lstrMessage = string.Empty;
            //MessageProperties lobjMessageProperties = null;
            StringBuilder lobjMessageBuilder = new StringBuilder();

            lstrMessage = lobjRecordHelper.getField(0, 3);
            if (lobjRecordHelper.numberOfRecords() > 1 && lobjRecordHelper.getField(1, 0) == "O"
                && lobjRecordHelper.numberOfFields(1) >= 7 && lobjRecordHelper.getField(1, 7).Trim().Length > 0
                && IsNumeric(lobjRecordHelper.getField(1, 7)) && Convert.ToInt64(lobjRecordHelper.getField(1, 7)) < 0)
            {
                return;
            }

            MessageBox.Show(MessageString);
            //if (gblnUSEColorReports == true)
            //{
            //    if (lobjRecordHelper.numberOfRecords > 1)
            //    {
            //        if (lobjRecordHelper.getRecord(1).Length > 2)
            //        {
            //            //: GetMessageInfo(MessageIdentifer i.e. O T ,Buy/Sell ie. 1 2)
            //            lobjMessageProperties = GetMessageInfo(lobjRecordHelper.getField(1, 1), lobjRecordHelper.getField(1, 2));
            //            if (Strings.UCase(lstrMessage).IndexOf("CANCEL") != -1)
            //            {
            //                lobjMessageProperties.BuySellColor = Color.Black;
            //            }
            //        }
            //        //:if message doesnot conatins Buy/Sell indicators
            //    }
            //    else
            //    {
            //        if (Strings.UCase(lstrMessage).IndexOf("ORDER") > -1 && (Strings.UCase(lstrMessage).IndexOf("BUY") > -1 || Strings.UCase(lstrMessage).IndexOf("BOUGHT") > -1))
            //        {
            //            lobjMessageProperties = GetMessageInfo("O", 1);
            //        }
            //        else if (Strings.UCase(lstrMessage).IndexOf("ORDER") > -1 && (Strings.UCase(lstrMessage).IndexOf("SELL") > -1 || Strings.UCase(lstrMessage).IndexOf("SOLD") > -1))
            //        {
            //            lobjMessageProperties = GetMessageInfo("O", 2);
            //        }
            //        else if (Strings.UCase(lstrMessage).IndexOf("TRADE") > -1 && (Strings.UCase(lstrMessage).IndexOf("BUY") > -1 || Strings.UCase(lstrMessage).IndexOf("BOUGHT") > -1))
            //        {
            //            lobjMessageProperties = GetMessageInfo("T", 1);
            //        }
            //        else if (Strings.UCase(lstrMessage).IndexOf("TRADE") > -1 && (Strings.UCase(lstrMessage).IndexOf("SELL") > -1 || Strings.UCase(lstrMessage).IndexOf("SOLD") > -1))
            //        {
            //            lobjMessageProperties = GetMessageInfo("T", 2);
            //        }
            //    }
            //}
        }

        public static bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }
#endif

#if TWS
        internal static void ProcessCMWData(CommonMessagingWindowModel cmodel)
        {
            try
            {
                switch (cmodel.MessageType)
                {
                    case 1131:
                        ELogonReply oLogonReplyy = new ELogonReply();
                        oLogonReplyy = cmodel.Data as ELogonReply;
                        cmodel.Category = "NA";
                        cmodel.Message = oLogonReplyy?.ReplyMsg;
                        Thread.Sleep(50);
                        cmodel.Time = CommonFunctions.GetDate().ToLongTimeString();
                        cmodel.ColorChange = "Black";
                        cmodel.CollatedCat = "Reply";
                        cmodel.mType = cmodel.mType;
                        cmodel.MessageTime = DateTime.Now;//CommonFunctions.GetDate();
                        CollectionCMW.Add(cmodel);
                        //WriteToCsv(CommonFunctions.GetDate().ToLongTimeString(), 101, "Reply", "NA", "BoltPro Log In Successful");
                        break;

                    case 1132:
                        ELogOffReply oLogoffReplyy = new ELogOffReply();
                        oLogoffReplyy = cmodel.Data as ELogOffReply;
                        cmodel.Category = "NA";
                        cmodel.Message = oLogoffReplyy?.ReplyMsg;
                        cmodel.Time = CommonFunctions.GetDate().ToLongTimeString();
                        cmodel.ColorChange = "Black";
                        cmodel.CollatedCat = "Reply";
                        cmodel.mType = cmodel.mType;
                        cmodel.MessageTime = DateTime.Now;//CommonFunctions.GetDate();
                        CollectionCMW.Add(cmodel);
                        //WriteToCsv(CommonFunctions.GetDate().ToLongTimeString(), 102, "Reply", "NA", "Reset Password Successful");
                        break;
                    case 1133:
                    case 1134:
                        EChangePwdReply oChngPwdRply = new EChangePwdReply();
                        oChngPwdRply = cmodel.Data as EChangePwdReply;
                        cmodel.Category = "NA";
                        cmodel.Message = oChngPwdRply?.ReplyMsg;
                        cmodel.Time = CommonFunctions.GetDate().ToLongTimeString();
                        cmodel.ColorChange = "Black";
                        cmodel.CollatedCat = "Reply";
                        cmodel.mType = cmodel.mType;
                        cmodel.MessageTime = DateTime.Now;//CommonFunctions.GetDate();
                        CollectionCMW.Add(cmodel);
                        //WriteToCsv(CommonFunctions.GetDate().ToLongTimeString(), 103, "Reply", "NA", "BoltPro Password Change/ Reset Successful");
                        // MessageBox.Show("BoltPro Password Change Successful");
                        break;
                    case 1135:
                        ETraderPwdResetReply oETraderPwdResetReply = new ETraderPwdResetReply();
                        oETraderPwdResetReply = cmodel.Data as ETraderPwdResetReply;
                        cmodel.Category = "NA";
                        cmodel.Message = oETraderPwdResetReply?.ReplyMsg;
                        cmodel.Time = CommonFunctions.GetDate().ToLongTimeString();
                        cmodel.ColorChange = "Black";
                        cmodel.CollatedCat = "Reply";
                        cmodel.mType = cmodel.mType;
                        cmodel.MessageTime = DateTime.Now;//CommonFunctions.GetDate();
                        CollectionCMW.Add(cmodel);
                        //WriteToCsv(CommonFunctions.GetDate().ToLongTimeString(), 103, "Reply", "NA", "BoltPro Password Change/ Reset Successful");
                        //MessageBox.Show("BoltPro Password Reset Successful");
                        break;
                    //case 50004:
                    //case 1095:
                    //    cmodel.Category = "Qry";
                    //    cmodel.Message = "Trade download started";
                    //    cmodel.Time = CommonFunctions.GetDate().ToLongTimeString();
                    //    cmodel.ColorChange = "Black";
                    //    cmodel.CollatedCat = "NA";
                    //    cmodel.mType = cmodel.mType;
                    //    cmodel.MessageTime = CommonFunctions.GetDate();
                    //    CollectionCMW.Add(cmodel);
                    //    //count = oEndOfDownload.NoofRecords;
                    //    //WriteToCsv(CommonFunctions.GetDate().ToLongTimeString(), 103, "NA", "Qry", "Trade downloaded started");
                    //    //  OnResponseMessageReceived(new CommonMessageWindowModel { Category = "Trd", Message = "Res:Trade Received", Time = System.DateTime.Now.ToLongTimeString() }, msgType);
                    //    break;
                    case 50005://End of download
                        EAdminEndOfDownload oEndOfDownload = new EAdminEndOfDownload();
                        oEndOfDownload = (EAdminEndOfDownload)cmodel.Data;
                        int count = oEndOfDownload.NoOfRecords;
                        if (new[] { 601, 602, 603 }.Any(x => x == oEndOfDownload.MessageTag)) //For time being done for Equity 602 - Derivative and 603 - CUrr
                        {
                            string Segment = string.Empty;
                            if (oEndOfDownload.MessageTag == 601)
                                Segment = "Equity";
                            else if (oEndOfDownload.MessageTag == 602)
                                Segment = "Derivative";
                            else if (oEndOfDownload.MessageTag == 603)
                                Segment = "Currency";
                            cmodel.Category = "Qry";
                            cmodel.Message = "Trade download successful";
                            cmodel.Time = CommonFunctions.GetDate().ToLongTimeString();
                            cmodel.ColorChange = "Black";
                            cmodel.CollatedCat = "Reply";
                            cmodel.mType = cmodel.mType;
                            cmodel.MessageTime = DateTime.Now;//CommonFunctions.GetDate();
                            CollectionCMW.Add(cmodel);

                            cmodel = new CommonMessagingWindowModel();
                            cmodel.Category = "Qry";

                            cmodel.Message = string.Format("Trade Download {0} Records Received for {1}", count.ToString(), Segment);
                            cmodel.Time = CommonFunctions.GetDate().ToLongTimeString();
                            cmodel.ColorChange = "Black";
                            cmodel.CollatedCat = "UMS";
                            cmodel.mType = cmodel.mType;
                            cmodel.MessageTime = DateTime.Now;//CommonFunctions.GetDate();
                            CollectionCMW.Add(cmodel);

                            // WriteToCsv(CommonFunctions.GetDate().ToLongTimeString(), 110, "Reply", "Qry", "Trade download successful");
                            // WriteToCsv(CommonFunctions.GetDate().ToLongTimeString(), 110, "Reply", "UMS", string.Format("Trade Download {0} Records Received for Equity", count.ToString()));
                            //}//
                            //else if (oTradeUMS.OrderCategory == "oddLot")
                            //{
                            //    CMWCollection.Add(new CommonMessageWindowModel { Category = "Qry", Message = string.Format("Trade Download {0} Records Received for Odd Lot", count.ToString()), Time = System.DateTime.Now.ToLongTimeString(), ColorChange = commonColor, CollatedCat = "UMS", mType = msgType });
                            //    WriteToCsv(System.DateTime.Now.ToLongTimeString(), 110, "Reply", "UMS", string.Format("Trade Download {0} Records Received for Odd Lot", count.ToString()));
                        }
                        break;
                    case 201:

                        cmodel.Category = "NA";
                        cmodel.Message = "Order placed Succesfully";
                        cmodel.Time = CommonFunctions.GetDate().ToLongTimeString();
                        cmodel.ColorChange = "Black";
                        cmodel.CollatedCat = "Reply";
                        cmodel.mType = cmodel.mType;
                        cmodel.MessageTime = DateTime.Now;//CommonFunctions.GetDate();
                        CollectionCMW.Add(cmodel);

                        //CollectionCMW.Add(new CommonMessagingWindowModel { Category = "NA", Message = "Order placed Succesfully",
                        //    Time = CommonFunctions.GetDate().ToLongTimeString(), ColorChange = "Black", CollatedCat = "Reply", mType = cmodel.mType,
                        //    MessageTime = CommonFunctions.GetDate() });
                        //WriteToCsv(CommonFunctions.GetDate().ToLongTimeString(), 201, "Reply", "NA", "Order placed Succesfully");
                        // OnResponseMessageReceived(new CommonMessageWindowModel { Category = "Ord", Message = "Order placed Succesfully", Time = System.DateTime.Now.ToLongTimeString() }, msgType);
                        break;
                    case 99:
                        ErrorMessage oErrorMessage = new ErrorMessage();
                        oErrorMessage = cmodel.Data as ErrorMessage;

                        cmodel.Category = "NA";
                        cmodel.Message = oErrorMessage?.ReasonText;
                        cmodel.Time = CommonFunctions.GetDate().ToLongTimeString();
                        cmodel.ColorChange = "Black";
                        cmodel.CollatedCat = "NA";
                        cmodel.mType = cmodel.mType;
                        cmodel.MessageTime = DateTime.Now;//CommonFunctions.GetDate();
                        CollectionCMW.Add(cmodel);

                        //CollectionCMW.Add(new CommonMessagingWindowModel { Category = "NA", Message = oErrorMessage.ReasonText,
                        //    Time = CommonFunctions.GetDate().ToLongTimeString(), ColorChange = "Black", CollatedCat = "NA", mType = cmodel.mType,
                        //    MessageTime = CommonFunctions.GetDate() });
                        //WriteToCsv(CommonFunctions.GetDate().ToLongTimeString(), 99, "NA", "NA", oErrorMessage.ReasonText);
                        break;
                    case (int)Enumerations.OrderTypeDownload.NormalOrders:
                    case (int)Enumerations.OrderTypeDownload.StopLossOrders:
                    case (int)Enumerations.OrderTypeDownload.ReturnOrders:
                    case (int)Enumerations.OrderTypeDownload.ReturnStopLossOrders:

                        switch (cmodel.TypeResponse)
                        {
                            case 1:
                                EOrderNomralReply oEOrderNomralReply = new EOrderNomralReply();
                                oEOrderNomralReply = cmodel.Data as EOrderNomralReply;
                                string Segment = string.Empty;
                                if (oEOrderNomralReply != null && oEOrderNomralReply.ReplyCode == 0)
                                {
                                    cmodel.Category = "Qry";
                                    cmodel.Time = CommonFunctions.GetDate().ToLongTimeString();
                                    cmodel.ColorChange = "Black";
                                    cmodel.CollatedCat = "Reply";
                                    cmodel.mType = cmodel.mType;
                                    cmodel.MessageTime = DateTime.Now;//CommonFunctions.GetDate();

                                    switch (oEOrderNomralReply.MessageTag)
                                    {
                                        case 601:
                                            Segment = Enumerations.Segment.Equity.ToString();
                                            break;
                                        case 602:
                                            Segment = Enumerations.Segment.Derivative.ToString();
                                            break;
                                        case 603:
                                            Segment = Enumerations.Segment.Currency.ToString();
                                            break;
                                    }
                                    switch (cmodel.MessageType)
                                    {
                                        case (int)Enumerations.OrderTypeDownload.NormalOrders:
                                            cmodel.Message = "Orders download successful: " + Segment;
                                            Thread.Sleep(10);
                                            break;
                                        case (int)Enumerations.OrderTypeDownload.StopLossOrders:
                                            cmodel.Message = "Stop Loss Orders download successful: " + Segment;
                                            break;
                                        case (int)Enumerations.OrderTypeDownload.ReturnOrders:
                                            cmodel.Message = "Return Orders download successful: " + Segment;
                                            break;
                                        case (int)Enumerations.OrderTypeDownload.ReturnStopLossOrders:
                                            cmodel.Message = "Return Stop Loss Orders download successful: " + Segment;
                                            break;
                                    }
                                    CollectionCMW.Add(cmodel);


                                }
                                break;

                            case 0:
                                switch (cmodel.MessageType)
                                {
                                    case (int)Enumerations.OrderTypeDownload.NormalOrders:
                                        EOrderNomralUMS oEOrderNomralUMS = cmodel.Data as EOrderNomralUMS;
                                        if (oEOrderNomralUMS != null && oEOrderNomralUMS.lstEOrderNomralUMSGrp != null && oEOrderNomralUMS.lstEOrderNomralUMSGrp.Count > 0)
                                        {
                                            var length = oEOrderNomralUMS.lstEOrderNomralUMSGrp.Count;
                                            for (int i = 0; i < length; i++)
                                            {
                                                EOrderNomralUMSGrp oEOrderNomralUMSGrp = new EOrderNomralUMSGrp();
                                                oEOrderNomralUMSGrp = (EOrderNomralUMSGrp)oEOrderNomralUMS.lstEOrderNomralUMSGrp[i];
                                                if (oEOrderNomralUMSGrp != null)
                                                {
                                                    cmodel = new CommonMessagingWindowModel();
                                                    string BuyorSell = string.Empty;
                                                    cmodel.Category = "Ord";
                                                    cmodel.Time = CommonFunctions.GetDate().ToLongTimeString();
                                                    cmodel.MessageTime = DateTime.Now;
                                                    if (oEOrderNomralUMSGrp.BuyOrSell.Trim().ToUpper() == "B")
                                                    {
                                                        cmodel.ColorChange = "Blue";
                                                        BuyorSell = "+";
                                                    }
                                                    else
                                                    {
                                                        cmodel.ColorChange = "Red";
                                                        BuyorSell = "-";
                                                    }

                                                    cmodel.CollatedCat = "Ord";
                                                    cmodel.CatBackGroundColor = "#ffbf00";
                                                    cmodel.mType = cmodel.mType;
                                                    string Segment_Name = CommonFunctions.GetSegmentID(oEOrderNomralUMSGrp.ScripCode);
                                                    int Decimal_pnt = CommonFunctions.GetDecimal(System.Convert.ToInt32(oEOrderNomralUMSGrp.ScripCode), "BSE", Segment_Name);
                                                    string Symbol = CommonFrontEnd.Common.CommonFunctions.GetScripId(oEOrderNomralUMSGrp.ScripCode, "BSE", Segment_Name).Trim();
                                                    int quantity = oEOrderNomralUMSGrp.PendQty;
                                                    double Rate = oEOrderNomralUMSGrp.Rate / Math.Pow(10, Decimal_pnt);
                                                    string ClientID = oEOrderNomralUMSGrp.ClientId.Trim();
                                                    string OrderID = oEOrderNomralUMSGrp.OrderId.ToString().Trim();
                                                    if (Decimal_pnt == 2)
                                                    {
                                                        cmodel.Message = Symbol + BuyorSell + " " + quantity + " @ " + $"{Rate:0.00}" + " " + ClientID + " " + OrderID + ": OrderDownload";
                                                    }
                                                    else if (Decimal_pnt == 4)
                                                    {
                                                        cmodel.Message = Symbol + BuyorSell + " " + quantity + " @ " + $"{Rate:0.0000}" + " " + ClientID + " " + OrderID + ": OrderDownload";
                                                    }
                                                    else
                                                    {
                                                        cmodel.Message = Symbol + BuyorSell + " " + quantity + " @ " + $"{Rate:0.00}" + " " + ClientID + " " + OrderID + ": OrderDownload";
                                                    }
                                                    CollectionCMW.Add(cmodel);
                                                }
                                            }
                                        }
                                        break;
                                    case (int)Enumerations.OrderTypeDownload.StopLossOrders:

                                        EOrderStopLossUMS oEOrderStopLossUMSGrp = cmodel.Data as EOrderStopLossUMS;
                                        if (oEOrderStopLossUMSGrp != null && oEOrderStopLossUMSGrp.lstEOrderNomralUMSGrp != null && oEOrderStopLossUMSGrp.lstEOrderNomralUMSGrp.Count > 0)
                                        {
                                            var length = oEOrderStopLossUMSGrp.lstEOrderNomralUMSGrp.Count;
                                            for (int i = 0; i < length; i++)
                                            {
                                                EOrderStopLossUMSGrp oEOrderNomralUMSGrp = new EOrderStopLossUMSGrp();
                                                oEOrderNomralUMSGrp = (EOrderStopLossUMSGrp)oEOrderStopLossUMSGrp.lstEOrderNomralUMSGrp[i];
                                                if (oEOrderNomralUMSGrp != null)
                                                {
                                                    cmodel = new CommonMessagingWindowModel();
                                                    string BuyorSell = string.Empty;
                                                    cmodel.Category = "SL";
                                                    cmodel.Time = CommonFunctions.GetDate().ToLongTimeString();
                                                    cmodel.MessageTime = DateTime.Now;
                                                    if (oEOrderNomralUMSGrp.BuyOrSell.Trim().ToUpper() == "B")
                                                    {
                                                        cmodel.ColorChange = "Blue";
                                                        BuyorSell = "+";
                                                    }
                                                    else
                                                    {
                                                        cmodel.ColorChange = "Red";
                                                        BuyorSell = "-";
                                                    }

                                                    cmodel.CollatedCat = "SL";
                                                    cmodel.CatBackGroundColor = "#ffbf00";
                                                    cmodel.mType = cmodel.mType;
                                                    string Segment_Name = CommonFunctions.GetSegmentID(oEOrderNomralUMSGrp.ScripCode);
                                                    int Decimal_pnt = CommonFunctions.GetDecimal(System.Convert.ToInt32(oEOrderNomralUMSGrp.ScripCode), "BSE", Segment_Name);
                                                    string Symbol = CommonFrontEnd.Common.CommonFunctions.GetScripId(oEOrderNomralUMSGrp.ScripCode, "BSE", Segment_Name).Trim();
                                                    int quantity = oEOrderNomralUMSGrp.RevealQty;
                                                    double LimitRate = oEOrderNomralUMSGrp.LmtRate / Math.Pow(10, Decimal_pnt);
                                                    double TrgRate = oEOrderNomralUMSGrp.TrgRate / Math.Pow(10, Decimal_pnt);
                                                    string ClientID = oEOrderNomralUMSGrp.ClientId.Trim();
                                                    string OrderID = oEOrderNomralUMSGrp.SLOrderId.ToString().Trim();

                                                    if (LimitRate == -2077252342)
                                                        LimitRate = 0;

                                                    if (Decimal_pnt == 2)
                                                    {
                                                        cmodel.Message = Symbol + BuyorSell + " " + quantity + " @ " + $"{LimitRate:0.00}" + " Trg @  " + $"{TrgRate:0.00}" + " " + ClientID + " " + OrderID + ": StopLoss Order Download";
                                                    }
                                                    else if (Decimal_pnt == 4)
                                                    {
                                                        cmodel.Message = Symbol + BuyorSell + " " + quantity + " @ " + $"{LimitRate:0.0000}" + " Trg @  " + $"{TrgRate:0.0000}" + " " + ClientID + " " + OrderID + ": StopLoss Order Download";
                                                    }
                                                    else
                                                    {
                                                        cmodel.Message = Symbol + BuyorSell + " " + quantity + " @ " + $"{LimitRate:0.00}" + " Trg @  " + $"{TrgRate:0.00}" + " " + ClientID + " " + OrderID + ": StopLoss Order Download";
                                                    }
                                                    CollectionCMW.Add(cmodel);
                                                }
                                            }
                                        }
                                        break;

                                    case (int)Enumerations.OrderTypeDownload.ReturnOrders:
                                        EOrderRetUMS oEOrderRetUMS = cmodel.Data as EOrderRetUMS;
                                        if (oEOrderRetUMS != null && oEOrderRetUMS.lstEOrderNomralUMSGrp != null && oEOrderRetUMS.lstEOrderNomralUMSGrp.Count > 0)
                                        {
                                            var length = oEOrderRetUMS.lstEOrderNomralUMSGrp.Count;
                                            for (int i = 0; i < length; i++)
                                            {
                                                EOrderNomralUMSGrp oEOrderNomralUMSGrp = new EOrderNomralUMSGrp();
                                                oEOrderNomralUMSGrp = (EOrderNomralUMSGrp)oEOrderRetUMS.lstEOrderNomralUMSGrp[i];
                                                if (oEOrderNomralUMSGrp != null)
                                                {
                                                    cmodel = new CommonMessagingWindowModel();
                                                    string BuyorSell = string.Empty;
                                                    cmodel.Category = "Ret";
                                                    cmodel.Time = CommonFunctions.GetDate().ToLongTimeString();
                                                    cmodel.MessageTime = DateTime.Now;
                                                    if (oEOrderNomralUMSGrp.BuyOrSell.Trim().ToUpper() == "B")
                                                    {
                                                        cmodel.ColorChange = "Blue";
                                                        BuyorSell = "+";
                                                    }
                                                    else
                                                    {
                                                        cmodel.ColorChange = "Red";
                                                        BuyorSell = "-";
                                                    }

                                                    cmodel.CollatedCat = "Ret";
                                                    cmodel.CatBackGroundColor = "#ffbf00";
                                                    cmodel.mType = cmodel.mType;
                                                    string Segment_Name = CommonFunctions.GetSegmentID(oEOrderNomralUMSGrp.ScripCode);
                                                    int Decimal_pnt = CommonFunctions.GetDecimal(System.Convert.ToInt32(oEOrderNomralUMSGrp.ScripCode), "BSE", Segment_Name);
                                                    string Symbol = CommonFrontEnd.Common.CommonFunctions.GetScripId(oEOrderNomralUMSGrp.ScripCode, "BSE", Segment_Name).Trim();
                                                    int quantity = oEOrderNomralUMSGrp.PendQty;
                                                    double Rate = oEOrderNomralUMSGrp.Rate / Math.Pow(10, Decimal_pnt);
                                                    string ClientID = oEOrderNomralUMSGrp.ClientId.Trim();
                                                    string OrderID = oEOrderNomralUMSGrp.OrderId.ToString().Trim();
                                                    if (Decimal_pnt == 2)
                                                    {
                                                        cmodel.Message = Symbol + BuyorSell + " " + quantity + " @ " + $"{Rate:0.00}" + " " + ClientID + " " + OrderID + ": Return Order Download";
                                                    }
                                                    else if (Decimal_pnt == 4)
                                                    {
                                                        cmodel.Message = Symbol + BuyorSell + " " + quantity + " @ " + $"{Rate:0.0000}" + " " + ClientID + " " + OrderID + ": Return Order Download";
                                                    }
                                                    else
                                                    {
                                                        cmodel.Message = Symbol + BuyorSell + " " + quantity + " @ " + $"{Rate:0.00}" + " " + ClientID + " " + OrderID + ": Return Order Download";
                                                    }

                                                    CollectionCMW.Add(cmodel);
                                                }
                                            }
                                        }
                                        break;

                                    case (int)Enumerations.OrderTypeDownload.ReturnStopLossOrders:
                                        EOrderRetStopLossUMS oEOrderRetStopLossUMS = cmodel.Data as EOrderRetStopLossUMS;
                                        if (oEOrderRetStopLossUMS != null && oEOrderRetStopLossUMS.lstEOrderNomralUMSGrp != null && oEOrderRetStopLossUMS.lstEOrderNomralUMSGrp.Count > 0)
                                        {
                                            var length = oEOrderRetStopLossUMS.lstEOrderNomralUMSGrp.Count;
                                            for (int i = 0; i < length; i++)
                                            {
                                                EOrderStopLossUMSGrp oEOrderNomralUMSGrp = new EOrderStopLossUMSGrp();
                                                oEOrderNomralUMSGrp = (EOrderStopLossUMSGrp)oEOrderRetStopLossUMS.lstEOrderNomralUMSGrp[i];
                                                if (oEOrderNomralUMSGrp != null)
                                                {
                                                    cmodel = new CommonMessagingWindowModel();
                                                    string BuyorSell = string.Empty;
                                                    cmodel.Category = "RetSL";
                                                    cmodel.Time = CommonFunctions.GetDate().ToLongTimeString();
                                                    cmodel.MessageTime = DateTime.Now;
                                                    if (oEOrderNomralUMSGrp.BuyOrSell.Trim().ToUpper() == "B")
                                                    {
                                                        cmodel.ColorChange = "Blue";
                                                        BuyorSell = "+";
                                                    }
                                                    else
                                                    {
                                                        cmodel.ColorChange = "Red";
                                                        BuyorSell = "-";
                                                    }

                                                    cmodel.CollatedCat = "RetSL";
                                                    cmodel.CatBackGroundColor = "#ffbf00";
                                                    cmodel.mType = cmodel.mType;
                                                    string Segment_Name = CommonFunctions.GetSegmentID(oEOrderNomralUMSGrp.ScripCode);
                                                    int Decimal_pnt = CommonFunctions.GetDecimal(System.Convert.ToInt32(oEOrderNomralUMSGrp.ScripCode), "BSE", Segment_Name);
                                                    string Symbol = CommonFrontEnd.Common.CommonFunctions.GetScripId(oEOrderNomralUMSGrp.ScripCode, "BSE", Segment_Name).Trim();
                                                    int quantity = oEOrderNomralUMSGrp.RevealQty;
                                                    double LimitRate = oEOrderNomralUMSGrp.LmtRate / Math.Pow(10, Decimal_pnt);
                                                    double TrgRate = oEOrderNomralUMSGrp.TrgRate / Math.Pow(10, Decimal_pnt);
                                                    string ClientID = oEOrderNomralUMSGrp.ClientId.Trim();
                                                    string OrderID = oEOrderNomralUMSGrp.SLOrderId.ToString().Trim();
                                                    if (Decimal_pnt == 2)
                                                    {
                                                        cmodel.Message = Symbol + BuyorSell + " " + quantity + " @ " + $"{LimitRate:0.00}" + " Trg @  " + " " + $"{TrgRate:0.00}" + ClientID + " " + OrderID + ": StopLoss Return Order Download";
                                                    }
                                                    else if (Decimal_pnt == 4)
                                                    {
                                                        cmodel.Message = Symbol + BuyorSell + " " + quantity + " @ " + $"{LimitRate:0.0000}" + " Trg @  " + " " + $"{TrgRate:0.0000}" + ClientID + " " + OrderID + ": StopLoss Return Order Download";
                                                    }
                                                    else
                                                    {
                                                        cmodel.Message = Symbol + BuyorSell + " " + quantity + " @ " + $"{LimitRate:0.00}" + " Trg @  " + " " + $"{TrgRate:0.00}" + ClientID + " " + OrderID + ": StopLoss Return Order Download";
                                                    }


                                                    CollectionCMW.Add(cmodel);
                                                }
                                            }
                                        }
                                        break;
                                }
                                break;
                        }
                        break;


                    case (int)Enumerations.TradeTypeDownload.TradeRequest:
                    case (int)Enumerations.TradeTypeDownload.TradeEnhancementRequest:

                        switch (cmodel.TypeResponse)
                        {
                            case 1:
                                ETraderTradeReply oETraderTradeReply = new ETraderTradeReply();
                                oETraderTradeReply = cmodel.Data as ETraderTradeReply;
                                string Segment_trade = string.Empty;
                                if (oETraderTradeReply != null && oETraderTradeReply.ReplyCode == 0)
                                {
                                    cmodel.Category = "Qry";
                                    cmodel.Time = CommonFunctions.GetDate().ToLongTimeString();
                                    cmodel.ColorChange = "Black";
                                    cmodel.CollatedCat = "Reply";
                                    cmodel.mType = cmodel.mType;
                                    cmodel.MessageTime = DateTime.Now;//CommonFunctions.GetDate();

                                    switch (oETraderTradeReply.MessageTag)
                                    {
                                        case 601:
                                            Segment_trade = Enumerations.Segment.Equity.ToString();
                                            break;
                                        case 602:
                                            Segment_trade = Enumerations.Segment.Derivative.ToString();
                                            Thread.Sleep(10);
                                            break;
                                        case 603:
                                            Segment_trade = Enumerations.Segment.Currency.ToString();
                                            break;
                                    }
                                    switch (cmodel.MessageType)
                                    {
                                        case (int)Enumerations.TradeTypeDownload.TradeRequest:
                                            cmodel.Message = "Trade download successful: " + Segment_trade;
                                            break;
                                        case (int)Enumerations.TradeTypeDownload.TradeEnhancementRequest:
                                            cmodel.Message = "Trade Enhancement download successful: " + Segment_trade;
                                            break;
                                    }
                                    CollectionCMW.Add(cmodel);
                                }
                                break;

                            case 0:
                                switch (cmodel.MessageType)
                                {
                                    case (int)Enumerations.TradeTypeDownload.TradeRequest:
                                        ETraderTradeUMS oETraderTradeUMS = cmodel.Data as ETraderTradeUMS;
                                        if (oETraderTradeUMS != null && oETraderTradeUMS.lstETraderTradeUMSGrp != null && oETraderTradeUMS.lstETraderTradeUMSGrp.Count > 0)
                                        {
                                            var length = oETraderTradeUMS.lstETraderTradeUMSGrp.Count;
                                            for (int i = 0; i < length; i++)
                                            {
                                                TradeUMS objTradeUMS = new TradeUMS();
                                                objTradeUMS.MessageTag = oETraderTradeUMS.MessageTag;
                                                ETraderTradeUMSGrp oEAdminTradeDetailUMSGrp = new ETraderTradeUMSGrp();
                                                oEAdminTradeDetailUMSGrp = (ETraderTradeUMSGrp)oETraderTradeUMS.lstETraderTradeUMSGrp[i];

                                                if (oEAdminTradeDetailUMSGrp != null)
                                                {
                                                    cmodel = new CommonMessagingWindowModel();
                                                    string BuyorSell = string.Empty;
                                                    cmodel.Category = "Trd";
                                                    cmodel.Time = CommonFunctions.GetDate().ToLongTimeString();
                                                    cmodel.MessageTime = DateTime.Now;
                                                    if (oEAdminTradeDetailUMSGrp.BuyOrSell.Trim().ToUpper() == "B")
                                                    {
                                                        cmodel.ColorChange = "Blue";
                                                        BuyorSell = "+";
                                                    }
                                                    else
                                                    {
                                                        cmodel.ColorChange = "Red";
                                                        BuyorSell = "-";
                                                    }

                                                    cmodel.CollatedCat = "Trd";
                                                    cmodel.CatBackGroundColor = "#00e673";
                                                    cmodel.mType = cmodel.mType;
                                                    string Segment_Name = CommonFunctions.GetSegmentID(oEAdminTradeDetailUMSGrp.ScripCode);
                                                    int Decimal_pnt = CommonFunctions.GetDecimal(System.Convert.ToInt32(oEAdminTradeDetailUMSGrp.ScripCode), "BSE", Segment_Name);
                                                    string Symbol = CommonFrontEnd.Common.CommonFunctions.GetScripId(oEAdminTradeDetailUMSGrp.ScripCode, "BSE", Segment_Name).Trim();
                                                    int quantity = oEAdminTradeDetailUMSGrp.Qty;
                                                    double Rate = oEAdminTradeDetailUMSGrp.Rate / Math.Pow(10, Decimal_pnt);
                                                    string ClientID = oEAdminTradeDetailUMSGrp.ClientId.Trim();
                                                    string OrderID = oEAdminTradeDetailUMSGrp.TransactionId.ToString().Trim();
                                                    if (Decimal_pnt == 2)
                                                    {
                                                        cmodel.Message = Symbol + BuyorSell + " " + quantity + " @ " + $"{Rate:0.00}" + " " + ClientID + " " + OrderID + ": TradeDownload";
                                                    }
                                                    else if (Decimal_pnt == 4)
                                                    {
                                                        cmodel.Message = Symbol + BuyorSell + " " + quantity + " @ " + $"{Rate:0.0000}" + " " + ClientID + " " + OrderID + ": TradeDownload";
                                                    }
                                                    else
                                                    {
                                                        cmodel.Message = Symbol + BuyorSell + " " + quantity + " @ " + $"{Rate:0.00}" + " " + ClientID + " " + OrderID + ": TradeDownload";
                                                    }

                                                    CollectionCMW.Add(cmodel);
                                                }

                                            }
                                        }
                                        break;

                                    case (int)Enumerations.TradeTypeDownload.TradeEnhancementRequest:
                                        //TODO: Complete this once Gaurav imptements 1080 message
                                        break;
                                }


                                break;
                        }
                        break;


                    case 1520://End of download
                              //string DownloadedType = string.Empty;

                        //switch (cmodel.RequestedMessage)
                        //{
                        //    case (int)Enumerations.OrderTypeDownload.NormalOrders:
                        //    case (int)Enumerations.OrderTypeDownload.ReturnOrders:
                        //    case (int)Enumerations.OrderTypeDownload.StopLossOrders:
                        //    case (int)Enumerations.OrderTypeDownload.ReturnStopLossOrders:
                        //        DownloadedType = "Order";
                        //        break;
                        //    case (int)Enumerations.TradeTypeDownload.TradeRequest:
                        //    case (int)Enumerations.TradeTypeDownload.TradeEnhancementRequest:
                        //        DownloadedType = "Trade";
                        //        break;
                        //    default:
                        //        DownloadedType = string.Empty;
                        //        break;
                        //}
                        ETraderEndOfDownload oETraderEndOfDownload = new ETraderEndOfDownload();
                        oETraderEndOfDownload = cmodel.Data as ETraderEndOfDownload;
                        oETraderEndOfDownload = (ETraderEndOfDownload)cmodel.Data;
                        int Cnt = oETraderEndOfDownload.NoOfRecords;

                        cmodel = new CommonMessagingWindowModel();

                        if (oETraderEndOfDownload.MessageTag == 601) //For time being done for Equity 602 - Derivative and 603 - CUrr
                            cmodel.Message = string.Format("End of Download {0} Records Received for Equity", Cnt.ToString());

                        else if (oETraderEndOfDownload.MessageTag == 602)
                            cmodel.Message = string.Format("End of Download {0} Records Received for Derivative", Cnt.ToString());

                        else if (oETraderEndOfDownload.MessageTag == 603)
                            cmodel.Message = string.Format("End of Download {0} Records Received for Currency", Cnt.ToString());

                        cmodel.Category = "Qry";
                        cmodel.Time = CommonFunctions.GetDate().ToLongTimeString();
                        cmodel.ColorChange = "Black";
                        cmodel.CollatedCat = "UMS";
                        cmodel.mType = cmodel.mType;
                        cmodel.MessageTime = DateTime.Now;//CommonFunctions.GetDate();
                        CollectionCMW.Add(cmodel);
                        break;


                    //case 1025:
                    //    EOrderReply oEOrderReply = new EOrderReply();
                    //    oEOrderReply = cmodel.Data as EOrderReply;

                    //    cmodel = new CommonMessagingWindowModel();
                    //    string BuyorSell_1025 = string.Empty;
                    //    string message = string.Empty;
                    //    cmodel.Category = "Ord";
                    //    cmodel.Time = CommonFunctions.GetDate().ToLongTimeString();
                    //    cmodel.MessageTime = DateTime.Now;
                    //    if (oEOrderReply.BuyOrSell.Trim().ToUpper() == "B")
                    //    {
                    //        cmodel.ColorChange = "Blue";
                    //        BuyorSell_1025 = "+";
                    //    }
                    //    else
                    //    {
                    //        cmodel.ColorChange = "Red";
                    //        BuyorSell_1025 = "-";
                    //    }

                    //    cmodel.CollatedCat = "Ord";
                    //    cmodel.CatBackGroundColor = "#ffbf00";
                    //    cmodel.mType = cmodel.mType;
                    //    if (oEOrderReply.ReplyCode == 0)
                    //    {
                    //        if (oEOrderReply.AUDCode == Enumerations.Order.Modes.A.ToString())
                    //        {
                    //            message = "Added";
                    //        }
                    //        else if (oEOrderReply.AUDCode == Enumerations.Order.Modes.U.ToString())
                    //        {
                    //            message = "Updated";
                    //        }
                    //        else if (oEOrderReply.AUDCode == Enumerations.Order.Modes.D.ToString())
                    //        {
                    //            message = "Deleted";
                    //        }
                    //    }
                    //    else
                    //    {
                    //        message = oEOrderReply.ReplyTxt;
                    //    }


                    //    OrderModel oTempOrderModel = new OrderModel();

                    //    if (MemoryManager.DummyOrderDictionary.ContainsKey(oEOrderReply.MessageTag))
                    //    {
                    //        MemoryManager.DummyOrderDictionary.TryGetValue(oEOrderReply.MessageTag, out oTempOrderModel);
                    //    }


                    //    string Segment_Name_1025 = CommonFunctions.GetSegmentID(oTempOrderModel.ScripCode);
                    //    int Decimal_pnt_1025 = CommonFunctions.GetDecimal(System.Convert.ToInt32(oTempOrderModel.ScripCode), "BSE", Segment_Name_1025);
                    //    string Symbol_1025 = CommonFrontEnd.Common.CommonFunctions.GetScripId(oTempOrderModel.ScripCode, "BSE", Segment_Name_1025).Trim();
                    //    int quantity_1025 = oEOrderReply.AcceptedQty;
                    //    double Rate_1025 = oTempOrderModel. / Math.Pow(10, Decimal_pnt);
                    //    string ClientID_1025 = oEOrderReply.ClientId;
                    //    string OrderID_1025 = oEOrderReply.TransactionId.ToString().Trim();
                    //    cmodel.Message = Symbol_1025 + BuyorSell_1025 + " " + quantity_1025 + " @ " + Rate_1025 + " " + ClientID_1025 + OrderID_1025 + ": " + message;

                    //    CollectionCMW.Add(cmodel);
                    //    break;
                    default:
                        break;
                }
                if (cmodel.Category != null && !customerFilters.Any(str => str.Item.Contains(cmodel.Category)))
                {
                    customerFilters.Add(new CheckedListItem<string> { Item = cmodel.Category, IsChecked = true });
                }
                //NotifyStaticPropertyChanged(nameof(CollectionCMW));
                //CollectionCMW = new AsyncObservableCollection<CommonMessagingWindowModel>(CollectionCMW.Reverse());

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        private void viewSource_Filter(object sender, FilterEventArgs e)
        {
            try
            {
                CommonMessagingWindowModel cust = (CommonMessagingWindowModel)e.Item;

                int count = customerFilters.Where(w => w.IsChecked).Count(w => w.Item == cust.Category);

                if (count == 0)
                {
                    e.Accepted = false;
                    return;
                }

                e.Accepted = true;
            }
            catch (Exception)
            {
            }
        }


        private void btnCountryFilter_Click_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                viewSource.Filter += viewSource_Filter;
                CommonMessagingWindow mWindow = System.Windows.Application.Current.Windows.OfType<CommonMessagingWindow>().FirstOrDefault();
                mWindow.popCountry.IsOpen = true;
            }
            catch (Exception)
            {

            }

        }

        private void btnSelectAll_Click_Click(object sender, RoutedEventArgs e)
        {
            if (FilterSelectContent == "DeselectAll")
            {
                foreach (CheckedListItem<string> item in customerFilters)
                {
                    item.IsChecked = false;
                }
                FilterSelectContent = "SelectAll";
            }
            else if (FilterSelectContent == "SelectAll")
            {

                foreach (CheckedListItem<string> item in customerFilters)
                {
                    item.IsChecked = true;
                }
                FilterSelectContent = "DeselectAll";
            }
        }

        //private void btnUnselectAll_Click_Click(object sender, RoutedEventArgs e)
        //{
        //    foreach (CheckedListItem<string> item in customerFilters)
        //    {
        //        item.IsChecked = false;
        //    }
        //}

        private void FilterFill()
        {
            try
            {
                viewSource.Filter += viewSource_Filter;
                viewSource.Source = CollectionCMW;
                CommonMessagingWindow mWindow = System.Windows.Application.Current.Windows.OfType<CommonMessagingWindow>().FirstOrDefault();
                mWindow.CMWDataGrid.ItemsSource = viewSource.View;
                mWindow.lstCountries.ItemsSource = customerFilters;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }

        }

        private void btnSearchClick_Click(object e, object p)
        {
            if (SearchBoxVisibility == "Visible")
                SearchBoxVisibility = "Hidden";
            else
                SearchBoxVisibility = "Visible";
        }

        private void OnChangeofSearchBoxTxt()
        {
            try
            {
                ICollectionView Itemlist = viewSource.View;
                var yourCostumFilter = new Predicate<object>(item => ((CommonMessagingWindowModel)item).Message.ToLower().Contains(SearchBoxTxt.Trim().ToLower()));
                viewSource.View.Filter = Itemlist.Filter = yourCostumFilter;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }

        }

        internal static void ProcessOnlineOrders(OrderModel oTempOrderModel, string OrderModesMessage)
        {
            if (oTempOrderModel != null)
            {
                CommonMessagingWindowModel cmodel = new CommonMessagingWindowModel();
                string BuyorSell_1025 = string.Empty;
                string message = string.Empty;
                cmodel.Category = "Ord";
                cmodel.Time = CommonFunctions.GetDate().ToLongTimeString();
                cmodel.MessageTime = DateTime.Now;
                if (oTempOrderModel.BuySellIndicator.Trim().ToUpper() == "B")
                {
                    cmodel.ColorChange = "Blue";
                    BuyorSell_1025 = "+";
                }
                else
                {
                    cmodel.ColorChange = "Red";
                    BuyorSell_1025 = "-";
                }

                cmodel.CollatedCat = "Ord";
                cmodel.CatBackGroundColor = "#ffbf00";
                cmodel.mType = cmodel.mType;


                string Segment_Name_1025 = CommonFunctions.GetSegmentID(oTempOrderModel.ScripCode);
                int Decimal_pnt_1025 = CommonFunctions.GetDecimal(System.Convert.ToInt32(oTempOrderModel.ScripCode), "BSE", Segment_Name_1025);
                string Symbol_1025 = CommonFrontEnd.Common.CommonFunctions.GetScripId(oTempOrderModel.ScripCode, "BSE", Segment_Name_1025).Trim();
                int quantity_1025 = oTempOrderModel.OriginalQty;
                double Rate_1025 = oTempOrderModel.Price / Math.Pow(10, Decimal_pnt_1025);
                string ClientID_1025 = oTempOrderModel.ClientId?.Trim();
                string OrderID_1025 = oTempOrderModel.OrderNumber?.ToString().Trim();
                if (Decimal_pnt_1025 == 2)
                {
                    cmodel.Message = Symbol_1025 + BuyorSell_1025 + " " + quantity_1025 + " @ " + $"{Rate_1025:0.00}" + " " + ClientID_1025 + " " + OrderID_1025 + ": " + OrderModesMessage;
                }
                else if (Decimal_pnt_1025 == 4)
                {
                    cmodel.Message = Symbol_1025 + BuyorSell_1025 + " " + quantity_1025 + " @ " + $"{Rate_1025:0.0000}" + " " + ClientID_1025 + " " + OrderID_1025 + ": " + OrderModesMessage;
                }
                else
                {
                    cmodel.Message = Symbol_1025 + BuyorSell_1025 + " " + quantity_1025 + " @ " + $"{Rate_1025:0.00}" + " " + ClientID_1025 + " " + OrderID_1025 + ": " + OrderModesMessage;
                }

                CollectionCMW.Add(cmodel);

                if (cmodel.Category != null && !customerFilters.Any(str => str.Item.Contains(cmodel.Category)))
                {
                    customerFilters.Add(new CheckedListItem<string> { Item = cmodel.Category, IsChecked = true });
                }
            }
        }

        internal static void ProcessOnlineTrades(TradeUMS oTradeUMS)
        {
            if (oTradeUMS != null)
            {
                CommonMessagingWindowModel cmodel = new CommonMessagingWindowModel();
                string BuyorSell_1025 = string.Empty;
                string message = string.Empty;
                cmodel.Category = "Trd";
                cmodel.Time = CommonFunctions.GetDate().ToLongTimeString();
                cmodel.MessageTime = DateTime.Now;
                if (oTradeUMS.BSFlag.Trim().ToUpper() == "B")
                {
                    cmodel.ColorChange = "Blue";
                    BuyorSell_1025 = "+";
                }
                else
                {
                    cmodel.ColorChange = "Red";
                    BuyorSell_1025 = "-";
                }

                cmodel.CollatedCat = "Trd";
                cmodel.CatBackGroundColor = "#00e673";
                cmodel.mType = cmodel.mType;


                string Segment_Name = CommonFunctions.GetSegmentID(oTradeUMS.ScripCode);
                int Decimal_pnt = CommonFunctions.GetDecimal(System.Convert.ToInt32(oTradeUMS.ScripCode), "BSE", Segment_Name);
                string Symbol = CommonFrontEnd.Common.CommonFunctions.GetScripId(oTradeUMS.ScripCode, "BSE", Segment_Name).Trim();
                int quantity = oTradeUMS.LastQty;
                double Rate = oTradeUMS.Rate / Math.Pow(10, Decimal_pnt);
                string ClientID = oTradeUMS.FreeText1.Trim();
                string TransactionID = oTradeUMS.TransactionId.ToString().Trim();
                if (Decimal_pnt == 2)
                {
                    cmodel.Message = Symbol + BuyorSell_1025 + " " + quantity + " @ " + $"{Rate:0.00}" + " " + ClientID + " " + TransactionID + ": Traded";
                }
                else if (Decimal_pnt == 4)
                {
                    cmodel.Message = Symbol + BuyorSell_1025 + " " + quantity + " @ " + $"{Rate:0.0000}" + " " + ClientID + " " + TransactionID + ": Traded";
                }
                else
                {
                    cmodel.Message = Symbol + BuyorSell_1025 + " " + quantity + " @ " + $"{Rate:0.00}" + " " + ClientID + " " + TransactionID + ": Traded";
                }


                CollectionCMW.Add(cmodel);

                if (cmodel.Category != null && !customerFilters.Any(str => str.Item.Contains(cmodel.Category)))
                {
                    customerFilters.Add(new CheckedListItem<string> { Item = cmodel.Category, IsChecked = true });
                }
            }
        }


        internal static void ProcessMiscellaneousMessages(string message, string Category)
        {
            CommonMessagingWindowModel cmodel = new CommonMessagingWindowModel();
            cmodel.Message = message;
            cmodel.Category = Category;
            cmodel.CollatedCat = Category;

            if (Category == "Ord")
                cmodel.CatBackGroundColor = "#ffbf00";

            cmodel.Time = CommonFunctions.GetDate().ToLongTimeString();
            cmodel.ColorChange = "Black";
            //cmodel.mType = cmodel.mType;
            cmodel.MessageTime = DateTime.Now;
            CollectionCMW.Add(cmodel);

            if (cmodel.Category != null && !customerFilters.Any(str => str.Item.Contains(cmodel.Category)))
            {
                customerFilters.Add(new CheckedListItem<string> { Item = cmodel.Category, IsChecked = true });
            }
        }

#endif
    }

    public class AsyncObservableCollection<T> : ObservableCollection<T>
    {
        private AsyncOperation asyncOp = null;

        public AsyncObservableCollection()
        {
            CreateAsyncOp();
        }

        public AsyncObservableCollection(IEnumerable<T> list)
            : base(list)
        {
            CreateAsyncOp();
        }

        private void CreateAsyncOp()
        {
            // Create the AsyncOperation to post events on the creator thread
            asyncOp = AsyncOperationManager.CreateOperation(null);
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            // Post the CollectionChanged event on the creator thread
            asyncOp.Post(RaiseCollectionChanged, e);
        }

        private void RaiseCollectionChanged(object param)
        {
            // We are in the creator thread, call the base implementation directly
            base.OnCollectionChanged((NotifyCollectionChangedEventArgs)param);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            // Post the PropertyChanged event on the creator thread
            asyncOp.Post(RaisePropertyChanged, e);
        }

        private void RaisePropertyChanged(object param)
        {
            // We are in the creator thread, call the base implementation directly
            base.OnPropertyChanged((PropertyChangedEventArgs)param);
        }
    }


    public class ObservableStack<T> : Stack<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public ObservableStack()
        {
        }

        public ObservableStack(IEnumerable<T> collection)
        {
            foreach (var item in collection)
                base.Push(item);
        }

        public ObservableStack(List<T> list)
        {
            foreach (var item in list)
                base.Push(item);
        }


        public new virtual void Clear()
        {
            base.Clear();
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public new virtual T Pop()
        {
            var item = base.Pop();
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            return item;
        }

        public new virtual void Push(T item)
        {
            base.Push(item);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }


        public virtual event NotifyCollectionChangedEventHandler CollectionChanged;


        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            this.RaiseCollectionChanged(e);
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            this.RaisePropertyChanged(e);
        }


        protected virtual event PropertyChangedEventHandler PropertyChanged;


        private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (this.CollectionChanged != null)
                this.CollectionChanged(this, e);
        }

        private void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, e);
        }


        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { this.PropertyChanged += value; }
            remove { this.PropertyChanged -= value; }
        }
    }
}
