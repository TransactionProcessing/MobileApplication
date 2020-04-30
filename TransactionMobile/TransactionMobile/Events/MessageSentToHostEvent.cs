namespace TransactionMobile.Events
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="TransactionMobile.Events.BaseLoggingEvent" />
    public class MessageSentToHostEvent : BaseLoggingEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSentToHostEvent"/> class.
        /// </summary>
        /// <param name="hostAddress">The host address.</param>
        /// <param name="message">The message.</param>
        /// <param name="timestamp">The timestamp.</param>
        private MessageSentToHostEvent(String hostAddress,
                                       String message,
                                       DateTime timestamp)
        {
            this.HostAddress = hostAddress;
            this.Message = message;
            this.Timestamp = timestamp;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the host address.
        /// </summary>
        /// <value>
        /// The host address.
        /// </value>
        public String HostAddress { get; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public String Message { get; }

        /// <summary>
        /// Gets the timestamp.
        /// </summary>
        /// <value>
        /// The timestamp.
        /// </value>
        public DateTime Timestamp { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified host address.
        /// </summary>
        /// <param name="hostAddress">The host address.</param>
        /// <param name="message">The message.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <returns></returns>
        public static MessageSentToHostEvent Create(String hostAddress,
                                                    String message,
                                                    DateTime timestamp)
        {
            return new MessageSentToHostEvent(hostAddress, message, timestamp);
        }

        /// <summary>
        /// Gets the event data.
        /// </summary>
        /// <returns></returns>
        public override Dictionary<String, String> GetEventData()
        {
            return new Dictionary<String, String>
                   {
                       {"HostAddress", this.HostAddress},
                       {"Message", this.Message},
                       {"Timestamp", this.Timestamp.ToString("dd/MM/yyyy HH:mm:ss.fff")}
                   };
        }

        #endregion
    }
}