namespace TransactionMobile.Events
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseLoggingEvent
    {
        #region Methods

        /// <summary>
        /// Gets the event data.
        /// </summary>
        /// <returns></returns>
        public abstract Dictionary<String, String> GetEventData();

        #endregion
    }
}