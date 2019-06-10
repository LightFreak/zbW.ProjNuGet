using System.Collections.Generic;
using System.Data;
using System;
using MySql.Data.MySqlClient;

namespace zbW.ProjNuGet.Connection
{
    /// <summary>
    /// Hilfsklasse zur Datenbankverbindung
    /// </summary>
    class EntryServices : IEntryServices
    {
        
        public void AddEntry(LogEntry logEntry, String connection)
        {
            //string date = logEntry.timestamp.ToString("yyyy-MM-dd hh:mm:ss");
            IDbConnection con = null; // Verbindung deklarieren
            try
            {
                string date = logEntry.timestamp.ToString("yyyy-MM-dd hh:mm:ss");
                con = new MySqlConnection(connection); //Verbindung erzeugen
                con.Open();
                //----- SQL-Kommando aufbauen
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandText = "call LogMessageAdd('" + date + "','" + logEntry.hostname + "'," + logEntry.severity + ",'" + logEntry.message + "');";
                //----- SQL-Kommando ausführen; liefert einen OleDbDataReader
                cmd.ExecuteNonQuery();
                
                //----- Reader schließen
                con.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                try
                {
                    if (con != null)
                    {
                        // Verbindung schließen
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void ConfirmEntry(List<int> id, String connection)
        {
            
            IDbConnection con = null; // Verbindung deklarieren
            try
            {
                con = new MySqlConnection(connection); //Verbindung erzeugen
                con.Open();
                //----- SQL-Kommando aufbauen
                IDbCommand cmd = con.CreateCommand();
                foreach (int i in id)
                {
                    cmd.CommandText = "call LogClear(" + i + ");";
                    //----- SQL-Kommando ausführen; liefert einen OleDbDataReader
                    cmd.ExecuteNonQuery();
                }
                //----- Reader schließen
                con.Close();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                try
                {
                    if (con != null)
                    {
                        // Verbindung schließen
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
                   
        
        public List<LogEntry> GetEntrys(String connection)
        {
            List<LogEntry> result = new List<LogEntry>();

            IDbConnection con = null; // Verbindung deklarieren
            try
            {
                con = new MySqlConnection(connection); //Verbindung erzeugen

                con.Open();
                //----- SQL-Kommando aufbauen
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT id,pod,location,hostname,severity,timestamp,message FROM v_logentries order by timestamp;";
                //----- SQL-Kommando ausführen; liefert einen OleDbDataReader
                IDataReader reader = cmd.ExecuteReader();
                

                object[] dataRow = new object[reader.FieldCount];
                //----- Daten zeilenweise lesen und verarbeiten
                while (reader.Read())
                {
                    // solange noch Daten vorhanden sind
                    int cols = reader.GetValues(dataRow); // tatsächliches Lesen 
                    var curEntry = new LogEntry((int)reader["id"], (string)reader["pod"], (string)reader["location"], (string)reader["hostname"], (int)reader["severity"], (DateTime)reader["timestamp"], (string)reader["message"]);
                    result.Add(curEntry);
                }

                //----- Reader schließen
                reader.Close();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
                    Console.WriteLine(ex.Message);
                }
            }

            Console.ReadLine();
            return new List<LogEntry>();

        }
    
    }
}
