using BroadcastReceiver;
using CommonFrontEnd.Common;
using CommonFrontEnd.Model;
using CommonFrontEnd.View;
using SubscribeList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonFrontEnd.ViewModel
{
  public  class VarPercentageBroadcastVM : INotifyPropertyChanged
    {
        #region Local Memory
        static SynchronizationContext uiContext;

        private static ObservableCollection<VarPercentageBroadcastModel> _ObjVarPercentageBroadcastollection = new ObservableCollection<VarPercentageBroadcastModel>();

        public static ObservableCollection<VarPercentageBroadcastModel> ObjVarPercentageBroadcastCollection
        {
            get { return _ObjVarPercentageBroadcastollection; }
            set { _ObjVarPercentageBroadcastollection = value; /*NotifyStaticPropertyChanged(nameof(ObjIndicesCollection));*/ }
        }
        #endregion
        #region relayCommand
        private RelayCommand _ShortCut_Escape;

        public RelayCommand ShortCut_Escape
        {
            get
            {
                return _ShortCut_Escape ?? (_ShortCut_Escape = new RelayCommand(
                    (object e) => EscapeUsingUserControl(e)
                        ));
            }
        }

        private RelayCommand _FetchFreshVar;
        public RelayCommand FetchFreshVar
        {
            get
            {
                return _FetchFreshVar ?? (_FetchFreshVar = new RelayCommand(
                    (object e) => EqtListener_VarTick()
                        ));
            }
        }


        #endregion
        #region methos
        private void EscapeUsingUserControl(object e)
        {
            VarPercentageBroadcast oVarPercentageBroadcast = e as VarPercentageBroadcast;
            oVarPercentageBroadcast.Hide();
        }
        #endregion

        #region NotifyProperty
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, e);
            }
        }
        #endregion


        public VarPercentageBroadcastVM()
        {
            #region WebApps link
           // Process.Start("http://10.1.101.125:3000/twsreports/webapp.htm");
            #endregion

            //  ObjVarPercentageBroadcastCollection = new ObservableCollection<VarPercentageBroadcastModel>();
        }

        public static void EqtListener_VarTick()
        {
            try
            {
                if (VarMemory.SubscribeVarMemoryDict.Count > 0 && Globals.VarWindowOpen == true)
                {
                    objvarPercentageBroadcastProcessor_OnBroadCastRecievedNew();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally { }
        }
        public static void objvarPercentageBroadcastProcessor_OnBroadCastRecievedNew()
        {
            try
            {
                //uiContext = SynchronizationContext.Current;

                //ObjVarPercentageBroadcastCollection.Clear();
                foreach (VarMainDetails Br in VarMemory.SubscribeVarMemoryDict.Values.ToList())
                {
                    if (Br != null && CommonFrontEnd.SharedMemories.MasterSharedMemory.objMastertxtDictBaseBSE.ContainsKey(Br.InstrumentCode) && Br.ReservedField5 == 1)
                    {
                        VarPercentageBroadcastModel objVarPercentageBroadcastModel = new VarPercentageBroadcastModel();
                        objVarPercentageBroadcastModel.InstrumentCode = Br.InstrumentCode;

                        //int index = ObjVarPercentageBroadcastCollection.IndexOf(objVarPercentageBroadcastModel);
                        int index = ObjVarPercentageBroadcastCollection.IndexOf(ObjVarPercentageBroadcastCollection.Where(x => x.InstrumentCode == objVarPercentageBroadcastModel.InstrumentCode).FirstOrDefault());

                        if (Br.Identifier == 'E')
                        {
                            objVarPercentageBroadcastModel.ScripName = CommonFunctions.GetScripName(Br.InstrumentCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                            objVarPercentageBroadcastModel.ScripID = CommonFunctions.GetScripId(Br.InstrumentCode, Enumerations.Exchange.BSE, Enumerations.Segment.Equity);
                        }
                        else
                        {
                            objVarPercentageBroadcastModel.ScripName = CommonFunctions.GetScripName(Br.InstrumentCode, Enumerations.Exchange.BSE, Enumerations.Segment.Derivative);
                            objVarPercentageBroadcastModel.ScripID = CommonFunctions.GetScripId(Br.InstrumentCode, Enumerations.Exchange.BSE, Enumerations.Segment.Derivative);
                        }
                        
                        objVarPercentageBroadcastModel.VARIMPercetage = string.Format("{0:0.00}", Convert.ToDouble(Br.IMPercentage) / 100);
                        objVarPercentageBroadcastModel.ELMVARPercentage = string.Format("{0:0.00}", Convert.ToDouble(Br.ELMPercentage) / 100);
                        objVarPercentageBroadcastModel.Identifier = Br.Identifier;
                        lock (ObjVarPercentageBroadcastCollection)
                        {
                            //ObjVarPercentageBroadcastCollection.Add(objVarPercentageBroadcastModel);

                            if(index != -1)
                            {
                                ObjVarPercentageBroadcastCollection[index] = objVarPercentageBroadcastModel;
                            }
                            else
                            {
                                ObjVarPercentageBroadcastCollection.Add(objVarPercentageBroadcastModel);
                            }
                            VarMemory.SubscribeVarMemoryDict[Br.InstrumentCode].ReservedField5 = 0;

                            //if (ObjVarPercentageBroadcastCollection.Any(x => x.InstrumentCode == objVarPercentageBroadcastModel.InstrumentCode))
                            //{
                            //    //int index = ObjVarPercentageBroadcastCollection.IndexOf(ObjVarPercentageBroadcastCollection.Where(x => x.InstrumentCode == objVarPercentageBroadcastModel.InstrumentCode).FirstOrDefault());
                            //    //ObjVarPercentageBroadcastCollection[index] = objVarPercentageBroadcastModel;
                            //}
                            //else
                            //{
                            //    ObjVarPercentageBroadcastCollection.Add(objVarPercentageBroadcastModel);
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }

            //}
        }


    }
}
