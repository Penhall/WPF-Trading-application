using CommonFrontEnd.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.SharedMemories
{
   
        static class BroadcastMasterMemory
        {
            public static ConcurrentDictionary<int, ScripDetails> objScripDetailsConDict = new ConcurrentDictionary<int, ScripDetails>();
       
        public static Dictionary<int, IdicesDetailsMain> objIndexDetailsConDict = new Dictionary<int, IdicesDetailsMain>();

        public static Dictionary<int, IndexData> indicesDataDict = new Dictionary<int, IndexData>();

       
    }
    
}
