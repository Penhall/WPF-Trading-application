using CommonFrontEnd.ControllerModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.SharedMemories
{
    public static class ConfigurationMasterMemory
    {
        public static ConcurrentDictionary<string, object> ConfigurationDict = new ConcurrentDictionary<string, object>();

        //ETI 
        public static ConcurrentDictionary<long, SenderControllerModel.MessagesMessage> RequestDict = new ConcurrentDictionary<long, SenderControllerModel.MessagesMessage>();

        public static ConcurrentDictionary<long, SenderControllerModel.MessagesMessage> ReplyDict = new ConcurrentDictionary<long, SenderControllerModel.MessagesMessage>();

        public static ConcurrentDictionary<long, SenderControllerModel.MessagesMessage> UmsDict = new ConcurrentDictionary<long, SenderControllerModel.MessagesMessage>();

        public static ConcurrentDictionary<long, SenderControllerModel.MessagesMessage> RepeatDict = new ConcurrentDictionary<long, SenderControllerModel.MessagesMessage>();
    }
}
