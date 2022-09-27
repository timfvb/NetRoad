/*
 * ===============================================
 * N-ROAD OPEN SOURCE NETWORK LIBRARY PROJECT
 * AUTHOR: https://github.com/timfvb
 * ===============================================
 */

using System.Net;
using System.Text;

namespace NetRoad.Test.Tests;

public class ClientTest
{
    private readonly NRoadTcpClient _client;
    
    public ClientTest()
    {
        _client = new NRoadTcpClient("127.0.0.1", 9991, Encoding.UTF8);
        
        _client.Connected += ClientOnConnected;
        _client.Disconnected += ClientOnDisconnected;
        _client.DataReceived += ClientOnDataReceived;
    }

    private void ClientOnDisconnected(object sender) => Console.WriteLine("Raised:\tDisconnected!");

    private void ClientOnConnected(object sender) => Console.WriteLine("Raised:\tConnected!");
    
    private void ClientOnDataReceived(object sender, string e) => Console.WriteLine("Output: " + e);

    public void Run()
    {
        var connectable = _client.Connect();

        Console.WriteLine("Connectable\t{0}", connectable);
        
        if (connectable)
            _client.Send("Hello World!");
    }
}