﻿namespace TransactionMobile.Clients
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public interface IConfiguration
    {
        #region Properties

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        String ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        /// <value>
        /// The client secret.
        /// </value>
        String ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the security service.
        /// </summary>
        /// <value>
        /// The security service.
        /// </value>
        String SecurityService { get; set; }

        /// <summary>
        /// Gets or sets the transaction processor acl.
        /// </summary>
        /// <value>
        /// The transaction processor acl.
        /// </value>
        String TransactionProcessorACL { get; set; }

        /// <summary>
        /// Gets or sets the estate management.
        /// </summary>
        /// <value>
        /// The estate management.
        /// </value>
        String EstateManagement { get; set; }

        /// <summary>
        /// Gets or sets the log level.
        /// </summary>
        /// <value>
        /// The log level.
        /// </value>
        LogLevel LogLevel { get; set; }

        Boolean EnableAutoUpdates { get; set; }

        #endregion
    }
}