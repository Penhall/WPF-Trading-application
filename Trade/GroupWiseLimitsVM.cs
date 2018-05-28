using CommonFrontEnd.Common;
using CommonFrontEnd.Model.Profiling;
using CommonFrontEnd.Model.Trade;
using CommonFrontEnd.SharedMemories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.ViewModel.Trade
{
    class GroupWiseLimitsVM : BaseViewModel
    {
#if TWS
        private static ObservableCollection<GroupWiseLimitsModel> _GroupWiseLimitsCollection;

        public static ObservableCollection<GroupWiseLimitsModel> GroupWiseLimitsCollection
        {
            get { return _GroupWiseLimitsCollection; }
            set { _GroupWiseLimitsCollection = value; }
        }

        //private static ObservableCollection<GroupWiseLimitsModel> _GroupWiseLimitsCollectionTemp;

        //public static ObservableCollection<GroupWiseLimitsModel> GroupWiseLimitsCollectionTemp
        //{
        //    get { return _GroupWiseLimitsCollectionTemp; }
        //    set { _GroupWiseLimitsCollectionTemp = value; }
        //}

        private List<string> _ScripGrp;
        public List<string> ScripGrp
        {
            get { return _ScripGrp; }
            set { _ScripGrp = value; NotifyPropertyChanged("ScripGrp"); }
        }



        public GroupWiseLimitsVM()
        {
            //GroupWiseLimitsCollection = new ObservableCollection<GroupWiseLimitsModel>();            
            MemoryManager.OnGroupwiseLimitReceive += UpdateGroupWiseLimits;
            PopulatingScripGroupDropDowns();
        }
        /// <summary>
        /// After Groupwise limit calc- 
        /// Equity -  Offline (Trades) limit calc, Order Placed
        /// </summary>
        /// <param name="e"></param>
        private void UpdateGroupWiseLimits(GroupWiseLimitsModel obj)
        {

            //GroupWiseLimitsModel oGroupWiseLimitsModel = new GroupWiseLimitsModel();
            //oGroupWiseLimitsModel = e as GroupWiseLimitsModel;

            var index = GroupWiseLimitsCollection.IndexOf(GroupWiseLimitsCollection.Where(x => x.Group == obj.Group).FirstOrDefault());
            if (index != -1)
            {
                GroupWiseLimitsCollection[index].BuyValue = obj.BuyValue;
                GroupWiseLimitsCollection[index].SellValue = obj.SellValue;
                GroupWiseLimitsCollection[index].AvlBuy = obj.AvlBuy;
                GroupWiseLimitsCollection[index].AvlSell = obj.AvlSell;
            }


        }

        private void PopulatingScripGroupDropDowns()
        {

            try
            {
                GroupWiseLimitsCollection = new ObservableCollection<GroupWiseLimitsModel>();
                GroupWiseLimitsCollection.Clear();

                foreach (var item in MasterSharedMemory.GroupWiseLimitDict)
                {
                    GroupWiseLimitsCollection.Add(item.Value);
                }

                //GroupWiseLimitsCollection.OrderBy(i => i.Group);
                GroupWiseLimitsCollection = new ObservableCollection<GroupWiseLimitsModel>(GroupWiseLimitsCollection.OrderBy(i => i.Group));
            }

            catch (Exception e)
            {
                ExceptionUtility.LogError(e);
            }


        }
#endif
    }
}
