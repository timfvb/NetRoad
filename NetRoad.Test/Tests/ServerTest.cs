/*
 * ===============================================
 * N-ROAD OPEN SOURCE NETWORK LIBRARY PROJECT
 * AUTHOR: https://github.com/timfvb
 * ===============================================
 */

using System.Text;

namespace NetRoad.Test.Tests;

public class ServerTest
{
    private NRoadTcpServer _server;
    
    public void Run()
    {
        _server = new NRoadTcpServer("127.0.0.1", 9991, Encoding.UTF8);
        _server.DataReceived += ServerOnDataReceived;
        
        _server.Start();
    }

    private void ServerOnDataReceived(object sender, string e)
    {
        Console.WriteLine("Server received data: " + e);
        
        _server.SendToConnectedClients("hello this is a global message");
    }
}