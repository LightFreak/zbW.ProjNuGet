using System;
using DuplicateCheckerLib;

namespace zbW.ProjNuGet
{
    class Entry : IEntity
    {
        public int id { get; set; }
        public String pod { get; set; }
        public String location { get; set; }
        public String hostname { get; set; }
        public int severity { get; set; }
        public DateTime timestamp { get; set; }
        public String message { get; set; }
        public bool confirm { get; set; }

        public Entry(int id, string pod, string location, string hostname,
            int severity, DateTime timestamp, string message)
        {
            this.id = id;
            this.pod = pod;
            this.location = location;
            this.hostname = hostname;
            this.severity = severity;
            this.timestamp = timestamp;
            this.message = message;
            this.confirm = false;
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
