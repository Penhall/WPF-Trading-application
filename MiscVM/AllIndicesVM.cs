using CommonFrontEnd.Common;
using CommonFrontEnd.Model;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace CommonFrontEnd.ViewModel
{
    public class AllIndicesVM : BaseViewModel
    {
        private string _selectedExchange;

        public string selectedExchange
        {
            get { return _selectedExchange; }
            set
            {
                _selectedExchange = value;
                NotifyPropertyChanged(nameof(selectedExchange));
                PopulatingDropDown();
            }
        }

       

        private string _setColor;

        public string setColor
        {
            get { return _setColor; }
            set { _setColor = value; NotifyPropertyChanged(nameof(setColor)); }
        }

        private string _GrdlineVisible;

        public string GrdlineVisible
        {
            get { return _GrdlineVisible; }
            set { _GrdlineVisible = value; NotifyPropertyChanged("GrdlineVisible"); }
        }

        private string _GridLinesVisibility;

        public string GridLinesVisibility
        {
            get { return _GridLinesVisibility; }
            set
            {
                _GridLinesVisibility = value;
                //MainWindowVM.parserAllIndices.AddSetting("Visibility", "GridLine", value);
                //MainWindowVM.parserAllIndices.SaveSettings(MainWindowVM.AllIndicesINIPath.ToString());
                NotifyPropertyChanged("GridLinesVisibility");
            }
        }






        public double TrendDiff = 0;

        private static string _selectedIndex;

        public static string selectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value; NotifyStaticPropertyChanged(nameof(selectedIndex)); }
        }
        private string _foreColor;

        public string foreColor
        {
            get { return _foreColor; }
            set { _foreColor = value; NotifyPropertyChanged(nameof(foreColor)); }
        }


        private List<string> _listExchange;

        public List<string> listExchange
        {
            get { return _listExchange; }
            set
            {
                _listExchange = value;
                NotifyPropertyChanged(nameof(listExchange));

            }
        }

        private static List<string> _listIndices;

        public static List<string> listIndices
        {
            get { return _listIndices; }
            set
            {
                _listIndices = value;
                NotifyStaticPropertyChanged(nameof(listIndices));
            }
        }

        private static AllIndicesVM _getinstance;
        public static AllIndicesVM GetInstance
        {
            get
            {
                if (_getinstance == null)
                {
                    _getinstance = new AllIndicesVM();
                }
                return _getinstance;
            }
        }

        private List<AllIndicesWindow> _selectedIndicesRow = new List<AllIndicesWindow>();

        public List<AllIndicesWindow> SelectedIndicesRow
        {
            get { return _selectedIndicesRow; }
            set
            {
                _selectedIndicesRow = value;
                NotifyPropertyChanged(nameof(SelectedIndicesRow));
            }
        }


        private static ObservableCollection<AllIndicesWindow> _ObjMinIndicesCollection = new ObservableCollection<AllIndicesWindow>();

        public static ObservableCollection<AllIndicesWindow> ObjMinIndicesCollection
        {
            get { return _ObjMinIndicesCollection; }
            set { _ObjMinIndicesCollection = value; /*NotifyStaticPropertyChanged(nameof(ObjMinIndicesCollection));*/ }
        }



        private RelayCommand _addIndices;

        public RelayCommand addIndices
        {
            get
            {
                return _addIndices ?? (_addIndices = new RelayCommand(
                    (Obj) => AddIndicesbtn_Click()
                    ));
            }
        }

        private RelayCommand _GridVisibilitySetting;
        public RelayCommand GridVisibilitySetting
        {
            get
            {
                return _GridVisibilitySetting ?? (_GridVisibilitySetting = new RelayCommand(
                    (object e) => GridVisibilitySetting_Click(e)
                        ));
            }
        }


        //private RelayCommand _AllSelected;

        //public RelayCommand AllSelected
        //{
        //    get
        //    {
        //        return _AllSelected ?? (_AllSelected = new RelayCommand(
        //            (Obj) => AddIndicesbtn_Click()
        //            ));
        //    }
        //}

        private RelayCommand _deleteIndices;

        public RelayCommand deleteIndices
        {
            get
            {
                return _deleteIndices ?? (_deleteIndices = new RelayCommand(
                   (Obj) => DeleteIndicesbtn_Click()
                   ));
            }
            // set { _deleteIndices = value; }
        }

        All_Indices window = null;

        public AllIndicesVM()
        {

            if (!string.IsNullOrEmpty(MainWindowVM.parserAllIndices.GetSetting("Visibility", "GridLine")))
            {
                GridLinesVisibility = MainWindowVM.parserAllIndices.GetSetting("Visibility", "GridLine");
                if (GridLinesVisibility == "Enable GridLines")
                    GrdlineVisible = DataGridGridLinesVisibility.None.ToString();
                else
                    GrdlineVisible = DataGridGridLinesVisibility.All.ToString();
            }
            else
            {
                GridLinesVisibility = "Disable GridLines";
                GrdlineVisible = DataGridGridLinesVisibility.All.ToString();
            }
            if (!string.IsNullOrEmpty(MainWindowVM.parserAllIndices.GetSetting("Indices", "Configure Indexes")))
            {
                string str = MainWindowVM.parserAllIndices.GetSetting("Indices", "Configure Indexes");
                string[] strArr = str.Split('|');
                populateGridByINI(strArr);
            }
            else
            {
                popoulteDefaultGrid();
            }
            popoulteData();
            popoulteDefaultGrid();

#if TWS
            MainWindowVM.indicesBroadCast += objIndicesBroadCastProcessor_OnBroadCastRecievedNew;
#elif BOW
            SettingsManager.indicesBroadCastBow += objIndicesBowBroadCastProcessor_OnBroadCastRecievedNew;
#endif
            window = Application.Current.Windows.OfType<All_Indices>().FirstOrDefault();
        }

        private void populateGridByINI(string[] strArr)
        {
            
            for(int index=1; index < strArr.Length; index++)
            {
                AllIndicesWindow oAllIndicesModel = new AllIndicesWindow();
                oAllIndicesModel.IndexCode = MasterSharedMemory.objMasterDicSyb.Where(x => x.Value.IndexID == strArr[index]).Select(z => z.Key).FirstOrDefault(); ;
                oAllIndicesModel.IndexId = MasterSharedMemory.objMasterDicSyb.Where(x => x.Value.IndexCode == oAllIndicesModel.IndexCode).Select(z => z.Value.IndexID).FirstOrDefault();
                oAllIndicesModel.IndexValue = 0;
                oAllIndicesModel.PreviousIndexClose = 0;
                if (ObjMinIndicesCollection.Any(p => p.IndexCode == oAllIndicesModel.IndexCode))
                { }
                else
                {
                    ObjMinIndicesCollection.Add(oAllIndicesModel);
                }
            }
            //ObjMinIndicesCollection.Reverse();
        }

        private void objIndicesBowBroadCastProcessor_OnBroadCastRecievedNew(IdicesDetailsMain obj)
        {
            if (obj != null && ObjMinIndicesCollection.Count > 0)
            {
                foreach (var item in BroadcastMasterMemory.indicesDataDict)
                {
                    int? index;
                    index = ObjMinIndicesCollection.Where(i => i.IndexCode == item.Value.code).Select(x => ObjMinIndicesCollection.IndexOf(x)).FirstOrDefault();
                    if (index != null)
                    {
                        ObjMinIndicesCollection[(int)index].IndexValue = Convert.ToDouble(item.Value.value);
                        ObjMinIndicesCollection[(int)index].perChange = Convert.ToString(item.Value.chngValue);

                        if (item.Value.chngValue >= 0)
                        {
                            ObjMinIndicesCollection[(int)index].PrevLTP1 = "Blue";
                            setColor = "Blue";
                        }
                        else if (item.Value.chngValue < 0)
                        {
                            ObjMinIndicesCollection[(int)index].PrevLTP1 = "OrangeRed";
                            setColor = "Red";
                        }
                        else
                        {
                            ObjMinIndicesCollection[(int)index].PrevLTP1 = "Blue";
                            setColor = "Blue";
                        }
                    }
                }

            }
        }

        private void objIndicesBroadCastProcessor_OnBroadCastRecievedNew(IdicesDetailsMain obj)
        {
            if (selectedExchange == Common.Enumerations.Exchange.BSE.ToString())
            {

                if (obj != null && ObjMinIndicesCollection.Count > 0)
                {
                    IdicesDetailsMain item = obj;
                    //foreach (var item in obj.listindMain)
                    {
                        //int? index;
                        int count = ObjMinIndicesCollection.Count;

                        for (int i = 0; i < count; i++)
                        {
                            if (ObjMinIndicesCollection[i].IndexCode == item.IndexCode)
                            {

                                ObjMinIndicesCollection[(int)i].IndexValue = item.IndexValue / Math.Pow(10, 2);
                                ObjMinIndicesCollection[(int)i].PreviousIndexClose = item.PreviousIndexClose / Math.Pow(10, 2);
                                if (ObjMinIndicesCollection[(int)i].IndexValue != 0 && ObjMinIndicesCollection[(int)i].PreviousIndexClose != 0)
                                    if (Math.Round(Convert.ToDouble((ObjMinIndicesCollection[(int)i].IndexValue - ObjMinIndicesCollection[(int)i].PreviousIndexClose) / ObjMinIndicesCollection[(int)i].PreviousIndexClose * 100), 2) > 0)
                                        ObjMinIndicesCollection[(int)i].perChange = string.Format("{0}{1} {2}", "+", Math.Round(Convert.ToDouble((ObjMinIndicesCollection[(int)i].IndexValue - ObjMinIndicesCollection[(int)i].PreviousIndexClose) / ObjMinIndicesCollection[(int)i].PreviousIndexClose * 100), 2).ToString("0.00"), "%");
                                    else
                                        ObjMinIndicesCollection[(int)i].perChange = string.Format("{0} {1}", Math.Round(Convert.ToDouble((ObjMinIndicesCollection[(int)i].IndexValue - ObjMinIndicesCollection[(int)i].PreviousIndexClose) / ObjMinIndicesCollection[(int)i].PreviousIndexClose * 100), 2).ToString("0.00"), "%");

                                TrendDiff = ObjMinIndicesCollection[(int)i].IndexValue - ObjMinIndicesCollection[(int)i].PreviousIndexClose;

                                if (TrendDiff > 0)
                                {
                                    ObjMinIndicesCollection[(int)i].PrevLTP1 = "Blue";
                                    ObjMinIndicesCollection[(int)i].ChangeInValue = string.Format("{0}{1:0.00}", "+", TrendDiff);
                                    setColor = "Blue";
                                }
                                else if (TrendDiff < 0)
                                {
                                    ObjMinIndicesCollection[(int)i].PrevLTP1 = "OrangeRed";
                                    ObjMinIndicesCollection[(int)i].ChangeInValue = string.Format("{0:0.00}", TrendDiff);
                                    setColor = "Red";
                                }
                                else
                                {
                                    ObjMinIndicesCollection[(int)i].PrevLTP1 = "Blue";
                                    ObjMinIndicesCollection[(int)i].ChangeInValue = string.Format("{0:0.00}", TrendDiff);
                                    setColor = "Blue";
                                }

                            }
                        }

                        //index = k;
                        //index = ObjMinIndicesCollection.Where(i => i.IndexCode == item.IndexCode).Select(x => ObjMinIndicesCollection.IndexOf(x)).FirstOrDefault();
                        //if (index != null)
                        //{
                        //    //ObjMinIndicesCollection[(int)index].IndexId = item.IndexId;
                        //    ObjMinIndicesCollection[(int)index].IndexValue = item.IndexValue / Math.Pow(10, 2);
                        //    ObjMinIndicesCollection[(int)index].PreviousIndexClose = item.PreviousIndexClose / Math.Pow(10, 2);
                        //    if (ObjMinIndicesCollection[(int)index].IndexValue != 0 && ObjMinIndicesCollection[(int)index].PreviousIndexClose != 0)
                        //        if (Math.Round(Convert.ToDouble((ObjMinIndicesCollection[(int)index].IndexValue - ObjMinIndicesCollection[(int)index].PreviousIndexClose) / ObjMinIndicesCollection[(int)index].PreviousIndexClose * 100), 2) > 0)
                        //            ObjMinIndicesCollection[(int)index].perChange = string.Format("{0}{1} {2}", "+", Math.Round(Convert.ToDouble((ObjMinIndicesCollection[(int)index].IndexValue - ObjMinIndicesCollection[(int)index].PreviousIndexClose) / ObjMinIndicesCollection[(int)index].PreviousIndexClose * 100), 2).ToString("0.00"), "%");
                        //        else
                        //            ObjMinIndicesCollection[(int)index].perChange = string.Format("{0} {1}", Math.Round(Convert.ToDouble((ObjMinIndicesCollection[(int)index].IndexValue - ObjMinIndicesCollection[(int)index].PreviousIndexClose) / ObjMinIndicesCollection[(int)index].PreviousIndexClose * 100), 2).ToString("0.00"), "%");
                        //    //ObjMinIndicesCollection[(int)index].perChange = Math.Round((ObjMinIndicesCollection[(int)index].IndexValue - ObjMinIndicesCollection[(int)index].PreviousIndexClose) / ObjMinIndicesCollection[(int)index].PreviousIndexClose * 100, 2);

                        //}
                    }
                }

            }
            //var IndexCode = MasterSharedMemory.objMasterDicSyb.Where(x => x.Value.IndexID == selectedIndex).Select(z => z.Key).FirstOrDefault();
            //if (BroadcastMaster.BroadcastMasterMemory.objIndicesDetailsConDict.ContainsKey(IndexCode))
            //{
            //    ObjMinIndicesCollection[IndexCode].IndexId = BroadcastMaster.BroadcastMasterMemory.objIndicesDetailsConDict[IndexCode].IndexId;
            //    ObjMinIndicesCollection[IndexCode].IndexValue = BroadcastMaster.BroadcastMasterMemory.objIndicesDetailsConDict[IndexCode].IndexValue;
            //    ObjMinIndicesCollection[IndexCode].perChange = Math.Round((ObjMinIndicesCollection[IndexCode].IndexValue - ObjMinIndicesCollection[IndexCode].PreviousIndexClose) / ObjMinIndicesCollection[IndexCode].PreviousIndexClose * 100, 2);
            //}
        }

        private void popoulteData()
        {
#if TWS
            listExchange = new List<string>();
            selectedExchange = Common.Enumerations.Exchange.BSE.ToString();
            listExchange.Add(Common.Enumerations.Exchange.BSE.ToString());
            //listExchange.Add(Common.Enumerations.Exchange.BSEINX.ToString());
            //listExchange.Add(Common.Enumerations.Exchange.NSE.ToString());
            //listExchange.Add(Common.Enumerations.Exchange.USE.ToString());
            //listExchange.Add(Common.Enumerations.Exchange.BSEINX.ToString());
            //listExchange.Add(Common.Enumerations.Exchange.NSE.ToString());
            //listExchange.Add(Common.Enumerations.Exchange.USE.ToString());
#elif BOW
            listIndices = new List<string>();
            selectedExchange = Common.Enumerations.Exchange.BSE.ToString();
            listExchange.Add(Common.Enumerations.Exchange.BSEINX.ToString());
            listExchange.Add(Common.Enumerations.Exchange.NSE.ToString());
            listExchange.Add(Common.Enumerations.Exchange.USE.ToString());
            //#elif BOW
            //listIndices = new List<string>();
            // listIndices = BroadcastMasterMemory.indicesDataDict.Values.Select(y => y.name).ToList();
#endif
        }
        public static void popoulteDefaultGrid()
        {
            AllIndicesWindow oAllIndicesModel = new AllIndicesWindow();

            oAllIndicesModel.IndexCode = MasterSharedMemory.objMasterDicSyb.Where(x => x.Value.IndexID == "SENSEX").Select(z => z.Key).FirstOrDefault(); ;
            oAllIndicesModel.IndexId = MasterSharedMemory.objMasterDicSyb.Where(x => x.Value.IndexCode == oAllIndicesModel.IndexCode).Select(z => z.Value.IndexID).FirstOrDefault();
            oAllIndicesModel.IndexValue = 0;
            oAllIndicesModel.PreviousIndexClose = 0;
            if (ObjMinIndicesCollection.Any(p => p.IndexCode == oAllIndicesModel.IndexCode))
            { }
            else
            {
                ObjMinIndicesCollection.Add(oAllIndicesModel);
            }
        }

        private void PopulatingDropDown()
        {
            try
            {
                if (selectedExchange == Common.Enumerations.Exchange.BSE.ToString())
                {
                    listIndices = new List<string>();
                    listIndices = MasterSharedMemory.objMasterDicSyb.Values.Select(y => y.IndexID).ToList();
                    selectedIndex = listIndices[0];
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }
#if TWS
        private void AddIndicesbtn_Click()
        {
            if(window != null)
            {
                window.SizeToContent = SizeToContent.WidthAndHeight;
            }
            var IndexCode = MasterSharedMemory.objMasterDicSyb.Where(x => x.Value.IndexID == selectedIndex).Select(z => z.Key).FirstOrDefault();
            //var IndexID = MasterSharedMemory.objMasterDicSyb.Where(x=>)

            if (SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict.Keys.Contains(IndexCode))
            {
                //if (ObjMinIndicesCollection != null && ObjMinIndicesCollection.Count > 1)
                //{
                if (ObjMinIndicesCollection.Count != 0)
                {

                    foreach (var item in ObjMinIndicesCollection)
                    {
                        if (item.IndexCode == IndexCode)
                        {
                            int? i;
                            i = ObjMinIndicesCollection.Where(x => x.IndexCode == item.IndexCode).Select(y => ObjMinIndicesCollection.IndexOf(y)).FirstOrDefault();
                            if (i != null)
                            {
                                //ObjMinIndicesCollection[(int)i].IndexId = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[IndexCode].IndexId;
                                ObjMinIndicesCollection[(int)i].IndexId = selectedIndex.ToString().Trim();
                                ObjMinIndicesCollection[(int)i].IndexValue = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[IndexCode].IndexValue / Math.Pow(10, 2);
                                ObjMinIndicesCollection[(int)i].PreviousIndexClose = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[IndexCode].PreviousIndexClose / Math.Pow(10, 2);
                                if (ObjMinIndicesCollection[(int)i].IndexValue != 0 && ObjMinIndicesCollection[(int)i].PreviousIndexClose != 0)
                                    if (Math.Round(Convert.ToDouble((ObjMinIndicesCollection[(int)i].IndexValue - ObjMinIndicesCollection[(int)i].PreviousIndexClose) / ObjMinIndicesCollection[(int)i].PreviousIndexClose * 100), 2) > 0)
                                        ObjMinIndicesCollection[(int)i].perChange = string.Format("{0}{1} {2}", "+", Math.Round(Convert.ToDouble((ObjMinIndicesCollection[(int)i].IndexValue - ObjMinIndicesCollection[(int)i].PreviousIndexClose) / ObjMinIndicesCollection[(int)i].PreviousIndexClose * 100), 2).ToString("0.00"), "%");
                                    else
                                        ObjMinIndicesCollection[(int)i].perChange = string.Format("{0} {1}", Math.Round(Convert.ToDouble((ObjMinIndicesCollection[(int)i].IndexValue - ObjMinIndicesCollection[(int)i].PreviousIndexClose) / ObjMinIndicesCollection[(int)i].PreviousIndexClose * 100), 2).ToString("0.00"), "%");
                                //ObjMinIndicesCollection[(int)i].perChange = Math.Round((ObjMinIndicesCollection[(int)i].IndexValue - ObjMinIndicesCollection[(int)i].PreviousIndexClose) / ObjMinIndicesCollection[(int)i].PreviousIndexClose * 100, 2);
                                TrendDiff = ObjMinIndicesCollection[(int)i].IndexValue - ObjMinIndicesCollection[(int)i].PreviousIndexClose;
                                if (TrendDiff > 0)
                                {
                                    ObjMinIndicesCollection[(int)i].PrevLTP1 = "Blue";
                                    ObjMinIndicesCollection[(int)i].ChangeInValue = string.Format("{0}{1:0.00}", "+", TrendDiff);
                                    setColor = "Blue";

                                }
                                else if (TrendDiff < 0)
                                {
                                    ObjMinIndicesCollection[(int)i].PrevLTP1 = "OrangeRed";
                                    ObjMinIndicesCollection[(int)i].ChangeInValue = string.Format("{0:0.00}", TrendDiff);
                                    setColor = "Red";
                                }
                                else
                                {
                                    ObjMinIndicesCollection[(int)i].PrevLTP1 = "Blue";
                                    ObjMinIndicesCollection[(int)i].ChangeInValue = string.Format("{0:0.00}", TrendDiff);
                                    setColor = "Blue";
                                }
                            }
                            break;

                        }
                        else
                        {
                            AllIndicesWindow oAllIndicesModel = new AllIndicesWindow();
                            oAllIndicesModel.IndexCode = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[IndexCode].IndexCode;
                            // oAllIndicesModel.IndexId = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[IndexCode].IndexId;
                            oAllIndicesModel.IndexId = selectedIndex.ToString().Trim();
                            oAllIndicesModel.IndexValue = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[IndexCode].IndexValue / Math.Pow(10, 2);
                            oAllIndicesModel.PreviousIndexClose = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[IndexCode].PreviousIndexClose / Math.Pow(10, 2);
                            if (oAllIndicesModel.IndexValue != 0 && oAllIndicesModel.PreviousIndexClose != 0)
                                if (Math.Round(Convert.ToDouble((oAllIndicesModel.IndexValue - oAllIndicesModel.PreviousIndexClose) / oAllIndicesModel.PreviousIndexClose * 100), 2) > 0)
                                    oAllIndicesModel.perChange = string.Format("{0}{1} {2}", "+", Math.Round(Convert.ToDouble((oAllIndicesModel.IndexValue - oAllIndicesModel.PreviousIndexClose) / oAllIndicesModel.PreviousIndexClose * 100), 2).ToString("0.00"), "%");
                                else
                                    oAllIndicesModel.perChange = string.Format("{0} {1}", Math.Round(Convert.ToDouble((oAllIndicesModel.IndexValue - oAllIndicesModel.PreviousIndexClose) / oAllIndicesModel.PreviousIndexClose * 100), 2).ToString("0.00"), "%");

                            TrendDiff = oAllIndicesModel.IndexValue - oAllIndicesModel.PreviousIndexClose;
                            if (TrendDiff > 0)
                            {
                                oAllIndicesModel.PrevLTP1 = "Blue";
                                oAllIndicesModel.ChangeInValue = string.Format("{0}{1:0.00}", "+", TrendDiff);
                                setColor = "Blue";
                            }
                            else if (TrendDiff < 0)
                            {
                                oAllIndicesModel.PrevLTP1 = "OrangeRed";

                                oAllIndicesModel.ChangeInValue = string.Format("{0:0.00}", TrendDiff);
                                setColor = "Red";
                            }
                            else
                            {
                                oAllIndicesModel.PrevLTP1 = "Blue";
                                oAllIndicesModel.ChangeInValue = string.Format("{0:0.00}", TrendDiff);
                                setColor = "Blue";
                            }

                            // oAllIndicesModel.perChange = Math.Round((oAllIndicesModel.IndexValue - oAllIndicesModel.PreviousIndexClose) / oAllIndicesModel.PreviousIndexClose * 100, 2);
                            //if (!ObjMinIndicesCollection<AllIndicesWindow>.Contains(oAllIndicesModel)){ }
                            if (ObjMinIndicesCollection.Any(p => p.IndexCode == oAllIndicesModel.IndexCode))
                            { }
                            else
                            {
                                ObjMinIndicesCollection.Add(oAllIndicesModel);
                            }

                            break;
                        }


                        //assign values  to object and add the object to collectio n

                    }
                }
                else
                {
                    AllIndicesWindow oAllIndicesModel = new AllIndicesWindow();
                    oAllIndicesModel.IndexCode = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[IndexCode].IndexCode;
                    //oAllIndicesModel.IndexId = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[IndexCode].IndexId;
                    oAllIndicesModel.IndexId = selectedIndex.ToString().Trim();

                    oAllIndicesModel.IndexValue = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[IndexCode].IndexValue / Math.Pow(10, 2);
                    oAllIndicesModel.PreviousIndexClose = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[IndexCode].PreviousIndexClose / Math.Pow(10, 2);
                    //oAllIndicesModel.perChange = Math.Round((oAllIndicesModel.IndexValue - oAllIndicesModel.PreviousIndexClose) / oAllIndicesModel.PreviousIndexClose * 100, 2);
                    //ObjMinIndicesCollection.Add(oAllIndicesModel);
                    TrendDiff = oAllIndicesModel.IndexValue - oAllIndicesModel.PreviousIndexClose;
                    if (TrendDiff > 0)
                    {
                        oAllIndicesModel.PrevLTP1 = "Blue";
                        oAllIndicesModel.ChangeInValue = string.Format("{0}{1:0.00}", "+", TrendDiff);
                        setColor = "Blue";
                    }
                    else if (TrendDiff < 0)
                    {
                        oAllIndicesModel.PrevLTP1 = "OrangeRed";
                        oAllIndicesModel.ChangeInValue = string.Format("{0:0.00}", TrendDiff);
                        setColor = "Red";
                    }
                    else
                    {
                        oAllIndicesModel.PrevLTP1 = "Blue";
                        oAllIndicesModel.ChangeInValue = string.Format("{0:0.00}", TrendDiff);
                        setColor = "Blue";
                    }

                    if (ObjMinIndicesCollection.Any(p => p.IndexCode == oAllIndicesModel.IndexCode))
                    { }
                    else
                    {
                        ObjMinIndicesCollection.Add(oAllIndicesModel);
                    }
                }
                //}
                //else {
                // ObjMinIndicesCollection[0].IndexId = BroadcastMaster.BroadcastMasterMemory.objIndicesDetailsConDict[IndexCode].IndexId;
                //ObjMinIndicesCollection[0].IndexValue = BroadcastMaster.BroadcastMasterMemory.objIndicesDetailsConDict[IndexCode].IndexValue;
                //ObjMinIndicesCollection[0].perChange = Math.Round((ObjMinIndicesCollection[0].IndexValue - ObjMinIndicesCollection[0].PreviousIndexClose) / ObjMinIndicesCollection[0].PreviousIndexClose * 100, 2);
                // }

            }
            else
            {
                AllIndicesWindow oAllIndicesModel = new AllIndicesWindow();
                oAllIndicesModel.IndexCode = IndexCode;
                oAllIndicesModel.IndexId = selectedIndex.ToString().Trim();
                oAllIndicesModel.IndexValue = 0;
                oAllIndicesModel.PreviousIndexClose = 0;
                TrendDiff = oAllIndicesModel.IndexValue - oAllIndicesModel.PreviousIndexClose;
                if (TrendDiff > 0)
                {
                    oAllIndicesModel.PrevLTP1 = "Blue";
                    oAllIndicesModel.ChangeInValue = string.Format("{0}{1:0.00}", "+", TrendDiff);
                    setColor = "Blue";
                }
                else if (TrendDiff < 0)
                {
                    oAllIndicesModel.PrevLTP1 = "OrangeRed";
                    oAllIndicesModel.ChangeInValue = string.Format("{0:0.00}", TrendDiff);
                    setColor = "Red";
                }
                else
                {
                    oAllIndicesModel.PrevLTP1 = "Blue";
                    oAllIndicesModel.ChangeInValue = string.Format("{0:0.00}", TrendDiff);
                    setColor = "Blue";
                }
                if (ObjMinIndicesCollection.Any(p => p.IndexCode == oAllIndicesModel.IndexCode))
                { }
                else
                {
                    ObjMinIndicesCollection.Add(oAllIndicesModel);
                }
            }
        }

        private void GridVisibilitySetting_Click(object e)
        {
            if (GridLinesVisibility == "Disable GridLines")
            {
                //datagrid.GridLinesVisibility = DataGridGridLinesVisibility.None;
                GrdlineVisible = DataGridGridLinesVisibility.None.ToString();
                GridLinesVisibility = "Enable GridLines";
            }
            else
            {
                //datagrid.GridLinesVisibility = DataGridGridLinesVisibility.All;
                GrdlineVisible = DataGridGridLinesVisibility.All.ToString();
                GridLinesVisibility = "Disable GridLines";
            }

        }

        private void DeleteIndicesbtn_Click()
        {
            try
            {
               
                if (SelectedIndicesRow != null && SelectedIndicesRow.Count != 0)
                {
                    List<AllIndicesWindow> tempselectedIndicesRow = new List<AllIndicesWindow>();

                    foreach (AllIndicesWindow item in System.Windows.Application.Current.Windows.OfType<All_Indices>().FirstOrDefault().AllIndicesDataGrid.SelectedItems)
                    {
                        tempselectedIndicesRow.Add(item);
                    }
                    if (MessageBox.Show("Do you really want to Remove this Index/ies?", "Remove Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {

                        //tempselectedIndicesRow = SelectedIndicesRow;
                        foreach (AllIndicesWindow item in tempselectedIndicesRow)
                        {
                            ObjMinIndicesCollection.Remove(item);
                        }
                        if (window != null)
                        {
                            window.SizeToContent = SizeToContent.WidthAndHeight;
                        }
                    }
                }
                else { MessageBox.Show("Please Select the Index to Remove", "Remove Index", MessageBoxButton.OK, MessageBoxImage.Warning); }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
            // throw new NotImplementedException();
        }
#elif BOW
        //private void AddIndicesbtn_Click()
        //{
        //    int selectedId = BroadcastMasterMemory.indicesDataDict.Where(x => x.Value.name == selectedIndex).Select(z => z.Key).FirstOrDefault();
        //    var IndexCode = BroadcastMasterMemory.objIndexDetailsConDict.Where(x => x.Value.IndexCode == selectedId).Select(z => z.Key).FirstOrDefault();
        //    if (SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict.Keys.Contains(IndexCode))
        //    {
        //        //if (ObjMinIndicesCollection != null && ObjMinIndicesCollection.Count > 1)
        //        //{
        //        if (ObjMinIndicesCollection.Count != 0)
        //        {
        //            foreach (var item in ObjMinIndicesCollection)
        //            {
        //                if (item.IndexCode == IndexCode)
        //                {
        //                    int? i;
        //                    i = ObjMinIndicesCollection.Where(x => x.IndexCode == item.IndexCode).Select(y => ObjMinIndicesCollection.IndexOf(y)).FirstOrDefault();
        //                    if (i != null)
        //                    {
        //                        //ObjMinIndicesCollection[(int)i].IndexId = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[IndexCode].IndexId;
        //                        ObjMinIndicesCollection[(int)i].IndexId = selectedIndex.ToString().Trim();
        //                        ObjMinIndicesCollection[(int)i].IndexValue = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[IndexCode].IndexValue;
        //                        ObjMinIndicesCollection[(int)i].PreviousIndexClose = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[IndexCode].PreviousIndexClose;
        //                        if (ObjMinIndicesCollection[(int)i].IndexValue != 0 && ObjMinIndicesCollection[(int)i].PreviousIndexClose != 0)
        //                            if (Math.Round(Convert.ToDouble((ObjMinIndicesCollection[(int)i].IndexValue - ObjMinIndicesCollection[(int)i].PreviousIndexClose) / ObjMinIndicesCollection[(int)i].PreviousIndexClose * 100), 2) > 0)
        //                                ObjMinIndicesCollection[(int)i].perChange = string.Format("{0}{1} {2}", "+", Math.Round(Convert.ToDouble((ObjMinIndicesCollection[(int)i].IndexValue - ObjMinIndicesCollection[(int)i].PreviousIndexClose) / ObjMinIndicesCollection[(int)i].PreviousIndexClose * 100), 2).ToString("0.00"), "%");
        //                            else
        //                                ObjMinIndicesCollection[(int)i].perChange = string.Format("{0} {1}", Math.Round(Convert.ToDouble((ObjMinIndicesCollection[(int)i].IndexValue - ObjMinIndicesCollection[(int)i].PreviousIndexClose) / ObjMinIndicesCollection[(int)i].PreviousIndexClose * 100), 2).ToString("0.00"), "%");
        //                        //ObjMinIndicesCollection[(int)i].perChange = Math.Round((ObjMinIndicesCollection[(int)i].IndexValue - ObjMinIndicesCollection[(int)i].PreviousIndexClose) / ObjMinIndicesCollection[(int)i].PreviousIndexClose * 100, 2);

        //                    }
        //                    break;

        //                }
        //                else
        //                {
        //                    AllIndicesWindow oAllIndicesModel = new AllIndicesWindow();
        //                    oAllIndicesModel.IndexCode = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[IndexCode].IndexCode;
        //                    // oAllIndicesModel.IndexId = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[IndexCode].IndexId;
        //                    oAllIndicesModel.IndexId = selectedIndex.ToString().Trim();
        //                    oAllIndicesModel.IndexValue = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[IndexCode].IndexValue;
        //                    oAllIndicesModel.PreviousIndexClose = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[IndexCode].PreviousIndexClose;
        //                    if (oAllIndicesModel.IndexValue != 0 && oAllIndicesModel.PreviousIndexClose != 0)
        //                        if (Math.Round(Convert.ToDouble((oAllIndicesModel.IndexValue - oAllIndicesModel.PreviousIndexClose) / oAllIndicesModel.PreviousIndexClose * 100), 2) > 0)
        //                            oAllIndicesModel.perChange = string.Format("{0}{1} {2}", "+", Math.Round(Convert.ToDouble((oAllIndicesModel.IndexValue - oAllIndicesModel.PreviousIndexClose) / oAllIndicesModel.PreviousIndexClose * 100), 2).ToString("0.00"), "%");
        //                        else
        //                            oAllIndicesModel.perChange = string.Format("{0} {1}", Math.Round(Convert.ToDouble((oAllIndicesModel.IndexValue - oAllIndicesModel.PreviousIndexClose) / oAllIndicesModel.PreviousIndexClose * 100), 2).ToString("0.00"), "%");

        //                    // oAllIndicesModel.perChange = Math.Round((oAllIndicesModel.IndexValue - oAllIndicesModel.PreviousIndexClose) / oAllIndicesModel.PreviousIndexClose * 100, 2);
        //                    ObjMinIndicesCollection.Add(oAllIndicesModel);
        //                    break;
        //                }


        //                //assign values  to object and add the object to collectio n

        //            }
        //        }
        //        else
        //        {
        //            AllIndicesWindow oAllIndicesModel = new AllIndicesWindow();
        //            oAllIndicesModel.IndexCode = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[IndexCode].IndexCode;
        //            //oAllIndicesModel.IndexId = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[IndexCode].IndexId;
        //            oAllIndicesModel.IndexId = selectedIndex.ToString().Trim();
        //            oAllIndicesModel.IndexValue = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[IndexCode].IndexValue;
        //            oAllIndicesModel.PreviousIndexClose = SharedMemories.BroadcastMasterMemory.objIndexDetailsConDict[IndexCode].PreviousIndexClose;
        //            //oAllIndicesModel.perChange = Math.Round((oAllIndicesModel.IndexValue - oAllIndicesModel.PreviousIndexClose) / oAllIndicesModel.PreviousIndexClose * 100, 2);
        //            ObjMinIndicesCollection.Add(oAllIndicesModel);
        //        }
        //        //}
        //        //else {
        //        // ObjMinIndicesCollection[0].IndexId = BroadcastMaster.BroadcastMasterMemory.objIndicesDetailsConDict[IndexCode].IndexId;
        //        //ObjMinIndicesCollection[0].IndexValue = BroadcastMaster.BroadcastMasterMemory.objIndicesDetailsConDict[IndexCode].IndexValue;
        //        //ObjMinIndicesCollection[0].perChange = Math.Round((ObjMinIndicesCollection[0].IndexValue - ObjMinIndicesCollection[0].PreviousIndexClose) / ObjMinIndicesCollection[0].PreviousIndexClose * 100, 2);
        //        // }

        //    }
        //}

        private void AddIndicesbtn_Click()
        {
            if (BroadcastMasterMemory.objIndexDetailsConDict != null && BroadcastMasterMemory.objIndexDetailsConDict.Count != 0)
            {
                int selectedId = BroadcastMasterMemory.indicesDataDict.Where(x => x.Value.name == selectedIndex).Select(z => z.Key).FirstOrDefault();
                var IndexCode = BroadcastMasterMemory.objIndexDetailsConDict.Where(x => x.Value.IndexCode == selectedId).Select(z => z.Key).FirstOrDefault();
            }
        }

        private void DeleteIndicesbtn_Click()
        {
            try
            {
                if (SelectedIndicesRow != null && SelectedIndicesRow.Count != 0)
                {
                    if (MessageBox.Show("Do you really want to Remove this Index/ies?", "Remove Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {

                    }
                }
                else { MessageBox.Show("Please Select the Index to Remove", "Remove Index", MessageBoxButton.OK, MessageBoxImage.Warning); }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
        }

#endif


        #region NotifyPropertyChangedEvent

        //public event PropertyChangedEventHandler PropertyChanged;

        //private void NotifyPropertyChanged(String propertyName = "")
        //{
        //    PropertyChangedEventHandler handler = this.PropertyChanged;
        //    if (handler != null)
        //    {
        //        var e = new PropertyChangedEventArgs(propertyName);
        //        this.PropertyChanged(this, e);
        //    }
        //}

        //public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged
        //        = delegate { };
        //public static void NotifyStaticPropertyChanged(string propertyName)
        //{
        //    StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        //}
        #endregion
    }
}
