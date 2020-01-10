namespace TransactionMobile.Droid
{
    using System;
    using Com.Instabug.Library;
    using Common;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="TransactionMobile.Common.IDevice" />
    public class AndroidDevice : IDevice
    {
       
        #region Methods

        /// <summary>
        /// Clears the instabug user data.
        /// </summary>
        public void ClearInstabugUserData()
        {
            Instabug.LogoutUser();
        }
        
        /// <summary>
        /// Sets the instabug user data.
        /// </summary>
        /// <param name="userData">The user data.</param>
        public void SetInstabugUserData(String userData)
        {
            // TODO: May protect overwriting 
            // TODO: Max length 1000 chars
            Instabug.UserData = userData;
        }

        /// <summary>
        /// Sets the instabug user details.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="emailAddress">The email address.</param>
        public void SetInstabugUserDetails(String userName,
                                           String emailAddress)
        {
            Instabug.IdentifyUser(userName, emailAddress);
        }

        #endregion
    }
}