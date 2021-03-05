using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SimpleServer
{
    class Program
    {
        private const int TASK_COUNT = 8;

        static void Main(string[] args)
        {
            Master master = new Master();
            Task.Run(master.Start);

            Task[] tasks = new Task[TASK_COUNT];
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < TASK_COUNT; i++)
            {
                tasks[i] = Task.Run(StatelessSlave.RunOneInstance);
            }

            Task.WaitAll(tasks);

            stopwatch.Stop();

            Console.WriteLine(stopwatch.Elapsed);

            Console.ReadLine();
        }
    }
}