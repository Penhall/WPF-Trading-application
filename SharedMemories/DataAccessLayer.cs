using System;
using System.Data;
using System.Collections;
using System.Globalization;
using System.Text;
using System.IO;

using System.Configuration;
using System.Data.SQLite;
using CommonFrontEnd.Common;

namespace CommonFrontEnd.SharedMemories
{
    public class DataAccessLayer : IDisposable
    {
        public enum ConnectionDB
        {
            Masters = 0,
            Setting = 1,
            Transactions = 2,
            TraderEntitlement = 3
        }
        static string szConnStrMasters = "";
        static string szConnStrSettings = "";
        static string szConnStrTransactions = "";
        static string szConnStrTraderEntitlement = "";

        private SQLiteConnection ConnMasters = null;
        private SQLiteConnection ConnSetting = null;
        private SQLiteConnection ConnTransactions = null;
        private SQLiteConnection ConnTraderEntitlement = null;

        static DataAccessLayer()
        {
            string pstrDataSource = "MasterCFE";
            szConnStrMasters = "Data Source=" + Environment.CurrentDirectory + "\\Database\\" + pstrDataSource + ".db;Compress=True;Pooling=True;";

            pstrDataSource = "Settings";
            szConnStrSettings = "Data Source=" + Environment.CurrentDirectory + "\\Database\\" + pstrDataSource + ".db;Compress=True;Pooling=True;";

            pstrDataSource = "Transactions";
            szConnStrTransactions = "Data Source=" + Environment.CurrentDirectory + "\\Database\\" + pstrDataSource + ".db;Compress=True;Pooling=True;";

            pstrDataSource = "TraderEntitlement";
            szConnStrTraderEntitlement = "Data Source=" + Environment.CurrentDirectory + "\\Database\\" + pstrDataSource + ".db;Compress=True;Pooling=True;";
        }

        public DataAccessLayer()
        {

        }


        SQLiteTransaction oSqlTransaction = null;

        /// <summary>
        /// BeginTransaction
        /// </summary>
        /// <param name="param">0,1,2</param>
        /// <returns></returns>
        public SQLiteTransaction BeginTransaction(int param)
        {
            try
            {
                switch (param)
                {
                    case (int)ConnectionDB.Masters:
                        if (ConnMasters == null)
                            ConnMasters = new SQLiteConnection(szConnStrMasters);

                        if (oSqlTransaction == null)
                            oSqlTransaction = ConnMasters.BeginTransaction(IsolationLevel.ReadUncommitted);
                        break;
                    case (int)ConnectionDB.Setting:
                        if (ConnSetting == null)
                            ConnSetting = new SQLiteConnection(szConnStrSettings);

                        if (oSqlTransaction == null)
                            oSqlTransaction = ConnSetting.BeginTransaction(IsolationLevel.ReadUncommitted);
                        break;
                    case (int)ConnectionDB.Transactions:
                        if (ConnTransactions == null)
                            ConnTransactions = new SQLiteConnection(szConnStrTransactions);

                        if (oSqlTransaction == null)
                            oSqlTransaction = ConnTransactions.BeginTransaction(IsolationLevel.ReadUncommitted);
                        break;
                    case (int)ConnectionDB.TraderEntitlement:
                        if (ConnTraderEntitlement == null)
                            ConnTraderEntitlement = new SQLiteConnection(szConnStrTraderEntitlement);

                        if (oSqlTransaction == null)
                            oSqlTransaction = ConnTraderEntitlement.BeginTransaction(IsolationLevel.ReadUncommitted);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oSqlTransaction;
        }

        public bool OpenConnection(int param)
        {
            bool bSuccess = false;
            try
            {
                switch (param)
                {
                    case (int)ConnectionDB.Masters:
                        if (ConnMasters == null)
                            ConnMasters = new SQLiteConnection(szConnStrMasters);

                        if (ConnMasters.State != ConnectionState.Open)
                        {
                            ConnMasters.Open();
                            bSuccess = true;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                        break;
                    case (int)ConnectionDB.Setting:
                        if (ConnSetting == null)
                            ConnSetting = new SQLiteConnection(szConnStrSettings);

                        if (ConnSetting.State != ConnectionState.Open)
                        {
                            ConnSetting.Open();
                            bSuccess = true;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                        break;
                    case (int)ConnectionDB.Transactions:
                        if (ConnTransactions == null)
                            ConnTransactions = new SQLiteConnection(szConnStrTransactions);

                        if (ConnTransactions.State != ConnectionState.Open)
                        {
                            ConnTransactions.Open();
                            bSuccess = true;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                        break;
                    case (int)ConnectionDB.TraderEntitlement:
                        if (ConnTraderEntitlement == null)
                            ConnTraderEntitlement = new SQLiteConnection(szConnStrTraderEntitlement);

                        if (ConnTraderEntitlement.State != ConnectionState.Open)
                        {
                            ConnTraderEntitlement.Open();
                            bSuccess = true;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                bSuccess = false;
                throw ex;

            }
            return bSuccess;
        }

        public bool CloseConnection(int param)
        {
            bool bSuccess = false;
            try
            {
                switch (param)
                {
                    case (int)ConnectionDB.Masters:
                        if (ConnMasters.State != ConnectionState.Closed)
                        {
                            ConnMasters.Close();
                            oSqlTransaction = null;
                            bSuccess = true;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                        break;
                    case (int)ConnectionDB.Setting:
                        if (ConnSetting.State != ConnectionState.Closed)
                        {
                            ConnSetting.Close();
                            oSqlTransaction = null;
                            bSuccess = true;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                        break;
                    case (int)ConnectionDB.Transactions:
                        if (ConnTransactions.State != ConnectionState.Closed)
                        {
                            ConnTransactions.Close();
                            oSqlTransaction = null;
                            bSuccess = true;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                        break;
                    case (int)ConnectionDB.TraderEntitlement:
                        if (ConnTraderEntitlement.State != ConnectionState.Closed)
                        {
                            ConnTraderEntitlement.Close();
                            oSqlTransaction = null;
                            bSuccess = true;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                bSuccess = false;
                throw ex;

            }
            return bSuccess;
        }

        public bool CommitTransaction(int param)
        {
            bool bSuccess = false;
            try
            {
                switch (param)
                {
                    case (int)ConnectionDB.Masters:
                        if (oSqlTransaction != null)
                        {
                            oSqlTransaction.Commit();
                            oSqlTransaction = null;
                            bSuccess = true;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                        break;
                    case (int)ConnectionDB.Setting:
                        if (oSqlTransaction != null)
                        {
                            oSqlTransaction.Commit();
                            oSqlTransaction = null;
                            bSuccess = true;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                        break;
                    case (int)ConnectionDB.Transactions:
                        if (oSqlTransaction != null)
                        {
                            oSqlTransaction.Commit();
                            oSqlTransaction = null;
                            bSuccess = true;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                        break;
                    case (int)ConnectionDB.TraderEntitlement:
                        if (oSqlTransaction != null)
                        {
                            oSqlTransaction.Commit();
                            oSqlTransaction = null;
                            bSuccess = true;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                bSuccess = false;
                oSqlTransaction = null;
                throw ex;
            }
            return bSuccess;
        }

        public bool RollBackTransaction(int param)
        {
            bool bSuccess = false;
            try
            {
                switch (param)
                {
                    case (int)ConnectionDB.Masters:
                        if (oSqlTransaction != null)
                        {
                            oSqlTransaction.Rollback();
                            oSqlTransaction = null;
                            bSuccess = true;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                        break;
                    case (int)ConnectionDB.Setting:
                        if (oSqlTransaction != null)
                        {
                            oSqlTransaction.Rollback();
                            oSqlTransaction = null;
                            bSuccess = true;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                        break;
                    case (int)ConnectionDB.Transactions:
                        if (oSqlTransaction != null)
                        {
                            oSqlTransaction.Rollback();
                            oSqlTransaction = null;
                            bSuccess = true;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                        break;
                    case (int)ConnectionDB.TraderEntitlement:
                        if (oSqlTransaction != null)
                        {
                            oSqlTransaction.Rollback();
                            oSqlTransaction = null;
                            bSuccess = true;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                bSuccess = false;
                throw ex;

            }
            return bSuccess;
        }

        /// <summary>
        /// Executes Command text and returns true if succeed else returns false
        /// </summary>
        /// <param name="strCommandText">Gets Command text either as query string or as stored Procedure</param>
        /// <param name="SqlCommandType"></param>
        /// <param name="oSqlParameter"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(int param, string strCommandText, CommandType SqlCommandType, params SQLiteParameter[] oSqlParameter)
        {
            #region Commented


            //SQLiteCommand oSqlCommand;
            //int nReturn = 0;
            //try
            //{
            //    if (ConnMasters.State == ConnectionState.Open)
            //    {
            //        //Conn.Open();
            //        oSqlCommand = new SQLiteCommand();
            //        oSqlCommand.Connection = ConnMasters;
            //        oSqlCommand.CommandType = SqlCommandType;
            //        oSqlCommand.CommandText = strCommandText;
            //        oSqlCommand.Transaction = oSqlTransaction;
            //        if (oSqlParameter != null)
            //            oSqlCommand.Parameters.Add(oSqlParameter);

            //        nReturn = oSqlCommand.ExecuteNonQuery((int)ConnectionDB.Masters);
            //    }
            //    else
            //    {
            //        nReturn = -1;

            //    }
            //}
            //catch (Exception ex)
            //{
            //    nReturn = -1;
            //    throw ex;
            //}
            //return nReturn;
            #endregion

            SQLiteCommand oSqlCommand;
            int nReturn = 0;
            try
            {
                switch (param)
                {
                    case (int)ConnectionDB.Masters:
                        if (ConnMasters.State == ConnectionState.Open)
                        {
                            //Conn.Open();
                            oSqlCommand = new SQLiteCommand();
                            oSqlCommand.Connection = ConnMasters;
                            oSqlCommand.CommandType = SqlCommandType;
                            oSqlCommand.CommandText = strCommandText;
                            oSqlCommand.Transaction = oSqlTransaction;
                            if (oSqlParameter != null)
                                oSqlCommand.Parameters.Add(oSqlParameter);

                            nReturn = oSqlCommand.ExecuteNonQuery();
                        }
                        else
                        {
                            nReturn = -1;

                        }
                        break;
                    case (int)ConnectionDB.Setting:
                        if (ConnSetting.State == ConnectionState.Open)
                        {
                            //Conn.Open();
                            oSqlCommand = new SQLiteCommand();
                            oSqlCommand.Connection = ConnSetting;
                            oSqlCommand.CommandType = SqlCommandType;
                            oSqlCommand.CommandText = strCommandText;
                            oSqlCommand.Transaction = oSqlTransaction;
                            if (oSqlParameter != null)
                                oSqlCommand.Parameters.Add(oSqlParameter);

                            nReturn = oSqlCommand.ExecuteNonQuery();
                        }
                        else
                        {
                            nReturn = -1;

                        }
                        break;
                    case (int)ConnectionDB.Transactions:
                        if (ConnTransactions.State == ConnectionState.Open)
                        {
                            //Conn.Open();
                            oSqlCommand = new SQLiteCommand();
                            oSqlCommand.Connection = ConnTransactions;
                            oSqlCommand.CommandType = SqlCommandType;
                            oSqlCommand.CommandText = strCommandText;
                            oSqlCommand.Transaction = oSqlTransaction;
                            if (oSqlParameter != null)
                                oSqlCommand.Parameters.Add(oSqlParameter);

                            nReturn = oSqlCommand.ExecuteNonQuery();
                        }
                        else
                        {
                            nReturn = -1;

                        }
                        break;
                    case (int)ConnectionDB.TraderEntitlement:
                        if (ConnTraderEntitlement.State == ConnectionState.Open)
                        {
                            //Conn.Open();
                            oSqlCommand = new SQLiteCommand();
                            oSqlCommand.Connection = ConnTraderEntitlement;
                            oSqlCommand.CommandType = SqlCommandType;
                            oSqlCommand.CommandText = strCommandText;
                            oSqlCommand.Transaction = oSqlTransaction;
                            if (oSqlParameter != null)
                                oSqlCommand.Parameters.Add(oSqlParameter);

                            nReturn = oSqlCommand.ExecuteNonQuery();
                        }
                        else
                        {
                            nReturn = -1;

                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                nReturn = -1;
                ExceptionUtility.LogError(ex);
            }
            return nReturn;
        }

        public object ExecuteScalar(int param, string strQuery, CommandType SqlCommandType, params SQLiteParameter[] oSqlParameter)
        {
            SQLiteCommand oSqlCommand;
            object oReturn = null;

            #region Commented


            //try
            //{
            //    if (ConnMasters.State == ConnectionState.Open)
            //    {
            //        //Conn.Open();
            //        oSqlCommand = new SQLiteCommand();
            //        oSqlCommand.Connection = ConnMasters;
            //        oSqlCommand.CommandType = SqlCommandType;
            //        oSqlCommand.CommandText = strQuery;
            //        oSqlCommand.Transaction = oSqlTransaction;
            //        if (oSqlParameter != null)
            //            oSqlCommand.Parameters.Add(oSqlParameter);

            //        oReturn = oSqlCommand.ExecuteScalar();
            //    }
            //    else
            //    {
            //        oReturn = null;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    oReturn = null;
            //    throw ex;

            //}
            #endregion

            try
            {
                switch (param)
                {
                    case (int)ConnectionDB.Masters:
                        if (ConnMasters.State == ConnectionState.Open)
                        {
                            //Conn.Open();
                            oSqlCommand = new SQLiteCommand();
                            oSqlCommand.Connection = ConnMasters;
                            oSqlCommand.CommandType = SqlCommandType;
                            oSqlCommand.CommandText = strQuery;
                            oSqlCommand.Transaction = oSqlTransaction;
                            if (oSqlParameter != null)
                                oSqlCommand.Parameters.Add(oSqlParameter);

                            oReturn = oSqlCommand.ExecuteScalar();
                        }
                        else
                        {
                            oReturn = null;
                        }
                        break;
                    case (int)ConnectionDB.Setting:
                        if (ConnSetting.State == ConnectionState.Open)
                        {
                            //Conn.Open();
                            oSqlCommand = new SQLiteCommand();
                            oSqlCommand.Connection = ConnSetting;
                            oSqlCommand.CommandType = SqlCommandType;
                            oSqlCommand.CommandText = strQuery;
                            oSqlCommand.Transaction = oSqlTransaction;
                            if (oSqlParameter != null)
                                oSqlCommand.Parameters.Add(oSqlParameter);

                            oReturn = oSqlCommand.ExecuteScalar();
                        }
                        else
                        {
                            oReturn = null;
                        }
                        break;
                    case (int)ConnectionDB.Transactions:
                        if (ConnTransactions.State == ConnectionState.Open)
                        {
                            //Conn.Open();
                            oSqlCommand = new SQLiteCommand();
                            oSqlCommand.Connection = ConnTransactions;
                            oSqlCommand.CommandType = SqlCommandType;
                            oSqlCommand.CommandText = strQuery;
                            oSqlCommand.Transaction = oSqlTransaction;
                            if (oSqlParameter != null)
                                oSqlCommand.Parameters.Add(oSqlParameter);

                            oReturn = oSqlCommand.ExecuteScalar();
                        }
                        else
                        {
                            oReturn = null;
                        }
                        break;
                    case (int)ConnectionDB.TraderEntitlement:
                        if (ConnTraderEntitlement.State == ConnectionState.Open)
                        {
                            //Conn.Open();
                            oSqlCommand = new SQLiteCommand();
                            oSqlCommand.Connection = ConnTraderEntitlement;
                            oSqlCommand.CommandType = SqlCommandType;
                            oSqlCommand.CommandText = strQuery;
                            oSqlCommand.Transaction = oSqlTransaction;
                            if (oSqlParameter != null)
                                oSqlCommand.Parameters.Add(oSqlParameter);

                            oReturn = oSqlCommand.ExecuteScalar();
                        }
                        else
                        {
                            oReturn = null;
                        }
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                oReturn = null;
                ExceptionUtility.LogError(ex);
            }
            return oReturn;

        }

        public SQLiteDataReader ExecuteDataReader(int param, string strQuery, CommandType SqlCommandType, params SQLiteParameter[] oSqlParameter)
        {
            SQLiteDataReader oSqlDataReader = null;
            SQLiteCommand oSqlCommand;
            object oReturn = null;
            try
            {
                #region Commented


                //if (ConnMasters.State == ConnectionState.Open)
                //{
                //    //Conn.Open();
                //    oSqlCommand = new SQLiteCommand();
                //    oSqlCommand.Connection = ConnMasters;
                //    oSqlCommand.CommandType = SqlCommandType;
                //    oSqlCommand.CommandText = strQuery;
                //    oSqlCommand.Transaction = oSqlTransaction;
                //    if (oSqlParameter != null)
                //        oSqlCommand.Parameters.Add(oSqlParameter);

                //    oSqlDataReader = oSqlCommand.ExecuteReader();
                //}
                //else
                //{
                //    oSqlDataReader = null;
                //}
                #endregion

                switch (param)
                {
                    case (int)ConnectionDB.Masters:
                        if (ConnMasters.State == ConnectionState.Open)
                        {
                            //Conn.Open();
                            oSqlCommand = new SQLiteCommand();
                            oSqlCommand.Connection = ConnMasters;
                            oSqlCommand.CommandType = SqlCommandType;
                            oSqlCommand.CommandText = strQuery;
                            oSqlCommand.Transaction = oSqlTransaction;
                            if (oSqlParameter != null)
                                oSqlCommand.Parameters.Add(oSqlParameter);

                            oSqlDataReader = oSqlCommand.ExecuteReader();
                        }
                        else
                        {
                            oSqlDataReader = null;
                        }
                        break;
                    case (int)ConnectionDB.Setting:
                        if (ConnSetting.State == ConnectionState.Open)
                        {
                            //Conn.Open();
                            oSqlCommand = new SQLiteCommand();
                            oSqlCommand.Connection = ConnSetting;
                            oSqlCommand.CommandType = SqlCommandType;
                            oSqlCommand.CommandText = strQuery;
                            oSqlCommand.Transaction = oSqlTransaction;
                            if (oSqlParameter != null)
                                oSqlCommand.Parameters.Add(oSqlParameter);

                            oSqlDataReader = oSqlCommand.ExecuteReader();
                        }
                        else
                        {
                            oSqlDataReader = null;
                        }
                        break;
                    case (int)ConnectionDB.Transactions:
                        if (ConnTransactions.State == ConnectionState.Open)
                        {
                            //Conn.Open();
                            oSqlCommand = new SQLiteCommand();
                            oSqlCommand.Connection = ConnTransactions;
                            oSqlCommand.CommandType = SqlCommandType;
                            oSqlCommand.CommandText = strQuery;
                            oSqlCommand.Transaction = oSqlTransaction;
                            if (oSqlParameter != null)
                                oSqlCommand.Parameters.Add(oSqlParameter);

                            oSqlDataReader = oSqlCommand.ExecuteReader();
                        }
                        else
                        {
                            oSqlDataReader = null;
                        }
                        break;
                    case (int)ConnectionDB.TraderEntitlement:
                        if (ConnTraderEntitlement.State == ConnectionState.Open)
                        {
                            //Conn.Open();
                            oSqlCommand = new SQLiteCommand();
                            oSqlCommand.Connection = ConnTraderEntitlement;
                            oSqlCommand.CommandType = SqlCommandType;
                            oSqlCommand.CommandText = strQuery;
                            oSqlCommand.Transaction = oSqlTransaction;
                            if (oSqlParameter != null)
                                oSqlCommand.Parameters.Add(oSqlParameter);

                            oSqlDataReader = oSqlCommand.ExecuteReader();
                        }
                        else
                        {
                            oSqlDataReader = null;
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                oSqlDataReader = null;
                throw ex;
            }
            return oSqlDataReader;

        }

        public DataSet ExecuteDataSet(int param,string strQuery, CommandType SqlCommandType, params SQLiteParameter[] oSqlParameter)
        {
            DataSet oDataSet = null;
            SQLiteDataAdapter oSqlDataAdapter = null;
            SQLiteCommand oSqlCommand;
            object oReturn = null;
            try
            {
                #region Commented
                //if (ConnMasters.State == ConnectionState.Open)
                //{
                //    //Conn.Open();
                //    oSqlCommand = new SQLiteCommand();
                //    oSqlCommand.Connection = ConnMasters;
                //    oSqlCommand.CommandType = SqlCommandType;
                //    oSqlCommand.CommandText = strQuery;
                //    oSqlCommand.Transaction = oSqlTransaction;
                //    if (oSqlParameter != null)
                //        oSqlCommand.Parameters.Add(oSqlParameter);

                //    oSqlDataAdapter = new SQLiteDataAdapter(oSqlCommand);
                //    oDataSet = new DataSet();
                //    oSqlDataAdapter.Fill(oDataSet);
                //}
                //else
                //{
                //    oDataSet = null;
                //}
                #endregion

                switch (param)
                {
                    case (int)ConnectionDB.Masters:
                        if (ConnMasters.State == ConnectionState.Open)
                        {
                            //Conn.Open();
                            oSqlCommand = new SQLiteCommand();
                            oSqlCommand.Connection = ConnMasters;
                            oSqlCommand.CommandType = SqlCommandType;
                            oSqlCommand.CommandText = strQuery;
                            oSqlCommand.Transaction = oSqlTransaction;
                            if (oSqlParameter != null)
                                oSqlCommand.Parameters.Add(oSqlParameter);

                            oSqlDataAdapter = new SQLiteDataAdapter(oSqlCommand);
                            oDataSet = new DataSet();
                            oSqlDataAdapter.Fill(oDataSet);
                        }
                        else
                        {
                            oDataSet = null;
                        }
                        break;
                    case (int)ConnectionDB.Setting:
                        if (ConnSetting.State == ConnectionState.Open)
                        {
                            //Conn.Open();
                            oSqlCommand = new SQLiteCommand();
                            oSqlCommand.Connection = ConnSetting;
                            oSqlCommand.CommandType = SqlCommandType;
                            oSqlCommand.CommandText = strQuery;
                            oSqlCommand.Transaction = oSqlTransaction;
                            if (oSqlParameter != null)
                                oSqlCommand.Parameters.Add(oSqlParameter);

                            oSqlDataAdapter = new SQLiteDataAdapter(oSqlCommand);
                            oDataSet = new DataSet();
                            oSqlDataAdapter.Fill(oDataSet);
                        }
                        else
                        {
                            oDataSet = null;
                        }
                        break;
                    case (int)ConnectionDB.Transactions:
                        if (ConnTransactions.State == ConnectionState.Open)
                        {
                            //Conn.Open();
                            oSqlCommand = new SQLiteCommand();
                            oSqlCommand.Connection = ConnTransactions;
                            oSqlCommand.CommandType = SqlCommandType;
                            oSqlCommand.CommandText = strQuery;
                            oSqlCommand.Transaction = oSqlTransaction;
                            if (oSqlParameter != null)
                                oSqlCommand.Parameters.Add(oSqlParameter);

                            oSqlDataAdapter = new SQLiteDataAdapter(oSqlCommand);
                            oDataSet = new DataSet();
                            oSqlDataAdapter.Fill(oDataSet);
                        }
                        else
                        {
                            oDataSet = null;
                        }
                        break;
                    case (int)ConnectionDB.TraderEntitlement:
                        if (ConnTraderEntitlement.State == ConnectionState.Open)
                        {
                            //Conn.Open();
                            oSqlCommand = new SQLiteCommand();
                            oSqlCommand.Connection = ConnTraderEntitlement;
                            oSqlCommand.CommandType = SqlCommandType;
                            oSqlCommand.CommandText = strQuery;
                            oSqlCommand.Transaction = oSqlTransaction;
                            if (oSqlParameter != null)
                                oSqlCommand.Parameters.Add(oSqlParameter);

                            oSqlDataAdapter = new SQLiteDataAdapter(oSqlCommand);
                            oDataSet = new DataSet();
                            oSqlDataAdapter.Fill(oDataSet);
                        }
                        else
                        {
                            oDataSet = null;
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                oDataSet = null;
                throw ex;
            }
            return oDataSet;

        }

        public bool Connect(int param)
        {
            //string pstrDataSource = "MasterCFE";//ConfigurationManager.AppSettings["DatabaseName"]?.ToString();
            bool bError = false;
            switch (param)
            {
                case (int)ConnectionDB.Masters:
                    try
                    {
                        ConnMasters = new SQLiteConnection(szConnStrMasters);
                        ConnMasters.Open();
                        bError = true;
                    }
                    catch (SQLiteException ex)
                    {
                        bError = false;
                        ConnMasters = null;
                        throw ex;
                    }
                    catch (System.Exception ex)
                    {
                        ConnMasters = null;
                        bError = false;
                        throw ex;
                    }

                    finally
                    {

                    }
                    break;
                case (int)ConnectionDB.Setting:
                    try
                    {
                        ConnSetting = new SQLiteConnection(szConnStrSettings);
                        ConnSetting.Open();
                        bError = true;
                    }
                    catch (SQLiteException ex)
                    {
                        bError = false;
                        ConnSetting = null;
                        throw ex;
                    }
                    catch (System.Exception ex)
                    {
                        ConnSetting = null;
                        bError = false;
                        throw ex;
                    }

                    finally
                    {

                    }
                    break;
                case (int)ConnectionDB.Transactions:
                    try
                    {
                        ConnTransactions = new SQLiteConnection(szConnStrTransactions);
                        ConnTransactions.Open();
                        bError = true;
                    }
                    catch (SQLiteException ex)
                    {
                        bError = false;
                        ConnTransactions = null;
                        throw ex;
                    }
                    catch (System.Exception ex)
                    {
                        ConnTransactions = null;
                        bError = false;
                        throw ex;
                    }

                    finally
                    {

                    }
                    break;
                case (int)ConnectionDB.TraderEntitlement:
                    try
                    {
                        ConnTraderEntitlement = new SQLiteConnection(szConnStrTraderEntitlement);
                        ConnTraderEntitlement.Open();
                        bError = true;
                    }
                    catch (SQLiteException ex)
                    {
                        bError = false;
                        ConnTraderEntitlement = null;
                        throw ex;
                    }
                    catch (System.Exception ex)
                    {
                        ConnTraderEntitlement = null;
                        bError = false;
                        throw ex;
                    }

                    finally
                    {

                    }
                    break;
                default:
                    break;
            }
            return (bError);
        }

        /// <summary>
        /// Dispose each connections
        /// </summary>
        public void DisposeSQLite()
        {
            SQLiteConnection.ClearAllPools();
            GC.Collect();
        }

        public void Dispose(int param)
        {
            switch (param)
            {
                case (int)ConnectionDB.Masters:
                    if (ConnMasters != null)
                    {
                        ConnMasters.Dispose();
                        ConnMasters = null;
                    }
                    break;
                case (int)ConnectionDB.Setting:
                    if (ConnSetting != null)
                    {
                        ConnSetting.Dispose();
                        ConnSetting = null;
                    }
                    break;
                case (int)ConnectionDB.Transactions:
                    if (ConnTransactions != null)
                    {
                        ConnTransactions.Dispose();
                        ConnTransactions = null;
                    }
                    break;
                case (int)ConnectionDB.TraderEntitlement:
                    if (ConnTransactions != null)
                    {
                        ConnTransactions.Dispose();
                        ConnTransactions = null;
                    }
                    break;
                default:
                    break;
            }


        }

        void IDisposable.Dispose()
        {
            ConnMasters.Dispose();
            ConnMasters = null;

            ConnSetting.Dispose();
            ConnSetting = null;

            ConnTransactions.Dispose();
            ConnTransactions = null;

            ConnTraderEntitlement.Dispose();
            ConnTraderEntitlement = null;
        }

    }

}
