using CommonFrontEnd.Model.CorporateAction;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.ViewModel.CorporateAction
{
    public class DetailedCorporateActionVM : INotifyPropertyChanged
    {

        private List<DetailedCorporateActionModel> _PurposeNameList;
        public List<DetailedCorporateActionModel> PurposeNameList
        {
            get { return _PurposeNameList; }
            set { _PurposeNameList = value; }
        }

        public DetailedCorporateActionVM()
        {
            PurposeNameList = new List<DetailedCorporateActionModel>();
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "AC", PurposeFullName = "Accounts" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "ACT", PurposeFullName = "Accounts" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "AF", PurposeFullName = "Accounts" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "AGM", PurposeFullName = "A.G.M" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "AM", PurposeFullName = "Amalgamation" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "AR", PurposeFullName = "Audited Results" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "BGM", PurposeFullName = "Buy Back of Shares" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "BN", PurposeFullName = "Bonus issue" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "BOR", PurposeFullName = "Redemption of Bonds" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "BPR", PurposeFullName = "Redemption (Part) of Bonds" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "CDR", PurposeFullName = "CDR Package" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "CS", PurposeFullName = "Consolidation of Shares" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "DMF", PurposeFullName = "Dividend On Mutual Fund" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "DP", PurposeFullName = "Dividend" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "DPP", PurposeFullName = "Dividend on Preference Shares" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "EGM", PurposeFullName = "E.G.M." });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "ESC", PurposeFullName = "Exchange of Share Certificate" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "ESO", PurposeFullName = "Employees Stock Option Plan" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "ET", PurposeFullName = "Right Issue of Equity Shares" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "ETW", PurposeFullName = "Right Issue of Equity Shares with Warrants" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "FC", PurposeFullName = "Conversion of FCD" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "FD", PurposeFullName = "Final Dividend" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "FT", PurposeFullName = "Right Issue of Fully Convertible Debentures" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "FTW", PurposeFullName = "Right Issue of FCD with Warrants" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "HYR", PurposeFullName = "Half Yearly Results" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "IAC", PurposeFullName = "Increase in Authorised Capital" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "ID", PurposeFullName = "Interim Dividend" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "IM1", PurposeFullName = "General 1" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "IM2", PurposeFullName = "General 2" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "IM3", PurposeFullName = "General 3" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "IPB", PurposeFullName = "Payment of Interest for Bonds" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "IPD", PurposeFullName = "Payment of Interest" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "IPM", PurposeFullName = "Income Distribution of Mutual Fund" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "IW", PurposeFullName = "Issue Of Warrants" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "MC", PurposeFullName = "Conversion of Mutual Fund" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "MR", PurposeFullName = "Redemption of Mutual Fund" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "MT", PurposeFullName = "Right Issue of Mutual Fund" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "NPR", PurposeFullName = "Redemption(Part) of NCD" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "NR", PurposeFullName = "Redemption of NCD" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "NT", PurposeFullName = "Right Issue of Non Convertible Debentures" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "NTW", PurposeFullName = "Right Issue of NCD with Warrants" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "OGM", PurposeFullName = "General" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "PC", PurposeFullName = "Conversion of PCD" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "PFC", PurposeFullName = "Conversion of Preference Share" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "PFR", PurposeFullName = "Redemption of Preference Share" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "PFT", PurposeFullName = "Right Issue of Preference Shares" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "PO", PurposeFullName = "Preferential Issue of shares" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "PPC", PurposeFullName = "Conversion of Partly Paid up" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "PR", PurposeFullName = "Redemption of PCD" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "PRP", PurposeFullName = "Redemption(part) PCD" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "PT", PurposeFullName = "Right Issue of Partly Convertible Debentures" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "PTW", PurposeFullName = "Right Issue of PCD with Warrants" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "QR", PurposeFullName = "Quarterly Results" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "RF", PurposeFullName = "Re-Issue of Forfeited Equity Shares" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "RI", PurposeFullName = "Rights Issue" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "ROC", PurposeFullName = "Reduction of Capital" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "SA", PurposeFullName = "Scheme of Arrangement" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "SD", PurposeFullName = "Sub Division of Equity shares" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "SO", PurposeFullName = "Spin Off" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "SS", PurposeFullName = "Stock Split" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "STP", PurposeFullName = "Status of Project" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "SV", PurposeFullName = "Special Dividend" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "UFR", PurposeFullName = "Unaudited Financial Results" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "UO", PurposeFullName = "Purchase Offer" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "VGM", PurposeFullName = "Voluntary Delisting of Shares" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "WC", PurposeFullName = "Conversion of Warrants" });
            PurposeNameList.Add(new DetailedCorporateActionModel { Purpose = "WR", PurposeFullName = "Redemption of Warrants" });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, e);
            }
        }
    }
}

