namespace TransactionMobile.Events
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public interface IAnalysisLogger
    {
        #region Methods

        /// <summary>
        /// Initialises the specified use analysis reporting.
        /// </summary>
        /// <param name="useAnalysisReporting">if set to <c>true</c> [use analysis reporting].</param>
        /// <param name="useCrashReporting">if set to <c>true</c> [use crash reporting].</param>
        /// <param name="androidSecret">The android secret.</param>
        /// <param name="iosSecret">The ios secret.</param>
        void Initialise(Boolean useAnalysisReporting = true,
                        Boolean useCrashReporting = false,
                        String androidSecret = null,
                        String iosSecret = null);

        /// <summary>
        /// Sets the name of the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        void SetUserName(String userName);

        /// <summary>
        /// Tracks the crash.
        /// </summary>
        /// <param name="exception">The exception.</param>
        void TrackCrash(Exception exception);

        /// <summary>
        /// Tracks the event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="loggingEvent">The logging event.</param>
        void TrackEvent<T>(T loggingEvent);

        #endregion
    }
}