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
        private int _id;
        private string _pod;
        private string _location;
        private string _hostname;
        private int _severity;
        private DateTime _timestamp;
        private string _message;
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
        #region old        
        //public int id
        //{
        //    get
        //    {
        //        return _id;
        //    }
        //    set
        //    {
        //        _id = value; 

        //    }
        //}

        //public string pod
        //{
        //    get { return _pod; }
        //    set { _pod = value; }
        //}

        //public string location
        //{
        //    get { return _location; }
        //    set { _location = value; }
        //}

        //public string hostname
        //{
        //    get { return _hostname; }
        //    set { _hostname = value; }
        //}

        //public int severity
        //{
        //    get { return _severity;}
        //    set { _severity = value; }
        //}
        //public DateTime timestamp
        //{
        //    get { return _timestamp;}
        //    set { _timestamp = value; }
        //}
        //public string message
        //{
        //    get { return _message;}
        //    set { _message = value; }
        //}
        #endregion

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
        #region old
        //public LogEntry(int id, string pod, string location, string hostname,
        //    int severity, DateTime timestamp, string message)
        //{
        //    this._id = id;
        //    this._pod = pod;
        //    this._location = location;
        //    this._hostname = hostname;
        //    this._severity = severity;
        //    this._timestamp = timestamp;
        //    this._message = message;
        //    this._confirm = false;
        //    this._duplicate = false;

        //}

        //public LogEntry()
        //{
        //    this.id = 0;
        //    this.pod = "";
        //    this.location = "";
        //    this.hostname = "" ;
        //    this.severity = 0;
        //    this.timestamp = DateTime.Today;
        //    this.message = "";
        //    this.confirm = false;
        //    this.duplicate = false;


        //}
        #endregion

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
