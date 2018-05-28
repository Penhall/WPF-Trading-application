using CommonFrontEnd.Common;
using CommonFrontEnd.Model;
using CommonFrontEnd.Model.ETIMessageStructure;
using CommonFrontEnd.SharedMemories;
using System;
using System.Linq;
using static CommonFrontEnd.ControllerModel.SenderControllerModel;
using static CommonFrontEnd.SharedMemories.MemoryManager;

namespace CommonFrontEnd.Controller
{
#if TWS
    /// <summary>
    /// SenderController single instance created 22/3/2018 by Gaurav Jadhav
    /// </summary>
    public class SenderController:BaseController
    {
        private static SenderController oSenderController;
        public static SenderController GetInstance
        {
            get
            {
                if (oSenderController == null)
                {
                    oSenderController = new SenderController();
                }
                return oSenderController;
            }
        }
        #region Commented


        //public static Socket sockIML;
        //private byte[] requestHeaderBytes;
        //private byte[] requestBodyBytes;
        ////private byte[] repeatBodyBytes;
        //private byte[] initializeBytes;
        //private int slotNumber;
        //public static System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        //1) Formation of message structure
        //2) Sending it to sender shared memory
        //3) CommonFrontEnd.ControllerModel.SenderControllerModel.MessagesMessage msgStructure
        //public int CheckAvailableMemory<T>(T oModel) where T : class
        //{
        //    slotNumber = MemoryManager.FindingFreeMemory();

        //    if (!new[] { 1001, 1002 }.Any(x => x == slotNumber))
        //    {
        //        SenderController oSenderController = new SenderController();
        //        Processor.LoginProcessor.MapMessageTag(ref oModel);
        //        oSenderController.EncodeMessage(oModel, slotNumber);
        //    }
        //    return slotNumber;
        //}
        //public void EncodeMessage<T>(T oModel, int slotNumber) where T : class
        //{
        //    try
        //    {
        //        long messageType = 0;

        //        uint msgHeaderSize = 0;
        //        uint msgRequestSize = 0;
        //        //uint msgRepeatSize = 0;

        //        string modelName = string.Empty;

        //        EHeader oLogonHeader = new EHeader();

        //        modelName = oModel.GetType().Name;

        //        messageType = ConfigurationMasterMemory.RequestDict.Where(x => x.Value.Name.ToLower() == modelName.ToLower()).FirstOrDefault().Key;

        //        MessagesMessage msgHeader = ConfigurationMasterMemory.RequestDict[50];

        //        MessagesMessage requestMsgBody = ConfigurationMasterMemory.RequestDict[messageType];

        //        //if (requestMsgBody.Items.Any(x => x.Source == "R"))
        //        //{
        //        //    CommonFrontEnd.ControllerModel.SenderControllerModel.MessagesMessage repeatMsgBody = CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.RepeatDict[messageType];
        //        //    msgRepeatSize = Convert.ToUInt16(repeatMsgBody.Items.Sum(x => x.length));
        //        //    var NoOfEntries = oModel.GetType().GetProperty("NoQuoteEntries").GetValue(oModel);
        //        //    for (int i = 0; i < length; i++)
        //        //    {

        //        //    }
        //        //}
        //        msgRequestSize = Convert.ToUInt16(requestMsgBody.Items.Sum(x => x.Length));

        //        msgHeaderSize = Convert.ToUInt16(msgHeader.Items.Sum(x => x.Length));

        //        //msgRequestSize = msgRequestSize + msgRepeatSize;

        //        oLogonHeader.MessageType = Convert.ToUInt32(messageType);//Convert.ToUInt16(oModel.GetType().GetProperty("MessageType").GetValue(oModel))
        //        oLogonHeader.BodyLength = (msgHeaderSize + msgRequestSize) - 8;//subtract header size of 8 bytes as per IML
        //        if (messageType == 7)//UserRegistrationRequest
        //        {
        //            oLogonHeader.SlotNo = -1;
        //        }
        //        else
        //        {
        //            oLogonHeader.SlotNo = Convert.ToUInt16(slotNumber);
        //        }



        //        requestHeaderBytes = Converter.GetMessageInBytes(oLogonHeader, msgHeader);
        //        requestBodyBytes = Converter.GetMessageInBytes(oModel, requestMsgBody);

        //        initializeBytes = new byte[msgRequestSize + msgHeaderSize];
        //        Array.Copy(requestHeaderBytes, 0, initializeBytes, 0, requestHeaderBytes.Length);
        //        Array.Copy(requestBodyBytes, 0, initializeBytes, requestHeaderBytes.Length, requestBodyBytes.Length);


        //        MemoryManager.RequestDict.Add(slotNumber, initializeBytes);
        //        //MemoryManager.RequestQueue.Enqueue(initializeBytes);
        //        SendMessage(initializeBytes);
        //        MemoryManager.UpdateRequestReplyMappingMemory(slotNumber, false, true);

        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionUtility.LogError(ex);
        //    }
        //    finally
        //    {
        //        //timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
        //        //timer.Tick += new EventHandler(InitializeSendMessageTicker);
        //        //timer.Start();
        //    }
        //}
        //public static int SendMessage()
        //{
        //    int result = 0;
        //    try
        //    {
        //        CommonFrontEnd.SharedMemories.MemoryManager.AsynchronousClient.Send(CommonFrontEnd.SharedMemories.MemoryManager.AsynchronousClient.sockXML, MemoryManager.RequestQueue.Dequeue());
        //        CommonFrontEnd.SharedMemories.MemoryManager.AsynchronousClient.sendDone.WaitOne();
        //        result = Convert.ToInt32(ConstantErrorMessages.ErrorCd1000);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionUtility.LogException(ex, "SenderController.SendMessage");
        //    }
        //    return result;
        //}
        //static void InitializeSendMessageTicker(object sender, EventArgs e)
        //{
        //    for (int i = 0; i < 5; i++) //Request Execution count
        //    {
        //        if (MemoryManager.RequestQueue != null && MemoryManager.RequestQueue.Count > 0)
        //        {
        //            SendMessage();
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }
        //}

        //public static void SendMessage(byte[] message)
        //{
        //    AsynchronousClient.Send(AsynchronousClient.sockXML, message);
        //    AsynchronousClient.sendDone.WaitOne();
        //}
        #endregion
    }
#endif
}
