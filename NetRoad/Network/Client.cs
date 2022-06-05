/*
 * ===============================================
 * N-ROAD OPEN SOURCE NETWORK LIBRARY PROJECT
 * AUTHOR: https://github.com/timfvb
 * ===============================================
 */

using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetRoad.Network;

public class Client
{
    public readonly TcpClient tClient;
    public readonly Socket clientSocket;
    private readonly IPEndPoint _endPoint;
    private readonly Encoding _encoding;

    public Client(IPEndPoint endPoint, Encoding encoding)
    {
        tClient = new TcpClient();
        clientSocket = tClient.Client;
        _endPoint = endPoint;
        _encoding = encoding;
    }

    public bool Connect()
    {
        try
        {
            tClient.Connect(_endPoint);
            return true;
        }
        catch (Exception e)
        {
            // Destination not reachable
            Console.WriteLine(e.Message);
            if (e is SocketException)
                return false;
            
            throw;
        }
    }

    public void Send(string content, int timeout)
    {
        // Check if client is connected
        if (!tClient.Connected)
            throw new Exception("NRoadClient is not connected");

        // Set send timeout
        tClient.SendTimeout = timeout; 
            
        // Encode string to byte array
        var bytes = _encoding.GetBytes(content);
        
        var writer = new StreamWriter(tClient.GetStream(), _encoding);
        
        // Send bytes to the destination
        writer.WriteLine(bytes);
        writer.Flush();
    }
    
    public void Send(byte[] content, int timeout)
    {
        // Check if client is connected
        if (!tClient.Connected)
            throw new Exception("NRoadClient is not connected");

        // Set send timeout
        tClient.SendTimeout = timeout;

        var writer = new StreamWriter(tClient.GetStream(), _encoding);
        
        // Send bytes to the destination
        writer.WriteLine(content);
        writer.Flush();
    }
}