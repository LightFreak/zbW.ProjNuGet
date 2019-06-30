using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace zbW.ProjNuGet.Model
{
    [Table("Location")]
    class Location : ModelBase
    {
        private int _id;
        private string _name;
        private int _parent;
        private int _pod_id;
        private IQueryable<Location> _child;

        [Column("id"), PrimaryKey, NotNull]
        public override int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("parent")]
        public int Parent { get; set; }

        [Column("pod_id")]
        public int Pod_ID { get; set; }
                               
        public IQueryable<Location> Child
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

            return (String.Equals(_name, loc.Name) && _pod_id == loc.Pod_ID);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int HashingBase = (int)2166136283;
                const int HashingMultiplier = 16777619;

                int hash = HashingBase;
                hash = (hash * HashingMultiplier) ^ (!Object.ReferenceEquals(null, Name) ? Name.GetHashCode() : 0);
                hash = (hash * HashingMultiplier) ^ (!Object.ReferenceEquals(null, Pod_ID) ? Pod_ID.GetHashCode() : 0);

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
