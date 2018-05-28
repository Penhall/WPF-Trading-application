using CommonFrontEnd;
using CommonFrontEnd.SharedMemories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.Controller
{
#if TWS
    public class UMSController
    {
        public static System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Send);
        private static object newObject = new object();
        private Object newObject2 = new Object();
        public void ReceiveUMSMessage()
        {
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            timer.Tick += new EventHandler(timer_Tick);
            //timer.Start();
        }
        public void ReceiveUMSMessageAsync()
        {
            Task.Factory.StartNew(() => timer_TickAsync2());
        }
        void timer_Tick(object sender, EventArgs e)
        {
            byte[] recvData;
            for (int i = 0; i < 100; i++)
            {
                //ParallelOptions parallelOptions = new ParallelOptions
                //{
                //    MaxDegreeOfParallelism = 1
                //};
                //Parallel.For(0, MemoryManager.UmsQueue.Count, k =>
                //{
                if (MemoryManager.UmsQueue.Count > 0)
                {
                    recvData = MemoryManager.UmsQueue.Dequeue();
                    if (recvData != null)
                    {
                        lock (newObject)
                        {
                            MemoryManager.DecodeResponse(recvData);
                        }
                    }
                }
                //});
                else
                {
                    //Thread.Sleep(100);
                    break;
                }
            }

        }

        //void timer_TickAsync()
        //{
        //    byte[] recvData;
        //    while (true)
        //    {
        //        if (MemoryManager.UmsQueue.Count > 0)
        //        {
        //            recvData = MemoryManager.UmsQueue.Dequeue();
        //            if (recvData != null)
        //            {
        //                lock (newObject)
        //                {
        //                    MemoryManager.DecodeResponse(recvData);
        //                }
        //            }
        //        }

        //        else
        //        {
        //            Task.Delay(1000);
        //        }
        //    }

        //}

        void timer_TickAsync2()
        {
            byte[] recvData;
            ParallelOptions parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = 20
            };
            while (true)
            {

                Parallel.For(0, MemoryManager.UmsQueue.Count, parallelOptions, k =>
                {
                    if (MemoryManager.UmsQueue.Count > 0)
                    {
                        recvData = MemoryManager.UmsQueue.Dequeue();
                        if (recvData != null)
                        {
                            lock (newObject2)
                            {
                                MemoryManager.DecodeResponse(recvData);
                            }
                        }
                    }

                    else
                    {
                        Task.Delay(1);
                    }
                });
            }


        }
    }
#endif
}
