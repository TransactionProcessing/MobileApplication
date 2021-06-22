﻿namespace TransactionMobile.Clients
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="IConfiguration" />
    [ExcludeFromCodeCoverage]
    public class Configuration : IConfiguration
    {
        #region Properties

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        public String ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        /// <value>
        /// The client secret.
        /// </value>
        public String ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the security service.
        /// </summary>
        /// <value>
        /// The security service.
        /// </value>
        [JsonProperty("securityServiceUri")]
        public String SecurityService { get; set; }

        /// <summary>
        /// Gets or sets the transaction processor acl.
        /// </summary>
        /// <value>
        /// The transaction processor acl.
        /// </value>
        [JsonProperty("transactionProcessorACLUri")]
        public String TransactionProcessorACL { get; set; }

        /// <summary>
        /// Gets or sets the estate management.
        /// </summary>
        /// <value>
        /// The estate management.
        /// </value>
        [JsonProperty("estateManagementUri")]
        public String EstateManagement { get; set; }

        /// <summary>
        /// Gets or sets the log level.
        /// </summary>
        /// <value>
        /// The log level.
        /// </value>
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable automatic updates].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable automatic updates]; otherwise, <c>false</c>.
        /// </value>
        public Boolean EnableAutoUpdates { get; set; }

        #endregion
    }
}