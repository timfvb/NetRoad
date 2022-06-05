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
    private readonly NRoadClient _client;
    public ClientTest()
    {
        _client = new NRoadClient("127.0.0.1", 9991, Encoding.UTF8);    
    }
    
    public void Run()
    {
        var connectable = _client.Connect();
        Console.WriteLine(connectable);
        if (connectable)
            _client.Send("Hello World!");
    }
}