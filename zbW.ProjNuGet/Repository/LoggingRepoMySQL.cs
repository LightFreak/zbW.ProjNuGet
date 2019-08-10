using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;


namespace zbW.ProjNuGet.Repository
{
    public class LoggingRepoMySql : RepositoryBase<LogEntry>
    {
        /// <summary>
        /// Name der Tabelle innerhalb der Datenbank
        /// </summary>
        public override string TableName => "v_logentries";

        /// <summary>
        /// Parameter nach dem bei einer Standart Abfrage die Einträge sortiert werden.
        /// </summary>
        public override string Order => "order by timestamp";

        public override LogEntry GetSingle<P>(P pkValue)
        {
            var result = new LogEntry();
            try
            {
                using (var con = new MySqlConnection(ConnectionString))
                {
                    con.Open();
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM "+ TableName +" where id = @id; ";
                        
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
                            result = new LogEntry((int)reader["id"], (string)reader["pod"],
                                (string)reader["location"], (string)reader["hostname"],
                                (int)reader["severity"], (DateTime)reader["timestamp"],
                                (string)reader["message"]);
                            
                        }

                        //----- Reader schließen
                        reader.Close();
                        return result;
                    }

                }

            }
            catch (Exception e)
            {
                throw e;
            }
            
        }
        
        /// <summary>
        /// Fügt einen neuen Eintrag der Datenbank hinzu.
        /// </summary>
        /// <param name="entity"></param>
        public override void Add(LogEntry entity)
        {
            try
            {
                using (var con = new MySqlConnection(ConnectionString))
                {
                    con.Open();
                    using (var cmd = con.CreateCommand())
                    {
                        string date = entity.timestamp.ToString("yyy-MM-dd hh:mm:ss");
                        cmd.CommandText = "call LogMessageAdd(@date,@host,@severity,@msgParameter);";

                        var dataParameter = cmd.CreateParameter();
                        dataParameter.ParameterName = "@date";
                        dataParameter.Value = date;
                        cmd.Parameters.Add(dataParameter);
                        var hostParameter = cmd.CreateParameter();
                        hostParameter.ParameterName = "@host";
                        hostParameter.Value = entity.hostname;
                        cmd.Parameters.Add(hostParameter);
                        var severityParameter = cmd.CreateParameter();
                        severityParameter.ParameterName = "@severity";
                        severityParameter.Value = entity.severity;
                        cmd.Parameters.Add(severityParameter);
                        var msgParameter = cmd.CreateParameter();
                        msgParameter.ParameterName = "@msgParameter";
                        msgParameter.Value = entity.message;
                        cmd.Parameters.Add(msgParameter);

                        cmd.ExecuteScalar();
                        con.Close();
                       
                    }

                }

            }
            catch (Exception e)
            {
                throw e;
            }
            
        }
        
        /// <summary>
        /// Wird aufgerufen, wenn ein Eintrag bestätigt wird.
        /// </summary>
        /// <param name="entity"> Zu bestätigender Eintrag.</param>
        public override void Delete(LogEntry entity)
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
                            cmd.CommandText = "call LogClear(@id);";
                            var dataParameter = cmd.CreateParameter();
                            dataParameter.ParameterName = "@id";
                            dataParameter.Value = entity.id;
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
                    throw e;
                }
            }
        }

        public override void Update(LogEntry entity)
        {
            throw new NotImplementedException();
        }

        public override LogEntry CreateEntry(IDataReader reader)
        {
            if (reader != null)
            {
               return new LogEntry((int)reader["id"], (string)reader["pod"],
                    (string)reader["location"], (string)reader["hostname"],
                    (int)reader["severity"], (DateTime)reader["timestamp"],
                    (string)reader["message"]);
            }
            return new LogEntry();
        }

        public override List<LogEntry> GetAll(string whereCondition, Dictionary<string, Object> parameterValues)
        {
            
            List<LogEntry> result = new List<LogEntry>();
            try
            {
                using (var con = new MySqlConnection(ConnectionString))
                {
                    con.Open();
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM "+TableName+" where "+ whereCondition + Order +";";
                        foreach (var param in parameterValues)
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
                throw e;
            }

           
        }
        
        public override long Count(string whereCondition, Dictionary<string, object> parameterValues)
        {
            long result = 0;
            try
            {
                using (var con = new MySqlConnection(ConnectionString))
                {
                    con.Open();
                    using (var cmd = con.CreateCommand())
                    {

                        cmd.CommandText = "SELECT Count(*) FROM " + TableName + " where "+ whereCondition +" order by timestamp; ";
                        foreach (var param in parameterValues)
                        {
                            var parameter = cmd.CreateParameter();
                            parameter.ParameterName = param.Key;
                            parameter.Value = param.Value;
                            cmd.Parameters.Add(parameter);
                        }

                        var r = cmd.ExecuteScalar();
                        result = checked((long)r);
                        return result;
                    }

                }

            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}

    

