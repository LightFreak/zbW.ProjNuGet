using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zbW.ProjNuGet.Connection
{
    interface IEntryServices
    {
        List<LogEntry> GetEntrys(String connection);
        void ConfirmEntry(List<int> id, String connection);
        void AddEntry(LogEntry logEntry, String connection);
    }
}
