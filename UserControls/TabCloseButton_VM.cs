
using System;
using System.Windows;
using System.Linq;

using System.Collections.Generic;
//using System.ComponentModel;
//using System.Diagnostics;
//using System.IO;
//using System.Net;
//using System.Threading;
//using System.Windows.Input;
//using System.Windows.Media.Imaging;
using System.Windows.Controls;
using CommonFrontEnd.View.Touchline;
using CommonFrontEnd.ViewModel.Touchline;


namespace CommonFrontEnd.View.UserControls
{
    public partial class TabCloseButton_VM
    {
        public TabCloseButton_VM()
        {
        }

        public static void TabBtnClose_Click(object sender, RoutedEventArgs e)
        {
            
            TabControl maintab = System.Windows.Application.Current.Windows.OfType<MarketWatch>().FirstOrDefault().SnPSensexTab;

            string SelTabName = ((MarketWatchVM)maintab.DataContext).TabName.ToString();
            string CloseBtnTabName = ((TabCloseButton)(((FrameworkElement)((FrameworkElement)((FrameworkElement)((FrameworkElement)sender).Parent).Parent).Parent))).TabLblTitle.Content.ToString();
            string TabCloseType = ((FrameworkElement)((FrameworkElement)((FrameworkElement)((FrameworkElement)((FrameworkElement)e.OriginalSource).Parent).Parent).Parent).Parent).Name;
            //TabCloseType = ((FrameworkElement)((FrameworkElement)((FrameworkElement)((FrameworkElement)((FrameworkElement)sender).Parent).Parent).Parent).Parent).Name;

            //if (SelTabName == CloseBtnTabName)
            //    MarketWatchVM.DeleteTabItems_Click(maintab);

            List<TabItem> TabList = new List<TabItem>();
            TabList = maintab.ItemContainerGenerator.Items.Cast<TabItem>().ToList();

            for (int i = 0; i < TabList.Count; i++)
            {
                if (TabList[i].Header.ToString() != "Default")
                {
                    string tabName = ((CommonFrontEnd.View.UserControls.TabCloseButton)TabList[i].Header).TabLblTitle.Content.ToString();
                    string tabType = TabList[i].Name;

                    if (CloseBtnTabName == tabName && TabCloseType == tabType)
                    {
                        MarketWatchVM.DeleteTabItems_Click(TabList[i], i);
                        break;
                    }
                }
            }
        }
    }
}