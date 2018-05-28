using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.Global
{
   
    public class UtilityConnParameters
    {

        //: Details for sending data to server
        private const string PARAMETER_THICK_CLIENT = "Thick Client";
        private const string PARAMETER_THICK_CLIENT_VALUE = "Y";

        private const string PARAMETER_LOGIN_KEY = "LoginKey";
        private string mstrServer = "";
        private string mstrProxyServer = "";
        private int mintProxyPort;
        private string mstrProxyUser = "";
        private string mstrProxyPassword = "";
        private string mstrLoginKeyValue = "";
        private bool mblnCompressData;
        private long mlngUserId;
        private static UtilityConnParameters mobjInstance;
        //: Do not delete this as we are creating a Singleton Object.
        private UtilityConnParameters()
        {
        }


        public static UtilityConnParameters GetInstance
        {
            get
            {
                if (mobjInstance == null)
                {
                    mobjInstance = new UtilityConnParameters();
                }
                return mobjInstance;
            }
        }

        public string ThickClientParameter
        {
            get { return PARAMETER_THICK_CLIENT; }
        }
        public string ThickClientValue
        {
            get { return PARAMETER_THICK_CLIENT_VALUE; }
        }
        public string LoginKeyName
        {
            get { return PARAMETER_LOGIN_KEY; }
        }
        public string LoginKeyValue
        {
            get { return mstrLoginKeyValue; }
            set { mstrLoginKeyValue = value; }
        }
        public bool CompressData
        {
            get { return mblnCompressData; }
            set { mblnCompressData = value; }
        }
        public string SERVER
        {
            get { return mstrServer; }
            set { mstrServer = value; }
        }
        public string PROXYSERVER
        {
            get { return mstrProxyServer; }
            set { mstrProxyServer = value; }
        }
        public int PROXYPORT
        {
            get { return mintProxyPort; }
            set { mintProxyPort = value; }
        }
        public string PROXYUSER
        {
            get { return mstrProxyUser; }
            set { mstrProxyUser = value; }
        }
        public string PROXYPASSWORD
        {
            get { return mstrProxyPassword; }
            set { mstrProxyPassword = value; }
        }
        public long UserId
        {
            get { return mlngUserId; }
            set { mlngUserId = value; }
        }
        public bool UserSecureHTTP { get; set; }
    }

}

