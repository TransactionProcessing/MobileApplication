namespace TransactionMobile.Droid
{
    using System;
    using Android.App;
    using Android.Content;
    using Android.OS;
    using Android.Provider;
    using Android.Util;
    using Common;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="TransactionMobile.Common.IDevice" />
    public class AndroidDevice : IDevice
    {
        #region Methods
        
        /// <summary>
        /// Gets the device identifier.
        /// </summary>
        /// <returns></returns>
        public String GetDeviceIdentifier()
        {
            String id = Build.Serial;
            if (string.IsNullOrWhiteSpace(id) || id == Build.Unknown || id == "0")
            {
                try
                {
                    Context context = Application.Context;
                    id = Settings.Secure.GetString(context.ContentResolver, Settings.Secure.AndroidId);
                }
                catch(Exception ex)
                {
                    Log.Warn("DeviceInfo", "Unable to get id: " + ex);
                }
            }

            return id;
        }

        #endregion
    }
}