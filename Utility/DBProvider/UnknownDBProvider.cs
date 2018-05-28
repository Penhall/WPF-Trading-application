using CommonFrontEnd.Utility.Interfaces_Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.Utility.DBProvider
{
    public class UnknownDBProvider : IDBProvider
    {


        internal UnknownDBProvider()
        {
        }

        public bool CloseConnection()
        {
            return true;
        }


        public void ConnectTo(string pstrDataBasePathOrInstanceName, string pstrDataSource, string pstrDatabaseUserName = "", string pstrDatabasePassword = "")
        {
        }

        public System.Data.IDbConnection GetConnection
        {
            get { return null; }
        }

        public bool OpenConnection()
        {
            return false;
        }

        public System.Collections.Generic.List<string> GetListOfTables()
        {
            return null;
        }

        public string IsNull
        {
            get { return ""; }
        }

    }

}
