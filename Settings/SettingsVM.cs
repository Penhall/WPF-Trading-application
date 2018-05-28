using CommonFrontEnd;
using CommonFrontEnd.Common;
using CommonFrontEnd.Model;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.View;
using CommonFrontEnd.View.Profiling;
using CommonFrontEnd.View.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CommonFrontEnd.ViewModel.Settings
{

    public partial class SettingsVM
    {
        #region Properties
        ProfileSettings SettingsWindow = null;
        private string _TwsTabVisibility;

        public string TwsTabVisibility
        {
            get { return _TwsTabVisibility; }
            set { _TwsTabVisibility = value; NotifyPropertyChanged(nameof(TwsTabVisibility)); }
        }


        private string _BowTabVisibility;

        public string BowTabVisibility
        {
            get { return _BowTabVisibility; }
            set { _BowTabVisibility = value; NotifyPropertyChanged(nameof(BowTabVisibility)); }
        }
        private RelayCommand _TabSelectionChanged;

        public RelayCommand TabSelectionChanged
        {
            get
            {
                return _TabSelectionChanged ?? (_TabSelectionChanged = new RelayCommand(
                    (object e) => TabSelectionChanged_Event(e)
                        ));
            }
        }

        private void TabSelectionChanged_Event(object e)
        {
            TabControl tabcontrol = e as TabControl;


            if (tabcontrol.SelectedIndex == 1)
            {
                TabItem Tabitemvalue = (TabItem)tabcontrol.Items[1];
                if (Tabitemvalue.Content == null)
                    Tabitemvalue.Content = new ColourProfiling();
                Tabitemvalue.IsSelected = true;
                ((ColourProfiling)SettingsWindow?.ColourProfiling?.Content)?.WindowsAvailable?.Focus();
                return;
            }

            else if (tabcontrol.SelectedIndex == 3)
            {
                TabItem Tabitemvalue = (TabItem)tabcontrol.Items[3];
                if (Tabitemvalue.Content == null)
                    Tabitemvalue.Content = new BoltSettings();
                return;
            }

            else if (tabcontrol.SelectedIndex == 4)
            {
                TabItem Tabitemvalue = (TabItem)tabcontrol.Items[4];
                if (Tabitemvalue.Content == null)
                    Tabitemvalue.Content = new OrderProfiling();
                ((OrderProfiling)SettingsWindow?.OrderProfiling?.Content)?.TxtRevQty?.Focus();
                return;
            }

            else if (tabcontrol.SelectedIndex == 5)
            {
                TabItem Tabitemvalue = (TabItem)tabcontrol.Items[5];
                if (Tabitemvalue.Content == null)
                    Tabitemvalue.Content = new ColumnProfiling();
                return;
            }

            else if (tabcontrol.SelectedIndex == 6)
            {
                TabItem Tabitemvalue = (TabItem)tabcontrol.Items[6];
                if (Tabitemvalue.Content == null)
                    Tabitemvalue.Content = new ClientProfiling();

                ((ClientProfiling)SettingsWindow?.ClientProfiling?.Content)?.ClientSearch?.Focus();
                return;
            }

            else if (tabcontrol.SelectedIndex == 7)
            {
                TabItem Tabitemvalue = (TabItem)tabcontrol.Items[7];
                if (Tabitemvalue.Content == null)
                    Tabitemvalue.Content = new EmailProfiling();
                return;
            }

            else if (tabcontrol.SelectedIndex == 8)
            {
                TabItem Tabitemvalue = (TabItem)tabcontrol.Items[8];
                if (Tabitemvalue.Content == null)
                    Tabitemvalue.Content = new FunctionKeys();
                return;
            }

            else if (tabcontrol.SelectedIndex == 9)
            {
                TabItem Tabitemvalue = (TabItem)tabcontrol.Items[9];
                if (Tabitemvalue.Content == null)
                    Tabitemvalue.Content = new ThemeColor1();
                return;
            }

        }


        #endregion


        #region NotifyPropertyChange

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


        public SettingsVM()
        {
#if TWS

            TwsTabVisibility = "Visible";
            BowTabVisibility = "Collapsed";

            if (ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = new BoltAppSettingsWindowsPosition();
                oBoltAppSettingsWindowsPosition = (BoltAppSettingsWindowsPosition)ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.SETTINGS != null && oBoltAppSettingsWindowsPosition.SETTINGS.WNDPOSITION != null)
                {
                    Height = oBoltAppSettingsWindowsPosition.SETTINGS.WNDPOSITION.Down;
                    TopPosition = oBoltAppSettingsWindowsPosition.SETTINGS.WNDPOSITION.Top;
                    LeftPosition = oBoltAppSettingsWindowsPosition.SETTINGS.WNDPOSITION.Left;
                    Width = oBoltAppSettingsWindowsPosition.SETTINGS.WNDPOSITION.Right;
                }
            }
            SettingsWindow = System.Windows.Application.Current.Windows.OfType<ProfileSettings>().FirstOrDefault();

#elif BOW

            TwsTabVisibility = "Collapsed";
            BowTabVisibility = "Visible";

#endif

        }

    }

    public partial class SettingsVM
    {
#if TWS

        #region Properties

        private int _LeftPosition = 345;

        public int LeftPosition
        {
            get { return _LeftPosition; }
            set { _LeftPosition = value; NotifyPropertyChanged("LeftPosition"); }
        }

        private int _TopPosition = 200;

        public int TopPosition
        {
            get { return _TopPosition; }
            set { _TopPosition = value; NotifyPropertyChanged("TopPosition"); }
        }

        private int _Width = 449;

        public int Width
        {
            get { return _Width; }
            set { _Width = value; NotifyPropertyChanged("Width"); }
        }


        private int _Height = 358;

        public int Height
        {
            get { return _Height; }
            set { _Height = value; NotifyPropertyChanged("Height"); }
        }

        #endregion


        #region RelayCommand

        private RelayCommand _SettingClosing;

        public RelayCommand SettingClosing
        {

            get { return _SettingClosing ?? (_SettingClosing = new RelayCommand((object e) => _Setting_Closing(e))); }

        }

        private void _Setting_Closing(object e)
        {
            Windows_SettingsLocationChanged(e);
            //if (CommonFrontEnd.ViewModel.Profiling.FunctionKeysVM.ChkAlternate == true)
            //{
            //    MainWindowVM.parser.AddSetting("Login Settings", "NEWBUTTONS", "0");
            //}
            //else
            //{
            //    MainWindowVM.parser.AddSetting("Login Settings", "NEWBUTTONS", "1");
            //}
            //MainWindowVM.parser.SaveSettings(MainWindowVM.TwsINIPath.ToString());
            //System.Windows.MessageBox.Show("To bring the changes in the effect, Please restart CFE Application.", "Information");
            //MainWindowVM.ShortCutKeysFlag;
            if (CommonFrontEnd.ViewModel.Profiling.FunctionKeysVM.ChkAlternate == true)
            {
                if (MainWindowVM.ShortCutKeysFlag != "0")
                {
                    MainWindowVM.parser.AddSetting("Login Settings", "NEWBUTTONS", "0");
                    MainWindowVM.parser.SaveSettings(MainWindowVM.TwsINIPath.ToString());
                    System.Windows.MessageBox.Show("To bring the changes in the effect, Please Close the Current Window.", "Information");
                    MainWindowVM.ShortCutKeysFlag = "0";
                    MainWindowVM.GetInstance.RegisterGlobalKeys();
                }

            }
            if (CommonFrontEnd.ViewModel.Profiling.FunctionKeysVM.ChkExisting == true)
            {
                if (MainWindowVM.ShortCutKeysFlag != "1")
                {
                    MainWindowVM.parser.AddSetting("Login Settings", "NEWBUTTONS", "1");
                    MainWindowVM.parser.SaveSettings(MainWindowVM.TwsINIPath.ToString());
                    System.Windows.MessageBox.Show("To bring the changes in the effect, Please Close the Current Window.", "Information");
                    MainWindowVM.ShortCutKeysFlag = "1";
                    MainWindowVM.GetInstance.RegisterGlobalKeys();
                }
            }
        }
        private void DeactivateForChangeShortCutKey()
        {
            if (HotKey._dictHotKeyToCalBackProc != null && HotKey._dictHotKeyToCalBackProc.Count > 0)
            {
                foreach (var item in HotKey._dictHotKeyToCalBackProc?.ToList())
                {
                    HotKey._dictHotKeyToCalBackProc.Remove(item.Key);
                    HotKey.UnregisterHotKey(IntPtr.Zero, item.Key);
                }
            }
        }
        private void ActivateForChangeShortCutKey()
        {
            if (MainWindowVM.GetInstance != null)
            {
                // MainWindowVM.GetInstance.RegisterGlobalKeys();
            }
        }

        private RelayCommand _myLocationChanged;

        public RelayCommand myLocationChanged
        {
            get
            {
                return _myLocationChanged ?? (_myLocationChanged = new RelayCommand(
                    (object e) => Windows_SettingsLocationChanged(e)));

            }
        }

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


        private void Windows_SettingsLocationChanged(object e)
        {
            if (ConfigurationMasterMemory.ConfigurationDict.ContainsKey("WindowsPosition"))
            {
                BoltAppSettingsWindowsPosition oBoltAppSettingsWindowsPosition = (BoltAppSettingsWindowsPosition)ConfigurationMasterMemory.ConfigurationDict["WindowsPosition"];
                if (oBoltAppSettingsWindowsPosition != null && oBoltAppSettingsWindowsPosition.SETTINGS != null && oBoltAppSettingsWindowsPosition.SETTINGS.WNDPOSITION != null)
                {
                    oBoltAppSettingsWindowsPosition.SETTINGS.WNDPOSITION.Left = Convert.ToInt32(LeftPosition);
                    oBoltAppSettingsWindowsPosition.SETTINGS.WNDPOSITION.Top = Convert.ToInt32(TopPosition);
                    oBoltAppSettingsWindowsPosition.SETTINGS.WNDPOSITION.Right = Convert.ToInt32(Width);
                    oBoltAppSettingsWindowsPosition.SETTINGS.WNDPOSITION.Down = Convert.ToInt32(Height);
                }
                //CommonFrontEnd.SharedMemories.SaveConfiguration.SaveUserConfiguration(@"D:\TWS_DotNetNewStructure\TWS_DOTNETT\CommonFrontEnd\bin\Debug\xml\Users\AppSettings\920200000.xml", "WindowsPosition");
                SaveConfiguration.SaveUserConfiguration(SettingsManager.AppSettingsXmlPath, "WindowsPosition");
            }
        }


        //private RelayCommand _boltSettings;

        //public RelayCommand BoltSettings {
        //    get
        //    {
        //        return _boltSettings ?? (_boltSettings = new RelayCommand(
        //                (object e) => BoltSettingsClick()
        //                    ));
        //    } 
        //}

        //public void BoltSettingsClick()
        //{
        //    BoltSettings boltSettings = new BoltSettings();
        //    boltSettings.Show();
        //}


        private void EscapeUsingUserControl(object e)
        {
            ProfileSettings mainwindow = e as ProfileSettings;
            mainwindow.Hide();
        }

#endif
    }



    public partial class SettingsVM
    {
#if BOW

#endif
    }
}
