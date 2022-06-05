/*
 * ===============================================
 * N-ROAD OPEN SOURCE NETWORK LIBRARY PROJECT
 * AUTHOR: https://github.com/timfvb
 * ===============================================
 */

using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using NetRoad.Network;

namespace NetRoad;

public class NRoadClient
{
    private readonly Client _client;

    /// <summary>
    /// Initialize a new NRoad Client Object
    /// </summary>
    /// <param name="endPoint">Pair from IP and port</param>
    /// <param name="encoding">Encoding type</param>
    public NRoadClient(IPEndPoint endPoint, Encoding encoding)
    {
        _client = new Client(endPoint, encoding);
    }

    /// <summary>
    /// Initialize a new NRoad Client Object
    /// </summary>
    /// <param name="address">Address from destination</param>
    /// <param name="port">Port from destination</param>
    /// <param name="encoding"></param>
    /// <exception cref="FormatException">Invalid IPv4 Format</exception>
    /// <exception cref="ArgumentException">Invalid Port Range</exception>
    public NRoadClient(string address, int port, Encoding encoding)
    {
        // Check validation of ipv4 address format
        if (!IPAddress.TryParse(address, out var ipAddress))
            throw new FormatException("Address must be a valid IPv4 address format");
        
        // Check validation of the port format
        if (port > Math.Pow(2, 16) - 1)
            throw new ArgumentException("Port range format is wrong. Port Range: 0-65535");
        
        // Check encoding parameter 
        if (encoding == null)
            throw new ArgumentNullException(nameof(encoding));
        
        // Pair ipv4 and port to a endpoint
        var endpoint = new IPEndPoint(ipAddress, port);
        
        // Create new client object
        _client = new Client(endpoint, encoding);
    }
    
    /// <summary>
    /// Connect NRoad Client to destination
    /// </summary>
    /// <returns>If the connections was successful -> true will be returned</returns>
    public bool Connect()
    {
        // Is client successful initialized
        if (_client == null)
            throw new NullReferenceException("Client is not initialized");
        
        // Check if the client is already connected
        if (_client.clientSocket.Connected)
            throw new Exception("Client is already connected");
        
        // Start connecting NRoad client 
        return _client.Connect(); 
    }
    
    /// <summary>
    /// Send content to destination
    /// </summary>
    /// <param name="content">Content</param>
    /// <param name="timeout">Send Timeout, default: 3000</param>
    public void Send(string content, int timeout = 3000)
    {
        if (content.Length == 0 | string.IsNullOrWhiteSpace(content))
            throw new Exception("Content is empty");
        
        _client.Send(content, timeout);
    }
    
    /// <summary>
    /// Send content to destination
    /// </summary>
    /// <param name="content">Content</param>
    /// <param name="timeout">Send Timeout, default: 3000</param>
    public void Send(byte[] content, int timeout = 3000)
    {
        if (content.Length == 0)
            throw new Exception("Content is empty");
    
        _client.Send(content, timeout);
    }
}