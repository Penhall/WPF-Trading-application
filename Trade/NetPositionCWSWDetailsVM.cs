using CommonFrontEnd.Common;
using CommonFrontEnd.Global;
using CommonFrontEnd.Model;
using CommonFrontEnd.Model.Trade;
//using CommonFrontEnd.Model.Settings;
using CommonFrontEnd.Processor;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.View.Order;
using CommonFrontEnd.View.Trade;
using CommonFrontEnd.ViewModel.Order;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace CommonFrontEnd.ViewModel.Trade
{
#if TWS
    public class NetPositionCWSWDetailsVM : BaseViewModel
    {

        #region Properties
        private static string ClientIdCWSW { get; set; }

        DirectoryInfo directory = new DirectoryInfo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"User/NetPositionClientWiseDetail.csv")));

        private ObservableCollection<CWSWDetailPositionModel> _NetPositionCWSWDataCollectionWindow;

        public ObservableCollection<CWSWDetailPositionModel> NetPositionCWSWDataCollectionWindow
        {
            get { return _NetPositionCWSWDataCollectionWindow; }
            set { _NetPositionCWSWDataCollectionWindow = value; NotifyPropertyChanged("NetPositionCWSWDataCollectionWindow"); }
        }
        private string _LeftPosition = "345";

        public string LeftPosition
        {
            get { return _LeftPosition; }
            set { _LeftPosition = value; NotifyPropertyChanged("LeftPosition"); }
        }

        private string _txtReply;

        public string txtReply
        {
            get { return _txtReply; }
            set { _txtReply = value; NotifyPropertyChanged(nameof(txtReply)); }
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

        private CWSWDetailPositionModel _selectEntireRow;

        public CWSWDetailPositionModel selectEntireRow
        {
            get { return _selectEntireRow; }
            set
            {
                _selectEntireRow = value; NotifyPropertyChanged(nameof(selectEntireRow));
                txtReply = string.Empty;
            }
        }

        private string _Height = "358.914";

        public string Height
        {
            get { return _Height; }
            set { _Height = value; NotifyPropertyChanged("Height"); }
        }

        private long _TotalNetRealPLCWSW;

        public long TotalNetRealPLCWSW
        {
            get { return _TotalNetRealPLCWSW = Convert.ToInt64(MemoryManager.TotalNetRealPL / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))); }
            set { _TotalNetRealPLCWSW = value; NotifyPropertyChanged("TotalNetRealPLCWSW"); }
        }

        private long _TotalNetUnRealPLCWSW;

        public long TotalNetUnRealPLCWSW
        {
            get { return _TotalNetUnRealPLCWSW = Convert.ToInt64(MemoryManager.TotalNetUnRealPL / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))); }
            set { _TotalNetUnRealPLCWSW = value; NotifyPropertyChanged("TotalNetUnRealPLCWSW"); }
        }

        private long _TotalNetPLCWSW;

        public long TotalNetPLCWSW
        {
            get { return _TotalNetPLCWSW = Convert.ToInt64(MemoryManager.TotalNetPL / (Math.Pow(10, UtilityApplicationDetails.GetInstance.MaxDecimalPoint))); }
            set { _TotalNetPLCWSW = value; NotifyPropertyChanged("TotalNetPLCWSW"); }
        }

        private List<CWSWDetailPositionModel> _selectEntireRowList;

        public List<CWSWDetailPositionModel> selectEntireRowList
        {
            get { return _selectEntireRowList; }
            set { _selectEntireRowList = value; NotifyPropertyChanged(nameof(selectEntireRowList)); }
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
                
                ToggleVisibility4DecimalCheck(_Ratein4decimalChecked);
                return _Ratein4decimalChecked;
            }
            set
            {
                _Ratein4decimalChecked = value;
                MultiWindowCheckBoxCheckExtension.is4decimalCheckboxCheck = value;
                NotifyPropertyChanged("Ratein4decimalChecked");
                ToggleVisibility4DecimalCheck(value);
            }
        }

        private string _isAvgBuyRateString4decimalVisible = Visibility.Hidden.ToString();

        public string isAvgBuyRateString4decimalVisible
        {
            get { return _isAvgBuyRateString4decimalVisible; }
            set { _isAvgBuyRateString4decimalVisible = value; NotifyPropertyChanged("isAvgBuyRateString4decimalVisible"); }
        }

        private string _isAvgBuyRateString2decimalVisible = Visibility.Visible.ToString();

        public string isAvgBuyRateString2decimalVisible
        {
            get { return _isAvgBuyRateString2decimalVisible; }
            set { _isAvgBuyRateString2decimalVisible = value; NotifyPropertyChanged("isAvgBuyRateString2decimalVisible"); }
        }

        private string _isAvgSellRateString2decimalVisible = Visibility.Visible.ToString();

        public string isAvgSellRateString2decimalVisible
        {
            get { return _isAvgSellRateString2decimalVisible; }
            set { _isAvgSellRateString2decimalVisible = value; NotifyPropertyChanged("isAvgSellRateString2decimalVisible"); }
        }

        private string _isAvgSellRateString4decimalVisible = Visibility.Hidden.ToString();

        public string isAvgSellRateString4decimalVisible
        {
            get { return _isAvgSellRateString4decimalVisible; }
            set { _isAvgSellRateString4decimalVisible = value; NotifyPropertyChanged("isAvgSellRateString4decimalVisible"); }
        }
        #endregion

        #region RelayCommannd

        private RelayCommand _btnSquareOff;

        public RelayCommand btnSquareOff
        {
            get
            {
                return _btnSquareOff ?? (_btnSquareOff = new RelayCommand(
                    (object e) => SquareOffData(e)
                        ));
            }
        }



        private RelayCommand _ShortCut_Escape;

        public RelayCommand ShortCut_Escape
        {
            get
            {
                return _ShortCut_Escape ?? (_ShortCut_Escape = new RelayCommand(
                    (object e) => OnNetPositionCWSWWindowClosing(e)
                        ));
            }
        }

        private RelayCommand _myLocationChanged;

        public RelayCommand myLocationChanged
        {
            get
            {
                return _myLocationChanged ?? (_myLocationChanged = new RelayCommand(
                    (object e) => Windows_NetPositionClientwiseDetailsLocationChanged(e)));

            }
        }

        private RelayCommand _btnSquareOffList;

        public RelayCommand btnSquareOffList
        {
            get
            {
                return _btnSquareOffList ?? (_btnSquareOffList = new RelayCommand((object e) => btnSquareDataOffList(e)));
            }
        }



        private RelayCommand _NetPositionCWSWWindowClosing;

        public RelayCommand NetPositionCWSWWindowClosing
        {
            get { return _NetPositionCWSWWindowClosing ?? (_NetPositionCWSWWindowClosing = new RelayCommand((object e) => OnNetPositionCWSWWindowClosing(e))); }
        }

        private void OnNetPositionCWSWWindowClosing(object e)
        {

            // NetPositionClientWiseVM.Load_NetpositionCWSWEntry = false;
            //NetPositionClientWiseVM.onClientDoubleClickEventHandler -= UpdateGridByClient;
            //UMSProcessor.OnTradeCWSWReceived -= UpdateHeader;
            //UMSProcessor.OnTradeCWSWOnlineReceived -= UpdateGridByClientOnline;

            //TODO: Uncomment Windows Position ScripWiseVM - Gaurav 03/11/2017
            NetPositionClientWiseDetails oNetPositionClientWiseDetails = System.Windows.Application.Current.Windows.OfType<NetPositionClientWiseDetails>().FirstOrDefault();
            if (oNetPositionClientWiseDetails != null)
            {
                Windows_NetPositionClientwiseDetailsLocationChanged(e);
                //if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
                //{
                //    BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = (BoltAppSettingsWindowsPosition)CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                //    if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISEDETAILS != null && oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISEDETAILS.WNDPOSITION != null)
                //    {
                //        oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISEDETAILS.WNDPOSITION.Left = Convert.ToInt32(LeftPosition);
                //        oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISEDETAILS.WNDPOSITION.Top = Convert.ToInt32(TopPosition);
                //        oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISEDETAILS.WNDPOSITION.Right = Convert.ToInt32(Width);
                //        oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISEDETAILS.WNDPOSITION.Down = Convert.ToInt32(Height);
                //    }
                //    //CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(@"D:\TWS_DotNetNewStructure\TWS_DOTNETT\CommonFrontEnd\bin\Debug\xml\Users\AppSettings\920200000.xml", "WindowsPosition");
                //    CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(CommonFrontEnd.SharedMemories.SettingsManager.AppSettingsXmlPath, "WindowsPosition");
                //}

                oNetPositionClientWiseDetails.Hide();
                //}
            }
        }
        private void Windows_NetPositionClientwiseDetailsLocationChanged(object e)
        {
            if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = (BoltAppSettingsWindowsPosition)CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISEDETAILS != null && oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISEDETAILS.WNDPOSITION != null)
                {
                    oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISEDETAILS.WNDPOSITION.Left = Convert.ToInt32(LeftPosition);
                    oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISEDETAILS.WNDPOSITION.Top = Convert.ToInt32(TopPosition);
                    oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISEDETAILS.WNDPOSITION.Right = Convert.ToInt32(Width);
                    oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISEDETAILS.WNDPOSITION.Down = Convert.ToInt32(Height);
                }
                //CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(@"D:\TWS_DotNetNewStructure\TWS_DOTNETT\CommonFrontEnd\bin\Debug\xml\Users\AppSettings\920200000.xml", "WindowsPosition");
                CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(CommonFrontEnd.SharedMemories.SettingsManager.AppSettingsXmlPath, "WindowsPosition");
            }
        }


        private RelayCommand _ExportExcel;

        public RelayCommand ExportExcel
        {
            get
            {
                return _ExportExcel ?? (_ExportExcel = new RelayCommand((object e) => ExecuteMyCommand(NetPositionCWSWDataCollectionWindow)));
            }
        }


        #endregion

        //protected internal void OnNetPositionCWSWWindowClosing()
        //{
        //    NetPositionClientWiseVM.Load_NetpositionCWSWEntry = false;
        //    NetPositionClientWiseVM.onClientDoubleClickEventHandler -= UpdateGridByClient;
        //    UMSProcessor.OnTradeCWSWReceived -= UpdateHeader;
        //    UMSProcessor.OnTradeCWSWOnlineReceived -= UpdateGridByClientOnline;

        //}
        private void ToggleVisibility4DecimalCheck(bool value)
        {
            if (value == true)
            {
                isAvgBuyRateString4decimalVisible = Visibility.Visible.ToString();
                isAvgSellRateString4decimalVisible = Visibility.Visible.ToString();

                isAvgBuyRateString2decimalVisible = Visibility.Hidden.ToString();
                isAvgSellRateString2decimalVisible = Visibility.Hidden.ToString();

            }
            else
            {
                isAvgBuyRateString4decimalVisible = Visibility.Hidden.ToString();
                isAvgSellRateString4decimalVisible = Visibility.Hidden.ToString();

                isAvgBuyRateString2decimalVisible = Visibility.Visible.ToString();
                isAvgSellRateString2decimalVisible = Visibility.Visible.ToString();
            }
        }
        public void UpdateHeader()
        {
            TotalNetRealPLCWSW = MemoryManager.TotalNetRealPL;
            TotalNetUnRealPLCWSW = MemoryManager.TotalNetUnRealPL;
            TotalNetPLCWSW = MemoryManager.TotalNetPL;
        }

        private void SquareOffData(object e)
        {
            if (selectEntireRowList != null && selectEntireRowList.Count > 1)
            {
                System.Windows.Forms.MessageBox.Show("Select Single Row to Square Off", "Warning!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                // txtReply = "Select Single Row to Square Off";
                return;
            }
            if (UtilityOrderDetails.GETInstance.CurrentOrderEntry.ToLower() == Enumerations.OrderEntryWindow.Normal.ToString().ToLower())
            {

                #region for Normal Order Entry
                if (selectEntireRow != null)
                {
                    txtReply = string.Empty;
                    NormalOrderEntry objNormal = System.Windows.Application.Current.Windows.OfType<NormalOrderEntry>().FirstOrDefault();
                    // var key = string.Format("{0}_{1}", selectEntireRow.ScripCode);

                    if (objNormal != null)
                    {
                        if (selectEntireRow.NetQty > 0)
                        {
                            //((OrderEntryVM)objNormal.DataContext).BuySellWindow("SELL");
                            if (objNormal.WindowState == WindowState.Minimized)
                                objNormal.WindowState = WindowState.Normal;

                            if (((NormalOrderEntryVM)objNormal.DataContext).WindowColour != "#FFB3A7")
                                ((NormalOrderEntryVM)objNormal.DataContext).BuySellWindow("SELL");

                        }
                        else if (selectEntireRow.NetQty < 0)
                        {
                            if (objNormal.WindowState == WindowState.Minimized)
                                objNormal.WindowState = WindowState.Normal;

                            if (((NormalOrderEntryVM)objNormal.DataContext).WindowColour != "#89C4F4")
                                ((NormalOrderEntryVM)objNormal.DataContext).BuySellWindow("BUY");


                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Invalid Net Qty.", "Alert",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                            return;
                        }
                        ((NormalOrderEntryVM)objNormal.DataContext).PassByNetPositionClientWiseDetails(selectEntireRow.ScripCode, selectEntireRow);
                        objNormal.Focus();
                        objNormal.Show();
                    }
                    else
                    {
                        objNormal = new NormalOrderEntry();
                        objNormal.Owner = System.Windows.Application.Current.MainWindow;
                        //objswift.CmbExcangeType.Focus();

                        if (selectEntireRow.NetQty > 0)
                        {
                            //((OrderEntryVM)objNormal.DataContext).BuySellWindow("SELL");
                            if (((NormalOrderEntryVM)objNormal.DataContext).WindowColour != "#FFB3A7")
                                ((NormalOrderEntryVM)objNormal.DataContext).BuySellWindow("SELL");
                        }
                        else if (selectEntireRow.NetQty < 0)
                        {
                            // ((OrderEntryVM)objNormal.DataContext).BuySellWindow("BUY");
                            if (((NormalOrderEntryVM)objNormal.DataContext).WindowColour != "#89C4F4")
                                ((NormalOrderEntryVM)objNormal.DataContext).BuySellWindow("BUY");
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Invalid Net Qty.", "Alert",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                            return;
                        }

                        objNormal.Activate();
                        objNormal.Show();
                        ((NormalOrderEntryVM)objNormal.DataContext).PassByNetPositionClientWiseDetails(selectEntireRow.ScripCode, selectEntireRow);
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Please Select order to Resubmit", "Alert",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    //txtReply = "Please Select order to Resubmit";
                }
                #endregion
            }
            else if (UtilityOrderDetails.GETInstance.CurrentOrderEntry.ToLower() == Enumerations.OrderEntryWindow.Swift.ToString().ToLower())
            {
                #region for Swift Order Entry
                if (selectEntireRow != null)
                {
                    //txtReply = string.Empty;
                    SwiftOrderEntry objswift = System.Windows.Application.Current.Windows.OfType<SwiftOrderEntry>().FirstOrDefault();
                    //var key = string.Format("{0}_{1}", selectEntireRow.SCode, selectEntireRow.OnlyOrderID);

                    if (objswift != null)
                    {
                        if (selectEntireRow.NetQty > 0)
                        {
                            ((OrderEntryVM)objswift.DataContext).BuySellWindow("SELL");

                        }
                        else if (selectEntireRow.NetQty < 0)
                        {
                            ((OrderEntryVM)objswift.DataContext).BuySellWindow("BUY");
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Invalid Net Qty.", "Alert",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                            return;
                        }
                        ((OrderEntryVM)objswift.DataContext).PassByNetPositionClientWiseDetails(selectEntireRow.ScripCode, selectEntireRow);
                        objswift.Focus();
                        objswift.Show();
                    }
                    else
                    {
                        objswift = new SwiftOrderEntry();
                        objswift.Owner = System.Windows.Application.Current.MainWindow;
                        //objswift.CmbExcangeType.Focus();

                        if (selectEntireRow.NetQty > 0)
                        {
                            ((OrderEntryVM)objswift.DataContext).BuySellWindow("SELL");
                        }
                        else if (selectEntireRow.NetQty < 0)
                        {
                            ((OrderEntryVM)objswift.DataContext).BuySellWindow("BUY");
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Invalid Net Qty.", "Alert",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                            return;
                        }

                        objswift.Activate();
                        objswift.Show();
                        ((OrderEntryVM)objswift.DataContext).PassByNetPositionClientWiseDetails(selectEntireRow.ScripCode, selectEntireRow);
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Please Select order to Resubmit", "Alert",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
                #endregion
            }
        }

        private void btnSquareDataOffList(object e)
        {
            try
            {
                if (selectEntireRowList.Count <= 0)
                {
                    System.Windows.MessageBox.Show("No Order Selected to Save!!", "Warning!",MessageBoxButton.OK,MessageBoxImage.Warning);
                    return;
                }
                //txtReply = string.Empty;
                SaveFileDialog objFileDialogBatchResub = new SaveFileDialog();
                objFileDialogBatchResub.InitialDirectory = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"User/"))));
                if (!Directory.Exists(objFileDialogBatchResub.InitialDirectory))
                    Directory.CreateDirectory(objFileDialogBatchResub.InitialDirectory);

                //objFileDialogBatchResub.Title = "Browse CSV Files";
                objFileDialogBatchResub.DefaultExt = "csv";
                string Filter = "CSV files (*.csv)|*.csv";
                objFileDialogBatchResub.Filter = Filter;

                const string header = "Buy/Sell,Qty,Rev.Qty,Scrip Code,Rate,Short/Client ID,Retention Status,Client Type,Order Type,CP Code";
                StreamWriter writer = null;
                if (selectEntireRowList.Count > 0)
                {
                    if (objFileDialogBatchResub.ShowDialog() == DialogResult.OK)
                    {
                        Filter = objFileDialogBatchResub.FileName;

                        writer = new StreamWriter(Filter, false, Encoding.UTF8);

                        writer.WriteLine(header);
                        foreach (var item in selectEntireRowList.Where(x => x.NetQty != 0).ToList())
                        {
                            if (item.NetQty > 0)
                                writer.WriteLine($"{"S"}, {item.NetQty}, {item.NetQty}, {item.ScripCode}, {0}, {item.ClientID}, {"EOTODY"},{item.ClientType},{"G"},{""}");
                            else
                                writer.WriteLine($"{"B"}, {(item.NetQty * -1)}, {(item.NetQty * -1)}, {item.ScripCode}, {0}, {item.ClientID}, {"EOTODY"},{item.ClientType},{"G"},{""}");
                        }
                        writer.Close();

                        System.Windows.MessageBox.Show("Reserve Batch File is save in : "+ objFileDialogBatchResub.FileName, "Net Position - reserve Batch File",MessageBoxButton.OK,MessageBoxImage.Information);
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("No Order Selected to Save!!", "Net Position- Reserve Batch File",MessageBoxButton.OK,MessageBoxImage.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }
        }

        public NetPositionCWSWDetailsVM()
        {
            if (UMSProcessor.OnTradeCWSWReceived == null)
                UMSProcessor.OnTradeCWSWReceived += UpdateHeader;

            NetPositionCWSWDataCollectionWindow = new ObservableCollection<CWSWDetailPositionModel>();
            NetPositionCWSWDataCollectionWindow = NetPositionMemory.NetPositionCWSWDataCollection;
            NetPositionClientWiseVM.onClientDoubleClickEventHandler += UpdateGridByClient;

            UMSProcessor.OnTradeCWSWOnlineReceived += UpdateGridByClientOnline;
            selectEntireRowList = new List<CWSWDetailPositionModel>();
            NetPositionClientWiseScripWiseDetail = "Net Position Details";
            //TODO: Uncomment Windows Position CWSWVM - Gaurav 03/11/2017
            if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                CommonFrontEnd.Model.BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = new Model.BoltAppSettingsWindowsPosition();
                oBoltAppSettingsWindowsPosition = (CommonFrontEnd.Model.BoltAppSettingsWindowsPosition)CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISEDETAILS != null && oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISEDETAILS.WNDPOSITION != null)
                {
                    Height = oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISEDETAILS.WNDPOSITION.Down.ToString();
                    TopPosition = oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISEDETAILS.WNDPOSITION.Top.ToString();
                    LeftPosition = oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISEDETAILS.WNDPOSITION.Left.ToString();
                    Width = oBoltAppSettingsWindowsPosition.NETPOSITIONCLIENTWISEDETAILS.WNDPOSITION.Right.ToString();
                }
            }
            isAvgBuyRateString2decimalVisible = Visibility.Visible.ToString();
            isAvgBuyRateString4decimalVisible = Visibility.Hidden.ToString();
            isAvgSellRateString2decimalVisible = Visibility.Visible.ToString();
            isAvgSellRateString4decimalVisible = Visibility.Hidden.ToString();
        }

        private void UpdateGridByClient(string TraderId)
        {
            var SettlementNo = string.Empty;
            NetPositionCWSWDataCollectionWindow = null;
            if (!string.IsNullOrEmpty(TraderId))
            {
                ClientIdCWSW = TraderId;

                if (UtilityLoginDetails.GETInstance.Role?.ToLower() == "admin")
                {
                    NetPositionMemory.UpdateCWSWDNetPosition(TraderId, MemoryManager.NetPositionCWDemoDict.Where(x => ((NetPosition)x.Value).TraderId == TraderId).ToList());
                    NetPositionCWSWDataCollectionWindow = new ObservableCollection<CWSWDetailPositionModel>(NetPositionMemory.NetPositionCWSWDataCollection.Where(x => x.TraderId == TraderId));
                }
                else if (UtilityLoginDetails.GETInstance.Role?.ToLower() == "trader")
                {
                    NetPositionMemory.UpdateCWSWDNetPosition(TraderId, MemoryManager.NetPositionCWDemoDict.Where(x => ((NetPosition)x.Value).ClientId == TraderId).ToList());
                    NetPositionCWSWDataCollectionWindow = new ObservableCollection<CWSWDetailPositionModel>(NetPositionMemory.NetPositionCWSWDataCollection.Where(x => x.ClientID == TraderId));
                }

            }
            if (!string.IsNullOrEmpty(UtilityLoginDetails.GETInstance.SettlementNo))
            {
                SettlementNo = UtilityLoginDetails.GETInstance.SettlementNo.Split('/')[1] + UtilityLoginDetails.GETInstance.SettlementNo.Split('/')[0];
            }
            if (UtilityLoginDetails.GETInstance.Role != null)
            {
                if (UtilityLoginDetails.GETInstance.Role?.ToLower() == "admin")
                {
                    NetPositionClientWiseScripWiseDetail = string.Format("Net Position Details - Trd Id- {0} - EQ Sett.No : {1}", TraderId, SettlementNo);
                }
                else if (UtilityLoginDetails.GETInstance.Role?.ToLower() == "trader")
                {
                    NetPositionClientWiseScripWiseDetail = string.Format("Net Position Details - {0} {1}- EQ Sett.No : {2}", TraderId, "CLIENT", SettlementNo);
                }
            }

        }
        private void UpdateGridByClientOnline(long scripCode)
        {
            if (!string.IsNullOrEmpty(ClientIdCWSW))
            {
                var TraderId = ClientIdCWSW;
                // NetPositionCWSWDataCollectionWindow = null;
                if (!string.IsNullOrEmpty(TraderId))
                {
                    //NetPositionMemory.UpdateCWSWDNetPosition(TraderId, MemoryManager.NetPositionCWDemoDict.ToList());
                    //NetPositionCWSWDataCollectionWindow = new ObservableCollection<CWSWDetailPositionModel>(NetPositionMemory.NetPositionCWSWDataCollection.Where(x => x.TraderId == TraderId));
                    if (UtilityLoginDetails.GETInstance.Role?.ToLower() == "admin")
                    {
                        var lstCWSWDetailPositionModelByTrader = NetPositionMemory.NetPositionCWSWDataCollection.Where(x => x.TraderId == TraderId).ToList();

                        if (lstCWSWDetailPositionModelByTrader != null && lstCWSWDetailPositionModelByTrader.Count > 0)
                        {
                            var oCWSWDetailPositionModelByScripCd = lstCWSWDetailPositionModelByTrader.Where(y => y.ScripCode == scripCode).FirstOrDefault();

                            if (oCWSWDetailPositionModelByScripCd != null)
                            {
                                if (NetPositionCWSWDataCollectionWindow != null)
                                {
                                    var item = NetPositionCWSWDataCollectionWindow.Where(x => x.TraderId == TraderId && x.ScripCode == scripCode).FirstOrDefault();
                                    if (item != null)
                                    {
                                        int index = NetPositionCWSWDataCollectionWindow.IndexOf(item);
                                        if (index != -1)
                                        {
                                            NetPositionCWSWDataCollectionWindow[index] = oCWSWDetailPositionModelByScripCd;
                                        }
                                        else
                                        {
                                            NetPositionCWSWDataCollectionWindow.Add(oCWSWDetailPositionModelByScripCd);
                                        }
                                    }
                                    else
                                    {
                                        NetPositionCWSWDataCollectionWindow.Add(oCWSWDetailPositionModelByScripCd);
                                    }
                                }
                                else
                                {
                                    NetPositionCWSWDataCollectionWindow.Add(oCWSWDetailPositionModelByScripCd);
                                }
                            }
                        }
                    }
                    else if (UtilityLoginDetails.GETInstance.Role?.ToLower() == "trader")
                    {
                        var lstCWSWDetailPositionModelByTrader = NetPositionMemory.NetPositionCWSWDataCollection.Where(x => x.ClientID == TraderId).ToList();

                        if (lstCWSWDetailPositionModelByTrader != null && lstCWSWDetailPositionModelByTrader.Count > 0)
                        {
                            var oCWSWDetailPositionModelByScripCd = lstCWSWDetailPositionModelByTrader.Where(y => y.ScripCode == scripCode).FirstOrDefault();

                            if (oCWSWDetailPositionModelByScripCd != null)
                            {
                                if (NetPositionCWSWDataCollectionWindow != null)
                                {
                                    var item = NetPositionCWSWDataCollectionWindow.Where(x => x.ClientID == TraderId && x.ScripCode == scripCode).FirstOrDefault();
                                    if (item != null)
                                    {
                                        int index = NetPositionCWSWDataCollectionWindow.IndexOf(item);
                                        if (index != -1)
                                        {
                                            NetPositionCWSWDataCollectionWindow[index] = oCWSWDetailPositionModelByScripCd;
                                        }
                                        else
                                        {
                                            NetPositionCWSWDataCollectionWindow.Add(oCWSWDetailPositionModelByScripCd);
                                        }
                                    }
                                    else
                                    {
                                        NetPositionCWSWDataCollectionWindow.Add(oCWSWDetailPositionModelByScripCd);
                                    }
                                }
                                else
                                {
                                    NetPositionCWSWDataCollectionWindow.Add(oCWSWDetailPositionModelByScripCd);
                                }
                            }
                        }
                    }
                    //
                }
            }
        }

        private string _NetPositionClientWiseScripWiseDetail;

        public string NetPositionClientWiseScripWiseDetail
        {
            get { return _NetPositionClientWiseScripWiseDetail; }
            set
            {
                _NetPositionClientWiseScripWiseDetail = value;
                NotifyPropertyChanged("NetPositionClientWiseScripWiseDetail");
            }
        }


        public void ExecuteMyCommand(ObservableCollection<CWSWDetailPositionModel> NetPositionCWSWDataCollectionWindow)
        {
            if (NetPositionCWSWDataCollectionWindow.Count == 0)
            {
                System.Windows.MessageBox.Show("No Records to Save", "Information", MessageBoxButton.OK,MessageBoxImage.Information);
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
                    sw.Write("Scrip, Buy Quantity, BuyAvgRate, Sell Quantity, SellAvgRate, Net Quantity, Net Value, BEP, Real P/L, UnReal P/L, Net P/L, ISIN Number");
                    sw.Write(sw.NewLine);

                    foreach (CWSWDetailPositionModel dr in NetPositionCWSWDataCollectionWindow)
                    {
                        sw.Write(dr.ScripName + "," + dr.BuyQty + "," + dr.AvgBuyRateString + "," + dr.SellQty + "," + dr.AvgSellRateString + "," + dr.NetQty + "," + dr.NetValue + "," + dr.BEP + "," +
                            dr.RealPLIn2Long + "," + dr.UnRealPLIn2Long + "," + dr.NetPLIn2Long + "," + dr.ISINNum);

                        sw.Write(sw.NewLine);
                    }
                    System.Windows.MessageBox.Show("Trades Saved in file : " + dlg.FileName.ToString(),"Successfull!", MessageBoxButton.OK, MessageBoxImage.Information);
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
            else { return; }

        }


    }
#endif
}
