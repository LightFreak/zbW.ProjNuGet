using System;
using DuplicateCheckerLib;

namespace zbW.ProjNuGet
{
    class Entry : IEntity
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

        public int id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value; 

            }
        }

        public String pod
        {
            get { return _pod; }
            set { _pod = value; }
        }

        public String location
        {
            get { return _location; }
            set { _location = value; }
        }

        public String hostname
        {
            get { return _hostname; }
            set { _hostname = value; }
        }

        public int severity
        {
            get { return _severity;}
            set { _severity = value; }
        }
        public DateTime timestamp
        {
            get { return _timestamp;}
            set { _timestamp = value; }
        }
        public String message
        {
            get { return _message;}
            set { _message = value; }
        }
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

        public Entry(int id, string pod, string location, string hostname,
            int severity, DateTime timestamp, string message)
        {
            this._id = id;
            this._pod = pod;
            this._location = location;
            this._hostname = hostname;
            this._severity = severity;
            this._timestamp = timestamp;
            this._message = message;
            this._confirm = false;
            this._duplicate = false;
            
            
        }

        public Entry()
        {
            this.id = 0;
            this.pod = "";
            this.location = "";
            this.hostname = "" ;
            this.severity = 0;
            this.timestamp = DateTime.Today;
            this.message = "";
            this.confirm = false;
            this.duplicate = false;
        }


        /// <summary>
        /// OBJECT Equality....
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Entry);
        }

        public bool Equals(Entry entry)
        {
            if (Object.ReferenceEquals(null, entry)) return false;
            if (Object.ReferenceEquals(this, entry)) return true;

            return (severity == entry.severity && String.Equals(message, entry.message));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int HashingBase = (int)2166136283;
                const int HashingMultiplier = 16777619;

                int hash = HashingBase;
                hash = (hash * HashingMultiplier) ^ (!Object.ReferenceEquals(null, severity) ? severity.GetHashCode() : 0);
                hash = (hash * HashingMultiplier) ^ (!Object.ReferenceEquals(null, message) ? message.GetHashCode() : 0);
                
                return hash;
            }
        }

        public static bool operator ==(Entry entryA, Entry entryB)
        {
            if (Object.ReferenceEquals(entryA, entryB)) return true;
            if (Object.ReferenceEquals(null, entryA)) return false;
            return entryA.Equals(entryB);
        }
        
        public static bool operator !=(Entry entryA, Entry entryB)
        {
            return !(entryA == entryB);
        }

    }
}
