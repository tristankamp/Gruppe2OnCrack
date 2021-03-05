using System;
using System.Diagnostics;
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
            Slave slave3 = new Slave();
            Slave slave4 = new Slave();
            Slave slave5 = new Slave();
            Slave slave6 = new Slave();
            Slave slave7 = new Slave();
            Slave slave8 = new Slave();

            Stopwatch stopwatch = Stopwatch.StartNew();



            Task.WaitAll(
                Task.Run(slave1.LavMitArbejde),
                Task.Run(slave2.LavMitArbejde),
                Task.Run(slave3.LavMitArbejde),
                Task.Run(slave4.LavMitArbejde), 
                Task.Run(slave5.LavMitArbejde),
                Task.Run(slave6.LavMitArbejde),
                Task.Run(slave7.LavMitArbejde),
                Task.Run(slave8.LavMitArbejde));


            stopwatch.Stop();

            Console.WriteLine(stopwatch.Elapsed);

            Console.ReadLine();
        }
    }
}