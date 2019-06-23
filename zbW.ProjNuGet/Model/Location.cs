using System;
using System.Collections.Generic;

namespace zbW.ProjNuGet.Model
{
    class Location
    {
       
        public Location(int id = default, string name = null, int parent = default, int podId = default, List<Location> child = null)
        {
            _id = id;
            _name = name;
            _parent = parent;
            _pod_id = podId;
            _child = child;
        }

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public int Parent
        {
            get => _parent;
            set => _parent = value;
        }

        public int PodId
        {
            get => _pod_id;
            set => _pod_id = value;
        }

        private int _id;
        private string _name;
        private int _parent;
        private int _pod_id;
        private List<Location> _child;
        
        public List<Location> Child
        {
            get => _child;
            set => _child = value;
        }

        /// <summary>
        /// OBJECT Equality...
        /// </summary>
        /// 
        public override bool Equals(object obj)
        {
            return Equals(obj as Location);
        }

        public bool Equals(Location loc)
        {
            if (Object.ReferenceEquals(null, loc)) return false;
            if (Object.ReferenceEquals(this, loc)) return true;

            return (String.Equals(_name, loc.Name) && _pod_id == loc.PodId);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int HashingBase = (int)2166136283;
                const int HashingMultiplier = 16777619;

                int hash = HashingBase;
                hash = (hash * HashingMultiplier) ^ (!Object.ReferenceEquals(null, Name) ? Name.GetHashCode() : 0);
                hash = (hash * HashingMultiplier) ^ (!Object.ReferenceEquals(null, PodId) ? PodId.GetHashCode() : 0);

                return hash;
            }
        }

        public static bool operator ==(Location locA, Location locB)
        {
            if (Object.ReferenceEquals(locA, locB)) return true;
            if (Object.ReferenceEquals(null, locA)) return false;
            return locA.Equals(locB);
        }

        public static bool operator !=(Location locA, Location locB)
        {
            return !(locA == locB);
        }
    }
}
