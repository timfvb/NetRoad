/*
 * ===============================================
 * N-ROAD OPEN SOURCE NETWORK LIBRARY PROJECT
 * AUTHOR: https://github.com/timfvb
 * ===============================================
 */

using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetRoad.Test.Tests;

public class ServerTest
{
    public void Run()
    {
        var server = new NRoadServer("127.0.0.1", 9991, Encoding.UTF8);
        server.DataReceived += ServerOnDataReceived;
        
        server.Start();
    }

    private void ServerOnDataReceived(object sender, string e)
    {
        Console.WriteLine("Server received data: " + e);
    }
}