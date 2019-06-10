using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GenericRepository;
using MySql.Data.MySqlClient;

namespace zbW.ProjNuGet.Repository
{
    public abstract class RepositoryBase<M> : IRepositoryBase<M>
    {
        protected RepositoryBase()
        {
            this.ConnectionString = "<ConnectionString>";
        }

        public virtual string ConnectionString { get; set; }

        public void SetConnection(string connection)
        {
            ConnectionString = connection;
        }

        public abstract M GetSingle<P>(P pkValue);

        public abstract void Add(M entity);

        public abstract void Delete(M entity);
        
        public abstract void Update(M entity);
        
        public abstract List<M> GetAll(string whereCondition, Dictionary<string, object> parameterValue);

        public List<M> GetAll()
        {
            List<M> result = new List<M>();

            IDbConnection con = null; // Verbindung deklarieren
            try
            {

                con = new MySqlConnection(ConnectionString); //Verbindung erzeugen

                con.Open();
                //----- SQL-Kommando aufbauen
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT * FROM "+ TableName + " " + Order + ";";
                //----- SQL-Kommando ausführen; liefert einen OleDbDataReader
                IDataReader reader = cmd.ExecuteReader();


                object[] dataRow = new object[reader.FieldCount];
                //----- Daten zeilenweise lesen und verarbeiten
                while (reader.Read())
                {
                    // solange noch Daten vorhanden sind
                    int cols = reader.GetValues(dataRow); // tatsächliches Lesen 
                    var curEntry = CreateEntry(reader);
                    result.Add(curEntry);
                }

                //----- Reader schließen
                reader.Close();
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                try
                {
                    if (con != null)
                        // Verbindung schließen
                        con.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            

        }
    

        public virtual IQueryable<M> Query(string whereCondition, Dictionary<string, object> parameterValues)
        {
            throw new NotImplementedException();
        }

        public abstract long Count(string whereCondition, Dictionary<string, object> parameterValues);
        
        public virtual long Count()
        {
            long result = 0;
            try
            {
                using (var con = new MySqlConnection(ConnectionString))
                {
                    con.Open();
                    using (var cmd = con.CreateCommand())
                    {
                        
                        cmd.CommandText = "SELECT Count(*) FROM " + TableName +" order by timestamp; ";
                        
                        var r = cmd.ExecuteScalar();
                        result = checked((long) r);
                        return result;
                    }

                }

            }
            catch (Exception e)
            {
                throw e;
            }
            
        }

        public abstract string TableName { get; }
        public abstract string Order { get; }

        public abstract M CreateEntry(IDataReader reader);

    }
}
