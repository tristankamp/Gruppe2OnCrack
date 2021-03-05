using System;
using System.Threading.Tasks;

namespace SimpleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Master master = new Master();
            Task.Run(master.Start);


            Slave slave1 = new Slave();
            Slave slave2 = new Slave();

            Task.Run(slave1.LavMitArbejde);
            Task.Run(slave2.LavMitArbejde);

            Console.ReadLine();
        }
    }
}