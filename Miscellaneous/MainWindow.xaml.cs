using CommonFrontEnd.Common;
using CommonFrontEnd.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CommonFrontEnd.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : TitleBarHelperClass
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = MainWindowVM.GetInstance;

            #region BackGround
            if (!Directory.Exists(Environment.CurrentDirectory + "/Images/"))
                Directory.CreateDirectory(Environment.CurrentDirectory + "/Images/");

            string[] fileArray = Directory.GetFiles(Environment.CurrentDirectory + "/Images/");
            Random rand = new Random();
            if (fileArray.Length > 0)
            {
                try
                {
                    Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), fileArray[rand.Next(0, fileArray.Length)])));
                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogError(ex);
                }

            }
            #endregion
        }
    }
}
