using Qml.Net;

namespace Rancho.Client
{
    internal static class Program
    {
        public static QCoreApplication Application { get; private set; }

        private static int Main(string[] qmlNetArgs)
        {
            return Host.Run(qmlNetArgs, (_, application, _, runCallback) =>
            {
                Application = application;

                Qml.Net.Qml.RegisterType<Room>("Room");

                return runCallback();
            });
        }
    }
}