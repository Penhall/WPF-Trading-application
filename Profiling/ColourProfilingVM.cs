using CommonFrontEnd.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;

namespace CommonFrontEnd.ViewModel.Profiling
{
    class ColourProfilingVM : BaseViewModel
    {
        #region Properties

        public static Action OnColorSettingChange;

        private ObservableCollection<string> _WindowsAvailable;

        public ObservableCollection<string> WindowsAvailable
        {
            get { return _WindowsAvailable; }
            set { _WindowsAvailable = value; NotifyPropertyChanged("WindowsAvailable"); }
        }

        private System.Windows.Media.Brush _setBgColor;

        public System.Windows.Media.Brush setBgColor
        {
            get { return _setBgColor; }
            set { _setBgColor = value; NotifyPropertyChanged(nameof(setBgColor)); }
        }

        private System.Windows.Media.Brush _setForeColor;

        public System.Windows.Media.Brush setForeColor
        {
            get { return _setForeColor; }
            set { _setForeColor = value; NotifyPropertyChanged(nameof(setForeColor)); }
        }

        private string _SelectedWindow;
        public string SelectedWindow
        {
            get { return _SelectedWindow; }
            set
            {
                _SelectedWindow = value;
                NotifyPropertyChanged("SelectedWindow");
                populateEelements();
            }
        }

        private string _TouchlineVisibility;
        public string TouchlineVisibility
        {
            get { return _TouchlineVisibility; }
            set
            {
                _TouchlineVisibility = value;
                NotifyPropertyChanged("TouchlineVisibility");
                //TradesVisibility = "Hidden";
                //TimeSaleVisibility = "Hidden";
                //PendingVisibility = "Hidden";
            }
        }

        private string _PendingVisibility;
        public string PendingVisibility
        {
            get { return _PendingVisibility; }
            set
            {
                _PendingVisibility = value;
                NotifyPropertyChanged("PendingVisibility");
                //TradesVisibility = "Hidden";
                //TimeSaleVisibility = "Hidden";
                //TouchlineVisibility = "Hidden";
            }
        }

        private string _TradesVisibility;
        public string TradesVisibility
        {
            get { return _TradesVisibility; }
            set
            {
                _TradesVisibility = value;
                NotifyPropertyChanged("TradesVisibility");
                //TouchlineVisibility = "Hidden";
                //TimeSaleVisibility = "Hidden";
                //PendingVisibility = "Hidden";
            }
        }

        private string _TimeSaleVisibility;
        public string TimeSaleVisibility
        {
            get { return _TimeSaleVisibility; }
            set
            {
                _TimeSaleVisibility = value;
                NotifyPropertyChanged("TimeSaleVisibility");
                //TradesVisibility = "Hidden";
                //TouchlineVisibility = "Hidden";
                //PendingVisibility = "Hidden";
            }
        }

        private ObservableCollection<string> _ScreenElement;
        public ObservableCollection<string> ScreenElement
        {
            get { return _ScreenElement; }
            set { _ScreenElement = value; }
        }

        private string _SelectedScreenElement;
        public string SelectedScreenElement
        {
            get { return _SelectedScreenElement; }
            set
            {
                _SelectedScreenElement = value;
                NotifyPropertyChanged(nameof(SelectedScreenElement)); OnChangeOfSelectedScreenElement();
            }
        }

        private List<string> _ScreenElementPending;
        public List<string> ScreenElementPending
        {
            get { return _ScreenElementPending; }
            set { _ScreenElementPending = value; NotifyPropertyChanged("ScreenElementPending"); }
        }

        private string _SelectedScreenElementPending;
        public string SelectedScreenElementPending
        {
            get { return _SelectedScreenElementPending; }
            set
            {
                _SelectedScreenElementPending = value;
                NotifyPropertyChanged("SelectedScreenElementPending");
            }
        }

        private List<string> _ScreenElementTrades;
        public List<string> ScreenElementTrades
        {
            get { return _ScreenElementTrades; }
            set { _ScreenElementTrades = value; NotifyPropertyChanged("ScreenElementTrades"); }
        }

        private string _SelectedScreenElementTrades;
        public string SelectedScreenElementTrades
        {
            get { return _SelectedScreenElementTrades; }
            set
            {
                _SelectedScreenElementTrades = value;
                NotifyPropertyChanged("SelectedScreenElementTrades");
            }
        }

        private List<string> _ScreenElementTimeSale;
        public List<string> ScreenElementTimeSale
        {
            get { return _ScreenElementTimeSale; }
            set { _ScreenElementTimeSale = value; NotifyPropertyChanged("ScreenElementTimeSale"); }
        }

        private string _SelectedScreenElementTimeSale;
        public string SelectedScreenElementTimeSale
        {
            get { return _SelectedScreenElementTimeSale; }
            set
            {
                _SelectedScreenElementTimeSale = value;
                NotifyPropertyChanged("SelectedScreenElementTimeSale");
            }
        }

        private Dictionary<int, ColorVariable> _ColorValues;

        public Dictionary<int, ColorVariable> ColorKeyValues
        {
            get { return _ColorValues; }
            set { _ColorValues = value; }
        }

        public BrushConverter objBrushConvertor { get; set; }

        #endregion

        #region RelayCommands

        private RelayCommand _WindowsComboBoxChanged;

        public RelayCommand WindowsComboBoxChanged
        {
            get
            {
                return _WindowsComboBoxChanged ?? (_WindowsComboBoxChanged = new RelayCommand(
                    (object e1) => OnChangeOfWindowSelection()));
            }
        }

        private RelayCommand _openColorDialog;

        public RelayCommand openColorDialog
        {
            get
            {
                return _openColorDialog ?? (_openColorDialog = new RelayCommand(
                    (object e1) => OpenColorDialogBox()));
            }
        }

        private RelayCommand _ApplyColor;

        public RelayCommand ApplyColor
        {
            get
            {
                return _ApplyColor ?? (_ApplyColor = new RelayCommand(
                    (object e1) => OnClickofApply()));
            }
        }

        private RelayCommand _SaveDefColor;

        public RelayCommand SaveDefColor
        {
            get
            {
                return _SaveDefColor ?? (_SaveDefColor = new RelayCommand(
                    (object e1) => SaveDefColor_Save()));
            }
        }

        #endregion

        #region Methods

        private void populateEelements()
        {
            //if (SelectedWindow == Enumerations.WindowsAvailable.Touchline.ToString())
            //{
            //    PopulateScreenElementsTouchline();
            //    setBgColor = ColorKeyValues[0].TouchLineBackGround;
            //}
             if (SelectedWindow == Enumerations.WindowsAvailable.PendingOrder.ToString())
            {
                PopulateScreenElementsPending();
                setBgColor = ColorKeyValues[1].PendingBackGround;
            }
            else if (SelectedWindow == Enumerations.WindowsAvailable.Trades.ToString())
            {
                PopulateScreenElementsTrades();
                setBgColor = ColorKeyValues[2].TradesBackGround;
            }
            //else if (SelectedWindow == Enumerations.WindowsAvailable.TimeAndSales.ToString())
            //{
            //    PopulateScreenElementsTimeSale();
            //    setBgColor = ColorKeyValues[3].ScripListBackground;
            //}
        }

        private void PopulateWindow()
        {
            WindowsAvailable = new ObservableCollection<string>();
           // WindowsAvailable.Add(Enumerations.WindowsAvailable.Touchline.ToString());
            WindowsAvailable.Add(Enumerations.WindowsAvailable.PendingOrder.ToString());
            WindowsAvailable.Add(Enumerations.WindowsAvailable.Trades.ToString());
            //WindowsAvailable.Add(Enumerations.WindowsAvailable.TimeAndSales.ToString());
            SelectedWindow = Enumerations.WindowsAvailable.PendingOrder.ToString();
        }

        //private void PopulateScreenElementsTouchline()
        //{
        //    ScreenElement.Clear();
        //    ScreenElement.Add(Enumerations.TouchlineColor.BackgroundColor.ToString());
        //    ScreenElement.Add(Enumerations.TouchlineColor.Uptrend.ToString());
        //    ScreenElement.Add(Enumerations.TouchlineColor.Downtrend.ToString());
        //    ScreenElement.Add(Enumerations.TouchlineColor.ForeGroundColor.ToString());
        //    ScreenElement.Add(Enumerations.TouchlineColor.UptrendFlash.ToString());
        //    ScreenElement.Add(Enumerations.TouchlineColor.DowntrendFlash.ToString());
        //    SelectedScreenElement = ScreenElement[0];

        //}

        private void PopulateScreenElementsPending()
        {
            ScreenElement.Clear();
            ScreenElement.Add(Enumerations.PendingOrderColor.BackgroundColor.ToString());
            ScreenElement.Add(Enumerations.PendingOrderColor.BuyOrder.ToString());
            ScreenElement.Add(Enumerations.PendingOrderColor.SellOrder.ToString());
            ScreenElement.Add(Enumerations.PendingOrderColor.BuyStoplossOrder.ToString());
            ScreenElement.Add(Enumerations.PendingOrderColor.SellStoplossOrder.ToString());
            ScreenElement.Add(Enumerations.PendingOrderColor.RRMBuyOrder.ToString());
            ScreenElement.Add(Enumerations.PendingOrderColor.RRMSellOrder.ToString());
            SelectedScreenElement = ScreenElement[0];
        }

        private void PopulateScreenElementsTrades()
        {
            ScreenElement.Clear();
            ScreenElement.Add(Enumerations.TradesColor.BackgroundColor.ToString());
            ScreenElement.Add(Enumerations.TradesColor.BuyTrade.ToString());
            ScreenElement.Add(Enumerations.TradesColor.SellTrade.ToString());
            ScreenElement.Add(Enumerations.TradesColor.BuySpreadTrade.ToString());
            ScreenElement.Add(Enumerations.TradesColor.SellSpreadTrade.ToString());
            ScreenElement.Add(Enumerations.TradesColor.SpreadBackgroundColor.ToString());
            SelectedScreenElement = ScreenElement[0];
            //SelectedScreenElementTrades.Add(Enumerations.TradesColor.RRMSellOrder.ToString());

        }

        //private void PopulateScreenElementsTimeSale()
        //{
        //    ScreenElement.Clear();
        //    ScreenElement.Add(Enumerations.TimeAndSalesColor.ScripListBackground.ToString());
        //    ScreenElement.Add(Enumerations.TimeAndSalesColor.ScripListUptrend.ToString());
        //    ScreenElement.Add(Enumerations.TimeAndSalesColor.ScripListDowntrend.ToString());
        //    ScreenElement.Add(Enumerations.TimeAndSalesColor.ScripDataBackground.ToString());
        //    ScreenElement.Add(Enumerations.TimeAndSalesColor.ScripDataUptrend.ToString());
        //    ScreenElement.Add(Enumerations.TimeAndSalesColor.ScripDataDowntrend.ToString());
        //    SelectedScreenElement = ScreenElement[0];
        //    //SelectedScreenElementTrades.Add(Enumerations.TradesColor.RRMSellOrder.ToString());

        //}

        private void SaveDefColor_Save()
        {

        }

        private void OnClickofApply()
        {
            //MainWindowVM.ParserDefaultCPR.AddSetting("TOUCHLINE", "TouchLineBackGround", ColorKeyValues[0].TouchLineBackGround?.ToString());
            //MainWindowVM.ParserDefaultCPR.AddSetting("TOUCHLINE", "TouchLineForeGround", ColorKeyValues[0].TouchLineForeGround?.ToString());
            //MainWindowVM.ParserDefaultCPR.AddSetting("TOUCHLINE", "TouchLineUptrend", ColorKeyValues[0].TouchLineUptrend?.ToString());
            //MainWindowVM.ParserDefaultCPR.AddSetting("TOUCHLINE", "TouchLineDowntrend", ColorKeyValues[0].TouchLineDowntrend?.ToString());
            //MainWindowVM.ParserDefaultCPR.AddSetting("TOUCHLINE", "TouchLineUptrendFlash", ColorKeyValues[0].TouchLineUptrendFlash?.ToString());
            //MainWindowVM.ParserDefaultCPR.AddSetting("TOUCHLINE", "TouchLineDowntrendFlash", ColorKeyValues[0].TouchLineDowntrendFlash?.ToString());
            //MainWindowVM.ParserDefaultCPR.AddSetting("TOUCHLINE", "TouchLineQuantityRate", ColorKeyValues[0].TouchLineQuantityRate?.ToString());
            MainWindowVM.ParserDefaultCPR.AddSetting("PENDING", "PendingBackGround", ColorKeyValues[1].PendingBackGround?.ToString());
            MainWindowVM.ParserDefaultCPR.AddSetting("PENDING", "PendingBuyOrder", ColorKeyValues[1].PendingBuyOrder?.ToString());
            MainWindowVM.ParserDefaultCPR.AddSetting("PENDING", "PendingSellOrder", ColorKeyValues[1].PendingSellOrder?.ToString());
            MainWindowVM.ParserDefaultCPR.AddSetting("PENDING", "PendingBuyStoploss", ColorKeyValues[1].PendingBuyStoploss?.ToString());
            MainWindowVM.ParserDefaultCPR.AddSetting("PENDING", "PendingSellStoploss", ColorKeyValues[1].PendingSellStoploss?.ToString());
            MainWindowVM.ParserDefaultCPR.AddSetting("PENDING", "PendingRRMBuy", ColorKeyValues[1].PendingRRMBuy?.ToString());
            MainWindowVM.ParserDefaultCPR.AddSetting("PENDING", "PendingRRMSell", ColorKeyValues[1].PendingRRMSell?.ToString());
            MainWindowVM.ParserDefaultCPR.AddSetting("TRADES", "TradesBackGround", ColorKeyValues[2].TradesBackGround?.ToString());
            MainWindowVM.ParserDefaultCPR.AddSetting("TRADES", "BuyTrade", ColorKeyValues[2].BuyTrade?.ToString());
            MainWindowVM.ParserDefaultCPR.AddSetting("TRADES", "SellTrade", ColorKeyValues[2].SellTrade?.ToString());
            MainWindowVM.ParserDefaultCPR.AddSetting("TRADES", "BuySpreadTrade", ColorKeyValues[2].BuySpreadTrade?.ToString());
            MainWindowVM.ParserDefaultCPR.AddSetting("TRADES", "SellSpreadTrade", ColorKeyValues[2].SellSpreadTrade?.ToString());
            MainWindowVM.ParserDefaultCPR.AddSetting("TRADES", "SpreadBGColor", ColorKeyValues[2].SpreadBGColor?.ToString());
            //MainWindowVM.ParserDefaultCPR.AddSetting("TIMEANDSALES", "ScripListBackground", ColorKeyValues[3].ScripListBackground?.ToString());
            //MainWindowVM.ParserDefaultCPR.AddSetting("TIMEANDSALES", "ScripListUptrend", ColorKeyValues[3].ScripListUptrend?.ToString());
            //MainWindowVM.ParserDefaultCPR.AddSetting("TIMEANDSALES", "ScripListDowntrend", ColorKeyValues[3].ScripListDowntrend?.ToString());
            //MainWindowVM.ParserDefaultCPR.AddSetting("TIMEANDSALES", "ScripDataBackground", ColorKeyValues[3].ScripDataBackground?.ToString());
            //MainWindowVM.ParserDefaultCPR.AddSetting("TIMEANDSALES", "ScripDataUptrend", ColorKeyValues[3].ScripDataUptrend?.ToString());
            //MainWindowVM.ParserDefaultCPR.AddSetting("TIMEANDSALES", "ScripDataDowntrend", ColorKeyValues[3].ScripDataDowntrend?.ToString());

            MainWindowVM.ParserDefaultCPR.SaveSettings();

            MessageBox.Show("Colour Profiling Settings Successfully saved", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (OnColorSettingChange != null)
            {
                OnColorSettingChange?.Invoke();
            }
        }

        private void OpenColorDialogBox()
        {
            ColorDialog objColorDialog = new ColorDialog();

            objColorDialog.FullOpen = true;
            objColorDialog.AnyColor = true;

            if (objColorDialog.ShowDialog() == DialogResult.OK)
            {
                System.Drawing.Color myBGColor = System.Drawing.Color.FromArgb(objColorDialog.Color.R, objColorDialog.Color.G, objColorDialog.Color.B);

                var converter = new System.Windows.Media.BrushConverter();
                //Color myFGColor = Color.FromArgb((255 - objColorDialog.Color.R), (255 - objColorDialog.Color.G), (255 - objColorDialog.Color.B));
                //Color myFGColor = Color.FromArgb(objColorDialog.Color.R, objColorDialog.Color.G, objColorDialog.Color.B);


                var BGbrush = (System.Windows.Media.Brush)converter.ConvertFromString($"#{myBGColor.Name}");

                if (SelectedWindow == Enumerations.WindowsAvailable.Touchline.ToString())
                {

                    if (SelectedScreenElement == Enumerations.TouchlineColor.BackgroundColor.ToString())
                    {
                        setBgColor = ColorKeyValues[0].TouchLineBackGround = BGbrush;

                    }
                    else if (SelectedScreenElement == Enumerations.TouchlineColor.ForeGroundColor.ToString())
                    {
                        System.Drawing.Color myFGColor = System.Drawing.Color.FromArgb(objColorDialog.Color.R, objColorDialog.Color.G, objColorDialog.Color.B);

                        var FGbrush1 = (System.Windows.Media.Brush)converter.ConvertFromString($"#{myFGColor.Name}");
                        setForeColor = ColorKeyValues[0].TouchLineForeGround = FGbrush1;
                    }
                    else if (SelectedScreenElement == Enumerations.TouchlineColor.Uptrend.ToString())
                    {
                        System.Drawing.Color myFGColor = System.Drawing.Color.FromArgb(objColorDialog.Color.R, objColorDialog.Color.G, objColorDialog.Color.B);

                        var FGbrush2 = (System.Windows.Media.Brush)converter.ConvertFromString($"#{myFGColor.Name}");
                        setForeColor = ColorKeyValues[0].TouchLineUptrend = FGbrush2;
                    }
                    else if (SelectedScreenElement == Enumerations.TouchlineColor.Downtrend.ToString())
                    {
                        System.Drawing.Color myFGColor = System.Drawing.Color.FromArgb(objColorDialog.Color.R, objColorDialog.Color.G, objColorDialog.Color.B);

                        var FGbrush3 = (System.Windows.Media.Brush)converter.ConvertFromString($"#{myFGColor.Name}");
                        setForeColor = ColorKeyValues[0].TouchLineDowntrend = FGbrush3;
                    }
                    else if (SelectedScreenElement == Enumerations.TouchlineColor.QuantityRate.ToString())
                    {
                        System.Drawing.Color myFGColor = System.Drawing.Color.FromArgb(objColorDialog.Color.R, objColorDialog.Color.G, objColorDialog.Color.B);

                        var FGbrush4 = (System.Windows.Media.Brush)converter.ConvertFromString($"#{myFGColor.Name}");
                        setForeColor = ColorKeyValues[0].TouchLineQuantityRate = FGbrush4;
                    }
                    else if (SelectedScreenElement == Enumerations.TouchlineColor.UptrendFlash.ToString())
                    {
                        System.Drawing.Color myBgColor = System.Drawing.Color.FromArgb(objColorDialog.Color.R, objColorDialog.Color.G, objColorDialog.Color.B);

                        var FGbrush5 = (System.Windows.Media.Brush)converter.ConvertFromString($"#{myBgColor.Name}");
                        setBgColor = ColorKeyValues[0].TouchLineUptrendFlash = FGbrush5;

                        System.Drawing.Color myFGColor = System.Drawing.Color.FromArgb((255 - objColorDialog.Color.R), (255 - objColorDialog.Color.G), (255 - objColorDialog.Color.B));
                        var myFgBrush = (System.Windows.Media.Brush)converter.ConvertFromString($"#{myFGColor.Name}");
                        setForeColor = myFgBrush;

                    }
                    else if (SelectedScreenElement == Enumerations.TouchlineColor.DowntrendFlash.ToString())
                    {
                        System.Drawing.Color myBgColor = System.Drawing.Color.FromArgb(objColorDialog.Color.R, objColorDialog.Color.G, objColorDialog.Color.B);

                        var FGbrush6 = (System.Windows.Media.Brush)converter.ConvertFromString($"#{myBgColor.Name}");
                        setBgColor = ColorKeyValues[0].TouchLineDowntrendFlash = FGbrush6;

                        System.Drawing.Color myFGColor = System.Drawing.Color.FromArgb((255 - objColorDialog.Color.R), (255 - objColorDialog.Color.G), (255 - objColorDialog.Color.B));
                        var myFgBrush = (System.Windows.Media.Brush)converter.ConvertFromString($"#{myFGColor.Name}");
                        setForeColor = myFgBrush;
                    }
                }

                if (SelectedWindow == Enumerations.WindowsAvailable.PendingOrder.ToString())
                {
                    if (SelectedScreenElement == Enumerations.PendingOrderColor.BackgroundColor.ToString())
                    {
                        setBgColor = ColorKeyValues[1].PendingBackGround = BGbrush;
                    }
                    else if (SelectedScreenElement == Enumerations.PendingOrderColor.BuyOrder.ToString())
                    {
                        System.Drawing.Color myFGColor = System.Drawing.Color.FromArgb(objColorDialog.Color.R, objColorDialog.Color.G, objColorDialog.Color.B);

                        var FGbrush1 = (System.Windows.Media.Brush)converter.ConvertFromString($"#{myFGColor.Name}");
                        setForeColor = ColorKeyValues[1].PendingBuyOrder = FGbrush1;
                    }
                    else if (SelectedScreenElement == Enumerations.PendingOrderColor.SellOrder.ToString())
                    {
                        System.Drawing.Color myFGColor = System.Drawing.Color.FromArgb(objColorDialog.Color.R, objColorDialog.Color.G, objColorDialog.Color.B);

                        var FGbrush2 = (System.Windows.Media.Brush)converter.ConvertFromString($"#{myFGColor.Name}");
                        setForeColor = ColorKeyValues[1].PendingSellOrder = FGbrush2;
                    }
                    else if (SelectedScreenElement == Enumerations.PendingOrderColor.BuyStoplossOrder.ToString())
                    {
                        System.Drawing.Color myFGColor = System.Drawing.Color.FromArgb(objColorDialog.Color.R, objColorDialog.Color.G, objColorDialog.Color.B);

                        var FGbrush3 = (System.Windows.Media.Brush)converter.ConvertFromString($"#{myFGColor.Name}");
                        setForeColor = ColorKeyValues[1].PendingBuyStoploss = FGbrush3;
                    }
                    else if (SelectedScreenElement == Enumerations.PendingOrderColor.SellStoplossOrder.ToString())
                    {
                        System.Drawing.Color myFGColor = System.Drawing.Color.FromArgb(objColorDialog.Color.R, objColorDialog.Color.G, objColorDialog.Color.B);

                        var FGbrush4 = (System.Windows.Media.Brush)converter.ConvertFromString($"#{myFGColor.Name}");
                        setForeColor = ColorKeyValues[1].PendingSellStoploss = FGbrush4;
                    }
                    else if (SelectedScreenElement == Enumerations.PendingOrderColor.RRMBuyOrder.ToString())
                    {
                        System.Drawing.Color myFGColor = System.Drawing.Color.FromArgb(objColorDialog.Color.R, objColorDialog.Color.G, objColorDialog.Color.B);

                        var FGbrush5 = (System.Windows.Media.Brush)converter.ConvertFromString($"#{myFGColor.Name}");
                        setForeColor = ColorKeyValues[1].PendingRRMBuy = FGbrush5;
                    }
                    else if (SelectedScreenElement == Enumerations.PendingOrderColor.RRMSellOrder.ToString())
                    {
                        System.Drawing.Color myFGColor = System.Drawing.Color.FromArgb(objColorDialog.Color.R, objColorDialog.Color.G, objColorDialog.Color.B);

                        var FGbrush6 = (System.Windows.Media.Brush)converter.ConvertFromString($"#{myFGColor.Name}");
                        setForeColor = ColorKeyValues[1].PendingRRMSell = FGbrush6;

                    }

                }

                if (SelectedWindow == Enumerations.WindowsAvailable.Trades.ToString())
                {
                    if (SelectedScreenElement == Enumerations.TradesColor.BackgroundColor.ToString())
                    {
                        setBgColor = ColorKeyValues[2].TradesBackGround = BGbrush;
                    }
                    else if (SelectedScreenElement == Enumerations.TradesColor.BuyTrade.ToString())
                    {
                        System.Drawing.Color myFGColor = System.Drawing.Color.FromArgb(objColorDialog.Color.R, objColorDialog.Color.G, objColorDialog.Color.B);

                        var FGbrush1 = (System.Windows.Media.Brush)converter.ConvertFromString($"#{myFGColor.Name}");
                        setForeColor = ColorKeyValues[2].BuyTrade = FGbrush1;
                        setBgColor = ColorKeyValues[2].TradesBackGround;
                    }
                    else if (SelectedScreenElement == Enumerations.TradesColor.SellTrade.ToString())
                    {
                        System.Drawing.Color myFGColor = System.Drawing.Color.FromArgb(objColorDialog.Color.R, objColorDialog.Color.G, objColorDialog.Color.B);

                        var FGbrush2 = (System.Windows.Media.Brush)converter.ConvertFromString($"#{myFGColor.Name}");
                        setForeColor = ColorKeyValues[2].SellTrade = FGbrush2;
                        setBgColor = ColorKeyValues[2].TradesBackGround;
                    }
                    else if (SelectedScreenElement == Enumerations.TradesColor.SpreadBackgroundColor.ToString())
                    {
                        System.Drawing.Color myFGColor = System.Drawing.Color.FromArgb(objColorDialog.Color.R, objColorDialog.Color.G, objColorDialog.Color.B);

                        var FGbrush5 = (System.Windows.Media.Brush)converter.ConvertFromString($"#{myFGColor.Name}");
                        //setForeColor = ColorKeyValues[2].SpreadBGColor = FGbrush5;

                        setBgColor = ColorKeyValues[2].SpreadBGColor = BGbrush;
                    }

                    else if (SelectedScreenElement == Enumerations.TradesColor.BuySpreadTrade.ToString())
                    {
                        System.Drawing.Color myFGColor = System.Drawing.Color.FromArgb(objColorDialog.Color.R, objColorDialog.Color.G, objColorDialog.Color.B);

                        var FGbrush3 = (System.Windows.Media.Brush)converter.ConvertFromString($"#{myFGColor.Name}");
                        setForeColor = ColorKeyValues[2].BuySpreadTrade = FGbrush3;
                        setBgColor = ColorKeyValues[2].SpreadBGColor;
                    }
                    else if (SelectedScreenElement == Enumerations.TradesColor.SellSpreadTrade.ToString())
                    {
                        System.Drawing.Color myFGColor = System.Drawing.Color.FromArgb(objColorDialog.Color.R, objColorDialog.Color.G, objColorDialog.Color.B);

                        var FGbrush4 = (System.Windows.Media.Brush)converter.ConvertFromString($"#{myFGColor.Name}");
                        setForeColor = ColorKeyValues[2].SellSpreadTrade = FGbrush4;
                        setBgColor = ColorKeyValues[2].SpreadBGColor;
                    }
                   
                }

                //if (SelectedWindow == Enumerations.WindowsAvailable.TimeAndSales.ToString())
                //{
                //    if (SelectedScreenElement == Enumerations.TimeAndSalesColor.ScripListBackground.ToString())
                //    {
                //        setBgColor = ColorKeyValues[3].ScripListBackground = BGbrush;
                //    }
                //    else if (SelectedScreenElement == Enumerations.TimeAndSalesColor.ScripListUptrend.ToString())
                //    {
                //        System.Drawing.Color myFGColor = System.Drawing.Color.FromArgb(objColorDialog.Color.R, objColorDialog.Color.G, objColorDialog.Color.B);
                //        var FGbrush1 = (System.Windows.Media.Brush)converter.ConvertFromString($"#{myFGColor.Name}");
                //        setForeColor = ColorKeyValues[3].ScripListUptrend = FGbrush1;
                //        setBgColor = ColorKeyValues[3].ScripListBackground;
                //    }
                //    else if (SelectedScreenElement == Enumerations.TimeAndSalesColor.ScripListDowntrend.ToString())
                //    {
                //        System.Drawing.Color myFGColor = System.Drawing.Color.FromArgb(objColorDialog.Color.R, objColorDialog.Color.G, objColorDialog.Color.B);

                //        var FGbrush2 = (System.Windows.Media.Brush)converter.ConvertFromString($"#{myFGColor.Name}");
                //        setForeColor = ColorKeyValues[3].ScripListDowntrend = FGbrush2;
                //        setBgColor = ColorKeyValues[3].ScripListBackground;
                //    }
                //    else if (SelectedScreenElement == Enumerations.TimeAndSalesColor.ScripDataBackground.ToString())
                //    {
                //        System.Drawing.Color myFGColor = System.Drawing.Color.FromArgb(objColorDialog.Color.R, objColorDialog.Color.G, objColorDialog.Color.B);

                //        var FGbrush3 = (System.Windows.Media.Brush)converter.ConvertFromString($"#{myFGColor.Name}");
                //        setBgColor = ColorKeyValues[3].ScripDataBackground = BGbrush;
                //        // setForeColor = ColorKeyValues[3].ScripDataBackground = FGbrush3;
                //    }
                //    else if (SelectedScreenElement == Enumerations.TimeAndSalesColor.ScripDataUptrend.ToString())
                //    {
                //        System.Drawing.Color myFGColor = System.Drawing.Color.FromArgb(objColorDialog.Color.R, objColorDialog.Color.G, objColorDialog.Color.B);

                //        var FGbrush4 = (System.Windows.Media.Brush)converter.ConvertFromString($"#{myFGColor.Name}");
                //        setForeColor = ColorKeyValues[3].ScripDataUptrend = FGbrush4;
                //        setBgColor = ColorKeyValues[3].ScripDataBackground;
                //    }
                //    else if (SelectedScreenElement == Enumerations.TimeAndSalesColor.ScripDataDowntrend.ToString())
                //    {
                //        System.Drawing.Color myFGColor = System.Drawing.Color.FromArgb(objColorDialog.Color.R, objColorDialog.Color.G, objColorDialog.Color.B);

                //        var FGbrush5 = (System.Windows.Media.Brush)converter.ConvertFromString($"#{myFGColor.Name}");
                //        setForeColor = ColorKeyValues[3].ScripDataDowntrend = FGbrush5;
                //        setBgColor = ColorKeyValues[3].ScripDataBackground;
                //    }
                //}

            }

        }

        private void OnChangeOfWindowSelection()
        {
            //if (SelectedWindow == Enumerations.WindowsAvailable.Touchline.ToString())
            //{
            //    PopulateScreenElementsTouchline();
            //    TouchlineVisibility = "Visible";
            //    PendingVisibility = "Hidden";
            //    TradesVisibility = "Hidden";
            //    TimeSaleVisibility = "Hidden";
            //}
            if (SelectedWindow == Enumerations.WindowsAvailable.PendingOrder.ToString())
            {
                PopulateScreenElementsPending();
                PendingVisibility = "Visible";
                TouchlineVisibility = "Hidden";
                TradesVisibility = "Hidden";
                TimeSaleVisibility = "Hidden";
            }
            else if (SelectedWindow == Enumerations.WindowsAvailable.Trades.ToString())
            {
                PopulateScreenElementsTrades();
                TradesVisibility = "Visible";
                PendingVisibility = "Hidden";
                TouchlineVisibility = "Hidden";
                TimeSaleVisibility = "Hidden";
            }
            //else if (SelectedWindow == Enumerations.WindowsAvailable.TimeAndSales.ToString())
            //{
            //    PopulateScreenElementsTimeSale();
            //    TimeSaleVisibility = "Visible";
            //    PendingVisibility = "Hidden";
            //    TouchlineVisibility = "Hidden";
            //    TradesVisibility = "Hidden";
            //}
        }

        private void ReadColorFromFile()
        {
            if (MainWindowVM.ParserDefaultCPR != null)
            {
                //Read from File and Add values in respective key
                //#region TouchLine
                //ColorVariable objTouchLine = new ColorVariable();
                //objTouchLine.TouchLineBackGround = MainWindowVM.ParserDefaultCPR.GetSetting("TOUCHLINE", "TouchLineBackGround") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("TOUCHLINE", "TouchLineBackGround")) as SolidColorBrush : null;
                //objTouchLine.TouchLineForeGround = MainWindowVM.ParserDefaultCPR.GetSetting("TOUCHLINE", "TouchLineForeGround") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("TOUCHLINE", "TouchLineForeGround")) as SolidColorBrush : null;
                //objTouchLine.TouchLineUptrend = MainWindowVM.ParserDefaultCPR.GetSetting("TOUCHLINE", "TouchLineUptrend") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("TOUCHLINE", "TouchLineUptrend")) as SolidColorBrush : null;
                //objTouchLine.TouchLineDowntrend = MainWindowVM.ParserDefaultCPR.GetSetting("TOUCHLINE", "TouchLineDowntrend") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("TOUCHLINE", "TouchLineDowntrend")) as SolidColorBrush : null;
                //objTouchLine.TouchLineQuantityRate = MainWindowVM.ParserDefaultCPR.GetSetting("TOUCHLINE", "TouchLineQuantityRate") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("TOUCHLINE", "TouchLineQuantityRate")) as SolidColorBrush : null;
                //objTouchLine.TouchLineUptrendFlash = MainWindowVM.ParserDefaultCPR.GetSetting("TOUCHLINE", "TouchLineUptrendFlash") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("TOUCHLINE", "TouchLineUptrendFlash")) as SolidColorBrush : null;
                //objTouchLine.TouchLineDowntrendFlash = MainWindowVM.ParserDefaultCPR.GetSetting("TOUCHLINE", "TouchLineDowntrendFlash") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("TOUCHLINE", "TouchLineDowntrendFlash")) as SolidColorBrush : null;
                //ColorKeyValues.Add(0, objTouchLine);
                //#endregion

                #region Pending
                ColorVariable objPending = new ColorVariable();
                objPending.PendingBackGround = MainWindowVM.ParserDefaultCPR.GetSetting("PENDING", "PendingBackGround") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("PENDING", "PendingBackGround")) as SolidColorBrush : null;
                objPending.PendingBuyOrder = MainWindowVM.ParserDefaultCPR.GetSetting("PENDING", "PendingBuyOrder") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("PENDING", "PendingBuyOrder")) as SolidColorBrush : null;
                objPending.PendingSellOrder = MainWindowVM.ParserDefaultCPR.GetSetting("PENDING", "PendingSellOrder") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("PENDING", "PendingSellOrder")) as SolidColorBrush : null;
                objPending.PendingBuyStoploss = MainWindowVM.ParserDefaultCPR.GetSetting("PENDING", "PendingBuyStoploss") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("PENDING", "PendingBuyStoploss")) as SolidColorBrush : null;
                objPending.PendingSellStoploss = MainWindowVM.ParserDefaultCPR.GetSetting("PENDING", "PendingSellStoploss") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("PENDING", "PendingSellStoploss")) as SolidColorBrush : null;
                objPending.PendingRRMBuy = MainWindowVM.ParserDefaultCPR.GetSetting("PENDING", "PendingRRMBuy") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("PENDING", "PendingRRMBuy")) as SolidColorBrush : null;
                objPending.PendingRRMSell = MainWindowVM.ParserDefaultCPR.GetSetting("PENDING", "PendingRRMSell") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("PENDING", "PendingRRMSell")) as SolidColorBrush : null;
                ColorKeyValues.Add(1, objPending);

                #endregion

                #region Trades
                ColorVariable objTrades = new ColorVariable();
                objTrades.TradesBackGround = MainWindowVM.ParserDefaultCPR.GetSetting("TRADES", "TradesBackGround") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("TRADES", "TradesBackGround")) as SolidColorBrush : null;
                objTrades.BuyTrade = MainWindowVM.ParserDefaultCPR.GetSetting("TRADES", "BuyTrade") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("TRADES", "BuyTrade")) as SolidColorBrush : null;
                objTrades.SellTrade = MainWindowVM.ParserDefaultCPR.GetSetting("TRADES", "SellTrade") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("TRADES", "SellTrade")) as SolidColorBrush : null;
                objTrades.BuySpreadTrade = MainWindowVM.ParserDefaultCPR.GetSetting("TRADES", "BuySpreadTrade") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("TRADES", "BuySpreadTrade")) as SolidColorBrush : null;
                objTrades.SellSpreadTrade = MainWindowVM.ParserDefaultCPR.GetSetting("TRADES", "SellSpreadTrade") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("TRADES", "SellSpreadTrade")) as SolidColorBrush : null;
                objTrades.SpreadBGColor = MainWindowVM.ParserDefaultCPR.GetSetting("TRADES", "SpreadBGColor") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("TRADES", "SpreadBGColor")) as SolidColorBrush : null;
                // objTrades.PendingRRMSell = MainWindowVM.ParserDefaultCPR.GetSetting("TRADES", "PendingRRMSell") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("TRADES", "PendingRRMSell")) as SolidColorBrush : null;
                ColorKeyValues.Add(2, objTrades);

                #endregion

                //#region TimeAndSales
                //ColorVariable objTimeAndSales = new ColorVariable();
                //objTimeAndSales.ScripListBackground = MainWindowVM.ParserDefaultCPR.GetSetting("TIMEANDSALES", "ScripListBackground") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("TIMEANDSALES", "ScripListBackground")) as SolidColorBrush : null;
                //objTimeAndSales.ScripListUptrend = MainWindowVM.ParserDefaultCPR.GetSetting("TIMEANDSALES", "ScripListUptrend") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("TIMEANDSALES", "ScripListUptrend")) as SolidColorBrush : null;
                //objTimeAndSales.ScripListDowntrend = MainWindowVM.ParserDefaultCPR.GetSetting("TIMEANDSALES", "ScripListDowntrend") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("TIMEANDSALES", "ScripListDowntrend")) as SolidColorBrush : null;
                //objTimeAndSales.ScripDataBackground = MainWindowVM.ParserDefaultCPR.GetSetting("TIMEANDSALES", "ScripDataBackground") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("TIMEANDSALES", "ScripDataBackground")) as SolidColorBrush : null;
                //objTimeAndSales.ScripDataUptrend = MainWindowVM.ParserDefaultCPR.GetSetting("TIMEANDSALES", "ScripDataUptrend") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("TIMEANDSALES", "ScripDataUptrend")) as SolidColorBrush : null;
                //objTimeAndSales.ScripDataDowntrend = MainWindowVM.ParserDefaultCPR.GetSetting("TIMEANDSALES", "ScripDataDowntrend") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("TIMEANDSALES", "ScripDataDowntrend")) as SolidColorBrush : null;
                ////  objTimeAndSales.PendingRRMSell = MainWindowVM.ParserDefaultCPR.GetSetting("TIMEANDSALES", "PendingRRMSell") != null ? objBrushConvertor.ConvertFromString(MainWindowVM.ParserDefaultCPR.GetSetting("PENDING", "PendingRRMSell")) as SolidColorBrush : null;
                //ColorKeyValues.Add(3, objTimeAndSales);

                //#endregion
            }

        }

        private void OnChangeOfSelectedScreenElement()
        {
            //if (SelectedWindow == Enumerations.WindowsAvailable.Touchline.ToString())
            //{
            //    if (SelectedScreenElement == Enumerations.TouchlineColor.BackgroundColor.ToString())
            //    {
            //        setBgColor = ColorKeyValues[0].TouchLineBackGround;
            //        setForeColor = null;
            //    }
            //    else if (SelectedScreenElement == Enumerations.TouchlineColor.ForeGroundColor.ToString())
            //    {
            //        setForeColor = ColorKeyValues[0].TouchLineForeGround;
            //    }
            //    else if (SelectedScreenElement == Enumerations.TouchlineColor.Uptrend.ToString())
            //    {
            //        setForeColor = ColorKeyValues[0].TouchLineUptrend;
            //    }
            //    else if (SelectedScreenElement == Enumerations.TouchlineColor.Downtrend.ToString())
            //    {
            //        setForeColor = ColorKeyValues[0].TouchLineDowntrend;
            //    }
            //    else if (SelectedScreenElement == Enumerations.TouchlineColor.QuantityRate.ToString())
            //    {
            //        setForeColor = ColorKeyValues[0].TouchLineQuantityRate;
            //    }

            //    else if (SelectedScreenElement == Enumerations.TouchlineColor.UptrendFlash.ToString())
            //    {
            //        setBgColor = ColorKeyValues[0].TouchLineUptrendFlash;
            //        setBackGroundColor();
            //    }

            //    else if (SelectedScreenElement == Enumerations.TouchlineColor.DowntrendFlash.ToString())
            //    {
            //        setBgColor = ColorKeyValues[0].TouchLineDowntrendFlash;
            //        setBackGroundColor();
            //    }

            //}

            if (SelectedWindow == Enumerations.WindowsAvailable.PendingOrder.ToString())
            {
                if (SelectedScreenElement == Enumerations.PendingOrderColor.BackgroundColor.ToString())
                {
                    setBgColor = ColorKeyValues[1].PendingBackGround;
                    setForeColor = null;
                }
                else if (SelectedScreenElement == Enumerations.PendingOrderColor.BuyOrder.ToString())
                {
                    setForeColor = ColorKeyValues[1].PendingBuyOrder;
                }
                else if (SelectedScreenElement == Enumerations.PendingOrderColor.SellOrder.ToString())
                {
                    setForeColor = ColorKeyValues[1].PendingSellOrder;
                }
                else if (SelectedScreenElement == Enumerations.PendingOrderColor.BuyStoplossOrder.ToString())
                {
                    setForeColor = ColorKeyValues[1].PendingBuyStoploss;
                }
                else if (SelectedScreenElement == Enumerations.PendingOrderColor.SellStoplossOrder.ToString())
                {
                    setForeColor = ColorKeyValues[1].PendingSellStoploss;
                }

                else if (SelectedScreenElement == Enumerations.PendingOrderColor.RRMBuyOrder.ToString())
                {
                    setForeColor = ColorKeyValues[1].PendingRRMBuy;
                    //setBackGroundColor();
                }

                else if (SelectedScreenElement == Enumerations.PendingOrderColor.RRMSellOrder.ToString())
                {
                    setForeColor = ColorKeyValues[1].PendingRRMSell;
                    //setBackGroundColor();
                }
              
            }

            if (SelectedWindow == Enumerations.WindowsAvailable.Trades.ToString())
            {
                if (SelectedScreenElement == Enumerations.TradesColor.BackgroundColor.ToString())
                {
                    setBgColor = ColorKeyValues[2].TradesBackGround;
                    setForeColor = null;
                }
                else if (SelectedScreenElement == Enumerations.TradesColor.BuyTrade.ToString())
                {
                    setForeColor = ColorKeyValues[2].BuyTrade;
                    setBgColor = ColorKeyValues[2].TradesBackGround;
                }
                else if (SelectedScreenElement == Enumerations.TradesColor.SellTrade.ToString())
                {
                    setForeColor = ColorKeyValues[2].SellTrade;
                    setBgColor = ColorKeyValues[2].TradesBackGround;
                }
                else if (SelectedScreenElement == Enumerations.TradesColor.SpreadBackgroundColor.ToString())
                {
                    //  setBgColor = ColorKeyValues[2].SpreadBGColor;
                    setForeColor = null;
                    setBgColor = ColorKeyValues[2].SpreadBGColor;
                    //setBackGroundColor();
                }
                else if (SelectedScreenElement == Enumerations.TradesColor.BuySpreadTrade.ToString())
                {
                    setForeColor = ColorKeyValues[2].BuySpreadTrade;
                    setBgColor = ColorKeyValues[2].SpreadBGColor;
                }
                else if (SelectedScreenElement == Enumerations.TradesColor.SellSpreadTrade.ToString())
                {
                    setForeColor = ColorKeyValues[2].SellSpreadTrade;
                    setBgColor = ColorKeyValues[2].SpreadBGColor;
                }

                

                //else if (SelectedScreenElement == Enumerations.TradesColor.RRMSellOrder.ToString())
                //{
                //    setBgColor = ColorKeyValues[1].PendingRRMSell;
                //    //setBackGroundColor();
                //}

                //TODO
            }

            //if (SelectedWindow == Enumerations.WindowsAvailable.TimeAndSales.ToString())
            //{

            //    if (SelectedScreenElement == Enumerations.TimeAndSalesColor.ScripListBackground.ToString())
            //    {
            //        setBgColor = ColorKeyValues[3].ScripListBackground;
            //        setForeColor = null;
            //    }
            //    else if (SelectedScreenElement == Enumerations.TimeAndSalesColor.ScripListUptrend.ToString())
            //    {
            //        setForeColor = ColorKeyValues[3].ScripListUptrend;
            //        setBgColor = ColorKeyValues[3].ScripListBackground;
            //    }
            //    else if (SelectedScreenElement == Enumerations.TimeAndSalesColor.ScripListDowntrend.ToString())
            //    {
            //        setForeColor = ColorKeyValues[3].ScripListDowntrend;
            //        setBgColor = ColorKeyValues[3].ScripListBackground;
            //    }
            //    else if (SelectedScreenElement == Enumerations.TimeAndSalesColor.ScripDataBackground.ToString())
            //    {
            //        setBgColor = ColorKeyValues[3].ScripDataBackground;
            //        setForeColor = null;
            //    }
            //    else if (SelectedScreenElement == Enumerations.TimeAndSalesColor.ScripDataUptrend.ToString())
            //    {
            //        setForeColor = ColorKeyValues[3].ScripDataUptrend;
            //        setBgColor = ColorKeyValues[3].ScripDataBackground;
            //    }

            //    else if (SelectedScreenElement == Enumerations.TimeAndSalesColor.ScripDataDowntrend.ToString())
            //    {
            //        setForeColor = ColorKeyValues[3].ScripDataDowntrend;
            //        setBgColor = ColorKeyValues[3].ScripDataBackground;
            //        //setBackGroundColor();
            //    }
            //    //TODO
            //}
        }

        private void setBackGroundColor()
        {
            //if (setForeColor != null)
            //{
            //    byte a = ((System.Windows.Media.Color)(setForeColor.GetValue(SolidColorBrush.ColorProperty))).A;
            //    byte g = ((System.Windows.Media.Color)(setForeColor.GetValue(SolidColorBrush.ColorProperty))).G;
            //    byte r = ((System.Windows.Media.Color)(setForeColor.GetValue(SolidColorBrush.ColorProperty))).R;
            //    byte b = ((System.Windows.Media.Color)(setForeColor.GetValue(SolidColorBrush.ColorProperty))).B;
            //    System.Windows.Media.Color forecolor = 
            //         System.Windows.Media.Color.FromArgb(a, r, g, b);

            //    //setBgColor after converting brush to RGB
            //    System.Drawing.Color BgColor = System.Drawing.Color.FromArgb((255 - forecolor.R), (255 - forecolor.G), (255 - forecolor.B));
            //    setBgColor = (System.Windows.Media.Brush)objBrushConvertor.ConvertFromString($"#{BgColor.Name}");
            //    //setBgColor after converting brush to RGB
            //}
            if (setBgColor != null)
            {
                byte a = ((System.Windows.Media.Color)(setBgColor.GetValue(SolidColorBrush.ColorProperty))).A;
                byte g = ((System.Windows.Media.Color)(setBgColor.GetValue(SolidColorBrush.ColorProperty))).G;
                byte r = ((System.Windows.Media.Color)(setBgColor.GetValue(SolidColorBrush.ColorProperty))).R;
                byte b = ((System.Windows.Media.Color)(setBgColor.GetValue(SolidColorBrush.ColorProperty))).B;
                System.Windows.Media.Color forecolor = System.Windows.Media.Color.FromArgb(a, r, g, b);

                //setBgColor after converting brush to RGB
                System.Drawing.Color BgColor = System.Drawing.Color.FromArgb((255 - forecolor.R), (255 - forecolor.G), (255 - forecolor.B));
                setForeColor = (System.Windows.Media.Brush)objBrushConvertor.ConvertFromString($"#{BgColor.Name}");
            }
        }

        #endregion

        #region constructor

        public ColourProfilingVM()
        {
            ScreenElement = new ObservableCollection<string>();
            ColorKeyValues = new Dictionary<int, ColorVariable>();
            objBrushConvertor = new BrushConverter();
            //Add Default vales in COlorKeyValues dictionary - > Read from Files
            ReadColorFromFile();
            PopulateWindow();
            //WindowsAvailable.focus();
            // SaveDefault();
            // DropDown();
        }



        #endregion

    }

    public class ColorVariable
    {
        #region Properties

        public System.Windows.Media.Brush TouchLineBackGround { get; set; }
        public System.Windows.Media.Brush TouchLineForeGround { get; set; }
        public System.Windows.Media.Brush TouchLineUptrend { get; set; }
        public System.Windows.Media.Brush TouchLineDowntrend { get; set; }
        public System.Windows.Media.Brush TouchLineQuantityRate { get; set; }
        public System.Windows.Media.Brush TouchLineUptrendFlash { get; set; }
        public System.Windows.Media.Brush TouchLineDowntrendFlash { get; set; }
        public System.Windows.Media.Brush PendingBackGround { get; set; }
        public System.Windows.Media.Brush PendingBuyOrder { get; set; }
        public System.Windows.Media.Brush PendingSellOrder { get; set; }
        public System.Windows.Media.Brush PendingBuyStoploss { get; set; }
        public System.Windows.Media.Brush PendingSellStoploss { get; set; }
        public System.Windows.Media.Brush PendingRRMBuy { get; set; }
        public System.Windows.Media.Brush PendingRRMSell { get; set; }
        public System.Windows.Media.Brush TradesBackGround { get; set; }
        public System.Windows.Media.Brush BuyTrade { get; set; }
        public System.Windows.Media.Brush SellTrade { get; set; }
        public System.Windows.Media.Brush BuySpreadTrade { get; set; }
        public System.Windows.Media.Brush SellSpreadTrade { get; set; }
        public System.Windows.Media.Brush SpreadBGColor { get; set; }
        public System.Windows.Media.Brush ScripListBackground { get; set; }
        public System.Windows.Media.Brush ScripListUptrend { get; set; }
        public System.Windows.Media.Brush ScripListDowntrend { get; set; }
        public System.Windows.Media.Brush ScripDataBackground { get; set; }
        public System.Windows.Media.Brush ScripDataUptrend { get; set; }
        public System.Windows.Media.Brush ScripDataDowntrend { get; set; }




        #endregion
    }

}




