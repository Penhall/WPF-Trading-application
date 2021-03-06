﻿using CommonFrontEnd.ViewModel.Settings;
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

namespace CommonFrontEnd.View.Settings
{
    /// <summary>
    /// Interaction logic for BoltSettings.xaml
    /// </summary>
    public partial class BoltSettings : UserControl
    {
        public BoltSettings()
        {
            InitializeComponent();
#if TWS
            this.DataContext = new BoltSettingsVM();
#endif
        }
    }
}
