using CommonFrontEnd.Common;
using CommonFrontEnd.Global;
using CommonFrontEnd.Model;
using CommonFrontEnd.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace CommonFrontEnd.ViewModel
{
   public class NewsVM : INotifyPropertyChanged
    {
        #region Properties
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

        
        #endregion
        #region Local Memory.
        private static ObservableCollection<NewsModel> _ObjNewsCollection = new ObservableCollection<NewsModel>();

        public  static ObservableCollection<NewsModel> ObjNewsCollection
        {
            get { return _ObjNewsCollection; }
            set { _ObjNewsCollection = value; /*NotifyStaticPropertyChanged(nameof(ObjIndicesCollection));*/ }
        }

       

        #endregion

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

        //private RelayCommand _CellClicked;

        //public RelayCommand CellClicked
        //{
        //    get
        //    {
        //        return _CellClicked ?? (_CellClicked = new RelayCommand((object e) => Link_CellClicked(e)));
        //    }
        //}

        private RelayCommand _AllBSENews;

        public RelayCommand AllBSENews
        {
            get
            {
                return _AllBSENews ?? (_AllBSENews = new RelayCommand((object e) => Link_BtnClicked(e)));
            }
        }

        private void Link_BtnClicked(object e)
        {
            //MouseButtonEventArgs objMouseButtonEventArgs = (MouseButtonEventArgs)e;
            //DataGrid Source = (DataGrid)objMouseButtonEventArgs.Source;
            //int index = Source.SelectedIndex;
            //ObservableCollection<NewsModel> obj = Source.ItemsSource as ObservableCollection<NewsModel>;

            string url;
            //if (obj.Count == 1 && obj[0].NewsHeadline == "All Broadcast News")
            //{
                url = "http://10.1.101.125:3000/twsreports/exchangeAnn.aspx?&memid=" + UtilityLoginDetails.GETInstance.MemberId +"&trdId="+ UtilityLoginDetails.GETInstance.TraderId;

            //}
            //else
            //{
            //    url = "http://10.1.101.125:3000/twsreports/exchangeAnn_news.aspx?id=" + obj[index].NewsId;
            //}

            ProcessStartInfo sInfo = new ProcessStartInfo(url);
            Process.Start(sInfo);
        }

        //public void Link_CellClicked(object e)
        //{
        //    MouseButtonEventArgs objMouseButtonEventArgs = (MouseButtonEventArgs)e;
        //    DataGrid Source = (DataGrid)objMouseButtonEventArgs.Source;
        //    int index = Source.SelectedIndex;
        //    ObservableCollection<NewsModel> obj = Source.ItemsSource as ObservableCollection<NewsModel>;
        //    // ObservableCollection< NewsModel > objNewsModelC= obj.SourceCollection as ObservableCollection<NewsModel>;

        //    string url;
        //    if (obj.Count == 1 && obj[0].NewsHeadline == "All Broadcast News")
        //    {
        //        url = "http://10.1.101.125:3000/twsreports/exchangeAnn.aspx?&memid=" + UtilityLoginDetails.GETInstance.MemberId;

        //    }
        //    //new System.Collections.Generic.Mscorlib_CollectionDebugView<CommonFrontEnd.Model.NewsModel>(((System.Windows.Controls.ItemsControl)objMouseButtonEventArgs.Source).ItemsSource).Items[0]._NewsHeadline
        //    else
        //    {
        //        url = "http://10.1.101.125:3000/twsreports/exchangeAnn_news.aspx?id=" + obj[index].NewsId;
        //    }

        //    ProcessStartInfo sInfo = new ProcessStartInfo(url);
        //    Process.Start(sInfo);
        //}
        private void EscapeUsingUserControl(object e)
        {
            News oNews = e as News;
            oNews?.Hide();
        }
    }
}
