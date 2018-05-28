using CommonFrontEnd.Common;
using CommonFrontEnd.Global;
using CommonFrontEnd.Model;
using CommonFrontEnd.Model.Trade;
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
using System.Windows;
using System.Windows.Forms;
//using CommonFrontEnd.Model.Settings;

namespace CommonFrontEnd.ViewModel.Trade
{
#if TWS
    public class NetPositionScripWiseDetailsVM : BaseViewModel
    {
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

        private static long ScripCodeSWCW { get; set; }

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

        private string _txtReply = string.Empty;

        public string txtReply
        {
            get { return _txtReply; }
            set { _txtReply = value; NotifyPropertyChanged(nameof(txtReply)); }
        }

        private string _Width = "449";

        public string Width
        {
            get { return _Width; }
            set { _Width = value; NotifyPropertyChanged("Width"); }
        }

        private ScripWiseDetailPositionModel _selectEntireRow;

        public ScripWiseDetailPositionModel selectEntireRow
        {
            get { return _selectEntireRow; }
            set { _selectEntireRow = value; NotifyPropertyChanged(nameof(selectEntireRow));
                txtReply = string.Empty;
            }
        }

        private List<ScripWiseDetailPositionModel> _selectEntireRowList;

        public List<ScripWiseDetailPositionModel> selectEntireRowList
        {
            get { return _selectEntireRowList; }
            set { _selectEntireRowList = value; NotifyPropertyChanged(nameof(selectEntireRowList)); }
        }

        private string _Height = "358.914";

        public string Height
        {
            get { return _Height; }
            set { _Height = value; NotifyPropertyChanged("Height"); }
        }

        //Ratein4decimalChecked
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


        private ObservableCollection<ScripWiseDetailPositionModel> _NetPositionSWCWDataCollectionWindow;

        public ObservableCollection<ScripWiseDetailPositionModel> NetPositionSWCWDataCollectionWindow
        {
            get { return _NetPositionSWCWDataCollectionWindow; }
            set { _NetPositionSWCWDataCollectionWindow = value; NotifyPropertyChanged("NetPositionSWCWDataCollectionWindow"); }
        }

        DirectoryInfo directory = new DirectoryInfo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"User/NetPositionScripWiseDetails.csv")));


        #region RelayCommand
        private RelayCommand _NetPositionSWCWWindowClosing;

        public RelayCommand NetPositionSWCWWindowClosing
        {
            get { return _NetPositionSWCWWindowClosing ?? (_NetPositionSWCWWindowClosing = new RelayCommand((object e) => OnNetPositionSWCWWindowClosing(e))); }
        }
        private RelayCommand _btnSquareOff;

        public RelayCommand btnSquareOff
        {
            get { return _btnSquareOff ?? (_btnSquareOff = new RelayCommand((object e) => SquareOffData(e))); }
        }

        private RelayCommand _btnSquareOffSave;

        public RelayCommand btnSquareOffSave
        {
            get { return _btnSquareOffSave ?? (_btnSquareOffSave = new RelayCommand((object e) => btnSquareOffSaveBatch(e))); }
        }



        private void OnNetPositionSWCWWindowClosing(object e)
        {
            Windows_NetPositionScripwiseDetailsLocationChanged(e);
            //NetPositionScripWiseVM.Load_NetpositionSWCWEntry = false;
            //NetPositionScripWiseVM.OnScripDoubleClickEventhandler -= UpdateClientsByScrip;
            //UMSProcessor.OnTradeSWCWOnlineReceived -= UpdateClientsByScripOnline;

            //TODO: Uncomment Windows Position ScripWiseDetailsVM - Gaurav 03/11/2017
            NetPositionScripWiseDetails oNetPositionScripWiseDetails = System.Windows.Application.Current.Windows.OfType<NetPositionScripWiseDetails>().FirstOrDefault();
            if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = (BoltAppSettingsWindowsPosition)CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISEDETAILS != null && oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISEDETAILS.WNDPOSITION != null)
                {
                    oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISEDETAILS.WNDPOSITION.Left = Convert.ToInt32(LeftPosition);
                    oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISEDETAILS.WNDPOSITION.Top = Convert.ToInt32(TopPosition);
                    oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISEDETAILS.WNDPOSITION.Right = Convert.ToInt32(Width);
                    oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISEDETAILS.WNDPOSITION.Down = Convert.ToInt32(Height);
                }
                //CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(@"D:\TWS_DotNetNewStructure\TWS_DOTNETT\CommonFrontEnd\bin\Debug\xml\Users\AppSettings\920200000.xml", "WindowsPosition");
                CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(CommonFrontEnd.SharedMemories.SettingsManager.AppSettingsXmlPath, "WindowsPosition");
            }
            if (oNetPositionScripWiseDetails != null)
            {
                oNetPositionScripWiseDetails.Hide();
            }
        }
        //private void Windows_NetPositionScripwiseDetailsLocationChanged(object e)
        //{

        //}


        private RelayCommand _myLocationChanged;

        public RelayCommand myLocationChanged
        {
            get
            {
                return _myLocationChanged ?? (_myLocationChanged = new RelayCommand(
                    (object e) => Windows_NetPositionScripwiseDetailsLocationChanged(e)));

            }
        }



        private void Windows_NetPositionScripwiseDetailsLocationChanged(object e)
        {
            if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = (BoltAppSettingsWindowsPosition)CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISEDETAILS != null && oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISEDETAILS.WNDPOSITION != null)
                {
                    oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISEDETAILS.WNDPOSITION.Left = Convert.ToInt32(LeftPosition);
                    oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISEDETAILS.WNDPOSITION.Top = Convert.ToInt32(TopPosition);
                    oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISEDETAILS.WNDPOSITION.Right = Convert.ToInt32(Width);
                    oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISEDETAILS.WNDPOSITION.Down = Convert.ToInt32(Height);
                }
                //CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(@"D:\TWS_DotNetNewStructure\TWS_DOTNETT\CommonFrontEnd\bin\Debug\xml\Users\AppSettings\920200000.xml", "WindowsPosition");
                CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(CommonFrontEnd.SharedMemories.SettingsManager.AppSettingsXmlPath, "WindowsPosition");
            }
        }


        private RelayCommand _ShortCut_Escape;

        public RelayCommand ShortCut_Escape
        {
            get
            {
                return _ShortCut_Escape ?? (_ShortCut_Escape = new RelayCommand(
                    (object e) => OnNetPositionSWCWWindowClosing(e)
                        ));
            }
        }

        private RelayCommand _ExportExcel;

        public RelayCommand ExportExcel
        {
            get
            {
                return _ExportExcel ?? (_ExportExcel = new RelayCommand((object e) => ExecuteMyCommand(NetPositionSWCWDataCollectionWindow)));
            }
        }

        #endregion

        //protected internal void OnNetPositionSWCWWindowClosing()
        //{
        //    NetPositionScripWiseVM.Load_NetpositionSWCWEntry = false;
        //    NetPositionScripWiseVM.OnScripDoubleClickEventhandler -= UpdateClientsByScrip;
        //    UMSProcessor.OnTradeSWCWOnlineReceived -= UpdateClientsByScripOnline;
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
        public NetPositionScripWiseDetailsVM()
        {
            NetPositionSWCWDataCollectionWindow = new ObservableCollection<ScripWiseDetailPositionModel>();
            NetPositionSWCWDataCollectionWindow = NetPositionMemory.NetPositionSWCWDataCollection;
            NetPositionScripWiseVM.OnScripDoubleClickEventhandler += UpdateClientsByScrip;
            UMSProcessor.OnTradeSWCWOnlineReceived += UpdateClientsByScripOnline;
            NetPositionScripDetail = "Net Position Details";
            //TODO: Uncomment Windows Position ScripWiseDetailsVM - Gaurav 03/11/2017
            selectEntireRowList = new List<ScripWiseDetailPositionModel>();
            if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                CommonFrontEnd.Model.BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = new Model.BoltAppSettingsWindowsPosition();
                oBoltAppSettingsWindowsPosition = (CommonFrontEnd.Model.BoltAppSettingsWindowsPosition)CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISEDETAILS != null && oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISEDETAILS.WNDPOSITION != null)
                {
                    Height = oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISEDETAILS.WNDPOSITION.Down.ToString();
                    TopPosition = oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISEDETAILS.WNDPOSITION.Top.ToString();
                    LeftPosition = oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISEDETAILS.WNDPOSITION.Left.ToString();
                    Width = oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISEDETAILS.WNDPOSITION.Right.ToString();
                }
            }
            isAvgBuyRateString2decimalVisible = Visibility.Visible.ToString();
            isAvgBuyRateString4decimalVisible = Visibility.Hidden.ToString();
            isAvgSellRateString2decimalVisible = Visibility.Visible.ToString();
            isAvgSellRateString4decimalVisible = Visibility.Hidden.ToString();
        }

        #region Relaycommand
        //private RelayCommand _ShowGrid;

        //public RelayCommand ShowGrid
        //{
        //    get { return _ShowGrid ?? (_ShowGrid = new RelayCommand((object e) => Refresh())); }
        //}
        #endregion

        public void UpdateClientsByScrip(long scripCode)
        {

            NetPositionSWCWDataCollectionWindow = null;
            if (scripCode != 0)
            {
                ScripCodeSWCW = scripCode;
                NetPositionMemory.UpdateScripNetPositionDetail(scripCode.ToString(), MemoryManager.NetPositionSWDemoDict.Where(x => ((CommonFrontEnd.Model.Trade.NetPosition)x.Value).ScripCode == scripCode).ToList());
                NetPositionSWCWDataCollectionWindow = new ObservableCollection<ScripWiseDetailPositionModel>(NetPositionMemory.NetPositionSWCWDataCollection.Where(x => x.ScripCode == scripCode));
            }
            //BSE Equity
            var segment = CommonFunctions.GetSegmentID(scripCode);
            NetPositionScripDetail = string.Format("Net Position Details - {0}", Common.CommonFunctions.GetScripId(scripCode, "BSE", segment));
        }
        public void UpdateClientsByScripOnline(string traderId)
        {
            if (ScripCodeSWCW != 0)
            {
                var scripCode = ScripCodeSWCW;
                //NetPositionSWCWDataCollectionWindow = null;
                if (scripCode != 0)
                {
                    //NetPositionMemory.UpdateScripNetPositionDetail(scripCode.ToString(), MemoryManager.NetPositionSWDemoDict.Where(x => ((CommonFrontEnd.Model.Trade.NetPosition)x.Value).ScripCode == scripCode).ToList());
                    //NetPositionSWCWDataCollectionWindow = new ObservableCollection<ScripWiseDetailPositionModel>(NetPositionMemory.NetPositionSWCWDataCollection.Where(x => x.ScripCode == scripCode));
                    var lstSWCWDetailPositionModelByScripCd = NetPositionMemory.NetPositionSWCWDataCollection.Where(x => x.ScripCode == scripCode).ToList();

                    if (lstSWCWDetailPositionModelByScripCd != null && lstSWCWDetailPositionModelByScripCd.Count > 0)
                    {
                        var oSWCWDetailPositionModelByTrader = lstSWCWDetailPositionModelByScripCd.Where(y => y.TraderId == traderId).FirstOrDefault();

                        if (oSWCWDetailPositionModelByTrader != null)
                        {
                            if (NetPositionSWCWDataCollectionWindow != null)
                            {
                                var item = NetPositionSWCWDataCollectionWindow.Where(x => x.ScripCode == scripCode && x.TraderId == traderId).FirstOrDefault();
                                if (item != null)
                                {
                                    int index = NetPositionSWCWDataCollectionWindow.IndexOf(item);
                                    if (index != -1)
                                    {
                                        NetPositionSWCWDataCollectionWindow[index] = oSWCWDetailPositionModelByTrader;
                                    }
                                    else
                                    {
                                        NetPositionSWCWDataCollectionWindow.Add(oSWCWDetailPositionModelByTrader);
                                    }
                                }
                                else
                                {
                                    NetPositionSWCWDataCollectionWindow.Add(oSWCWDetailPositionModelByTrader);
                                }
                            }
                            else
                            {
                                NetPositionSWCWDataCollectionWindow.Add(oSWCWDetailPositionModelByTrader);
                            }
                        }
                    }
                }
            }

        }

        private string _NetPositionScripDetail;

        public string NetPositionScripDetail
        {
            get { return _NetPositionScripDetail; }
            set
            {
                _NetPositionScripDetail = value;
                NotifyPropertyChanged("NetPositionScripDetail");
            }
        }


        public void ExecuteMyCommand(ObservableCollection<ScripWiseDetailPositionModel> NetPositionSWCWDataCollectionWindow)
        {
            if (NetPositionSWCWDataCollectionWindow.Count == 0)
            {
                System.Windows.MessageBox.Show("No Records to Save","Information",MessageBoxButton.OK,MessageBoxImage.Information);
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
                    sw.Write("Trader ID, Client, Client Type, Buy Quantity, BuyAvgRate, Sell Quantity, SellAvgRate, Net Quantity, Net Value, BEP, Net P/L, Real P/L, UnReal P/L");
                    sw.Write(sw.NewLine);

                    foreach (ScripWiseDetailPositionModel dr in NetPositionSWCWDataCollectionWindow)
                    {
                        sw.Write(dr.TraderId + "," + dr.ClientID + "," + dr.ClientType + "," + dr.BuyQty + "," + dr.AvgBuyRateString + "," + dr.SellQty + "," + dr.AvgSellRateString + "," + dr.NetQty + "," + dr.NetValue + "," + dr.BEPIn2Long + "," +
                            dr.NetPLIn2Long + "," + dr.RealPLIn2Long + "," + dr.UnRealPLIn2Long);

                        sw.Write(sw.NewLine);
                    }
                    System.Windows.MessageBox.Show("Trades Saved in file : " + dlg.FileName.ToString(),"Information",MessageBoxButton.OK,MessageBoxImage.Information);
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

        private void SquareOffData(object e)
        {
            if (selectEntireRowList != null && selectEntireRowList.Count > 1)
            {
                System.Windows.Forms.MessageBox.Show("Select Single Row to Square Off","Warning!",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                return;
            }

            if (UtilityOrderDetails.GETInstance.CurrentOrderEntry.ToLower() == Enumerations.OrderEntryWindow.Normal.ToString().ToLower())
            {
                #region for Normal Order Entry
                if (selectEntireRow != null)
                {
                    //txtReply = string.Empty;
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
                            System.Windows.Forms.MessageBox.Show("Invalid Net Qty.","Alert",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            return;
                        }
                        ((NormalOrderEntryVM)objNormal.DataContext).PassByNetPositionScripWiseDetails(selectEntireRow.ScripCode, selectEntireRow);
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
                            System.Windows.Forms.MessageBox.Show("Invalid Net Qty.","Alert",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                            return;
                        }

                        objNormal.Activate();
                        objNormal.Show();
                        ((NormalOrderEntryVM)objNormal.DataContext).PassByNetPositionScripWiseDetails(selectEntireRow.ScripCode, selectEntireRow);
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Please Select order to Resubmit","Alert",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
                #endregion
            }
            else if (UtilityOrderDetails.GETInstance.CurrentOrderEntry.ToLower() == Enumerations.OrderEntryWindow.Swift.ToString().ToLower())
            {
                #region for Swift Order Entry
                if (selectEntireRow != null)
                {
                    txtReply = string.Empty;
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
                            System.Windows.Forms.MessageBox.Show("Invalid Net Qty.","Alert",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                            return;
                        }
                        ((OrderEntryVM)objswift.DataContext).PassByPassByNetPositionScripWiseDetails(selectEntireRow.ScripCode, selectEntireRow);
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
                            System.Windows.Forms.MessageBox.Show("Invalid Net Qty.","Alert",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                            return;
                        }

                        objswift.Activate();
                        objswift.Show();
                        ((OrderEntryVM)objswift.DataContext).PassByPassByNetPositionScripWiseDetails(selectEntireRow.ScripCode, selectEntireRow);
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Please Select order to Resubmit","Alert",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
                #endregion
            }
        }

        private void btnSquareOffSaveBatch(object e)
        {
            try
            {
                if (selectEntireRowList.Count <= 0)
                {
                    System.Windows.MessageBox.Show("No Order Selected to Save!!","Alert",MessageBoxButton.OK,MessageBoxImage.Warning);
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
                if (objFileDialogBatchResub.ShowDialog() == DialogResult.OK)
                {
                    Filter = objFileDialogBatchResub.FileName;

                    writer = new StreamWriter(Filter, false, Encoding.UTF8);

                    writer.WriteLine(header);
                    foreach (var item in selectEntireRowList.Where(x => x.NetQty != 0).ToList())
                    {
                        if(item.NetQty>0)
                            writer.WriteLine($"{"S"}, {item.NetQty}, {item.NetQty}, {ScripCodeSWCW}, {0}, {item.ClientID}, {"EOTODY"},{item.ClientType},{"G"},{""}");
                        else
                            writer.WriteLine($"{"B"}, {(item.NetQty*-1)}, {(item.NetQty*-1)}, {ScripCodeSWCW}, {0}, {item.ClientID}, {"EOTODY"},{item.ClientType},{"G"},{""}");
                    }
                    writer.Close();

                    System.Windows.MessageBox.Show("File Saved Successfully at : "+objFileDialogBatchResub.FileName,"Successfull",MessageBoxButton.OK,MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error saving data in file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ExceptionUtility.LogError(ex);
            }

        }

    }
#endif
}
