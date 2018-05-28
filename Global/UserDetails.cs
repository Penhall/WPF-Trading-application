using System;

namespace CommonFrontEnd.Global
{
    [Serializable()]
    public class UserDetails
    {
        private string mstrUserId;
        private string mstrLoginId;
        private string mstrBackOfficeId;
        private string mstrFirstName;
        private string mstrLastName;
        private string mstrParticipantCode = "";
        private string mstrCategory;
        private string mstrCPCode;
        private string mstrCustodianCode;
        public enum UserDetailsSettings
        {
            BackOffficeId = 1,
            FirstName = 2,
            LastName = 4,
            LoginId = 8
        }
        private static int mintUserDisplaySetttings = (int)UserDetailsSettings.BackOffficeId + (int)UserDetailsSettings.FirstName + (int)UserDetailsSettings.LastName + (int)UserDetailsSettings.LoginId;
        

        public static int UserDisplaySettings
        {
            get { return mintUserDisplaySetttings; }
            set { mintUserDisplaySetttings = value; }
        }

        public string UserId
        {
            get { return mstrUserId; }
            set { mstrUserId = value; }
        }
        public string LoginId
        {
            get { return mstrLoginId; }
            set { mstrLoginId = value; }
        }
        public string BackOfficeId
        {
            get { return mstrBackOfficeId; }
            set { mstrBackOfficeId = value; }
        }
        public string FirstName
        {
            get { return mstrFirstName; }
            set { mstrFirstName = value; }
        }
        public string LastName
        {
            get { return mstrLastName; }
            set { mstrLastName = value; }
        }
        public string ParticipantCode
        {
            get
            {
                if (mstrParticipantCode == null || mstrParticipantCode.Trim().Length == 0)
                {
                    return "";
                }
                else
                {
                    return mstrParticipantCode;
                }
            }
            set { mstrParticipantCode = value; }
        }
        public string OFSCATEGORY
        {
            get { return mstrCategory; }
            set { mstrCategory = value; }
        }
        public string OFSCPCODE
        {
            get { return mstrCPCode; }
            set { mstrCPCode = value; }
        }
        public string OFSCUSTODIANCODE
        {
            get { return mstrCustodianCode; }
            set { mstrCustodianCode = value; }
        }

        public bool Is_InstitutionClient { get; set; }
        public static string SelectedTab { get; set; }
        public override string ToString()
        {
            string lstrDisplay = "";
            if (((mintUserDisplaySetttings & (int) UserDetailsSettings.BackOffficeId) != 0))
            {
                if (lstrDisplay.Trim().Length > 0)
                    lstrDisplay += " - ";
                lstrDisplay += mstrBackOfficeId;
            }
            if (((mintUserDisplaySetttings & (int)UserDetailsSettings.FirstName) != 0))
            {
                if (lstrDisplay.Trim().Length > 0)
                    lstrDisplay += " - ";
                lstrDisplay += mstrFirstName;
            }
            if (((mintUserDisplaySetttings & (int)UserDetailsSettings.LastName) != 0))
            {
                if (lstrDisplay.Trim().Length > 0)
                    lstrDisplay += " - ";
                lstrDisplay += mstrLastName;
            }
            if (((mintUserDisplaySetttings & (int)UserDetailsSettings.LoginId) != 0))
            {
                if (lstrDisplay.Trim().Length > 0)
                    lstrDisplay += " - ";
                lstrDisplay += mstrLoginId;
            }

            return (lstrDisplay);
        }

        public static string GetFormattedLoginDetailWhereClause()
        {
            //USLoginid,USBackOfficeId,USFirstName,USLastName
            string lstrDisplay = "";
            if (((mintUserDisplaySetttings & (int)UserDetailsSettings.BackOffficeId) != 0))
            {
                if (lstrDisplay.Trim().Length > 0)
                    lstrDisplay += " &' - '& ";
                lstrDisplay += "USBackOfficeId";
            }
            if (((mintUserDisplaySetttings & (int)UserDetailsSettings.FirstName) != 0))
            {
                if (lstrDisplay.Trim().Length > 0)
                    lstrDisplay += " &' - '& ";
                lstrDisplay += "USFirstName";
            }
            if (((mintUserDisplaySetttings & (int)UserDetailsSettings.LastName) != 0))
            {
                if (lstrDisplay.Trim().Length > 0)
                    lstrDisplay += " &' - '& ";
                lstrDisplay += "USLastName";
            }
            if (((mintUserDisplaySetttings & (int)UserDetailsSettings.LoginId) != 0))
            {
                if (lstrDisplay.Trim().Length > 0)
                    lstrDisplay += " &' - '& ";
                lstrDisplay += "USLoginid";
            }

            return (lstrDisplay);


        }
    }

   
}
