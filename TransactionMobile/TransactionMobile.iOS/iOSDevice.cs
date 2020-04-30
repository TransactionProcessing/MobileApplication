namespace TransactionMobile.iOS
{
    using System;
    using System.Threading.Tasks;
    using Common;
    using Events;
    //using InstabugLib;
    using UIKit;

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
        
        #endregion
    }
}