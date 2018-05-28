using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CommonFrontEnd.Global
{
    public class UtilityDbDetails
    {
        public enum DbEnum
        {
            Masters,
            Transactions,
            Settings
        }
        public static DbCollection[] GetDbConnections;
        public class DbCollection
        {
            public string ConnectionString { get; set; }
        }

        public UtilityDbDetails()
        {
            var values = Enum.GetValues(typeof(DbEnum));
            var length = values.Length;
            DbCollection[] oDbCollection = new DbCollection[length];
            foreach (var item in values)
            {
                var index = Convert.ToInt32(item);
                oDbCollection[index] = new DbCollection();
                var key = Enum.GetName(typeof(DbEnum), item);
                var dbName = ConfigurationManager.AppSettings[key];
                var connectionString = "Data Source=" + Environment.CurrentDirectory + "\\Database\\" + dbName + "Compress=True;";
                oDbCollection[index].ConnectionString = connectionString;
            }
            GetDbConnections = oDbCollection;
        }
        private static UtilityDbDetails oUtilityDbDetails;

        public static UtilityDbDetails GetInstance
        {
            get
            {
                if (oUtilityDbDetails == null)
                    oUtilityDbDetails = new UtilityDbDetails();
                return oUtilityDbDetails;
            }
        }

    }
}
