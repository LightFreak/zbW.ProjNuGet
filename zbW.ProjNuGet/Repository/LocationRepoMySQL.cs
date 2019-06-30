using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using LinqToDB;
using MySql.Data.MySqlClient;
using zbW.ProjNuGet.Model;

namespace zbW.ProjNuGet.Repository
{
    

    class LocationRepoMySql : RepositoryBase<Location>
    {
        public LocationRepoMySql(string connectionString): base(connectionString)
        {

        }
             
       
        public IQueryable<Location> GetAllHirachical(int id)
        {
            IQueryable<Location> list = Enumerable.Empty<Location>().AsQueryable();
            List<Location> Hirachical;
            using (var db = new LinqToDB.DataContext(ProviderName, ConnectionString))
            {
                try
                {
                    list = (from e in db.GetTable<Location>() where e.Parent.Equals(id) select e);
                    foreach(var entity in list)
                    {
                        entity.Child = GetAllHirachical(entity.Id);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                return list;
            }
            //List<Location> result = new List<Location>();
            //using (var con = new MySqlConnection(ConnectionString))
            //{
            //    con.Open();
            //    using (var cmd = con.CreateCommand())
            //    {
            //       // cmd.CommandText = "SELECT * FROM " + TableName + " where parent_id = " + id + ";";

            //        IDataReader reader = cmd.ExecuteReader();

            //        object[] dataRow = new object[reader.FieldCount];
            //        //----- Daten zeilenweise lesen und verarbeiten
            //        while (reader.Read())
            //        {
            //            // solange noch Daten vorhanden sind
            //            int cols = reader.GetValues(dataRow); // tatsächliches Lesen
            //          ////  var tmp = CreateEntry(reader);
            //          //  try
            //          //  {
            //          //      tmp.Child = GetAllHirachical(tmp.Id);
            //          //  }
            //          //  catch (Exception e)
            //          //  {
            //          //      throw e;
            //          //  }

            //          //  result.Add(tmp);
            //        }

            //        //----- Reader schließen
            //        reader.Close();
            //        return result;
            //    }
            //}
        }
    }
}
