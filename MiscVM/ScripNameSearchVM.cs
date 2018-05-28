using CommonFrontEnd.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommonFrontEnd.Model;
using System.Collections.ObjectModel;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.Global;
using CommonFrontEnd.View;
using CommonFrontEnd.View.Order;
using CommonFrontEnd.ViewModel.Order;

namespace CommonFrontEnd.ViewModel
{
    class ScripNameSearchVM : BaseViewModel
    {
        #region Properties
        private BindingList<string> _ScripSegmentList;
        public BindingList<string> ScripSegmentList
        {
            get { return _ScripSegmentList; }
            set { _ScripSegmentList = value; NotifyPropertyChanged("ScripSegmentLst"); }
        }

        private string _inputScripName;
        public string inputScripName
        {
            get { return _inputScripName; }
            set { _inputScripName = value.Trim(); NotifyPropertyChanged("inputScripName"); }
        }

        private string _scripSearchResult;
        public string scripSearchResult
        {
            get { return _scripSearchResult; }
            set { _scripSearchResult = value; NotifyPropertyChanged("scripSearchResult"); }
        }

        //private string _searchBy;
        //public string searchBy
        //{
        //    get { return _searchBy; }
        //    set { _searchBy = value; NotifyPropertyChanged("searchBy"); }
        //}

        private static ScripNameSearchVM _getinstance;
        public static ScripNameSearchVM GetInstance
        {
            get
            {
                if (_getinstance == null)
                {
                    _getinstance = new ScripNameSearchVM();
                }
                return _getinstance;
            }
        }

        private string _ScripSelectedSegment = "ALL";
        public string ScripSelectedSegment
        {
            get { return _ScripSelectedSegment; }
            set
            {
                _ScripSelectedSegment = value;
                NotifyPropertyChanged("ScripSelectedSegment");
                //EnableDisable();
            }
        }

        private ObservableCollection<ScripSearchModel> _scripCollection;

        public ObservableCollection<ScripSearchModel> ScripCollection
        {
            get { return _scripCollection; }
            set { _scripCollection = value; }
        }


        private Boolean _isScripNameChecked = true;
        public Boolean isScripNameChecked
        {
            get { return _isScripNameChecked; }
            set { _isScripNameChecked = value; NotifyPropertyChanged("isScripNameChecked"); }
        }


        private Boolean _isScripIdChecked;
        public Boolean isScripIdChecked
        {
            get { return _isScripIdChecked; }
            set { _isScripIdChecked = value; NotifyPropertyChanged("isScripIdChecked"); }
        }


        private Boolean _isISINChecked;
        public Boolean isISINChecked
        {
            get { return _isISINChecked; }
            set { _isISINChecked = value; NotifyPropertyChanged("isISINChecked"); }
        }

        private string _ErrorMessage = "";

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; NotifyPropertyChanged("ErrorMessage"); }
        }

        private Boolean _isExcludeChecked = false;
        public Boolean isExcludeChecked
        {
            get { return _isExcludeChecked; }
            set { _isExcludeChecked = value; NotifyPropertyChanged("isExcludeChecked"); }
        }

        private ScripSearchModel _SelectedItem;

        public ScripSearchModel SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; NotifyPropertyChanged(nameof(SelectedItem)); }
        }

        scripNameSearch obj = null;
        #endregion


        #region Relay Commands

        private RelayCommand _findButtonClick;

        public RelayCommand findButtonClick
        {
            get { return _findButtonClick ?? (_findButtonClick = new RelayCommand((object e) => findButtonClick_click(e))); }

        }

        private RelayCommand _AddToOrderEntry;

        public RelayCommand AddToOrderEntry
        {
            get { return _AddToOrderEntry ?? (_AddToOrderEntry = new RelayCommand((object e) => AddToOrderEntry_click(e))); }

        }

        private RelayCommand _CloseOnCancel;

        public RelayCommand CloseOnCancel
        {
            get { return _CloseOnCancel?? (_CloseOnCancel = new RelayCommand((object e) => CloseOnCancel_click(e))); }
        }

        private RelayCommand _ShortCut_Escape;

        public RelayCommand ShortCut_Escape
        {
            get { return _ShortCut_Escape ?? (_ShortCut_Escape = new RelayCommand((object e) => CloseOnCancel_click(e))); }
        }
        #endregion

        #region constructors
        public ScripNameSearchVM()
        {
            ScripSegmentList = new BindingList<String>();
            ScripCollection = new ObservableCollection<ScripSearchModel>();
            populateScripSegmentList();
            obj = System.Windows.Application.Current.Windows.OfType<scripNameSearch>().FirstOrDefault();
            
        }
        #endregion

        #region Functions


        private void AddToOrderEntry_click(object e)
        {
            if (SelectedItem != null)
            {
                if (UtilityOrderDetails.GETInstance.CurrentOrderEntry?.ToLower() == Enumerations.OrderEntryWindow.Normal.ToString().ToLower())
                {
                    NormalOrderEntry objnormal = Application.Current.Windows.OfType<NormalOrderEntry>().FirstOrDefault();

                    if (objnormal != null)
                    {
                        if (objnormal.WindowState == WindowState.Minimized)
                            objnormal.WindowState = WindowState.Normal;

                        ((NormalOrderEntryVM)objnormal.DataContext).SetScripCodeandID();
                        objnormal.Focus();
                        objnormal.Show();
                        ((NormalOrderEntryVM)objnormal?.DataContext)?.AssignDefaultFocusStart(null);
                        //MemoryManager.InvokeWindowOnScripCodeSelection(Convert.ToString(UtilityLoginDetails.GETInstance.SelectedTouchLineScripCode), "Normal");
                    }
                    else
                    {
                        objnormal = new NormalOrderEntry();
                        objnormal.Owner = System.Windows.Application.Current.MainWindow;
                        //objswift.CmbExcangeType.Focus();
                        ((NormalOrderEntryVM)objnormal.DataContext).SetScripCodeandID();
                        objnormal.Activate();
                        objnormal.Show();
                    }
                }
                else if (UtilityOrderDetails.GETInstance.CurrentOrderEntry?.ToLower() == Enumerations.OrderEntryWindow.Swift.ToString().ToLower())
                {
                    SwiftOrderEntry objswift = Application.Current.Windows.OfType<SwiftOrderEntry>().FirstOrDefault();

                    if (objswift != null)
                    {
                        if (objswift.WindowState == WindowState.Minimized)
                            objswift.WindowState = WindowState.Normal;

                        //((OrderEntryVM)objswift.DataContext).BuySellWindow(e);
                        ((OrderEntryVM)objswift.DataContext).SetScripCodeandID();

                        objswift.Focus();
                        objswift.Show();
                    }
                    else
                    {
                        objswift = new SwiftOrderEntry();
                        objswift.Owner = System.Windows.Application.Current.MainWindow;
                        //objswift.CmbExcangeType.Focus();

                        //((OrderEntryVM)objswift.DataContext).BuySellWindow(e);

                        objswift.Activate();
                        objswift.Show();
                    }
                }

                MemoryManager.InvokeWindowOnScripCodeSelection?.Invoke(SelectedItem.ScripCode.ToString(), UtilityOrderDetails.GETInstance.CurrentOrderEntry.ToString());
                obj?.Close();

            }
            else
                System.Windows.Forms.MessageBox.Show("Select value", "!Warning!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            
        }
        private void CloseOnCancel_click(object e)
        {
            obj?.Close();
        }
        private void populateScripSegmentList()
        {

            ScripSegmentList.Add("ALL");
            ScripSegmentList.Add(Enumerations.Order.ScripSegment.Equity.ToString());
            ScripSegmentList.Add(Enumerations.Order.ScripSegment.Derivative.ToString());
            ScripSegmentList.Add(Enumerations.Order.ScripSegment.Currency.ToString());

        }

        private void findButtonClick_click(object e)
        {
            if (inputScripName.Trim().Length == 0)
            {
                MessageBox.Show("Please Enter At Least 1 Characters.");
                return;
            }
            else
            {
                ScripCollection.Clear();
                if (ScripSelectedSegment == "ALL")
                {
                    if (isScripNameChecked)
                    {
                        foreach (var item in MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Value.ScripName?.IndexOf(inputScripName, StringComparison.OrdinalIgnoreCase) >= 0).ToList())
                        {
                            int Lvalue= Convert.ToInt32(item.Key);
                            if (isExcludeChecked)
                            { 
                                if(item.Value.GroupName == "F" || item.Value.GroupName == "G") continue;
                                else if (Lvalue >= 400000 && Lvalue < 500000) continue;
                                else if (Lvalue >= 600000 && Lvalue < 700000) continue;
                            }
                            ScripSearchModel obj = new ScripSearchModel();
                            obj.ScripCode = Convert.ToInt32(item.Key);
                            obj.ScripName = item.Value.ScripName;
                            obj.ScripID = item.Value.ScripId;
                            obj.GroupName = item.Value.GroupName;
                            obj.ISIN = item.Value.IsinCode;
                            ScripCollection.Add(obj);
                        }

                        foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Where(x => x.Value.InstrumentName?.IndexOf(inputScripName, StringComparison.OrdinalIgnoreCase) >= 0).ToList())
                        {
                            int Lvalue = Convert.ToInt32(item.Key);
                            if (isExcludeChecked)
                            {
                                if (item.Value.GroupName == "F" || item.Value.GroupName == "G") continue;
                                else if (Lvalue >= 400000 && Lvalue < 500000) continue;
                                else if (Lvalue >= 600000 && Lvalue < 700000) continue;
                            }
                            ScripSearchModel obj = new ScripSearchModel();
                            obj.ScripCode = Convert.ToInt32(item.Key);
                            obj.ScripName = item.Value.InstrumentName;
                            obj.ScripID = item.Value.ScripId;
                            obj.GroupName = item.Value.GroupName;
                            
                            ScripCollection.Add(obj);
                        }

                        foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Where(x => x.Value.InstrumentName?.IndexOf(inputScripName, StringComparison.OrdinalIgnoreCase) >= 0).ToList())
                        {
                            int Lvalue = Convert.ToInt32(item.Key);
                            if (isExcludeChecked)
                            {
                                if (item.Value.GroupName == "F" || item.Value.GroupName == "G") continue;
                                else if (Lvalue >= 400000 && Lvalue < 500000) continue;
                                else if (Lvalue >= 600000 && Lvalue < 700000) continue;
                            }
                            ScripSearchModel obj = new ScripSearchModel();
                            obj.ScripCode = Convert.ToInt32(item.Key);
                            obj.ScripName = item.Value.InstrumentName;
                            obj.ScripID = item.Value.ScripId;
                            obj.GroupName = item.Value.GroupName;
                            ScripCollection.Add(obj);
                        }

                    }
                    else if (isScripIdChecked)
                    {
                        foreach (var item in MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Value.ScripId?.IndexOf(inputScripName, StringComparison.OrdinalIgnoreCase) >= 0).ToList())
                        {
                            int Lvalue = Convert.ToInt32(item.Key);
                            if (isExcludeChecked)
                            {
                                if (item.Value.GroupName == "F" || item.Value.GroupName == "G") continue;
                                else if (Lvalue >= 400000 && Lvalue < 500000) continue;
                                else if (Lvalue >= 600000 && Lvalue < 700000) continue;
                            }
                            ScripSearchModel obj = new ScripSearchModel();
                            obj.ScripCode = Convert.ToInt32(item.Key);
                            obj.ScripName = item.Value.ScripName;
                            obj.ScripID = item.Value.ScripId;
                            obj.GroupName = item.Value.GroupName;
                            obj.ISIN = item.Value.IsinCode;
                            ScripCollection.Add(obj);
                        }
                        foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Where(x => (x.Value.ScripId != null && x.Value.ScripId.IndexOf(inputScripName, StringComparison.OrdinalIgnoreCase) >= 0)).ToList())
                        {
                            int Lvalue = Convert.ToInt32(item.Key);
                            if (isExcludeChecked)
                            {
                                if (item.Value.GroupName == "F" || item.Value.GroupName == "G") continue;
                                else if (Lvalue >= 400000 && Lvalue < 500000) continue;
                                else if (Lvalue >= 600000 && Lvalue < 700000) continue;
                            }
                            ScripSearchModel obj = new ScripSearchModel();
                            obj.ScripCode = Convert.ToInt32(item.Key);
                            obj.ScripName = item.Value.InstrumentName;
                            obj.ScripID = item.Value.ScripId;
                            obj.GroupName = item.Value.GroupName;
                            ScripCollection.Add(obj);
                        }
                        foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Where(x => (x.Value.ScripId != null && x.Value.ScripId.IndexOf(inputScripName, StringComparison.OrdinalIgnoreCase) >= 0)).ToList())
                        {
                            int Lvalue = Convert.ToInt32(item.Key);
                            if (isExcludeChecked)
                            {
                                if (item.Value.GroupName == "F" || item.Value.GroupName == "G") continue;
                                else if (Lvalue >= 400000 && Lvalue < 500000) continue;
                                else if (Lvalue >= 600000 && Lvalue < 700000) continue;
                            }
                            ScripSearchModel obj = new ScripSearchModel();
                            obj.ScripCode = Convert.ToInt32(item.Key);
                            obj.ScripName = item.Value.InstrumentName;
                            obj.ScripID = item.Value.ScripId;
                            obj.GroupName = item.Value.GroupName;
                            ScripCollection.Add(obj);
                        }

                    }
                    else if (isISINChecked)
                    {
                        foreach (var item in MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Value.IsinCode?.IndexOf(inputScripName, StringComparison.OrdinalIgnoreCase) >= 0).ToList())
                        {
                            int Lvalue = Convert.ToInt32(item.Key);
                            if (isExcludeChecked)
                            {
                                if (item.Value.GroupName == "F" || item.Value.GroupName == "G") continue;
                                else if (Lvalue >= 400000 && Lvalue < 500000) continue;
                                else if (Lvalue >= 600000 && Lvalue < 700000) continue;
                            }
                            ScripSearchModel obj = new ScripSearchModel();
                            obj.ScripCode = Convert.ToInt32(item.Key);
                            obj.ScripName = item.Value.ScripName;
                            obj.ScripID = item.Value.ScripId;
                            obj.GroupName = item.Value.GroupName;
                            obj.ISIN = item.Value.IsinCode;
                            ScripCollection.Add(obj);
                        }

                    }
                }

                else if (ScripSelectedSegment == "Equity")
                {
                    if (isScripNameChecked)
                    {
                        foreach (var item in MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Value.ScripName?.IndexOf(inputScripName, StringComparison.OrdinalIgnoreCase) >= 0).ToList())
                        {
                            int Lvalue = Convert.ToInt32(item.Key);
                            if (isExcludeChecked)
                            {
                                if (item.Value.GroupName == "F" || item.Value.GroupName == "G") continue;
                                else if (Lvalue >= 400000 && Lvalue < 500000) continue;
                                else if (Lvalue >= 600000 && Lvalue < 700000) continue;
                            }
                            ScripSearchModel obj = new ScripSearchModel();
                            obj.ScripCode = Convert.ToInt32(item.Key);
                            obj.ScripName = item.Value.ScripName;
                            obj.ScripID = item.Value.ScripId;
                            obj.ISIN = item.Value.IsinCode;
                            obj.GroupName = item.Value.GroupName;
                            ScripCollection.Add(obj);
                        }
                    }
                    else if (isScripIdChecked)
                    {
                        foreach (var item in MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Value.ScripId?.IndexOf(inputScripName, StringComparison.OrdinalIgnoreCase) >= 0).ToList())
                        {
                            int Lvalue = Convert.ToInt32(item.Key);
                            if (isExcludeChecked)
                            {
                                if (item.Value.GroupName == "F" || item.Value.GroupName == "G") continue;
                                else if (Lvalue >= 400000 && Lvalue < 500000) continue;
                                else if (Lvalue >= 600000 && Lvalue < 700000) continue;
                            }
                            ScripSearchModel obj = new ScripSearchModel();
                            obj.ScripCode = Convert.ToInt32(item.Key);
                            obj.ScripName = item.Value.ScripName;
                            obj.ScripID = item.Value.ScripId;
                            obj.GroupName = item.Value.GroupName;
                            obj.ISIN = item.Value.IsinCode;
                            ScripCollection.Add(obj);
                        }
                    }
                    else if (isISINChecked)
                    {
                        foreach (var item in MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Value.IsinCode?.IndexOf(inputScripName, StringComparison.OrdinalIgnoreCase) >= 0).ToList())
                        {
                            int Lvalue = Convert.ToInt32(item.Key);
                            if (isExcludeChecked)
                            {
                                if (item.Value.GroupName == "F" || item.Value.GroupName == "G") continue;
                                else if (Lvalue >= 400000 && Lvalue < 500000) continue;
                                else if (Lvalue >= 600000 && Lvalue < 700000) continue;
                            }
                            ScripSearchModel obj = new ScripSearchModel();
                            obj.ScripCode = Convert.ToInt32(item.Key);
                            obj.ScripName = item.Value.ScripName;
                            obj.ScripID = item.Value.ScripId;
                            obj.GroupName = item.Value.GroupName;
                            obj.ISIN = item.Value.IsinCode;
                            ScripCollection.Add(obj);
                        }
                    }
                }
                else if (ScripSelectedSegment == "Derivative")
                {
                    if (isScripNameChecked)
                    {
                        foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Where(x => x.Value.InstrumentName?.IndexOf(inputScripName, StringComparison.OrdinalIgnoreCase) >= 0).ToList())
                        {
                            int Lvalue = Convert.ToInt32(item.Key);
                            if (isExcludeChecked)
                            {
                                if (item.Value.GroupName == "F" || item.Value.GroupName == "G") continue;
                                else if (Lvalue >= 400000 && Lvalue < 500000) continue;
                                else if (Lvalue >= 600000 && Lvalue < 700000) continue;
                            }
                            ScripSearchModel obj = new ScripSearchModel();
                            obj.ScripCode = Convert.ToInt32(item.Key);
                            obj.ScripName = item.Value.InstrumentName;
                            obj.ScripID = item.Value.ScripId;
                            obj.GroupName = item.Value.GroupName;
                            ScripCollection.Add(obj);
                        }
                    }
                    else if (isScripIdChecked)
                    {
                        foreach (var item in MasterSharedMemory.objMasterDerivativeDictBaseBSE.Where(x => x.Value.ScripId?.IndexOf(inputScripName, StringComparison.OrdinalIgnoreCase) >= 0).ToList())
                        {
                            int Lvalue = Convert.ToInt32(item.Key);
                            if (isExcludeChecked)
                            {
                                if (item.Value.GroupName == "F" || item.Value.GroupName == "G") continue;
                                else if (Lvalue >= 400000 && Lvalue < 500000) continue;
                                else if (Lvalue >= 600000 && Lvalue < 700000) continue;
                            }
                            ScripSearchModel obj = new ScripSearchModel();
                            obj.ScripCode = Convert.ToInt32(item.Key);
                            obj.ScripName = item.Value.InstrumentName;
                            obj.ScripID = item.Value.ScripId;
                            obj.GroupName = item.Value.GroupName;
                            ScripCollection.Add(obj);
                        }
                    }

                }
                else if (ScripSelectedSegment == "Currency")
                {
                    if (isScripNameChecked)
                    {
                        foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Where(x => x.Value.InstrumentName?.IndexOf(inputScripName, StringComparison.OrdinalIgnoreCase) >= 0).ToList())
                        {
                            int Lvalue = Convert.ToInt32(item.Key);
                            if (isExcludeChecked)
                            {
                                if (item.Value.GroupName == "F" || item.Value.GroupName == "G") continue;
                                else if (Lvalue >= 400000 && Lvalue < 500000) continue;
                                else if (Lvalue >= 600000 && Lvalue < 700000) continue;
                            }
                            ScripSearchModel obj = new ScripSearchModel();
                            obj.ScripCode = Convert.ToInt32(item.Key);
                            obj.ScripName = item.Value.InstrumentName;
                            obj.ScripID = item.Value.ScripId;
                            obj.GroupName = item.Value.GroupName;
                            ScripCollection.Add(obj);
                        }
                    }
                    else if (isScripIdChecked)
                    {
                        foreach (var item in MasterSharedMemory.objMasterCurrencyDictBaseBSE.Where(x => x.Value.ScripId?.IndexOf(inputScripName, StringComparison.OrdinalIgnoreCase) >= 0).ToList())
                        {
                            int Lvalue = Convert.ToInt32(item.Key);
                            if (isExcludeChecked)
                            {
                                if (item.Value.GroupName == "F" || item.Value.GroupName == "G") continue;
                                else if (Lvalue >= 400000 && Lvalue < 500000) continue;
                                else if (Lvalue >= 600000 && Lvalue < 700000) continue;
                            }
                            ScripSearchModel obj = new ScripSearchModel();
                            obj.ScripCode = Convert.ToInt32(item.Key);
                            obj.ScripName = item.Value.InstrumentName;
                            obj.ScripID = item.Value.ScripId;
                            obj.GroupName = item.Value.GroupName;
                            ScripCollection.Add(obj);
                        }
                    }

                }
            }
            if (ScripCollection.Count == 0)
            {
                ErrorMessage = "No Results Found.";
            }
            else { ErrorMessage = ScripCollection.Count.ToString()+" Records Found."; }

        }

        #endregion
    }

}
