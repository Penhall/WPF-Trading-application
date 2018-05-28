using CommonFrontEnd.Common;
using CommonFrontEnd.Global;
using CommonFrontEnd.Model;
using CommonFrontEnd.Model.Trade;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.View.Trade;
using CommonFrontEnd.ViewModel.Profiling;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
//using CommonFrontEnd.Model.Settings;

namespace CommonFrontEnd.ViewModel.Trade
{
#if TWS
    public partial class AdminTradeViewVM : BaseViewModel
    {
        #region Properties

        // static View.Trade.Saudas_Admin mWindow = null;
        static DirectoryInfo directory = new DirectoryInfo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"User/Saudas.csv")));
        static DirectoryInfo directoryAD2TR = new DirectoryInfo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"User/")));
        public static int SeqCount = 1;
        public static int PrintCount = 0;
        public static string folderpath = string.Empty;
        static StreamWriter writer = null;

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

        private string _Width = "800";

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


        private static ObservableCollection<TradeUMS> _TradeViewDataCollection;
        public static ObservableCollection<TradeUMS> TradeViewDataCollection
        {
            get { return _TradeViewDataCollection; }
            set { _TradeViewDataCollection = value; NotifyStaticPropertyChanged("TradeViewDataCollection"); }
        }

        private static ObservableCollection<SaudasUMSModel> _TradeViewDataCollectionCopy;
        public static ObservableCollection<SaudasUMSModel> TradeViewDataCollectionCopy
        {
            get { return _TradeViewDataCollectionCopy; }
            set { _TradeViewDataCollectionCopy = value; }
        }


        private static string _Title;

        public static string Title
        {
            get { return _Title; }
            set { _Title = value; NotifyStaticPropertyChanged("Title"); }
        }

        private long _DisplayCount;

        public long DisplayCount
        {
            get { return _DisplayCount; }
            set { _DisplayCount = value; NotifyStaticPropertyChanged("DisplayCount"); }
        }


        private long _TradeCount;

        public long TradeCount
        {
            get
            {
                if (_TradeCount == 0)
                {
                    _TradeCount = MemoryManager.TradeCount;
                    return _TradeCount;
                }
                else
                {
                    return _TradeCount;
                }
            }
            set { _TradeCount = value; NotifyPropertyChanged("TradeCount"); }
        }


        private static string _ElapsedTime;

        public static string ElapsedTime
        {
            get { return _ElapsedTime; }
            set { _ElapsedTime = value; NotifyStaticPropertyChanged("ElapsedTime"); }
        }

        private static string _TotalElapsedTime;

        public static string TotalElapsedTime
        {
            get { return _TotalElapsedTime; }
            set { _TotalElapsedTime = value; NotifyStaticPropertyChanged("TotalElapsedTime"); }
        }

        private Visibility _ScripNameVisible;

        public Visibility ScripNameVisible
        {
            get { return _ScripNameVisible; }
            set { _ScripNameVisible = value; NotifyPropertyChanged("ScripNameVisible"); }
        }

        private Visibility _ScripCodeVisible;

        public Visibility ScripCodeVisible
        {
            get { return _ScripCodeVisible; }
            set { _ScripCodeVisible = value; NotifyPropertyChanged("ScripCodeVisible"); }
        }

        private Visibility _LocationIdVisible;

        public Visibility LocationIdVisible
        {
            get { return _LocationIdVisible; }
            set { _LocationIdVisible = value; NotifyPropertyChanged("LocationIdVisible"); }
        }

        private static string _traderIdVisibilty;

        public static string TraderIdVisibilty
        {
            get { return _traderIdVisibilty; }
            set { _traderIdVisibilty = value; }
        }

        private static string _buySellFlagVisibilty;

        public static string BuySellFlagVisibilty
        {
            get { return _buySellFlagVisibilty; }
            set { _buySellFlagVisibilty = value; }
        }

        private static string _lastQtyVisibility;

        public static string LastQtyVisibility
        {
            get { return _lastQtyVisibility; }
            set { _lastQtyVisibility = value; }
        }

        private static string _scripCodeVisbility;

        public static string ScripCodeVisbility
        {
            get { return _scripCodeVisbility; }
            set { _scripCodeVisbility = value; }
        }

        private static string _rateInRupeesVisibility;

        public static string RateInRupeesVisibility
        {
            get { return _rateInRupeesVisibility; }
            set { _rateInRupeesVisibility = value; }
        }

        private static string _clientVisibility;

        public static string ClientVisibility
        {
            get { return _clientVisibility; }
            set { _clientVisibility = value; }
        }

        private static string _dateTimeVisibility;

        public static string DateTimeVisibility
        {
            get { return _dateTimeVisibility; }
            set { _dateTimeVisibility = value; }
        }

        private static string _orderIdVisibility;

        public static string OrderIdVisibility
        {
            get { return _orderIdVisibility; }
            set { _orderIdVisibility = value; }
        }

        private static string _tradeIDVisibility;

        public static string TradeIDVisibility
        {
            get { return _tradeIDVisibility; }
            set { _tradeIDVisibility = value; }
        }
        private static string _clientTypeVisibility;

        public static string ClientTypeVisibility
        {
            get { return _clientTypeVisibility; }
            set { _clientTypeVisibility = value; }
        }

        private string _locationIDVisibility;

        public string LocationIDVisibility
        {
            get { return _locationIDVisibility; }
            set { _locationIDVisibility = value; NotifyPropertyChanged("LocationIDVisibility"); }
        }

        private static string _dealCodeVisibility;

        public static string DealCodeVisibility
        {
            get { return _dealCodeVisibility; }
            set { _dealCodeVisibility = value; }
        }

        private static string _cPCodeVisibility;

        public static string CPCodeVisibility
        {
            get { return _cPCodeVisibility; }
            set { _cPCodeVisibility = value; }
        }

        private static string _statusVisibility;

        public static string StatusVisibility
        {
            get { return _statusVisibility; }
            set { _statusVisibility = value; }
        }

        private static string _yieldVisibility;

        public static string YieldVisibility
        {
            get { return _yieldVisibility; }
            set { _yieldVisibility = value; }
        }

        private static string _underlyingDirtyPriceVisibilty;

        public static string UnderlyingDirtyPriceVisibilty
        {
            get { return _underlyingDirtyPriceVisibilty; }
            set { _underlyingDirtyPriceVisibilty = value; }
        }

        private string _SaveTradeBtnVisibility;

        public string SaveTradeBtnVisibility
        {
            get { return _SaveTradeBtnVisibility; }
            set { _SaveTradeBtnVisibility = value; NotifyPropertyChanged(nameof(SaveTradeBtnVisibility)); }
        }


        private string _TradeFeedBtnVisibilty;

        public string TradeFeedBtnVisibilty
        {
            get { return _TradeFeedBtnVisibilty; }
            set { _TradeFeedBtnVisibilty = value; NotifyPropertyChanged(nameof(TradeFeedBtnVisibilty)); }
        }

        private Brush _DataGridBgColor;

        public Brush DataGridBgColor
        {
            get { return _DataGridBgColor; }
            set { _DataGridBgColor = value; NotifyPropertyChanged(nameof(DataGridBgColor)); }
        }

        private Brush _BuyForegroundColor;

        public Brush BuyForegroundColor
        {
            get { return _BuyForegroundColor; }
            set { _BuyForegroundColor = value; NotifyPropertyChanged(nameof(BuyForegroundColor)); }
        }

        private Brush _SellForegroundColor;

        public Brush SellForegroundColor
        {
            get { return _SellForegroundColor; }
            set { _SellForegroundColor = value; NotifyPropertyChanged(nameof(SellForegroundColor)); }
        }



        #endregion

        #region RelayCommand
        private RelayCommand _AdminWindow_Loaded;

        public RelayCommand AdminWindow_Loaded
        {
            get { return _AdminWindow_Loaded ?? (_AdminWindow_Loaded = new RelayCommand((object e) => AdminWindow_LoadedClick(e))); }
        }


        private RelayCommand _AdminTradeWindowClosing;

        public RelayCommand AdminTradeWindowClosing
        {
            get { return _AdminTradeWindowClosing ?? (_AdminTradeWindowClosing = new RelayCommand((object e) => OnAdminTradeWindow_Closing(e))); }
        }


        private RelayCommand _ShortCut_Escape;

        public RelayCommand ShortCut_Escape
        {
            get
            {
                return _ShortCut_Escape ?? (_ShortCut_Escape = new RelayCommand(
                    (object e) => OnAdminTradeWindow_Closing(e)
                        ));
            }
        }

        private RelayCommand _ShowGrid;

        public RelayCommand ShowGrid
        {
            get
            {
                return _ShowGrid ?? (_ShowGrid = new RelayCommand(
                    (Obj) => ShowGridData()
                    ));
            }
        }

        private RelayCommand _LogData;

        public RelayCommand LogData
        {
            get
            {
                return _LogData ?? (_LogData = new RelayCommand(
                 (Obj) => Logdata()
                 ));
            }

        }

        private RelayCommand _ExportToExcelEquityDebt;
        public RelayCommand ExportToExcelEquityDebt
        {
            get
            {
                return _ExportToExcelEquityDebt ?? (_ExportToExcelEquityDebt = new RelayCommand((object e) => ExportToExcel(1)));
            }
        }

        private RelayCommand _ExportExcelDerivative;
        public RelayCommand ExportExcelDerivative
        {
            get
            {
                return _ExportExcelDerivative ?? (_ExportExcelDerivative = new RelayCommand((object e) => ExportToExcel(2)));
            }
        }

        private RelayCommand _ExportExcelCurrency;
        public RelayCommand ExportExcelCurrency
        {
            get
            {
                return _ExportExcelCurrency ?? (_ExportExcelCurrency = new RelayCommand((object e) => ExportToExcel(3)));
            }
        }

        private RelayCommand _ExportExcelOddLots;
        public RelayCommand ExportExcelOddLots
        {
            get
            {
                return _ExportExcelOddLots ?? (_ExportExcelOddLots = new RelayCommand((object e) => ExportToExcel(4)));
            }
        }

        private RelayCommand _DisplayOption;

        public RelayCommand DisplayOption
        {
            get
            {
                return _DisplayOption ?? (_DisplayOption = new RelayCommand((object e) => OpenDisplayOption()));
            }
        }



        private RelayCommand _DisplayTradeFeed;

        public RelayCommand DisplayTradeFeed
        {
            get
            {
                return _DisplayTradeFeed ?? (_DisplayTradeFeed = new RelayCommand((object e) => OpenTradeFeed()));
            }
        }


        private RelayCommand _myLocationChanged;

        public RelayCommand myLocationChanged
        {
            get
            {
                return _myLocationChanged ?? (_myLocationChanged = new RelayCommand(
                    (object e) => Windows_AdminTradeViewLocationChanged(e)));

            }
        }

        public BrushConverter objBrushConvertor { get; private set; }

        private void Windows_AdminTradeViewLocationChanged(object e)
        {
            if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = (BoltAppSettingsWindowsPosition)CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.ADMINTRADEVIEW != null && oBoltAppSettingsWindowsPosition.ADMINTRADEVIEW.WNDPOSITION != null)
                {
                    oBoltAppSettingsWindowsPosition.ADMINTRADEVIEW.WNDPOSITION.Left = Convert.ToInt32(LeftPosition);
                    oBoltAppSettingsWindowsPosition.ADMINTRADEVIEW.WNDPOSITION.Top = Convert.ToInt32(TopPosition);
                    oBoltAppSettingsWindowsPosition.ADMINTRADEVIEW.WNDPOSITION.Right = Convert.ToInt32(Width);
                    oBoltAppSettingsWindowsPosition.ADMINTRADEVIEW.WNDPOSITION.Down = Convert.ToInt32(Height);
                }
                //CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(@"D:\TWS_DotNetNewStructure\TWS_DOTNETT\CommonFrontEnd\bin\Debug\xml\Users\AppSettings\920200000.xml", "WindowsPosition");
                CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(CommonFrontEnd.SharedMemories.SettingsManager.AppSettingsXmlPath, "WindowsPosition");
            }
        }

        #endregion



        /// <summary>
        /// Hide/Show controls rolewise
        /// </summary>
        public void ToggleDisplay()
        {
            try
            {
                if (!string.IsNullOrEmpty(UtilityLoginDetails.GETInstance.Role))
                {
                    if (UtilityLoginDetails.GETInstance.Role.ToLower() == "admin")
                    {
                        // LocationIdVisible = Visibility.Visible;
                        LocationIDVisibility = "Visible";
                        TradeIDVisibility = "Visible";
                        Title = "Admin Trade View";
                        SaveTradeBtnVisibility = "Visible";
                        TradeFeedBtnVisibilty = "Visible";
                        //ScripCodeVisible = Visibility.Visible;
                        //ScripNameVisible = Visibility.Hidden;
                    }
                    else if (UtilityLoginDetails.GETInstance.Role.ToLower() == "trader")
                    {
                        //LocationIdVisible = Visibility.Collapsed;
                        LocationIDVisibility = "Collapsed";
                        TradeIDVisibility = "Collapsed";
                        Title = "Saudas";
                        SaveTradeBtnVisibility = "hidden";
                        TradeFeedBtnVisibilty = "hidden";
                        //ScripCodeVisible = Visibility.Hidden;
                        //ScripNameVisible = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }




        //private RelayCommand _PipeSeparated;

        //public RelayCommand PipeSeparated
        //{
        //    get
        //    {
        //        return _PipeSeparated ?? (_PipeSeparated = new RelayCommand((object e) => ExecuteMyRequest(TradeViewDataCollection)));
        //    }
        //}


        private void OnAdminTradeWindow_Closing(object e)
        {

            try
            {

                //TODO: Uncomment Windows Position Saudas - Gaurav 03/11/2017
                Saudas_Admin SaudasAdminWindow = System.Windows.Application.Current.Windows.OfType<Saudas_Admin>().FirstOrDefault();

                if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
                {
                    BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = (BoltAppSettingsWindowsPosition)CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                    if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.ADMINTRADEVIEW != null && oBoltAppSettingsWindowsPosition.ADMINTRADEVIEW.WNDPOSITION != null)
                    {
                        oBoltAppSettingsWindowsPosition.ADMINTRADEVIEW.WNDPOSITION.Left = Convert.ToInt32(LeftPosition);
                        oBoltAppSettingsWindowsPosition.ADMINTRADEVIEW.WNDPOSITION.Top = Convert.ToInt32(TopPosition);
                        oBoltAppSettingsWindowsPosition.ADMINTRADEVIEW.WNDPOSITION.Right = Convert.ToInt32(Width);
                        oBoltAppSettingsWindowsPosition.ADMINTRADEVIEW.WNDPOSITION.Down = Convert.ToInt32(Height);
                    }
                    //CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(@"D:\TWS_DotNetNewStructure\TWS_DOTNETT\CommonFrontEnd\bin\Debug\xml\Users\AppSettings\920200000.xml", "WindowsPosition");
                    CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(CommonFrontEnd.SharedMemories.SettingsManager.AppSettingsXmlPath, "WindowsPosition");
                }

                if (SaudasAdminWindow != null)
                {
                    SaudasAdminWindow.Hide();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        private void OpenDisplayOption()
        {
            try
            {
                Option oOption = System.Windows.Application.Current.Windows.OfType<Option>().FirstOrDefault();
                if (oOption != null)
                {
                    oOption.Show();
                    oOption.Focus();
                }
                else
                {
                    oOption = new Option();
                    oOption.Owner = Application.Current.MainWindow;
                    oOption.Activate();
                    oOption.Show();

                }

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }


        private void OpenTradeFeed()
        {
            try
            {
                // Delivery 2 - Jan 15 2018 onwards
                TradeFeed oTradeFeed = System.Windows.Application.Current.Windows.OfType<TradeFeed>().FirstOrDefault();
                if (oTradeFeed != null)
                {
                    oTradeFeed.Show();
                    oTradeFeed.Focus();
                }
                else
                {
                    oTradeFeed = new TradeFeed();
                    oTradeFeed.Owner = Application.Current.MainWindow;
                    oTradeFeed.Activate();
                    oTradeFeed.Show();
                }

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        private void ExportToExcel(int param)
        {
            //if (TradeViewDataCollection.Count > 0)
            //{
            var dataCountList = TradeViewDataCollection?.ToList<TradeUMS>();
            if (param == 1)
            {
                var EqCount = dataCountList.Where(x => (x.Segment == "Equity" || x.Segment == "Debt")).Count();
                if (EqCount == 0)
                {
                    MessageBox.Show("Equity and Debt Trades are not present in Saudas!","Information",MessageBoxButton.OK,MessageBoxImage.Information);
                    return;
                }
            }
            if (param == 2)
            {
                var DerCount = dataCountList.Where(x => x.Segment == "Derivative").Count();
                if (DerCount == 0)
                {
                    MessageBox.Show("Derivative Trades are not present in Saudas!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            if (param == 3)
            {
                var CurCount = dataCountList.Where(x => x.Segment == "Currency").Count();
                if (CurCount == 0)
                {
                    MessageBox.Show("Currency Trades are not present in Saudas!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            if (param == 4)
            {
                var OddLots = dataCountList.Where(x => (x.Segment == "Equity" && x.OrderType == "O")).Count();
                if (OddLots == 0)
                {
                    MessageBox.Show("OddLots Trades are not present in Saudas!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }

            //}
            StreamWriter sw = null;
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV Files (.csv)|*.csv";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                try
                {
                    var data = MemoryManager.TradeMemoryConDict.ToList();


                    using (sw = new StreamWriter(dlg.FileName, false, System.Text.Encoding.UTF8))
                    {
                        if (UtilityLoginDetails.GETInstance.Role.ToLower() == "admin")
                        {
                            sw.Write("Trader ID, B/S, Quantity, ScripCode, Rate, Client, Time, OrdId, TrdID, Client Type, Location ID, Deal Code, CP Code, Status, Yield, Dirty Price");
                            sw.Write(sw.NewLine);

                            foreach (KeyValuePair<long, object> item in data)
                            {
                                var dr = (TradeUMS)item.Value;
                                if (dr.SegmentId == UtilityTradeDetails.GetInstance.SelectedID)
                                {
                                    sw.Write(dr.TraderId + "," + dr.BSFlag + "," + dr.LastQty + "," + dr.ScripCode + "," + dr.Rate + "," + dr.Client + "," + dr.TimeOnly + "," + dr.OrderID + "," + dr.TradeID + "," + dr.ClientType + "," +
                                        dr.SenderLocationID + "," + dr.OppTraderId + "," + dr.CPCode + "," + dr.FreeText3 + "," + dr.Yield + "," + dr.UnderlyingDirtyPrice);

                                    sw.Write(sw.NewLine);
                                }
                            }
                            System.Windows.MessageBox.Show("Admin Trades are Saved in file : " + dlg.FileName.ToString());
                        }
                        else if (UtilityLoginDetails.GETInstance.Role.ToLower() == "trader")
                        {
                            //sw.Write("B/S, Quantity, ScripCode, Rate, Client, Time, OrdId, TrdID, Client Type, Location ID, Deal Code, CP Code, Status, Yield, Dirty Price");
                            //sw.Write(sw.NewLine);

                            //foreach (KeyValuePair<long, object> item in data)
                            //{
                            //    var dr = (TradeUMS)item.Value;
                            //    if (dr.SegmentId == UtilityTradeDetails.GetInstance.SelectedID)
                            //    {
                            //        sw.Write(dr.BSFlag + "," + dr.LastQty + "," + dr.ScripCode + "," + dr.Rate + "," + dr.Client + "," + dr.TimeOnly + "," + dr.OrderID + "," + dr.TradeID + "," + dr.ClientType + "," +
                            //        dr.SenderLocationID + "," + dr.OppTraderId + "," + dr.CPCode + "," + dr.FreeText3 + "," + dr.Yield + "," + dr.UnderlyingDirtyPrice);

                            //        sw.Write(sw.NewLine);
                            //    }
                            //}
                            //System.Windows.MessageBox.Show("Trades Saved in file : " + dlg.FileName.ToString());

                            sw.Write("Scrip Code, Scrip ID, Trader ID, Rate, Quantity, Pending Qty, Reserve Field, Time, Date, Client ID, BorS, Transaction Type, Transaction ID, Client type, ISIN Code, Scrip Group, Settlement Number, OrderTimeStamp");
                            sw.Write(sw.NewLine);

                            foreach (KeyValuePair<long, object> item in data)
                            {
                                var dr = (TradeUMS)item.Value;
                                var settlementNumber = (UtilityLoginDetails.GETInstance.SettlementNo != null) ? String.Format("{0}/{1}", UtilityLoginDetails.GETInstance.SettlementNo?.Split('/')[0], UtilityLoginDetails.GETInstance.SettlementNo?.Split('/')[1]) : "";
                                if (dr.SegmentId == UtilityTradeDetails.GetInstance.SelectedID)
                                {
                                    sw.Write(dr.ScripCode + "," + dr.ScripID + "," + dr.TradeID + "," + dr.Rate + "," + dr.LastQty + "," + dr.LeavesQty + "," + dr.Reserved + "," + dr.TimeOnly + "," + String.Format("{0}/{1}/{2}", dr.Year, dr.Month, dr.Day) + "," +
                                    dr.Client + "," + dr.BSFlag + "," + dr.OrderType + "," + dr.OrderID + "," + dr.ClientType + "," + dr.ISIN + "," + dr.ScripGroup + "," + settlementNumber + "," + dr.OrderTimeStamp);

                                    sw.Write(sw.NewLine);
                                }
                            }
                            System.Windows.MessageBox.Show("Trades Saved in file : " + dlg.FileName.ToString());
                        }
                    }

                }
                catch (IOException oIOException)
                {
                    System.Windows.MessageBox.Show("File is being used by another process", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Error in Exporting data to CSV", "ERROR!!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    //sw.Flush();
                    // sw.Close();
                }
            }
            else
            {
                return;
            }
        }
        //private void ExportToExcelEquityNDebtCMD(object e)
        //{
        //    ExportToExcel(1);
        //}
        //private void ExportToExcelDerivativeCMD(object e)
        //{
        //    ExportToExcel(2);
        //}
        //private void ExportToExcelCurrencyCMD(object e)
        //{
        //    ExportToExcel(3);
        //}
        //private void ExportToExcelOddLotCMD(object e)
        //{
        //    ExportToExcel(4);
        //}
        //public void ExecuteMyCommand()
        //{
        //    StreamWriter sw = null;
        //    try
        //    {
        //        DirectoryInfo directoryPath = new DirectoryInfo(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"User/Saudas.csv")));
        //        if (File.Exists(directoryPath.ToString()))
        //        {
        //            File.Delete(directoryPath.ToString());
        //        }
        //        List<DataGridColumn> objColumnHeader = new List<DataGridColumn>();
        //        objColumnHeader = System.Windows.Application.Current.Windows.OfType<Saudas_Admin>().FirstOrDefault().dataGridView1.Columns.ToList();
        //        //  System.IO.FileStream directory = new System.IO.FileStream(Path.Combine(System.Environment.CurrentDirectory, @"xml/Users/TouchLineGrid.csv"),FileMode.Create);

        //        sw = new StreamWriter(directoryPath.ToString());
        //        //for (int i = 0; i < objColumnHeader.Count; i++)
        //        //{
        //        //    if(objColumnHeader[i].Visibility==Visibility.Visible)
        //        //    sw.Write(objColumnHeader[i].Header);
        //        //}
        //        //   sw.Write("Script Id,Scrip Code,Buy Quantity,Buy Rate,Sell Quantity,Sell Rate,LTP,NoOfBidBuy,NoOfBidSell,OpenRateL,CloseRate,HighRate,LowRate,TotBuyQty,TotSellQty,WtAvgRate,52H,52L,CTValue,CTVolume");
        //        //sw.Write("Test");
        //        sw.Write(sw.NewLine);
        //        DataGrid objDataGrid = System.Windows.Application.Current.Windows.OfType<Saudas_Admin>().FirstOrDefault().dataGridView1;
        //        //objDataGrid
        //        objDataGrid.SelectAllCells();


        //        objDataGrid.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
        //        Clipboard.Clear();
        //        ApplicationCommands.Copy.Execute(null, objDataGrid);
        //        objDataGrid.UnselectAllCells();

        //        String result = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
        //        try
        //        {
        //            sw.Write(result);


        //            // Process.Start(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"xml/Users/TouchLineGrid.csv")));
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }

        //        System.Windows.MessageBox.Show("Successfully exported data to Excel Sheet at: " + directoryPath.ToString(), "Export Data to Excel", MessageBoxButton.OK, MessageBoxImage.None);
        //    }
        //    catch (Exception e)
        //    {
        //        ExceptionUtility.LogError(e);
        //        System.Windows.MessageBox.Show("Error in Exporting data to Excel", "Export Data to Excel", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //    finally
        //    {
        //        if (sw != null)
        //        {
        //            sw.Flush();
        //            sw.Close();
        //        }
        //    }

        //}


        private void ShowGridData()
        {
            //TradeViewDataCollection.Clear();

            //if (MemoryManager.TradeMemoryConDict != null && MemoryManager.TradeMemoryConDict.Count > 0)
            //{
            //    foreach (var item in MemoryManager.TradeMemoryConDict)
            //    {
            //       // TradeViewDataCollection.Add(ProcessData(item.Value));
            //    }
            //}
        }


        private void AdminWindow_LoadedClick(object e)
        {
            try
            {
                TradeViewDataCollection = NetPositionMemory.TraderTradeDataCollection;
                NotifyPropertyChanged("TradeViewDataCollection");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        private void Logdata()
        {
            //CommonFrontEnd.Common.ExceptionUtility.LogException();
        }


        private RelayCommand<EventArgs> scrollCommand;

        //public RelayCommand<EventArgs> ScrollCommand
        //{
        //    get { return scrollCommand ?? (scrollCommand = new RelayCommand<EventArgs>(OnScrollChangedEvent)); }
        //    set { scrollCommand = value; }
        //}
        //private void OnScrollChangedEvent(object e)
        //{
        //    TradeViewDataCollection.Clear();
        //    ScrollChangedEventArgs scrollChangedEventArgs = e as ScrollChangedEventArgs;
        //    if (scrollChangedEventArgs != null)
        //    {
        //        //objListOfVisibleRecords = new ConcurrentBag<int>();
        //        if (NetPositionMemory.TraderTradeDataCollection != null && NetPositionMemory.TraderTradeDataCollection.Count > 0)
        //        {
        //            int firstIndex = (int)scrollChangedEventArgs.VerticalOffset;
        //            int lastIndex = Math.Min(NetPositionMemory.TraderTradeDataCollection.Count - 1, (int)(scrollChangedEventArgs.VerticalOffset + scrollChangedEventArgs.ViewportHeight));


        //                ParallelOptions parallelOptions = new ParallelOptions
        //                {
        //                    MaxDegreeOfParallelism = 1
        //                };
        //            Parallel.ForEach(NetPositionMemory.TraderTradeDataCollection, parallelOptions, item =>
        //            {
        //                if (item != null)
        //                {
        //                    int index = NetPositionMemory.TraderTradeDataCollection.IndexOf(item);
        //                    if (index <= firstIndex && index >= lastIndex)
        //                    {
        //                        TradeViewDataCollection.Add(item);
        //                    }
        //                }
        //            }
        //            );
        //            NotifyStaticPropertyChanged("TradeViewDataCollection");
        //        }
        //    }
        //}

        public AdminTradeViewVM()
        {
            try
            {

                //TODO: Uncomment Windows Position Saudas - Gaurav 03/11/2017
                if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
                {
                    CommonFrontEnd.Model.BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = new Model.BoltAppSettingsWindowsPosition();
                    oBoltAppSettingsWindowsPosition = (CommonFrontEnd.Model.BoltAppSettingsWindowsPosition)CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                    if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.ADMINTRADEVIEW != null && oBoltAppSettingsWindowsPosition.ADMINTRADEVIEW.WNDPOSITION != null)
                    {
                        Height = oBoltAppSettingsWindowsPosition.ADMINTRADEVIEW.WNDPOSITION.Down.ToString();
                        TopPosition = oBoltAppSettingsWindowsPosition.ADMINTRADEVIEW.WNDPOSITION.Top.ToString();
                        LeftPosition = oBoltAppSettingsWindowsPosition.ADMINTRADEVIEW.WNDPOSITION.Left.ToString();
                        Width = oBoltAppSettingsWindowsPosition.ADMINTRADEVIEW.WNDPOSITION.Right.ToString();
                    }
                }

                TradeViewDataCollection = new ObservableCollection<TradeUMS>();
                TradeViewDataCollectionCopy = new ObservableCollection<SaudasUMSModel>();
                TradeViewDataCollection = NetPositionMemory.TraderTradeDataCollection;

                MemoryManager.OnTradeUMSRecieved += ReceiveUmsData;

                objBrushConvertor = new BrushConverter();



                ToggleDisplay();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            ColourProfilingVM.OnColorSettingChange += SetColor;
            SetColor();
            #region Commented


            //MemoryManager.NetPositionScripsDict = new ConcurrentDictionary<string, object>();
            //SaudasUMSModel td = new SaudasUMSModel();
            //td.PartitionID = 1;
            //Messenger.Default.Register<SaudasUMSModel>(this, ReceiveUmsData);
            //  NotifyPropertyChanged("TradeViewDataCollection");
            //TradeViewDataCollection.OrderByDescending(x => x.Time);
            //MemoryManager.OnTradeUMSRecieved += ExecuteAD2TR;
            //NetPositionMemory.AD2TRDataUpdation += ExecuteAD2TR;
            //DisplayCount = CommonFrontEnd.SharedMemories.MemoryManager.CountTrade;

            #endregion
        }

        private void SetColor()
        {
            DataGridBgColor = MainWindowVM.ParserDefaultCPR.GetSetting("TRADES", "TradesBackGround") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("TRADES", "TradesBackGround")) as SolidColorBrush : null;
            BuyForegroundColor = MainWindowVM.ParserDefaultCPR.GetSetting("TRADES", "BuyTrade") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("TRADES", "BuyTrade")) as SolidColorBrush : null;
            SellForegroundColor = MainWindowVM.ParserDefaultCPR.GetSetting("TRADES", "SellTrade") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("TRADES", "SellTrade")) as SolidColorBrush : null;

        }

        //private void EscapeUsingUserControl(object e)
        //{
        //    Saudas_Admin mainwindow = e as Saudas_Admin;
        //    mainwindow.Close();
        //}

        #region Commented
        //public static SaudasUMSModel ProcessData(object obj)
        //{
        //    TradeUMS objumsmodel = obj as TradeUMS;
        //    SaudasUMSModel oSaudasUMSModel = new SaudasUMSModel();

        //    oSaudasUMSModel.BSFlag = Enum.GetName(typeof(Enumerations.Side), objumsmodel.Side);

        //    oSaudasUMSModel.Client = objumsmodel.FreeText1.Trim();

        //    oSaudasUMSModel.ScripID = MasterSharedMemory.objMastertxtDic.Where(x => x.Value.ScripCode == (objumsmodel.SecurityID)).Select(x => x.Value.ScripId).FirstOrDefault();

        //    oSaudasUMSModel.ClientType = Enum.GetName(typeof(Enumerations.AccountType), objumsmodel.AccountType);

        //    oSaudasUMSModel.CPCd = objumsmodel.CPCode;

        //    oSaudasUMSModel.DealCode = objumsmodel.TradeID;

        //    oSaudasUMSModel.LocationId = objumsmodel.SenderLocationID;

        //    oSaudasUMSModel.OrderId = objumsmodel.OrderID;

        //    oSaudasUMSModel.Qty = objumsmodel.LastQty;

        //    oSaudasUMSModel.Rate = CommonFunctions.DisplayInDecimalFormat(objumsmodel.LastPx, 100000000d, 2);//Convert.ToDecimal(objumsmodel.LastPx / 100000000d);

        //    oSaudasUMSModel.ScripCode = objumsmodel.SecurityID;

        //    oSaudasUMSModel.ScripName = CommonFrontEnd.Common.CommonFunctions.GetScripName(objumsmodel.SecurityID);

        //    oSaudasUMSModel.Status = objumsmodel.FreeText3;

        //    oSaudasUMSModel.Time = String.Format("{0:00}:{1:00}:{2:00}", objumsmodel.Hour, objumsmodel.Minute, objumsmodel.Second);//CommonFrontEnd.Common.Converter.UnixTimeStampToDateTime(objumsmodel.TransactTime).ToString();

        //    oSaudasUMSModel.TradeId = objumsmodel.SideTradeID;
        //    oSaudasUMSModel.TraderId = Convert.ToInt32(objumsmodel.RootPartyIDSessionID.ToString().Remove(0, 6));

        //    if (objumsmodel.UnderlyingDirtyPrice == long.MinValue || objumsmodel.UnderlyingDirtyPrice == long.MaxValue)
        //    {
        //        oSaudasUMSModel.DirtyPrice = 0;
        //    }
        //    else
        //    {
        //        oSaudasUMSModel.DirtyPrice = objumsmodel.UnderlyingDirtyPrice;
        //    }
        //    if (objumsmodel.Yield == long.MinValue || objumsmodel.Yield == long.MaxValue)
        //    {
        //        oSaudasUMSModel.Yield = 0;
        //    }
        //    else
        //    {
        //        oSaudasUMSModel.Yield = objumsmodel.Yield;
        //    }

        //    oSaudasUMSModel.ElaspedTime = ElapsedTime;

        //    // Title = "Admin trade View " + TradeViewDataCollection.Count.ToString();

        //    //MemoryManager.NetPositionScripsDict.TryAdd(oSaudasUMSModel.CPCd + "_" + oSaudasUMSModel.ScripCode, ProcessNetPosition(objumsmodel));

        //    //ProcessNetPosition(objumsmodel);

        //    return oSaudasUMSModel;


        //}
        #endregion

        public void ReceiveUmsData(long count)
        {
            //Title = "Admin Trade View";
            //NotifyStaticPropertyChanged("Title");// count.ToString();
            TradeCount = count;
        }


        public static void ExecuteAD2TR(TradeUMS oTradeUMS)
        {
            PrintCount++;

            try
            {
                if (PrintCount % 50000 == 0)
                    SeqCount++;

                CreateNewFile(oTradeUMS);

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
                //System.Windows.MessageBox.Show("Error in Exporting data to AD2TR");
            }
            finally
            {
                if (writer != null)
                {
                    writer.Flush();
                    writer.Close();
                }
            };


        }


        private static void CreateNewFile(TradeUMS oTradeUMS)
        {
            try
            {
                //            if (App.MemberId != null && App.TraderId != null)
                folderpath = string.Format("{0}AD2TR{1}{2:00}.csv", UtilityLoginDetails.GETInstance.MemberId, CommonFunctions.GetDate().Day.ToString("00"), SeqCount);

                string folderpath1 = Path.Combine(directoryAD2TR.ToString(), folderpath);
                FileStream fsrw = null;
                if (!Directory.Exists(directoryAD2TR.ToString()))
                {
                    Directory.CreateDirectory(directoryAD2TR.ToString());
                }
                if (UtilityLoginDetails.GETInstance.Role.ToString() == "Admin")// Role: Admin
                {
                    if (!File.Exists(folderpath1))
                    // Create a file to write to.
                    {
                        writer = File.CreateText(folderpath1);
                        writer.Write("MemId, TdrId, SCd, ScId, Rt, Qty, PendQty, Reserve, Time, Date, ClId, TrnId, TrnType, Buy/Sell, TradeId, Client Type, ISINCd, ScrGrp, SettNo, OrdTimestamp, RectFlag, LocationId, Segment, CpCode, Yield, Dirty Price, Decimal Locator");
                        writer.Write(writer.NewLine);
                    }
                    else
                        writer = File.AppendText(folderpath1);

                    writer.WriteLine(UtilityLoginDetails.GETInstance.MemberId + "," + oTradeUMS.TraderId + "," + oTradeUMS.ScripCode + "," + oTradeUMS.ScripID + "," + oTradeUMS.Rate + "," + oTradeUMS.LastQty + "," + oTradeUMS.OppMemberId + "," +
                            oTradeUMS.Reserved + "," + oTradeUMS.TimeOnly + "," + oTradeUMS.DateOnly + "," + oTradeUMS.Client + "," + oTradeUMS.OrderID + "," + oTradeUMS.OrderType + "," + oTradeUMS.BSFlag + "," + oTradeUMS.TradeID + "," +
                            oTradeUMS.ClientType + "," + oTradeUMS.ISIN + "," + oTradeUMS.ScripGroup + "," + oTradeUMS.SettlNo[0] /*string.Join("/", oTradeUMS.SettlNo)*/ + "," + oTradeUMS.OrderTimeStamp1 + "," + "11" + "," + oTradeUMS.SenderLocationID + "," + oTradeUMS.Market
                            + "," + oTradeUMS.CPCode + "," + oTradeUMS.Yield + "," + oTradeUMS.UnderlyingDirtyPrice + "," + oTradeUMS.DecimalLocator);
                }
                else// Role Trader
                {
                    if (!File.Exists(folderpath1))
                    // Create a file to write to.
                    {
                        writer = File.CreateText(folderpath1);
                        writer.Write("MemId, TdrId, SCd, ScId, Rt, Qty, PendQty, Reserve, Time, Date, ClId, TrnId, TrnType, Buy/Sell, TradeId, Client Type, ISINCd, ScrGrp, SettNo, OrdTimestamp, RectFlag, Segment, CpCode, Yield, Dirty Price, Decimal Locator");
                        writer.Write(writer.NewLine);
                    }
                    else
                        writer = File.AppendText(folderpath1);

                    writer.WriteLine(UtilityLoginDetails.GETInstance.MemberId + "," + oTradeUMS.TraderId + "," + oTradeUMS.ScripCode + "," + oTradeUMS.ScripID + "," + oTradeUMS.Rate + "," + oTradeUMS.LastQty + "," + oTradeUMS.OppMemberId + "," +
                            oTradeUMS.Reserved + "," + oTradeUMS.TimeOnly + "," + oTradeUMS.DateOnly + "," + oTradeUMS.Client + "," + oTradeUMS.OrderID + "," + oTradeUMS.OrderType + "," + oTradeUMS.BSFlag + "," + oTradeUMS.TradeID + "," +
                            oTradeUMS.ClientType + "," + oTradeUMS.ISIN + "," + oTradeUMS.ScripGroup + "," + oTradeUMS.SettlNo[0] /*string.Join("/", oTradeUMS.SettlNo)*/ + "," + oTradeUMS.OrderTimeStamp1 + "," + "11" + "," + oTradeUMS.Market
                            + "," + oTradeUMS.CPCode + "," + oTradeUMS.Yield + "," + oTradeUMS.UnderlyingDirtyPrice + "," + oTradeUMS.DecimalLocator);
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }


        //public event PropertyChangedEventHandler PropertyChanged;

        //public void NotifyPropertyChanged(String propertyName = "")
        //{
        //    PropertyChangedEventHandler handler = this.PropertyChanged;

        //    if (handler != null)
        //    {

        //        var e = new PropertyChangedEventArgs(propertyName);

        //        this.PropertyChanged(this, e);

        //    }

        //}

    }
#endif
}


