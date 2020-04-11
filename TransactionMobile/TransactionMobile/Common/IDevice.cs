namespace TransactionMobile.Common
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// 
    /// </summary>
    public interface IDevice
    {
        #region Methods

        /// <summary>
        /// Clears the instabug user data.
        /// </summary>
        //void ClearInstabugUserData();

        /// <summary>
        /// Sets the instabug user data.
        /// </summary>
        /// <param name="userData">The user data.</param>
        //void SetInstabugUserData(String userData);

        /// <summary>
        /// Sets the instabug user details.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="emailAddress">The email address.</param>
        //void SetInstabugUserDetails(String userName,
        //                           String emailAddress);

        /// <summary>
        /// Gets the device identifier.
        /// </summary>
        /// <returns></returns>
        String GetDeviceIdentifier();

        /// <summary>
        /// Adds the debug information.
        /// </summary>
        void AddDebugInformation(String debug);

        /// <summary>
        /// Gets the debug information.
        /// </summary>
        /// <value>
        /// The debug information.
        /// </value>
        String DebugInformation { get; }



        #endregion
    }
}