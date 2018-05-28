using CommonFrontEnd.SharedMemories;
using System;
using System.Linq;
using static CommonFrontEnd.SharedMemories.MemoryManager;

namespace CommonFrontEnd.Controller
{
#if TWS
    public class ReceiverController
    {
        //1) Retreive the data from Receiver memory
        //2) Update the dictionary in FindingFreeMemorySlot (y/n)
        //3) 
        public static System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        public void ReceiveMessage()
        {
            AsynchronousClient.Receive(AsynchronousClient.sockXML);

            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }
        static void timer_Tick(object sender, EventArgs e)
        {
            byte[] recvData;
            for (int i = 0; i < 100; i++)
            {
                if (ReplyQueue.Count > 0)
                {
                    recvData = ReplyQueue.Dequeue();
                    MemoryManager.DecodeResponse(recvData);
                }
                else
                {
                    break;
                }
            }
        }

    }
#endif
}
