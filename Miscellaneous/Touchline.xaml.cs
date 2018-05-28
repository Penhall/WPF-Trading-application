using CommonFrontEnd.Common;
using CommonFrontEnd.View;
using CommonFrontEnd.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CommonFrontEnd.View
{
    /// <summary>
    /// Interaction logic for Touchline.xaml
    /// </summary>
    public partial class Touchline : TitleBarHelperClass
    {
        public Touchline()
        {
            InitializeComponent();
            this.DataContext = new TouchlineVM();

            //lblStart.Content = DateTime.UtcNow.ToLocalTime().ToString("HH:mm:ss.fff",
            //                              CultureInfo.InvariantCulture);

        }

        private RelayCommand<string> ExportToExcel;
        public RelayCommand<string> ExportToExcel1
        {
            get { return ExportToExcel; }
            set { ExportToExcel = value; }
        }

        private RelayCommand<string> Window1_Loaded;
        public RelayCommand<string> Window1_Loaded1
        {
            get { return Window1_Loaded; }
            set { Window1_Loaded = value; }
        }

        /// <summary>
        /// This is the Win32 Interop Handle for this Window
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                return new WindowInteropHelper(this).Handle;
            }
        }
        public static readonly DependencyProperty ScrollChangedCommandProperty =
DependencyProperty.RegisterAttached("ScrollChangedCommand", typeof(ICommand), typeof(Touchline), new PropertyMetadata(default(ICommand), OnCommandPropertyChanged));

        private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataGrid dataGrid = d as DataGrid;
            if (dataGrid == null)
                return;
            if (e.NewValue != null)
            {
                dataGrid.Loaded += DataGridOnLoaded;
            }
            else if (e.OldValue != null)
            {
                dataGrid.Loaded -= DataGridOnLoaded;
            }


        }

        private static void DataGridOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid == null)
                return;

            ScrollViewer scrollViewer = UIHelper.FindChildren<ScrollViewer>(dataGrid)[0];
            if (scrollViewer != null)
            {
                scrollViewer.ScrollChanged += ScrollViewerOnScrollChanged;
            }
        }


        private static void ScrollViewerOnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            DataGrid dataGrid = UIHelper.FindParent<DataGrid>(sender as ScrollViewer);
            if (dataGrid != null)
            {
                ICommand command = GetScrollChangedCommand(dataGrid);
                command.Execute(e);
            }
        }

        public static void SetScrollChangedCommand(DependencyObject element, ICommand value)
        {
            element.SetValue(ScrollChangedCommandProperty, value);
        }

        public static ICommand GetScrollChangedCommand(DependencyObject element)
        {
            return (ICommand)element.GetValue(ScrollChangedCommandProperty);
        }



        //private void Start_Click(object sender, RoutedEventArgs e)
        //{
        //    lblStart.Content = DateTime.UtcNow.ToLocalTime().ToString("HH:mm:ss.fff",
        //                                    CultureInfo.InvariantCulture);
        //}
        //public delegate void OnStopClick();
        //public event OnStopClick OnStopClickEvent;
        //private void Stop_Click(object sender, RoutedEventArgs e)
        //{

        //    //lblStop.Content = DateTime.UtcNow.ToLocalTime().ToString("HH:mm:ss.fff",
        //    //                                CultureInfo.InvariantCulture);
        //    //lbltimer.Content = UV.Text;
        //    OnStopClickEvent += VM_OnStopClickEvent;

        //}

        //void VM_OnStopClickEvent()
        //{
        //    throw new NotImplementedException();
        //}


    }
}

