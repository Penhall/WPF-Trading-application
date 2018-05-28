using CommonFrontEnd.Common;
using CommonFrontEnd.Model;
using CommonFrontEnd.Model.Order;
using CommonFrontEnd.SharedMemories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CommonFrontEnd.Common.Enumerations;
using BroadcastMaster;
using CommonFrontEnd.View;
using System.Collections.Specialized;
using CommonFrontEnd.ViewModel.Touchline;
using System.Windows;
using CommonFrontEnd.View.Touchline;
using System.Windows.Controls;
using System.Threading;
using CommonFrontEnd.View.DigitalClock;

namespace CommonFrontEnd.ViewModel
{
    public partial class indexDetailsVM
    {
        #region properies
        static IndexDetails mWindow = null;
        private static string _SelectedExchange = Enumerations.Order.Exchanges.BSE.ToString();

        public static string SelectedExchange
        {
            get { return _SelectedExchange; }
            set
            {
                _SelectedExchange = value; NotifyStaticPropertyChanged("SelectedExchange");
                populateGrid();


            }
        }



        private List<string> _ExchangeLst;

        public List<string> ExchangeLst
        {
            get { return _ExchangeLst; }
            set { _ExchangeLst = value; NotifyPropertyChanged("SelectedExchange"); }
        }

        private static string _bowVisibility;

        public static string bowVisibility
        {
            get { return _bowVisibility; }
            set { _bowVisibility = value; NotifyStaticPropertyChanged("bowVisibility"); }
        }
        //private static ObservableCollection<AllIndicesModel> _ObjIndicesCollection = new ObservableCollection<AllIndicesModel>();
        //public static ObservableCollection<AllIndicesModel> ObjIndicesCollection
        //{
        //    get { return _ObjIndicesCollection; }
        //    set
        //    {
        //        _ObjIndicesCollection = value;
        //        NotifyStaticPropertyChanged("ObjIndicesCollection");
        //    }
        //}
        private static ObservableCollection<AllIndicesModel> _ObjIndicesCollection;

        public static ObservableCollection<AllIndicesModel> ObjIndicesCollection
        {
            get { return _ObjIndicesCollection; }
            set { _ObjIndicesCollection = value; /*NotifyStaticPropertyChanged(nameof(ObjIndicesCollection));*/ }
        }


        private AllIndicesModel _SelectedItem;

        public AllIndicesModel SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; NotifyPropertyChanged(nameof(SelectedItem)); }
        }


        #endregion

        #region relayCommand
        private RelayCommand _ShortCut_Escape;

        public RelayCommand ShortCut_Escape
        {
            get
            {
                return _ShortCut_Escape ?? (_ShortCut_Escape = new RelayCommand(
                    (object e) => EscapeUsingUserControl(e)
                        ));
            }
        }

        private RelayCommand _CloseWindowsOnEscape;

        public RelayCommand CloseWindowsOnEscape
        {
            get
            {
                return _CloseWindowsOnEscape ?? (_CloseWindowsOnEscape = new RelayCommand(
                    (object e) => CloseWindowsOnEscape_Click()
                        ));
            }
        }

        private RelayCommand _ShowintTouchline;

        public RelayCommand ShowintTouchline
        {
            get
            {
                return _ShowintTouchline ?? (_ShowintTouchline = new RelayCommand(
                    (object e) => ShowintTouchline_Click()
                        ));
            }
        }

        private RelayCommand _ShowSensexGraph;

        public RelayCommand ShowSensexGraph
        {
            get
            {
                return _ShowSensexGraph ?? (_ShowSensexGraph = new RelayCommand(
                    (object e) => ShowSensexGraph_Click()
                        ));
            }
        }

        #endregion
        #region constructor
        public indexDetailsVM()
        {
            ObjIndicesCollection = new ObservableCollection<AllIndicesModel>();
            mWindow = System.Windows.Application.Current.Windows.OfType<IndexDetails>().FirstOrDefault();
            populateExchages();
#if TWS
            MainWindowVM.indicesBroadCast += objIndicesBroadCastProcessor_OnBroadCastRecievedNew;
#elif BOW
            SettingsManager.indicesBroadCastBow += objIndicesBroadCastProcessor_OnBroadCastRecievedNewBow;
#endif
        }

        internal void objIndicesBroadCastProcessor_OnBroadCastRecievedNewBow(IdicesDetailsMain obj)
        {
            try
            {
                if (SelectedExchange == Enumerations.Order.Exchanges.BSE.ToString())
                {
                    if (obj != null && ObjIndicesCollection.Count > 0)
                    {
                        foreach (var item in SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict)
                        {
                            int? index = null;
                            if (ObjIndicesCollection.Any(x => x.IndexCode == item.Key))
                            {
                                index = ObjIndicesCollection.Where(i => i.IndexCode == item.Key).Select(x => ObjIndicesCollection.IndexOf(x)).FirstOrDefault();
                            }
                            if (index != null)
                            {
                                ObjIndicesCollection[(int)index].IndexValue = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[item.Key].IndexValue / Math.Pow(10, 2);
                                ObjIndicesCollection[(int)index].IndexHigh = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[item.Key].IndexHigh / Math.Pow(10, 2);
                                ObjIndicesCollection[(int)index].IndexLow = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[item.Key].IndexLow / Math.Pow(10, 2);
                                ObjIndicesCollection[(int)index].IndexOpen = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[item.Key].IndexOpen / Math.Pow(10, 2);
                                ObjIndicesCollection[(int)index].PreviousIndexClose = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[item.Key].PreviousIndexClose / Math.Pow(10, 2);
                                if (ObjIndicesCollection[(int)index].IndexValue != null && ObjIndicesCollection[(int)index].PreviousIndexClose != null)
                                {
                                    ObjIndicesCollection[(int)index].chngInValue = $"{ObjIndicesCollection[(int)index].IndexValue - ObjIndicesCollection[(int)index].PreviousIndexClose:0.00}";
                                }//if (ObjIndicesCollection[(int)index].IndexValue != 0 && ObjIndicesCollection[(int)index].PreviousIndexClose != 0)
                                // if (Math.Round(Convert.ToDouble((ObjIndicesCollection[(int)index].IndexValue - ObjIndicesCollection[(int)index].PreviousIndexClose) / ObjIndicesCollection[(int)index].PreviousIndexClose * 100), 2) > 0)
                                //  ObjIndicesCollection[(int)index].perChange = string.Format("{0}{1} {2}", "+", Math.Round(Convert.ToDouble((ObjIndicesCollection[(int)index].IndexValue - ObjIndicesCollection[(int)index].PreviousIndexClose) / ObjIndicesCollection[(int)index].PreviousIndexClose * 100), 2).ToString("0.00"), "%");
                                // else
                                //     ObjIndicesCollection[(int)index].perChange = string.Format("{0} {1}", Math.Round(Convert.ToDouble((ObjIndicesCollection[(int)index].IndexValue - ObjIndicesCollection[(int)index].PreviousIndexClose) / ObjIndicesCollection[(int)index].PreviousIndexClose * 100), 2).ToString("0.00"), "%");
                            }
                        }
                        // NotifyStaticPropertyChanged(nameof(ObjIndicesCollection));
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

        #endregion

        internal static void objIndicesBroadCastProcessor_OnBroadCastRecievedNew(IdicesDetailsMain objIndexMain)
        {
            try
            {
                if (SelectedExchange == Enumerations.Order.Exchanges.BSE.ToString())
                {
                    if (objIndexMain != null && ObjIndicesCollection.Count > 0)
                    {
                        IdicesDetailsMain item = objIndexMain;
                        //foreach (var item in objIndexMain.listindMain)
                        {
                            int? index = null;
                            if (ObjIndicesCollection.Any(x => x.IndexCode == item.IndexCode))
                            {
                                index = ObjIndicesCollection.Where(i => i.IndexCode == item.IndexCode).Select(x => ObjIndicesCollection.IndexOf(x)).FirstOrDefault();
                            }
                            if (index != null)
                            {
                                ObjIndicesCollection[(int)index].IndexValue = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[item.IndexCode].IndexValue / Math.Pow(10, 2);
                                ObjIndicesCollection[(int)index].IndexHigh = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[item.IndexCode].IndexHigh / Math.Pow(10, 2);
                                ObjIndicesCollection[(int)index].IndexLow = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[item.IndexCode].IndexLow / Math.Pow(10, 2);
                                ObjIndicesCollection[(int)index].IndexOpen = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[item.IndexCode].IndexOpen / Math.Pow(10, 2);
                                ObjIndicesCollection[(int)index].PreviousIndexClose = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[item.IndexCode].PreviousIndexClose / Math.Pow(10, 2);
                                if (ObjIndicesCollection[(int)index].IndexValue != 0 && ObjIndicesCollection[(int)index].PreviousIndexClose != 0)
                                    if (Math.Round(Convert.ToDouble((ObjIndicesCollection[(int)index].IndexValue - ObjIndicesCollection[(int)index].PreviousIndexClose) / ObjIndicesCollection[(int)index].PreviousIndexClose * 100), 2) > 0)
                                    {
                                        ObjIndicesCollection[(int)index].perChange = string.Format("{0}{1}", "+", Math.Round(Convert.ToDouble((ObjIndicesCollection[(int)index].IndexValue - ObjIndicesCollection[(int)index].PreviousIndexClose) / ObjIndicesCollection[(int)index].PreviousIndexClose * 100), 2).ToString("0.00"));
                                        //ColorFont = "Red";
                                    }
                                    else
                                    {
                                        ObjIndicesCollection[(int)index].perChange = string.Format("{0}", Math.Round(Convert.ToDouble((ObjIndicesCollection[(int)index].IndexValue - ObjIndicesCollection[(int)index].PreviousIndexClose) / ObjIndicesCollection[(int)index].PreviousIndexClose * 100), 2).ToString("0.00"));
                                    }
                                //ObjIndicesCollection[(int)index].chngInValue = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[item.IndexCode].IndexChangeValue;
                                if (ObjIndicesCollection[(int)index].IndexValue != 0 && ObjIndicesCollection[(int)index].PreviousIndexClose != 0)
                                {
                                    ObjIndicesCollection[(int)index].chngInValue = $"{ObjIndicesCollection[(int)index].IndexValue - ObjIndicesCollection[(int)index].PreviousIndexClose:0.00}";
                                }

                            }
                        }
                        // NotifyStaticPropertyChanged(nameof(ObjIndicesCollection));
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }

        }

        private static void populateGrid()
        {
            // MainWindowVM.indicesBroadCast -= objIndicesBroadCastProcessor_OnBroadCastRecievedNew;
            if (SelectedExchange == Enumerations.Order.Exchanges.BSE.ToString())
            {

                foreach (var item in MasterSharedMemory.objMasterDicSyb)
                {
                    AllIndicesModel oAllIndicesModel = new AllIndicesModel();
                    oAllIndicesModel.IndexCode = item.Key;
                    oAllIndicesModel.IndexId = item.Value.IndexID;
                    bool hasNonEmptyObject = MasterSharedMemory.objSpnIndicesDic.Values.Any(x => x.IndexCode == item.Key);
                    if (hasNonEmptyObject)
                        oAllIndicesModel.IndexName = MasterSharedMemory.objSpnIndicesDic.Values.Where(x => x.IndexCode == item.Key).Select(x => x.RebrandedLongName_ca).FirstOrDefault();
                    else
                        oAllIndicesModel.IndexName = item.Value.IndexID;

                    if (SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict.Keys.Contains(oAllIndicesModel.IndexCode))
                    {
                        oAllIndicesModel.IndexValue = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[oAllIndicesModel.IndexCode].IndexValue / Math.Pow(10, 2);
                        oAllIndicesModel.IndexHigh = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[oAllIndicesModel.IndexCode].IndexHigh / Math.Pow(10, 2);
                        oAllIndicesModel.IndexLow = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[oAllIndicesModel.IndexCode].IndexLow / Math.Pow(10, 2);
                        oAllIndicesModel.IndexOpen = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[oAllIndicesModel.IndexCode].IndexOpen / Math.Pow(10, 2);
                        oAllIndicesModel.PreviousIndexClose = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[oAllIndicesModel.IndexCode].PreviousIndexClose / Math.Pow(10, 2);
                        oAllIndicesModel.chngInValue = $"{oAllIndicesModel.IndexValue - oAllIndicesModel.PreviousIndexClose:0.00}";
#if TWS
                        if (oAllIndicesModel.IndexValue != 0 && oAllIndicesModel.PreviousIndexClose != 0)
                            if (Math.Round(Convert.ToDouble((oAllIndicesModel.IndexValue - oAllIndicesModel.PreviousIndexClose) / oAllIndicesModel.PreviousIndexClose * 100), 2) > 0)
                                oAllIndicesModel.perChange = string.Format("{0}{1}", "+", Math.Round(Convert.ToDouble((oAllIndicesModel.IndexValue - oAllIndicesModel.PreviousIndexClose) / oAllIndicesModel.PreviousIndexClose * 100), 2).ToString("0.00"));
                            else
                                oAllIndicesModel.perChange = string.Format("{0}", Math.Round(Convert.ToDouble((oAllIndicesModel.IndexValue - oAllIndicesModel.PreviousIndexClose) / oAllIndicesModel.PreviousIndexClose * 100), 2).ToString("0.00"));
#elif BOW
                        oAllIndicesModel.perChange = Convert.ToString(SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[oAllIndicesModel.IndexCode].IndexChangeValue);
#endif
                    }

                    ObjIndicesCollection.Add(oAllIndicesModel);
                }
                //MainWindowVM.indicesBroadCast += objIndicesBroadCastProcessor_OnBroadCastRecievedNew;
                //public static void objBroadCastProcessor_OnBroadCastRecievedNew(ScripDetails objScripDetails)
                //{
                //    try
                //    {
                //        int scripCode = objScripDetails.ScriptCode_BseToken_NseToken;
                //        //ScripDetails objScripDetails = BroadcastMasterMemory.objScripDetailsConDict.Where(x => x.Key == scripCode).Select(x => x.Value).FirstOrDefault();
                //        if (objScripDetails != null && ObjTouchlineDataCollection.Count > 0)
                //        {
                //            List<int> list = new List<int>();
                //            list = ObjTouchlineDataCollection.Where(i => i.Scriptcode1 == scripCode).Select(x => ObjTouchlineDataCollection.IndexOf(x)).ToList();

                //            for (int i = 0; i < list.Count; i++)
                //                {
                //                UpdateGrid(list[i], objScripDetails);

                //                }
                //         }
                //// BestFiveVM.objBroadCastProcessor_OnBroadCastRecieved(scripCode);
                //    }
                //    catch (Exception ex)
                //    {
                //         ExceptionUtility.LogError(ex);
                //        return;
                //    }
                //}
                //foreach (var item in MasterSharedMemory.objMasterDicSyb)
                //{
                //    AllIndicesModel oAllIndicesModel = new AllIndicesModel();
                //    oAllIndicesModel.IndexCode = item.Key;
                //    oAllIndicesModel.IndexId = item.Value.IndexID;
                //    if (BroadcastMaster.BroadcastMasterMemory.objIndicesDetailsConDict.Keys.Contains(oAllIndicesModel.IndexCode))
                //    {
                //        oAllIndicesModel.IndexValue = BroadcastMaster.BroadcastMasterMemory.objIndicesDetailsConDict[oAllIndicesModel.IndexCode].IndexValue;
                //        oAllIndicesModel.IndexHigh = BroadcastMaster.BroadcastMasterMemory.objIndicesDetailsConDict[oAllIndicesModel.IndexCode].IndexHigh;
                //        oAllIndicesModel.IndexLow = BroadcastMaster.BroadcastMasterMemory.objIndicesDetailsConDict[oAllIndicesModel.IndexCode].IndexLow;
                //        oAllIndicesModel.IndexOpen = BroadcastMaster.BroadcastMasterMemory.objIndicesDetailsConDict[oAllIndicesModel.IndexCode].IndexOpen;
                //        oAllIndicesModel.PreviousIndexClose = BroadcastMaster.BroadcastMasterMemory.objIndicesDetailsConDict[oAllIndicesModel.IndexCode].PreviousIndexClose;
                //    }

                //    ObjIndicesCollection.Add(oAllIndicesModel);
                //}
            }
        }

        private void EscapeUsingUserControl(object e)
        {
            IndexDetails oIndexDetails = e as IndexDetails;
            oIndexDetails?.Hide();
        }
        private void CloseWindowsOnEscape_Click()
        {
            mWindow?.Close();
        }

        private void ShowintTouchline_Click()
        {
            if (SelectedItem!=null)
            {
                MarketWatch mainwindow = System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault();

                if (mainwindow != null)
                {
                    if (mainwindow.WindowState == WindowState.Minimized)
                        mainwindow.WindowState = WindowState.Normal;
                    mainwindow.Focus();
                    mainwindow.Show();
                }
                else
                {
                    mainwindow = new MarketWatch();
                    mainwindow.Owner = System.Windows.Application.Current.MainWindow;
                    mainwindow.dataGridView1.Sorting += new DataGridSortingEventHandler(MarketWatchVM.DataGrid_Sorting);
                    mainwindow.dataGridView1.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(MarketWatchVM.DataGrid_PreviewPageUpDown);
                    mainwindow.dataGridView1.AddHandler(ScrollViewer.ScrollChangedEvent, new ScrollChangedEventHandler(MarketWatchVM.DataGrid_ScrollChanged));
                    mainwindow.Show();
                }

                MarketWatchVM.OpenMarketFromIndexWindow(SelectedItem.IndexCode);
            }
            else
            {
                MessageBox.Show("Indice is not selected to show in touchline","!Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            
        }

        private void ShowSensexGraph_Click()
        {
            SensexChart objsensexchart = Application.Current.Windows.OfType<SensexChart>().FirstOrDefault();
            objsensexchart = new SensexChart();
            objsensexchart.Show();
            objsensexchart.Activate();

        }

        #region
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, e);
            }

        }

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged
                = delegate { };
        public static void NotifyStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }


        #endregion
    }
#if TWS
    public partial class indexDetailsVM
    {

        private void populateExchages()
        {
            ExchangeLst = new List<string>();
            SelectedExchange = Enumerations.Order.Exchanges.BSE.ToString();
            ExchangeLst.Add(Enumerations.Order.Exchanges.BSE.ToString());
            bowVisibility = "Hidden";
        }


    }
#elif BOW
    partial class indexDetailsVM
    {
        private void populateExchages()
        {
            ExchangeLst = new List<string>();
            SelectedExchange = Exchange.BSE.ToString();
            ExchangeLst.Add(Exchange.BSE.ToString());
            ExchangeLst.Add(Exchange.NSE.ToString());
            ExchangeLst.Add(Exchange.BSEINX.ToString());
            ExchangeLst.Add(Exchange.USE.ToString());
        }
    }
#endif
}
