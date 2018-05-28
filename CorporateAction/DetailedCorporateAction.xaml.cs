using CommonFrontEnd.ViewModel.CorporateAction;
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

namespace CommonFrontEnd.View.CorporateAction
{
    /// <summary>
    /// Interaction logic for DetailedCorporateAction.xaml
    /// </summary>
    public partial class DetailedCorporateAction : Window
    {
        public DetailedCorporateAction()
        {
            InitializeComponent();
            this.DataContext = new DetailedCorporateActionVM();
        }
    }
}
