using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Xml;

namespace TCPLibrary.TCPServer
{

    public class JsonTraceListener : TraceListener
    {
        private StreamWriter sw = new StreamWriter("Abstractserver.Json");
        public override void Write(string message)
        {
            string json = JsonSerializer.Serialize(message);

            
            sw.Write(json);
            sw.Flush();
        }

        public override void WriteLine(string message)
        {
            string json = JsonSerializer.Serialize(message);

            sw.Write(json);
            sw.Flush();
        }

    }
}
