using System;
using System.Collections.Generic;
using DuplicateCheckerLib;
using LinqToDB.Mapping;
using zbW.ProjNuGet.Model;

namespace zbW.ProjNuGet
{
    [Table("v_logentries")]
    public class LogEntry : ModelBase
    {
        private bool _confirm;
        private bool _duplicate;
        
        [Column("id"), PrimaryKey, NotNull]
        public override int Id { get; set; }

        [Column("pod")]
        public string Pod { get; set; }

        [Column("location")]
        public string Location { get; set; }

        [Column("hostname")]
        public string Hostname { get; set; }

        [Column("severity")]
        public int Severity { get; set; }

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }

        [Column("message")]
        public string Message { get; set; }
       

        public bool confirm
        {
            get { return _confirm;}
            set { _confirm = value; }
        }
        public bool duplicate
        {
            get { return _duplicate;}
            set { _duplicate = value; }
        }
        

        /// <summary>
        /// OBJECT Equality....
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as LogEntry);
        }

        public bool Equals(LogEntry logEntry)
        {
            if (Object.ReferenceEquals(null, logEntry)) return false;
            if (Object.ReferenceEquals(this, logEntry)) return true;

            return (Severity == logEntry.Severity && String.Equals(Message, logEntry.Message));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int HashingBase = (int)2166136283;
                const int HashingMultiplier = 16777619;

                int hash = HashingBase;
                hash = (hash * HashingMultiplier) ^ (!Object.ReferenceEquals(null, Severity) ? Severity.GetHashCode() : 0);
                hash = (hash * HashingMultiplier) ^ (!Object.ReferenceEquals(null, Message) ? Message.GetHashCode() : 0);
                
                return hash;
            }
        }

        public static bool operator ==(LogEntry logEntryA, LogEntry logEntryB)
        {
            if (Object.ReferenceEquals(logEntryA, logEntryB)) return true;
            if (Object.ReferenceEquals(null, logEntryA)) return false;
            return logEntryA.Equals(logEntryB);
        }
        
        public static bool operator !=(LogEntry logEntryA, LogEntry logEntryB)
        {
            return !(logEntryA == logEntryB);
        }

    }
}
