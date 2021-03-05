using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TCPLibrary.TCPServer
{
    public class TraceWorker
    {
        private TraceSource ts = new TraceSource("Wateverforever");
        private EventLogTraceListener el = new EventLogTraceListener("Application");
        public TraceWorker()
        {

            ts.Switch = new SourceSwitch("SS", "All");
            TraceListener consoleLog = new ConsoleTraceListener();
            ts.Listeners.Add(consoleLog);
            ts.Listeners.Add(el);
            TraceListener fileLog = new TextWriterTraceListener();
            ts.Listeners.Add(fileLog);

          
        }

        public void start()
        { 
            ts.TraceEvent(TraceEventType.Information,1337,"Dette er information");
            ts.TraceEvent(TraceEventType.Error, 1337, "Dette er fejl");
        }

        public void startmessage()
        {
            ts.TraceEvent(TraceEventType.Information, 1337, "Programmet er startet");
        }

        public void connectedmessage()
        {
            ts.TraceEvent(TraceEventType.Information, 1337, "A new face touches the beacon");
        }

    }
}
