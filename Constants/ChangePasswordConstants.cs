using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.Constants
{
#if BOW
    class ChangePasswordConstants
    {
        // Servlet Names
        public static string SERVLET_NAME_PASSWORD = "ChangePassword";
        public static string SERVLET_NAME_TRANS_PASSWORD = "ChangeTransactionPassword";

        // Parameter Names
        public static string OLD_PASSWORD = "OldPassword";
        public static string NEW_PASSWORD = "PHPassword";
        public static string CONFIRM_PASSWORD = "ConfirmPassword";
        public static string USERFLAG = "userflag";
    }
#endif
}
