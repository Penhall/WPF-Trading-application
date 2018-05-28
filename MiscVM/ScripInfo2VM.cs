using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonFrontEnd.Model;
using CommonFrontEnd.Common;
using CommonFrontEnd.View;

namespace CommonFrontEnd.ViewModel
{
    class ScripInfo2VM : INotifyPropertyChanged
    {
        #region Properties
        static ScripInfo2 mWindow = null;
        private string _ScripID;
        public string ScripID
        {
            get { return _ScripID; }
            set { _ScripID = value; NotifyPropertyChanged(nameof(ScripID)); }
        }

        private string _ContractTokenNo;
        public string ContractTokenNo
        {
            get { return _ContractTokenNo; }
            set { _ContractTokenNo = value; NotifyPropertyChanged(nameof(ContractTokenNo)); }
        }

        
        private string _AssetTokenNo;
        public string AssetTokenNo
        {
            get { return _AssetTokenNo; }
            set { _AssetTokenNo = value; NotifyPropertyChanged(nameof(AssetTokenNo)); }
        }
               
        private string _SName;
        public string SName
        {
            get { return _SName; }
            set { _SName = value; NotifyPropertyChanged(nameof(SName)); }
        }
        
        private string _InstType;
        public string InstType
        {
            get { return _InstType; }
            set { _InstType = value; NotifyPropertyChanged(nameof(InstType)); }
        }

        private string _AssetCode;
        public string AssetCode
        {
            get { return _AssetCode; }
            set { _AssetCode = value; NotifyPropertyChanged(nameof(AssetCode)); }
        }
       
        private string _ExpDate;
        public string ExpDate
        {
            get { return _ExpDate; }
            set { _ExpDate = value; NotifyPropertyChanged(nameof(ExpDate)); }
        }
       
        private string _UnderAsset;
        public string UnderAsset
        {
            get { return _UnderAsset; }
            set { _UnderAsset = value; NotifyPropertyChanged(nameof(UnderAsset)); }
        }
        
        private string _ProductID;
        public string ProductID
        {
            get { return _ProductID; }
            set { _ProductID = value; NotifyPropertyChanged(nameof(ProductID)); }
        }
        
        private string _PartitionID;
        public string PartitionID
        {
            get { return _PartitionID; }
            set { _PartitionID = value; NotifyPropertyChanged(nameof(PartitionID)); }
        }
        
        private string _CapacityGrpID;
        public string CapacityGrpID
        {
            get { return _CapacityGrpID; }
            set { _CapacityGrpID = value; NotifyPropertyChanged(nameof(CapacityGrpID)); }
        }
       
        private string _StkPrice;
        public string StkPrice
        {
            get { return _StkPrice; }
            set { _StkPrice = value; NotifyPropertyChanged(nameof(StkPrice)); }
        }
        
        private string _OptionType;
        public string OptionType
        {
            get { return _OptionType; }
            set { _OptionType = value; NotifyPropertyChanged(nameof(OptionType)); }
        }
       
        private string _Precision;
        public string Precision
        {
            get { return _Precision; }
            set { _Precision = value; NotifyPropertyChanged(nameof(Precision)); }
        }
        
        private string _MinLotSize;
        public string MinLotSize
        {
            get { return _MinLotSize; }
            set { _MinLotSize = value; NotifyPropertyChanged(nameof(MinLotSize)); }

        }
        
        private string _TickSize;
        public string TickSize
        {
            get { return _TickSize; }
            set { _TickSize = value; NotifyPropertyChanged(nameof(TickSize)); }

        }
        
        private string _QtyMult;
        public string QtyMult
        {
            get { return _QtyMult; }
            set { _QtyMult = value; NotifyPropertyChanged(nameof(QtyMult)); }

        }
        
        private string _UnderMarket;
        public string UnderMarket
        {
            get { return _UnderMarket; }
            set { _UnderMarket = value; NotifyPropertyChanged(nameof(UnderMarket)); }

        }
        
        private string _ContractType;
        public string ContractType
        {
            get { return _ContractType; }
            set { _ContractType = value; NotifyPropertyChanged(nameof(ContractType)); }

        }
        
        private string _ProdCode;
        public string ProdCode
        {
            get { return _ProdCode; }
            set { _ProdCode = value; NotifyPropertyChanged(nameof(ProdCode)); }

        }
        
        private string _BasePrice;
        public string BasePrice
        {
            get { return _BasePrice; }
            set { _BasePrice = value; NotifyPropertyChanged(nameof(BasePrice)); }

        }
        
        private string _DeleteFlag;
        public string DeleteFlag
        {
            get { return _DeleteFlag; }
            set { _DeleteFlag = value; NotifyPropertyChanged(nameof(DeleteFlag)); }

        }
        
        private string _CTNML1;
        public string CTNML1
        {
            get { return _CTNML1; }
            set { _CTNML1 = value; NotifyPropertyChanged(nameof(CTNML1)); }

        }
        
        private string _CTNML2;
        public string CTNML2
        {
            get { return _CTNML2; }
            set { _CTNML2 = value; NotifyPropertyChanged(nameof(CTNML2)); }

        }
        
        private string _NTAScripCode;
        public string NTAScripCode
        {
            get { return _NTAScripCode; }
            set { _NTAScripCode = value; NotifyPropertyChanged(nameof(NTAScripCode)); }

        }
        
        private string _StrategyID;
        public string StrategyID
        {
            get { return _StrategyID; }
            set { _StrategyID = value; NotifyPropertyChanged(nameof(StrategyID)); }

        }
       
        private string _SPDVisible;
        public string SPDVisible
        {
            get { return _SPDVisible; }
            set { _SPDVisible = value; NotifyPropertyChanged(nameof(SPDVisible)); }

        }
        private RelayCommand _CloseOnEscape;

        public RelayCommand CloseOnEscape
        {
            get
            {
                return _CloseOnEscape ?? (_CloseOnEscape = new RelayCommand(
                    (object e) => CloseOnEscape_Click()
                        ));
            }
        }
             
        #endregion
       public ScripInfo2VM()
        {
            mWindow = System.Windows.Application.Current.Windows.OfType<ScripInfo2>().FirstOrDefault();
        } 

        private void CloseOnEscape_Click()
        {
            mWindow?.Close();
        }

        internal void SetDataDerivative(ScripHelpBSEDerivativeCO selectedItemDerivateCO)
        {
            ScripID = selectedItemDerivateCO.ScripID;
            ContractTokenNo = Convert.ToString(selectedItemDerivateCO.ContractTokenNum);
            AssetTokenNo = Convert.ToString(selectedItemDerivateCO.AssestTokenNum);
            SName = selectedItemDerivateCO.InstrumentName;
            InstType = selectedItemDerivateCO.InstrumentType;
            AssetCode = selectedItemDerivateCO.AssetCd;
            ExpDate = selectedItemDerivateCO.ExpiryDate;
            UnderAsset = selectedItemDerivateCO.UnderlyingAsset;
            ProductID = Convert.ToString(selectedItemDerivateCO.ProductID);
            PartitionID= Convert.ToString(selectedItemDerivateCO.PartitionID);
            CapacityGrpID = Convert.ToString(selectedItemDerivateCO.CapacityGroupID);
            StkPrice = Convert.ToString(selectedItemDerivateCO.StrikePrice);
            OptionType = selectedItemDerivateCO.OptionType;
            Precision = Convert.ToString(selectedItemDerivateCO.Precision);
            MinLotSize = Convert.ToString(selectedItemDerivateCO.MinimumLotSize);
            TickSize = Convert.ToString(selectedItemDerivateCO.TickSize);
            QtyMult = Convert.ToString(selectedItemDerivateCO.QuantityMultiplier);
            UnderMarket = Convert.ToString(selectedItemDerivateCO.UnderlyingMarket);
            ContractType = selectedItemDerivateCO.ContractType;
            ProdCode = selectedItemDerivateCO.ProductCode;
            BasePrice = Convert.ToString(selectedItemDerivateCO.BasePrice);
            DeleteFlag = Convert.ToString(selectedItemDerivateCO.DeleteFlag);


        }

        internal void SetDataCurrency(ScripHelpBSECurrencyCO SelectedItemCurrency)
        {
            ScripID = SelectedItemCurrency.ScripID;
            ContractTokenNo = Convert.ToString(SelectedItemCurrency.ContractTokenNum);
            AssetTokenNo = Convert.ToString(SelectedItemCurrency.AssestTokenNum);
            UnderAsset = null;
            SName = SelectedItemCurrency.InstrumentName;
            InstType = SelectedItemCurrency.InstrumentType;
            AssetCode = SelectedItemCurrency.AssetCD;
            ExpDate = SelectedItemCurrency.ExpiryDate;
            StkPrice = SelectedItemCurrency.StrikePrice;
            OptionType = SelectedItemCurrency.OptionType;
            Precision = Convert.ToString(SelectedItemCurrency.Precision);
            MinLotSize = Convert.ToString(SelectedItemCurrency.MinimumLotSize);
            TickSize = Convert.ToString(SelectedItemCurrency.TickSize);
            PartitionID = Convert.ToString(SelectedItemCurrency.PartitionID);
            ProductID = Convert.ToString(SelectedItemCurrency.ProductID);
            CapacityGrpID = Convert.ToString(SelectedItemCurrency.CapacityGroupID);
            QtyMult = Convert.ToString(SelectedItemCurrency.QuantityMultiplier);
            UnderMarket = Convert.ToString(SelectedItemCurrency.UnderlyingMarket);
            ContractType = Convert.ToString(SelectedItemCurrency.ContractType);
            ProdCode = null;
            BasePrice = Convert.ToString(SelectedItemCurrency.BasePrice);
            DeleteFlag = Convert.ToString(SelectedItemCurrency.DeleteFlag);

        }

        internal void SetDataDerivateSPD(ScripHelpBSEDerivativeCO SelectedItemDerivateSPD)
        {
            ScripID = SelectedItemDerivateSPD.ScripID;
            ContractTokenNo = Convert.ToString(SelectedItemDerivateSPD.ContractTokenNum);
            AssetTokenNo = Convert.ToString(SelectedItemDerivateSPD.AssestTokenNum);
            UnderAsset = SelectedItemDerivateSPD.UnderlyingAsset;
            SName = SelectedItemDerivateSPD.InstrumentName;
            InstType = SelectedItemDerivateSPD.InstrumentType;
            AssetCode = SelectedItemDerivateSPD.AssetCd;
            ExpDate = SelectedItemDerivateSPD.ExpiryDate;
            StkPrice = Convert.ToString(SelectedItemDerivateSPD.StrikePrice);
            OptionType = SelectedItemDerivateSPD.OptionType;
            Precision = Convert.ToString(SelectedItemDerivateSPD.Precision);
            MinLotSize = Convert.ToString(SelectedItemDerivateSPD.MinimumLotSize);
            TickSize = Convert.ToString(SelectedItemDerivateSPD.TickSize);
            PartitionID = Convert.ToString(SelectedItemDerivateSPD.PartitionID);
            ProductID = Convert.ToString(SelectedItemDerivateSPD.ProductID);
            CapacityGrpID = Convert.ToString(SelectedItemDerivateSPD.CapacityGroupID);
            QtyMult = Convert.ToString(SelectedItemDerivateSPD.QuantityMultiplier);
            UnderMarket = Convert.ToString(SelectedItemDerivateSPD.UnderlyingMarket);
            ContractType = Convert.ToString(SelectedItemDerivateSPD.ContractType);
            ProdCode = SelectedItemDerivateSPD.ProductCode;
            BasePrice = Convert.ToString(SelectedItemDerivateSPD.BasePrice);
            DeleteFlag = Convert.ToString(SelectedItemDerivateSPD.DeleteFlag);
            CTNML1 = Convert.ToString(SelectedItemDerivateSPD.ContractTokenNum_Leg1);
            CTNML2 = Convert.ToString(SelectedItemDerivateSPD.ContractTokenNum_Leg2);
            NTAScripCode = Convert.ToString(SelectedItemDerivateSPD.NTAScripCode);
            StrategyID = Convert.ToString(SelectedItemDerivateSPD.StrategyID);
        }

        internal void SetDataCurrencySPD(ScripHelpBSECurrencyCO selectedItemCurrencySPD)
        {
            ScripID = selectedItemCurrencySPD.ScripID;
            ContractTokenNo = Convert.ToString(selectedItemCurrencySPD.ContractTokenNum);
            SName = selectedItemCurrencySPD.InstrumentName;
            InstType = selectedItemCurrencySPD.InstrumentType;
            AssetCode = selectedItemCurrencySPD.AssetCD;
            ExpDate = selectedItemCurrencySPD.ExpiryDate;
            StkPrice = selectedItemCurrencySPD.StrikePrice;
            OptionType = selectedItemCurrencySPD.OptionType;
            Precision = Convert.ToString(selectedItemCurrencySPD.Precision);
            MinLotSize = Convert.ToString(selectedItemCurrencySPD.MinimumLotSize);
            TickSize = Convert.ToString(selectedItemCurrencySPD.TickSize);
            PartitionID = Convert.ToString(selectedItemCurrencySPD.PartitionID);
            ProductID = Convert.ToString(selectedItemCurrencySPD.ProductID);
            CapacityGrpID = Convert.ToString(selectedItemCurrencySPD.CapacityGroupID);
            QtyMult = Convert.ToString(selectedItemCurrencySPD.QuantityMultiplier);
            UnderMarket = Convert.ToString(selectedItemCurrencySPD.UnderlyingMarket);
            ContractType = Convert.ToString(selectedItemCurrencySPD.ContractType);
            ProdCode = null;
            BasePrice = Convert.ToString(selectedItemCurrencySPD.BasePrice);
            DeleteFlag = Convert.ToString(selectedItemCurrencySPD.DeleteFlag);
            CTNML1 = Convert.ToString(selectedItemCurrencySPD.ContractTokenNum_Leg1);
            CTNML2 = Convert.ToString(selectedItemCurrencySPD.ContractTokenNum_Leg2);
            NTAScripCode = Convert.ToString(selectedItemCurrencySPD.NTAScripCode);
            StrategyID = Convert.ToString(selectedItemCurrencySPD.StrategyID);
        }


       


















        private static ScripInfo2VM _getInstance;

        public static ScripInfo2VM GetInstance
        {
            get
            {
                if (_getInstance == null)
                {
                    _getInstance = new ScripInfo2VM();
                }
                return _getInstance;
            }

        }








        #region NotifyProperty
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

        




        #endregion
    }
}
