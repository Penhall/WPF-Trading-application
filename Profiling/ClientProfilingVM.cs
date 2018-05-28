using CommonFrontEnd.Common;
using CommonFrontEnd.Model.Profiling;
using CommonFrontEnd.SharedMemories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using System.Windows;
using static CommonFrontEnd.SharedMemories.MasterSharedMemory;
using System.Linq;
using System.IO;
using CommonFrontEnd.View.Settings;
using System.Windows.Controls;
using CommonFrontEnd.View.Profiling;
using static CommonFrontEnd.SharedMemories.DataAccessLayer;
using System.Windows.Forms;

namespace CommonFrontEnd.ViewModel.Profiling

{
    class ClientProfilingVM : BaseViewModel
    {

        #region Properties
        public static Action OnClientProfilingChange;
        DirectoryInfo CsvFilesPath = new DirectoryInfo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"User\")));
        public string winName = String.Empty;
        public string str = string.Empty;
        public DataAccessLayer oDataAccessLayer;

        private ObservableCollection<ClientProfilingModel> _ObjClientCollection = new ObservableCollection<ClientProfilingModel>();

        public ObservableCollection<ClientProfilingModel> ObjClientCollection
        {
            get { return _ObjClientCollection; }
            set { _ObjClientCollection = value; /*NotifyStaticPropertyChanged(nameof(ObjIndicesCollection));*/ }
        }

        private List<ClientProfilingModel> _ViewSuccessCollection;
        public List<ClientProfilingModel> ViewSuccessCollection
        {
            get { return _ViewSuccessCollection; }
            set
            {
                _ViewSuccessCollection = value;
                NotifyPropertyChanged("ViewSuccessCollection");
            }
        }

        //public bool IsSelected { get; set; }
        private string _txtClientSearch;
        public string txtClientSearch
        {
            get { return _txtClientSearch; }
            set
            {
                _txtClientSearch = value;
                NotifyPropertyChanged("txtClientSearch");
            }
        }

        private string _SerialNo;
        public string SerialNo
        {
            get { return _SerialNo; }
            set
            {
                _SerialNo = value;
                NotifyPropertyChanged("SerialNo");
            }
        }

        private string _Replytxt;
        public string Replytxt
        {
            get { return _Replytxt; }
            set
            {
                _Replytxt = value;
                NotifyPropertyChanged("Replytxt");
            }
        }

        private string _ClientSearch;
        public string ClientSearch
        {
            get { return _ClientSearch; }
            set
            {
                _ClientSearch = value;
                NotifyPropertyChanged("ClientSearch");
            }
        }

        private string _Go;
        public string Go
        {
            get { return _Go; }
            set
            {
                _Go = value;
                NotifyPropertyChanged("Go");
            }
        }

        private string _ShortClientIdtxt;
        public string ShortClientIdtxt
        {
            get { return _ShortClientIdtxt; }
            set
            {
                _ShortClientIdtxt = value;
                NotifyPropertyChanged("ShortClientIdtxt");
            }
        }
        private string _ShortClientIdtxtCopy;
        public string ShortClientIdtxtCopy
        {
            get { return _ShortClientIdtxtCopy; }
            set
            {
                _ShortClientIdtxtCopy = value;
                NotifyPropertyChanged("ShortClientIdtxtCopy");
            }
        }

        private string _CompleteClientIdtxt;
        public string CompleteClientIdtxt
        {
            get { return _CompleteClientIdtxt; }
            set
            {
                _CompleteClientIdtxt = value;
                NotifyPropertyChanged("CompleteClientIdtxt");
            }
        }

        private string _CompleteClientIdtxtCopy;
        public string CompleteClientIdtxtCopy
        {
            get { return _CompleteClientIdtxtCopy; }
            set
            {
                _CompleteClientIdtxtCopy = value;
                NotifyPropertyChanged("CompleteClientIdtxtCopy");
            }
        }

        private string _FirstNametxt;
        public string FirstNametxt
        {
            get { return _FirstNametxt; }
            set
            {
                _FirstNametxt = value;
                NotifyPropertyChanged("FirstNametxt");
            }
        }

        private string _FirstNameCopytxt;
        public string FirstNameCopytxt
        {
            get { return _FirstNameCopytxt; }
            set
            {
                _FirstNameCopytxt = value;
                NotifyPropertyChanged("FirstNameCopytxt");
            }
        }

        private string _LastNametxt;
        public string LastNametxt
        {
            get { return _LastNametxt; }
            set
            {
                _LastNametxt = value;
                NotifyPropertyChanged("LastNametxt");
            }
        }

        private string _LastNameCopytxt;
        public string LastNameCopytxt
        {
            get { return _LastNameCopytxt; }
            set
            {
                _LastNameCopytxt = value;
                NotifyPropertyChanged("LastNameCopytxt");
            }
        }

        private string _MobileNumbertxt;
        public string MobileNumbertxt
        {
            get { return _MobileNumbertxt; }
            set
            {
                _MobileNumbertxt = value;
                NotifyPropertyChanged("MobileNumbertxt");
            }
        }

        private string _MobileNumberCopytxt;
        public string MobileNumberCopytxt
        {
            get { return _MobileNumberCopytxt; }
            set
            {
                _MobileNumberCopytxt = value;
                NotifyPropertyChanged("MobileNumberCopytxt");
            }
        }

        private string _EmailIDtxt;
        public string EmailIDtxt
        {
            get { return _EmailIDtxt; }
            set
            {
                _EmailIDtxt = value;
                NotifyPropertyChanged("EmailIDtxt");
            }
        }

        private string _EmailIDCopytxt;
        public string EmailIDCopytxt
        {
            get { return _EmailIDCopytxt; }
            set
            {
                _EmailIDCopytxt = value;
                NotifyPropertyChanged("EmailIDCopytxt");
            }
        }

        private string _CPCodeDerivativetxt;
        public string CPCodeDerivativetxt
        {
            get { return _CPCodeDerivativetxt; }
            set
            {
                _CPCodeDerivativetxt = value;
                NotifyPropertyChanged("CPCodeDerivativetxt");
            }
        }

        private bool _CPCodeDerivativetxtEnable;
        public bool CPCodeDerivativetxtEnable
        {
            get { return _CPCodeDerivativetxtEnable; }
            set
            {
                _CPCodeDerivativetxtEnable = value;
                NotifyPropertyChanged("CPCodeDerivativetxtEnable");
            }
        }

        private string _CPCodeDerivativeNametxt;
        public string CPCodeDerivativeNametxt
        {
            get { return _CPCodeDerivativeNametxt; }
            set
            {
                _CPCodeDerivativeNametxt = value;
                NotifyPropertyChanged("CPCodeDerivativeNametxt");
                // OnChangeofCPCodeDerivative();
            }
        }

        private string _CPCodeDerivative;

        public string CPCodeDerivative
        {
            get { return _CPCodeDerivative; }
            set { _CPCodeDerivative = value; NotifyPropertyChanged("CPCodeDerivative"); }
        }

        private string _CPCodeCurrency;

        public string CPCodeCurrency
        {
            get { return _CPCodeCurrency; }
            set { _CPCodeCurrency = value; NotifyPropertyChanged("CPCodeCurrency"); }
        }

        private bool _CPCodeCurrencytxtEnable;
        public bool CPCodeCurrencytxtEnable
        {
            get { return _CPCodeCurrencytxtEnable; }
            set
            {
                _CPCodeCurrencytxtEnable = value;
                NotifyPropertyChanged("CPCodeCurrencytxtEnable");
            }
        }

        private string _CPCodeCurrencyNametxt;
        public string CPCodeCurrencyNametxt
        {
            get { return _CPCodeCurrencyNametxt; }
            set
            {
                _CPCodeCurrencyNametxt = value;
                NotifyPropertyChanged("CPCodeCurrencyNametxt");
                //OnChangeofCPCodeCurrency();
            }
        }

        private List<string> _ClientType1;
        public List<string> ClientType1
        {
            get { return _ClientType1; }
            set { _ClientType1 = value; NotifyPropertyChanged("ClientType1"); }
        }

        private string _SelectedClientType;

        public string SelectedClientType
        {
            get { return _SelectedClientType; }
            set { _SelectedClientType = value; NotifyPropertyChanged("SelectedClientType"); }
        }

        private string _SelectedClientCopyType;

        public string SelectedClientCopyType
        {
            get { return _SelectedClientCopyType; }
            set { _SelectedClientCopyType = value; NotifyPropertyChanged("SelectedClientCopyType"); }
        }

        private string _CPCodeCurrencytxt;
        public string CPCodeCurrencytxt
        {
            get { return _CPCodeCurrencytxt; }
            set
            {
                _CPCodeCurrencytxt = value;
                NotifyPropertyChanged("CPCodeCurrencytxt");
            }
        }

        private bool _FirstNameOEChecked;

        public bool FirstNameOEChecked
        {
            get { return _FirstNameOEChecked; }
            set
            {
                _FirstNameOEChecked = value;
                NotifyPropertyChanged("FirstNameOEChecked");

            }
        }

        private bool _FirstNameOEEnabled;

        public bool FirstNameOEEnabled
        {
            get { return _FirstNameOEEnabled; }
            set
            {
                _FirstNameOEEnabled = value;
                NotifyPropertyChanged("FirstNameOEEnabled");



            }
        }

        private bool _LastNameOEChecked;

        public bool LastNameOEChecked
        {
            get { return _LastNameOEChecked; }
            set
            {
                _LastNameOEChecked = value;
                NotifyPropertyChanged("LastNameOEChecked");


            }
        }

        private bool _LastNameOEEnabled;

        public bool LastNameOEEnabled
        {
            get { return _LastNameOEEnabled; }
            set
            {
                _LastNameOEEnabled = value;
                NotifyPropertyChanged("LastNameOEEnabled");

            }
        }

        private bool _MobileNumberOEChecked;

        public bool MobileNumberOEChecked
        {
            get { return _MobileNumberOEChecked; }
            set
            {
                _MobileNumberOEChecked = value;
                NotifyPropertyChanged("MobileNumberOEChecked");

            }
        }

        private string _MobileNumberOEEnabled;

        public string MobileNumberOEEnabled
        {
            get { return _MobileNumberOEEnabled; }
            set
            {
                _MobileNumberOEEnabled = value;
                NotifyPropertyChanged("MobileNumberOEEnabled");

            }
        }

        private bool _EmailIDOEChecked;

        public bool EmailIDOEChecked
        {
            get { return _EmailIDOEChecked; }
            set
            {
                _EmailIDOEChecked = value;
                NotifyPropertyChanged("EmailIDOEChecked");

            }
        }

        private string _EmailIDOEEnabled;

        public string EmailIDOEEnabled
        {
            get { return _EmailIDOEEnabled; }
            set
            {
                _EmailIDOEEnabled = value;
                NotifyPropertyChanged("EmailIDOEEnabled");

            }
        }

        private bool _CPCodeDerivativeChecked;

        public bool CPCodeDerivativeChecked
        {
            get { return _CPCodeDerivativeChecked; }
            set
            {
                _CPCodeDerivativeChecked = value;
                NotifyPropertyChanged("CPCodeDerivativeChecked");
                ControlEnability();
            }
        }



        private string _CPCodeDerivativeEnabled;

        public string CPCodeDerivativeEnabled
        {
            get { return _CPCodeDerivativeEnabled; }
            set
            {
                _CPCodeDerivativeEnabled = value;
                NotifyPropertyChanged("CPCodeDerivativeEnabled");

            }
        }

        private bool _CPCodeCurrencyChecked;

        public bool CPCodeCurrencyChecked
        {
            get { return _CPCodeCurrencyChecked; }
            set
            {
                _CPCodeCurrencyChecked = value;
                NotifyPropertyChanged("CPCodeCurrencyChecked");
                CurrencyControlEnability();
            }
        }



        private string _CPCodeCurrencyEnabled;

        public string CPCodeCurrencyEnabled
        {
            get { return _CPCodeCurrencyEnabled; }
            set
            {
                _CPCodeCurrencyEnabled = value;
                NotifyPropertyChanged("CPCodeCurrencyEnabled");

            }
        }


        private ClientProfilingModel _SelectedClient;

        public ClientProfilingModel SelectedClient
        {
            get { return _SelectedClient; }
            set
            {
                _SelectedClient = value;
                NotifyPropertyChanged("SelectedClient");

            }
        }

        private string _SelectedClientType1;
        public string SelectedClientType1
        {
            get { return _SelectedClientType1; }
            set
            {
                _SelectedClientType1 = value;
                NotifyPropertyChanged("SelectedClientType1");
                ControlsUpdation();
                //CPCode();
            }
        }



        private string _btnAddEnable;

        public string btnAddEnable
        {
            get { return _btnAddEnable; }
            set
            {
                _btnAddEnable = value;
                NotifyPropertyChanged("btnAddEnable");
            }

        }

        private string _btnEditEnable;

        public string btnEditEnable
        {
            get { return _btnEditEnable; }
            set
            {
                _btnEditEnable = value;
                NotifyPropertyChanged("btnEditEnable");
            }

        }

        private string _btnDeleteEnable;

        public string btnDeleteEnable
        {
            get { return _btnDeleteEnable; }
            set
            {
                _btnDeleteEnable = value;
                NotifyPropertyChanged("btnDeleteEnable");
            }

        }

        private string _btnDeleteAllEnable;

        public string btnDeleteAllEnable
        {
            get { return _btnDeleteAllEnable; }
            set
            {
                _btnDeleteAllEnable = value;
                NotifyPropertyChanged("btnDeleteAllEnable");
            }

        }

        private string _btnSaveEnable;

        public string btnSaveEnable
        {
            get { return _btnSaveEnable; }
            set
            {
                _btnSaveEnable = value;
                NotifyPropertyChanged("btnSaveEnable");
            }

        }

        private string _btnCancelEnable;

        public string btnCancelEnable
        {
            get { return _btnCancelEnable; }
            set
            {
                _btnCancelEnable = value;
                NotifyPropertyChanged("btnCancelEnable");
            }

        }

        private string _btnGoEnable;

        public string btnGoEnable
        {
            get { return _btnGoEnable; }
            set
            {
                _btnGoEnable = value;
                NotifyPropertyChanged("btnGoEnable");
            }

        }




        //private string _txtReplyBox;

        //public string txtReplyBox
        //{
        //    get { return _txtReplyBox; }
        //    set
        //    {
        //        _txtReplyBox = value;
        //        NotifyPropertyChanged("txtReplyBox");
        //    }

        //}

        //public string Validate_Message { get; private set; }
        #endregion

        #region RelayCommands

        private RelayCommand _btnGo_Click;

        public RelayCommand btnGo_Click
        {
            get
            {
                return _btnGo_Click ?? (_btnGo_Click = new RelayCommand(
                    (object e) => FilterAndPopulate(e)
                        ));
            }
        }


        private RelayCommand _SelectionChanged;

        public RelayCommand SelectionChanged
        {
            get
            {
                return _SelectionChanged ?? (_SelectionChanged = new RelayCommand(
                    (object e) => UpdateDataGrid(e)
                        ));
            }
        }

        private RelayCommand _btnSaveinCSV;

        public RelayCommand btnSaveinCSV
        {
            get
            {
                return _btnSaveinCSV ?? (_btnSaveinCSV = new RelayCommand(
                    (object e) => SaveinCSV(e)
                    ));
            }
        }

        //private RelayCommand _BtnLoadFile;
        //public RelayCommand BtnLoadFile
        //{
        //    get
        //    {
        //        return _BtnLoadFile ?? (_BtnLoadFile = new RelayCommand(
        //            (object e1) => OnClickOfLoadButton()));
        //    }
        //}


        private RelayCommand _txtClientSearch_TextChanged;

        public RelayCommand txtClientSearch_TextChanged
        {
            get
            {
                return _txtClientSearch_TextChanged ?? (_txtClientSearch_TextChanged = new RelayCommand((object e) => txtClientSearch_TextChangedevent(e)));
            }

        }

        private RelayCommand _ShortClientIdtxt_TextChanged;

        public RelayCommand ShortClientIdtxt_TextChanged
        {
            get
            {
                return _ShortClientIdtxt_TextChanged ?? (_ShortClientIdtxt_TextChanged = new RelayCommand((object e) => ShortClientIdtxt_TextChangedevent(e)));
            }

        }

        private RelayCommand _CompleteClientIdtxt_TextChanged;

        public RelayCommand CompleteClientIdtxt_TextChanged
        {
            get
            {
                return _CompleteClientIdtxt_TextChanged ?? (_CompleteClientIdtxt_TextChanged = new RelayCommand((object e) => CompleteClientIdtxt_TextChangedevent(e)));
            }

        }

        private RelayCommand _FirstNametxt_TextChanged;

        public RelayCommand FirstNametxt_TextChanged
        {
            get
            {
                return _FirstNametxt_TextChanged ?? (_FirstNametxt_TextChanged = new RelayCommand((object e) => FirstNametxt_TextChangedevent(e)));
            }

        }

        private RelayCommand _LastNametxt_TextChanged;

        public RelayCommand LastNametxt_TextChanged
        {
            get
            {
                return _LastNametxt_TextChanged ?? (_LastNametxt_TextChanged = new RelayCommand((object e) => LastNametxt_TextChangedevent(e)));
            }

        }

        private RelayCommand _MobileNumbertxt_TextChanged;

        public RelayCommand MobileNumbertxt_TextChanged
        {
            get
            {
                return _MobileNumbertxt_TextChanged ?? (_MobileNumbertxt_TextChanged = new RelayCommand((object e) => MobileNumbertxt_TextChangedevent(e)));
            }

        }

        private RelayCommand _EmailIDtxt_TextChanged;

        public RelayCommand EmailIDtxt_TextChanged
        {

            get
            {
                return _EmailIDtxt_TextChanged ?? (_EmailIDtxt_TextChanged = new RelayCommand((object e) => EmailIDtxt_TextChangedevent(e)));
            }

        }

        private RelayCommand _CPCodeDerivativetxt_TextChanged;

        public RelayCommand CPCodeDerivativetxt_TextChanged
        {
            get
            {
                return _CPCodeDerivativetxt_TextChanged ?? (_CPCodeDerivativetxt_TextChanged = new RelayCommand((object e) => CPCodeDerivativetxt_TextChangedevent(e)));
            }

        }

        private RelayCommand _CPCodeDerivativeNametxt_TextChanged;

        public RelayCommand CPCodeDerivativeNametxt_TextChanged
        {
            get
            {
                return _CPCodeDerivativeNametxt_TextChanged ?? (_CPCodeDerivativeNametxt_TextChanged = new RelayCommand((object e) => CPCodeDerivativeNametxt_TextChangedevent(e)));
            }

        }

        private RelayCommand _CPCodeCurrencyNametxt_TextChanged;

        public RelayCommand CPCodeCurrencyNametxt_TextChanged
        {
            get
            {
                return _CPCodeCurrencyNametxt_TextChanged ?? (_CPCodeCurrencyNametxt_TextChanged = new RelayCommand((object e) => CPCodeCurrencyNametxt_TextChangedevent(e)));
            }

        }

        private RelayCommand _CPCodeCurrencytxt_TextChanged;

        public RelayCommand CPCodeCurrencytxt_TextChanged
        {
            get
            {
                return _CPCodeCurrencytxt_TextChanged ?? (_CPCodeCurrencytxt_TextChanged = new RelayCommand((object e) => CPCodeCurrencytxt_TextChangedevent(e)));
            }

        }

        private RelayCommand _Replytxt_TextChanged;

        public RelayCommand Replytxt_TextChanged
        {
            get
            {
                return _Replytxt_TextChanged ?? (_Replytxt_TextChanged = new RelayCommand((object e) => Replytxt_TextChangedevent(e)));
            }

        }

        private RelayCommand _btnAdd_Click;

        public RelayCommand btnAdd_Click
        {
            get
            {
                return _btnAdd_Click ?? (_btnAdd_Click = new RelayCommand(
                    (object e) => AddChanges(e)));

            }

        }

        //private RelayCommand _btnSaveAs_Click;

        //public RelayCommand btnSaveAs_Click
        //{
        //    get
        //    {
        //        return _btnSaveAs_Click ?? (_btnSaveAs_Click = new RelayCommand(
        //            (object e) => SaveAsChanges(e)));

        //    }

        //}

        //private RelayCommand _btnApply_Click;

        //public RelayCommand btnApply_Click
        //{
        //    get
        //    {
        //        return _btnApply_Click ?? (_btnApply_Click = new RelayCommand(
        //            (object e) => ApplyChanges(e)));

        //    }

        //}

        private RelayCommand _btnEdit_Click;

        public RelayCommand btnEdit_Click
        {
            get
            {
                return _btnEdit_Click ?? (_btnEdit_Click = new RelayCommand(
                    (object e) => EditChanges(e)));

            }

        }

        private RelayCommand _btnDelete_Click;

        public RelayCommand btnDelete_Click
        {
            get
            {
                return _btnDelete_Click ?? (_btnDelete_Click = new RelayCommand(
                    (object e) => DeleteChanges(e)));

            }

        }

        private RelayCommand _btnDeleteAll_Click;

        public RelayCommand btnDeleteAll_Click
        {
            get
            {
                return _btnDeleteAll_Click ?? (_btnDeleteAll_Click = new RelayCommand(
                    (object e) => DeleteAllChanges(e)));

            }

        }

        public string CPCodeDerivativeEnable { get; private set; }
        public string ClientsCount { get; private set; }

        #endregion

        #region VALIDATION

        private void txtClientSearch_TextChangedevent(object e)
        {

        }

        private void ShortClientIdtxt_TextChangedevent(object e)
        {

        }

        private void CompleteClientIdtxt_TextChangedevent(object e)
        {

        }

        private void FirstNametxt_TextChangedevent(object e)
        {

        }

        private void LastNametxt_TextChangedevent(object e)
        {

        }

        private void MobileNumbertxt_TextChangedevent(object e)
        {
            if (MobileNumbertxt != null)
            {
                MobileNumbertxt = Regex.Replace(MobileNumbertxt, "[^0-9]+", "");
            }
        }

        private void EmailIDtxt_TextChangedevent(object e)
        {

            //Condition for Checking duplicate Short Client ID

            if (EmailIDtxt != null)
            {

            }

        }

        private void CPCodeDerivativetxt_TextChangedevent(object e)
        {
            if (CPCodeDerivativetxt != null)
            {
                System.Text.RegularExpressions.Regex.Replace(CPCodeDerivativetxt, "<test>.*<test>", string.Empty, RegexOptions.Singleline);
                string cpCode = CPCodeDerivativetxt;
                if (objCPCodeDerivativeDict.ContainsKey(cpCode)) {
                    CPCodeDerivativeNametxt = objCPCodeDerivativeDict[cpCode].ParticipantName;
                    Replytxt = string.Empty;
                }
                else if (string.IsNullOrEmpty(CPCodeDerivativetxt))
                {
                    Replytxt = "";
                }
                else {
                    CPCodeDerivativeNametxt = string.Empty;
                    Replytxt = "Entered CP Code Derivative not present in the CP Code Derivative Master";
                }

            }
        }
        private void CPCodeDerivativeNametxt_TextChangedevent(object e)
        {
            CPCodeDerivativeNametxt = CPCodeDerivativeNametxt.Trim();
        }

        private void CPCodeCurrencyNametxt_TextChangedevent(object e)
        {

        }

        private void CPCodeCurrencytxt_TextChangedevent(object e)
        {
            if (CPCodeCurrencytxt != null)
            {
                System.Text.RegularExpressions.Regex.Replace(CPCodeCurrencytxt, "<test>.*<test>", string.Empty, RegexOptions.Singleline);
                string cpCode = CPCodeCurrencytxt;
                if (objCPCodeCurrencyDict.ContainsKey(cpCode)) {
                    CPCodeCurrencyNametxt = objCPCodeCurrencyDict[cpCode].ParticipantName;
                    Replytxt = string.Empty;
                }
                else if (string.IsNullOrEmpty(CPCodeCurrencytxt))
                {
                    Replytxt = "";
                }
                else { CPCodeCurrencyNametxt = string.Empty;
                    Replytxt = "Entered CP Code Currency not present in the CP Code Currency Master";
                }
            }
        }
        private void Replytxt_TextChangedevent(object e)
        {

        }

        #endregion

        #region Constructor

        public ClientProfilingVM()
        {
            oDataAccessLayer = new DataAccessLayer();
            oDataAccessLayer.Connect((int)DataAccessLayer.ConnectionDB.Masters);
            //txtClientSearch..Focus();
            PopulateGrid();
            PopulateClientType();
            // ObjClientCollection = new ObservableCollection<ClientProfilingModel>();
            // ViewSuccessCollection = new List<ClientProfilingModel>();

            //CPCode();
            // VisibilityCPCodeDerivativetxt = 
            // ControlVisibility();
        }

        #endregion

        #region Methods


        private void SaveinCSV(object e)
        {
            if (ObjClientCollection == null)
            {
                Replytxt = "No clients are present in Datagrid";
            }
            else if(ObjClientCollection != null)
            {
                try
                {
                    //txtReply = string.Empty;
                    SaveFileDialog objSaveinCSV = new SaveFileDialog();
                    objSaveinCSV.InitialDirectory = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"User/"))));
                    if (!Directory.Exists(objSaveinCSV.InitialDirectory))
                        Directory.CreateDirectory(objSaveinCSV.InitialDirectory);

                    //objFileDialogBatchResub.Title = "Browse CSV Files";
                    objSaveinCSV.DefaultExt = "csv";
                    string Filter = "CSV files (*.csv)|*.csv";
                    objSaveinCSV.Filter = Filter;


                    const string header = "ShortClientId, CompleteClientId, FirstName, LastName, ClientType, ClientStatus, MobileNum, Email,  CPCodeDerivative, CPCodeCurrency";
                    StreamWriter writer = null;

                    //objFileDialogBatchResub.ShowDialog();
                    if (objSaveinCSV.ShowDialog() == DialogResult.OK)
                    {
                        Filter = objSaveinCSV.FileName;

                        writer = new StreamWriter(Filter, false, System.Text.Encoding.UTF8);

                        writer.WriteLine(header);

                        foreach (var item in ObjClientCollection)
                        {
                            // string Rate;

                            // Rate = Convert.ToString(Convert.ToDouble(item.rate) * Math.Pow(10, DecimalPoint));
                            // if (seg == Segment.Equity.ToString())
                            writer.WriteLine($"{item.ShortClientId}, {item.CompleteClientId}, {item.FirstName}, {item.LastName}, {item.ClientType}, {item.ClientStatus}, {item.MobileNumber}, {item.EmailID},{item.CPCodeDerivative},{item.CPCodeCurrency}");

                        }
                    }

                    writer.Close();
                    Replytxt = "Client(s) stored in file:/bin/user";
                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogError(ex);
                }
            }
         
           
        }

        //private void OnClickOfLoadButton()
        //{
        //    //BatchOrderCollection = new ObservableCollection<OrderModel>();
        //    // ViewSuccessCollection = new List<OrderModel>();
        //    string fileName;
        //    OpenFileDialog openFileDialog1 = new OpenFileDialog();

        //    string StartupPath = CsvFilesPath.ToString();
        //    openFileDialog1.InitialDirectory = Path.Combine(Path.GetDirectoryName(StartupPath));

        //    openFileDialog1.Title = "Browse CSV Files";

        //    openFileDialog1.DefaultExt = "csv";
        //    openFileDialog1.FilterIndex = 1;
        //    openFileDialog1.CheckFileExists = true;
        //    // openFileDialog1.Filter = "CSV Files(*.CSV) | *.CSV | DOT(*.txt) | *.txt ";//"CSV files (*.csv)|*.csv";
        //    openFileDialog1.Filter = "CSV files (*.csv)|*.csv";

        //    if (openFileDialog1.ShowDialog() == DialogResult.OK)
        //    {
        //        ObjClientCollection = new ObservableCollection<ClientProfilingModel>();
        //        ViewSuccessCollection = new List<ClientProfilingModel>();
        //        fileName = openFileDialog1.FileName;
        //        string[] strScrips = File.ReadAllLines(fileName);

        //      //  ViewSuccessCollection = CommonFunctions.ReadCSVFile(f).ToList();
        //        int i = 1;
        //        int segment;
        //        foreach (var item in ViewSuccessCollection)
        //        {


        //            int cnt = 0, z = 0, error = 0;
        //           // string Segment_Name = string.Empty;
        //            ClientProfilingModel csv = new ClientProfilingModel();
        //            csv.ShortClientId = item.ShortClientId.ToString();
        //            csv.CompleteClientId = item.CompleteClientId;
        //            csv.FirstName = item.FirstName;
        //            csv.LastName = item.LastName;
        //            csv.ClientType = item.ClientType;
        //            csv.ClientStatus = item.ClientStatus;
        //            csv.MobileNumber = item.MobileNumber;
        //            csv.EmailID = item.EmailID;


        //    bool result = int.TryParse(item.CompleteClientId, out z);
        //    if (result == false)
        //    {
        //        csv.CompleteClientId = CommonFunctions.GetScripCodeFromScripID(item.ScripCodeID);
        //        Segment_Name = CommonFunctions.GetSegmentID(csv.ScripCode);
        //        csv.Symbol = item.ScripCodeID.Trim();
        //    }
        //    else
        //    {
        //        csv.Symbol = CommonFunctions.GetScripId(Convert.ToInt64(item.ScripCodeID), "BSE");
        //        csv.ScripCode = Convert.ToInt64(item.ScripCodeID);
        //        Segment_Name = CommonFunctions.GetSegmentID(csv.ScripCode);
        //    }
        //    csv.ScripCodeID = csv.Symbol;
        //    // csv.ScripCode = item.ScripCode;
        //    csv.ScreenId = (int)Enumerations.WindowName.Batch_Order;
        //    csv.ExecInst = Enumerations.Order.ExecInst.PersistentOrder.ToString();
        //    csv.ParticipantCode = "";
        //    csv.FreeText3 = "fdf";
        //    csv.Filler_c = "fdf";
        //    segment = CommonFunctions.GetSegmentFromScripCode(csv.ScripCode);
        //    string ExchangeSegment = string.Empty;

        //    if (segment == 1)
        //    {
        //        item.Segment = "EQX";
        //        ExchangeSegment = Enumerations.Segment.Equity.ToString();
        //    }
        //    else if (segment == 2)
        //    {
        //        item.Segment = "EDX";
        //        ExchangeSegment = Enumerations.Segment.Derivative.ToString();
        //    }
        //    else if (segment == 3)
        //    {
        //        item.Segment = "CDX";
        //        ExchangeSegment = Enumerations.Segment.Currency.ToString();
        //    }
        //    else
        //    {
        //        System.Windows.Forms.MessageBox.Show("Invalid Scrip Code", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //        error += 1;
        //        // return;
        //    }


        //    if (item.Segment == "EQX") //Equity
        //    {
        //        csv.Segment = Enumerations.Order.ScripSegment.Equity.ToString();
        //        csv.SegmentFlag = CommonFunctions.SegmentFlag(csv.Segment); //(int)Enum.Parse(typeof(Enumerations.Order.ScripSegment), Enumerations.Order.ScripSegment.Equity.ToString());
        //        csv.Symbol = CommonFunctions.GetScripId(csv.ScripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
        //        csv.MarketLot = MasterSharedMemory.objMastertxtDictBaseBSE.Values.Where(x => x.ScripCode == csv.ScripCode).Select(x => x.MarketLot).First();
        //        csv.TickSize = MasterSharedMemory.objMastertxtDictBaseBSE.Values.Where(x => x.ScripCode == csv.ScripCode).Select(x => x.TickSize).First().ToString();
        //        csv.Group = CommonFunctions.GetGroupName(csv.ScripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
        //        //DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(csv.ScripCode), Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
        //        i++;
        //    }
        //    else if (item.Segment == "EDX") //Derivative
        //    {
        //        csv.Segment = Enumerations.Order.ScripSegment.Derivative.ToString();
        //        csv.SegmentFlag = CommonFunctions.SegmentFlag(csv.Segment); //(int)Enum.Parse(typeof(Enumerations.Order.ScripSegment), Enumerations.Order.ScripSegment.Derivative.ToString());
        //        csv.Symbol = CommonFunctions.GetScripId(csv.ScripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Derivative);
        //        try
        //        {
        //            csv.MarketLot = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Where(x => x.ContractTokenNum == csv.ScripCode).Select(x => x.MinimumLotSize).First();
        //            // csv.TickSize = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Where(x => x.ContractTokenNum == csv.ScripCode).Select(x => x.TickSize).First().ToString();
        //            csv.TickSize = MasterSharedMemory.objMasterDerivativeDictBaseBSE.Values.Where(x => x.ContractTokenNum == csv.ScripCode).Select(x => x.TickSize).First().ToString();
        //            // DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(csv.ScripCode), Enumerations.Exchange.BSE, Enumerations.Segment.Derivative);
        //            i++;
        //            //csv.Group = CommonFunctions.GetGroupName(item.ScripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Derivative);
        //        }
        //        catch (Exception ex)
        //        {
        //            System.Windows.MessageBox.Show(i + " position scrip is not of Derivative Segment");
        //            cnt++;
        //            i++;
        //        }
        //    }
        //    else if (item.Segment == "CDX") //Currency
        //    {
        //        csv.Segment = Enumerations.Order.ScripSegment.Currency.ToString();
        //        csv.SegmentFlag = CommonFunctions.SegmentFlag(csv.Segment); //(int)Enum.Parse(typeof(Enumerations.Order.ScripSegment), Enumerations.Order.ScripSegment.Currency.ToString());
        //        csv.Symbol = CommonFunctions.GetScripId(csv.ScripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Currency);
        //        try
        //        {
        //            csv.MarketLot = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Where(x => x.ContractTokenNum == csv.ScripCode).Select(x => x.MinimumLotSize).First();
        //            // csv.TickSize = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Where(x => x.ContractTokenNum == csv.ScripCode).Select(x => x.TickSize).First().ToString();
        //            csv.TickSize = MasterSharedMemory.objMasterCurrencyDictBaseBSE.Values.Where(x => x.ContractTokenNum == csv.ScripCode).Select(x => x.TickSize).First().ToString();
        //            // DecimalPoint = CommonFunctions.GetDecimal(Convert.ToInt32(csv.ScripCode), Enumerations.Exchange.BSE, Enumerations.Segment.Currency);
        //            i++;
        //            //Derivative Group fetch from memory
        //            //csv.Group = CommonFunctions.GetGroupName(item.ScripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Currency);
        //        }
        //        catch (Exception ex)
        //        {
        //            System.Windows.MessageBox.Show(i + " position scrip is not of Currency Segment");
        //            cnt++;
        //            i++;
        //        }
        //    }

        //    #region Fields value not stored yet
        //    //decimalpoint
        //    //Remarks
        //    #endregion


        //    if (DecimalPoint == 2)
        //    {
        //        csv.PriceS = String.Format("{0:0.00}", (item.Price / Math.Pow(10, DecimalPoint))).ToString();
        //        //Convert.ToInt64(Convert.ToDouble(item.Price / Math.Pow(10, DecimalPoint)));
        //        csv.Price = item.Price;
        //        csv.TriggerPriceS = String.Format("{0:0.00}", (item.TriggerPrice / Math.Pow(10, DecimalPoint))).ToString();
        //        csv.TriggerPrice = item.TriggerPrice;
        //    }
        //    else
        //    {
        //        csv.PriceS = String.Format("{0:0.0000}", ((item.Price / Math.Pow(10, DecimalPoint)))).ToString();
        //        //csv.PriceS = Convert.ToInt64(item.Price / Math.Pow(10, DecimalPoint));
        //        csv.Price = item.Price;
        //        csv.TriggerPriceS = String.Format("{0:0.00}", (item.TriggerPrice / Math.Pow(10, DecimalPoint))).ToString();
        //        csv.TriggerPrice = item.TriggerPrice;
        //    }
        //    csv.ClientType = item.ClientType;

        //    if (csv.ClientType == "OWN")
        //        csv.ClientId = "OWN";
        //    else
        //        csv.ClientId = item.ClientId;

        //    csv.OrderType = item.OrderType;
        //    if (item.OrderType == "G")
        //    {
        //        LoadMarketProtection = UtilityOrderDetails.GETInstance.MktProtection == null ? "1.0" : UtilityOrderDetails.GETInstance.MktProtection;
        //        var protectionPercent = Convert.ToInt32(Convert.ToDecimal(LoadMarketProtection) * 100);
        //        csv.ProtectionPercentage = Convert.ToString(protectionPercent);
        //        // csv.ProtectionPercentage = UtilityOrderDetails.GETInstance.MktProtection == null ? "1.0" : UtilityOrderDetails.GETInstance.MktProtection;// Convert.ToString(1);
        //        csv.Price = 0;
        //        csv.PriceS = item.ProtectionPercentage != null ? $"{"M("}{("1.0"):0.00}{"%)"}" : $"{"M("}{UtilityOrderDetails.GETInstance.MktProtection:0.00}{"%)"}";// "M ( " + UtilityOrderDetails.GETInstance.MktProtection == null ? "1.0" : UtilityOrderDetails.GETInstance.MktProtection + " % )";
        //    }
        //    else
        //        csv.ProtectionPercentage = (UtilityOrderDetails.GETInstance.MktProtection == null ? "1.0" : UtilityOrderDetails.GETInstance.MktProtection).ToString();//Convert.ToString(1);
        //    //if (item.OrderType == "G")
        //    //    csv.ProtectionPercentage = Convert.ToString(1);
        //    //else
        //    //    csv.ProtectionPercentage = Convert.ToString(1);

        //    csv.ParticipantCode = item.ParticipantCode;

        //    csv.OrderRetentionStatus = item.OrderRetentionStatus;// check that retention should be EOD,IOC,EOS
        //    //csv.TriggerPrice = item.TriggerPrice;

        //    csv.SenderLocationID = UtilityLoginDetails.GETInstance.SenderLocationId;
        //    csv.MessageTag = GetOrderMessageTag();
        //    if (cnt == 0 && error == 0)
        //    {
        //        csv.BatchKey = string.Format("{0}_{1}", csv.ScripCode, csv.MessageTag);
        //        ObjClientCollection.Add(csv);
        //        //i++;
        //    }
        //    csv.OrderFomLoadButton = item.OrderFomLoadButton;

   // }
    // TotBuyQty = ObjClientCollection.Where(y => y.BuySellIndicator == "B").Sum<OrderModel>(x => x.OriginalQty).ToString();
    // TotSellQty = ObjClientCollection.Where(y => y.BuySellIndicator == "S").Sum<OrderModel>(x => x.OriginalQty).ToString();
    //        ClientsCount = "Batch Submission - Orders (Total Count : " + ObjClientCollection.Count + ")";
    //    }
    //    else
    //    {
    //        return;
    //    }
    //}


        //private static void Read_CPCodeDerivative()
        //{

        //    MemoryManager.lines = File.ReadAllLines(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"Master files/EQD_PARTICIPANT12032018.TXT"))).ToList<string>();

        //}

        // private void   OnChangeofCPCodeCurrency()
        //{
        //    if (CPCodeCurrencytxt != null)
        //    {
        //        string cpCode = CPCodeDerivativetxt;
        //        CPCodeCurrencyNametxt = objCPCodeCurrencyDict[cpCode].ParticipantName;
        //    }
        //}

        //private void OnChangeofCPCodeDerivative()
        //{
        //    if (CPCodeDerivativetxt != null)
        //    {
        //        string cpCode = CPCodeDerivativetxt;
        //        CPCodeDerivativeNametxt = objCPCodeDerivativeDict[cpCode].ParticipantName;
        //    }
        //}

        private void ControlsUpdation()
        {
            CPCodeDerivativeChecked = false;
            CPCodeDerivativetxtEnable = false;
            CPCodeCurrencyChecked = false;

            //CPCodeCurrencyChecked = false;
            CPCodeCurrencytxtEnable = false;
            // throw new NotImplementedException();
        }
        private void FilterAndPopulate(object e)
        {
            if (!string.IsNullOrEmpty(txtClientSearch))
            {
                var a = ObjClientCollection.FirstOrDefault(x => x.CompleteClientId == txtClientSearch);
                SelectedClient = a;
               
                UpdateDataGrid(e);
               
                //this.SetFocus("Property");
                Replytxt = "";
            }
            else if (string.IsNullOrEmpty(txtClientSearch) == true)
            {
                Replytxt = "Insert Complete ClientId for Search";
            }

        }

        //private void CPCode()
        //{
        //    if (SelectedClientType1 == Enumerations.Order.ClientTypes.CLIENT.ToString())
        //    {
        //        CPCodeDerivativeEnabled = Boolean.FalseString;
        //        CPCodeCurrencyEnabled = Boolean.FalseString;
        //        //if (CPCodeDerivativeChecked == true || CPCodeCurrencyChecked == true)
        //        //{
        //        //    MessageBox.Show("CP Code cannot be entered for client type 'CLIENT'", "!Warning!", MessageBoxButton.OK, MessageBoxImage.Error);
        //        //}

        //    }
        //    //else if (SelectedClientType1 == Enumerations.Order.ClientTypes.INST.ToString())
        //    //{
        //    //    CPCodeDerivativeEnabled = Boolean.TrueString;
        //    //    CPCodeCurrencyEnabled = Boolean.TrueString;
        //    //}
        //    //else if(SelectedClientType1 == Enumerations.Order.ClientTypes.SPLCLI.ToString())
        //    //{
        //    //    CPCodeDerivativeEnabled = Boolean.TrueString;
        //    //    CPCodeCurrencyEnabled = Boolean.TrueString;
        //    //}
        //}
        private void PopulateGrid()
        {
            try
            {
                ObjClientCollection.Clear();

                foreach (var item in MasterSharedMemory.objClientMasterDict)
                {
                    ClientProfilingModel objClientProfilingModel = new ClientProfilingModel();

                    objClientProfilingModel.SerialNo = item.Value.SerialNo;
                    objClientProfilingModel.ShortClientId = item.Value.ShortClientId;
                    objClientProfilingModel.CompleteClientId = item.Value.CompleteClientId;
                    objClientProfilingModel.FirstName = item.Value.FirstName;
                    objClientProfilingModel.LastName = item.Value.LastName;
                    objClientProfilingModel.ClientType = item.Value.ClientType;
                    objClientProfilingModel.ClientStatus = item.Value.ClientStatus;
                    objClientProfilingModel.CPCodeDerivative = item.Value.CPCodeDerivative;
                    objClientProfilingModel.CPCodeCurrency = item.Value.CPCodeCurrency;
                    objClientProfilingModel.EmailID = item.Value.EmailID;
                    objClientProfilingModel.MobileNumber = item.Value.MobileNumber;


                    ObjClientCollection.Add(objClientProfilingModel);
                    // objClientMasterLst.Add(objClientMasters);
                }
            }
            catch (Exception e)
            {

            }
        }
        private void ControlEnability()
        {
            if (CPCodeDerivativeChecked == true && SelectedClientType1 == Enumerations.Order.ClientTypes.CLIENT.ToString())
            {
                System.Windows.MessageBox.Show("CP Code cannot be entered for client type 'CLIENT'", "!Warning!", MessageBoxButton.OK, MessageBoxImage.Error);
                CPCodeDerivativetxtEnable = false;
                CPCodeDerivativeChecked = false;
            }
            else if (CPCodeDerivativeChecked == true && SelectedClientType1 == Enumerations.Order.ClientTypes.INST.ToString())
            {
                CPCodeDerivativetxtEnable = true;
            }
            else if (CPCodeDerivativeChecked == true && SelectedClientType1 == Enumerations.Order.ClientTypes.SPLCLI.ToString())
            {
                CPCodeDerivativetxtEnable = true;
            }
            else
            {
                CPCodeDerivativetxtEnable = false;
            }

            if (CPCodeCurrencyChecked == true)
            {

            }
             if(CPCodeDerivativeChecked == false)
            {

                if (!string.IsNullOrEmpty(CPCodeDerivativetxt))
                {
                    
                }
                else
                {
                    //obj.CPCodeDerivative = string.Empty;
                   // Replytxt = "Entered CP Code Derivative not present in the CP Code Derivative Master.";
                }
                //CPCodeDerivativetxt = string.Empty;
            }
             if(CPCodeCurrencytxtEnable  == false)
            {

                if (!string.IsNullOrEmpty(CPCodeCurrencytxt))
                {

                }
                else
                {
                    //obj.CPCodeDerivative = string.Empty;
                    //Replytxt = "Entered CP Code Currency not present in the CP Code Currency Master.";
                }
                //CPCodeCurrencytxt = string.Empty;
            }
        }

        private void CurrencyControlEnability()
        {
            if (CPCodeCurrencyChecked == true && SelectedClientType1 == Enumerations.Order.ClientTypes.CLIENT.ToString())
            {
                System.Windows.MessageBox.Show("CP Code cannot be entered for client type 'CLIENT'", "!Warning!", MessageBoxButton.OK, MessageBoxImage.Error);
                CPCodeCurrencytxtEnable = false;
                CPCodeCurrencyChecked = false;
            }
            else if (CPCodeCurrencyChecked == true && SelectedClientType1 == Enumerations.Order.ClientTypes.INST.ToString())
            {
                CPCodeCurrencytxtEnable = true;
            }
            else if (CPCodeCurrencyChecked == true && SelectedClientType1 == Enumerations.Order.ClientTypes.SPLCLI.ToString())
            {
                CPCodeCurrencytxtEnable = true;
            }
            else
            {
                CPCodeCurrencytxtEnable = false;
            }
        }

        private void PopulateClientType()
        {


            ClientType1 = new List<string>();
            if (string.IsNullOrEmpty(SelectedClientType1))
                SelectedClientType1 = Enumerations.Order.ClientTypes.CLIENT.ToString();


            {
                ClientType1.Add(Enumerations.Order.ClientTypes.CLIENT.ToString());

                ClientType1.Add(Enumerations.Order.ClientTypes.INST.ToString());
                ClientType1.Add(Enumerations.Order.ClientTypes.SPLCLI.ToString());

            }
        }

        //private void CPCode()
        //{
        //    if(SelectedClientType1 = INST || SelectedClientType1 = SPLCLI)
        //    {

        //    }
        //}

        private void UpdateDataGrid(object e)
        {

            if (SelectedClient != null)
            {
                Replytxt = string.Empty;
                ShortClientIdtxt = SelectedClient.ShortClientId;
                CompleteClientIdtxt = SelectedClient.CompleteClientId;
                FirstNametxt = SelectedClient.FirstName;
                LastNametxt = SelectedClient.LastName;
                MobileNumbertxt = SelectedClient.MobileNumber;
                EmailIDtxt = SelectedClient.EmailID;
                CPCodeCurrencytxt = SelectedClient.CPCodeCurrency;
                if (string.IsNullOrEmpty(CPCodeCurrencytxt)) {
                    
                    CPCodeCurrencyNametxt = string.Empty;
                }
                
                CPCodeDerivativetxt = SelectedClient.CPCodeDerivative;
                if (string.IsNullOrEmpty(CPCodeDerivativetxt))
                {
                    CPCodeDerivativeNametxt = string.Empty;
                }
                if (SerialNo != null)
                {
                    SerialNo = SelectedClient.SerialNo;
                }

                SelectedClientType1 = SelectedClient.ClientType;
            }
        }

        private void AddChanges(object e)
        {

            ClientProfilingModel obj = new ClientProfilingModel();
            //  obj.SerialNo = SerialNo?.Trim();
            obj.ShortClientId = ShortClientIdtxt?.Trim();
            obj.CompleteClientId = CompleteClientIdtxt?.Trim();
            obj.FirstName = FirstNametxt?.Trim();
            obj.LastName = LastNametxt?.Trim();
            obj.MobileNumber = MobileNumbertxt?.Trim();
            obj.EmailID = EmailIDtxt?.Trim();
            obj.ClientType = SelectedClientType1?.Trim();
            //if(CPCodeDerivativeChecked == false)
            //{
            //    if (!string.IsNullOrEmpty(CPCodeDerivativeNametxt))
            //        obj.CPCodeDerivative = CPCodeDerivativetxt?.Trim();
            //    else
            //    {
            //        obj.CPCodeDerivative = string.Empty;
            //    }
            //}
            //else
            //{
            //    if(!string.IsNullOrEmpty(CPCodeDerivativeNametxt))
            //        obj.CPCodeDerivative = CPCodeDerivativetxt?.Trim();
            //    else
            //    {
            //        obj.CPCodeDerivative = string.Empty;
            //    }
            //}
            //if(CPCodeCurrencyChecked == false)
            //{
            //    if (!string.IsNullOrEmpty(CPCodeCurrencyNametxt))
            //        obj.CPCodeCurrency = CPCodeCurrencytxt?.Trim();
            //    else
            //    {
            //        obj.CPCodeCurrency = string.Empty;
            //    }
            //}
            //else {
            //    if (!string.IsNullOrEmpty(CPCodeCurrencyNametxt))
            //        obj.CPCodeCurrency = CPCodeCurrencytxt?.Trim();
            //    else
            //    {
            //        obj.CPCodeCurrency = string.Empty;
            //    }
            //}
            if (CPCodeDerivativeChecked == false)
            {
                if (!string.IsNullOrEmpty(CPCodeDerivativeNametxt))
                    obj.CPCodeDerivative = CPCodeDerivativetxt?.Trim();
                else if (string.IsNullOrEmpty(CPCodeDerivativetxt))
                {
                    Replytxt = "";
                }
                else
                {
                    obj.CPCodeDerivative = string.Empty;
                    Replytxt = "Entered CP Code Derivative not present in the CP Code Derivative Master.";
                    return;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(CPCodeDerivativeNametxt))
                    obj.CPCodeDerivative = CPCodeDerivativetxt?.Trim();
                else if (string.IsNullOrEmpty(CPCodeDerivativetxt))
                {
                    Replytxt = "";
                }

                else
                {
                    obj.CPCodeDerivative = string.Empty;
                    Replytxt = "Entered CP Code Derivative not present in the CP Code Derivative Master.";
                    return;
                }
            }
            if (CPCodeCurrencyChecked == false)
            {
                if (!string.IsNullOrEmpty(CPCodeCurrencyNametxt))
                    obj.CPCodeCurrency = CPCodeCurrencytxt?.Trim();
                else if (string.IsNullOrEmpty(CPCodeCurrencytxt))
                {
                    Replytxt = "";
                }

                else
                {
                    obj.CPCodeCurrency = string.Empty;
                    Replytxt = "Entered CP Code Currency not present in the CP Code Currency Master.";
                    return;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(CPCodeCurrencyNametxt))
                    obj.CPCodeCurrency = CPCodeCurrencytxt?.Trim();
                else if (string.IsNullOrEmpty(CPCodeCurrencytxt))
                {
                    Replytxt = "";
                }
                else
                {
                    obj.CPCodeCurrency = string.Empty;
                    Replytxt = "Entered CP Code Currency not present in the CP Code Currency Master.";
                    return;
                }
            }



            if (String.IsNullOrEmpty(ShortClientIdtxt))
            {
                Replytxt = "Enter Short ClientId";
                return;
                // return because we don't want to run normal code of buton click
            }
            //check short client id duplication case
            else if (ObjClientCollection.Count > 0)
            {
                foreach (var item in ObjClientCollection)
                {
                    if (item.ShortClientId == obj.ShortClientId)
                    {
                        Replytxt = "Short Client Id should be Unique";
                        return;
                    }
                }
            }

            if (String.IsNullOrEmpty(CompleteClientIdtxt))
            {
                Replytxt = "Enter Complete ClientId";
                return;

                // return because we don't want to run normal code of buton click
            }
            if (!String.IsNullOrEmpty(CompleteClientIdtxt))
            {
                foreach (var item in ObjClientCollection)
                {
                    if (item.CompleteClientId == obj.CompleteClientId)
                    {
                        Replytxt = "Complete Client Id should be Unique";
                        return;
                    }
                }

            }

            if (!String.IsNullOrEmpty(EmailIDtxt))
            {
                if (!Regex.IsMatch(EmailIDtxt, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$"))
                {
                    Replytxt = "Enter a valid email";

                }
            }
            else
            {
                Replytxt = "";
            }

            if (!String.IsNullOrEmpty(MobileNumbertxt))
            {

                if (Regex.Match(MobileNumbertxt, @"^(\+[0-9])$").Success)
                {
                    Replytxt = "Enter valid Mobile Number";
                    //return;
                }
            }
            else
            {
                Replytxt = "";
            }

            ObjClientCollection.Add(obj);
           // Replytxt = "Client Added Successfully.";
            //Replytxt = "";
            try
            {
                //  ClientProfilingModel obj = new ClientProfilingModel();
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

                str = @"INSERT INTO CLIENT_MASTER(FirstName, MiddleName, LastName, ClientType, ClientStatus, MobileNum, Email, ShortClientId, CompleteClientId, CPCodeDerivative, CPCodeCurrency, OwnerLoginId, UserLoginId, Password, MobilePwd, TransactionPwd, Salutation, PanCardNum) VALUES(" + "'" + obj.FirstName + "'," + "'NA'," + "'" + obj.LastName + "'," + "'" + obj.ClientType + "'," + "'NA'," + "'" + obj.MobileNumber + "','" + obj.EmailID + "','" + obj.ShortClientId + "','" + obj.CompleteClientId + "','" + obj.CPCodeDerivative + "','" + obj.CPCodeCurrency + "'," + "'NA'," + "'NA'," + "'NA'," + "'NA'," + "'NA'," + "'NA'," + "'NA');";
                //str = @"INSERT INTO USER_DEFINED_PROFILE(MemberID,TraderID,ScreenName,ColumnName,ProfileName,ColPriorityVal,DefProfile)
                //                 VALUES( " + obj.FirstName + "," + textbox3.Text+ " );";
                //                         

                int result = oDataAccessLayer.ExecuteNonQuery((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);

            }

            catch (Exception ex)
            {
                return;
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
            }
           

            ClientMaster objClientMaster = new ClientMaster();
            objClientMaster.FirstName = obj.FirstName;
            objClientMaster.LastName = obj.LastName;
            objClientMaster.ShortClientId = obj.ShortClientId;
            objClientMaster.ClientType = obj.ClientType;
            objClientMaster.CompleteClientId = obj.CompleteClientId;
            objClientMaster.EmailID = obj.EmailID;
            objClientMaster.MobileNumber = obj.MobileNumber;
            objClientMaster.CPCodeDerivative = obj.CPCodeDerivative;
            objClientMaster.CPCodeCurrency = obj.CPCodeCurrency;
            var matchkeyClientMaster = string.Format("{0}_{1}", objClientMaster.CompleteClientId, objClientMaster.ClientType);

            objClientMasterDict.TryAdd(matchkeyClientMaster, objClientMaster);

            ShortClientIdtxt = string.Empty;
            CompleteClientIdtxt = string.Empty;
            FirstNametxt = string.Empty;
            LastNametxt = string.Empty;
            MobileNumbertxt = string.Empty;
            EmailIDtxt = string.Empty;
            CPCodeDerivativetxt = string.Empty;
            CPCodeDerivativeNametxt = string.Empty;
            CPCodeCurrencytxt = string.Empty;
            CPCodeCurrencyNametxt = string.Empty;
            CPCodeDerivativeChecked = false;
            CPCodeCurrencyChecked = false;



            Replytxt = "Client Added Successfully.";

            if (OnClientProfilingChange != null)
            {
                OnClientProfilingChange?.Invoke();
            }
        }

        SharedMemories.MasterSharedMemory.ClientMaster objOldClientMaster = new SharedMemories.MasterSharedMemory.ClientMaster();
        private void EditChanges(object e)
        {
            try
            {

                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

                ClientProfilingModel obj = new ClientProfilingModel();

                

                if (String.IsNullOrEmpty(ShortClientIdtxt))
                {
                    Replytxt = "Enter Short ClientId";
                    return;

                }


                if (String.IsNullOrEmpty(CompleteClientIdtxt))
                {
                    Replytxt = "Enter Complete ClientId";
                    return;

                    // return because we don't want to run normal code of buton click
                }



                if (ObjClientCollection.Count > 0)
                {

                    //foreach (var item in ObjClientCollection)
                    //{

                        if (SelectedClient.ShortClientId == ShortClientIdtxt && SelectedClient.CompleteClientId == CompleteClientIdtxt && SelectedClient.ClientType == SelectedClientType1
                            && SelectedClient.FirstName == FirstNametxt && SelectedClient.LastName == LastNametxt && SelectedClient.MobileNumber == MobileNumbertxt && SelectedClient.EmailID == EmailIDtxt && SelectedClient.CPCodeDerivative == CPCodeDerivativetxt && SelectedClient.CPCodeCurrency == CPCodeCurrencytxt)
                        {
                            Replytxt = "No Attributes Changed";
                            return;
                        }
                    //}

                }

               



                if (!SelectedClient.CompleteClientId.Equals(CompleteClientIdtxt))
                {
                    if (SelectedClient.ShortClientId.Equals(ShortClientIdtxt))
                    {
                        foreach (var item in ObjClientCollection.Where(x => x.ShortClientId != ShortClientIdtxt).ToList())
                        {
                            if (item.CompleteClientId == CompleteClientIdtxt)
                            {
                                Replytxt = "Complete Client Id should be Unique";
                                return;
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in ObjClientCollection.Where(x => x.ShortClientId != SelectedClient.ShortClientId).ToList())
                        {
                            if (item.ShortClientId == ShortClientIdtxt)
                            {
                                Replytxt = "Short Client Id should be Unique";
                                return;
                            }
                        }
                    }

                }

                else if (!SelectedClient.ShortClientId.Equals(ShortClientIdtxt))
                {
                    if (SelectedClient.CompleteClientId.Equals(CompleteClientIdtxt))
                    {
                        foreach (var item in ObjClientCollection.Where(x => x.CompleteClientId != CompleteClientIdtxt).ToList())
                        {
                            if (item.ShortClientId == ShortClientIdtxt)
                            {
                                Replytxt = "Short Client Id should be Unique";
                                return;
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in ObjClientCollection.Where(x => x.CompleteClientId != SelectedClient.CompleteClientId).ToList())
                        {
                            if (item.CompleteClientId == CompleteClientIdtxt)
                            {
                                Replytxt = "Complete Client Id should be Unique";
                                return;
                            }
                        }
                    }
                }


                //foreach (var item in ObjClientCollection)
                //{
                //    if (item.CompleteClientId == CompleteClientIdtxt)
                //    {
                //        Replytxt = "Complete Client Id should be Unique";
                //        return;
                //    }
                //}




                //foreach (var item in ObjClientCollection)
                //{
                //    if (item.ShortClientId == ShortClientIdtxt)
                //    {
                //        if (ShortClientIdtxt == SelectedClient.ShortClientId)
                //            Replytxt = "Short Client Id should be Unique";
                //        return;
                //    }
                //}



                if (!String.IsNullOrEmpty(EmailIDtxt))
                {
                    if (!Regex.IsMatch(EmailIDtxt, @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                 @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                 @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
                    {
                        Replytxt = "Enter a valid email";

                    }
                }
                else
                {
                    Replytxt = "";
                }



                if (!String.IsNullOrEmpty(MobileNumbertxt))
                {

                    if (Regex.Match(MobileNumbertxt, @"^(\+[0-9])$").Success)
                    {
                        Replytxt = "Enter valid Mobile Number";
                        //return;
                    }
                }
                else
                {
                    Replytxt = "";
                }



                if (CPCodeDerivativeChecked == false)
                {
                    if (!string.IsNullOrEmpty(CPCodeDerivativeNametxt))
                        obj.CPCodeDerivative = CPCodeDerivativetxt?.Trim();
                    else if(string.IsNullOrEmpty(CPCodeDerivativetxt))
                        {
                        Replytxt = "";
                        }
                    else
                    {
                        obj.CPCodeDerivative = string.Empty;
                        Replytxt = "Entered CP Code Derivative not present in the CP Code Derivative Master.";
                        return;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(CPCodeDerivativeNametxt))
                        obj.CPCodeDerivative = CPCodeDerivativetxt?.Trim();
                    else if (string.IsNullOrEmpty(CPCodeDerivativetxt))
                    {
                        //CPCodeDerivativeNametxt = string.Empty;
                        Replytxt = "";
                    }

                    else
                    {
                        obj.CPCodeDerivative = string.Empty;
                        Replytxt = "Entered CP Code Derivative not present in the CP Code Derivative Master.";
                        return;
                    }
                }
                if (CPCodeCurrencyChecked == false)
                {
                    if (!string.IsNullOrEmpty(CPCodeCurrencyNametxt))
                        obj.CPCodeCurrency = CPCodeCurrencytxt?.Trim();
                    else if (string.IsNullOrEmpty(CPCodeCurrencytxt))
                    {
                        Replytxt = "";
                    }

                    else
                    {
                        obj.CPCodeCurrency = string.Empty;
                        Replytxt = "Entered CP Code Currency not present in the CP Code Currency Master.";
                        return;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(CPCodeCurrencyNametxt))
                        obj.CPCodeCurrency = CPCodeCurrencytxt?.Trim();
                    else if (string.IsNullOrEmpty(CPCodeCurrencytxt))
                    {
                        Replytxt = "";
                    }
                    else
                    {
                        obj.CPCodeCurrency = string.Empty;
                        Replytxt = "Entered CP Code Currency not present in the CP Code Currency Master.";
                        return;
                    }
                }




                //ClientProfilingModel obj = new ClientProfilingModel();
                str =
           @"UPDATE CLIENT_MASTER SET FirstName = '" + FirstNametxt + "', " +
           "LastName = '" + LastNametxt + "', " +
           "ClientType = '" + SelectedClientType1 + "', " +
           "MobileNum = '" + MobileNumbertxt + "' ," +
           "Email = '" + EmailIDtxt + "', " +
           "ShortClientId = '" + ShortClientIdtxt + "' ," +
           "CompleteClientId = '" + CompleteClientIdtxt + "'," +
           "CPCodeDerivative = '" + obj.CPCodeDerivative + "'," +
           "CPCodeCurrency = '" + obj.CPCodeCurrency + "'" +
           "WHERE ShortClientId = '" + SelectedClient.ShortClientId + "'";


                int result = oDataAccessLayer.ExecuteNonQuery((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);

                if (result > 0)
                {
                    //Update Dict
                    int index = ObjClientCollection.Where(i => i.ShortClientId == SelectedClient.ShortClientId).Select(x => ObjClientCollection.IndexOf(x)).FirstOrDefault();
                    ClientMaster objClientMasterNew = new ClientMaster();
                    objClientMasterNew.ShortClientId = ShortClientIdtxt?.ToString();
                    objClientMasterNew.CompleteClientId = CompleteClientIdtxt?.ToString();
                    objClientMasterNew.FirstName = FirstNametxt?.ToString();
                    objClientMasterNew.LastName = LastNametxt?.ToString();
                    objClientMasterNew.MobileNumber = MobileNumbertxt?.ToString();
                    objClientMasterNew.EmailID = EmailIDtxt?.ToString();
                    objClientMasterNew.ClientType = SelectedClientType1?.ToString();
                    objClientMasterNew.CPCodeDerivative = obj.CPCodeDerivative;
                    objClientMasterNew.CPCodeCurrency = obj.CPCodeCurrency;

                    var key1 = string.Format("{0}_{1}", ObjClientCollection[index].CompleteClientId, ObjClientCollection[index].ClientType);
                    var key2 = string.Format("{0}_{1}", CompleteClientIdtxt, SelectedClientType1);

                    if (key1 != key2)
                    {
                        objClientMasterDict.TryRemove(key1, out objOldClientMaster);
                        objClientMasterDict.TryAdd(key2, objClientMasterNew);
                    }
                    else
                    {

                        objClientMasterDict.TryUpdate(key2, objClientMasterNew, objClientMasterDict[key2]);

                    }

                    //update collection
                    ObjClientCollection[index].ShortClientId = ShortClientIdtxt?.ToString();
                    ObjClientCollection[index].CompleteClientId = CompleteClientIdtxt?.ToString();
                    ObjClientCollection[index].FirstName = FirstNametxt?.ToString();
                    ObjClientCollection[index].LastName = LastNametxt?.ToString();
                    ObjClientCollection[index].MobileNumber = MobileNumbertxt?.ToString();
                    ObjClientCollection[index].EmailID = EmailIDtxt?.ToString();
                    ObjClientCollection[index].ClientType = SelectedClientType1?.ToString();
                    ObjClientCollection[index].CPCodeDerivative = obj.CPCodeDerivative;
                    ObjClientCollection[index].CPCodeCurrency = obj.CPCodeCurrency;
                    //upate Collection


                    //UPdate Dict

                    // objClientMaster.SerialNo = SerialNo;

                    //var key = string.Format("{0}_{1}", SelectedClient.CompleteClientId, SelectedClient.ClientType);
                    //if (objClientMasterDict != null && objClientMasterDict.Count > 0 && objClientMasterDict.TryUpdate(key, objClientMaster,))
                    //{
                    //    if (ObjClientCollection != null && ObjClientCollection.Count > 0)
                    //    {
                    //        var index = ObjClientCollection.IndexOf(ObjClientCollection.Where(x => (x.ShortClientId == SelectedClient.ShortClientId) && (x.CompleteClientId == SelectedClient.CompleteClientId) && (x.ClientType == SelectedClient.ClientType)).FirstOrDefault());
                    //        //if (index != -1)
                    //        //{
                    //        //    ObjClientCollection.RemoveAt(index);
                    //        //}
                    //    }

                    //}
                }

                else
                {
                    System.Windows.MessageBox.Show("Item not found in Database", "!Warning!", MessageBoxButton.OK, MessageBoxImage.Error);
                }


            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
            }
            ShortClientIdtxt = string.Empty;
            CompleteClientIdtxt = string.Empty;
            FirstNametxt = string.Empty;
            LastNametxt = string.Empty;
            MobileNumbertxt = string.Empty;
            EmailIDtxt = string.Empty;
            CPCodeDerivativetxt = string.Empty;
            CPCodeDerivativeNametxt = string.Empty;
            CPCodeCurrencytxt = string.Empty;
            CPCodeCurrencyNametxt = string.Empty;
            CPCodeDerivativeChecked = false;
            CPCodeCurrencyChecked = false;
            Replytxt = "Client updated successfully.";
            //if (!string.IsNullOrEmpty(Replytxt))
            //{

         
            if (OnClientProfilingChange != null)
            {
                OnClientProfilingChange?.Invoke();
            }


            //    Replytxt = string.Empty;
            //    return;
            //}
        }

        //private void ApplyChanges(object e)
        //{

        //}

        //private void SaveAsChanges(object e)
        //{

        //}

        private void DeleteChanges(object e)
        {
            try
            {
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);
                ClientMaster objClientMaster = new ClientMaster();
                foreach (var item in ObjClientCollection.Where(x => x.IsSelected).ToList())
                {

                    var key1 = string.Format("{0}_{1}", item.CompleteClientId, item.ClientType);
                    DeleteFromDataBase(item.ClientType, item.ShortClientId, item.CompleteClientId);  //Delete from Database
                    bool deleted = objClientMasterDict.TryRemove(key1, out objClientMaster);   // delete from Dict
                    ObjClientCollection.Remove(item);  // delete from Collection
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
            }
            ShortClientIdtxt = string.Empty;
            CompleteClientIdtxt = string.Empty;
            FirstNametxt = string.Empty;
            LastNametxt = string.Empty;
            MobileNumbertxt = string.Empty;
            EmailIDtxt = string.Empty;
            CPCodeDerivativetxt = string.Empty;
            CPCodeDerivativeNametxt = string.Empty;
            CPCodeCurrencytxt = string.Empty;
            CPCodeCurrencyNametxt = string.Empty;
            CPCodeDerivativeChecked = false;
            CPCodeCurrencyChecked = false;

            Replytxt = "Client Deleted Successfully";
            if (OnClientProfilingChange != null)
            {
                OnClientProfilingChange?.Invoke();
            }


        }

        private void DeleteFromDataBase(string clientType, string ShortClientId, string completeClientId)
        {
            try
            {
                oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

                str = @"DELETE FROM CLIENT_MASTER WHERE ClientType ='" + clientType + "' AND ShortClientId ='" + ShortClientId + "' AND CompleteClientId ='" + completeClientId + "'";

                int result = oDataAccessLayer.ExecuteNonQuery((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
            finally
            {
                oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
            }
        }

        private void DeleteAllChanges(object e)
        {
           
            System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Are you sure?", "Delete All Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo);

            if (dialogResult == System.Windows.Forms.DialogResult.Yes)  // error is here
            {
                try
                {
                    oDataAccessLayer.OpenConnection((int)DataAccessLayer.ConnectionDB.Masters);

                    str = @"DELETE FROM CLIENT_MASTER";
                    int result = oDataAccessLayer.ExecuteNonQuery((int)ConnectionDB.Masters,str, System.Data.CommandType.Text, null);



                    if (result > 0)
                    {
                        ObjClientCollection.Clear();
                        objClientMasterDict.Clear();
                    }

                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogError(ex);
                }
                finally
                {
                    oDataAccessLayer.CloseConnection((int)DataAccessLayer.ConnectionDB.Masters);
                }

                ShortClientIdtxt = string.Empty;
                CompleteClientIdtxt = string.Empty;
                FirstNametxt = string.Empty;
                LastNametxt = string.Empty;
                MobileNumbertxt = string.Empty;
                EmailIDtxt = string.Empty;
                CPCodeDerivativetxt = string.Empty;
                CPCodeDerivativeNametxt = string.Empty;
                CPCodeCurrencytxt = string.Empty;
                CPCodeCurrencyNametxt = string.Empty;
                CPCodeDerivativeChecked = false;
                CPCodeCurrencyChecked = false;

                Replytxt = "All Clients are Deleted Successfully";


               
            }
            else
            {
                return;
            }
            if (OnClientProfilingChange != null)
            {
                OnClientProfilingChange?.Invoke();
            }

        }
         
    }

    #endregion

}



