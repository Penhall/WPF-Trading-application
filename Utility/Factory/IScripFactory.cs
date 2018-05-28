using CommonFrontEnd.Utility.Interfaces_Abstract;
using System;
using System.Collections.Generic;
using System.Reflection;
using CommonFrontEnd.Utility.Entity;

namespace CommonFrontEnd.Utility.Factory
{
    public class IScripFactory
    {


        private static Dictionary<string, Type> IScrip_Types;
        static IScripFactory()
        {
            LoadTypesICanReturn();
        }

        private static void LoadTypesICanReturn()
        {
            IScrip_Types = new Dictionary<string, Type>();

            Type[] typesInThisAssembly = Assembly.GetExecutingAssembly().GetTypes();

            foreach (Type type in typesInThisAssembly)
            {
                if ((type.GetInterface(typeof(IScript).ToString()) != null))
                {
                    IScrip_Types.Add(type.Name.ToUpper(), type);
                }
            }
        }
        /// <summary>
        /// Dynamically creates an instance of Type IScrip based on the ClassType passed as parameter.
        /// </summary>
        /// <param name="pstrClassType"></param>
        /// <returns>Object of Type IScript</returns>
        /// <remarks>Should be called only from GetScripFactory Initialization. Use GetScripFactory.GetIScripObjectFor(int,int) to create new objects of type IScrip.</remarks>
        public static IScript CreateInstance(string pstrClassType)
        {
            Type t = GetTypeToCreate(pstrClassType.ToUpper());

            if (t == null)
            {
                return new UnknownScrip();
            }

            return Activator.CreateInstance(t, true) as IScript;
        }

        private static Type GetTypeToCreate(string pstrClassType)
        {
            //TODO Here we may have to append the ClassType with the Namespace as we are not specifying them in ExchangeColumnNameMap.
            if (IScrip_Types.ContainsKey(pstrClassType))
            {
                return IScrip_Types[pstrClassType];
            }
            else
            {
                return null;
            }
        }

    }
}
