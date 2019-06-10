using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using zbW.ProjNuGet.Model;

namespace zbW.ProjNuGet.Repository
{
    

    class LocationRepoMySql : RepositoryBase<Location>
    {
        public override long Count(string whereCondition, Dictionary<string, object> parameterValues)
        {
            throw new NotImplementedException();
        }

        public override string TableName => "location";
        public override string Order => "";

        public override string ConnectionString
        {
            get => base.ConnectionString;
            set => base.ConnectionString = value;
        }
        /// <summary>
        /// Abfrage einer einzelnen Location
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <param name="pkValue"></param>
        /// <returns></returns>
        public override Location GetSingle<P>(P pkValue)
        {
            var result = new Location();
            try
            {
                using (var con = new MySqlConnection(ConnectionString))
                {
                    con.Open();
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM " + TableName + " where id = @id; ";

                        var param1 = cmd.CreateParameter();
                        param1.ParameterName = "id";
                        param1.Value = pkValue;
                        cmd.Parameters.Add(param1);

                        IDataReader reader = cmd.ExecuteReader();

                        object[] dataRow = new object[reader.FieldCount];
                        //----- Daten zeilenweise lesen und verarbeiten
                        while (reader.Read())
                        {
                            // solange noch Daten vorhanden sind
                            int cols = reader.GetValues(dataRow); // tatsächliches Lesen 
                            result = new Location((int)reader["id"], (string)reader["name"],
                                (int)reader["parent_id"], (int)reader["pod_id"]);

                            return result;

                        }

                        //----- Reader schließen
                        reader.Close();
                        return result;
                    }

                }

            }
            catch (Exception e)
            {
                throw;
            }
            return result;
        }
        /// <summary>
        /// Hinzufügen einer Location
        /// </summary>
        /// <param name="entity"></param>
        public override void Add(Location entity)
        {
            try
            {
                using (var con = new MySqlConnection(ConnectionString))
                {
                    con.Open();
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO `"+ TableName +"` (`id`, `name`, `POD_id`,`Parent_id`)";
                        cmd.CommandText += "VALUES(null,@name,@pod,@parent);";

                        var dataParameter = cmd.CreateParameter();
                        dataParameter.ParameterName = "@name";
                        dataParameter.Value = entity.Name;
                        cmd.Parameters.Add(dataParameter);
                        var hostParameter = cmd.CreateParameter();
                        hostParameter.ParameterName = "@pod";
                        hostParameter.Value = entity.PodId;
                        cmd.Parameters.Add(hostParameter);
                        var severityParameter = cmd.CreateParameter();
                        severityParameter.ParameterName = "@parent";
                        severityParameter.Value = entity.Parent;
                        cmd.Parameters.Add(severityParameter);
                        
                        cmd.ExecuteScalar();
                        con.Close();

                    }

                }

            }
            catch (Exception e)
            {
               Console.WriteLine(e);
            }
        }
        /// <summary>
        /// Löschen einer Location
        /// </summary>
        /// <param name="entity"></param>
        public override void Delete(Location entity)
        {
            if (entity != null)
            {
                try
                {
                    using (var con = new MySqlConnection(ConnectionString))
                    {
                        con.Open();
                        using (var cmd = con.CreateCommand())
                        {
                            cmd.CommandText = "DELETE FROM `" + TableName + "` WHERE id = @id;";
                            var dataParameter = cmd.CreateParameter();
                            dataParameter.ParameterName = "@id";
                            dataParameter.Value = entity.Id;
                            cmd.Parameters.Add(dataParameter);

                            //----- SQL-Kommando ausführen;
                            cmd.ExecuteNonQuery();
                            //----- Reader schließen;
                            con.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// Anpassen einer Location
        /// </summary>
        /// <param name="entity"></param>
        public override void Update(Location entity)
        {
            if (entity != null)
            {
                try
                {
                    using (var con = new MySqlConnection(ConnectionString))
                    {
                        con.Open();
                        using (var cmd = con.CreateCommand())
                        {
                            cmd.CommandText = "REPLACE INTO "+ TableName +" VALUES (@id,@name,@pod,@parent);";
                            var idParameter = cmd.CreateParameter();
                            idParameter.ParameterName = "@id";
                            idParameter.Value = entity.Id;
                            cmd.Parameters.Add(idParameter);
                            var namePara = cmd.CreateParameter();
                            namePara.ParameterName = "@name";
                            namePara.Value = entity.Name;
                            cmd.Parameters.Add(namePara);
                            var podParameter = cmd.CreateParameter();
                            podParameter.ParameterName = "@pod";
                            podParameter.Value = entity.PodId;
                            cmd.Parameters.Add(podParameter);
                            var parentParameter = cmd.CreateParameter();
                            parentParameter.ParameterName = "@parent";
                            parentParameter.Value = entity.Parent;
                            cmd.Parameters.Add(parentParameter);
                            //----- SQL-Kommando ausführen;
                            cmd.ExecuteNonQuery();
                            //----- Reader schließen;
                            con.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public override List<Location> GetAll(string whereCondition, Dictionary<string, object> parameterValue)
        {
            List<Location> result = new List<Location>();
            try
            {
                using (var con = new MySqlConnection(ConnectionString))
                {
                    con.Open();
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM " + TableName + " where " + whereCondition + ";";
                        foreach (var param in parameterValue)
                        {
                            var parameter = cmd.CreateParameter();
                            parameter.ParameterName = param.Key;
                            parameter.Value = param.Value;
                            cmd.Parameters.Add(parameter);
                        }

                        IDataReader reader = cmd.ExecuteReader();


                        object[] dataRow = new object[reader.FieldCount];
                        //----- Daten zeilenweise lesen und verarbeiten
                        while (reader.Read())
                        {
                            // solange noch Daten vorhanden sind
                            int cols = reader.GetValues(dataRow); // tatsächliches Lesen 
                            result.Add(CreateEntry(reader));
                        }

                        //----- Reader schließen
                        reader.Close();
                        return result;
                    }

                }

            }
            catch (Exception e)
            {
                throw;
            }
            return new List<Location>();
        }

        public override Location CreateEntry(IDataReader reader)
        {
            if (reader != null)
            {
                return new Location((int)reader["id"], (string)reader["name"],
                    (int)reader["parent_id"], (int)reader["pod_id"]);
            }
            return new Location();
        }
    }
}
