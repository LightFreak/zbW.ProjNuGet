using LinqToDB.Data;
using MySql.Data;


namespace zbW.ProjNuGet.Repository
{
    public class LoggingRepoMySql : RepositoryBase<LogEntry>
    {
        public LoggingRepoMySql(string connectionString) : base(connectionString)
        {
        }
       
        public void Add(LogEntry entity)
        {
            using (var db = new DataConnection(ProviderName , ConnectionString))
            {
                var dataParams = new DataParameter[4];
                dataParams[0] = new DataParameter("@i_pod", entity.Pod);
                dataParams[0] = new DataParameter("@i_hostname", entity.Hostname);
                dataParams[0] = new DataParameter("@i_severity", entity.Severity);
                dataParams[0] = new DataParameter("@i_message", entity.Message);
                db.QueryProc<LogEntry>("LogMessageAdd", dataParams);
            }
        }


       public void Delete(LogEntry entity)
       {
            using (var db = new DataConnection(ProviderName, ConnectionString))
            {
                db.QueryProc<LogEntry>("LogClear", new DataParameter("@id", entity.Id));
            }
       }

       public void Update()
       {
            throw new System.NotSupportedException();
       } 

       
    }
}

    

