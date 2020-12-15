namespace TransactionMobile.iOS
{
    using System;
    using System.Threading.Tasks;
    using Common;
    using UIKit;
    using Xamarin.Forms;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="TransactionMobile.Common.IDevice" />
    public class iOSDevice : IDevice
    {
        #region Methods

        /// <summary>
        /// Gets the device identifier.
        /// </summary>
        /// <returns></returns>
        public String GetDeviceIdentifier()
        {
            return UIDevice.CurrentDevice.IdentifierForVendor.AsString().Replace("-", "");
        }

        /// <summary>
        /// Gets the software version.
        /// </summary>
        /// <returns></returns>
        public String GetSoftwareVersion()
        {
            return null;
        }

        #endregion
    }
}