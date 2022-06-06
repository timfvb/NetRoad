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

    public delegate void ConnectionEventHandler(object sender);

    
    public event ConnectionEventHandler? OnConnect;
    public event ConnectionEventHandler? OnDisconnect;

    public Client(IPEndPoint endPoint, Encoding encoding, bool enableTcpListener)
    {
        NetClient = new TcpClient();
        _endPoint = endPoint;
        _encoding = encoding;
    }

    public bool Connect()
    {
        try
        {
            // Try to connect to destination
            NetClient.Connect(_endPoint);
            
            // Invoke Connection Success Event
            OnConnect?.Invoke(this);
            return true;
        }
        catch (Exception e)
        {
            // Destination not reachable
            if (e is SocketException)
                return false;
            throw;
        }
    }
    
    public void Disconnect()
    {
        // Check if client still connected
        if (!NetClient.Connected)
            throw new Exception("Client is not connected");
        
        // Close current network connection
        NetClient.Close();
        
        // Invoke Disconnection Event
        OnDisconnect?.Invoke(this);
    }

    public void Send(string content, int timeout)
    {
        // Check if client is connected
        if (!NetClient.Connected)
            OnDisconnect?.Invoke(this);

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
            OnDisconnect?.Invoke(this);

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