namespace TransactionMobile.Droid
{
    using System;
    using System.Collections.Generic;
    using Android.Content;
    using Android.OS;
    using Android.Provider;
    //using Com.Instabug.Library;
    using Common;
    using Microsoft.AppCenter.Analytics;

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
            //Instabug.LogoutUser();
        }
        
        /// <summary>
        /// Sets the instabug user data.
        /// </summary>
        /// <param name="userData">The user data.</param>
        public void SetInstabugUserData(String userData)
        {
            // TODO: May protect overwriting 
            // TODO: Max length 1000 chars
            //Instabug.UserData = userData;
        }

        /// <summary>
        /// Sets the instabug user details.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="emailAddress">The email address.</param>
        public void SetInstabugUserDetails(String userName,
                                           String emailAddress)
        {
            //Instabug.IdentifyUser(userName, emailAddress);
        }

        public String GetDeviceIdentifier()
        {
            String id = Android.OS.Build.Serial;
            if (string.IsNullOrWhiteSpace(id) || id == Build.Unknown || id == "0")
            {
                try
                {
                    Context context = Android.App.Application.Context;
                    id = Settings.Secure.GetString(context.ContentResolver, Settings.Secure.AndroidId);
                }
                catch (Exception ex)
                {
                    Android.Util.Log.Warn("DeviceInfo", "Unable to get id: " + ex.ToString());
                }
            }

            return id;
        }

        public String DebugInformation { get; private set; }

        public void AddDebugInformation(String debug)
        {
            this.DebugInformation += $"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff},{debug}|";
            Analytics.TrackEvent("DebugEvent", new Dictionary<String, String>()
                                               {
                                                   { $"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff}", debug}
                                               });
        }


        #endregion
    }
}