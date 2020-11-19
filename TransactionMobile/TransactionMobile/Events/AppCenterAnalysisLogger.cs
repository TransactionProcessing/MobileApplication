namespace TransactionMobile.Events
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.AppCenter;
    using Microsoft.AppCenter.Analytics;
    using Microsoft.AppCenter.Crashes;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="TransactionMobile.Events.IAnalysisLogger" />
    public class AppCenterAnalysisLogger : IAnalysisLogger
    {
        #region Methods

        /// <summary>
        /// Initialises the specified use analysis reporting.
        /// </summary>
        /// <param name="useAnalysisReporting">if set to <c>true</c> [use analysis reporting].</param>
        /// <param name="useCrashReporting">if set to <c>true</c> [use crash reporting].</param>
        /// <param name="androidSecret">The android secret.</param>
        /// <param name="iosSecret">The ios secret.</param>
        public void Initialise(Boolean useAnalysisReporting = true,
                               Boolean useCrashReporting = false,
                               String androidSecret = null,
                               String iosSecret = null)
        {
            List<Type> services = new List<Type>();

            if (useAnalysisReporting)
            {
                services.Add(typeof(Analytics));
            }

            if (useCrashReporting)
            {
                services.Add(typeof(Crashes));
            }

            StringBuilder appSecrets = new StringBuilder();

            if (string.IsNullOrEmpty(androidSecret) == false)
            {
                appSecrets.Append($"android={androidSecret};");
            }

            if (string.IsNullOrEmpty(iosSecret) == false)
            {
                appSecrets.Append($"ios={iosSecret};");
            }

            AppCenter.Start(appSecrets.ToString(), services.ToArray());
        }

        /// <summary>
        /// Sets the name of the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        public void SetUserName(String userName)
        {
            AppCenter.SetUserId(userName);
        }

        /// <summary>
        /// Tracks the crash.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void TrackCrash(Exception exception)
        {
            try
            {
                Crashes.TrackError(exception);
            }
            catch
            {
                // just suppress any error logging exceptions
            }
        }

        /// <summary>
        /// Tracks the event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="loggingEvent">The logging event.</param>
        public void TrackEvent<T>(T loggingEvent)
        {
            this.TrackEvent((dynamic)loggingEvent);
        }

        /// <summary>
        /// Tracks the event.
        /// </summary>
        /// <param name="pageRequestedEvent">The page requested event.</param>
        private void TrackEvent(PageRequestedEvent pageRequestedEvent)
        {
            String eventName = pageRequestedEvent.GetType().Name;
            
            //Analytics.TrackEvent(eventName, pageRequestedEvent.GetEventData());
        }

        /// <summary>
        /// Tracks the event.
        /// </summary>
        /// <param name="pageInitialisedEvent">The page initialised event.</param>
        private void TrackEvent(PageInitialisedEvent pageInitialisedEvent)
        {
            String eventName = pageInitialisedEvent.GetType().Name;

            //Analytics.TrackEvent(eventName, pageInitialisedEvent.GetEventData());
        }

        /// <summary>
        /// Tracks the event.
        /// </summary>
        /// <param name="debugInformationEvent">The debug information event.</param>
        private void TrackEvent(DebugInformationEvent debugInformationEvent)
        {
            String eventName = debugInformationEvent.GetType().Name;

            Analytics.TrackEvent(eventName, debugInformationEvent.GetEventData());
        }

        /// <summary>
        /// Tracks the event.
        /// </summary>
        /// <param name="messageSentToHostEvent">The message sent to host event.</param>
        private void TrackEvent(MessageSentToHostEvent messageSentToHostEvent)
        {
            String eventName = messageSentToHostEvent.GetType().Name;

            //Analytics.TrackEvent(eventName, messageSentToHostEvent.GetEventData());
        }

        /// <summary>
        /// Tracks the event.
        /// </summary>
        /// <param name="messageReceivedFromHostEvent">The message received from host event.</param>
        private void TrackEvent(MessageReceivedFromHostEvent messageReceivedFromHostEvent)
        {
            String eventName = messageReceivedFromHostEvent.GetType().Name;

            //Analytics.TrackEvent(eventName, messageReceivedFromHostEvent.GetEventData());
        }

        #endregion
    }
}