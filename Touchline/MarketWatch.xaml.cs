using CommonFrontEnd.Common;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CommonFrontEnd.ViewModel;
using System.Windows.Interop;
using CommonFrontEnd.View;
using CommonFrontEnd.ViewModel.Touchline;
using System.Collections;
using System.ComponentModel;

namespace CommonFrontEnd.View.Touchline
{
    /// <summary>
    /// Interaction logic for MarketWatch.xaml
    /// </summary>
    public partial class MarketWatch : TitleBarHelperClass
    {
        public MarketWatch()
        {
            InitializeComponent();
            this.DataContext = new MarketWatchVM();
            this.SourceInitialized += (x, y) =>
            {
                WindowExtensions.HideMinimizeAndMaximizeButtons(this);
            };
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Hide();
        }

        private ObjectDataProvider MyObjectDataProvider
        {
            get
            {
                return this.TryFindResource("ObjTouchlineDataCollection") as ObjectDataProvider;
            }
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
DependencyProperty.RegisterAttached("ScrollChangedCommand", typeof(ICommand), typeof(MarketWatch), new PropertyMetadata(default(ICommand), OnCommandPropertyChanged));

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

        private void DataGrid_MouseMove(object sender, MouseEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            int selIndex = dg.SelectedIndex == -1 ? 0 : dg.SelectedIndex;
            if (selIndex != -1)
            {
                if (dg != null && dg.SelectedItems.Count == 1 && dg.SelectedItem != null && e.LeftButton == MouseButtonState.Pressed  )
                {
                    DataGridRow rowItem = (DataGridRow)((System.Windows.Controls.ItemsControl)e.Source).ItemContainerGenerator.ContainerFromIndex(selIndex);
                    if (rowItem != null)
                    {
                        double rowWidth = rowItem.ActualWidth;
                        //double rowWidth = ((DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(selIndex)).ActualWidth;
                        double capturedWidth = ((System.Windows.FrameworkElement)((System.Windows.Input.MouseDevice)e.Device).DirectlyOver).ActualWidth;
                        double capturedHeight = ((System.Windows.FrameworkElement)((System.Windows.Input.MouseDevice)e.Device).DirectlyOver).ActualHeight;

                        //if(capturedWidth <= rowWidth)
                        if ((capturedHeight > 17) && (capturedWidth > 17) && (rowWidth != capturedWidth))
                            DragDrop.DoDragDrop(dg, dg.SelectedItem, DragDropEffects.Copy);
                    }
                }
            }
        }

        private void DataGrid_DragEnter(object sender, DragEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            if (dg != null)
            {
                e.Effects = DragDropEffects.Copy | DragDropEffects.Move;

                //if (e.Data.GetDataPresent(DataFormats.Text))
                //    e.Effects = DragDropEffects.Copy;
                //else
                //    e.Effects = DragDropEffects.None;
            }

        }

        private void DataGrid_DragLeave(object sender, DragEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            if (dg != null)
            {
                e.Effects = DragDropEffects.None;
                //dg.SelectedItem = null;
            }
        }

        private void DataGrid_DragOver(object sender, DragEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            if (dg != null)
            {
                e.Effects = DragDropEffects.Copy | DragDropEffects.Move;
                //e.Effects = DragDropEffects.None;
            }
        }
    }
}
