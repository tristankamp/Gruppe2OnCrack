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
            
        }

        public void startmessage()
        {
            
        }

        public void connectedmessage()
        {
            
        }

    }
}
