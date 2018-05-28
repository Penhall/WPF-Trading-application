using CommonFrontEnd.Common;
using CommonFrontEnd.Model.ETIMessageStructure;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CommonFrontEnd.ControllerModel.SenderControllerModel;
using static CommonFrontEnd.SharedMemories.MemoryManager;

namespace CommonFrontEnd.Controller
{
    public class BaseController
    {
        private byte[] requestHeaderBytes;
        private byte[] requestBodyBytes;
        private byte[] initializeBytes;
        private int slotNumber;

        public int CheckAvailableMemory<T>(T oModel) where T : class
        {
            slotNumber = MemoryManager.FindingFreeMemory();

            if (!new[] { 1001, 1002 }.Any(x => x == slotNumber))
            {
                //SenderController oSenderController = new SenderController();
                MapMessageTag(ref oModel);
                EncodeMessage(oModel, slotNumber);
            }
            else
            {
                if (Convert.ToString(slotNumber) == Common.ConstantErrorMessages.ErrorCd1001)
                {
                    //System.Windows.MessageBox.Show(Common.ConstantErrorMessages.ErrorMessage1001);
                    CommonMessagingWindowVM.ProcessMiscellaneousMessages(Common.ConstantErrorMessages.ErrorMessage1001, "Others");
                }
                else if (Convert.ToString(slotNumber) == Common.ConstantErrorMessages.ErrorCd1002)
                {
                    //System.Windows.MessageBox.Show(Common.ConstantErrorMessages.ErrorMessage1002);
                    CommonMessagingWindowVM.ProcessMiscellaneousMessages(Common.ConstantErrorMessages.ErrorMessage1002, "Others");
                }
                
            }
            return slotNumber;
        }

        public static void MapMessageTag<T>(ref T oModel)
        {
            uint messageTag = 0;
            var modelName = oModel.GetType().Name;
            var messageType = SharedMemories.ConfigurationMasterMemory.RequestDict.Where(x => x.Value.Name.ToLower() == modelName.ToLower()).FirstOrDefault().Key;

            if (new[] { 1131, 50004, 1095, 1092, 1170, 1097, 1173, 2111, 22002, 22012, 1025 }.Any(x => x == messageType))
            {
                messageTag = Convert.ToUInt32(oModel.GetType().GetProperty("MessageTag").GetValue(oModel));
            }
            else
            {
                messageTag = SharedMemories.MemoryManager.GetMesageTag();
            }


            oModel.GetType().GetProperty("MessageTag").SetValue(oModel, messageTag);

            SharedMemories.MemoryManager.MessageTagResponseMappingDict.AddOrUpdate(messageTag, messageType, (key, Oldval) => messageType);
        }

        public void EncodeMessage<T>(T oModel, int slotNumber) where T : class
        {
            try
            {
                long messageType = 0;

                uint msgHeaderSize = 0;
                uint msgRequestSize = 0;
                //uint msgRepeatSize = 0;

                string modelName = string.Empty;

                EHeader oLogonHeader = new EHeader();

                modelName = oModel.GetType().Name;

                messageType = ConfigurationMasterMemory.RequestDict.Where(x => x.Value.Name.ToLower() == modelName.ToLower()).FirstOrDefault().Key;

                MessagesMessage msgHeader = ConfigurationMasterMemory.RequestDict[50];

                MessagesMessage requestMsgBody = ConfigurationMasterMemory.RequestDict[messageType];

                //if (requestMsgBody.Items.Any(x => x.Source == "R"))
                //{
                //    CommonFrontEnd.ControllerModel.SenderControllerModel.MessagesMessage repeatMsgBody = CommonFrontEnd.SharedMemories.ConfigurationMasterMemory.RepeatDict[messageType];
                //    msgRepeatSize = Convert.ToUInt16(repeatMsgBody.Items.Sum(x => x.length));
                //    var NoOfEntries = oModel.GetType().GetProperty("NoQuoteEntries").GetValue(oModel);
                //    for (int i = 0; i < length; i++)
                //    {

                //    }
                //}
                msgRequestSize = Convert.ToUInt16(requestMsgBody.Items.Sum(x => x.Length));

                msgHeaderSize = Convert.ToUInt16(msgHeader.Items.Sum(x => x.Length));

                //msgRequestSize = msgRequestSize + msgRepeatSize;

                oLogonHeader.MessageType = Convert.ToUInt32(messageType);//Convert.ToUInt16(oModel.GetType().GetProperty("MessageType").GetValue(oModel))
                oLogonHeader.BodyLength = (msgHeaderSize + msgRequestSize) - 8;//subtract header size of 8 bytes as per IML
                if (messageType == 7)//UserRegistrationRequest
                {
                    oLogonHeader.SlotNo = -1;
                }
                else
                {
                    oLogonHeader.SlotNo = Convert.ToUInt16(slotNumber);
                }



                requestHeaderBytes = Converter.GetMessageInBytes(oLogonHeader, msgHeader);
                requestBodyBytes = Converter.GetMessageInBytes(oModel, requestMsgBody);

                initializeBytes = new byte[msgRequestSize + msgHeaderSize];
                Array.Copy(requestHeaderBytes, 0, initializeBytes, 0, requestHeaderBytes.Length);
                Array.Copy(requestBodyBytes, 0, initializeBytes, requestHeaderBytes.Length, requestBodyBytes.Length);


                //MemoryManager.RequestDict.Add(slotNumber, initializeBytes);
                //MemoryManager.RequestQueue.Enqueue(initializeBytes);
                SendMessage(initializeBytes);
                MemoryManager.UpdateRequestReplyMappingMemory(slotNumber, false, true);

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
            }
            finally
            {
                //timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
                //timer.Tick += new EventHandler(InitializeSendMessageTicker);
                //timer.Start();
            }

        }
        public static void SendMessage(byte[] message)
        {
            AsynchronousClient.Send(AsynchronousClient.sockXML, message);
            AsynchronousClient.sendDone.WaitOne();
        }
    }
}
