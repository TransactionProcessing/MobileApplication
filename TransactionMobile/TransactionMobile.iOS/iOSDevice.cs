namespace TransactionMobile.iOS
{
    using System;
    using Common;
    using InstabugLib;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="TransactionMobile.Common.IDevice" />
    public class iOSDevice : IDevice
    {
        #region Methods

        /// <summary>
        /// Clears the instabug user data.
        /// </summary>
        public void ClearInstabugUserData()
        {
            Instabug.LogOut();
        }

        /// <summary>
        /// Sets the instabug user data.
        /// </summary>
        /// <param name="userData">The user data.</param>
        public void SetInstabugUserData(String userData)
        {
            Instabug.SetUserData(userData);
        }

        /// <summary>
        /// Sets the instabug user details.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="emailAddress">The email address.</param>
        public void SetInstabugUserDetails(String userName,
                                           String emailAddress)
        {
            Instabug.IdentifyUserWithEmail(userName, emailAddress);
        }

        #endregion
    }
}