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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CommonFrontEnd.View;
using CommonFrontEnd.Common;

namespace CommonFrontEnd.View
{
    /// <summary>
    /// Interaction logic for scripNameSearch.xaml
    /// </summary>
    public partial class scripNameSearch : TitleBarHelperClass
    {
        public scripNameSearch()
        {
            InitializeComponent();
            this.DataContext = new ScripNameSearchVM();
            this.Owner = Application.Current.MainWindow;
            
        }

       
    }
}
