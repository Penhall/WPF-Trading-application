using CommonFrontEnd.Common;
using CommonFrontEnd.Global;
using CommonFrontEnd.SharedMemories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CommonFrontEnd.Controller
{
#if TWS
    #region Commented


    //#region Process Trade


    //public class ProcessTrade
    //{
    //    private static object newCWobject = new object();
    //    private static object newSWobject = new object();

    //    public static Thread CWThread { get; set; }
    //    public static Thread SWThread { get; set; }
    //    public static Thread CWSWThread { get; set; }
    //    public static Thread SWCWThread { get; set; }

    //    public void ProcessCWSWTrade()
    //    {
    //        CWSWThread = new Thread(() => CWSWThread_Tick());
    //        CWSWThread.Start();
    //        CWSWThread.IsBackground = true;

    //    }
    //    public void ProcessSWCWTrade()
    //    {
    //        SWCWThread = new Thread(() => SWCWThread_Tick());
    //        SWCWThread.Start();
    //        SWCWThread.IsBackground = true;
    //    }
    //    public void ProcessSWTrade()
    //    {
    //        SWThread = new Thread(() => SWThread_Tick());
    //        SWThread.Start();
    //        SWThread.IsBackground = true;
    //    }
    //    public void ProcessCWTrade()
    //    {
    //        CWThread = new Thread(() => CWThread_Tick());
    //        CWThread.Start();
    //        CWThread.IsBackground = true;
    //    }


    //    private void CWSWThread_Tick()
    //    {
    //        object recvData;

    //        while (true)
    //        {
    //            if (MemoryManager.oCWSWConcurrentQueue.Count > 0)
    //            {

    //                CommonFrontEnd.SharedMemories.MemoryManager.oCWSWConcurrentQueue.TryDequeue(out recvData);
    //                CommonFrontEnd.Processor.UMSProcessor.ProcessNetPositionCWSWDetailsDemo(recvData);

    //            }
    //            else
    //            {
    //                Thread.Sleep(1000);
    //            }
    //        }
    //    }
    //    private void SWCWThread_Tick()
    //    {
    //        object recvData;

    //        while (true)
    //        {
    //            if (MemoryManager.oSWCWConcurrentQueue.Count > 0)
    //            {

    //                CommonFrontEnd.SharedMemories.MemoryManager.oSWCWConcurrentQueue.TryDequeue(out recvData);
    //                CommonFrontEnd.Processor.UMSProcessor.ProcessNetPositionSWCWDetailsDemo(recvData);

    //            }
    //            else
    //            {
    //                Thread.Sleep(1000);
    //            }
    //        }
    //    }
    //    static void SWThread_Tick()
    //    {
    //        object recvData;

    //        while (true)
    //        {
    //            if (MemoryManager.oSWConcurrentQueue.Count > 0)
    //            {

    //                CommonFrontEnd.SharedMemories.MemoryManager.oSWConcurrentQueue.TryDequeue(out recvData);
    //                lock (newSWobject)
    //                {
    //                    CommonFrontEnd.Processor.UMSProcessor.ProcessNetPositionSWDemo(recvData);
    //                }

    //            }
    //            else
    //            {
    //                Thread.Sleep(1000);
    //            }
    //        }

    //    }
    //    static void CWThread_Tick()
    //    {
    //        object recvData;

    //        while (true)
    //        {
    //            if (MemoryManager.oCWConcurrentQueue.Count > 0)
    //            {

    //                CommonFrontEnd.SharedMemories.MemoryManager.oCWConcurrentQueue.TryDequeue(out recvData);
    //                lock (newCWobject)
    //                {
    //                    CommonFrontEnd.Processor.UMSProcessor.ProcessNetPositionCWDemo(recvData);
    //                }

    //            }
    //            else
    //            {
    //                Thread.Sleep(1000);
    //            }
    //        }

    //    }
    //}
    //#endregion

    //#region Process Display
    //public class ProcessDisplay
    //{
    //    public static Thread CWThreadDisplay { get; set; }
    //    public void ProcessCWDisplay()
    //    {
    //        CWThreadDisplay = new Thread(() => CWtask_Tick());
    //        CWThreadDisplay.Start();
    //        CWThreadDisplay.IsBackground = true;
    //    }
    //    public void ProcessSWDisplay()
    //    {

    //    }
    //    public void ProcessCWSWDisplay()
    //    {

    //    }
    //    public void ProcessSWCWDisplay()
    //    {

    //    }

    //    static Task CWtask_Tick()
    //    {

    //        CommonFrontEnd.Model.Trade.NetPosition oNp = new Model.Trade.NetPosition();
    //        while (true)
    //        {
    //            if (MemoryManager.NetPositionCWDemoDict.Count > 0)
    //            {
    //                var distinct = MemoryManager.NetPositionCWDemoDict.Values.GroupBy(x => ((CommonFrontEnd.Model.Trade.NetPosition)x).ClientId).Select(g => g.First()).ToList();
    //                foreach (var item in distinct)
    //                {
    //                    oNp = (CommonFrontEnd.Model.Trade.NetPosition)item;
    //                    CommonFrontEnd.SharedMemories.NetPositionMemory.UpdateClientNetPosition(oNp.ClientId, MemoryManager.NetPositionCWDemoDict.Where(x => ((CommonFrontEnd.Model.Trade.NetPosition)x.Value).ClientId == oNp.ClientId).ToList());
    //                }
    //            }

    //        }
    //    }
    //}
    //#endregion
    #endregion

    #region Calculate NetPosition
    public class NetPositionCalculate
    {
        private static object newCWobject = new object();
        private static object newSWobject = new object();

        [ThreadStatic]
        static int initialStartPointCW = 0;

        [ThreadStatic]
        static int initialStartPointSW = 0;

        public static Thread ClientWiseThread { get; set; }
        public static Thread ScripWiseThread { get; set; }
        public static Thread SendTradeFeed { get; set; }

        //TODO unused
        public void ProcessClientWiseTrade()
        {
            ThreadStart ClientWiseThreadStart = new ThreadStart(CWThread_Tick);
            ClientWiseThread = new Thread(ClientWiseThreadStart);
            ClientWiseThread.IsBackground = true;
            ClientWiseThread.Priority = ThreadPriority.AboveNormal;
            ClientWiseThread.Start();
        }

        public void ProcessScripWiseTrade()
        {
            ThreadStart ScripWiseThreadStart = new ThreadStart(SWThread_Tick);
            ScripWiseThread = new Thread(ScripWiseThreadStart);
            ScripWiseThread.IsBackground = true;
            ScripWiseThread.Priority = ThreadPriority.AboveNormal;
            ScripWiseThread.Start();
        }

        #region SendTradeFeed
        public static void ProcessOnlineTrades()
        {
            ThreadStart SendTradeFeedStart = new ThreadStart(CommonFrontEnd.ViewModel.Trade.TradeFeedVM.AsynchronousTradeFeed.SendTradeToTradeFeed);
            SendTradeFeed = new Thread(SendTradeFeedStart);
            SendTradeFeed.IsBackground = true;
            SendTradeFeed.Priority = ThreadPriority.AboveNormal;
            SendTradeFeed.Start();
        }
        public static void CloseOnlineTrades()
        {
            SendTradeFeed.Abort();
        }
        #endregion
        void CWThread_Tick()
        {
            while (true)
            {
                int length = MemoryManager.TradeMemoryConDict.Count;

                if (MemoryManager.TradeMemoryConDict.Count > 0)
                {
                    for (int index = initialStartPointCW; index < length; index++)
                    {

                        lock (newCWobject)
                        {
                            CommonFrontEnd.Processor.UMSProcessor.ProcessNetPositionCWDemo(MemoryManager.TradeMemoryConDict[index]);
                        }
                    }
                    initialStartPointCW = length;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }

        }

        void SWThread_Tick()
        {
            while (true)
            {
                int length = MemoryManager.TradeMemoryConDict.Count;

                if (MemoryManager.TradeMemoryConDict.Count > 0)
                {
                    for (int index = initialStartPointSW; index < length; index++)
                    {

                        lock (newSWobject)
                        {
                            CommonFrontEnd.Processor.UMSProcessor.ProcessNetPositionSWDemo(MemoryManager.TradeMemoryConDict[index]);
                        }
                    }
                    initialStartPointSW = length;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }

        }
    }
    #endregion

    #region Calculate NetPosition Async


    public class NetPositionCalculateAsync
    {
        private static object newSaudasobjectAsync = new object();
        private static object newCWobjectAsync = new object();
        private static object newSWobjectAsync = new object();

        //[ThreadStatic]
        static int initialStartPointCWAsync = 0;

        //[ThreadStatic]
        static int initialStartPointSWAsync = 0;

        // [ThreadStatic]
        static int initialStartPointSaudasAsync = 0;

        Thread thSaudasPersonalDownload;
        Thread thSWPersonalDownload;
        Thread thCWPersonalDownload;
        Thread thSWOnline;
        Thread thCWOnline;
        Thread thSaudasPersonalOnline;
        //Thread tD;

        public void ProcessSaudasTradeAsyncPD()
        {
            ThreadStart thSaudasPersonalDownloadStart = new ThreadStart(() => SaudasThread_TickAsyncPD());
            thSaudasPersonalDownload = new Thread(thSaudasPersonalDownloadStart);
            thSaudasPersonalDownload.IsBackground = true;
            thSaudasPersonalDownload.Priority = ThreadPriority.AboveNormal;
            thSaudasPersonalDownload.Start();

            #region Commented


            //Task t1= Task.Factory.StartNew(() => SaudasThread_TickAsyncPD()).ContinueWith(;
            //t1.Wait();
            //if (t1.IsCompleted)
            //{
            //}


            /////IsLoading = true;

            //var UIScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            //Task.Factory.StartNew(() =>
            //{
            //    SaudasThread_TickAsyncPD();
            //}).ContinueWith(x =>
            //{
            //    CommonFrontEnd.ViewModel.TwsMainWindowVM.SaudasWindow_PdCompleted = true;
            //    if (CommonFrontEnd.ViewModel.TwsMainWindowVM.LoadManual_SaudasWindow)
            //    {
            //        CommonFrontEnd.ViewModel.TwsMainWindowVM.Saudas.Execute(null);

            //    }
            //}, UIScheduler);
            #endregion

        }
        public void ProcessSaudasTradeAsync()
        {
            ThreadStart thSaudasPersonalDownloadOnlineStart = new ThreadStart(() => SaudasThread_TickAsync());
            thSaudasPersonalOnline = new Thread(thSaudasPersonalDownloadOnlineStart);
            thSaudasPersonalOnline.IsBackground = true;
            thSaudasPersonalOnline.Priority = ThreadPriority.AboveNormal;
            thSaudasPersonalOnline.Start();

            // Task.Factory.StartNew(() => SaudasThread_TickAsync());
        }

        public void ProcessScripWiseTradeAsyncPD()
        {
            //Task T2 = Task.Factory.StartNew(() => SWThread_TickAsync());
            //return T2;
            ThreadStart thSWPersonalDownloadStart = new ThreadStart(() => SWThread_TickAsyncPD());
            thSWPersonalDownload = new Thread(thSWPersonalDownloadStart);
            thSWPersonalDownload.IsBackground = true;
            thSWPersonalDownload.Priority = ThreadPriority.AboveNormal;
            thSWPersonalDownload.Start();
        }

        public void ProcessClientWiseTradeAsyncPD()
        {
            ThreadStart thCWPersonalDownloadStart = new ThreadStart(() => CWThread_TickAsyncPD());
            thCWPersonalDownload = new Thread(thCWPersonalDownloadStart);
            thCWPersonalDownload.IsBackground = true;
            thCWPersonalDownload.Priority = ThreadPriority.AboveNormal;
            thCWPersonalDownload.Start();

        }

        public void ProcessScripWiseTradeAsyncOnline()
        {
            ThreadStart thSWOnlineStart = new ThreadStart(() => SWThread_TickAsyncOnline());
            thSWOnline = new Thread(thSWOnlineStart);
            thSWOnline.IsBackground = true;
            thSWOnline.Priority = ThreadPriority.AboveNormal;
            thSWOnline.Start();
        }

        public void ProcessClientWiseTradeAsyncOnline()
        {
            ThreadStart thCWOnlineStart = new ThreadStart(() => CWThread_TickAsyncOnline());
            thCWOnline = new Thread(thCWOnlineStart);
            thCWOnline.IsBackground = true;
            thCWOnline.Priority = ThreadPriority.AboveNormal;
            thCWOnline.Start();
        }

        public Task ProcessSWTradeDisplayAsync()
        {
            //ThreadStart thD = new ThreadStart(() => SWThread_TickAsyncPDDisplay());
            //tD = new Thread(thD);
            //tD.IsBackground = true;
            //tD.Priority = ThreadPriority.AboveNormal;
            //tD.Start();
            //     var tokenSource2 = new CancellationTokenSource();
            //CancellationToken ct = tokenSource2.Token;
            Task T = Task.Factory.StartNew(() => SWThread_TickAsyncPDDisplay());
            return T;

            //var tokenSource2 = new CancellationTokenSource();
            //CancellationToken ct = tokenSource2.Token;

            //var task = Task.Factory.StartNew(() =>
            //{

            //    // Were we already canceled?
            //    ct.ThrowIfCancellationRequested();
            //    SWThread_TickAsyncPDDisplay();

            //}, tokenSource2.Token); // Pass same token to StartNew.

            //tokenSource2.Cancel();


        }

        //TODO unused
        void CWThread_TickAsync()
        {
            while (true)
            {
                int length = MemoryManager.TradeMemoryConDict.Count;

                if (MemoryManager.TradeMemoryConDict.Count > 0)
                {
                    for (int index = initialStartPointCWAsync; index < length; index++)
                    {

                        lock (newCWobjectAsync)
                        {
                            CommonFrontEnd.Processor.UMSProcessor.ProcessNetPositionCWDemo(MemoryManager.TradeMemoryConDict[index]);
                        }
                    }
                    initialStartPointCWAsync = length;
                }
                else
                {
                    Task.Delay(1000);
                }
            }

        }



        #region Personal Download

        void CWThread_TickAsyncPD()
        {
            bool DisplayFlag = true;
            while (true)
            {
                int length = MemoryManager.EndOfDownloadCount;//MemoryManager.TradeMemoryConDict.Count;

                if (length == initialStartPointCWAsync && DisplayFlag)
                {
                    DisplayFlag = false;
                    NetPositionMemory.UpdateClientNetPosition("", MemoryManager.NetPositionCWDemoDict.ToList());
                    //TODO call CWSW for pd
                    NetPositionMemory.UpdateCWSWDNetPosition("", MemoryManager.NetPositionCWDemoDict.ToList());
                    ProcessClientWiseTradeAsyncOnline();
                    thCWPersonalDownload.Abort();
                }

                if (MemoryManager.TradeMemoryConDict.Count > 0 && length > initialStartPointCWAsync)
                {
                    ParallelOptions oParallelOptions = new ParallelOptions();
                    oParallelOptions.MaxDegreeOfParallelism = 1;

                    Parallel.For(initialStartPointCWAsync, length, oParallelOptions, (index) =>
                    {
                        lock (newCWobjectAsync)
                        {
                            if (MemoryManager.TradeMemoryConDict.ContainsKey(index))
                            {
                                CommonFrontEnd.Processor.UMSProcessor.ProcessNetPositionCWDemo(MemoryManager.TradeMemoryConDict[index]);
                            }
                        }
                    });

                    initialStartPointCWAsync = length;
                }
                else
                {

                    Thread.Sleep(1000);
                }
            }

        }

        void SWThread_TickAsyncPD()
        {
            bool DisplayFlag = true;
            while (true)
            {
                int length = MemoryManager.EndOfDownloadCount;//MemoryManager.TradeMemoryConDict.Count;

                if (length == initialStartPointSWAsync && DisplayFlag)
                {
                    DisplayFlag = false;
                    NetPositionMemory.UpdateScripNetPosition("", MemoryManager.NetPositionSWDemoDict.ToList());
                    ProcessSWTradeDisplayAsync().Wait();
                    ProcessScripWiseTradeAsyncOnline();
                    thSWPersonalDownload.Abort();
                }

                if (MemoryManager.TradeMemoryConDict.Count > 0 && length > initialStartPointSWAsync)
                {
                    ParallelOptions oParallelOptions = new ParallelOptions();
                    oParallelOptions.MaxDegreeOfParallelism = 1;

                    Parallel.For(initialStartPointSWAsync, length, oParallelOptions, (index) =>
                    {
                        lock (newSWobjectAsync)
                        {
                            if (MemoryManager.TradeMemoryConDict.ContainsKey(index))
                            {
                                CommonFrontEnd.Processor.UMSProcessor.ProcessNetPositionSWDemo(MemoryManager.TradeMemoryConDict[index]);
                                //TODO NP No Need
                                //CommonFrontEnd.Processor.UMSProcessor.ProcessNetPositionCWSWDetailsDemo(MemoryManager.TradeMemoryConDict[index]);
                                //CommonFrontEnd.Processor.UMSProcessor.ProcessNetPositionSWCWDetailsDemo(MemoryManager.TradeMemoryConDict[index]);
                            }
                        }
                    });

                    initialStartPointSWAsync = length;
                }
                else
                {

                    Thread.Sleep(1000);
                }
            }

        }

        void SaudasThread_TickAsyncPD()
        {
            try
            {
                NetPositionMemory.TraderTradeDataCollection = new System.Collections.ObjectModel.ObservableCollection<Model.Trade.TradeUMS>(MemoryManager.TradeMemoryConDict.Values.Cast<Model.Trade.TradeUMS>().ToList());

                //var data = MemoryManager.TradeMemoryConDict.Values.Cast<Model.Trade.TradeUMS>().ToList();
                //var count = data.Count;
                //for (int index = 0; index < count; index++)
                //{
                //    NetPositionMemory.TraderTradeDataCollection.Add(data[index]);

                //    if(index ==50)
                //    {
                //        CommonFrontEnd.ViewModel.TwsMainWindowVM.SaudasWindow_PdCompleted = true;
                //        if (CommonFrontEnd.ViewModel.TwsMainWindowVM.LoadManual_SaudasWindow)
                //        {
                //            CommonFrontEnd.ViewModel.TwsMainWindowVM.Saudas.Execute(null);

                //        }
                //    }
                //}

                UtilityTradeDetails.GetInstance.SaudasWindow_PdCompleted = true;
                if (UtilityTradeDetails.GetInstance.LoadManual_SaudasWindow)
                {
                    CommonFrontEnd.ViewModel.MainWindowVM.Saudas.Execute(null);

                }
                //thSaudasPersonalDownload.Abort();

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
                throw ex;
            }
            finally
            {
                //initialStartPointSaudasAsync = NetPositionMemory.TraderTradeDataCollection.Count;
                ProcessSaudasTradeAsync();
            }

            #region Commented


            //bool DisplayFlag = true;
            // while (true)
            // {
            //int length = MemoryManager.EndOfDownloadCount;//MemoryManager.TradeMemoryConDict.Count;

            // if (length == initialStartPointSaudasAsync && DisplayFlag)
            //{
            //  DisplayFlag = false;
            //NetPositionMemory.UpdateScripNetPosition("", MemoryManager.NetPositionSWDemoDict.ToList());
            //ProcessSWTradeDisplayAsync().Wait();
            //ProcessScripWiseTradeAsyncOnline();
            //thSaudasPersonalDownload.Abort();

            //thSaudasPersonalDownload.Abort();
            // }

            //if (MemoryManager.TradeMemoryConDict.Count > 0 && length > initialStartPointSaudasAsync)
            //{
            //    ParallelOptions oParallelOptions = new ParallelOptions();
            //    oParallelOptions.MaxDegreeOfParallelism = 20;

            //    Parallel.For(initialStartPointSaudasAsync, length, oParallelOptions, (index) =>
            //    {
            //        lock (newSaudasobjectAsync)
            //        {
            //            CommonFrontEnd.Processor.UMSProcessor.ProcessTraderTradeData(MemoryManager.TradeMemoryConDict[index]);
            //        }
            //    });

            //    initialStartPointSaudasAsync = length;
            //}
            //else
            //{

            //    Thread.Sleep(1000);
            //}
            //  }
            #endregion
        }

        void SWThread_TickAsyncPDDisplay()
        {

            if (MemoryManager.NetPositionSWDemoDict != null && MemoryManager.NetPositionSWDemoDict.Count > 0)
            {
                var results = MemoryManager.NetPositionSWDemoDict.GroupBy(p => ((CommonFrontEnd.Model.Trade.NetPosition)p.Value).ScripCode,
                          p => p.Value,
                          (key, g) => new
                          {
                              scripCode = key,
                              scripData = g.ToList()
                          }
                         );

                foreach (var item in results)
                {
                    NetPositionMemory.UpdateScripNetPosition(item.scripCode.ToString(), MemoryManager.NetPositionSWDemoDict.Where(x => ((CommonFrontEnd.Model.Trade.NetPosition)x.Value).ScripCode == item.scripCode).ToList());
                    //TODO call SWCW for pd
                    NetPositionMemory.UpdateScripNetPositionDetail(item.scripCode.ToString(), MemoryManager.NetPositionSWDemoDict.Where(x => ((CommonFrontEnd.Model.Trade.NetPosition)x.Value).ScripCode == item.scripCode).ToList());
                }
            }

        }

        #endregion



        #region Online Download

        void CWThread_TickAsyncOnline()
        {
            while (true)
            {
                int length = MemoryManager.TradeMemoryConDict.Count;

                //initialStartPointCWAsync = MemoryManager.EndOfDownloadCount;
                if (MemoryManager.TradeMemoryConDict.Count > 0 && length > initialStartPointCWAsync)
                {
                    ParallelOptions oParallelOptions = new ParallelOptions();
                    oParallelOptions.MaxDegreeOfParallelism = 1;

                    Parallel.For(initialStartPointCWAsync, length, oParallelOptions, (index) =>
                    {
                        lock (newCWobjectAsync)
                        {
                            if (MemoryManager.TradeMemoryConDict.ContainsKey(index))
                            {
                                CommonFrontEnd.Processor.UMSProcessor.ProcessNetPositionCWDemo(MemoryManager.TradeMemoryConDict[index]);
                                if (UtilityLoginDetails.GETInstance.Role.ToLower() == "trader")
                                {
                                    NetPositionMemory.UpdateClientNetPosition(((CommonFrontEnd.Model.Trade.TradeUMS)MemoryManager.TradeMemoryConDict[index]).Client.Trim(), MemoryManager.NetPositionCWDemoDict.Where(x => ((CommonFrontEnd.Model.Trade.NetPosition)x.Value).ClientId == ((CommonFrontEnd.Model.Trade.TradeUMS)MemoryManager.TradeMemoryConDict[index]).Client.Trim()).ToList());
                                    NetPositionMemory.UpdateCWSWDNetPosition(((CommonFrontEnd.Model.Trade.TradeUMS)MemoryManager.TradeMemoryConDict[index]).Client.Trim(), MemoryManager.NetPositionCWDemoDict.ToList());
                                }
                                else if (UtilityLoginDetails.GETInstance.Role.ToLower() == "admin")
                                {
                                    NetPositionMemory.UpdateClientNetPosition(((CommonFrontEnd.Model.Trade.TradeUMS)MemoryManager.TradeMemoryConDict[index]).TraderId.ToString(), MemoryManager.NetPositionCWDemoDict.Where(x => ((CommonFrontEnd.Model.Trade.NetPosition)x.Value).TraderId == ((CommonFrontEnd.Model.Trade.TradeUMS)MemoryManager.TradeMemoryConDict[index]).TraderId.ToString()).ToList());
                                    NetPositionMemory.UpdateCWSWDNetPosition(((CommonFrontEnd.Model.Trade.TradeUMS)MemoryManager.TradeMemoryConDict[index]).TraderId.ToString(), MemoryManager.NetPositionCWDemoDict.ToList());
                                }
                            }
                        }
                    });

                    initialStartPointCWAsync = length;
                }
                else
                {

                    Thread.Sleep(1000);
                }
            }

        }

        void SWThread_TickAsyncOnline()
        {
            while (true)
            {
                int length = MemoryManager.TradeMemoryConDict.Count;

                // initialStartPointSWAsync = MemoryManager.EndOfDownloadCount;
                if (MemoryManager.TradeMemoryConDict.Count > 0 && length > initialStartPointSWAsync)
                {
                    ParallelOptions oParallelOptions = new ParallelOptions();
                    oParallelOptions.MaxDegreeOfParallelism = 1;

                    Parallel.For(initialStartPointSWAsync, length, oParallelOptions, (index) =>
                    {
                        lock (newSWobjectAsync)
                        {
                            if (MemoryManager.TradeMemoryConDict.ContainsKey(index))
                            {
                                CommonFrontEnd.Processor.UMSProcessor.ProcessNetPositionSWDemo(MemoryManager.TradeMemoryConDict[index]);
                                NetPositionMemory.UpdateScripNetPosition(((CommonFrontEnd.Model.Trade.TradeUMS)MemoryManager.TradeMemoryConDict[index]).ScripCode.ToString(), MemoryManager.NetPositionSWDemoDict.Where(x => ((CommonFrontEnd.Model.Trade.NetPosition)x.Value).ScripCode == ((CommonFrontEnd.Model.Trade.TradeUMS)MemoryManager.TradeMemoryConDict[index]).ScripCode).ToList());

                                NetPositionMemory.UpdateScripNetPositionDetail(((CommonFrontEnd.Model.Trade.TradeUMS)MemoryManager.TradeMemoryConDict[index]).ScripCode.ToString(), MemoryManager.NetPositionSWDemoDict.Where(x => ((CommonFrontEnd.Model.Trade.NetPosition)x.Value).ScripCode == ((CommonFrontEnd.Model.Trade.TradeUMS)MemoryManager.TradeMemoryConDict[index]).ScripCode).ToList());
                            }
                        }
                    });

                    initialStartPointSWAsync = length;
                }
                else
                {

                    Thread.Sleep(1000);
                }
            }

        }

        void SaudasThread_TickAsync()
        {
            try
            {

                initialStartPointSaudasAsync = MemoryManager.TradeMemoryConDict.Count;
                while (true)
                {
                    int length = MemoryManager.TradeMemoryConDict.Count;

                    //initialStartPointSaudasAsync = MemoryManager.TradeMemoryConDict.Count;

                    if (MemoryManager.TradeMemoryConDict.Count > 0 && length > initialStartPointSaudasAsync)
                    {
                        ParallelOptions oParallelOptions = new ParallelOptions();
                        oParallelOptions.MaxDegreeOfParallelism = 1;

                        Parallel.For(initialStartPointSaudasAsync, length, oParallelOptions, (index) =>
                        {
                            lock (newSaudasobjectAsync)
                            {
                                if (MemoryManager.TradeMemoryConDict.ContainsKey(index))
                                {
                                    Model.Trade.TradeUMS objTrade = (Model.Trade.TradeUMS)MemoryManager.TradeMemoryConDict[index];

                                    //NetPositionMemory.TraderTradeDataCollection.Add(obj);
                                    NetPositionMemory.UpdateTraderTradeData("", objTrade);
                                }
                            }
                        });

                        initialStartPointSaudasAsync = length;
                    }
                    else
                    {
                        //Task.Delay(1000);
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
                throw ex;

            }
        }

        #endregion


    }

    #endregion
#endif
}
