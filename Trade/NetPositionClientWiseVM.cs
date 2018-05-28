using CommonFrontEnd.Common;
using CommonFrontEnd.Global;
using CommonFrontEnd.Model;
using CommonFrontEnd.Model.Trade;
using CommonFrontEnd.Processor;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.View.Trade;
using CommonFrontEnd.ViewModel.Profiling;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Windows;
//using CommonFrontEnd.Model.Settings;

namespace CommonFrontEnd.ViewModel.Trade
{
#if TWS
    public class NetPositionClientWiseVM : BaseViewModel
    {
        #region Properties
        int SerialNumber = 1;
        int serialNumber = 1;

        DirectoryInfo directory = new DirectoryInfo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"User/NetPositionClientWise.csv")));
        DirectoryInfo directory1 = new DirectoryInfo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"Image")));

        public static System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        public static ObservableCollection<ClientWisePositionModel> NetPositionCWDataCollectionDemo { get; set; }

        private string _LeftPosition = "345";

        public string LeftPosition
        {
            get { return _LeftPosition; }
            set { _LeftPosition = value; NotifyPropertyChanged("LeftPosition"); }
        }

        private string _TopPosition = "200";

        public string TopPosition
        {
            get { return _TopPosition; }
            set { _TopPosition = value; NotifyPropertyChanged("TopPosition"); }
        }

        private string _Width = "449";

        public string Width
        {
            get { return _Width; }
            set { _Width = value; NotifyPropertyChanged("Width"); }
        }


        private string _Height = "358.914";

        public string Height
        {
            get { return _Height; }
            set { _Height = value; NotifyPropertyChanged("Height"); }
        }



        private long _NetRealPL;

        public long NetRealPL
        {
            get { return _NetRealPL = Convert.ToInt64(MemoryManager.NetRealPL / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))); }
            set { _NetRealPL = value; NotifyPropertyChanged("NetRealPL"); }
        }

        private long _NetUnRealPL;

        public long NetUnRealPL
        {
            get { return _NetUnRealPL = Convert.ToInt64(MemoryManager.NetUnRealPL / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))); }
            set { _NetUnRealPL = value; NotifyPropertyChanged("NetUnRealPL"); }
        }

        private long _NetPL;

        public long NetPL
        {
            get { return _NetPL = Convert.ToInt64(MemoryManager.NetPL / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))); }
            set { _NetPL = value; NotifyPropertyChanged("NetPL"); }
        }

        private List<ClientWisePositionModel> _selectEntireRowList;

        public List<ClientWisePositionModel> selectEntireRowList
        {
            get { return _selectEntireRowList; }
            set { _selectEntireRowList = value; NotifyPropertyChanged(nameof(selectEntireRowList)); }
        }

        private ClientWisePositionModel _SelectedItem;
        public ClientWisePositionModel SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; NotifyPropertyChanged("SelectedItem"); }
        }

        private int _RoleWiseMinWidth;

        public int RoleWiseMinWidth
        {
            get { return _RoleWiseMinWidth; }
            set { _RoleWiseMinWidth = value; NotifyPropertyChanged("RoleWiseMinWidth"); }
        }

        private int _RoleWiseMaxWidth;

        public int RoleWiseMaxWidth
        {
            get { return _RoleWiseMaxWidth; }
            set { _RoleWiseMaxWidth = value; NotifyPropertyChanged("RoleWiseMaxWidth"); }
        }

        private string _rowColor;

        public string rowColor
        {
            get { return _rowColor; }
            set { _rowColor = value; NotifyPropertyChanged(nameof(rowColor)); }
        }

        private bool _Ratein4decimalChecked;

        public bool Ratein4decimalChecked
        {
            get
            {
                if (MultiWindowCheckBoxCheckExtension.is4decimalCheckboxCheck)
                {
                    _Ratein4decimalChecked = true;
                }

                return _Ratein4decimalChecked;
            }
            set
            {
                _Ratein4decimalChecked = value;
                MultiWindowCheckBoxCheckExtension.is4decimalCheckboxCheck = value;
                NotifyPropertyChanged("Ratein4decimalChecked");
            }
        }
        #endregion

        #region RelayCommand
        //RefreshPL
        private RelayCommand _RefreshPL;

        public RelayCommand RefreshPL
        {
            get { return _RefreshPL ?? (_RefreshPL = new RelayCommand((object e) => ToggleRefreshPL(e))); }
        }

        private RelayCommand _myLocationChanged;

        public RelayCommand myLocationChanged
        {
            get
            {
                return _myLocationChanged ?? (_myLocationChanged = new RelayCommand(
                    (object e) => Windows_NetPositionClientwiseLocationChanged(e)));

            }
        }
        private void ToggleRefreshPL(object e)
        {
            //var oNetPositionList = MemoryManager.NetPositionCWDemoDict.Values.Cast<NetPosition>().ToList();
            //for (int i = 0; i < oNetPositionList.Count; i++)
            //{
            //    NetPosition oNetPosition = oNetPositionList[i];
            //    var segment = CommonFrontEnd.Common.CommonFunctions.GetSegmentID(oNetPosition.ScripCode);
            //    var QuantityMultiplier = CommonFrontEnd.Common.CommonFunctions.GetQuantityMultiplier(oNetPosition.ScripCode, "BSE", segment);
            //    var DecimalLocator = Common.CommonFunctions.GetDecimal(Convert.ToInt32(oNetPosition.ScripCode), "BSE", segment);
            //    if (oNetPosition.SellQty > 0 && oNetPosition.BuyQty > 0)
            //    {
            //        int tradedQty = 0;

            //        if (oNetPosition.BuyQty <= oNetPosition.SellQty)
            //            tradedQty = oNetPosition.BuyQty;

            //        if (oNetPosition.BuyQty >= oNetPosition.SellQty)
            //            tradedQty = oNetPosition.SellQty;

            //        NetRealPL = MemoryManager.NetRealPL = oNetPosition.RealPL = Convert.ToInt64(((oNetPosition.AvgSellRate - oNetPosition.AvgBuyRate) * tradedQty * QuantityMultiplier));

            //    }

            //    if (BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Count > 0 && BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.ContainsKey(Convert.ToInt32(oNetPosition.ScripCode)))
            //    {
            //        BroadcastReceiver.ScripDetails Br = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == oNetPosition.ScripCode).Select(x => x.Value).FirstOrDefault() == null ? new BroadcastReceiver.ScripDetails() : BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == oNetPosition.ScripCode).Select(x => x.Value).FirstOrDefault();
            //        ScripDetails objScripDetails = new ScripDetails();
            //        objScripDetails = MainWindowVM.UpdateScripDataFromMemory(Br);
            //        objScripDetails.ScriptCode_BseToken_NseToken = Convert.ToInt32(oNetPosition.ScripCode);

            //        string LTPPrice = "0";

            //        if (objScripDetails.NoOfTrades > 0)
            //        {

            //            if (DecimalLocator == 4)
            //            {
            //                LTPPrice = string.Format("{0:0.0000}", CommonFunctions.GetValueInDecimal(objScripDetails.lastTradeRateL, DecimalLocator) == "0" ? "" : CommonFunctions.GetValueInDecimal(objScripDetails.lastTradeRateL, DecimalLocator));
            //            }
            //            else
            //            {
            //                LTPPrice = string.Format("{0:0.00}", CommonFunctions.GetValueInDecimal(objScripDetails.lastTradeRateL, DecimalLocator) == "0" ? "" : CommonFunctions.GetValueInDecimal(objScripDetails.lastTradeRateL, DecimalLocator));
            //            }
            //            //LTPPrice = CommonFunctions.GetValueInDecimal(objScripDetails.lastTradeRateL, DecimalPoint) == "0" ? "" : CommonFunctions.GetValueInDecimal(objScripDetails.lastTradeRateL, DecimalPoint);
            //        }

            //        double LTP = 0d;
            //        LTP = Convert.ToDouble(LTPPrice);

            //        if (oNetPosition.NetQty > 0)
            //        {
            //            var unRealPLBuy = Convert.ToInt64((LTP - oNetPosition.AvgBuyRate) * oNetPosition.NetQty * QuantityMultiplier);
            //            NetUnRealPL = MemoryManager.NetUnRealPL = oNetPosition.UnRealPL = unRealPLBuy;
            //        }
            //        else if (oNetPosition.NetQty < 0)
            //        {
            //            var unRealPLSell = Convert.ToInt64((oNetPosition.AvgSellRate - LTP) * Math.Abs(oNetPosition.NetQty * QuantityMultiplier));
            //            NetUnRealPL = MemoryManager.NetUnRealPL = oNetPosition.UnRealPL = unRealPLSell;
            //        }

            //    }

            //    oNetPosition.NetPL = MemoryManager.NetPL = oNetPosition.RealPL + oNetPosition.UnRealPL;

            //    NetPL = MemoryManager.NetPL;
            //}
            var data = MemoryManager.NetPositionCWDemoDict.ToList();
            foreach (var item in data)
            {
                NetPosition oNetPosition = (NetPosition)item.Value;
                var segment = CommonFrontEnd.Common.CommonFunctions.GetSegmentID(oNetPosition.ScripCode);
                var QuantityMultiplier = CommonFrontEnd.Common.CommonFunctions.GetQuantityMultiplier(oNetPosition.ScripCode, "BSE", segment);
                var DecimalLocator = Common.CommonFunctions.GetDecimal(Convert.ToInt32(oNetPosition.ScripCode), "BSE", segment);
                if (oNetPosition.SellQty > 0 && oNetPosition.BuyQty > 0)
                {
                    int tradedQty = 0;

                    if (oNetPosition.BuyQty <= oNetPosition.SellQty)
                        tradedQty = oNetPosition.BuyQty;

                    if (oNetPosition.BuyQty >= oNetPosition.SellQty)
                        tradedQty = oNetPosition.SellQty;

                    NetRealPL = MemoryManager.NetRealPL = oNetPosition.RealPL = Convert.ToInt64((((oNetPosition.AvgSellRate / Math.Pow(10, 4)) - (oNetPosition.AvgBuyRate / Math.Pow(10, 4))) * tradedQty * QuantityMultiplier));
                    NetRealPL = MemoryManager.NetRealPL = MemoryManager.NetRealPL * 10000;

                }

                if (BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Count > 0 && BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.ContainsKey(Convert.ToInt32(oNetPosition.ScripCode)))
                {
                    string LTPPrice = "0";
                    //BroadcastReceiver.ScripDetails Br = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == oNetPosition.ScripCode).Select(x => x.Value).FirstOrDefault() == null ? new BroadcastReceiver.ScripDetails() : BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == oNetPosition.ScripCode).Select(x => x.Value).FirstOrDefault();
                    //ScripDetails objScripDetails = new ScripDetails();
                    //objScripDetails = MainWindowVM.UpdateScripDataFromMemory(Br);
                    //objScripDetails.ScriptCode_BseToken_NseToken = Convert.ToInt32(oNetPosition.ScripCode);



                    //if (objScripDetails.NoOfTrades > 0)
                    //{

                    //    if (DecimalLocator == 4)
                    //    {
                    //        LTPPrice = string.Format("{0:0.0000}", CommonFunctions.GetValueInDecimal(objScripDetails.lastTradeRateL, DecimalLocator) == "0" ? "" : CommonFunctions.GetValueInDecimal(objScripDetails.lastTradeRateL, DecimalLocator));
                    //    }
                    //    else
                    //    {
                    //        LTPPrice = string.Format("{0:0.00}", CommonFunctions.GetValueInDecimal(objScripDetails.lastTradeRateL, DecimalLocator) == "0" ? "" : CommonFunctions.GetValueInDecimal(objScripDetails.lastTradeRateL, DecimalLocator));
                    //    }
                    //    //LTPPrice = CommonFunctions.GetValueInDecimal(objScripDetails.lastTradeRateL, DecimalPoint) == "0" ? "" : CommonFunctions.GetValueInDecimal(objScripDetails.lastTradeRateL, DecimalPoint);
                    //}
                    LTPPrice = CommonFunctions.GetLTPBCast(Convert.ToInt32(oNetPosition.ScripCode));
                    double LTP = 0d;
                    LTP = Convert.ToDouble(LTPPrice);

                    if (oNetPosition.NetQty > 0)
                    {
                        var unRealPLBuy = Convert.ToInt64((LTP - (oNetPosition.AvgBuyRate / Math.Pow(10, 4))) * oNetPosition.NetQty * QuantityMultiplier);
                        MemoryManager.NetUnRealPL = oNetPosition.UnRealPL = unRealPLBuy;
                        NetUnRealPL = MemoryManager.NetUnRealPL = MemoryManager.NetUnRealPL * 10000;
                    }
                    else if (oNetPosition.NetQty < 0)
                    {
                        var unRealPLSell = Convert.ToInt64(((oNetPosition.AvgSellRate / Math.Pow(10, 4)) - LTP) * Math.Abs(oNetPosition.NetQty * QuantityMultiplier));
                        NetUnRealPL = MemoryManager.NetUnRealPL = oNetPosition.UnRealPL = unRealPLSell;
                        NetUnRealPL = MemoryManager.NetUnRealPL = MemoryManager.NetUnRealPL * 10000;
                    }

                }

                oNetPosition.NetPL = MemoryManager.NetPL = (oNetPosition.RealPL + oNetPosition.UnRealPL) * 10000;

                NetPL = MemoryManager.NetPL;
                //UMSProcessor.ProcessNetPositionCWDemo(item.Value);
            }

        }
        private void Windows_NetPositionClientwiseLocationChanged(object e)
        {
            if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = (BoltAppSettingsWindowsPosition)CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISE != null && oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISE.WNDPOSITION != null)
                {
                    oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISE.WNDPOSITION.Left = Convert.ToInt32(LeftPosition);
                    oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISE.WNDPOSITION.Top = Convert.ToInt32(TopPosition);
                    oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISE.WNDPOSITION.Right = Convert.ToInt32(Width);
                    oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISE.WNDPOSITION.Down = Convert.ToInt32(Height);
                }
                //CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(@"D:\TWS_DotNetNewStructure\TWS_DOTNETT\CommonFrontEnd\bin\Debug\xml\Users\AppSettings\920200000.xml", "WindowsPosition");
                CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(CommonFrontEnd.SharedMemories.SettingsManager.AppSettingsXmlPath, "WindowsPosition");
            }

        }


        private RelayCommand _DataGridDoubleClick;

        public RelayCommand DataGridDoubleClick
        {
            get
            {
                return _DataGridDoubleClick ?? (_DataGridDoubleClick = new RelayCommand(
                    (object e) => DataGrid_DoubleClick()
                        ));
            }
        }


        private RelayCommand _ExportExcel;

        public RelayCommand ExportExcel
        {
            get
            {
                return _ExportExcel ?? (_ExportExcel = new RelayCommand((object e) => ExecuteMyCommand(NetPositionCWDataCollectionDemo)));
            }
        }

        private RelayCommand _NPCWWindowClosing;

        public RelayCommand NPCWWindowClosing
        {
            get { return _NPCWWindowClosing ?? (_NPCWWindowClosing = new RelayCommand((object e) => ClientWindow_Closing(e))); }
        }

        private RelayCommand _ShortCut_Escape;

        public RelayCommand ShortCut_Escape
        {
            get
            {
                return _ShortCut_Escape ?? (_ShortCut_Escape = new RelayCommand(
                    (object e) => ClientWindow_Closing(e)
                        ));
            }
        }


        private RelayCommand _SaveImage;

        public RelayCommand SaveImage
        {
            get
            {
                return _SaveImage ?? (_SaveImage = new RelayCommand(
                    (object e) => SaveImageClick()
                        ));
            }
        }

        private RelayCommand _EmailClick;

        public RelayCommand EmailClick
        {
            get
            {
                return _EmailClick ?? (_EmailClick = new RelayCommand(
                    (object e) => EmailClick_Click()
                        ));
            }
        }

        private RelayCommand _btnSquareOffSave;

        public RelayCommand btnSquareOffSave
        {
            get { return _btnSquareOffSave ?? (_btnSquareOffSave = new RelayCommand((object e) => btnSquareOffSaveBatch(e))); }
        }



        public static string SMTPIP { get; private set; }
        public static string LiveSettings { get; private set; }
        public static bool IsPing { get; private set; }
        public static string GmailEmailID { get; private set; }
        public static string GmailPassword { get; private set; }
        public static string LiveFromEmail { get; private set; }
        public static string LiveSMTPIP { get; private set; }
        public static string LiveSMTPPort { get; private set; }
        public static string LiveSMTPPassword { get; private set; }
        public static string LiveSMTPEmailID { get; private set; }

        public int PortStatus = 2;


        #endregion



        #region Event

        public delegate void ClientDoubleClickEventHandler(string ClientID);
        public static event ClientDoubleClickEventHandler onClientDoubleClickEventHandler;

        #endregion

        NetPositionClientWiseDetails oNetPositionClientWiseDetails;
        //public static bool Load_NetpositionCWSWEntry = false;

        #region SaveImageandEmail

        private void SaveImageClick()
        {
            try
            {
                if (SelectedItem != null)
                {
                    //var csv_file_path = (Array)null;

                    //csv_file_path = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Sub Systems" + "//NPFilesForImageMail", "*.csv"); ////while released

                    //foreach (string s in csv_file_path)
                    //{
                    ConvertCSVtoDataTableForWhatApp(null, 0); //for net position
                }
                else
                {
                    MessageBox.Show("Select Row to Save Image", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                //}

            }

            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        public void ConvertCSVtoDataTableForWhatApp(string strFilePath, int flag)
        {
            SerialNumber = 1;
            serialNumber = 1;
            string ClientID, SettlementNumber = "";
            Boolean bln_validfile = true;
            StringBuilder SBColumn0;
            StringBuilder SBColumn1;
            StringBuilder SBColumn2;
            StringBuilder SBColumn3;
            StringBuilder SBColumn4;

            int PageNo;
            int TotalPages = 1;

            DateTime serverTime = DateTime.Now;
            DataTable dt = new DataTable();
            #region NEw 
            string[] headers = { "Client", "Scrip Id", "Net P / L", "BQty", "BavRate", "SQty", "SAvRate", "NetQty", "NetValue", "ISIN No" };
            foreach (string header in headers)
            {
                dt.Columns.Add(header);
            }

            CWSWDetailPositionModel objCWSWDetailPositionModel = new CWSWDetailPositionModel();
            objCWSWDetailPositionModel = NetPositionMemory.NetPositionCWSWDataCollection.ToList().Where(x => x.ClientID == SelectedItem.ClientId).FirstOrDefault();
            int BuyQty = 0;
            double BavgRate = 0;
            int Sqty = 0;
            double SAvgRate = 0;
            int NetQty = 0;
            double NetValue = 0;
            string ISINNo = string.Empty;
            string settlementnumber = string.Empty;

            if (objCWSWDetailPositionModel != null)
            {
                bln_validfile = true;
                BuyQty = objCWSWDetailPositionModel.BuyQty;
                BavgRate = Convert.ToDouble(objCWSWDetailPositionModel.AvgBuyRateString);
                Sqty = objCWSWDetailPositionModel.SellQty;
                SAvgRate = Convert.ToDouble(objCWSWDetailPositionModel.AvgSellRateString);
                NetQty = objCWSWDetailPositionModel.NetQty;
                NetValue = objCWSWDetailPositionModel.NetValue;

                ISINNo = objCWSWDetailPositionModel.ISINNum;
                settlementnumber = UtilityLoginDetails.GETInstance.SettlementNo;
            }
            else
            {
                bln_validfile = false;
            }

            string[] rows1 = { SelectedItem.ClientId, CommonFunctions.GetScripId(objCWSWDetailPositionModel.ScripCode, "BSE", CommonFunctions.GetSegmentID(objCWSWDetailPositionModel.ScripCode)), objCWSWDetailPositionModel.NetPL.ToString(), BuyQty.ToString(), BavgRate.ToString(), Sqty.ToString(), SAvgRate.ToString(), NetQty.ToString(), NetValue.ToString(), ISINNo };
            DataRow dr = dt.NewRow();

            string[] rows = { "", "", "", "", "", "", "", settlementnumber, GmailEmailID, DateTime.Now.ToString("ddMMyy") };

            for (int i = 0; i < headers.Length; i++)
            {
                dr[i] = rows[i];
            }
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            for (int i = 0; i < headers.Length; i++)
            {
                dr[i] = rows1[i];
            }

            dt.Rows.Add(dr);

            #endregion


            //#region OLd
            //using (StreamReader sr = new StreamReader(strFilePath))
            //{
            //    string line;
            //    while ((line = sr.ReadLine()) != null)
            //    {
            //        string[] headers = line.Split(',');
            //        if ((headers.Length == 10 && flag == 0) || (headers.Length == 11 && flag == 1))  // Net Pos - 0 and Ret orders - 1
            //        {
            //            bln_validfile = true;
            //            foreach (string header in headers)
            //            { dt.Columns.Add(header); }

            //            while (!sr.EndOfStream)
            //            {
            //                string[] rows = sr.ReadLine().Split(',');
            //                DataRow dr = dt.NewRow();
            //                for (int i = 0; i < headers.Length; i++)
            //                {
            //                    dr[i] = rows[i];
            //                }
            //                dt.Rows.Add(dr);
            //            }
            //        }
            //        else
            //        { bln_validfile = false; }
            //        break;
            //    }

            //    if (line == null)
            //    { bln_validfile = false; }
            //}
            //#endregion
            if (bln_validfile)
            {
                ClientID = dt.Rows[1].ItemArray[0].ToString();
                if (flag == 0) //Net Position
                {
                    SettlementNumber = dt.Rows[0].ItemArray[7].ToString();
                    if (SettlementNumber == "NA")
                    {
                        SettlementNumber = "";
                    }
                }
                try
                {
                    int NoOfPages = dt.Rows.Count / 20;
                    int remainder = dt.Rows.Count / 20;
                    dt.Rows[0].Delete();
                    if (NoOfPages >= 1 && remainder != 0)
                    {



                        DataTable[] splittedtables = dt.AsEnumerable()
                                                       .Select((row, index) => new { row, index })
                                                       .GroupBy(x => x.index / 20)  // integer division, the fractional part is truncated
                                                       .Select(g => g.Select(x => x.row).CopyToDataTable())
                                                       .ToArray();
                        TotalPages = splittedtables.Length;
                        for (int i = 0; i < splittedtables.Length; i++)
                        {
                            SBColumn0 = new StringBuilder();
                            SBColumn1 = new StringBuilder();
                            SBColumn2 = new StringBuilder();
                            SBColumn3 = new StringBuilder();
                            SBColumn4 = new StringBuilder();
                            PageNo = i + 1;
                            int count = splittedtables[i].Rows.Count;

                            if (flag == 0)
                            {
                                NetPositionForImageCreation(splittedtables[i], ClientID, SBColumn0, SBColumn1, SBColumn2, SBColumn3, SBColumn4, PageNo);
                            }
                            else
                            {
                                ReturnOrderForImageCreation(splittedtables[i], ClientID, SBColumn0, SBColumn1, SBColumn2, SBColumn3, SBColumn4, PageNo);
                            }

                            CreatingAnImageFromText(count, flag, ClientID, SettlementNumber, SBColumn0, SBColumn1, SBColumn2, SBColumn3, SBColumn4, serverTime, PageNo, TotalPages);

                            File.Delete(strFilePath);
                        }
                    }
                    else
                    {
                        SBColumn0 = new StringBuilder();
                        SBColumn1 = new StringBuilder();
                        SBColumn2 = new StringBuilder();
                        SBColumn3 = new StringBuilder();
                        SBColumn4 = new StringBuilder();
                        PageNo = 1;
                        if (flag == 0)
                        {
                            NetPositionForImageCreation(dt, ClientID, SBColumn0, SBColumn1, SBColumn2, SBColumn3, SBColumn4, PageNo);
                        }
                        else
                        {
                            ReturnOrderForImageCreation(dt, ClientID, SBColumn0, SBColumn1, SBColumn2, SBColumn3, SBColumn4, PageNo);
                        }


                        CreatingAnImageFromText(dt.Rows.Count, flag, ClientID, SettlementNumber, SBColumn0, SBColumn1, SBColumn2, SBColumn3, SBColumn4, serverTime, PageNo, TotalPages);

                    }
                    string MessageInCMW;
                    if (flag == 0)
                    {
                        MessageInCMW = "Net Position";
                    }
                    else
                    {
                        MessageInCMW = "Return Order";
                    }
                    //CmwListDetails cmwErrorSuccess = new CmwListDetails
                    //{
                    //    BoltTime = DateTime.Now.ToString("HH:mm:ss"),
                    //    MsgCategory = Constants.MsgCat.Other,
                    //    MsgPrefix = Constants.MsgPrefix.Other,
                    //    MsgDetail = MessageInCMW + " Image created successfully at '..\\Bolt\\Bolt\\Image' for client:" + ClientID
                    //};
                    //AddRecordGrid(cmwErrorSuccess, 0);
                    // File.Delete(strFilePath);
                }
                catch (Exception ex)
                {
                    // File.Delete(strFilePath); //after sending the mail, delete the file from folder.  
                }

            }

        }

        private void NetPositionForImageCreation(DataTable dt, string ClientID, StringBuilder SBColumn0,
                                               StringBuilder SBColumn1, StringBuilder SBColumn2, StringBuilder SBColumn3, StringBuilder SBColumn4, int PageNo)
        {
            SBColumn0.Append("Sn" + Environment.NewLine);
            SBColumn1.Append("Scrip Id--ISIN" + Environment.NewLine);
            SBColumn2.Append("Net Qty/Lot" + Environment.NewLine);
            SBColumn3.Append("Net Price" + Environment.NewLine);

            foreach (DataRow dr in dt.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (dr["NetQty"].ToString() == "")
                    {
                        dr["NetQty"] = "0";
                    }
                    SBColumn0.Append(serialNumber + Environment.NewLine);
                    SBColumn1.Append(dr["Scrip Id"].ToString());
                    SBColumn2.Append(dr["NetQty"].ToString() + Environment.NewLine);
                    if (dr["NetQty"].ToString() != "0")
                    {
                        if (dr["NetQty"].ToString().Contains("-"))
                        {
                            SBColumn3.Append(dr["SavRate"].ToString() + Environment.NewLine);
                        }
                        else
                        {
                            SBColumn3.Append(dr["BavRate"].ToString() + Environment.NewLine);
                        }
                    }
                    else
                    {
                        SBColumn3.Append("--" + Environment.NewLine);
                    }
                    if (dr["ISIN No"].ToString().Trim() != "")
                    {
                        SBColumn1.Append("--" + dr["ISIN No"].ToString().Trim() + Environment.NewLine);
                    }
                    else
                    {
                        SBColumn1.Append(Environment.NewLine);
                    }

                    serialNumber++;
                }
            }
            //for (int i = 0; i < 2; i++)
            //{
            //    SBColumn0.Append(serialNumber + Environment.NewLine);
            //    SBColumn1.Append("USDINR16JUNE63.50000CE" + Environment.NewLine);
            //    SBColumn2.Append("8888888888" + Environment.NewLine);

            //    SBColumn3.Append("88888888888" + Environment.NewLine);
            //    serialNumber++;
            //}
            //for (int i = 0; i < 2; i++)
            //{
            //    SBColumn0.Append(serialNumber + Environment.NewLine);
            //    SBColumn1.Append("USDINR16JUNE63.50000CE000CE" + Environment.NewLine);
            //    SBColumn2.Append("888" + Environment.NewLine);

            //    SBColumn3.Append("88888888888" + Environment.NewLine);
            //    serialNumber++;
            //}
        }

        private void ReturnOrderForImageCreation(DataTable dt, string ClientID, StringBuilder SBColumn0,
            StringBuilder SBColumn1, StringBuilder SBColumn2, StringBuilder SBColumn3, StringBuilder SBColumn4, int PageNo)
        {
            SBColumn0.Append("Sn" + Environment.NewLine);
            SBColumn1.Append("Scrip Id" + Environment.NewLine);
            SBColumn2.Append("B/S" + Environment.NewLine);
            SBColumn3.Append("Qty" + Environment.NewLine);
            SBColumn4.Append("Rate" + Environment.NewLine);


            foreach (DataRow dr in dt.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    SBColumn0.Append(SerialNumber + Environment.NewLine);
                    SBColumn1.Append(dr["Scrip Id"].ToString() + Environment.NewLine);
                    SBColumn2.Append(dr["B/S"].ToString() + Environment.NewLine);
                    if (dr["Total Qty"].ToString() == "")
                    {
                        dr["Total Qty"] = "0";
                    }
                    SBColumn3.Append(dr["Total Qty"].ToString() + Environment.NewLine);
                    if (dr["Total Qty"] != "0")
                    {
                        SBColumn4.Append(dr["Rate"].ToString() + Environment.NewLine);
                    }
                    else
                    {
                        SBColumn4.Append("--" + Environment.NewLine);
                    }

                    SerialNumber++;
                }
            }
            //for (int i = 0; i < 2; i++)
            //{
            //    SBColumn0.Append(serialNumber + Environment.NewLine);
            //    SBColumn1.Append("USDINR16JUNE63.50000CE" + Environment.NewLine);
            //    SBColumn2.Append("B" + Environment.NewLine);

            //    SBColumn3.Append("88888888888" + Environment.NewLine);
            //    SBColumn4.Append("88888888888" + Environment.NewLine);
            //    SerialNumber++;
            //}

        }

        private void CreatingAnImageFromText(int Count, int flag, string ClientID, string SettlementNumber, StringBuilder SBColumn0, StringBuilder SBColumn1,
    StringBuilder SBColumn2, StringBuilder SBColumn3, StringBuilder SBColumn4, DateTime serverTime, int PageNo, int TotalPages)
        {
            int y1 = 50;
            for (int i = 0; i < Count; i++)
            {
                y1 = y1 + 18;
            }
            int FinalHeight = y1 + 18;
            //12-font
            //Bitmap bitmap = new Bitmap(630, FinalHeight);
            Bitmap bitmap = new Bitmap(430, FinalHeight);
            // Bitmap bitmap = new Bitmap(600, 300);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                Font font = new Font("Arial", 11, System.Drawing.FontStyle.Bold);
                graphics.FillRectangle(new SolidBrush(Color.LightSkyBlue), 0, 0, bitmap.Width, bitmap.Height);
                graphics.FillRectangle(new SolidBrush(Color.LightBlue), 0, 30, bitmap.Width, bitmap.Height);
                int y2 = 46;

                for (int i = 0; i < Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        graphics.FillRectangle(new SolidBrush(Color.MintCream), 0, y2, bitmap.Width, bitmap.Height);
                    }
                    else
                    {
                        graphics.FillRectangle(new SolidBrush(Color.LightGray), 0, y2, bitmap.Width, bitmap.Height);
                    }
                    y2 = y2 + 17;
                }
                graphics.FillRectangle(new SolidBrush(Color.LightSkyBlue), 0, y2, bitmap.Width, bitmap.Height);

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Far;
                StringFormat stringFormat1 = new StringFormat();
                stringFormat1.Alignment = StringAlignment.Center;
                Font font1 = new Font("Arial", 15, System.Drawing.FontStyle.Bold);
                if (flag == 0)
                {
                    graphics.DrawString("                   NET POSITION for " + ClientID, font1, new SolidBrush(Color.Black), 0, 0);
                }
                else
                {
                    graphics.DrawString("       RETURN ORDER for " + ClientID + " (End Of Day)", font1, new SolidBrush(Color.Black), 0, 0);
                }
                graphics.DrawString(SBColumn0.ToString(), font, new SolidBrush(Color.Black), 0, 30);
                graphics.DrawString(SBColumn1.ToString(), font, new SolidBrush(Color.Black), 27, 30);
                if (flag == 0)
                {
                    graphics.DrawString(SBColumn2.ToString(), font, new SolidBrush(Color.Black), 329, 30, stringFormat);
                    graphics.DrawString(SBColumn3.ToString(), font, new SolidBrush(Color.Black), 430, 30, stringFormat);
                }
                else
                {
                    graphics.DrawString(SBColumn2.ToString(), font, new SolidBrush(Color.Black), 220, 30);
                    graphics.DrawString(SBColumn3.ToString(), font, new SolidBrush(Color.Black), 329, 30, stringFormat);
                    graphics.DrawString(SBColumn4.ToString(), font, new SolidBrush(Color.Black), 430, 30, stringFormat);
                }

                if (flag == 0)
                {
                    graphics.DrawString("Eq.Set: " + SettlementNumber + "   Date: " + serverTime + "   Image:" + PageNo + "/" + TotalPages, font, new SolidBrush(Color.Black), 0, y1);
                }
                else
                {
                    graphics.DrawString("Date: " + serverTime + "                           Image:" + PageNo + "/" + TotalPages, font, new SolidBrush(Color.Black), 0, y1);
                }
                graphics.Flush();
                font.Dispose();
                graphics.Dispose();
            }


            bool FileCreated = System.IO.Directory.Exists(Directory.GetCurrentDirectory() + "\\Image\\" + DateTime.Now.ToString("ddMMyyyy"));
            //  bool FileCreated = System.IO.Directory.Exists("C:\\BitmapImages");
            string NPorRO;
            if (flag == 0)
            {
                NPorRO = "_NP_";
            }
            else
            {
                NPorRO = "_RO_";
            }
            if (!FileCreated)
            {
                System.IO.Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Image\\" + DateTime.Now.ToString("ddMMyyyy"));
                //Array file = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Image\\" + DateTime.Now.ToString("ddMMyyyy"));
                //foreach (string s in file)
                //{
                //    if(s.Contains(ClientID + NPorRO+1))
                //    {
                //    File.Delete(s);
                //    }
                //}
                bitmap.Save(Directory.GetCurrentDirectory() + "\\Image\\" + DateTime.Now.ToString("ddMMyyyy") + "\\" + ClientID + NPorRO + PageNo + ".Jpeg", ImageFormat.Jpeg);
                bitmap.Dispose();
            }
            else
            {
                bitmap.Save(Directory.GetCurrentDirectory() + "\\Image\\" + DateTime.Now.ToString("ddMMyyyy") + "\\" + ClientID + NPorRO + PageNo + ".Jpeg", ImageFormat.Jpeg);
                bitmap.Dispose();
            }

            MessageBox.Show("Image Created in" + directory1, "Information",MessageBoxButton.OK,MessageBoxImage.Information);

        }
        //private void NetPositionForImageCreation(DataTable dt, string ClientID, StringBuilder SBColumn0,
        //                                           StringBuilder SBColumn1, StringBuilder SBColumn2, StringBuilder SBColumn3, StringBuilder SBColumn4, int PageNo)
        //{
        //    SBColumn0.Append("Sn" + Environment.NewLine);
        //    SBColumn1.Append("Scrip Id--ISIN" + Environment.NewLine);
        //    SBColumn2.Append("Net Qty/Lot" + Environment.NewLine);
        //    SBColumn3.Append("Net Price" + Environment.NewLine);

        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        if (dr.RowState != DataRowState.Deleted)
        //        {
        //            if (dr["NetQty"].ToString() == "")
        //            {
        //                dr["NetQty"] = "0";
        //            }
        //            SBColumn0.Append(serialNumber + Environment.NewLine);
        //            SBColumn1.Append(dr["Scrip Id"].ToString());
        //            SBColumn2.Append(dr["NetQty"].ToString() + Environment.NewLine);
        //            if (dr["NetQty"] != "0")
        //            {
        //                if (dr["NetQty"].ToString().Contains("-"))
        //                {
        //                    SBColumn3.Append(dr["SavRate"].ToString() + Environment.NewLine);
        //                }
        //                else
        //                {
        //                    SBColumn3.Append(dr["BavRate"].ToString() + Environment.NewLine);
        //                }
        //            }
        //            else
        //            {
        //                SBColumn3.Append("--" + Environment.NewLine);
        //            }
        //            if (dr["ISIN No"].ToString().Trim() != "")
        //            {
        //                SBColumn1.Append("--" + dr["ISIN No"].ToString().Trim() + Environment.NewLine);
        //            }
        //            else
        //            {
        //                SBColumn1.Append(Environment.NewLine);
        //            }

        //            serialNumber++;
        //        }
        //    }
        //    //for (int i = 0; i < 2; i++)
        //    //{
        //    //    SBColumn0.Append(serialNumber + Environment.NewLine);
        //    //    SBColumn1.Append("USDINR16JUNE63.50000CE" + Environment.NewLine);
        //    //    SBColumn2.Append("8888888888" + Environment.NewLine);

        //    //    SBColumn3.Append("88888888888" + Environment.NewLine);
        //    //    serialNumber++;
        //    //}
        //    //for (int i = 0; i < 2; i++)
        //    //{
        //    //    SBColumn0.Append(serialNumber + Environment.NewLine);
        //    //    SBColumn1.Append("USDINR16JUNE63.50000CE000CE" + Environment.NewLine);
        //    //    SBColumn2.Append("888" + Environment.NewLine);

        //    //    SBColumn3.Append("88888888888" + Environment.NewLine);
        //    //    serialNumber++;
        //    //}
        //}

        private void EmailClick_Click()
        {
            try
            {
                var csv_file_path = (Array)null;
                SMTPIP = ReadEmailDetailFromINI();
                //bool ForEmail = true;

                csv_file_path = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Sub Systems" + "//NPFilesForMail", "*.csv"); ////while released
                                                                                                                                     // csv_file_path = Directory.GetFiles("C:\\bolt\\bolt\\Sub Systems\\NPFilesForMail", "*.csv"); // //Need to change if working with development

                if (LiveSettings == "2") //i.e SMTP, not direct connection 
                {
                    IsPing = EmailProfilingVM.PingValidation(SMTPIP); //PORT and IP address checks are built   

                    if (IsPing == true && PortStatus == 1)
                    {

                        foreach (string s in csv_file_path)
                        {
                            ConvertCSVtoDataTable(s, 0); //for net position
                        }
                        //Intimation to TWS that all the files are processed  
                    }
                    else
                    {
                        foreach (string s in csv_file_path)
                        {
                            File.Delete(s);
                        }
                        MessageBox.Show("Either SMTP IP address ping is not reachable or port is not open", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    //Win32.PostMessage(CommonUtil.GhTwsWnd, AM_CMSNETPOSSAVE, 1, 0);

                }
                else if (LiveSettings == "1")  //direct connection
                {

                    foreach (string s in csv_file_path)
                    {
                        ConvertCSVtoDataTable(s, 0); //for net position
                    }
                    //Win32.PostMessage(CommonUtil.GhTwsWnd, AM_CMSNETPOSSAVE, 1, 0); //Intimation to TWS that all the files are processed  

                }
            }

            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }

        }

        public void ConvertCSVtoDataTable(string strFilePath, int flag)
        {
            //MessageBox.Show("1", "information");
            string email, GenTime, ClientID, SettlementNumber = "";
            Boolean bln_validfile = true;
            StringBuilder SB = new StringBuilder();

            DateTime serverTime = DateTime.Now;
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] headers = line.Split(',');
                    if ((headers.Length == 10 && flag == 0) || (headers.Length == 11 && flag == 1))
                    {
                        bln_validfile = true;
                        foreach (string header in headers)
                        { dt.Columns.Add(header); }

                        while (!sr.EndOfStream)
                        {
                            string[] rows = sr.ReadLine().Split(',');
                            DataRow dr = dt.NewRow();
                            for (int i = 0; i < headers.Length; i++)
                            {
                                dr[i] = rows[i];
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    else
                    { bln_validfile = false; }
                    break;
                }

                if (line == null)
                { bln_validfile = false; }
            }
            if (bln_validfile)
            {
                ClientID = dt.Rows[1].ItemArray[0].ToString();
                if (flag == 0) //Net Position
                {
                    email = dt.Rows[0].ItemArray[8].ToString();
                    GenTime = dt.Rows[0].ItemArray[9].ToString();
                    SettlementNumber = dt.Rows[0].ItemArray[7].ToString();
                    if (SettlementNumber == "NA")
                    {
                        SettlementNumber = "";
                    }

                    NetPositionMessageBodyFormation(dt, ClientID, SB);

                }
                else //(flag == 1) //Returned Orders
                {
                    email = dt.Rows[0].ItemArray[9].ToString();
                    GenTime = dt.Rows[0].ItemArray[10].ToString();

                    ReturnOrderMessageBodyFormation(dt, ClientID, SB);

                }
                SB.Append("<font-family:calibri>Generated at: " + serverTime + "(IST)" + "</font>");
                if (flag == 0) //Net Position
                {
                    SB.Append("</b></br><font-family:calibri;text-align:right>Equity Settlement Number: " + SettlementNumber + "</font>");
                }
                SB.Append("<p><font-family:calibri>This communication is for your internal purpose only. In case you find any mismatch/discrepancy in transaction(s), please inform to your trading member only. This email is nonbinding document/ communication and meant for your perusal only. You agree not to refer this communication in any legal proceedings.</font></p>");
                SB.Append("<br><b><font-family:calibri>Thanks & Regards,</font></b></br>");
                SB.Append("<br><font-family:calibri>(This is a System Generated mail)</font></br>");
                SB.Append("<font-family:calibri>BSE: World’s fastest Exchange with a speed of 6 microseconds</font>");

                if (email != "NA")
                {
                    if (LiveSettings == "1") //1 is direct internet connection
                    {
                        EmailFormatForDirectInternetConnection(email, ClientID, flag, strFilePath, SB);
                    }

                    else if (LiveSettings == "2") //SMTP connection
                    {
                        EmailFormatForSMTPConnection(email, ClientID, flag, strFilePath, SB);
                    }

                }
                else
                {
                    File.Delete(strFilePath); //In case of NA, delete the file from folder.   
                }

            }
            else
            {
                File.Delete(strFilePath); //In case of junk file, delete the file from folder.
            }

        }

        private void NetPositionMessageBodyFormation(DataTable dt, string ClientID, StringBuilder SB)
        {

            dt.Rows[0].Delete();

            SB.Append("<p><font-family:calibri>Dear Investor (Client ID- " + ClientID + ")," + "</font></p>");
            SB.Append("<p><font-family:calibri>Provisional Trade Summary (excluding brokerage & other charges) of your transaction(s) is given below- </font></p>");
            SB.Append("<table>");
            SB.Append("<tr><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>Scrip Id</b></td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>ISIN No</b></td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>Buy Qty</b></td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>Buy Av Rate</b></td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>Sell Qty</b></td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>Sell Av Rate</b></td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>Net Qty</b></td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>Net Value</b></td></tr>");

            foreach (DataRow dr in dt.Rows)
            {
                SB.Append("<tr><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;font-family:calibri\">" + dr["Scrip Id"].ToString() + "</td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;font-family:calibri\">" + dr["ISIN No"].ToString() + "</td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;font-family:calibri\">" + dr["BQty"].ToString() + "</td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;font-family:calibri\">" + dr["BavRate"].ToString() + "</td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;font-family:calibri\">" + dr["SQty"].ToString() + "</td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;font-family:calibri\">" + dr["SavRate"].ToString() + "</td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;font-family:calibri\">" + dr["NetQty"].ToString() + "</td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;font-family:calibri\">" + dr["NetValue"].ToString() + "</td></tr>");
            }
            SB.Append("</table>");
        }

        private void ReturnOrderMessageBodyFormation(DataTable dt, string ClientID, StringBuilder SB)
        {
            dt.Rows[0].Delete();
            SB.Append("<p><font-family:calibri>Dear Investor (Client ID- " + ClientID + ")," + "</font></p>");
            SB.Append("<p><font-family:calibri>Your unexecuted order summary is given below- </font></p>");
            SB.Append("<table>");
            SB.Append("<tr><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>Scrip Id</b></td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>Scrip Code</b></td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>BuySell</b></td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>Total Qty</b></td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>Rev Qty</b></td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>Rate</b></td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>Order ID</b></td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>Client Type</b></td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>Retention</b></td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;border-top:1pt solid green;background-color:#b5dbe8;font-family:calibri\"><b>Reason Of Return</b></td></tr>");

            foreach (DataRow dr in dt.Rows)
            {
                SB.Append("<tr><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solidgreen;font-family:calibri\">" + dr["Scrip Id"].ToString() + "</td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;font-family:calibri\">" + dr["Scrip Code"].ToString() + "</td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;font-family:calibri\">" + dr["B/S"].ToString() + "</td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;font-family:calibri\">" + dr["Total Qty"].ToString() + "</td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;font-family:calibri\">" + dr["Rev Qty"].ToString() + "</td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;font-family:calibri\">" + dr["Rate"].ToString() + "</td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;font-family:calibri\">" + dr["Order Id"].ToString() + "</td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;font-family:calibri\">" + dr["Client Type"].ToString() + "</td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;font-family:calibri\">" + dr["Retention"].ToString() + "</td><td style=\"text-align:center;border-bottom:1pt solid green;border-right:1pt solid green;border-left:1pt solid green;font-family:calibri\">" + dr["Reason Of Return"].ToString() + "</td></tr>");
            }
            SB.Append("</table>");
        }

        private void EmailFormatForDirectInternetConnection(string email, string ClientID, int flag, string strFilePath, StringBuilder SB)
        {
            // MessageBox.Show("inside  ConvertCSVtoDataTable LiveSettings == 1", "information");
            //MailMessage msg = new MailMessage(GmailEmailID, "ajay.sengar@bseindia.com,cmcbolt@bseindia.com");
            MailMessage msg = new MailMessage(GmailEmailID, email);
            if (flag == 0)
                msg.Subject = "Netposition Summary (Client ID - " + ClientID + ") ";
            else
                msg.Subject = "Unexecuted Order Summary (Client ID - " + ClientID + ") ";

            msg.Body = SB.ToString();
            msg.BodyEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            // System.Net.NetworkCredential basicCredential = new System.Net.NetworkCredential(GmailEmailID, GmailPassword);
            client.Credentials = new System.Net.NetworkCredential(GmailEmailID, GmailPassword);
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            //client.Credentials = basicCredential;
            try
            {
                client.Send(msg);
                File.Delete(strFilePath); //after sending the mail, delete the file from folder.  
            }
            catch (IOException ex)
            {
                ExceptionUtility.LogError(ex);

            }
            catch (SocketException ex)
            {
                ExceptionUtility.LogError(ex);
            }

            catch (Exception ex)
            {
                bool exceptioncaught = false;

                if (ex.ToString().Contains("The remote name could not be resolved: 'smtp.gmail.com'"))
                {
                    MessageBox.Show("The remote name could not be resolved: 'smtp.gmail.com'","Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    exceptioncaught = true;
                }

                if (ex.ToString().Contains("The SMTP server requires a secure connection or the client was not authenticated."))
                {
                    MessageBox.Show("The remote name could not be resolved: 'smtp.gmail.com'", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    exceptioncaught = true;

                }
                if (!exceptioncaught)
                {
                    MessageBox.Show("Email Sending Failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            finally
            {
                File.Delete(strFilePath);
            }
        }

        private void EmailFormatForSMTPConnection(string email, string ClientID, int flag, string strFilePath, StringBuilder SB)
        {

            // MessageBox.Show("inside  ConvertCSVtoDataTable LiveSettings == 2", "information");
            //MailMessage msg = new MailMessage(LiveFromEmail, "ajay.sengar@bseindia.com,cmcbolt@bseindia.com");
            MailMessage msg = new MailMessage(LiveFromEmail, email);
            //msg.Subject = "Netposition Summary (Client ID - ) ";
            if (flag == 0)
                msg.Subject = "Netposition Summary (Client ID - " + ClientID + ") ";
            else
                msg.Subject = "Unexecuted Order Summary (Client ID - " + ClientID + ") ";


            msg.Body = SB.ToString();
            msg.BodyEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            SmtpClient client = new SmtpClient(LiveSMTPIP, Convert.ToInt16(LiveSMTPPort));
            client.UseDefaultCredentials = false;
            System.Net.NetworkCredential basicCredential = new System.Net.NetworkCredential(LiveSMTPEmailID, LiveSMTPPassword);
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.Credentials = basicCredential;
            try
            {
                client.Send(msg);
                File.Delete(strFilePath); //after sending the mail, delete the file from folder.    
            }
            catch (Exception ex)
            {
                File.Delete(strFilePath);
                ExceptionUtility.LogError(ex);
            }
        }

        public static string ReadEmailDetailFromINI()
        {
            SMTPIP = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "TestSMTPServerIP");
            //For Test SMTP : End
            //For Live SMTP: Start
            LiveSMTPIP = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "LiveSMTPServerIP");
            LiveSMTPPort = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "LiveSMTPPort");
            LiveSMTPEmailID = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "LiveSMTPEmailID");
            LiveSMTPPassword = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "LiveSMTPPassword");
            if (!string.IsNullOrEmpty(LiveSMTPPassword))//Vijayalakshmi - To implement Decryption of the encrypted password
            {
                string LiveDecrypted = EmailProfilingVM.EncryptDecrypt(LiveSMTPPassword);
                LiveSMTPPassword = LiveDecrypted;
            }
            LiveFromEmail = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "LiveFromEmailID");
            LiveSettings = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "LiveSettings");
            //MessageBox.Show(LiveSettings, "Information");
            //For Live SMTP: End
            //For Direct Internent : Start
            GmailEmailID = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "GmailID");
            GmailPassword = MainWindowVM.parser.GetSetting("SMTP EMAIL PROFILING", "Password");
            if (!string.IsNullOrEmpty(GmailPassword))//Vijayalakshmi - To implement Decryption of the encrypted password
            {
                string GmailDecrypted = EmailProfilingVM.EncryptDecrypt(GmailPassword);
                GmailPassword = GmailDecrypted;
            }
            //For Direct Internent : End


            return SMTPIP;

        }
        #endregion


        private void ClientWindow_Closing(object e)
        {
            Windows_NetPositionClientwiseLocationChanged(e);
            //if (CommonFrontEnd.Processor.UMSProcessor.OnTradeReceivedCW != null)
            //CommonFrontEnd.Processor.UMSProcessor.OnTradeReceivedCW -= NetPositionMemory.UpdateClientNetPosition;

            //if (UMSProcessor.OnTradeCWReceived != null)
            //    UMSProcessor.OnTradeCWReceived -= UpdateHeader;

            NetPositionClientWise oNetPositionClientWise = System.Windows.Application.Current.Windows.OfType<NetPositionClientWise>().FirstOrDefault();
            if (oNetPositionClientWise != null)
            {
                if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
                {
                    BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = (BoltAppSettingsWindowsPosition)CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                    if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISE != null && oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISE.WNDPOSITION != null)
                    {
                        oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISE.WNDPOSITION.Left = Convert.ToInt32(LeftPosition);
                        oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISE.WNDPOSITION.Top = Convert.ToInt32(TopPosition);
                        oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISE.WNDPOSITION.Right = Convert.ToInt32(Width);
                        oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISE.WNDPOSITION.Down = Convert.ToInt32(Height);
                    }
                    //CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(@"D:\TWS_DotNetNewStructure\TWS_DOTNETT\CommonFrontEnd\bin\Debug\xml\Users\AppSettings\920200000.xml", "WindowsPosition");
                    CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(CommonFrontEnd.SharedMemories.SettingsManager.AppSettingsXmlPath, "WindowsPosition");
                }
                oNetPositionClientWise.Hide();
            }

        }

        private void DataGrid_DoubleClick()
        {
            var data = SelectedItem;
            if (data != null)
            {
                #region Commented gaurav 25/04/2017


                //if (!Load_NetpositionCWSWEntry)
                //{
                //    oNetPositionClientWiseDetails = new NetPositionClientWiseDetails();
                //}
                //oNetPositionClientWiseDetails.Focus();
                //oNetPositionClientWiseDetails.Show();
                //oNetPositionClientWiseDetails.Activate();


                //Load_NetpositionCWSWEntry = true;

                //if (onClientDoubleClickEventHandler != null)
                //    onClientDoubleClickEventHandler(data.TraderId);
                #endregion

                NetPositionClientWiseDetails oNetPositionClientWiseDetails = System.Windows.Application.Current.Windows.OfType<NetPositionClientWiseDetails>().FirstOrDefault();
                if (oNetPositionClientWiseDetails != null)
                {
                    oNetPositionClientWiseDetails.Focus();
                    oNetPositionClientWiseDetails.Show();
                    if (oNetPositionClientWiseDetails.WindowState == System.Windows.WindowState.Minimized)
                    {
                        oNetPositionClientWiseDetails.WindowState = System.Windows.WindowState.Normal;
                    }
                }
                else
                {
                    oNetPositionClientWiseDetails = new NetPositionClientWiseDetails();
                    oNetPositionClientWiseDetails.Activate();
                    oNetPositionClientWiseDetails.Show();
                }
                if (UtilityLoginDetails.GETInstance.Role?.ToLower() == "admin")
                {
                    if (onClientDoubleClickEventHandler != null)
                        onClientDoubleClickEventHandler(data.TraderId);
                }
                else if (UtilityLoginDetails.GETInstance.Role?.ToLower() == "trader")
                {
                    if (onClientDoubleClickEventHandler != null)
                        onClientDoubleClickEventHandler(data.ClientId);
                }
            }

        }

        public void UpdateHeader()
        {
            NetRealPL = MemoryManager.NetRealPL;
            NetUnRealPL = MemoryManager.NetUnRealPL;
            NetPL = MemoryManager.NetPL;
        }

        private void btnSquareOffSaveBatch(object e)
        {
            try
            {
                if (selectEntireRowList.Count <= 0)
                {
                    System.Windows.Forms.MessageBox.Show("No Order Selected to Save!!", "Warning!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }
                System.Windows.Forms.SaveFileDialog objFileDialogBatchResub = new System.Windows.Forms.SaveFileDialog();
                objFileDialogBatchResub.InitialDirectory = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"User/"))));
                if (!Directory.Exists(objFileDialogBatchResub.InitialDirectory))
                    Directory.CreateDirectory(objFileDialogBatchResub.InitialDirectory);

                objFileDialogBatchResub.DefaultExt = "csv";
                string Filter = "CSV files (*.csv)|*.csv";
                objFileDialogBatchResub.Filter = Filter;
                const string header = "Buy/Sell,Qty,Rev.Qty,Scrip Code,Rate,Short/Client ID,Retention Status,Client Type,Order Type,CP Code";
                StreamWriter writer = null;
                if (objFileDialogBatchResub.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Filter = objFileDialogBatchResub.FileName;

                    writer = new StreamWriter(Filter, false, Encoding.UTF8);

                    writer.WriteLine(header);
                    foreach (var item in selectEntireRowList.ToList())
                    {
                        int NetQty = 0;
                        if (UtilityLoginDetails.GETInstance.Role?.ToLower() == "trader")
                        {
                            //var NetQty = string.Empty;
                            var Obj = MemoryManager.NetPositionCWDemoDict.Where(x => ((NetPosition)x.Value).ClientId == item.ClientId).ToList();
                            var results = Obj.AsParallel().GroupBy(p => ((NetPosition)p.Value).ScripName,
                                        p => p.Value,
                                        (key, g) => new
                                        {
                                            ScripName = key,
                                            ScripData = g.ToList()
                                        }
                                       );
                            foreach (var resultItem in results)
                            {
                                NetQty = (resultItem.ScripData.Cast<NetPosition>().Sum(x => x.NetQty));

                                if (NetQty > 0)
                                {
                                    writer.WriteLine($"{"S"}, {NetQty}, {NetQty}, {item.ScripCode}, {0}, {item.ClientId}, {"EOTODY"},{item.ClientType},{"G"},{""}");
                                }
                                else if (NetQty < 0)
                                {
                                    writer.WriteLine($"{"B"}, {(NetQty * -1)}, {(NetQty * -1)}, {item.ScripCode}, {0}, {item.ClientId}, {"EOTODY"},{item.ClientType},{"G"},{""}");
                                }
                            }
                            
                        }
                        
                        
                        
                    }
                    writer.Close();

                    System.Windows.MessageBox.Show("File Saved Successfully", "Successfull!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                }

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        public NetPositionClientWiseVM()
        {
            if (UMSProcessor.OnTradeCWReceived == null)
                UMSProcessor.OnTradeCWReceived += UpdateHeader;

            NetPositionCWDataCollectionDemo = new ObservableCollection<ClientWisePositionModel>();
            NetPositionCWDataCollectionDemo = NetPositionMemory.NetPositionCWDataCollection;
            selectEntireRowList = new List<ClientWisePositionModel>();
            if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                CommonFrontEnd.Model.BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = new Model.BoltAppSettingsWindowsPosition();
                oBoltAppSettingsWindowsPosition = (CommonFrontEnd.Model.BoltAppSettingsWindowsPosition)CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISE != null && oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISE.WNDPOSITION != null)
                {
                    Height = oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISE.WNDPOSITION.Down.ToString();
                    TopPosition = oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISE.WNDPOSITION.Top.ToString();
                    LeftPosition = oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISE.WNDPOSITION.Left.ToString();
                    Width = oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISE.WNDPOSITION.Right.ToString();
                }
            }
            //TODO: Temporary Condition. Remove after Column Profiling
            if (UtilityLoginDetails.GETInstance.Role?.ToLower() == "trader")
            {
                RoleWiseMinWidth = 70;
                RoleWiseMaxWidth = 200;
            }
            else if (UtilityLoginDetails.GETInstance.Role?.ToLower() == "admin")
            {
                RoleWiseMinWidth = 0;
                RoleWiseMaxWidth = 0;
            }

        }




        #region Export_to_CSV

        public void ExecuteMyCommand(ObservableCollection<ClientWisePositionModel> NetPositionCWDataCollectionDemo)
        {
            if (NetPositionCWDataCollectionDemo.Count == 0)
            {
                MessageBox.Show("No Records to Save", "Information", MessageBoxButton.OK,MessageBoxImage.Information);
                return;
            }
            StreamWriter sw = null;
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV Files (.csv)|*.csv";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {

                sw = new StreamWriter(dlg.FileName, false, System.Text.Encoding.UTF8);
                try
                {
                    sw.Write("Client, Client Type, Gr.Purchase, Gr.Sell, Net Value, Net P/L, Real P/L, UnReal P/L");
                    sw.Write(sw.NewLine);

                    foreach (ClientWisePositionModel dr in NetPositionCWDataCollectionDemo)
                    {
                        sw.Write(dr.ClientId + "," + dr.ClientType + "," + dr.GrossPurchaseString + "," + dr.GrossSellString + "," + dr.NetValue + "," + dr.NetPLIn2Long + "," + dr.RealPLIn2Long + "," +
                            dr.UnRealPLIn2Long);

                        sw.Write(sw.NewLine);
                    }
                    System.Windows.MessageBox.Show("Trades Saved in file : " + dlg.FileName.ToString(),"Net Position Information",MessageBoxButton.OK,MessageBoxImage.Information);
                }
                catch (Exception e)
                {
                    ExceptionUtility.LogError(e);
                    System.Windows.MessageBox.Show("Error in Exporting data to CSV","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                }
                finally
                {
                    sw.Flush();
                    sw.Close();
                }
            }
            else
            {
                return;
            }

        }
        #endregion

        //using (StreamWriter writer = new StreamWriter(@"C:\Users\tcs.prafulla\Desktop\Test\new\bfx_co\myBFX_CO.csv", false))
        //{
        //    foreach (ScripMasterBfxCo line in objMasterDicBfxCo.Values)
        //        writer.WriteLine(line.SeriesID + "," + line.AssetTokenNumber + "," + line.ProductType + "," + line.AssetCode + "," +
        //            line.filler1 + "," + line.filler2 + "," + line.ExpiryDate + "," + line.StrikePrice
        //            + "," + line.OptionType + "," + line.Precision + "," + line.Filler3 + "," +
        //            line.Filler4 + "," + line.PartitionId + "," + line.Filler5 + "," + line.Filler6 + "," +
        //            line.Filler7 + "," + line.Filler8 + "," +
        //            line.Filler9 + "," + line.Filler10 + "," +
        //            line.Filler11 + "," + line.Filler12 + "," + line.Filler13 + "," + line.Filler14 + "," +
        //             line.Filler15 + "," + line.Filler16 + "," + line.Filler17 + "," + line.ContractStartDate + ","
        //             + line.SettlementDate + "," + line.MarketSegmentID + "," + line.CapacityGroupID + "," + line.MinimumLotSize +
        //             "," + line.BoardLotQuantity + "," + line.TiceSize + "," + line.Filler18 + "," + line.Filler19 + "," + line.Filler20 +
        //             "," + line.SimpleSecurityID + "," + line.Filler21 + "," + line.Filler22 + "," + line.Filler23 + "," + line.Filler24 + "," +
        //             line.Filler25 + "," + line.Filler26 + "," + line.Filler27 + "," + line.Filler28 + "," + line.Filler29 + "," + line.Filler30 +
        //             "," + line.Filler31 + "," + line.ContractExerciseStartDate + "," + line.ContractExerciseEndDate + "," + line.Filler32 +
        //             "," + line.QuantityMultiplier1 + "," + line.Filler33 + "," + line.InstrumentName + "," + line.Filler34 + "," + line.Filler35 + "," +
        //             line.Filler36 + "," + line.UnderlyingMarket + "," + line.Filler37 + "," + line.ContractType + "," + line.Filler38 + "," +
        //             line.Filler39 + "," + line.Filler40 + "," + line.Filler41 + "," + line.Filler42 + "," + line.Filler43 + "," + line.Filler44 + "," +
        //             line.BasePrice + "," + line.DeleteFlag
        //            );
        //}
    }
#endif 
}
