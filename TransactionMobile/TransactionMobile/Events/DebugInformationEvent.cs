namespace TransactionMobile.Events
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="TransactionMobile.Events.BaseLoggingEvent" />
    public class DebugInformationEvent : BaseLoggingEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugInformationEvent"/> class.
        /// </summary>
        /// <param name="debugMessage">The debug message.</param>
        private DebugInformationEvent(String debugMessage)
        {
            this.DebugMessage = debugMessage;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the debug message.
        /// </summary>
        /// <value>
        /// The debug message.
        /// </value>
        public String DebugMessage { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified debug message.
        /// </summary>
        /// <param name="debugMessage">The debug message.</param>
        /// <returns></returns>
        public static DebugInformationEvent Create(String debugMessage)
        {
            return new DebugInformationEvent(debugMessage);
        }

        /// <summary>
        /// Gets the event data.
        /// </summary>
        /// <returns></returns>
        public override Dictionary<String, String> GetEventData()
        {
            return new Dictionary<String, String>
                   {
                       {"DebugMessage", this.DebugMessage}
                   };
        }

        #endregion
    }
}