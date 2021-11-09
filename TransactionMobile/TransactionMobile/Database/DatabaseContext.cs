namespace TransactionMobile.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Clients;
    using Entities;
    using SQLite;
    using LogMessage = Entities.LogMessage;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="TransactionMobile.Database.IDatabaseContext" />
    public class DatabaseContext : IDatabaseContext
    {
        #region Fields

        /// <summary>
        /// The connection
        /// </summary>
        private readonly SQLiteAsyncConnection Connection;

        #endregion

        #region Constructors

        public DatabaseContext()
        {
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseContext" /> class.
        /// </summary>
        /// <param name="connectionStringResolver">The connection string resolver.</param>
        public DatabaseContext(Func<String> connectionStringResolver) : this(connectionStringResolver())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseContext" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public DatabaseContext(String connectionString)
        {
            if (connectionString != String.Empty)
            {
                this.Connection = new SQLiteAsyncConnection(connectionString);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the debug log message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static LogMessage CreateDebugLogMessage(String message)
        {
            return DatabaseContext.CreateLogMessage(message, LogLevel.Debug);
        }

        /// <summary>
        /// Creates the error log message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static LogMessage CreateErrorLogMessage(String message)
        {
            return DatabaseContext.CreateLogMessage(message, LogLevel.Error);
        }

        /// <summary>
        /// Creates the error log messages.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public static List<LogMessage> CreateErrorLogMessages(Exception exception)
        {
            List<LogMessage> logMessages = new List<LogMessage>();

            Exception e = exception;
            while (e != null)
            {
                logMessages.Add(DatabaseContext.CreateLogMessage(e.Message, LogLevel.Error));
                e = e.InnerException;
            }

            return logMessages;
        }

        /// <summary>
        /// Creates the fatal log message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static LogMessage CreateFatalLogMessage(String message)
        {
            return DatabaseContext.CreateLogMessage(message, LogLevel.Fatal);
        }

        /// <summary>
        /// Creates the fatal log messages.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public static List<LogMessage> CreateFatalLogMessages(Exception exception)
        {
            List<LogMessage> logMessages = new List<LogMessage>();

            Exception e = exception;
            while (e != null)
            {
                logMessages.Add(DatabaseContext.CreateLogMessage(e.Message, LogLevel.Fatal));
                e = e.InnerException;
            }

            return logMessages;
        }

        /// <summary>
        /// Creates the information log message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static LogMessage CreateInformationLogMessage(String message)
        {
            return DatabaseContext.CreateLogMessage(message, LogLevel.Info);
        }

        /// <summary>
        /// Creates the trace log message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static LogMessage CreateTraceLogMessage(String message)
        {
            return DatabaseContext.CreateLogMessage(message, LogLevel.Trace);
        }

        /// <summary>
        /// Creates the warning log message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static LogMessage CreateWarningLogMessage(String message)
        {
            return DatabaseContext.CreateLogMessage(message, LogLevel.Warn);
        }

        /// <summary>
        /// Gets the log messages.
        /// </summary>
        /// <param name="batchSize">Size of the batch.</param>
        /// <returns></returns>
        public async Task<List<LogMessage>> GetLogMessages(Int32 batchSize)
        {
            if (this.Connection == null)
                return new List<LogMessage>();

            List<LogMessage> messages = await this.Connection.Table<LogMessage>().OrderBy(l => l.EntryDateTime).Take(batchSize).ToListAsync();

            return messages;
        }

        /// <summary>
        /// Initialises the database.
        /// </summary>
        public async Task InitialiseDatabase()
        {
            if (this.Connection == null)
                return;

            // Create the required tables
            await this.Connection.CreateTableAsync<LogMessage>();
            await this.Connection.CreateTableAsync<OperatorTotals>();
        }

        /// <summary>
        /// Inserts the log message.
        /// </summary>
        /// <param name="logMessage">The log message.</param>
        public async Task InsertLogMessage(LogMessage logMessage)
        {
            if (this.Connection == null)
                return;
            
            LogLevel messageLevel = (LogLevel)Enum.Parse(typeof(LogLevel), logMessage.LogLevel, true);
            if (App.Configuration == null || messageLevel <= App.Configuration.LogLevel)
            {
                await this.Connection.InsertAsync(logMessage);
            }
        }

        /// <summary>
        /// Inserts the log messages.
        /// </summary>
        /// <param name="logMessages">The log messages.</param>
        public async Task InsertLogMessages(List<LogMessage> logMessages)
        {
            foreach (LogMessage logMessage in logMessages)
            {
                await this.InsertLogMessage(logMessage);
            }
        }

        /// <summary>
        /// Removes the uploaded messages.
        /// </summary>
        /// <param name="logMessagesToRemove">The log messages to remove.</param>
        public async Task RemoveUploadedMessages(List<LogMessage> logMessagesToRemove)
        {
            if (this.Connection == null)
                return;
            foreach (LogMessage logMessage in logMessagesToRemove)
            {
                await this.Connection.DeleteAsync(logMessage);
            }
        }

        /// <summary>
        /// Updates the operator totals.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="transactionCount">The transaction count.</param>
        /// <param name="transactionValue">The transaction value.</param>
        public async Task UpdateOperatorTotals(String operatorId,
                                               Int32 transactionCount,
                                               Decimal transactionValue)
        {
            if (this.Connection == null)
                return;

            // get the totals record for this operator
            OperatorTotals operatorTotals = await this.GetOperatorTotals(operatorId);

            if (operatorTotals == null)
            {
                operatorTotals = new OperatorTotals
                                                {
                                                    OperatorId = operatorId,
                                                    LastUpdateDateTime = DateTime.UtcNow,
                                                    TransactionCount = transactionCount,
                                                    TransactionValue = transactionValue
                                                };

                // No Totals found so insert a row
                await this.Connection.InsertAsync(operatorTotals);

            }
            else
            {
                operatorTotals.LastUpdateDateTime = DateTime.Now;
                operatorTotals.TransactionCount += transactionCount;
                operatorTotals.TransactionValue += transactionValue;

                // Update the row
                await this.Connection.UpdateAsync(operatorTotals);
            }
        }

        /// <summary>
        /// Gets the overall totals.
        /// </summary>
        /// <returns></returns>
        private async Task<List<OperatorTotals>> GetAllTotals()
        {
            if (this.Connection == null)
                return new List<OperatorTotals>();

            List<OperatorTotals> totals = await this.Connection.QueryAsync<OperatorTotals>("Select * From OperatorTotals");

            return totals;
        }

        /// <summary>
        /// Gets the operator totals.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        private async Task<OperatorTotals> GetOperatorTotals(String operatorId)
        {
            if (this.Connection == null)
                return new OperatorTotals();

            List<OperatorTotals> totals = await this.Connection.QueryAsync<OperatorTotals>("Select * From OperatorTotals Where OperatorId ='" + operatorId + "'");

            OperatorTotals operatorTotals = totals.SingleOrDefault();

            return operatorTotals;
        }

        /// <summary>
        /// Resets the totals.
        /// </summary>
        public async Task ResetTotals()
        {
            if (this.Connection == null)
                return;
            
            await this.Connection.DeleteAllAsync<OperatorTotals>();
        }

        /// <summary>
        /// Gets the totals.
        /// </summary>
        /// <returns></returns>
        public async Task<OperatorTotals> GetTotals()
        {
            List<OperatorTotals> totals = await this.GetAllTotals();

               return new OperatorTotals
                       {
                           LastUpdateDateTime = totals.Any() ? totals.Max(t => t.LastUpdateDateTime) : DateTime.Now,
                           TransactionCount = totals.Any() ? totals.Sum(t => t.TransactionCount) : 0,
                           TransactionValue = totals.Any() ? totals.Sum(t => t.TransactionValue) : 0,
                       };
        }

        /// <summary>
        /// Creates the log message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logLevel">The log level.</param>
        /// <returns></returns>
        private static LogMessage CreateLogMessage(String message,
                                                   LogLevel logLevel)
        {
            return new LogMessage
                   {
                       EntryDateTime = DateTime.UtcNow,
                       Message = message,
                       LogLevel = logLevel.ToString()
                   };
        }

        #endregion
    }
}