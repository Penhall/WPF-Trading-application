using CommonFrontEnd.Common;
using CommonFrontEnd.Global;
using CommonFrontEnd.Model.Trade;
using CommonFrontEnd.View.Trade;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace CommonFrontEnd.ViewModel.Trade
{
#if TWS
    class OptionVM : BaseViewModel
    {
        //public static ObservableCollection<SaudasUMSModel> TradeViewDataCollectionDemo { get; set; }
        DirectoryInfo directoryadm = new DirectoryInfo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"User/Saudas_ADM.txt")));
        DirectoryInfo directorybrq = new DirectoryInfo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"User/Saudas_BRQ.txt")));

        public OptionVM()
        {
            // TradeViewDataCollectionDemo = new ObservableCollection<SaudasUMSModel>();
            //TradeViewDataCollectionDemo = AdminTradeViewVM.TradeViewDataCollection;
        }

        #region RelayCommand

        private RelayCommand _btnCloseClick;

        public RelayCommand btnCloseClick
        {
            get
            {
                return _btnCloseClick ?? (_btnCloseClick = new RelayCommand(
                    (object e) => OptionWindow_Close(e)));

            }

        }

        private void OptionWindow_Close(object e)
        {
            Option oOption = System.Windows.Application.Current.Windows.OfType<Option>().FirstOrDefault();
            if (oOption != null)
            {
                oOption.Hide();
            }
        }

        private RelayCommand _PipeSeparatedADM;

        public RelayCommand PipeSeparatedADM
        {
            get
            {
                return _PipeSeparatedADM ?? (_PipeSeparatedADM = new RelayCommand((object e) => ExecuteMyRequestADM()));

            }
        }


        private RelayCommand _PipeSeparatedBrokerQuery;

        public RelayCommand PipeSeparatedBrokerQuery
        {
            get
            {
                return _PipeSeparatedBrokerQuery ?? (_PipeSeparatedBrokerQuery = new RelayCommand((object e) => ExecuteMyRequestBQ()));
            }
        }

        #endregion

        private void ExecuteMyRequestADM()
        {
            try
            {
                var TradeViewDataCollection = SharedMemories.MemoryManager.TradeMemoryConDict.ToList();

                if (File.Exists(directoryadm.ToString()))
                {
                    File.Delete(directoryadm.ToString());
                }

                using (StreamWriter sw = new StreamWriter(directoryadm.ToString()))
                {

                    if (UtilityLoginDetails.GETInstance.Role.ToLower() == "admin")
                    {
                        foreach (KeyValuePair<long, object> item in TradeViewDataCollection)
                        {
                            var dr = (TradeUMS)item.Value;
                            sw.Write(dr.ScripCode + "|" + dr.ScripID + "|" + dr.TraderId + "|" + dr.Rate + "|" + dr.LastQty + "|" + dr.OppMemberId + "|" + dr.Reserved + "|" +
                                dr.TimeOnly + "|" + dr.DateOnly + "|" + dr.Client + "|" + dr.BSFlag + "|" + dr.OrderType + "|" + dr.OrderID + "|" + dr.ClientType + "|" + dr.ISIN + "|" +
                                dr.ScripGroup + "|" + /*string.Join("/", dr.SettlNo)*/ dr.SettlNo[2] + "|" + dr.SenderLocationID + "|");

                            sw.Write(sw.NewLine);
                        }
                        System.Windows.MessageBox.Show("ADM Format Trades Saved in file :" + directoryadm.ToString());
                    }
                    //else if (UtilityLoginDetails.GETInstance.Role.ToLower() == "trader")
                    //{
                    //    foreach (KeyValuePair<long, object> item in TradeViewDataCollection)
                    //    {
                    //        var dr = (TradeUMS)item.Value;
                    //        sw.Write(dr.ScripCode + "|" + dr.ScripID + "|" + dr.TradeID + "|" + dr.Rate + "|" + dr.LastQty + "|" + dr.OppMemberId + "|" + dr.Reserved + "|" +
                    //            dr.TimeOnly + "|" + dr.DateOnly + "|" + dr.Client + "|" + dr.BSFlag + "|" + dr.OrderType + "|" + dr.OrderID + "|" + dr.ClientType + "|" + dr.ISIN + "|" +
                    //            dr.ScripGroup + "|" + /*string.Join("/", dr.SettlNo)*/ dr.SettlNo[2] + "|");

                    //        sw.Write(sw.NewLine);
                    //    }
                    //    System.Windows.MessageBox.Show("ADM Format Trades Saved in file :" + directoryadm.ToString());
                    //}

                }
            }
            catch (IOException oIOException)
            {
                System.Windows.MessageBox.Show("File is being used by another process", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
                System.Windows.MessageBox.Show("Error in Exporting data in ADM Format", "ERROR!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                //sw.Flush();
                //sw.Close();
            }
        }


        private void ExecuteMyRequestBQ()
        {
            try
            {
                var data = SharedMemories.MemoryManager.TradeMemoryConDict.ToList();

                if (File.Exists(directorybrq.ToString()))
                {
                    File.Delete(directorybrq.ToString());
                }

                using (StreamWriter sw = new StreamWriter(directorybrq.ToString()))
                {
                    if (UtilityLoginDetails.GETInstance.Role.ToLower() == "admin")
                    {
                        foreach (KeyValuePair<long, object> item in data)
                        {
                            var dr = (TradeUMS)item.Value;
                            sw.Write(dr.MemberID + "|" + dr.TraderId + "|" + dr.ScripCode + "|" + dr.ScripID + "|" + dr.Rate + "|" + dr.LastQty + "|" + dr.OppMemberId + "|" + dr.Reserved + "|" + dr.TimeOnly + "|" + dr.DateOnly + "|" + dr.Client + "|" +
                                dr.OrderID + "|" + dr.OrderType + "|" + dr.BSFlag + "|" + dr.TradeID + "|" + dr.ClientType + "|" + dr.ISIN + "|" + dr.ScripGroup + "|" + /*string.Join("/", dr.SettlNo)*/ dr.SettlNo[2] + "|" + dr.OrderTimeStamp + "|" + dr.SenderLocationID + "|");

                            sw.Write(sw.NewLine);
                        }
                        System.Windows.MessageBox.Show("Broker Query Format Trades Saved in file :" + directorybrq.ToString());
                    }
                    //else if (UtilityLoginDetails.GETInstance.Role.ToLower() == "trader")
                    //{
                    //    foreach (KeyValuePair<long, object> item in data)
                    //    {
                    //        var dr = (TradeUMS)item.Value;
                    //        sw.Write(dr.TraderId + "|" + dr.ScripCode + "|" + dr.ScripID + "|" + dr.Rate + "|" + dr.LastQty + "|" + dr.OppMemberId + "|" + dr.Reserved + "|" + dr.TimeOnly + "|" + dr.DateOnly + "|" + dr.Client + "|" +
                    //            dr.OrderID + "|" + dr.OrderType + "|" + dr.BSFlag + "|" + dr.TradeID + "|" + dr.ClientType + "|" + dr.ISIN + "|" + dr.ScripGroup + "|" + /*string.Join("/", dr.SettlNo)*/ dr.SettlNo[2] + "|" + dr.OrderTimeStamp + "|" + dr.SenderLocationID + "|");

                    //        sw.Write(sw.NewLine);
                    //    }
                    //    System.Windows.MessageBox.Show("Broker Query Format Trades Saved in file :" + directorybrq.ToString());
                    //}
                    
                }
            }
            catch (IOException oIOException)
            {
                System.Windows.MessageBox.Show("File is being used by another process", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
                System.Windows.MessageBox.Show("Error in Exporting data in Broker Query Format", "ERROR!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                //sw.Flush();
                //sw.Close();
            }
        }
    }

#endif
}
