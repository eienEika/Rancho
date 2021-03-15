using System;

namespace Rancho.Server
{
    internal static class Program
    {
        private static Server _server;

        private static void Main(string[] args)
        {
            var ip = args[0];
            ushort port;
            ushort.TryParse(args[1], out port);

            _server = new Server(ip, port);
            _server.Start();

            while (true)
            {
                Console.Read();
            }
        }
    }
}