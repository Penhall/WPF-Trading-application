using CommonFrontEnd.View;
using CommonFrontEnd.Common;
using CommonFrontEnd.Model;
using CommonFrontEnd.SharedMemories;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static CommonFrontEnd.Model.Order.OrderModel;

namespace CommonFrontEnd.ViewModel
{
   public class HourlyStatisticsVM : INotifyPropertyChanged
    {

        #region Properties

        static string ScripID = String.Empty;
        static string ScripName = String.Empty;
        private static bool flag = true;
        private static bool scripIdChange = true;
        //private static bool scripCodeChange = true;

        // public static Action<long> HRChangeScripIDOrCode;

        private static List<Scrip_Code_ID> scripEquityDetails;
        public static List<Scrip_Code_ID> ScripEquityDetails
        {
            get { return scripEquityDetails; }
            set { scripEquityDetails = value; NotifyStaticPropertyChanged("ScripEquityDetails"); }
        }


        private static List<Scrip_Code_ID> scripCodeEquityDetails;
        public static List<Scrip_Code_ID> ScripCodeEquityDetails
        {
            get { return scripCodeEquityDetails; }
            set { scripCodeEquityDetails = value; NotifyStaticPropertyChanged("ScripCodeEquityDetails"); }
        }

        private static ObservableCollection<HourlyStatisticsModel> _HourlyStatisticsCollection;
        public static ObservableCollection<HourlyStatisticsModel> HourlyStatisticsCollection
        {
            get { return _HourlyStatisticsCollection; }
            set { _HourlyStatisticsCollection = value; NotifyStaticPropertyChanged("HourlyStatisticsCollection"); }
        }

        private RelayCommand _ExportExcel;
        public RelayCommand ExportExcel
        {
            get
            {
                return _ExportExcel ?? (_ExportExcel = new RelayCommand((object e) => SaveHourlyStatisticsTrades()));
            }
        }

        private RelayCommand _ShortCut_Escape;

        public RelayCommand ShortCut_Escape
        {
            get
            {
                return _ShortCut_Escape ?? (_ShortCut_Escape = new RelayCommand(
                    (object e) => OnHourlyStatisticsWindow_Closing(e)
                        ));
            }
        }


        private static string _TitleHourlyStatistics;

        public static string TitleHourlyStatistics
        {
            get { return _TitleHourlyStatistics; }
            set
            {
                _TitleHourlyStatistics = value;
                NotifyStaticPropertyChanged("TitleHourlyStatistics");
            }
        }

        private static string selectedScripId;
        public static string SelectedScripId
        {
            get { return selectedScripId; }
            set
            {
                selectedScripId = value;
                NotifyStaticPropertyChanged("SelectedScripId");
                OnChangeOfScripId();
            }
        }

        private static long? selectedScripCode;

        public static long? SelectedScripCode
        {
            get { return selectedScripCode; }
            set
            {
                selectedScripCode = value;
                NotifyStaticPropertyChanged("SelectedScripCode");
                OnChangeOfScripCode();
            }
        }


        private static string equityScripName;

        public static string EquityScripName
        {
            get { return equityScripName; }
            set
            {
                equityScripName = value;
                NotifyStaticPropertyChanged("EquityScripName");
            }
        }


        #endregion

        #region Business Logic

        public HourlyStatisticsVM()
        {
            FillListData();
        }

        /* Summary 
         * Filling ComboBoxes and TextBox with Equity Scrip Masters, ScripCode and ScripName
         * Maintained list for storing scrips, ScripCode and ScripName */

        public static void FillListData()
        {
            ScripEquityDetails = new List<Scrip_Code_ID>();
            ScripCodeEquityDetails = new List<Scrip_Code_ID>();
            foreach (var item in MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Value.InstrumentType == 'E').OrderBy(y => y.Value.ScripId).ToDictionary(x => x.Key, y => y.Value))
            {
                Scrip_Code_ID scrip = new Scrip_Code_ID();
                scrip.ScripCode = item.Value.ScripCode;
                scrip.ScripId = item.Value.ScripId;
                scrip.ScripName = item.Value.ScripName;

                ScripEquityDetails.Add(scrip);
                ScripCodeEquityDetails.Add(scrip);
            }
            scripEquityDetails.OrderBy(x => x.ScripId);
            if (ScripEquityDetails != null && ScripEquityDetails.Count != 0)
            {
                // SelectedScripId = ScripEquityDetails[0].ScripId;
                SelectedScripCode = ScripEquityDetails[0].ScripCode;
              //  EquityScripName = ScripEquityDetails[0].ScripName;
            }
        }



        /* Summary 
            Both Functions OnChangeOfScripId and OnChangeOfScripCode are used for changing ones output while other is chnaged
            For Eg : If we change ScripCode then corresponding ScripID get Changed */

        private static void OnChangeOfScripId()
        {
            if (scripIdChange)
            {
                flag = false;
                if (SelectedScripId != null)
                {
                    SelectedScripCode = Common.CommonFunctions.GetScripCode(SelectedScripId, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                    EquityScripName = MasterSharedMemory.objMastertxtDictBaseBSE[(long)SelectedScripCode].ScripName;

                }
                else
                {
                    SelectedScripCode = null;
                    EquityScripName = string.Empty;
                }
            }
            scripIdChange = true;
            //if (SelectedScripCode != null && HRChangeScripIDOrCode != null)
            //    HRChangeScripIDOrCode((long)SelectedScripCode);
        }

        private static void OnChangeOfScripCode()
        {
            if (flag)
            {
                scripIdChange = false;
                if (SelectedScripCode != null)
                {
                    SelectedScripId = Common.CommonFunctions.GetScripId((long)SelectedScripCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                    EquityScripName = MasterSharedMemory.objMastertxtDictBaseBSE[(long)SelectedScripCode].ScripName;
                }
                else
                {
                    SelectedScripId = null;
                    EquityScripName = string.Empty;
                }
            }
            flag = true;
            if (selectedScripCode != null)
                UpdateTitleHourlyStatistics(Convert.ToInt32(SelectedScripCode), false);
            else
            {
                HourlyStatisticsCollection.Clear();
                TitleHourlyStatistics = "Hourly Statistics -- ";
            }
            //if (SelectedScripCode != null && HRChangeScripIDOrCode != null)
            //    HRChangeScripIDOrCode((long)SelectedScripCode);
        }



        public static void Initialize()
        {

        }

        /* Summary 
           Fetches data of the corresponding ScripCode from given URL and display the same in the grid and also updates the TITLE  */

        public static void UpdateTitleHourlyStatistics(int ScripCode, bool UpdateDropDowns)
        {
            try
            {

                if (UpdateDropDowns)
                    SelectedScripCode = (long)ScripCode;
                ScripID = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Value.ScripCode == ScripCode).Select(x => x.Value.ScripId).FirstOrDefault();
                ScripName = MasterSharedMemory.objMastertxtDictBaseBSE.Where(x => x.Value.ScripCode == ScripCode).Select(x => x.Value.ScripName).FirstOrDefault();
                TitleHourlyStatistics = "Hourly Statistics -- " + ScripID + "          " + "[" + ScripName + " : " + ScripCode + "]";

                //string url = String.Format("{0}{1}", "http://10.1.101.211:3500/twsreports/HourlyStat.aspx?scripcode=", ScripCode);
                string url = String.Format("{0}{1}", "http://10.1.101.125:3000/twsreports/HourlyStat.aspx?scripcode=", ScripCode);
                HourlyStatisticsCollection = new ObservableCollection<HourlyStatisticsModel>();
                using (WebClient wc = new WebClient())
                {
                    var item = wc.DownloadString(url);
                    string[] data = item.Split('#');
                    int count = data.Length;
                    //HourlyStatisticsModel oHourlyStatisticsModel = new HourlyStatisticsModel();
                    try
                    {
                        if (count > 0)
                        {
                            for (int index = 2; index < count - 1; index++)
                            {
                                string[] row = data[index].Split('|');
                                HourlyStatisticsModel oHourlyStatisticsModel = new HourlyStatisticsModel();
                                oHourlyStatisticsModel.timeperiod = string.Format("{0}-{1}", row[0], row[1]);
                                oHourlyStatisticsModel.open = row[2];
                                oHourlyStatisticsModel.high = row[3];
                                oHourlyStatisticsModel.low = row[4];
                                oHourlyStatisticsModel.close = row[5];
                                oHourlyStatisticsModel.trades = row[6];
                                oHourlyStatisticsModel.Qty = row[7];

                                oHourlyStatisticsModel.value1 = row[8];

                                if (!string.IsNullOrEmpty(oHourlyStatisticsModel.value1))
                                {
                                    if (Convert.ToDecimal(oHourlyStatisticsModel.value1) != 0)
                                    {
                                        if (Convert.ToDecimal(oHourlyStatisticsModel.value1) >= 100000)
                                        {
                                            oHourlyStatisticsModel.value = string.Format("{0:0.00}", Convert.ToDecimal(oHourlyStatisticsModel.value1) / 100000).ToString() + " L";
                                        }
                                        else
                                        {
                                            oHourlyStatisticsModel.value = string.Format("{0:0.00}", (oHourlyStatisticsModel.value1));
                                        }
                                    }
                                    else
                                    {
                                        oHourlyStatisticsModel.value = string.Format("{0:0.00}", (oHourlyStatisticsModel.value1));
                                    }
                                }
                                //else
                                //{
                                //    System.Windows.Forms.MessageBox.Show("ERROR");
                                //}

                                if (!string.IsNullOrEmpty(row[8]))
                                {
                                    if (Convert.ToDecimal(row[8]) != 0)
                                    {
                                        oHourlyStatisticsModel.avgrate = String.Format("{0:0.00}", Convert.ToDecimal(row[8]) / Convert.ToDecimal(row[7])).ToString();
                                    }
                                }


                                HourlyStatisticsCollection.Add(oHourlyStatisticsModel);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        System.Windows.Forms.MessageBox.Show(ex.ToString());
                        //throw ex;
                    }

                }
            }
            catch
            {

                System.Windows.Forms.MessageBox.Show("Unable to Connect to Remote Server");
                //throw ex;
            }
        }

        private void SaveHourlyStatisticsTrades()
        {
            StreamWriter writer = null;
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "HourlyStatistics_" + SelectedScripCode;
            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV Files (.csv)|*.csv";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                writer = new StreamWriter(dlg.FileName, true, Encoding.UTF8);
                try
                {
                    writer.Write("Time Period, Open, High, Low, Close, Qty, Value, Avg.Rate, Trades");
                    writer.Write(writer.NewLine);

                    foreach (HourlyStatisticsModel dr in HourlyStatisticsCollection)
                    {
                        writer.Write(dr.timeperiod + "," + dr.open + "," + dr.high + "," + dr.low + "," + dr.close + "," + dr.Qty + "," + dr.value + "," +
                            dr.avgrate + "," + dr.trades);

                        writer.Write(writer.NewLine);
                    }
                    System.Windows.MessageBox.Show("Trades Saved in file :" + dlg.FileName.ToString());
                }
                catch (Exception e)
                {
                    ExceptionUtility.LogError(e);
                    System.Windows.MessageBox.Show("Error in Exporting data in CSV Format");
                }
                finally
                {
                    if (writer != null)
                    {
                        writer.Flush();
                        writer.Close();
                    }
                }
            }

            else
            {
                return;
            }

        }

        private void OnHourlyStatisticsWindow_Closing(object e)
        {
            try
            {
                HourlyStatistics HourlyStatisticsWindow = System.Windows.Application.Current.Windows.OfType<HourlyStatistics>().FirstOrDefault();

                if (HourlyStatisticsWindow != null)
                {
                    HourlyStatisticsWindow.Close();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region NotifyPropertyChangedEvent

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {

                var e = new PropertyChangedEventArgs(propertyName);

                this.PropertyChanged(this, e);

            }

        }
        #endregion


        #region StaticNotifyPropertyChangedEvent

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged
                 = delegate { };
        private static void NotifyStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
    }
