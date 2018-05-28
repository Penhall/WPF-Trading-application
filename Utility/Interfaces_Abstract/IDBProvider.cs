using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.Utility.Interfaces_Abstract
{
    public interface IDBProvider
    {
        string IsNull { get; }
        IDbConnection GetConnection { get; }
        void ConnectTo(string pstrDataBasePathOrInstanceName, string pstrDataSource, string pstrDatabaseUserName = "", string pstrDatabasePassword = "");
        bool OpenConnection();
        bool CloseConnection();
        System.Collections.Generic.List<string> GetListOfTables();
    }
}
