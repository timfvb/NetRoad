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
    public readonly TcpClient NetClient;
    private readonly IPEndPoint _endPoint;
    private readonly Encoding _encoding;

    public Client(IPEndPoint endPoint, Encoding encoding)
    {
        NetClient = new TcpClient();
        _endPoint = endPoint;
        _encoding = encoding;
    }

    public bool Connect()
    {
        try
        {
            NetClient.Connect(_endPoint);
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
        if (!NetClient.Connected)
            throw new Exception("NRoadClient is not connected");

        // Set send timeout
        NetClient.SendTimeout = timeout; 
        
        // Create a network writer
        var writer = new StreamWriter(NetClient.GetStream(), _encoding);
        
        // Send bytes to the destination
        writer.WriteLine(content);
        writer.Flush();
    }
    
    public void Send(byte[] content, int timeout)
    {
        // Check if client is connected
        if (!NetClient.Connected)
            throw new Exception("NRoadClient is not connected");

        // Set send timeout
        NetClient.SendTimeout = timeout;
        
        // Decode byte array to string
        var decoded = _encoding.GetString(content);

        // Create a network writer
        var writer = new StreamWriter(NetClient.GetStream(), _encoding);
        
        // Send bytes to the destination
        writer.WriteLine(decoded);
        writer.Flush();
    }
}