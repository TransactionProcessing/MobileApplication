namespace TransactionMobile.Events
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="TransactionMobile.Events.BaseLoggingEvent" />
    public class MessageReceivedFromHostEvent : BaseLoggingEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageReceivedFromHostEvent"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="timestamp">The timestamp.</param>
        private MessageReceivedFromHostEvent(String message,
                                             DateTime timestamp)
        {
            this.Message = message;
            this.Timestamp = timestamp;
        }

        #endregion

        #region Properties

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
        /// Creates the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <returns></returns>
        public static MessageReceivedFromHostEvent Create(String message,
                                                          DateTime timestamp)
        {
            return new MessageReceivedFromHostEvent(message, timestamp);
        }

        /// <summary>
        /// Gets the event data.
        /// </summary>
        /// <returns></returns>
        public override Dictionary<String, String> GetEventData()
        {
            return new Dictionary<String, String>
                   {
                       {"Message", this.Message},
                       {"Timestamp", this.Timestamp.ToString("dd/MM/yyyy HH:mm:ss.fff")}
                   };
        }

        #endregion
    }
}