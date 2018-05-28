using CommonFrontEnd;
using CommonFrontEnd.Common;
using CommonFrontEnd.Utility.Entity;
using CommonFrontEnd.Utility.Interfaces_Abstract;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.Utility.Factory
{
#if BOW
    [Serializable()]
    public class GetScripFactory
    {

#region " Prototype Pattern"
        //: Used Prottype to reduce the creation time
        // Key ,IScript
        private static Dictionary<string, IScript> Prototype_IScrips = new Dictionary<string, IScript>();
        // Key > Property Name ,Property Info
        private static Dictionary<string, Dictionary<string, System.Reflection.PropertyInfo>> Prototype_ExchangeColumnMap = new Dictionary<string, Dictionary<string, PropertyInfo>>();
        //Rem Could pass in a different DBProvider or could take this call outside to mdi and take the string from the UtilityLoginDetails.GETInstance.confi file.
        private static IDBProvider CurrentDBProvider = DBProviderFactory.CreateInstance();

        private static List<string> TablesPresent;
        static GetScripFactory()
        {
            IScript lobjScrip = default(IScript);
            System.Reflection.PropertyInfo[] lobjProperties = null;
            Dictionary<string, PropertyInfo> lobjPropertiesMap = default(Dictionary<string, PropertyInfo>);
            ExchangeColumnName_Map lobjExchangeColumnMap = default(ExchangeColumnName_Map);
            IEnumerator<string> lIenum = ColumnNameFactory.GetExchangeColumnMapKeysEnum();
            while (lIenum.MoveNext())
            {
                lobjPropertiesMap = new Dictionary<string, PropertyInfo>();
                lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(lIenum.Current);
                lobjScrip = IScripFactory.CreateInstance(lobjExchangeColumnMap.ClassType);
                lobjProperties = lobjScrip.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                for (Int16 lintTemp = 0; lintTemp <= lobjProperties.Count() - 1; lintTemp++)
                {
                    lobjPropertiesMap.Add(lobjProperties[lintTemp].Name.ToUpper(), lobjProperties[lintTemp]);
                }
                Prototype_IScrips.Add(lIenum.Current, lobjScrip);
                Prototype_ExchangeColumnMap.Add(lIenum.Current, lobjPropertiesMap);
            }

            CurrentDBProvider.ConnectTo(AppDomain.CurrentDomain.BaseDirectory, BowConstants.DATABASE_MASTERS);
        }

        /// <summary>
        /// A new instance of Type IScrip, based on the Exchange and Market passed in
        /// </summary>
        /// <param name="pintExchangeID"></param>
        /// <param name="pintMarketID"></param>
        /// <returns>A new instance of Type IScrip</returns>
        /// <remarks>Use this method to reduce creation time of instances of IScrip</remarks>
        static internal IScript GetNewIScripObjectFor(int pintExchangeID, int pintMarketID)
        {
            return (IScript)Prototype_IScrips.Where(x=>x.Key==ExchangeColumnName_Map.GenerateKey(pintExchangeID, pintMarketID)).Select(x=>x.Value).FirstOrDefault().Clone();
        }
        /// <summary>
        /// Collection of Properties supported by the instance of IScrip pertaining to the Exchange and Market passed in.
        /// </summary>
        /// <param name="pintExchangeID"></param>
        /// <param name="pintMarketID"></param>
        /// <returns>Dictionary of Properties in IScrip instance corresponding to the exchange and market passed in. Key = Name of the Property. Value = Instance of System.Reflection.PropertyInfo</returns>
        /// <remarks></remarks>
        private static Dictionary<string, System.Reflection.PropertyInfo> GetPropertyMap(int pintExchangeID, int pintMarketID)
        {
            return Prototype_ExchangeColumnMap.Where(x=>x.Key==ExchangeColumnName_Map.GenerateKey(pintExchangeID, pintMarketID)).Select(x=>x.Value).FirstOrDefault();
        }
#endregion

        //: Introduced locking only to stop concurrent database connection opening or closing.
        private static object mobjLockObject = new object();
        public static bool MastersDownloadInProcess;
        public static IDbConnection Connection
        {
            get { return CurrentDBProvider.GetConnection; }
        }
        public static bool OpenConnection()
        {
            bool lblSuccess = false;
            if (MastersDownloadInProcess)
                return false;
            lock (mobjLockObject)
            {
                lblSuccess = CurrentDBProvider.OpenConnection();
                if (lblSuccess == false)
                {
                    lblSuccess = CurrentDBProvider.OpenConnection();
                    //: Giving the Connection one more change to connect, as we are the one that is needy
                }
            }
            return lblSuccess;
        }

        public static bool CloseConnection()
        {
            lock (mobjLockObject)
            {
                return CurrentDBProvider.CloseConnection();
            }
        }

        public static void CacheTableNames()
        {
            TablesPresent = CurrentDBProvider.GetListOfTables();
        }

        //public static Constants.ProductInfo GetProductInfo()
        //{
        //    string lstrQuery = "Select Product from MasterInfo";
        //    IDbCommand lobjCommand = null;
        //    lock (mobjLockObject)
        //    {
        //        if (OpenConnection() && TablesPresent.Contains("MasterInfo"))
        //        {
        //            lobjCommand = CurrentDBProvider.GetConnection.CreateCommand();
        //            lobjCommand.CommandType = CommandType.Text;
        //            lobjCommand.CommandText = lstrQuery;
        //            IDataReader lobjIDataReader = null;
        //            try
        //            {
        //                lobjIDataReader = lobjCommand.ExecuteReader();
        //                if (lobjIDataReader.Read())
        //                {
        //                    foreach (Array current_loopVariable in Enum.GetValues(typeof(Constants.ProductInfo)))
        //                    {
                               
        //                        current = current_loopVariable;
        //                        if (lobjIDataReader["Product"] == current)
        //                        {
        //                            return (current);
        //                        }
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Infrastructure.Logger.WriteLog("Error while getting Product Info" + ex.Message.ToString(), true);
        //            }
        //            finally
        //            {
        //                if ((lobjIDataReader != null))
        //                {
        //                    lobjIDataReader.Close();
        //                    lobjIDataReader = null;
        //                }
        //            }
        //        }
        //    }
        //    return Constants.ProductInfo.None;
        //}

        public static IScript GetScrip(int pintExchangeID, int pintMarketID, string pstrWhereClause)
        {
            string lstrQuery = "";
            IDbCommand lobjCommand = null;
            bool lblnScripFound = false;
            try
            {
                IScript lobjScrip = GetNewIScripObjectFor(pintExchangeID, pintMarketID);
                Dictionary<string, PropertyInfo> lobjPropertiesMap = GetPropertyMap(pintExchangeID, pintMarketID);
                ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(pintExchangeID, pintMarketID);

                if (!string.IsNullOrEmpty(lobjExchangeColumnMap.TableName) && TablesPresent.Contains(lobjExchangeColumnMap.TableName))
                {
                    lstrQuery = "Select * From " + lobjExchangeColumnMap.TableName + " Where 1=1 ";
                    if (string.IsNullOrWhiteSpace(pstrWhereClause) == false)
                    {
                        lstrQuery += " And " + pstrWhereClause;
                    }

                    //HACK Introduced Locking only because the current version of SQLite dose not even supports concurrent select query. 
                    //TODO This lock should be removed when we upgrade our SQLite dll.
                    lock (mobjLockObject)
                    {
                        if (OpenConnection())
                        {
                            lobjCommand = CurrentDBProvider.GetConnection.CreateCommand();
                            lobjCommand.CommandType = CommandType.Text;
                            lobjCommand.CommandTimeout = 90;
                            lobjCommand.CommandText = lstrQuery;
                            IDataReader lobjIDataReader = null;
                            try
                            {
                                lobjIDataReader = lobjCommand.ExecuteReader();
                                if (lobjIDataReader.Read())
                                {
                                    lblnScripFound = true;
                                    IEnumerator lEnum = lobjExchangeColumnMap.ColumnName_Map.Keys.GetEnumerator();
                                    string lstrBaseFieldName = null;
                                    while (lEnum.MoveNext())
                                    {
                                        lstrBaseFieldName = Convert.ToString(lEnum.Current).ToUpper();
                                        if (lobjPropertiesMap.ContainsKey(lstrBaseFieldName))
                                        {
                                            lobjPropertiesMap.Where(x=>x.Key==lstrBaseFieldName).Select(x=>x.Value).FirstOrDefault().SetValue(lobjScrip,
                                                Convert.ChangeType(lobjIDataReader[lobjExchangeColumnMap.ColumnName_Map.Where(x => x.Key == lstrBaseFieldName).Select(x => x.Value).FirstOrDefault()],
                                                lobjPropertiesMap.Where(x => x.Key == lstrBaseFieldName).Select(x => x.Value).FirstOrDefault().PropertyType), null);
                                        }
                                    }

                                    lobjScrip.ExchangeId = pintExchangeID;
                                    lobjScrip.MarketId = pintMarketID;
                                }
                            }
                            catch (Exception ex)
                            {
                                Infrastructure.Logger.WriteLog("Error in GetScripFactory.GetScrip Inner Loop for Exchange=" + pintExchangeID.ToString() + " Market=" + pintMarketID.ToString() + " Query =" + lstrQuery + " Exception =" + ex.Message.ToString());
                            }
                            finally
                            {
                                if ((lobjIDataReader != null))
                                {
                                    lobjIDataReader.Close();
                                    lobjIDataReader = null;
                                }
                            }
                            if ((lobjScrip) is UnknownScrip || lblnScripFound == false)
                            {
                                return null;
                            }
                            else
                            {
                                return lobjScrip;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Infrastructure.Logger.WriteLog("Error in GetScripFactory.GetScrip for Exchange=" + pintExchangeID.ToString() + " Market=" + pintMarketID.ToString() + " Query =" + lstrQuery + " Exception =" + ex.Message.ToString());
                Infrastructure.Logger.WriteLog("Error in GetScripFactory.GetScrip . Stack " + ex.StackTrace.ToString());
            }
            return null;
        }

        //public static ScriptCollection GetScrips(int pintExchangeID, int pintMarketID, string pstrWhereClause,  ScriptCollection pobjScripCollection = null)
        //{
        //    string lstrQuery = "";
        //    IDbCommand lobjCommand = default(IDbCommand);

        //    try
        //    {
        //        IScript lobjScrip = default(IScript);
        //        string lstrBaseFieldName = null;
        //        IEnumerator lEnum = default(IEnumerator);
        //        Dictionary<string, PropertyInfo> lobjPropertiesMap = GetPropertyMap(pintExchangeID, pintMarketID);
        //        ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(pintExchangeID, pintMarketID);


        //        if (!string.IsNullOrEmpty(lobjExchangeColumnMap.TableName) && TablesPresent.Contains(lobjExchangeColumnMap.TableName))
        //        {
        //            if (pobjScripCollection == null)
        //            {
        //                pobjScripCollection = new ScriptCollection();
        //                //: Reusing the same variable
        //            }

        //            lstrQuery = "Select * From " + lobjExchangeColumnMap.TableName + " Where 1=1 ";
        //            if (string.IsNullOrWhiteSpace(pstrWhereClause) == false)
        //            {
        //                lstrQuery += " And " + pstrWhereClause;
        //            }

        //            //HACK Introduced Locking only because the current version of SQLite dose not even supports concurrent select query. 
        //            //TODO This lock should be removed when we upgrade our SQLite dll.
        //            lock (mobjLockObject)
        //            {
        //                if (OpenConnection())
        //                {
        //                    lobjCommand = CurrentDBProvider.GetConnection.CreateCommand();
        //                    lobjCommand.CommandType = CommandType.Text;
        //                    lobjCommand.CommandTimeout = 90;
        //                    lobjCommand.CommandText = lstrQuery;
        //                    IDataReader lobjIDataReader = null;
        //                    try
        //                    {
        //                        lobjIDataReader = lobjCommand.ExecuteReader();
        //                        while (lobjIDataReader.Read())
        //                        {
        //                            lobjScrip = GetNewIScripObjectFor(pintExchangeID, pintMarketID);
        //                            lEnum = lobjExchangeColumnMap.ColumnName_Map.Keys.GetEnumerator();
        //                            while (lEnum.MoveNext())
        //                            {
        //                                lstrBaseFieldName = Convert.ToString(lEnum.Current).ToUpper();
        //                                if (lobjPropertiesMap.ContainsKey(lstrBaseFieldName))
        //                                {
        //                                    lobjPropertiesMap.Where(x=>x.Key==lstrBaseFieldName).Select(x=>x.Value).FirstOrDefault().SetValue(lobjScrip, 
        //                                        Convert.ChangeType(lobjIDataReader[Convert.ToInt32(lobjExchangeColumnMap.ColumnName_Map.Where(x=>x.Key==lstrBaseFieldName).Select(x=>x.Value))],
        //                                        lobjPropertiesMap.Where(x => x.Key == lstrBaseFieldName).Select(x => x.Value).FirstOrDefault().PropertyType), null);
        //                                }
        //                            }

        //                            lobjScrip.ExchangeId = pintExchangeID;
        //                            lobjScrip.MarketId = pintMarketID;

        //                            pobjScripCollection.Add(lobjScrip);
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Infrastructure.Logger.WriteLog("Error in GetScripFactory.GetScrips Inner loop for Exchange=" + pintExchangeID.ToString() + " Market=" + pintMarketID.ToString() + " Query =" + lstrQuery + " Exception =" + ex.Message.ToString());
        //                    }
        //                    finally
        //                    {
        //                        if ((lobjIDataReader != null))
        //                        {
        //                            lobjIDataReader.Close();
        //                            lobjIDataReader = null;
        //                        }
        //                    }
        //                }
        //            }
        //            return pobjScripCollection;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Infrastructure.Logger.WriteLog("Error in GetScripFactory.GetScrips for Exchange=" + pintExchangeID.ToString() + " Market=" + pintMarketID.ToString() + " Query =" + lstrQuery + " Exception =" + ex.Message.ToString());
        //        Infrastructure.Logger.WriteLog("Error in GetScripFactory.GetScrips . Stack " + ex.StackTrace.ToString());
        //    }
        //    return null;
        //}

        public static string[] GetScrips(int pintExchangeID, int pintMarketID, string[] pstrBaseColumnName, string pstrWhereClause = null, string pstrQueryOpertaor = "")
        {
            System.Text.StringBuilder lobjQuery = new System.Text.StringBuilder();
            IDbCommand lobjCommand = default(IDbCommand);
            List<string> lobjDistinctValues = new List<string>();
            try
            {
                ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(pintExchangeID, pintMarketID);

                if (!string.IsNullOrEmpty(lobjExchangeColumnMap.TableName) && TablesPresent.Contains(lobjExchangeColumnMap.TableName))
                {
                    lobjQuery.Append("Select ");
                    lobjQuery.Append(pstrQueryOpertaor);
                    if (pstrBaseColumnName.Count() > 1)
                    {
                        for (int lintTemp = 0; lintTemp <= pstrBaseColumnName.Count() - 1; lintTemp++)
                        {
                            lobjQuery.Append(lobjExchangeColumnMap.ColumnName_Map.Where(x=>x.Key==pstrBaseColumnName[lintTemp]).Select(x=>x.Value));
                            lobjQuery.Append(",");
                        }
                    }
                    else
                    {
                        lobjQuery.Append(lobjExchangeColumnMap.ColumnName_Map.Where(x=>x.Key==pstrBaseColumnName[0]).Select(x=>x.Value));
                        lobjQuery.Append(",");
                    }
                    lobjQuery.Remove(lobjQuery.Length - 1, 1);
                    lobjQuery.Append(" From ");
                    lobjQuery.Append(lobjExchangeColumnMap.TableName);
                    lobjQuery.Append(" Where 1=1 ");

                    if (string.IsNullOrWhiteSpace(pstrWhereClause) == false)
                    {
                        lobjQuery.Append(" And ");
                        lobjQuery.Append(pstrWhereClause);
                    }
                    lobjQuery.Append(" Order By 1");
                    //HACK Introduced Locking only because the current version of SQLite dose not even supports concurrent select query. 
                    //TODO This lock should be removed when we upgrade our SQLite dll.
                    lock (mobjLockObject)
                    {
                        if (OpenConnection())
                        {
                            lobjCommand = CurrentDBProvider.GetConnection.CreateCommand();
                            lobjCommand.CommandType = CommandType.Text;
                            lobjCommand.CommandTimeout = 90;
                            lobjCommand.CommandText = lobjQuery.ToString();
                            IDataReader lobjIDataReader = null;
                            try
                            {
                                lobjIDataReader = lobjCommand.ExecuteReader();
                                System.Text.StringBuilder lstrValue = default(System.Text.StringBuilder);
                                while (lobjIDataReader.Read())
                                {
                                    lstrValue = new System.Text.StringBuilder();
                                    for (int lintTemp = 0; lintTemp <= pstrBaseColumnName.Count() - 1; lintTemp++)
                                    {
                                        lstrValue.Append(lobjIDataReader[lintTemp].ToString());
                                        lstrValue.Append("|");
                                    }
                                    lobjDistinctValues.Add(lstrValue.ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                Infrastructure.Logger.WriteLog("Error in GetScripFactory.GetDistinct inner loop for Exchange=" + pintExchangeID.ToString() + " Market=" + pintMarketID.ToString() + " Query =" + lobjQuery.ToString() + " Exception =" + ex.Message.ToString());
                            }
                            finally
                            {
                                if ((lobjIDataReader != null))
                                {
                                    lobjIDataReader.Close();
                                    lobjIDataReader = null;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Infrastructure.Logger.WriteLog("Error in GetScripFactory.GetDistinct for Exchange=" + pintExchangeID.ToString() + " Market=" + pintMarketID.ToString() + " Query =" + lobjQuery.ToString() + " Exception =" + ex.Message.ToString());
                Infrastructure.Logger.WriteLog("Error in GetScripFactory.GetDistinct . Stack " + ex.StackTrace.ToString());
            }
            return lobjDistinctValues.ToArray();
        }

        public static string[] GetDistinct(int pintExchangeID, int pintMarketID, string pstrBaseColumnName, bool pblnAllowBlank = false, string pstrWhereClause = null, string pstrDivideBy = "")
        {
            string lstrQuery = "";
            IDbCommand lobjCommand = default(IDbCommand);
            List<string> lobjDistinctValues = new List<string>();
            try
            {
                ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(pintExchangeID, pintMarketID);

                if (!string.IsNullOrEmpty(lobjExchangeColumnMap.TableName) && TablesPresent.Contains(lobjExchangeColumnMap.TableName))
                {
                    lstrQuery = "Select DISTINCT (" + lobjExchangeColumnMap.ColumnName_Map.Where(x => x.Value == pstrBaseColumnName.ToUpper()).Select(x => x.Value) + pstrDivideBy + ") From " + lobjExchangeColumnMap.TableName + " Where 1=1 ";
                    if (string.IsNullOrWhiteSpace(pstrWhereClause) == false)
                    {
                        lstrQuery += " And " + pstrWhereClause;
                    }
                    lstrQuery += " Order By " + lobjExchangeColumnMap.ColumnName_Map.Where(x => x.Value == pstrBaseColumnName.ToUpper()).Select(x=>x.Value);
                    //HACK Introduced Locking only because the current version of SQLite dose not even supports concurrent select query. 
                    //TODO This lock should be removed when we upgrade our SQLite dll.
                    lock (mobjLockObject)
                    {
                        if (OpenConnection())
                        {
                            lobjCommand = CurrentDBProvider.GetConnection.CreateCommand();
                            lobjCommand.CommandType = CommandType.Text;
                            lobjCommand.CommandTimeout = 90;
                            lobjCommand.CommandText = lstrQuery;
                            IDataReader lobjIDataReader = null;
                            try
                            {
                                lobjIDataReader = lobjCommand.ExecuteReader();
                                if (pblnAllowBlank)
                                {
                                    lobjDistinctValues.Add("");
                                }

                                while (lobjIDataReader.Read())
                                {
                                    lobjDistinctValues.Add(lobjIDataReader.GetString(0));
                                }
                            }
                            catch (Exception ex)
                            {
                                Infrastructure.Logger.WriteLog("Error in GetScripFactory.GetDistinct inner loop for Exchange=" + pintExchangeID.ToString() + " Market=" + pintMarketID.ToString() + " BaseColumn =" + pstrBaseColumnName + " Query =" + lstrQuery + " Exception =" + ex.Message.ToString());
                            }
                            finally
                            {
                                if ((lobjIDataReader != null))
                                {
                                    lobjIDataReader.Close();
                                    lobjIDataReader = null;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Infrastructure.Logger.WriteLog("Error in GetScripFactory.GetDistinct for Exchange=" + pintExchangeID.ToString() + " Market=" + pintMarketID.ToString() + " BaseColumn =" + pstrBaseColumnName + " Query =" + lstrQuery + " Exception =" + ex.Message.ToString());
                Infrastructure.Logger.WriteLog("Error in GetScripFactory.GetDistinct . Stack " + ex.StackTrace.ToString());
            }
            return lobjDistinctValues.ToArray();
        }

        public static long GetMax(int pintExchangeID, int pintMarketID, string pstrBaseColumnName, string pstrWhereClause = null)
        {
            string lstrQuery = "";
            IDbCommand lobjCommand = default(IDbCommand);
            List<string> lobjDistinctValues = new List<string>();
            try
            {
                ExchangeColumnName_Map lobjExchangeColumnMap = ColumnNameFactory.GetExchangeColumnMap(pintExchangeID, pintMarketID);

                if (!string.IsNullOrEmpty(lobjExchangeColumnMap.TableName) && TablesPresent.Contains(lobjExchangeColumnMap.TableName))
                {
                    lstrQuery = "Select " + CurrentDBProvider.IsNull + "( Max (" + lobjExchangeColumnMap.ColumnName_Map.Where(x=>x.Key==pstrBaseColumnName.ToUpper()).Select(x=>x.Value) + "),0) From " + lobjExchangeColumnMap.TableName + " Where 1=1 ";
                    if (string.IsNullOrWhiteSpace(pstrWhereClause) == false)
                    {
                        lstrQuery += " And " + pstrWhereClause;
                    }

                    //HACK Introduced Locking only because the current version of SQLite dose not even supports concurrent select query. 
                    //TODO This lock should be removed when we upgrade our SQLite dll.
                    lock (mobjLockObject)
                    {
                        if (OpenConnection())
                        {
                            lobjCommand = CurrentDBProvider.GetConnection.CreateCommand();
                            lobjCommand.CommandType = CommandType.Text;
                            lobjCommand.CommandTimeout = 90;
                            lobjCommand.CommandText = lstrQuery;
                            return Convert.ToInt64(lobjCommand.ExecuteScalar());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Infrastructure.Logger.WriteLog("Error in GetScripFactory.GetDistinct for Exchange=" + pintExchangeID.ToString() + " Market=" + pintMarketID.ToString() + " BaseColumn =" + pstrBaseColumnName + " Query =" + lstrQuery + " Exception =" + ex.Message.ToString());
                Infrastructure.Logger.WriteLog("Error in GetScripFactory.GetDistinct . Stack " + ex.StackTrace.ToString());
            }
            return 0;
        }

    }

#endif
}
