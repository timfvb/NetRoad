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
    private TcpListener _listener;
    
    public ServerTest()
    {
        _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 9991);
    }

    public void Run()
    {
        _listener.Start();

        Console.WriteLine("ServerListener started");
        
        Receive();
    }

    private void Receive()
    {
        while (true)
        {
            var client = _listener.AcceptTcpClient();
            var writer = new StreamWriter(client.GetStream(), Encoding.UTF8);
            var reader = new StreamReader(client.GetStream(), Encoding.UTF8);

            while (!reader.EndOfStream && client.Connected)
            {
                var read = reader.ReadLine();
                writer.WriteLine(read);
                writer.Flush();
                Console.WriteLine("read:\t" + read);    
            }
        }
    }
}