using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommonFrontEnd.ViewModel.BSEBulletin;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CommonFrontEnd.Common;

namespace CommonFrontEnd.View.BSEBulletin
{
    /// <summary>
    /// Interaction logic for BSEBulletinsBoard.xaml
    /// </summary>
    public partial class BSEBulletinsBoard : Window
    {
        public BSEBulletinsBoard()
        {
            InitializeComponent();
            this.DataContext = new ViewModel.BSEBulletin.BSEBulletinsBoardVM();
            this.Owner = Application.Current.MainWindow;
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

    }
}
