using CommonFrontEnd.Common;
using CommonFrontEnd.View.Order;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.ViewModel.Order
{
    public class BatchErrorWindowVM:INotifyPropertyChanged
    {
        #region Properties
        private static int _ListTotalCount;

        public static int ListTotalCount
        {
            get { return _ListTotalCount; }
            set { _ListTotalCount = value; NotifyStaticPropertyChanged(nameof(ListTotalCount)); }
        }

        private static int _TotalCount=0;

        public static int TotalCount
        {
            get { return _TotalCount; }
            set { _TotalCount = value; NotifyStaticPropertyChanged(nameof(TotalCount)); }
        }

        private int _WarningCount;

        public int WarningCount
        {
            get { return _WarningCount; }
            set { _WarningCount = value; NotifyPropertyChanged(nameof(WarningCount)); }
        }

        private int _ErrorCount;

        public int ErrorCount
        {
            get { return _ErrorCount; }
            set { _ErrorCount = value; NotifyPropertyChanged(nameof(ErrorCount)); }
        }

        #endregion

        #region Collections
        private ObservableCollection<string> _ErrorneousCollection;
        public  ObservableCollection<string> ErrorneousCollection
        {
            get { return _ErrorneousCollection; }
            set
            {
                _ErrorneousCollection = value;
                //  NotifyPropertyChanged("SuccessCollection");
            }
        }
        #endregion

        #region RelayCommand
        private RelayCommand _BtnCloseWindow;
        public RelayCommand BtnCloseWindow
        {
            get
            {
                return _BtnCloseWindow ?? (_BtnCloseWindow = new RelayCommand(
                    (object e1) => OnClickOfCloseButton(e1)));
            }
        }
        #endregion

        public BatchErrorWindowVM()
        {
            DisplayData();
        }

        private void DisplayData()
        {
            ErrorneousCollection = new ObservableCollection<string>();
            foreach (var item in CommonFunctions.ErrorCollection)
            {
                ErrorneousCollection.Add(item);
            }
            //ListTotalCount = ErrorneousCollection.Count+ CommonFunctions.SuccessCollection.Count;
            ListTotalCount = TotalCount;
            WarningCount = CommonFunctions.WarningCount;
            ErrorCount = CommonFunctions.ErrorCount;
        }

        private void OnClickOfCloseButton(object e)
        {
            BatchErrorWindow b=e as BatchErrorWindow;
            //BatchErrorWindow b = new BatchErrorWindow();
            b.Close();
        }


        #region Notify Properties
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
        #endregion
        #region StaticNotifyPropertyChangedEvent
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged
                 = delegate { };
        private static void NotifyStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
