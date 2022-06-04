/*
 * ===============================================
 * N-ROAD OPEN SOURCE NETWORK LIBRARY PROJECT
 * BY https://github.com/timfvb
 * ===============================================
 */

using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetRoad.Network;

public class Client
{
    private Socket _socket;
    private EndPoint _endPoint;
    private Encoding _encoding;
    
    public Client(EndPoint endPoint, Encoding encoding)
    {
        _socket = new Socket(GetCurrentAddressFamily(), SocketType.Stream, ProtocolType.Tcp);
        _endPoint = endPoint;
        _encoding = encoding;
    }

    public bool Connect()
    {
        try
        {
            _socket.Connect(_endPoint);
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

    public void Send(string content, int timeout = 3000)
    {
        // Check if client is connected
        if (!_socket.Connected)
            throw new Exception("NRoadClient is not connected");

        // Set send timeout
        _socket.SendTimeout = timeout; 
            
        // Encode string to byte array
        var bytes = _encoding.GetBytes(content);
        
        // Send bytes to the destination
        _socket.Send(bytes);
    }

    private static AddressFamily GetCurrentAddressFamily()
    {
        var hostInfo = Dns.GetHostEntry(Dns.GetHostName());  
        var ipAddress = hostInfo.AddressList[0];
        
        return ipAddress.AddressFamily;
    }
}