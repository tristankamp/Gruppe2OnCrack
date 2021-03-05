using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleServer
{
    public static class StatelessSlave
    {
        public static void RunOneInstance()
        {
            Slave slave = new Slave();
            slave.Work();
        }
    }
}
