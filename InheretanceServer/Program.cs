using InheretanceServer.Server;

namespace InheretanceServer
{
    class Program
    {
        static void Main(string[] args)
        {
            MyServer InheritedServer = new MyServer();
            InheritedServer.Start();
        
        }
    }
}