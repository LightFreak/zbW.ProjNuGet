using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using LinqToDB;
using zbW.ProjNuGet.Model;

namespace zbW.ProjNuGet.Repository
{
    

    class LocationRepoMySql : RepositoryBase<Location>
    {
        public LocationRepoMySql(string connectionString): base(connectionString)
        {

        }
             
       
        public List<Location> GetAllHirachical(int id, List<Location> source)
        {
            List<Location> result = new List<Location>();
            foreach(var entity in source)
            {
                if(entity.Parent == id)
                {
                    entity.Child = GetAllHirachical(entity.Id, source);
                    result.Add(entity);
                }
            }
            return result;
            
        }
    }
}
