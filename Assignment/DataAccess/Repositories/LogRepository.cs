using Assignment.DataAccess.Interfaces;
using Google.Api;
using Google.Cloud.Logging.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment.DataAccess.Repositories
{
    public class LogRepository : ILogRepository
    {

        public void Log(string message)
        {
            var loggingClient = LoggingServiceV2Client.Create();

            LogName logName = new LogName("pristine-abacus-307313","cloudAssignmentLogs");

            MonitoredResource res = new MonitoredResource();
            res.Type = "global";

            List<LogEntry> entries = new List<LogEntry>();
            entries.Add(new LogEntry() { LogName = logName.ToString(), Severity = Google.Cloud.Logging.Type.LogSeverity.Error, TextPayload = message});

            loggingClient.WriteLogEntries(logName, res, null, entries);
        }
    }
}
