using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zbW.ProjNuGet.Model
{
    [Table("Customer")]
    class Customer : ModelBase
    {
        [Column("id"), PrimaryKey, NotNull]
        public override int Id { get; set; }

        [Column("Customer_id")]
        public string Customer_Id { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("website")]
        public string Website { get; set; }

        [Column("password")]
        public string Password { get; set; }
    }
}
