namespace TransactionMobile.Events
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="TransactionMobile.Events.BaseLoggingEvent" />
    public class PageInitialisedEvent : BaseLoggingEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PageInitialisedEvent"/> class.
        /// </summary>
        /// <param name="pageName">Name of the page.</param>
        private PageInitialisedEvent(String pageName)
        {
            this.PageName = pageName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the page.
        /// </summary>
        /// <value>
        /// The name of the page.
        /// </value>
        public String PageName { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified page name.
        /// </summary>
        /// <param name="pageName">Name of the page.</param>
        /// <returns></returns>
        public static PageInitialisedEvent Create(String pageName)
        {
            return new PageInitialisedEvent(pageName);
        }

        /// <summary>
        /// Gets the event data.
        /// </summary>
        /// <returns></returns>
        public override Dictionary<String, String> GetEventData()
        {
            return new Dictionary<String, String>
                   {
                       {"PageName", this.PageName}
                   };
        }

        #endregion
    }
}