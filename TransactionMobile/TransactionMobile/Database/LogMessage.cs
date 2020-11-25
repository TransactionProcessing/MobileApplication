using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionMobile.Database
{
    using SQLite;

    public class LogMessage
    {
        [PrimaryKey, AutoIncrement]
        public Int32 Id { get; set; }

        public DateTime EntryDateTime { get; set; }

        public String LogLevel { get; set; }

        public String Message { get; set; }
    }
}
