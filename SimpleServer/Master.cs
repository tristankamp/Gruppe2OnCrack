using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using TCPLibrary.TCPServer;

namespace SimpleServer
{
    class Master:AbstractTCPServer
    {
        List<string> dict = new List<string>();
        public BlockingCollection<string> passwords = new BlockingCollection<string>();
        List<string> SolvedPasswords = new List<string>();
        public Master()
        {
            
            FileStream fs = new FileStream(@"C:\Users\trist\Documents\skole\advancedprogramming\SimpleServer\webster-dictionary.txt", FileMode.Open, FileAccess.Read);

            StreamReader sr = new StreamReader(fs);

            while (!sr.EndOfStream)
            {
                dict.Add(sr.ReadLine());
            }

            FileStream fsp = new FileStream(@"C:\Users\trist\Documents\skole\advancedprogramming\SimpleServer\passwords.txt", FileMode.Open, FileAccess.Read);

            StreamReader srp = new StreamReader(fsp);

            while (!srp.EndOfStream)
            {
                passwords.Add(srp.ReadLine());
            }

            sr.Close();
            srp.Close();

        }

        protected override void TcpServerWork(StreamReader sr, StreamWriter sw, int PortNumber, string Name)
        {
            sw.AutoFlush = true;
            
            switch (sr.ReadLine())
            {
                case "0":
                    foreach (string line in dict)
                    {
                        sw.WriteLine(line);
                        
                    }
                    break;

                case "1":
                    if (passwords.Count > 0)
                    {
                        sw.WriteLine(passwords.Take());
                        Console.WriteLine("passwords given");
                    }
                    else sw.WriteLine("fuck af");

                    break;

                case "2":
                    SolvedPasswords.Add(sr.ReadLine());
                    foreach (string password in SolvedPasswords)
                    {
                        Console.WriteLine(password);
                    }
                    break;
            }


        }
    }
}