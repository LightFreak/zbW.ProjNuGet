using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zbW.ProjNuGet.Model;
using zbW.ProjNuGet.Repository;

namespace zbW.ProjNuGet
{
    class CustomerRepoMySQL : RepositoryBase<Customer>
    {
        public CustomerRepoMySQL(string connectionString):base(connectionString)
        {

        }
    }
}
