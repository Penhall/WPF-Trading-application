using CommonFrontEnd.Common;
using CommonFrontEnd.Global;
using CommonFrontEnd.Model.Order;
using CommonFrontEnd.Model.Trade;
using CommonFrontEnd.Processor.Order;
using CommonFrontEnd.View;
using CommonFrontEnd.Processor.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static CommonFrontEnd.Common.Enumerations;
using CommonFrontEnd.Model.ETIMessageStructure;
using CommonFrontEnd.Controller;

namespace CommonFrontEnd.ViewModel.PersonalDownload
{
    class PersonalDownloadVM : BaseViewModel
    {
        #region Properties
        static View.PersonalDownload.PersonalDownload mWindow = null;
        public static AutoResetEvent autoReset = new AutoResetEvent(false);
        private static TradeRequest _tradeRequest;
        private static TradeRequestProcessor _tradeRequestProcessor;
        //public static SynchronizationContext uiContent;
        static View.Trade.PersonalDownloadProgessWindow oPersonalDownloadProgessWindow = System.Windows.Application.Current.Windows.OfType<View.Trade.PersonalDownloadProgessWindow>().FirstOrDefault();

        static View.LocationID oLocationID = System.Windows.Application.Current.Windows.OfType<View.LocationID>().FirstOrDefault();
        static BestFiveMarketPicture mainwindow;
        #region OrderAllSegments
        //Order Eq
        private string _imgRingOrdEqVisibility = "Hidden";

        public string imgRingOrdEqVisibility
        {
            get { return _imgRingOrdEqVisibility; }
            set { _imgRingOrdEqVisibility = value; }
        }

        private string _imgticksOrdEqVisibility = "Hidden";

        public string imgticksOrdEqVisibility
        {
            get { return _imgticksOrdEqVisibility; }
            set { _imgticksOrdEqVisibility = value; NotifyPropertyChanged(nameof(imgticksOrdEqVisibility)); }
        }

        private string _imgCrossOrdEqVisibility = "Hidden";

        public string imgCrossOrdEqVisibility
        {
            get { return _imgCrossOrdEqVisibility; }
            set { _imgCrossOrdEqVisibility = value; NotifyPropertyChanged(nameof(imgCrossOrdEqVisibility)); }
        }

        //Order Der
        private string _imgRingOrdDerVisibility = "Hidden";

        public string imgRingOrdDerVisibility
        {
            get { return _imgRingOrdDerVisibility; }
            set { _imgRingOrdDerVisibility = value; }
        }

        private string _imgticksOrdDerVisibility = "Hidden";

        public string imgticksOrdDerVisibility
        {
            get { return _imgticksOrdDerVisibility; }
            set { _imgticksOrdDerVisibility = value; NotifyPropertyChanged(nameof(imgticksOrdDerVisibility)); }
        }

        private string _imgCrossOrdDerVisibility = "Hidden";

        public string imgCrossOrdDerVisibility
        {
            get { return _imgCrossOrdDerVisibility; }
            set { _imgCrossOrdDerVisibility = value; NotifyPropertyChanged(nameof(imgCrossOrdDerVisibility)); }
        }
        //Order Curr
        private string _imgRingOrdCurVisibility = "Hidden";

        public string imgRingOrdCurVisibility
        {
            get { return _imgRingOrdCurVisibility; }
            set { _imgRingOrdCurVisibility = value; }
        }
        private string _imgticksOrdCurVisibility = "Hidden";

        public string imgticksOrdCurVisibility
        {
            get { return _imgticksOrdCurVisibility; }
            set { _imgticksOrdCurVisibility = value; NotifyPropertyChanged(nameof(imgticksOrdCurVisibility)); }
        }

        private string _imgCrossOrdCurVisibility = "Hidden";

        public string imgCrossOrdCurVisibility
        {
            get { return _imgCrossOrdCurVisibility; }
            set { _imgCrossOrdCurVisibility = value; NotifyPropertyChanged(nameof(imgCrossOrdCurVisibility)); }
        }
        //Order OdLot
        private string _imgRingOrdOLVisibility = "Hidden";

        public string imgRingOrdOLVisibility
        {
            get { return _imgRingOrdOLVisibility; }
            set { _imgRingOrdOLVisibility = value; }
        }
        private string _imgticksOrdOLVisibility = "Hidden";

        public string imgticksOrdOLVisibility
        {
            get { return _imgticksOrdOLVisibility; }
            set { _imgticksOrdOLVisibility = value; NotifyPropertyChanged(nameof(imgticksOrdOLVisibility)); }
        }

        private string _imgCrossOrdOLVisibility = "Hidden";

        public string imgCrossOrdOLVisibility
        {
            get { return _imgCrossOrdOLVisibility; }
            set { _imgCrossOrdOLVisibility = value; NotifyPropertyChanged(nameof(imgCrossOrdOLVisibility)); }
        }
        #endregion

        #region TredesAllSegments
        //Order Eq
        private string _imgRingTrdEqVisibility = "Hidden";

        public string imgRingTrdEqVisibility
        {
            get { return _imgRingTrdEqVisibility; }
            set { _imgRingTrdEqVisibility = value; NotifyPropertyChanged(nameof(imgRingTrdEqVisibility)); }
        }

        private string _imgticksTrdEqVisibility = "Hidden";

        public string imgticksTrdEqVisibility
        {
            get { return _imgticksTrdEqVisibility; }
            set { _imgticksTrdEqVisibility = value; NotifyPropertyChanged(nameof(imgticksTrdEqVisibility)); }
        }

        private string _imgCrossTrdEqVisibility = "Hidden";

        public string imgCrossTrdEqVisibility
        {
            get { return _imgCrossTrdEqVisibility; }
            set { _imgCrossTrdEqVisibility = value; NotifyPropertyChanged(nameof(imgCrossTrdEqVisibility)); }
        }

        //Order Der
        private string _imgRingTrdDerVisibility = "Hidden";

        public string imgRingTrdDerVisibility
        {
            get { return _imgRingTrdDerVisibility; }
            set { _imgRingTrdDerVisibility = value; NotifyPropertyChanged(nameof(imgRingTrdDerVisibility)); }
        }

        private string _imgticksTrdDerVisibility = "Hidden";

        public string imgticksTrdDerVisibility
        {
            get { return _imgticksTrdDerVisibility; }
            set { _imgticksTrdDerVisibility = value; NotifyPropertyChanged(nameof(imgticksTrdDerVisibility)); }
        }

        private string _imgCrossTrdDerVisibility = "Hidden";

        public string imgCrossTrdDerVisibility
        {
            get { return _imgCrossTrdDerVisibility; }
            set { _imgCrossTrdDerVisibility = value; NotifyPropertyChanged(nameof(imgCrossTrdDerVisibility)); }
        }
        //Order Curr
        private string _imgRingTrdCurVisibility = "Hidden";

        public string imgRingTrdCurVisibility
        {
            get { return _imgRingTrdCurVisibility; }
            set { _imgRingTrdCurVisibility = value; NotifyPropertyChanged(nameof(imgRingTrdCurVisibility)); }
        }
        private string _imgticksTrdCurVisibility = "Hidden";

        public string imgticksTrdCurVisibility
        {
            get { return _imgticksTrdCurVisibility; }
            set { _imgticksTrdCurVisibility = value; NotifyPropertyChanged(nameof(imgticksTrdCurVisibility)); }
        }

        private string _imgCrossTrdCurVisibility = "Hidden";

        public string imgCrossTrdCurVisibility
        {
            get { return _imgCrossTrdCurVisibility; }
            set { _imgCrossTrdCurVisibility = value; NotifyPropertyChanged(nameof(imgCrossTrdCurVisibility)); }
        }
        //Order OdLot
        private string _imgRingTrdOLVisibility = "Hidden";

        public string imgRingTrdOLVisibility
        {
            get { return _imgRingTrdOLVisibility; }
            set { _imgRingTrdOLVisibility = value; NotifyPropertyChanged(nameof(imgRingTrdOLVisibility)); }
        }
        private string _imgticksTrdOLVisibility = "Hidden";

        public string imgticksTrdOLVisibility
        {
            get { return _imgticksTrdOLVisibility; }
            set { _imgticksTrdOLVisibility = value; NotifyPropertyChanged(nameof(imgticksTrdOLVisibility)); }
        }

        private string _imgCrossTrdOLVisibility = "Hidden";

        public string imgCrossTrdOLVisibility
        {
            get { return _imgCrossTrdOLVisibility; }
            set { _imgCrossTrdOLVisibility = value; NotifyPropertyChanged(nameof(imgCrossTrdOLVisibility)); }
        }
        #endregion

        #region SLOrdAllSegments
        //Order Eq
        private string _imgRingSLOrdEqVisibility = "Hidden";

        public string imgRingSLOrdEqVisibility
        {
            get { return _imgRingSLOrdEqVisibility; }
            set { _imgRingSLOrdEqVisibility = value; NotifyPropertyChanged(nameof(imgRingSLOrdEqVisibility)); }
        }

        private string _imgticksSLOrdEqVisibility = "Hidden";

        public string imgticksSLOrdEqVisibility
        {
            get { return _imgticksSLOrdEqVisibility; }
            set { _imgticksSLOrdEqVisibility = value; NotifyPropertyChanged(nameof(imgticksSLOrdEqVisibility)); }
        }

        private string _imgCrossSLOrdEqVisibility = "Hidden";

        public string imgCrossSLOrdEqVisibility
        {
            get { return _imgCrossSLOrdEqVisibility; }
            set { _imgCrossSLOrdEqVisibility = value; }
        }

        //Order Der
        private string _imgRingSLOrdDerVisibility = "Hidden";

        public string imgRingSLOrdDerVisibility
        {
            get { return _imgRingSLOrdDerVisibility; }
            set { _imgRingSLOrdDerVisibility = value; NotifyPropertyChanged(nameof(imgRingSLOrdDerVisibility)); }
        }

        private string _imgticksSLOrdDerVisibility = "Hidden";

        public string imgticksSLOrdDerVisibility
        {
            get { return _imgticksSLOrdDerVisibility; }
            set { _imgticksSLOrdDerVisibility = value; NotifyPropertyChanged(nameof(imgticksSLOrdDerVisibility)); }
        }

        private string _imgCrossSLOrdDerVisibility = "Hidden";

        public string imgCrossSLOrdDerVisibility
        {
            get { return _imgCrossSLOrdDerVisibility; }
            set { _imgCrossSLOrdDerVisibility = value; NotifyPropertyChanged(nameof(imgCrossSLOrdDerVisibility)); }
        }
        //Order Curr
        private string _imgRingSLOrdCurVisibility = "Hidden";

        public string imgRingSLOrdCurVisibility
        {
            get { return _imgRingSLOrdCurVisibility; }
            set { _imgRingSLOrdCurVisibility = value; NotifyPropertyChanged(nameof(imgRingSLOrdCurVisibility)); }
        }
        private string _imgticksSLOrdCurVisibility = "Hidden";

        public string imgticksSLOrdCurVisibility
        {
            get { return _imgticksSLOrdCurVisibility; }
            set { _imgticksSLOrdCurVisibility = value; NotifyPropertyChanged(nameof(imgticksSLOrdCurVisibility)); }
        }

        private string _imgCrossSLOrdCurVisibility = "Hidden";

        public string imgCrossSLOrdCurVisibility
        {
            get { return _imgCrossSLOrdCurVisibility; }
            set { _imgCrossSLOrdCurVisibility = value; NotifyPropertyChanged(nameof(imgCrossSLOrdCurVisibility)); }
        }
        //Order OdLot
        private string _imgRingSLOrdOLVisibility = "Hidden";

        public string imgRingSLOrdOLVisibility
        {
            get { return _imgRingSLOrdOLVisibility; }
            set { _imgRingSLOrdOLVisibility = value; NotifyPropertyChanged(nameof(imgRingSLOrdOLVisibility)); }
        }
        private string _imgticksSLOrdOLVisibility = "Hidden";

        public string imgticksSLOrdOLVisibility
        {
            get { return _imgticksSLOrdOLVisibility; }
            set { _imgticksSLOrdOLVisibility = value; NotifyPropertyChanged(nameof(imgticksSLOrdOLVisibility)); }
        }

        private string _imgCrossSLOrdOLVisibility = "Hidden";

        public string imgCrossSLOrdOLVisibility
        {
            get { return _imgCrossSLOrdOLVisibility; }
            set { _imgCrossSLOrdOLVisibility = value; NotifyPropertyChanged(nameof(imgCrossSLOrdOLVisibility)); }
        }
        #endregion

        #region RtOrdAllSegments
        //Order Eq
        private string _imgRingRtOrdEqVisibility = "Hidden";

        public string imgRingRtOrdEqVisibility
        {
            get { return _imgRingRtOrdEqVisibility; }
            set { _imgRingRtOrdEqVisibility = value; NotifyPropertyChanged(nameof(imgRingRtOrdEqVisibility)); }
        }

        private string _imgticksRtOrdEqVisibility = "Hidden";

        public string imgticksRtOrdEqVisibility
        {
            get { return _imgticksRtOrdEqVisibility; }
            set { _imgticksRtOrdEqVisibility = value; NotifyPropertyChanged(nameof(imgticksRtOrdEqVisibility)); }
        }

        private string _imgCrossRtOrdEqVisibility = "Hidden";

        public string imgCrossRtOrdEqVisibility
        {
            get { return _imgCrossRtOrdEqVisibility; }
            set { _imgCrossRtOrdEqVisibility = value; NotifyPropertyChanged(nameof(imgCrossRtOrdEqVisibility)); }
        }

        //Order Der
        private string _imgRingRtOrdDerVisibility = "Hidden";

        public string imgRingRtOrdDerVisibility
        {
            get { return _imgRingRtOrdDerVisibility; }
            set { _imgRingRtOrdDerVisibility = value; NotifyPropertyChanged(nameof(imgRingRtOrdDerVisibility)); }
        }

        private string _imgticksRtOrdDerVisibility = "Hidden";

        public string imgticksRtOrdDerVisibility
        {
            get { return _imgticksRtOrdDerVisibility; }
            set { _imgticksRtOrdDerVisibility = value; NotifyPropertyChanged(nameof(imgticksRtOrdDerVisibility)); }
        }

        private string _imgCrossRtOrdDerVisibility = "Hidden";

        public string imgCrossRtOrdDerVisibility
        {
            get { return _imgCrossRtOrdDerVisibility; }
            set { _imgCrossRtOrdDerVisibility = value; NotifyPropertyChanged(nameof(imgCrossRtOrdDerVisibility)); }
        }
        //Order Curr
        private string _imgRingRtOrdCurVisibility = "Hidden";

        public string imgRingRtOrdCurVisibility
        {
            get { return _imgRingRtOrdCurVisibility; }
            set { _imgRingRtOrdCurVisibility = value; NotifyPropertyChanged(nameof(imgRingRtOrdCurVisibility)); }
        }
        private string _imgticksRtOrdCurVisibility = "Hidden";

        public string imgticksRtOrdCurVisibility
        {
            get { return _imgticksRtOrdCurVisibility; }
            set { _imgticksRtOrdCurVisibility = value; NotifyPropertyChanged(nameof(imgticksRtOrdCurVisibility)); }
        }

        private string _imgCrossRtOrdCurVisibility = "Hidden";

        public string imgCrossRtOrdCurVisibility
        {
            get { return _imgCrossRtOrdCurVisibility; }
            set { _imgCrossRtOrdCurVisibility = value; NotifyPropertyChanged(nameof(imgCrossRtOrdCurVisibility)); }
        }
        //Order OdLot
        private string _imgRingRtOrdOLVisibility = "Hidden";

        public string imgRingRtOrdOLVisibility
        {
            get { return _imgRingRtOrdOLVisibility; }
            set { _imgRingRtOrdOLVisibility = value; NotifyPropertyChanged(nameof(imgRingRtOrdOLVisibility)); }
        }
        private string _imgticksRtOrdOLVisibility = "Hidden";

        public string imgticksRtOrdOLVisibility
        {
            get { return _imgticksRtOrdOLVisibility; }
            set { _imgticksRtOrdOLVisibility = value; NotifyPropertyChanged(nameof(imgticksRtOrdOLVisibility)); }
        }

        private string _imgCrossRtOrdOLVisibility = "Hidden";

        public string imgCrossRtOrdOLVisibility
        {
            get { return _imgCrossRtOrdOLVisibility; }
            set { _imgCrossRtOrdOLVisibility = value; NotifyPropertyChanged(nameof(imgCrossRtOrdOLVisibility)); }
        }
        #endregion

        #region RtSLOrderAllSegments
        //Order Eq
        private string _imgRingRtSLOrdEqVisibility = "Hidden";

        public string imgRingRtSLOrdEqVisibility
        {
            get { return _imgRingRtSLOrdEqVisibility; }
            set { _imgRingRtSLOrdEqVisibility = value; NotifyPropertyChanged(nameof(imgRingRtSLOrdEqVisibility)); }
        }

        private string _imgticksRtSLOrdEqVisibility = "Hidden";

        public string imgticksRtSLOrdEqVisibility
        {
            get { return _imgticksRtSLOrdEqVisibility; }
            set { _imgticksRtSLOrdEqVisibility = value; NotifyPropertyChanged(nameof(imgticksRtSLOrdEqVisibility)); }
        }

        private string _imgCrossRtSLOrdEqVisibility = "Hidden";

        public string imgCrossRtSLOrdEqVisibility
        {
            get { return _imgCrossRtSLOrdEqVisibility; }
            set { _imgCrossRtSLOrdEqVisibility = value; NotifyPropertyChanged(nameof(imgCrossRtSLOrdEqVisibility)); }
        }

        //Order Der
        private string _imgRingRtSLOrdDerVisibility = "Hidden";

        public string imgRingRtSLOrdDerVisibility
        {
            get { return _imgRingRtSLOrdDerVisibility; }
            set { _imgRingRtSLOrdDerVisibility = value; NotifyPropertyChanged(nameof(imgRingRtSLOrdDerVisibility)); }
        }

        private string _imgticksRtSLOrdDerVisibility = "Hidden";

        public string imgticksRtSLOrdDerVisibility
        {
            get { return _imgticksRtSLOrdDerVisibility; }
            set { _imgticksRtSLOrdDerVisibility = value; NotifyPropertyChanged(nameof(imgticksRtSLOrdDerVisibility)); }
        }

        private string _imgCrossRtSLOrdDerVisibility = "Hidden";

        public string imgCrossRtSLOrdDerVisibility
        {
            get { return _imgCrossRtSLOrdDerVisibility; }
            set { _imgCrossRtSLOrdDerVisibility = value; NotifyPropertyChanged(nameof(imgCrossRtSLOrdDerVisibility)); }
        }
        //Order Curr
        private string _imgRingRtSLOrdCurVisibility = "Hidden";

        public string imgRingRtSLOrdCurVisibility
        {
            get { return _imgRingRtSLOrdCurVisibility; }
            set { _imgRingRtSLOrdCurVisibility = value; NotifyPropertyChanged(nameof(imgRingRtSLOrdCurVisibility)); }
        }
        private string _imgticksRtSLOrdCurVisibility = "Hidden";

        public string imgticksRtSLOrdCurVisibility
        {
            get { return _imgticksRtSLOrdCurVisibility; }
            set { _imgticksRtSLOrdCurVisibility = value; NotifyPropertyChanged(nameof(imgticksRtSLOrdCurVisibility)); }
        }

        private string _imgCrossRtSLOrdCurVisibility = "Hidden";

        public string imgCrossRtSLOrdCurVisibility
        {
            get { return _imgCrossRtSLOrdCurVisibility; }
            set { _imgCrossRtSLOrdCurVisibility = value; NotifyPropertyChanged(nameof(imgCrossRtSLOrdCurVisibility)); }
        }
        //Order OdLot
        private string _imgRingRtSLOrdOLVisibility = "Hidden";

        public string imgRingRtSLOrdOLVisibility
        {
            get { return _imgRingRtSLOrdOLVisibility; }
            set { _imgRingRtSLOrdOLVisibility = value; NotifyPropertyChanged(nameof(imgRingRtSLOrdOLVisibility)); }
        }
        private string _imgticksRtSLOrdOLVisibility = "Hidden";

        public string imgticksRtSLOrdOLVisibility
        {
            get { return _imgticksRtSLOrdOLVisibility; }
            set { _imgticksRtSLOrdOLVisibility = value; NotifyPropertyChanged(nameof(imgticksRtSLOrdOLVisibility)); }
        }

        private string _imgCrossRtSLOrdOLVisibility = "Hidden";

        public string imgCrossRtSLOrdOLVisibility
        {
            get { return _imgCrossRtSLOrdOLVisibility; }
            set { _imgCrossRtSLOrdOLVisibility = value; NotifyPropertyChanged(nameof(imgCrossRtSLOrdOLVisibility)); }
        }
        #endregion

        #region GroupLimitAllSegments
        private string _imgRingGrpLmtEqVisibility;

        public string imgRingGrpLmtEqVisibility
        {
            get { return _imgRingGrpLmtEqVisibility; }
            set { _imgRingGrpLmtEqVisibility = value; }
        }

        private string _imgticksGrpLmtEqVisibility = "Hidden";

        public string imgticksGrpLmtEqVisibility
        {
            get { return _imgticksGrpLmtEqVisibility; }
            set { _imgticksGrpLmtEqVisibility = value; NotifyPropertyChanged(nameof(imgticksGrpLmtEqVisibility)); }
        }

        private string _imgCrossGrpLmtEqVisibility = "Hidden";

        public string imgCrossGrpLmtEqVisibility
        {
            get { return _imgCrossGrpLmtEqVisibility; }
            set { _imgCrossGrpLmtEqVisibility = value; NotifyPropertyChanged(nameof(imgCrossGrpLmtEqVisibility)); }
        }

        //Order Der
        private string _imgRingGrpLmtDerVisibility;

        public string imgRingGrpLmtDerVisibility
        {
            get { return _imgRingGrpLmtDerVisibility; }
            set { _imgRingGrpLmtDerVisibility = value; }
        }

        private string _imgticksGrpLmtDerVisibility = "Hidden";

        public string imgticksGrpLmtDerVisibility
        {
            get { return _imgticksGrpLmtDerVisibility; }
            set { _imgticksGrpLmtDerVisibility = value; NotifyPropertyChanged(nameof(imgticksGrpLmtDerVisibility)); }
        }

        private string _imgCrossGrpLmtDerVisibility = "Hidden";

        public string imgCrossGrpLmtDerVisibility
        {
            get { return _imgCrossGrpLmtDerVisibility; }
            set { _imgCrossGrpLmtDerVisibility = value; NotifyPropertyChanged(nameof(imgCrossGrpLmtDerVisibility)); }
        }
        //Order Curr
        private string _imgRingGrpLmtCurVisibility;

        public string imgRingGrpLmtCurVisibility
        {
            get { return _imgRingGrpLmtCurVisibility; }
            set { _imgRingGrpLmtCurVisibility = value; }
        }
        private string _imgticksGrpLmtCurVisibility = "Hidden";

        public string imgticksGrpLmtCurVisibility
        {
            get { return _imgticksGrpLmtCurVisibility; }
            set { _imgticksGrpLmtCurVisibility = value; NotifyPropertyChanged(nameof(imgticksGrpLmtCurVisibility)); }
        }

        private string _imgCrossGrpLmtCurVisibility = "Hidden";

        public string imgCrossGrpLmtCurVisibility
        {
            get { return _imgCrossGrpLmtCurVisibility; }
            set { _imgCrossGrpLmtCurVisibility = value; NotifyPropertyChanged(nameof(imgCrossGrpLmtCurVisibility)); }
        }
        //Order OdLot
        private string _imgRingGrpLmtOLVisibility;

        public string imgRingGrpLmtOLVisibility
        {
            get { return _imgRingGrpLmtOLVisibility; }
            set { _imgRingGrpLmtOLVisibility = value; }
        }
        private string _imgticksGrpLmtOLVisibility;

        public string imgticksGrpLmtOLVisibility
        {
            get { return _imgticksGrpLmtOLVisibility; }
            set { _imgticksGrpLmtOLVisibility = value; }
        }

        private string _imgCrossGrpLmtOLVisibility;

        public string imgCrossGrpLmtOLVisibility
        {
            get { return _imgCrossGrpLmtOLVisibility; }
            set { _imgCrossGrpLmtOLVisibility = value; }
        }
        #endregion

        #region TrdLimitAllSegments
        private string _imgRingTrdLmtEqVisibility = "Hidden";

        public string imgRingTrdLmtEqVisibility
        {
            get { return _imgRingTrdLmtEqVisibility; }
            set { _imgRingTrdLmtEqVisibility = value; }
        }

        private string _imgticksTrdLmtEqVisibility = "Hidden";

        public string imgticksTrdLmtEqVisibility
        {
            get { return _imgticksTrdLmtEqVisibility; }
            set { _imgticksTrdLmtEqVisibility = value; NotifyPropertyChanged(nameof(imgticksTrdLmtEqVisibility)); }
        }

        private string _imgCrossTrdLmtEqVisibility = "Hidden";

        public string imgCrossTrdLmtEqVisibility
        {
            get { return _imgCrossTrdLmtEqVisibility; }
            set { _imgCrossTrdLmtEqVisibility = value; NotifyPropertyChanged(nameof(imgCrossTrdLmtEqVisibility)); }
        }

        //Order Der
        private string _imgRingTrdLmtDerVisibility = "Hidden";

        public string imgRingTrdLmtDerVisibility
        {
            get { return _imgRingTrdLmtDerVisibility; }
            set { _imgRingTrdLmtDerVisibility = value; }
        }

        private string _imgticksTrdLmtDerVisibility = "Hidden";

        public string imgticksTrdLmtDerVisibility
        {
            get { return _imgticksTrdLmtDerVisibility; }
            set { _imgticksTrdLmtDerVisibility = value; NotifyPropertyChanged(nameof(imgticksTrdLmtDerVisibility)); }
        }

        private string _imgCrossTrdLmtDerVisibility = "Hidden";

        public string imgCrossTrdLmtDerVisibility
        {
            get { return _imgCrossTrdLmtDerVisibility; }
            set { _imgCrossTrdLmtDerVisibility = value; NotifyPropertyChanged(nameof(imgCrossTrdLmtDerVisibility)); }
        }
        //Order Curr
        private string _imgRingTrdLmtCurVisibility = "Hidden";

        public string imgRingTrdLmtCurVisibility
        {
            get { return _imgRingTrdLmtCurVisibility; }
            set { _imgRingTrdLmtCurVisibility = value; }
        }
        private string _imgticksTrdLmtCurVisibility = "Hidden";

        public string imgticksTrdLmtCurVisibility
        {
            get { return _imgticksTrdLmtCurVisibility; }
            set { _imgticksTrdLmtCurVisibility = value; NotifyPropertyChanged(nameof(imgticksTrdLmtCurVisibility)); }
        }

        private string _imgCrossTrdLmtCurVisibility = "Hidden";

        public string imgCrossTrdLmtCurVisibility
        {
            get { return _imgCrossTrdLmtCurVisibility; }
            set { _imgCrossTrdLmtCurVisibility = value; NotifyPropertyChanged(nameof(imgCrossTrdLmtCurVisibility)); }
        }
        //Order OdLot
        private string _imgRingTrdLmtOLVisibility = "Hidden";

        public string imgRingTrdLmtOLVisibility
        {
            get { return _imgRingTrdLmtOLVisibility; }
            set { _imgRingTrdLmtOLVisibility = value; }
        }
        private string _imgticksTrdLmtOLVisibility = "Hidden";

        public string imgticksTrdLmtOLVisibility
        {
            get { return _imgticksTrdLmtOLVisibility; }
            set { _imgticksTrdLmtOLVisibility = value; NotifyPropertyChanged(nameof(imgticksTrdLmtOLVisibility)); }
        }

        private string _imgCrossTrdLmtOLVisibility = "Hidden";

        public string imgCrossTrdLmtOLVisibility
        {
            get { return _imgCrossTrdLmtOLVisibility; }
            set { _imgCrossTrdLmtOLVisibility = value; }
        }
        private int _NormalOrdersCount;

        public int NormalOrdersCount
        {
            get { return _NormalOrdersCount; }
            set { _NormalOrdersCount = value; NotifyPropertyChanged(nameof(NormalOrdersCount)); }
        }

        private int _StopLossOrdersCount;

        public int StopLossOrdersCount
        {
            get { return _StopLossOrdersCount; }
            set { _StopLossOrdersCount = value; NotifyPropertyChanged(nameof(StopLossOrdersCount)); }
        }

        private int _ReturnOrdersCount;

        public int ReturnOrdersCount
        {
            get { return _ReturnOrdersCount; }
            set { _ReturnOrdersCount = value; NotifyPropertyChanged(nameof(ReturnOrdersCount)); }
        }

        private int _ReturnStopLossOrdersCount;

        public int ReturnStopLossOrdersCount
        {
            get { return _ReturnStopLossOrdersCount; }
            set { _ReturnStopLossOrdersCount = value; NotifyPropertyChanged(nameof(ReturnStopLossOrdersCount)); }
        }

        private int _TradeDownloadCount;

        public int TradeDownloadCount
        {
            get { return _TradeDownloadCount; }
            set { _TradeDownloadCount = value; NotifyPropertyChanged(nameof(TradeDownloadCount)); }
        }

        private int _GroupWiseLimitCount;

        public int GroupWiseLimitCount
        {
            get { return _GroupWiseLimitCount; }
            set { _GroupWiseLimitCount = value; NotifyPropertyChanged(nameof(GroupWiseLimitCount)); }
        }

        private int _TradeWiseLimitCount;

        public int TradeWiseLimitCount
        {
            get { return _TradeWiseLimitCount; }
            set { _TradeWiseLimitCount = value; NotifyPropertyChanged(nameof(TradeWiseLimitCount)); }
        }


        private static PersonalDownloadVM _getinstance;

        public static PersonalDownloadVM GetInstance
        {
            get
            {
                if (_getinstance == null)
                {
                    _getinstance = new PersonalDownloadVM();
                }
                return _getinstance;
            }
        }

        #endregion

        private string _ReplyMessageTxt = string.Empty;

        public string ReplyMessageTxt
        {
            get { return _ReplyMessageTxt; }
            set { _ReplyMessageTxt = value; NotifyPropertyChanged(nameof(ReplyMessageTxt)); }
        }


        #endregion

        #region Constructor

        private PersonalDownloadVM()
        {
            mWindow = System.Windows.Application.Current.Windows.OfType<View.PersonalDownload.PersonalDownload>().FirstOrDefault();
        }


        #endregion

        #region RelayCommands

        private RelayCommand _btnClickOk;

        public RelayCommand btnClickOk
        {
            get
            {
                return _btnClickOk ?? (_btnClickOk = new RelayCommand(
                    (object e) => btnClickOk_Click()));

            }
        }

        private RelayCommand _CloseWindowsOnEscape;

        public RelayCommand CloseWindowsOnEscape
        {
            get
            {
                return _CloseWindowsOnEscape ?? (_CloseWindowsOnEscape = new RelayCommand(
                    (object e) => mWindow?.Close()
                        ));
            }
        }


        #endregion

        #region Methods



        private void btnClickOk_Click()
        {
            View.PersonalDownload.PersonalDownload objPersonalDownload = Application.Current.Windows.OfType<View.PersonalDownload.PersonalDownload>().FirstOrDefault();
            if (objPersonalDownload != null)
                objPersonalDownload.Close();
            Application.Current.MainWindow.Focus();
            //OpenLocationID();
        }

        //private void OpenLocationID()
        //{
        //    LocationID objLocationID = Application.Current.Windows.OfType<LocationID>().FirstOrDefault();
        //    if (objLocationID != null)
        //    {
        //        if (objLocationID.WindowState == WindowState.Minimized)
        //            objLocationID.WindowState = WindowState.Normal;
        //        objLocationID.Focus();
        //        objLocationID.Activate();
        //        objLocationID.Show();
        //    }
        //    else
        //    {
        //        objLocationID = new LocationID();
        //        objLocationID.Owner = System.Windows.Application.Current.MainWindow;
        //        objLocationID.Show();

        //    }
        //}

        /// <summary>
        /// Processes Personal Download for Traders
        /// </summary>
        internal static void ProcessTrdrPrsnlDwld()
        {
            #region OrderPersonalDownload
            //uiContent = SynchronizationContext.Current;

            if (MainWindowVM.objLogOnStatus[(int)Segment.Equity].isLoggedIn)
            {
                UtilityLoginDetails.GETInstance.SegmentIndex = 0;
                QueryPersonalDownload("E", (uint)Enumerations.Trade.AdminTradeRequestMsgTag.EQTYSEGINDICATOR, MainWindowVM.objLogOnStatus[0].HFLFFLag);
            }

            if (MainWindowVM.objLogOnStatus[(int)Segment.Derivative].isLoggedIn)
            {
                UtilityLoginDetails.GETInstance.SegmentIndex = 1;
                QueryPersonalDownload("D", (uint)Enumerations.Trade.AdminTradeRequestMsgTag.DERISEGINDICATOR, MainWindowVM.objLogOnStatus[1].HFLFFLag);
            }

            if (MainWindowVM.objLogOnStatus[(int)Segment.Currency].isLoggedIn)
            {
                UtilityLoginDetails.GETInstance.SegmentIndex = 2;
                QueryPersonalDownload("C", (uint)Enumerations.Trade.AdminTradeRequestMsgTag.CURRSEGINDICATOR, MainWindowVM.objLogOnStatus[2].HFLFFLag);
            }
            //Allow online trade after Personal Download- Added by Gaurav Jadhav 15/3/2018
            UtilityLoginDetails.GETInstance.AllowOnlineTradeProcessingAfterPD = true;
            MainWindowVM.uiContext.Send(x => oPersonalDownloadProgessWindow = System.Windows.Application.Current.Windows.OfType<View.Trade.PersonalDownloadProgessWindow>().FirstOrDefault(), null);
            if (oPersonalDownloadProgessWindow != null)
            {
                MainWindowVM.uiContext.Send(x => oPersonalDownloadProgessWindow.Hide(), null);
                MainWindowVM.uiContext.Send(x => SharedMemories.MemoryManager.ProcessOnlineTrade(), null);
            }


            //Location ID Window logic
            string input = string.Format("LOCID{0}{1}", UtilityLoginDetails.GETInstance.MemberId.ToString(), UtilityLoginDetails.GETInstance.TraderId.ToString());
            string locidvalue = UtilityLoginDetails.GETInstance.SenderLocationId = MainWindowVM.parser.GetSetting("Login Settings", input);
            if (locidvalue == null)
            {
                //LocationID oLocationID = System.Windows.Application.Current.Windows.OfType<LocationID>().FirstOrDefault();
                MainWindowVM.uiContext.Send(x => oLocationID = System.Windows.Application.Current.Windows.OfType<View.LocationID>().FirstOrDefault(), null);
                if (oLocationID != null)
                {
                    MainWindowVM.uiContext.Send(x => oLocationID.Activate(), null);
                    MainWindowVM.uiContext.Send(x => oLocationID.Owner = System.Windows.Application.Current.MainWindow, null);
                    MainWindowVM.uiContext.Send(x => oLocationID.ShowDialog(), null);
                }
                else
                {
                    MainWindowVM.uiContext.Send(x => oLocationID = new LocationID(), null);
                    MainWindowVM.uiContext.Send(x => oLocationID.Activate(), null);
                    MainWindowVM.uiContext.Send(x => oLocationID.Owner = System.Windows.Application.Current.MainWindow, null);
                    MainWindowVM.uiContext.Send(x => oLocationID.Activate(), null);
                    MainWindowVM.uiContext.Send(x => oLocationID.ShowDialog(), null);
                }
            }
            //Location ID Window logic

            // For Best Five Screen
            {

                MainWindowVM.uiContext.Send(x => mainwindow = System.Windows.Application.Current.Windows.OfType<BestFiveMarketPicture>().FirstOrDefault(),null);
                if (mainwindow == null)
                    MainWindowVM.uiContext.Send(x => mainwindow = new BestFiveMarketPicture(),null);
                MainWindowVM.uiContext.Send(x => mainwindow.Hide(),null);
            }
            #endregion
        }

        /// <summary>
        /// Process Personal Download For Admin. Added by Gaurav Jadhav 16/3/2018
        /// </summary>
        internal static void ProcessAdminPrsnlDwld()
        {
            #region OrderPersonalDownload
            //uiContent = SynchronizationContext.Current;

            if (MainWindowVM.objLogOnStatus[(int)Segment.Equity].isLoggedIn)
            {
                UtilityLoginDetails.GETInstance.SegmentIndex = 0;
                QueryAdminPersonalDownload("E", (uint)Enumerations.Trade.AdminTradeRequestMsgTag.EQTYSEGINDICATOR, MainWindowVM.objLogOnStatus[0].HFLFFLag);
            }

            if (MainWindowVM.objLogOnStatus[(int)Segment.Derivative].isLoggedIn)
            {
                UtilityLoginDetails.GETInstance.SegmentIndex = 1;
                QueryAdminPersonalDownload("D", (uint)Enumerations.Trade.AdminTradeRequestMsgTag.DERISEGINDICATOR, MainWindowVM.objLogOnStatus[1].HFLFFLag);
            }

            if (MainWindowVM.objLogOnStatus[(int)Segment.Currency].isLoggedIn)
            {
                UtilityLoginDetails.GETInstance.SegmentIndex = 2;
                QueryAdminPersonalDownload("C", (uint)Enumerations.Trade.AdminTradeRequestMsgTag.CURRSEGINDICATOR, MainWindowVM.objLogOnStatus[2].HFLFFLag);
            }
            //Allow online trade after Personal Download- Added by Gaurav Jadhav 15/3/2018
            UtilityLoginDetails.GETInstance.AllowOnlineTradeProcessingAfterPD = true;

            MainWindowVM.uiContext.Send(x => SharedMemories.MemoryManager.ProcessOnlineTrade(), null);


            //Location ID Window logic
            string input = string.Format("LOCID{0}{1}", UtilityLoginDetails.GETInstance.MemberId.ToString(), UtilityLoginDetails.GETInstance.TraderId.ToString());
            string locidvalue = UtilityLoginDetails.GETInstance.SenderLocationId = MainWindowVM.parser.GetSetting("Login Settings", input);
            if (locidvalue == null)
            {
                //LocationID oLocationID = System.Windows.Application.Current.Windows.OfType<LocationID>().FirstOrDefault();
                MainWindowVM.uiContext.Send(x => oLocationID = System.Windows.Application.Current.Windows.OfType<View.LocationID>().FirstOrDefault(), null);
                if (oLocationID != null)
                {
                    MainWindowVM.uiContext.Send(x => oLocationID.Activate(), null);
                    MainWindowVM.uiContext.Send(x => oLocationID.Owner = System.Windows.Application.Current.MainWindow, null);
                    MainWindowVM.uiContext.Send(x => oLocationID.ShowDialog(), null);
                }
                else
                {
                    MainWindowVM.uiContext.Send(x => oLocationID = new LocationID(), null);
                    MainWindowVM.uiContext.Send(x => oLocationID.Activate(), null);
                    MainWindowVM.uiContext.Send(x => oLocationID.Owner = System.Windows.Application.Current.MainWindow, null);
                    MainWindowVM.uiContext.Send(x => oLocationID.Activate(), null);
                    MainWindowVM.uiContext.Send(x => oLocationID.ShowDialog(), null);
                }
            }
            //Location ID Window logic

            #endregion
        }

        /// <summary>
        /// Query Order Personal Download
        /// </summary>
        /// <param name="Segment"></param>
        /// <param name="MessageTag"></param>
        private static void QueryPersonalDownload(string Segment, uint MessageTag, ushort HFLFFlag)
        {
            if (HFLFFlag == (int)Enumerations.HFLFFlag.LFFlag) //Request Order, Trades and Limit
            {

                //Set reply and EOD flags to false
                UtilityLoginDetails.GETInstance.PersonalReplyReceived = false;
                UtilityLoginDetails.GETInstance.EndofdownloadReceived = false;
                GetInstance.ReplyMessageTxt = ConstantMessages.WaitOrderMessage;
                OrderRequestProcessor orderreqprocessor = null;
                OrderNomralRequest orequest = new OrderNomralRequest();
                orequest.Hour = "0";
                orequest.Minute = "0";
                orequest.Second = "0";
                orequest.Filler = Segment;
                orequest.MessageTag = MessageTag;
                orderreqprocessor = new OrderRequestProcessor(new OrderNormalPersonalDownload());
                orderreqprocessor.ProcessRequest(orequest, (int)OrderTypeDownload.NormalOrders);
                autoReset.WaitOne();
                GetInstance.ReplyMessageTxt = ConstantMessages.WaitRetOrderMessage;
                StartNextDownload(Segment, MessageTag, (int)OrderTypeDownload.ReturnOrders);
                autoReset.WaitOne();
                GetInstance.ReplyMessageTxt = ConstantMessages.WaitStopLossMessage;
                StartNextDownload(Segment, MessageTag, (int)OrderTypeDownload.StopLossOrders);
                autoReset.WaitOne();
                GetInstance.ReplyMessageTxt = ConstantMessages.WaitRetStopLossMessage;
                StartNextDownload(Segment, MessageTag, (int)OrderTypeDownload.ReturnStopLossOrders);
                autoReset.WaitOne();
                GetInstance.ReplyMessageTxt = ConstantMessages.WaitTradeMessage;
                StartNextTradeDownload(Segment, MessageTag, null);
                autoReset.WaitOne();
                GetInstance.ReplyMessageTxt = ConstantMessages.WaitLimitMessage;
                StartNextLimitDownload(MessageTag, null);
                autoReset.WaitOne();
                GetInstance.ReplyMessageTxt = ConstantMessages.WaitGWLimitMessage;
                StartNextLimitDownload(MessageTag, UtilityLoginDetails.GETInstance.TraderId);
                autoReset.WaitOne();
            }
            else if (HFLFFlag == (int)Enumerations.HFLFFlag.HFFlag) // Request Only Trades and Limits
            {
                GetInstance.ReplyMessageTxt = ConstantMessages.WaitTradeMessage;
                StartNextTradeDownload(Segment, MessageTag, null);
                autoReset.WaitOne();
                GetInstance.ReplyMessageTxt = ConstantMessages.WaitLimitMessage;
                StartNextLimitDownload(MessageTag, null);
                autoReset.WaitOne();
                GetInstance.ReplyMessageTxt = ConstantMessages.WaitGWLimitMessage;
                StartNextLimitDownload(MessageTag, UtilityLoginDetails.GETInstance.TraderId);
                autoReset.WaitOne();
            }

            GetInstance.ReplyMessageTxt = ConstantMessages.CompletedMessage;
        }

        /// <summary>
        /// Query Admin Personal Download. Added by Gaurav Jadhav 16/3/2018
        /// </summary>
        /// <param name="Segment"></param>
        /// <param name="MessageTag"></param>
        /// <param name="HFLFFlag"></param>
        private static void QueryAdminPersonalDownload(string Segment, uint MessageTag, ushort HFLFFlag)
        {


            //Set reply and EOD flags to false
            UtilityLoginDetails.GETInstance.PersonalReplyReceived = false;
            UtilityLoginDetails.GETInstance.EndofdownloadReceived = false;
            //      GetInstance.ReplyMessageTxt = ConstantMessages.WaitOrderMessage;

            //GetInstance.ReplyMessageTxt = ConstantMessages.WaitTradeMessage;
            StartNextAdminTradeDownload(Segment, MessageTag, null);
            autoReset.WaitOne();

            //GetInstance.ReplyMessageTxt = ConstantMessages.CompletedMessage;
        }

        private static void StartNextLimitDownload(uint messageTag, uint? TraderID)
        {
            //Set reply and EOD flags to false
            UtilityLoginDetails.GETInstance.PersonalReplyReceived = false;
            UtilityLoginDetails.GETInstance.EndofdownloadReceived = false;

            if (TraderID == null)
            {
                ETradeLimitRequest oETradeLimitRequest = new ETradeLimitRequest();
                oETradeLimitRequest.MessageTag = messageTag;
                SenderController.GetInstance.CheckAvailableMemory(oETradeLimitRequest);
            }

            else
            {
                ETradeGWLimitRequest oETradeGWLimitRequest = new ETradeGWLimitRequest();
                oETradeGWLimitRequest.MessageTag = messageTag;
                oETradeGWLimitRequest.TraderId = (uint)TraderID;
                SenderController.GetInstance.CheckAvailableMemory(oETradeGWLimitRequest);
            }
        }

        private static void StartNextTradeDownload(string segment, uint messageTag, int? messageType)
        {
            //Set reply and EOD flags to false
            UtilityLoginDetails.GETInstance.PersonalReplyReceived = false;
            UtilityLoginDetails.GETInstance.EndofdownloadReceived = false;

            _tradeRequest = new TradeRequest();
            _tradeRequest.AppBeginSequenceNum = 1;
            _tradeRequest.AppEndSequenceNum = 1;
            _tradeRequest.Exchange = 1;
            _tradeRequest.Market = 1;
            //_tradeRequest.MessageTag = MemoryManager.GetMesageTag();
            _tradeRequest.PartitionID = 1;
            _tradeRequest.ReservedField = 0;
            _tradeRequest.Hour = "0";
            _tradeRequest.Minute = "0";
            _tradeRequest.Second = "0";
            _tradeRequest.Filler = segment;

            if (segment == "E")
                _tradeRequest.MessageTag = (uint)Enumerations.Trade.AdminTradeRequestMsgTag.EQTYSEGINDICATOR;
            if (segment == "D")
                _tradeRequest.MessageTag = (uint)Enumerations.Trade.AdminTradeRequestMsgTag.DERISEGINDICATOR;
            if (segment == "C")
                _tradeRequest.MessageTag = (uint)Enumerations.Trade.AdminTradeRequestMsgTag.CURRSEGINDICATOR;

            _tradeRequestProcessor = new TradeRequestProcessor(new TraderTradePersonalDownload());
            _tradeRequestProcessor.ProcessRequest(_tradeRequest, (int)TradeTypeDownload.TradeRequest);
        }

        private static void StartNextAdminTradeDownload(string segment, uint messageTag, int? messageType)
        {
            //Set reply and EOD flags to false
            UtilityLoginDetails.GETInstance.PersonalReplyReceived = false;
            UtilityLoginDetails.GETInstance.EndofdownloadReceived = false;

            _tradeRequest = new TradeRequest();
            _tradeRequest.AppBeginSequenceNum = 1;
            _tradeRequest.AppEndSequenceNum = 1;
            _tradeRequest.Exchange = 1;
            _tradeRequest.Market = 1;
            //_tradeRequest.MessageTag = MemoryManager.GetMesageTag();
            _tradeRequest.PartitionID = 1;
            _tradeRequest.ReservedField = 0;
            _tradeRequest.Hour = "0";
            _tradeRequest.Minute = "0";
            _tradeRequest.Second = "0";
            _tradeRequest.Filler = segment;

            if (segment == "E")
                _tradeRequest.MessageTag = (uint)Enumerations.Trade.AdminTradeRequestMsgTag.EQTYSEGINDICATOR;
            if (segment == "D")
                _tradeRequest.MessageTag = (uint)Enumerations.Trade.AdminTradeRequestMsgTag.DERISEGINDICATOR;
            if (segment == "C")
                _tradeRequest.MessageTag = (uint)Enumerations.Trade.AdminTradeRequestMsgTag.CURRSEGINDICATOR;

            _tradeRequestProcessor = new TradeRequestProcessor(new AdminTradePersonalDownload());
            _tradeRequestProcessor.ProcessRequest(_tradeRequest, (int)TradeTypeDownload.TradeRequest);
        }
        private static void StartNextDownload(string segment, uint messageTag, int messageType)
        {
            //Set reply and EOD flags to false
            UtilityLoginDetails.GETInstance.PersonalReplyReceived = false;
            UtilityLoginDetails.GETInstance.EndofdownloadReceived = false;


            OrderRequestProcessor orderreqprocessor = null;
            OrderNomralRequest orequest = new OrderNomralRequest();
            orequest.Hour = "0";
            orequest.Minute = "0";
            orequest.Second = "0";
            orequest.Filler = segment;
            orequest.MessageTag = messageTag;
            if (messageType == (int)Enumerations.OrderTypeDownload.ReturnOrders)
                orderreqprocessor = new OrderRequestProcessor(new OrderRetPersonalDownload());
            else if (messageType == (int)Enumerations.OrderTypeDownload.StopLossOrders)
                orderreqprocessor = new OrderRequestProcessor(new OrderStopLossPersonalDownload());
            else if (messageType == (int)Enumerations.OrderTypeDownload.ReturnStopLossOrders)
                orderreqprocessor = new OrderRequestProcessor(new OrderRetStopPersonalDownload());
            orderreqprocessor.ProcessRequest(orequest, messageType);
        }

        #endregion
    }
}
