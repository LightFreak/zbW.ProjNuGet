using LinqToDB.Common;
using LinqToDB.Data;
using System;
using System.Data;
using System.Data.SqlTypes;

namespace zbW.ProjNuGet.Repository
{
    public class LoggingRepoMySql : RepositoryBase<LogEntry>
    {
        public LoggingRepoMySql(string connectionString) : base(connectionString)
        {
        }

       
        public override void Add(LogEntry entity)
        {
            using (var db = new DataConnection(ProviderName , ConnectionString))
            {
                try
                {
                    string date = entity.Timestamp.ToString("yyy-MM.dd hh:mm:ss");
                    var dataParams = new DataParameter[4];
                    dataParams[0] = new DataParameter("@date", date);
                    dataParams[1] = new DataParameter("@host", entity.Hostname);
                    dataParams[2] = new DataParameter("@LogLev", entity.Severity);
                    dataParams[3] = new DataParameter("@message", entity.Message);
                    var ret = db.ExecuteProc<LogEntry>("LogMessageAdd", dataParams);
                }
                catch (SqlNullValueException ex)
                {
                    //Console.WriteLine(ex);
                    return;
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
        }


       public override void Delete(LogEntry entity)
       {
            using (var db = new DataConnection(ProviderName, ConnectionString))
            {
                try
                {

                    var ret = db.ExecuteProc<LogEntry>("LogClear",
                    new DataParameter("id", entity.Id),
                    new DataParameter("total", null, LinqToDB.DataType.Decimal) { Direction = ParameterDirection.Output, Size = 22 });
            
                    decimal? total = Converter.ChangeTypeTo<decimal?>(((IDbDataParameter)db.Command.Parameters["total"]).Value);

                }
                catch (SqlNullValueException x)
                {
                    return;
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
        }

        public override void Update(LogEntry entity)
       {
            throw new System.NotSupportedException();
       }

        //private int callConfirm(this DataConnection dataConnection, int id, out decimal? total)
        //{
        //    var ret = dataConnection.ExecuteProc<int>("LogClear",
        //                                               new DataParameter("id", id),
        //                                               new DataParameter("total", null, LinqToDB.DataType.Decimal) { Direction = ParameterDirection.Output, Size = 22 });

        //    total = Converter.ChangeTypeTo<decimal?>(((IDbDataParameter)dataConnection.Command.Parameters["ret"]).Value);

        //    return ret;
        //} 

        //public static int OUTREFTEST(this DataConnection dataConnection, decimal? PID, out decimal? POUTPUTID, ref decimal? PINPUTOUTPUTID, string PSTR, out string POUTPUTSTR, ref string PINPUTOUTPUTSTR)
        //{
        //    var ret = dataConnection.ExecuteProc("TESTUSER.OUTREFTEST",
        //        new DataParameter("PID", PID, DataType.Decimal),
        //        new DataParameter("POUTPUTID", null, DataType.Decimal) { Direction = ParameterDirection.Output, Size = 22 });

        //    POUTPUTID = Converter.ChangeTypeTo<decimal?>(((IDbDataParameter)dataConnection.Command.Parameters["POUTPUTID"]).Value);
        

        //    return ret;
        //}

    }
}

    

