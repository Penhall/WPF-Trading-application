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
using CommonFrontEnd.ViewModel.PersonalDownload;
using CommonFrontEnd.Common;

namespace CommonFrontEnd.View.PersonalDownload
{
    /// <summary>
    /// Interaction logic for PersonalDownload.xaml
    /// </summary>
    public partial class PersonalDownload : Window
    {
        public PersonalDownload()
        {
            InitializeComponent();
            this.DataContext = PersonalDownloadVM.GetInstance;
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
