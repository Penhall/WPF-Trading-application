using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace CommonFrontEnd.Common
{
    public class TitleBarHelperClass : Window
    {
        private const uint WM_SYSCOMMAND = 0x112;

        private const uint WM_INITMENUPOPUP = 0x0117;

        private const uint MF_SEPARATOR = 0x800;

        private const uint MF_BYCOMMAND = 0x0;

        private const uint MF_BYPOSITION = 0x400;

        private const uint MF_STRING = 0x0;

        private const uint MF_ENABLED = 0x0;

        private const uint MF_DISABLED = 0x2;

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool InsertMenu(IntPtr hmenu, int position, uint flags, uint item_id, [MarshalAs(UnmanagedType.LPTStr)]string item_text);

        [DllImport("user32.dll")]
        private static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        public static readonly DependencyProperty MenuItemsProperty = DependencyProperty.Register(
            "MenuItems", typeof(FreezableCollection<SystemMenuItem>), typeof(TitleBarHelperClass), new PropertyMetadata(new PropertyChangedCallback(OnMenuItemsChanged)));

        private IntPtr systemMenu;

        public FreezableCollection<SystemMenuItem> MenuItems
        {
            get
            {
                return (FreezableCollection<SystemMenuItem>)this.GetValue(MenuItemsProperty);
            }

            set
            {
                this.SetValue(MenuItemsProperty, value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the SystemMenuWindow class.
        /// </summary>
        public TitleBarHelperClass()
        {
            this.Loaded += this.SystemMenuWindow_Loaded;

            this.MenuItems = new FreezableCollection<SystemMenuItem>();
        }

        private static void OnMenuItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TitleBarHelperClass obj = d as TitleBarHelperClass;

            if (obj != null)
            {
                if (e.NewValue != null)
                {
                    obj.MenuItems = e.NewValue as FreezableCollection<SystemMenuItem>;
                }
            }
        }

        private void SystemMenuWindow_Loaded(object sender, RoutedEventArgs e)
        {
            WindowInteropHelper interopHelper = new WindowInteropHelper(this);
            this.systemMenu = GetSystemMenu(interopHelper.Handle, false);

            if (this.MenuItems.Count > 0)
            {
                InsertMenu(this.systemMenu, -1, MF_BYPOSITION | MF_SEPARATOR, 0, String.Empty);
            }

            foreach (SystemMenuItem item in this.MenuItems)
            {
                InsertMenu(this.systemMenu, (int)item.Id, MF_BYCOMMAND | MF_STRING, (uint)item.Id, item.Header);
            }

            HwndSource hwndSource = HwndSource.FromHwnd(interopHelper.Handle);
            hwndSource.AddHook(this.WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch ((uint)msg)
            {
                case WM_SYSCOMMAND:
                    var menuItem = this.MenuItems.Where(mi => mi.Id == wParam.ToInt32()).FirstOrDefault();
                    if (menuItem != null)
                    {
                        menuItem.Command.Execute(menuItem.Command);
                        handled = true;
                    }

                    break;

                case WM_INITMENUPOPUP:
                    if (this.systemMenu == wParam)
                    {
                        foreach (SystemMenuItem item in this.MenuItems)
                        {
                            EnableMenuItem(this.systemMenu, (uint)item.Id, MF_ENABLED);
                        }
                        handled = true;
                    }

                    break;
            }

            return IntPtr.Zero;
        }
    }

    public class SystemMenuItem : Freezable
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command", typeof(ICommand), typeof(SystemMenuItem), new PropertyMetadata(new PropertyChangedCallback(OnCommandChanged)));

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof(string), typeof(SystemMenuItem));

        public static readonly DependencyProperty IdProperty = DependencyProperty.Register(
            "Id", typeof(int), typeof(SystemMenuItem));

        public ICommand Command
        {
            get
            {
                return (ICommand)this.GetValue(CommandProperty);
            }

            set
            {
                this.SetValue(CommandProperty, value);
            }
        }
        public string Header
        {
            get
            {
                return (string)GetValue(HeaderProperty);
            }

            set
            {
                SetValue(HeaderProperty, value);
            }
        }

        public int Id
        {
            get
            {
                return (int)GetValue(IdProperty);
            }

            set
            {
                SetValue(IdProperty, value);
            }
        }
        protected override Freezable CreateInstanceCore()
        {
            return new SystemMenuItem();
        }

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SystemMenuItem systemMenuItem = d as SystemMenuItem;

            if (systemMenuItem != null)
            {
                if (e.NewValue != null)
                {
                    systemMenuItem.Command = e.NewValue as ICommand;
                }
            }
        }
    }
}
