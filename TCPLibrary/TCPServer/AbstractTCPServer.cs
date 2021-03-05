using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;

namespace TCPLibrary.TCPServer
{
    public abstract class AbstractTCPServer
    {
        private int PortNumber = 7;
        private string Name = "Kage";
        private bool Running = true;
        private List<Task> tasks = new List<Task>();
        private TraceWorker tw = new TraceWorker();
        private JsonTraceListener jl = new JsonTraceListener();

        public AbstractTCPServer()
        {
            tw.start();
           
        }



        public void Start()
        {
            jl.Write("KageKageKage");
            tw.startmessage();
            
            TcpListener server = new TcpListener(IPAddress.Loopback, PortNumber);
            server.Start();

            Task.Run(StopListener);

            String path = Environment.GetEnvironmentVariable("AbstractServerConf");

            XmlDocument configDoc = new XmlDocument();
            configDoc.Load(path);

            XmlNode xxNode = configDoc.DocumentElement.SelectSingleNode("ServerPort");
            if (xxNode != null)
            {
                String xxStr = xxNode.InnerText.Trim();
                int xx = Convert.ToInt32(xxStr);
                Console.WriteLine(xx);
            }

            while (Running)
            {
                if (server.Pending())
                {
                    TcpClient socket = server.AcceptTcpClient(); // venter på client

                    // starter ny tråd
                    tasks.Add(Task.Run(
                        // indsætter en metode (delegate)
                        () =>
                        {
                            TcpClient tmpsocket = socket;
                            DoClient(tmpsocket);
                            tw.connectedmessage();
                        }
                    ));
                }
            }
            Task.WaitAll(tasks.ToArray());
        }


        private void Shutdown()
        {
            Running = false;
        }

        private void StopListener()
        {
            TcpListener server = new TcpListener(IPAddress.Loopback, PortNumber+1);
            server.Start();
            TcpClient socket = server.AcceptTcpClient();
            Shutdown();

        }

        private void DoClient(TcpClient socket)
        {
            using (StreamReader sr = new StreamReader(socket.GetStream()))
            using (StreamWriter sw = new StreamWriter(socket.GetStream()))
            {
                TcpServerWork(sr, sw, PortNumber, Name);
            }

            socket?.Close();
        }

        protected abstract void TcpServerWork(StreamReader sr, StreamWriter sw, int PortNumber, string Name);



    }
}
