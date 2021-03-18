using System;

namespace Rancho.Server
{
    internal static class Program
    {
        public static Server Server;

        private static void Main(string[] args)
        {
            var ip = args[0];
            ushort port;
            ushort.TryParse(args[1], out port);

            Server = new Server(ip, port);
            Server.Start();

            while (true)
            {
                Console.Read();
            }
        }
    }
}