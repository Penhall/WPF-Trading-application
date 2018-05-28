using CommonFrontEnd.Common;
using CommonFrontEnd.CSVReader;
using CommonFrontEnd.Global;
using CommonFrontEnd.Common;
using CommonFrontEnd.Model;
using CommonFrontEnd.Processor;
using CommonFrontEnd.SharedMemories;
using SubscribeList;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using CommonFrontEnd.View.UserControls;
using CommonFrontEnd.ViewModel.Order;
using CommonFrontEnd.View;
using System.Windows.Controls;

namespace CommonFrontEnd.ViewModel
{

    public class MarketWatchVM : INotifyPropertyChanged
    {
#if TWS
        private static ReadFromCSV ObjCommon = new ReadFromCSV();
        static DirectoryInfo masterPath = new DirectoryInfo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"Profile/MarketWatch/")));
        public static int DecimalPnt { get; set; } 

        private string _LeftPosition = "71";

        public string LeftPosition
        {
            get { return _LeftPosition; }
            set { _LeftPosition = value; NotifyPropertyChanged("LeftPosition"); }
        }

        private string _TopPosition = "17";

        public string TopPosition
        {
            get { return _TopPosition; }
            set { _TopPosition = value; NotifyPropertyChanged("TopPosition"); }
        }

        private string _Width = "1232";

        public string ClassicWidth
        {
            get { return _Width; }
            set { _Width = value; NotifyPropertyChanged("ClassicWidth"); }
        }


        private string _Height = "729";

        public string ClassicHeight
        {
            get { return _Height; }
            set { _Height = value; NotifyPropertyChanged("ClassicHeight"); }
        }
        private RelayCommand _ShortCut_EnterScrips;

        public RelayCommand ShortCut_EnterScrips
        {
            get
            {
                return _ShortCut_EnterScrips ?? (_ShortCut_EnterScrips = new RelayCommand(
                    (object e) => AddScripsUsingUserControl()
                        ));
            }
        }
        private static string _CurrentTabSlected;

        public static string CurrentTabSlected
        {
            get
            {
                if (System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault().SnPSensexTab.SelectedItem != null)
                    return ((HeaderedContentControl)System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault().SnPSensexTab.SelectedItem).Header.ToString();
                else
                    return string.Empty;
            }
            //set { _CurrentTabSlected = value; NotifyStaticPropertyChanged("CurrentTabSlected"); }
        }

        private void AddScripsUsingUserControl()
        {
            if (MarketsCombo.Contains(CurrentTabSlected))
                System.Windows.MessageBox.Show("Scrip Addition Funtionality Not Available", "Information", MessageBoxButton.OK, MessageBoxImage.Error);

            else
            {
                OrderEntryUC_VM.OnScripIDOrCodeChange = null;
                OrderEntryUC_VM.TouchLineInsert = true;
                UserControlWindow.Owner = System.Windows.Application.Current.MainWindow;
                UserControlWindow.ShowDialog();
            }
        }

        private RelayCommand _DataGridDoubleClick;

        public RelayCommand DataGridDoubleClick
        {
            get
            {
                return _DataGridDoubleClick ?? (_DataGridDoubleClick = new RelayCommand(
                    (object e) => DataGrid_DoubleClick()));
            }
        }
        private static ObservableCollection<MarketWatchModel> objTouchlineDataCollection;
        public static ObservableCollection<MarketWatchModel> ObjTouchlineDataCollection
        {
            get { return objTouchlineDataCollection; }
            set { objTouchlineDataCollection = value; NotifyStaticPropertyChanged("ObjTouchlineDataCollection"); }
        }
        private static ObservableCollection<string> _ScripProfilingCombo;
        public static ObservableCollection<string> ScripProfilingCombo
        {
            get { return _ScripProfilingCombo; }
            set { _ScripProfilingCombo = value; NotifyStaticPropertyChanged("ScripProfilingCombo"); }
        }
        private static ObservableCollection<MarketWatchModel> _indicesDataCollection;
        public static ObservableCollection<MarketWatchModel> IndicesDataCollection
        {
            get { return _indicesDataCollection; }
            set { _indicesDataCollection = value; NotifyStaticPropertyChanged("IndicesDataCollection"); }
        }
        private static List<string> marketMoversCombo;
        public static List<string> MarketMoversCombo
        {
            get { return marketMoversCombo; }
            set { marketMoversCombo = value; NotifyStaticPropertyChanged("MarketMoversCombo"); }
        }
        private static ObservableCollection<string> marketsCombo;
        public static ObservableCollection<string> MarketsCombo
        {
            get { return marketsCombo; }
            set { marketsCombo = value; NotifyStaticPropertyChanged("MarketsCombo"); }
        }

        private static List<string> groupCombo;
        public static List<string> GroupCombo
        {
            get { return groupCombo; }
            set { groupCombo = value; NotifyStaticPropertyChanged("GroupCombo"); }
        }
        private static List<MarketWatchModel> _SearchTemplist;
        public static List<MarketWatchModel> SearchTemplist
        {
            get { return _SearchTemplist; }
            set { _SearchTemplist = value; }
        }
        private static ConcurrentBag<int> objListOfVisibleRecordsList;
   
        public delegate void ShowVisibleRecordsEventHandler(ConcurrentBag<int> objListOfVisibleRecords);
        public static event ShowVisibleRecordsEventHandler OnScrollUpdateVisibleItemsOnly;

        static Window UserControlWindow = new Window();

        private static string _GroupComboSelectedItem;

        public static string GroupComboSelectedItem
        {
            get { return _GroupComboSelectedItem; }
            set
            {
                _GroupComboSelectedItem = value;
                NotifyStaticPropertyChanged("GroupComboSelectedItem");
                //OnChangeOfMarketMovers();
            }
        }

        private static string _MarketsComboSelectedItem;

        public static string MarketsComboSelectedItem
        {
            get { return _MarketsComboSelectedItem; }
            set
            {
                _MarketsComboSelectedItem = value;
                NotifyStaticPropertyChanged("MarketsComboSelectedItem");
                // UpdateDataGrid();
                //OnChangeOfMarkets();
            }
        }
        private static string _ScripProfComboSelectedItem;

        public static string ScripProfComboSelectedItem
        {
            get { return _ScripProfComboSelectedItem; }
            set
            {
                _ScripProfComboSelectedItem = value;
                NotifyStaticPropertyChanged("ScripProfComboSelectedItem");
            }
        }


        public MarketWatchVM()
        {
            if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = new BoltAppSettingsWindowsPosition();
                oBoltAppSettingsWindowsPosition = (BoltAppSettingsWindowsPosition)ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.ClassicTouchLine != null && oBoltAppSettingsWindowsPosition.ClassicTouchLine.WNDPOSITION != null)
                {
                    ClassicHeight = oBoltAppSettingsWindowsPosition.ClassicTouchLine.WNDPOSITION.Down.ToString();
                    TopPosition = oBoltAppSettingsWindowsPosition.ClassicTouchLine.WNDPOSITION.Top.ToString();
                    LeftPosition = oBoltAppSettingsWindowsPosition.ClassicTouchLine.WNDPOSITION.Left.ToString();
                    ClassicWidth = oBoltAppSettingsWindowsPosition.ClassicTouchLine.WNDPOSITION.Right.ToString();
                }
            }
       
            List<int> objScripCodes = CookTouchLineData(null);
          //  BroadCastProcessor.ScripCodesConfigured(objScripCodes);
          //  TitleTouchLine = "TouchLine - " + CurrentTabSlected + "-" + ObjTouchlineDataCollection.Count;
        }
        private static List<int> CookTouchLineData(string name)
        {
            List<int> objScripCodeFromSettingsSendLst = new List<int>();
            //int ScripCode = 0;
            string[] strArray;
            try
            {
                if (name == null)
                {
                    strArray = ObjCommon.ReaDFromCSV();
                }
                else
                {
                    string UserDefinedPath = Path.Combine(masterPath.ToString(), name);
                    strArray = File.ReadAllLines(UserDefinedPath + ".csv");
                }
                SubscribeScripMemory.objSubscribeScrip.Clear();
                SubscribeList.SubscribeScrip s = new SubscribeScrip();
                //strArray.GetLength(0);
                for (int i = 0; i < strArray.GetLength(0); i++)
                {
                    string[] strArray2 = strArray[i].Split(new char[] { ',' });
                    if (strArray2.Length == 2)
                        objScripCodeFromSettingsSendLst.Add(Convert.ToInt32(strArray2[1]));
                    else
                        objScripCodeFromSettingsSendLst.Add(Convert.ToInt32(CommonFunctions.GetScripCode(strArray2[0])));
                    MarketWatchModel item = new MarketWatchModel();
                    ScripDetails objscrip = new ScripDetails();
                    item.Index = i;
                    item.ScriptId1 = strArray2[0];
                    if (strArray2.Length == 2)
                        item.Scriptcode1 = Convert.ToInt32(strArray2[1]);
                    else
                        item.Scriptcode1 = Convert.ToInt32(CommonFunctions.GetScripCode(strArray2[0]));
                    DecimalPnt = CommonFunctions.GetDecimal(item.Scriptcode1);
                    //DecimalPoint =  CommonFunctions.GetDecimal(item.Scriptcode1),
                    //objscrip = BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == item.Scriptcode1).Select(x => x.Value).FirstOrDefault();                    
                    s.ScripCode_l = item.Scriptcode1;
                    s.UpdateFlag_s = 1;
                    if (SubscribeScripMemory.objSubscribeScrip.ContainsKey(s.ScripCode_l))
                    { }
                    else
                        SubscribeScripMemory.objSubscribeScrip.TryAdd(s.ScripCode_l, s);
                    BroadcastReceiver.ScripDetails Br = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == item.Scriptcode1).Select(x => x.Value).FirstOrDefault();
                    objscrip = MainWindowVM.UpdateScripDataFromMemory(Br);
                    objscrip.ScriptCode = item.Scriptcode1;
                    Initialize_Fields(objscrip, ref item, DecimalPnt, item.Scriptcode1);
                    ObjTouchlineDataCollection.Add(item);
                    objBroadCastProcessor_OnBroadCastRecievedNew(objscrip);
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
            SearchTemplist = ObjTouchlineDataCollection.Cast<MarketWatchModel>().ToList();
            return objScripCodeFromSettingsSendLst;
        }
        public static void Initialize()
        {
           
          //ScripProfilingVM.RefreshTouchLine += RefreshUserDefinedDropDowns;
          //OrderEntryUC_VM.OnScripAddTouchLine += AddNewScrip;
            //create the object of observable collection
            objTouchlineDataCollection = new ObservableCollection<MarketWatchModel>();
            SearchTemplist = new List<MarketWatchModel>();
            IndicesDataCollection = new ObservableCollection<MarketWatchModel>();
            MarketMoversCombo = new List<string>();
            MarketsCombo = new ObservableCollection<string>();
            GroupCombo = new List<string>();
            ScripProfilingCombo = new ObservableCollection<string>();
            //PopulatingDropDowns();
            //PopulatingScripProfilingDropDown();
            PopulateScripInsertControl();
            //Add Scripcodes to observable collection
            //Messenger.Default.Register<long>(this, UpdateValues);
            objListOfVisibleRecordsList = new ConcurrentBag<int>();
            //BindingOperations.EnableCollectionSynchronization(objTouchlineDataCollection, ObjectCW);
            OnScrollUpdateVisibleItemsOnly += new ShowVisibleRecordsEventHandler(broadcastReciever_OnScrollUpdateVisibleItemsOnly);
            ObjCommon.ReadVariables();
        }
        private static void broadcastReciever_OnScrollUpdateVisibleItemsOnly(ConcurrentBag<int> objListOfVisibleRecords)
        {
            objListOfVisibleRecordsList = new ConcurrentBag<int>();
            objListOfVisibleRecordsList = objListOfVisibleRecords;
            ParallelOptions parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = 10
            };
            Parallel.ForEach<MarketWatchModel>(ObjTouchlineDataCollection, parallelOptions, delegate (MarketWatchModel item)
            {
                if (item != null)
                {
                    if (!objListOfVisibleRecordsList.Contains(item.Scriptcode1))
                    {
                        item.IsVisible = true;
                    }
                    else
                    {
                        item.IsVisible = false;
                    }
                }
            });
        }

        private static void PopulateScripInsertControl()
        {
            //UserControlWindow = new Window();
            UserControlWindow.Title = "Add Scrips";
            //OrderEntryUC oOrderEntryUC = System.Windows.Application.Current.Windows.OfType<OrderEntryUC>().FirstOrDefault();
            //if (oOrderEntryUC != null)
            //UserControlWindow.Content = oOrderEntryUC;
            //else
            UserControlWindow.Content = new OrderEntryUC();
            UserControlWindow.Height = 78;
            UserControlWindow.Width = 780;
            UserControlWindow.ResizeMode = ResizeMode.NoResize;
            UserControlWindow.WindowStyle = WindowStyle.None;
            UserControlWindow.AllowsTransparency = false;
            UserControlWindow.BorderThickness = new Thickness(1);
            UserControlWindow.BorderBrush = System.Windows.Media.Brushes.Black;
            UserControlWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //BestFiveVM.OnScripChangeuserControl = null;
            UserControlWindow.ShowInTaskbar = false;
         //   UserControlWindow.Closing += UserControlWindow_Closing;
        }
        private static void PopulatingDropDowns()
        {
          
            MarketsCombo.Add("-" + MarketWatchModel.Markets.Pre_Defined.ToString() + "-");
            foreach (var item in MasterSharedMemory.objSpnIndicesDic.Values.GroupBy(x => x.ExistingShortName_ca))
            {
                MarketsCombo.Add(item.FirstOrDefault().ExistingShortName_ca.ToString());
            }

        
            MarketsComboSelectedItem = MarketsCombo[0];

            GroupCombo.Add("-" + MarketWatchModel.Group.Group.ToString() + "-");
            GroupCombo.Add(MarketWatchModel.Group.ALLEQ.ToString());
            GroupCombo.Add(MarketWatchModel.Group.A.ToString());
            GroupCombo.Add(MarketWatchModel.Group.B.ToString());
            GroupCombo.Add(MarketWatchModel.Group.DF.ToString());
            GroupCombo.Add(MarketWatchModel.Group.T.ToString());
            GroupCombo.Add(MarketWatchModel.Group.E.ToString());
            GroupCombo.Add(MarketWatchModel.Group.F.ToString());
            GroupCombo.Add(MarketWatchModel.Group.FC.ToString());
            GroupCombo.Add(MarketWatchModel.Group.G.ToString());
            GroupCombo.Add(MarketWatchModel.Group.GC.ToString());
            GroupCombo.Add(MarketWatchModel.Group.M.ToString());
            GroupCombo.Add(MarketWatchModel.Group.MT.ToString());
            GroupCombo.Add(MarketWatchModel.Group.R.ToString());
            GroupCombo.Add(MarketWatchModel.Group.Z.ToString());
            GroupCombo.Add(MarketWatchModel.Group.PCAS.ToString());
            GroupCombo.Add(MarketWatchModel.Group.CD.ToString());
            GroupComboSelectedItem = MarketWatchModel.Group.ALLEQ.ToString();
        }
        private static void RefreshUserDefinedDropDowns(bool obj)
        {
            //ScripProfilingCombo = new List<string>();
            PopulatingScripProfilingDropDown();
        }
        private static void PopulatingScripProfilingDropDown()
        {
            try
            {
                // string masterPath = @"C:\test";
                string filePattern = "*.csv";
                ScripProfilingCombo.Clear();
                var coFiles = from fullFilename
                     in Directory.EnumerateFiles(masterPath.ToString(), filePattern)
                              select Path.GetFileNameWithoutExtension(fullFilename);
                ScripProfilingCombo.Add("-Userdefined Marketwatch-");
                foreach (var item in coFiles)
                    ScripProfilingCombo.Add(item);

                ScripProfComboSelectedItem = ScripProfilingCombo[0];
                //NotifyStaticPropertyChanged("ScripProfilingCombo");
                //NotifyStaticPropertyChanged("ScripProfComboSelectedItem");
            }
            catch (Exception e)
            {
                ExceptionUtility.LogError(e);
                throw;
            }

        }
        public static void objBroadCastProcessor_OnBroadCastRecievedNew(ScripDetails objScripDetails)
        {
            try
            {
                int scripCode = objScripDetails.ScriptCode;
                //ScripDetails objScripDetails = BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == scripCode).Select(x => x.Value).FirstOrDefault();
                if (objScripDetails != null && ObjTouchlineDataCollection.Count > 0)
                {
                    List<int> list = new List<int>();
                    list = ObjTouchlineDataCollection.Where(i => i.Scriptcode1 == scripCode).Select(x => ObjTouchlineDataCollection.IndexOf(x)).ToList();
                    
                    for (int i = 0; i < list.Count; i++)
                    {
                                             UpdateGrid(list[i], objScripDetails);
                      
                    }
                }
               // BestFiveVM.objBroadCastProcessor_OnBroadCastRecieved(scripCode);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
                return;
            }
        }

        private static void Initialize_Fields(ScripDetails objscrip, ref MarketWatchModel item, int DecimalPoint, int ScripCode)
        {
            try
            {
                if (objscrip != null)
                {
                    item.BuyQualtity1 = objscrip.BuyQtyL;
                    item.BuyRate1 = objscrip.BuyRateL / Math.Pow(10, DecimalPnt);
                    item.SellQuantity1 = objscrip.SellQtyL;
                    item.SellRate1 = objscrip.SellRateL / Math.Pow(10, DecimalPnt);
                    item.LTP1 = objscrip.lastTradeRateL / Math.Pow(10, DecimalPnt);
                    item.NoofBidBuy1 = objscrip.NoOfBidBuyL;
                    item.NoOfBidSell1 = objscrip.NoOfBidSellL;
                    item.OpenRateL = objscrip.openRateL / Math.Pow(10, DecimalPnt);
                    item.CloseRateL = objscrip.closeRateL / Math.Pow(10, DecimalPnt);
                    item.HighRateL = objscrip.highRateL / Math.Pow(10, DecimalPnt);
                    item.LowRateL = objscrip.lowRateL / Math.Pow(10, DecimalPnt);
                    item.TotBuyQtyL = objscrip.totBuyQtyL;
                    item.TotSellQtyL = objscrip.totSellQtyL;
                    item.WtAvgRateL = objscrip.wtAvgRateL / Math.Pow(10, DecimalPnt);
                    item.CtValue = objscrip.TrdValue / Math.Pow(10, DecimalPnt);
                    item.CtVolume = objscrip.TrdVolume;
                }
                else
                {
                    item.BuyQualtity1 = 0;
                    item.BuyRate1 = 0;
                    item.SellQuantity1 = 0;
                    item.SellRate1 = 0;
                    item.LTP1 = 0;
                    item.NoofBidBuy1 = 0;
                    item.NoOfBidSell1 = 0;
                    item.OpenRateL = 0;
                    item.CloseRateL = 0;
                    item.HighRateL = 0;
                    item.LowRateL = 0;
                    item.TotBuyQtyL = 0;
                    item.TotSellQtyL = 0;
                    item.WtAvgRateL = 0;
                    item.CtVolume = 0;
                    item.CtValue = 0;
                }
                item.IsVisible = true;
                //if (MasterSharedMemory.objDicDP.ContainsKey(ScripCode))
                //{
                //    item.FiftyTwoHigh = MasterSharedMemory.objDicDP[ScripCode].WeeksHighprice / Math.Pow(10, DecimalPnt);
                //    item.FiftyTwoLow = MasterSharedMemory.objDicDP[ScripCode].WeeksLowprice / Math.Pow(10, DecimalPnt);
                //}
                //else
                {
                    item.FiftyTwoHigh = 0;
                    item.FiftyTwoLow = 0;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void DataGrid_DoubleClick()
        {
            //if (SelectedItem != null && SelectedItem.Scriptcode1 != 0)
            //{
            //    string ScripID = string.Empty;

            //    BestFiveMarketPicture mainwindow = System.Windows.Application.Current.Windows.OfType<BestFiveMarketPicture>().FirstOrDefault();
            //    if (mainwindow == null)
            //        mainwindow = new BestFiveMarketPicture();
            //    //ScripDetails objScripDetails = BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == SelectedItem.Scriptcode1).Select(x => x.Value).FirstOrDefault();
            //    //if (objScripDetails == null)
            //    //{
            //    //    objScripDetails = new ScripDetails();
            //    //    objScripDetails.ScriptCode = SelectedItem.Scriptcode1;
            //    //}
            //    BroadcastReceiver.ScripDetails Br = BroadcastMaster.BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == SelectedItem.Scriptcode1).Select(x => x.Value).FirstOrDefault();
            //    ScripDetails objScripDetails = new ControllerModel.ScripDetails();
            //    objScripDetails = TwsMainWindowVM.UpdateScripDataFromMemory(Br);
            //    objScripDetails.ScriptCode = SelectedItem.Scriptcode1;
            //    // bstfivewindow.Activate();
            //    AdvancedTWS.ViewModel.ClassicTouchLineVM.objBroadCastProcessor_OnBroadCastRecievedNew(objScripDetails);
            //    BestFiveVM.UpdateTitle(SelectedItem.Scriptcode1);
            //    BestFiveVM.UpdateBestWindow(objScripDetails, true);
            //    HourlyStatisticsVM.UpdateTitleHourlyStatistics(SelectedItem.Scriptcode1, true);
            //    //mainwindow.Activate();
            //    mainwindow.Topmost = true;
            //    mainwindow.Topmost = false;
            //    mainwindow.Focus();
            //    mainwindow.Show();
            //    Loaded = true;
            //}
        }

        private static void UpdateGrid(int index, ScripDetails _BstFive)
        {
            //if (this.ObjTouchlineDataCollection[index].IsVisible)
            //{
            //Code For COunters
            // int c;
            // CountUpdatedperView++;
            DecimalPnt = CommonFunctions.GetDecimal(ObjTouchlineDataCollection[index].Scriptcode1);
            int num = ObjTouchlineDataCollection[index].BuyQualtity1;
            double num2 = ObjTouchlineDataCollection[index].BuyRate1;
            int num3 = ObjTouchlineDataCollection[index].SellQuantity1;
            double num4 = ObjTouchlineDataCollection[index].SellRate1;
            double ltpPrevious = ObjTouchlineDataCollection[index].LTP1;
            //Code For COunters
            //c=++ this.ObjTouchlineDataCollection[index].Counter;
            // WriteLog objWriteLog=new WriteLog();
            // objWriteLog.ScriptCode=ObjTouchlineDataCollection[index].ScriptId1;
            // objWriteLog.startTime=_BstFive.startTime;
            // objWriteLog.Count = c;
            // objWriteLog.BuyQtyL = _BstFive.BuyQtyL;
            // objWriteLog.BuyRateL = _BstFive.BuyRateL;
            // objWriteLog.SellQtyL = _BstFive.SellRateL;
            // objWriteLog.SellRateL = _BstFive.SellRateL;
            // objWriteLog.lastTradeRateL = _BstFive.lastTradeRateL;
            // objWriteLog.NoOfBidBuyL = _BstFive.NoOfBidBuyL;
            // objWriteLog.NoOfBidSellL = _BstFive.NoOfBidSellL;
            // objWriteLog.openRateL = _BstFive.openRateL;
            // objWriteLog.closeRateL = _BstFive.closeRateL;
            //_listWriteinFile.TryAdd(objWriteLog.Count, objWriteLog);
            //if (calculatetime)
            //{
            //    this.ObjTouchlineDataCollection[index].StartTime = _BstFive.startTime;
            //    calculatetime = false;
            //}
            //if (calculatetime1)
            //{
            //    this.ObjTouchlineDataCollection[index].StartTime1 = _BstFive.startTime;
            //    calculatetime1 = false;
            //      using (StreamWriter writer = new StreamWriter(@"E:\Deepshikha\IML.csv", false))
            //    {
            //        foreach (WriteLog line in _listWriteinFile.Values)
            //            writer.WriteLine(line.ScriptCode + ": " + line.startTime + ": " + line.Count + ": " + line.BuyQtyL + ": " +
            //                line.BuyRateL + ": " + line.SellQtyL + ": " + line.SellRateL
            //                + line.lastTradeRateL + ": " + line.NoOfBidBuyL + ": " +
            //                line.NoOfBidSellL + ": " + line.openRateL + ": " + line.closeRateL);
            //    }
            //}

            ObjTouchlineDataCollection[index].BuyQualtity1 = _BstFive.BuyQtyL;
            ObjTouchlineDataCollection[index].BuyRate1 = _BstFive.BuyRateL / Math.Pow(10, DecimalPnt);
            objTouchlineDataCollection[index].SellQuantity1 = _BstFive.SellQtyL;
            objTouchlineDataCollection[index].SellRate1 = _BstFive.SellRateL / Math.Pow(10, DecimalPnt);
            ObjTouchlineDataCollection[index].LTP1 = _BstFive.lastTradeRateL / Math.Pow(10, DecimalPnt);
            ObjTouchlineDataCollection[index].NoofBidBuy1 = _BstFive.NoOfBidBuyL;
            ObjTouchlineDataCollection[index].NoOfBidSell1 = _BstFive.NoOfBidSellL;
            ObjTouchlineDataCollection[index].OpenRateL = _BstFive.openRateL / Math.Pow(10, DecimalPnt);
            ObjTouchlineDataCollection[index].CloseRateL = _BstFive.closeRateL / Math.Pow(10, DecimalPnt);
            ObjTouchlineDataCollection[index].HighRateL = _BstFive.highRateL / Math.Pow(10, DecimalPnt);
            ObjTouchlineDataCollection[index].LowRateL = _BstFive.lowRateL / Math.Pow(10, DecimalPnt);
            ObjTouchlineDataCollection[index].TotBuyQtyL = _BstFive.totBuyQtyL;
            ObjTouchlineDataCollection[index].TotSellQtyL = _BstFive.totSellQtyL;
            ObjTouchlineDataCollection[index].WtAvgRateL = _BstFive.wtAvgRateL / Math.Pow(10, DecimalPnt);
            ObjTouchlineDataCollection[index].CtVolume = _BstFive.TrdVolume;
            ObjTouchlineDataCollection[index].CtValue = _BstFive.TrdValue / Math.Pow(10, DecimalPnt);
            double LTP = ObjTouchlineDataCollection[index].LTP1;
            //   ObjTouchlineDataCollection.ListChanged += ObjTouchlineDataCollection_ListChanged;
            //if (!this.ObjTouchlineDataCollection[index].FirstTickExecuted)
            //{
            //    this.ObjTouchlineDataCollection[index].FirstTickExecuted = true;
            //}

            if (ObjTouchlineDataCollection[index].BuyQualtity1 != 0 && num != 0)
            {
                if (num < ObjTouchlineDataCollection[index].BuyQualtity1)
                {
                    ObjTouchlineDataCollection[index].PrevBuyQualtity1 = UtilityLoginDetails.GETInstance.UpTrendColorGlobal;
                    ObjTouchlineDataCollection[index].ForegroundColorBuyQuantity = "White";
                }
                else if (num > ObjTouchlineDataCollection[index].BuyQualtity1)
                {
                    ObjTouchlineDataCollection[index].PrevBuyQualtity1 = UtilityLoginDetails.GETInstance.DownTrendColorGlobal;
                    ObjTouchlineDataCollection[index].ForegroundColorBuyQuantity = "White";
                }

                else if (num == ObjTouchlineDataCollection[index].BuyQualtity1)
                {
                    ObjTouchlineDataCollection[index].ForegroundColorBuyQuantity = "White";
                    if (index % 2 != 0)
                    {
                        ObjTouchlineDataCollection[index].PrevBuyQualtity1 = "#FF333972";
                    }
                    else
                    {
                        ObjTouchlineDataCollection[index].PrevBuyQualtity1 = "#FF333972";
                    }
                }
            }
            else
            {

                ObjTouchlineDataCollection[index].ForegroundColorBuyQuantity = "White";
                if (index % 2 != 0)
                {
                    ObjTouchlineDataCollection[index].PrevBuyQualtity1 = "#FF333972";
                }
                else
                {
                    ObjTouchlineDataCollection[index].PrevBuyQualtity1 = "#FF333972";
                }
            }
            if (ObjTouchlineDataCollection[index].BuyRate1 != 0 && num2 != 0)
            {
                if (num2 < ObjTouchlineDataCollection[index].BuyRate1)
                {
                    ObjTouchlineDataCollection[index].PrevBuyRate1 = UtilityLoginDetails.GETInstance.UpTrendColorGlobal;
                    ObjTouchlineDataCollection[index].ForegroundBuyRate = "White";
                }
                else if (num2 > ObjTouchlineDataCollection[index].BuyRate1)
                {
                    ObjTouchlineDataCollection[index].PrevBuyRate1 = UtilityLoginDetails.GETInstance.DownTrendColorGlobal;
                    ObjTouchlineDataCollection[index].ForegroundBuyRate = "White";
                }
                else if (num2 == ObjTouchlineDataCollection[index].BuyRate1)
                {
                    ObjTouchlineDataCollection[index].ForegroundBuyRate = "White";
                    if (index % 2 != 0)
                    {
                        ObjTouchlineDataCollection[index].PrevBuyRate1 = "#FF333972";
                    }
                    else
                    {
                        ObjTouchlineDataCollection[index].PrevBuyRate1 = "#FF333972";
                    }
                }
            }
            else
            {
                ObjTouchlineDataCollection[index].ForegroundBuyRate = "White";
                if (index % 2 != 0)
                {
                    ObjTouchlineDataCollection[index].PrevBuyRate1 = "#FF333972";
                }
                else
                {
                    ObjTouchlineDataCollection[index].PrevBuyRate1 = "#FF333972";
                }
            }
            if (ObjTouchlineDataCollection[index].SellQuantity1 != 0 && num3 != 0)
            {
                if (num3 < ObjTouchlineDataCollection[index].SellQuantity1)
                {
                    ObjTouchlineDataCollection[index].PrevSellQuantity1 = UtilityLoginDetails.GETInstance.UpTrendColorGlobal;
                    ObjTouchlineDataCollection[index].ForegroundSellQ = "White";
                }
                else if (num3 > ObjTouchlineDataCollection[index].SellQuantity1)
                {
                    ObjTouchlineDataCollection[index].PrevSellQuantity1 = UtilityLoginDetails.GETInstance.DownTrendColorGlobal;
                    ObjTouchlineDataCollection[index].ForegroundSellQ = "White";
                }
                else if (num3 == ObjTouchlineDataCollection[index].SellQuantity1)
                {
                    ObjTouchlineDataCollection[index].ForegroundSellQ = "White";
                    if (index % 2 != 0)
                    {
                        ObjTouchlineDataCollection[index].PrevSellQuantity1 = "#FF333972";
                    }
                    else
                    {
                        ObjTouchlineDataCollection[index].PrevSellQuantity1 = "#FF333972";
                    }
                }
            }
            else
            {
                ObjTouchlineDataCollection[index].ForegroundSellQ = "White";
                if (index % 2 != 0)
                {
                    ObjTouchlineDataCollection[index].PrevSellQuantity1 = "#FF333972";
                }
                else
                {
                    ObjTouchlineDataCollection[index].PrevSellQuantity1 = "#FF333972";
                }
            }
            if (ObjTouchlineDataCollection[index].SellRate1 != 0 && num4 != 0)
            {
                if (num4 < ObjTouchlineDataCollection[index].SellRate1)
                {
                    ObjTouchlineDataCollection[index].PrevSellRate1 = UtilityLoginDetails.GETInstance.UpTrendColorGlobal;
                    ObjTouchlineDataCollection[index].ForegroundSellR = "White";
                }
                else if (num4 > ObjTouchlineDataCollection[index].SellRate1)
                {
                    ObjTouchlineDataCollection[index].PrevSellRate1 = UtilityLoginDetails.GETInstance.DownTrendColorGlobal;
                    ObjTouchlineDataCollection[index].ForegroundSellR = "White";
                }
                else if (num4 == ObjTouchlineDataCollection[index].SellRate1)
                {
                    ObjTouchlineDataCollection[index].ForegroundSellR = "White";
                    if (index % 2 != 0)
                    {
                        ObjTouchlineDataCollection[index].PrevSellRate1 = "#FF333972";
                    }
                    else
                    {
                        ObjTouchlineDataCollection[index].PrevSellRate1 = "#FF333972";
                    }
                }
            }
            else
            {
                ObjTouchlineDataCollection[index].ForegroundSellR = "White";
                if (index % 2 != 0)
                {
                    ObjTouchlineDataCollection[index].PrevSellRate1 = "#FF333972";
                }
                else
                {
                    ObjTouchlineDataCollection[index].PrevSellRate1 = "#FF333972";
                }
            }
            if (ltpPrevious == 0 && ObjTouchlineDataCollection[index].LTP1 != 0)
            {

                if (ObjTouchlineDataCollection[index].LTP1 > ObjTouchlineDataCollection[index].CloseRateL)
                {
                    ObjTouchlineDataCollection[index].PrevLTP1 = UtilityLoginDetails.GETInstance.UpTrendColorGlobal;
                    ObjTouchlineDataCollection[index].ForegroundLTP = "White";
                }
                else if (ObjTouchlineDataCollection[index].LTP1 < ObjTouchlineDataCollection[index].CloseRateL)
                {
                    ObjTouchlineDataCollection[index].PrevLTP1 = UtilityLoginDetails.GETInstance.DownTrendColorGlobal;
                    ObjTouchlineDataCollection[index].ForegroundLTP = "White";
                }
                else
                {
                    ObjTouchlineDataCollection[index].ForegroundLTP = "White";
                    ObjTouchlineDataCollection[index].PrevLTP1 = "#FF333972";
                }
            }
            else if (ObjTouchlineDataCollection[index].LTP1 != 0 && ltpPrevious != 0)
            {
                if (ltpPrevious < ObjTouchlineDataCollection[index].LTP1)
                {
                    ObjTouchlineDataCollection[index].PrevLTP1 = UtilityLoginDetails.GETInstance.UpTrendColorGlobal;
                    ObjTouchlineDataCollection[index].ForegroundLTP = "White";
                }
                else if (ltpPrevious > ObjTouchlineDataCollection[index].LTP1)
                {
                    ObjTouchlineDataCollection[index].PrevLTP1 = UtilityLoginDetails.GETInstance.DownTrendColorGlobal;
                    ObjTouchlineDataCollection[index].ForegroundLTP = "White";
                }
                else if (ltpPrevious == ObjTouchlineDataCollection[index].LTP1)
                {
                    ObjTouchlineDataCollection[index].ForegroundLTP = "White";
                    if (index % 2 != 0)
                    {
                        ObjTouchlineDataCollection[index].PrevLTP1 = "#FF333972";
                    }
                    else
                    {
                        ObjTouchlineDataCollection[index].PrevLTP1 = "#FF333972";
                    }
                }
            }
            else
            {
                ObjTouchlineDataCollection[index].ForegroundLTP = "White";
                if (index % 2 != 0)
                {
                    ObjTouchlineDataCollection[index].PrevLTP1 = "#FF333972";
                }
                else
                {
                    ObjTouchlineDataCollection[index].PrevLTP1 = "#FF333972";
                }
            }

            /*
            if (LTP != 0 && ObjTouchlineDataCollection[index].CloseRateL != 0)
            {
                if (LTP > ObjTouchlineDataCollection[index].CloseRateL)
                {
                    ObjTouchlineDataCollection[index].PrevLTP1 = App.UpTrendColorGlobal;
                    ObjTouchlineDataCollection[index].ForegroundLTP = "White";
                }
                else if (LTP < ObjTouchlineDataCollection[index].CloseRateL)
                {
                    ObjTouchlineDataCollection[index].PrevLTP1 = App.DownTrendColorGlobal;
                    ObjTouchlineDataCollection[index].ForegroundLTP = "White";
                }
                else if (LTP == ObjTouchlineDataCollection[index].CloseRateL)
                {
                    ObjTouchlineDataCollection[index].ForegroundLTP = "White";
                    if (index % 2 != 0)
                    {
                        ObjTouchlineDataCollection[index].PrevLTP1 = "#FF333972";
                    }
                    else
                    {
                        ObjTouchlineDataCollection[index].PrevLTP1 = "#FF333972";
                    }
                }
            }
            else
            {
                ObjTouchlineDataCollection[index].ForegroundLTP = "White";
                if (index % 2 != 0)
                {
                    ObjTouchlineDataCollection[index].PrevLTP1 = "#FF333972";
                }
                else
                {
                    ObjTouchlineDataCollection[index].PrevLTP1 = "#FF333972";
                }

            }*/

            if (LTP != 0 && ObjTouchlineDataCollection[index].FiftyTwoHigh != 0)
            {
                if (LTP > ObjTouchlineDataCollection[index].FiftyTwoHigh)
                {
                    // ObjTouchlineDataCollection[index].FiftyTwoHighBColor = App.UpTrendColorGlobal;
                    ObjTouchlineDataCollection[index].FiftyTwoHigh = LTP;
                    MasterSharedMemory.objDicDP[ObjTouchlineDataCollection[index].Scriptcode1].WeeksHighprice = _BstFive.lastTradeRateL;
                    MasterSharedMemory.objDicDP[ObjTouchlineDataCollection[index].Scriptcode1].Dateof52weeksHighprice = CommonFunctions.GetDate().ToString("ddMMyyyy");//DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("ddMMyyyy");
                }
            }

            if (LTP != 0 && ObjTouchlineDataCollection[index].FiftyTwoLow != 0)
            {
                if (LTP < ObjTouchlineDataCollection[index].FiftyTwoLow)
                {
                    //ObjTouchlineDataCollection[index].FiftyTwoLowBColor = App.DownTrendColorGlobal;
                    ObjTouchlineDataCollection[index].FiftyTwoLow = LTP;
                    MasterSharedMemory.objDicDP[ObjTouchlineDataCollection[index].Scriptcode1].WeeksLowprice = _BstFive.lastTradeRateL;
                    MasterSharedMemory.objDicDP[ObjTouchlineDataCollection[index].Scriptcode1].Dateof52weeksLowprice = CommonFunctions.GetDate().ToString("ddMMyyyy");//DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("ddMMyyyy");
                }
            }

            //}
        }
#endif
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
        public void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
                //var e = new PropertyChangedEventArgs(propertyName);
                //this.PropertyChanged(this, e);
            }

        }

        #endregion


    }


}
