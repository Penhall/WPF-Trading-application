using CommonFrontEnd.Common;
using CommonFrontEnd.Model;
using CommonFrontEnd.Model.Trade;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.View.Trade;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
//using CommonFrontEnd.Model.Settings;
using System.Windows;

namespace CommonFrontEnd.ViewModel.Trade
{
#if TWS
    public class NetPositionScripWiseVM : BaseViewModel
    {
        #region Properties

        DirectoryInfo directory = new DirectoryInfo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"User/NetPositionScripWise.csv")));

        //public static bool Load_NetpositionSWCWEntry;
        NetPositionScripWiseDetails oNetPositionScripWiseDetails;
        public static ObservableCollection<ScripWisePositionModel> NetPositionSWDataCollectionWindow { get; set; }

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

        private string _TotalGrossBuyValString;

        public string TotalGrossBuyValString
        {
            get { return _TotalGrossBuyValString; }
            set { _TotalGrossBuyValString = value; NotifyPropertyChanged("TotalGrossBuyValString"); }
        }

        private string _TotalGrossSellValString;

        public string TotalGrossSellValString
        {
            get { return _TotalGrossSellValString; }
            set { _TotalGrossSellValString = value; NotifyPropertyChanged("TotalGrossSellValString"); }
        }

        private string _TotalNetValString;

        public string TotalNetValString
        {
            get { return _TotalNetValString; }
            set { _TotalNetValString = value; NotifyPropertyChanged("TotalNetValString"); }
        }

        private string _TotalGrossValString;

        public string TotalGrossValString
        {
            get { return _TotalGrossValString; }
            set { _TotalGrossValString = value; NotifyPropertyChanged("TotalGrossValString"); }
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

        //private double _TotalGrossBuyVal;

        //public  double TotalGrossBuyVal
        //{
        //    get
        //    {
        //        return (
        //            _TotalGrossBuyVal = MemoryManager.TotalGrossBuyVal / (Math.Pow(10, CommonFrontEnd.Global.UtilityApplicationDetails.GetInstance.MaxDecimalPoint)));
        //    }
        //    set { _TotalGrossBuyVal = value; NotifyPropertyChanged("TotalGrossBuyVal"); }
        //}

        //private  double _TotalGrossSellVal;

        //public  double TotalGrossSellVal
        //{
        //    get { return _TotalGrossSellVal = MemoryManager.TotalGrossSellVal / (Math.Pow(10, 4)); }
        //    set { _TotalGrossSellVal = value; NotifyPropertyChanged("TotalGrossSellVal"); }
        //}

        //private  double _TotalNetVal;

        //public  double TotalNetVal
        //{
        //    get { return _TotalNetVal = MemoryManager.TotalNetVal / (Math.Pow(10, 4)); }
        //    set { _TotalNetVal = value; NotifyPropertyChanged("TotalNetVal"); }
        //}

        //private  double _TotalGrossVal;

        //public  double TotalGrossVal
        //{
        //    get { return _TotalGrossVal = MemoryManager.TotalGrossVal / (Math.Pow(10, 4)); }
        //    set { _TotalGrossVal = value; NotifyPropertyChanged("TotalGrossVal"); }
        //}




        private ScripWisePositionModel _SelectedItem;
        public ScripWisePositionModel SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; }
        }

        //todo remove
        private static int _DisplayCount;

        public static int DisplayCount
        {
            get { return _DisplayCount; }
            set { _DisplayCount = value; NotifyStaticPropertyChanged("DisplayCount"); value = CommonFrontEnd.Processor.UMSProcessor.TradeCount; }
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

        #region RelayCommand
        private List<ScripWisePositionModel> _selectEntireRowList;

        public List<ScripWisePositionModel> selectEntireRowList
        {
            get { return _selectEntireRowList; }
            set { _selectEntireRowList = value; NotifyPropertyChanged(nameof(selectEntireRowList)); }
        }
        private RelayCommand _ShortCut_Escape;

        public RelayCommand ShortCut_Escape
        {
            get
            {
                return _ShortCut_Escape ?? (_ShortCut_Escape = new RelayCommand(
                    (object e) => NetPositionScripWise_Closing(e)
                        ));
            }
        }

        private RelayCommand _btnSquareOffSave;

        public RelayCommand btnSquareOffSave
        {
            get { return _btnSquareOffSave ?? (_btnSquareOffSave = new RelayCommand((object e) => btnSquareOffSaveBatch(e))); }
        }



        private RelayCommand _NPSWWindowClosing;

        public RelayCommand NPSWWindowClosing
        {

            get { return _NPSWWindowClosing ?? (_NPSWWindowClosing = new RelayCommand((object e) => NetPositionScripWise_Closing(e))); }

        }
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
        private void NetPositionScripWise_Closing(object e)
        {
            //TODO: Uncomment Windows Position ScripWiseVM - Gaurav 03/11/2017
            NetPositionScripWise oNetPositionScripWise = System.Windows.Application.Current.Windows.OfType<NetPositionScripWise>().FirstOrDefault();
            if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = (BoltAppSettingsWindowsPosition)CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISE != null && oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISE.WNDPOSITION != null)
                {
                    oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISE.WNDPOSITION.Left = Convert.ToInt32(LeftPosition);
                    oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISE.WNDPOSITION.Top = Convert.ToInt32(TopPosition);
                    oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISE.WNDPOSITION.Right = Convert.ToInt32(Width);
                    oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISE.WNDPOSITION.Down = Convert.ToInt32(Height);
                }

                CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(CommonFrontEnd.SharedMemories.SettingsManager.AppSettingsXmlPath, "WindowsPosition");
            }
            if (oNetPositionScripWise != null)
            {
                oNetPositionScripWise.Hide();
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
                return _ExportExcel ?? (_ExportExcel = new RelayCommand((object e) => ExecuteMyCommand(NetPositionSWDataCollectionWindow)));
            }
        }

        private RelayCommand _myLocationChanged;

        public RelayCommand myLocationChanged
        {
            get
            {
                return _myLocationChanged ?? (_myLocationChanged = new RelayCommand(
                    (object e) => Windows_NetPositionScripwiseLocationChanged(e)));

            }
        }

        private void Windows_NetPositionScripwiseLocationChanged(object e)
        {
            if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = (BoltAppSettingsWindowsPosition)CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISE != null && oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISE.WNDPOSITION != null)
                {
                    oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISE.WNDPOSITION.Left = Convert.ToInt32(LeftPosition);
                    oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISE.WNDPOSITION.Top = Convert.ToInt32(TopPosition);
                    oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISE.WNDPOSITION.Right = Convert.ToInt32(Width);
                    oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISE.WNDPOSITION.Down = Convert.ToInt32(Height);
                }
                //CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(@"D:\TWS_DotNetNewStructure\TWS_DOTNETT\CommonFrontEnd\bin\Debug\xml\Users\AppSettings\920200000.xml", "WindowsPosition");
                CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(CommonFrontEnd.SharedMemories.SettingsManager.AppSettingsXmlPath, "WindowsPosition");
            }
        }


        #endregion

        #region Events

        public delegate void ScripDoubleClickEventhandler(long scripCode);
        public static event ScripDoubleClickEventhandler OnScripDoubleClickEventhandler;

        #endregion

        //private void NetPositionScripWise_Closing()
        //{
        //    Processor.UMSProcessor.OnTradeSWReceived -= UpdateHeader;
        //}


        private void btnSquareOffSaveBatch(object e)
        {
            try
            {
                if (selectEntireRowList.Count <= 0)
                {
                    System.Windows.Forms.MessageBox.Show("No Order Selected to Save!!", "Warning!!",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }
                //txtReply = string.Empty;
                System.Windows.Forms.SaveFileDialog objFileDialogBatchResub = new System.Windows.Forms.SaveFileDialog();
                objFileDialogBatchResub.InitialDirectory = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"User/"))));
                if (!Directory.Exists(objFileDialogBatchResub.InitialDirectory))
                    Directory.CreateDirectory(objFileDialogBatchResub.InitialDirectory);

                //objFileDialogBatchResub.Title = "Browse CSV Files";
                objFileDialogBatchResub.DefaultExt = "csv";
                string Filter = "CSV files (*.csv)|*.csv";
                objFileDialogBatchResub.Filter = Filter;
                const string header = "Buy/Sell,Qty,Rev.Qty,Scrip Code,Rate,Short/Client ID,Retention Status,Client Type,Order Type,CP Code";
                StreamWriter writer = null;
                if (objFileDialogBatchResub.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Filter = objFileDialogBatchResub.FileName;

                    writer = new StreamWriter(Filter, false, System.Text.Encoding.UTF8);

                    writer.WriteLine(header);
                    foreach (var item in selectEntireRowList.Where(x => x.NetQty != 0).ToList())
                    {
                        foreach (var item1 in NetPositionMemory.NetPositionSWCWDataCollection.Where(x => x.ScripCode == item.ScripCode).ToList())
                        {
                           if(item1.NetQty > 0)
                            {
                                writer.WriteLine($"{"S"}, {item1.NetQty}, {item1.NetQty}, {item1.ScripCode}, {0}, {item1.ClientID}, {"EOTODY"},{item1.ClientType},{"G"},{""}");
                            }
                           else if(item1.NetQty < 0)
                            {
                                writer.WriteLine($"{"B"}, {(item1.NetQty * -1)}, {(item1.NetQty * -1)}, {item1.ScripCode}, {0}, {item1.ClientID}, {"EOTODY"},{item1.ClientType},{"G"},{""}");
                            }
                            
                        }
                    }
                    writer.Close();

                    System.Windows.Forms.MessageBox.Show("File Saved Successfully", "Successfull",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {

                ExceptionUtility.LogError(ex);
            }
        }
        private void DataGrid_DoubleClick()
        {
            //string ScripID = string.Empty;
            var data = SelectedItem;
            if (data != null)
            {
                #region Commented Gaurav 25/04/


                //if (data != null)
                //{
                //    //if (!Load_NetpositionSWCWEntry)
                //    //{
                //    //    oNetPositionScripWiseDetails = new NetPositionScripWiseDetails();
                //    //}
                //    //oNetPositionScripWiseDetails.Focus();
                //    //oNetPositionScripWiseDetails.Show();
                //    //oNetPositionScripWiseDetails.Activate();


                //    //Load_NetpositionSWCWEntry = true;
                //    //if (OnScripDoubleClickEventhandler != null)
                //    //    OnScripDoubleClickEventhandler(data.ScripCode);
                //}
                #endregion

                NetPositionScripWiseDetails oNetPositionScripWiseDetails = System.Windows.Application.Current.Windows.OfType<NetPositionScripWiseDetails>().FirstOrDefault();
                if (oNetPositionScripWiseDetails != null)
                {
                    oNetPositionScripWiseDetails.Focus();
                    oNetPositionScripWiseDetails.Show();
                    if (oNetPositionScripWiseDetails.WindowState == WindowState.Minimized)
                    {
                        oNetPositionScripWiseDetails.WindowState = WindowState.Normal;
                    }
                }
                else
                {
                    oNetPositionScripWiseDetails = new NetPositionScripWiseDetails();
                    oNetPositionScripWiseDetails.Activate();
                    oNetPositionScripWiseDetails.Show();
                }
                if (OnScripDoubleClickEventhandler != null)
                    OnScripDoubleClickEventhandler(data.ScripCode);
            }

        }

        public void UpdateHeader()
        {
            var tempTotalGrossBuyVal = MemoryManager.TotalGrossBuyVal / (Math.Pow(10, CommonFrontEnd.Global.UtilityApplicationDetails.GetInstance.MaxDecimalPoint));
            TotalGrossBuyValString = string.Format($"{tempTotalGrossBuyVal:0.00}");

            var tempTotalGrossSellVal = MemoryManager.TotalGrossSellVal / (Math.Pow(10, CommonFrontEnd.Global.UtilityApplicationDetails.GetInstance.MaxDecimalPoint));
            TotalGrossSellValString = string.Format($"{tempTotalGrossSellVal:0.00}");

            var tempTotalNetVal = (MemoryManager.TotalGrossBuyVal - MemoryManager.TotalGrossSellVal) / (Math.Pow(10, CommonFrontEnd.Global.UtilityApplicationDetails.GetInstance.MaxDecimalPoint));
            TotalNetValString = string.Format($"{tempTotalNetVal:0.00}");

            var tempTotalGrossVal = (MemoryManager.TotalGrossBuyVal + MemoryManager.TotalGrossSellVal) / (Math.Pow(10, CommonFrontEnd.Global.UtilityApplicationDetails.GetInstance.MaxDecimalPoint));
            TotalGrossValString = string.Format($"{tempTotalGrossVal:0.00}");
            //TotalGrossBuyVal = MemoryManager.TotalGrossBuyVal;
            //TotalGrossSellVal = MemoryManager.TotalGrossSellVal;
            //TotalNetVal = TotalGrossBuyVal - TotalGrossSellVal;//MemoryManager.TotalNetVal;
            //TotalGrossVal = TotalGrossBuyVal + TotalGrossSellVal;//MemoryManager.TotalGrossVal;
        }

        public NetPositionScripWiseVM()
        {

            if (Processor.UMSProcessor.OnTradeSWReceived == null)
                Processor.UMSProcessor.OnTradeSWReceived += UpdateHeader;

            //NetPositionSWDataCollection = new ObservableCollection<ScripWisePositionModel>();
            NetPositionSWDataCollectionWindow = new ObservableCollection<ScripWisePositionModel>();
            NetPositionSWDataCollectionWindow = NetPositionMemory.NetPositionSWDataCollection;

            UpdateHeader();
            //TODO: Uncomment Windows Position ScripWiseVM - Gaurav 03/11/2017
            if (CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                CommonFrontEnd.Model.BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = new Model.BoltAppSettingsWindowsPosition();
                oBoltAppSettingsWindowsPosition = (CommonFrontEnd.Model.BoltAppSettingsWindowsPosition)CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISE != null && oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISE.WNDPOSITION != null)
                {
                    Height = oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISE.WNDPOSITION.Down.ToString();
                    TopPosition = oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISE.WNDPOSITION.Top.ToString();
                    LeftPosition = oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISE.WNDPOSITION.Left.ToString();
                    Width = oBoltAppSettingsWindowsPosition.NETPOSITIONSCRIPWISE.WNDPOSITION.Right.ToString();
                }
            }

            NetPositionScripWise oNetPositionScripWise = System.Windows.Application.Current.Windows.OfType<NetPositionScripWise>().FirstOrDefault();
            //oNetPositionScripWise
            selectEntireRowList = new List<ScripWisePositionModel>();
            isAvgBuyRateString2decimalVisible = Visibility.Visible.ToString();
            isAvgBuyRateString4decimalVisible = Visibility.Hidden.ToString();
            isAvgSellRateString2decimalVisible = Visibility.Visible.ToString();
            isAvgSellRateString4decimalVisible = Visibility.Hidden.ToString();
        }


        public void ExecuteMyCommand(ObservableCollection<ScripWisePositionModel> NetPositionSWDataCollectionWindow)
        {
            if (NetPositionSWDataCollectionWindow.Count == 0)
            {
                MessageBox.Show("No Records to Save","Information",MessageBoxButton.OK,MessageBoxImage.Information);
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
                    sw.Write("Scrip, ScripCode, Buy Quantity, BuyAvgRate, Sell Quantity, SellAvgRate, Net Quantity, Net Value, BEP, ISIN Number");
                    sw.Write(sw.NewLine);

                    foreach (ScripWisePositionModel dr in NetPositionSWDataCollectionWindow)
                    {
                        sw.Write(dr.ScripName + "," + dr.ScripCode + "," + dr.BuyQty + "," + dr.AvgBuyRateString + "," + dr.SellQty + "," + dr.AvgSellRateString + "," + dr.NetQty.ToString() + "," + dr.NetValue.ToString() + "," + dr.BEPString + "," + dr.ISINNum);

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
            else
            {
                return;
            }

        }

    }
#endif
}
