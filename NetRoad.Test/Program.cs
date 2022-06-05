/*
 * ===============================================
 * N-ROAD OPEN SOURCE NETWORK LIBRARY PROJECT
 * AUTHOR: https://github.com/timfvb
 * ===============================================
 */

using System.Text;
using NetRoad.Test.Tests;

namespace NetRoad.Test
{
    class Program
    {
        public static void Main(string[] args)
        {
            // Argument Condition
            if (args.Length == 0 | args.Length > 1)
                ArgumentsWrong();

            if (!int.TryParse(args[0], out var type))
                ArgumentsWrong();

            switch (type)
            {
                case 0:
                    var clientTest = new ClientTest();
                    clientTest.Run();
                    break;
                case 1:
                    var serverTest = new ServerTest();
                    serverTest.Run();
                    break;
                default:
                    ArgumentsWrong();
                    break;
            }
        }

        private static void ArgumentsWrong()
        {
            Console.WriteLine("Arguments are wrong.");
            Console.WriteLine("[0]\tNRoadClient");
            Console.WriteLine("[1]\tNRoadServer");
            Environment.Exit(0x1);
        }
    }
}