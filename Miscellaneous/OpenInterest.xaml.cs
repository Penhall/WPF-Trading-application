﻿using CommonFrontEnd.ViewModel;
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

namespace CommonFrontEnd.View
{
    /// <summary>
    /// Interaction logic for OpenInterest.xaml
    /// </summary>
    public partial class OpenInterest : Window
    {
        public OpenInterest()
        {
            InitializeComponent();
            this.DataContext = new OpenInterestVM();
        }
    }
}