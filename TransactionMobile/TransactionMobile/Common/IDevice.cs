﻿namespace TransactionMobile.Common
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IDevice
    {
        #region Methods

        /// <summary>
        /// Gets the device identifier.
        /// </summary>
        /// <returns></returns>
        String GetDeviceIdentifier();

        /// <summary>
        /// Gets the software version.
        /// </summary>
        /// <returns></returns>
        String GetSoftwareVersion();

        #endregion
    }

    public class TestDevice : IDevice
    {
        public String GetDeviceIdentifier()
        {
            return String.Empty;
        }

        public String GetSoftwareVersion()
        {
            return String.Empty;
        }
    }
}