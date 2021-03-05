using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using TCPLibrary.TCPServer;

namespace InheretanceServer.Server
{
    public class MyServer:AbstractTCPServer
    {
        private int PortNumber = 7;
        private string Name = "Kage"; 
        

        protected override void TcpServerWork(StreamReader sr, StreamWriter sw, int PortNumber, string Name)
        {
            string str = sr.ReadLine();
            sw.WriteLine(str.ToUpper());
            Console.WriteLine($"Started server {Name} at port " + PortNumber);
            sw.Flush();
        }
    }
}
