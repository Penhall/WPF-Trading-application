using CommonFrontEnd.Utility.DBProvider;
using CommonFrontEnd.Utility.Interfaces_Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CommonFrontEnd.Utility.Factory
{
    internal class DBProviderFactory
    {


        private static Dictionary<string, Type> IDBProvider_Types = new Dictionary<string, Type>();
        private DBProviderFactory()
        {
        }

        static DBProviderFactory()
        {
            //LoadDBProviders();
        }

        //private static void LoadDBProviders()
        //{
        //    Type[] lobjTypes = Assembly.GetExecutingAssembly.GetTypes();
        //    string lstrAssembly = Assembly.GetExecutingAssembly.GetName.Name;
        //    for (int lintTemp = 0; lintTemp <= lobjTypes.Length - 1; lintTemp++)
        //    {
        //        if ((lobjTypes[lintTemp].GetInterface(typeof(IDBProvider).ToString()) != null))
        //        {
        //            IDBProvider_Types.Add(lobjTypes[lintTemp].Name.ToUpper(), lobjTypes[lintTemp]);
        //        }
        //    }
        //}

        /// <summary>
        /// Creates a new instance of DBProvider, specified by the name passed in.
        /// </summary>
        /// <param name="pstrClassType">Optional Paramater if not passed, then the Default DBProvider will be passed back which in our case is SQLiteDBProvider.</param>
        /// <returns>Insatance of Type IDBProvider</returns>
        /// <remarks>If no provier with the name specified is found then an instance of UnknownDBProvider is returned and not null.</remarks>
        public static IDBProvider CreateInstance(string pstrClassType = "SQLiteDBProvider")
        {
            //TODO Possibly we will have to append the namespace
            Type lobjType = GetTypeToCreate(pstrClassType.ToUpper());
            if (lobjType == null)
            {
                return new UnknownDBProvider();
            }
            else
            {
                return Activator.CreateInstance(lobjType, true) as IDBProvider;
            }
        }

        private static Type GetTypeToCreate(string pstrClassType)
        {
            if (IDBProvider_Types.ContainsKey(pstrClassType))
            {

                return IDBProvider_Types.Where(x => x.Key == pstrClassType).Select(x => x.Value).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

    }

}
