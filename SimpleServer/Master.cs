using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using TCPLibrary.TCPServer;

namespace SimpleServer
{
    class Master : AbstractTCPServer
    {
        private readonly List<string> _dict;
        private readonly BlockingCollection<string> _passwords;
        private readonly List<string> _solvedPasswords;
        public Master()
        {
            _dict = new List<string>();
            _passwords = new BlockingCollection<string>();
            _solvedPasswords = new List<string>();

            FileStream fs = new FileStream(Paths.DICT_PATH, FileMode.Open, FileAccess.Read);

            StreamReader sr = new StreamReader(fs);

            while (!sr.EndOfStream)
            {
                _dict.Add(sr.ReadLine());
            }

            sr.Close();

            FileStream fsp = new FileStream(Paths.PASSWORD_PATH, FileMode.Open, FileAccess.Read);

            StreamReader srp = new StreamReader(fsp);

            while (!srp.EndOfStream)
            {
                _passwords.Add(srp.ReadLine());
            }

            srp.Close();
        }

        protected override void TcpServerWork(StreamReader sr, StreamWriter sw, int PortNumber, string Name)
        {
            sw.AutoFlush = true;

            while (true)
            {
                switch (sr.ReadLine())
                {
                    case "0": SendDictionary(sw); break;
                    case "1": SendSinglePassword(sw); break;
                    case "2": GetSolvedPassword(sr); break;
                    case "Disconnect": return;
                }
            }
        }

        private void GetSolvedPassword(StreamReader sr)
        {
            _solvedPasswords.Add(sr.ReadLine());
            foreach (string password in _solvedPasswords)
            {
                Console.WriteLine(password);
            }
        }

        private void SendSinglePassword(StreamWriter sw)
        {
            if (_passwords.Count > 0)
            {
                sw.WriteLine(_passwords.Take());
                Console.WriteLine("passwords given");
            }
            else sw.WriteLine("fuck af");
        }

        private void SendDictionary(StreamWriter sw)
        {
            foreach (string line in _dict)
            {
                sw.WriteLine(line);
            }

            Console.WriteLine("dict given");
            sw.WriteLine("1029384756");
        }
    }
}