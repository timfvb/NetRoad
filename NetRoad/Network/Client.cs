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
    public readonly Socket Socket;
    private readonly EndPoint _endPoint;
    private readonly Encoding _encoding;

    public Client(EndPoint endPoint, Encoding encoding)
    {
        Socket = new Socket(GetCurrentAddressFamily(), SocketType.Stream, ProtocolType.Tcp);
        _endPoint = endPoint;
        _encoding = encoding;
    }

    public bool Connect()
    {
        try
        {
            Socket.Connect(_endPoint);
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

    public void Send(string content, int timeout)
    {
        // Check if client is connected
        if (!Socket.Connected)
            throw new Exception("NRoadClient is not connected");

        // Set send timeout
        Socket.SendTimeout = timeout; 
            
        // Encode string to byte array
        var bytes = _encoding.GetBytes(content);
        
        // Send bytes to the destination
        Socket.Send(bytes);
    }
    
    public void Send(byte[] content, int timeout)
    {
        // Check if client is connected
        if (!Socket.Connected)
            throw new Exception("NRoadClient is not connected");

        // Set send timeout
        Socket.SendTimeout = timeout;

        // Send bytes to the destination
        Socket.Send(content);
    }

    private static AddressFamily GetCurrentAddressFamily()
    {
        var hostInfo = Dns.GetHostEntry(Dns.GetHostName());  
        var ipAddress = hostInfo.AddressList[0];
        
        return ipAddress.AddressFamily;
    }
}