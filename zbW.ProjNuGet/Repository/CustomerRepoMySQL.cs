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
