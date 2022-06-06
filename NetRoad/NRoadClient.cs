/*
 * ===============================================
 * N-ROAD OPEN SOURCE NETWORK LIBRARY PROJECT
 * AUTHOR: https://github.com/timfvb
 * ===============================================
 */

using System.Net;
using System.Text;
using NetRoad.Network;

namespace NetRoad;

public class NRoadClient
{
    private readonly Client _client;

    public delegate void ConnectionEventHandler(object sender);
    public delegate void ConnectionEventHandler<TEventArgs>(object sender, TEventArgs e);
    
    /// <summary>
    /// Raised when a successful connection between NRoad client and server has been established
    /// </summary>
    public event ConnectionEventHandler? OnConnect;
    /// <summary>
    /// Raised when an existing connection between NRoad client and server has been disconnected
    /// </summary>
    public event ConnectionEventHandler? OnDisconnect;
    /// <summary>
    /// Triggered when a valid data packet is received from the NRoad client
    /// </summary>
    public event ConnectionEventHandler<string>? OnDataReceived;

    /// <summary>
    /// Initialize a new NRoad Client Object
    /// </summary>
    /// <param name="endPoint">Pair from IP and port</param>
    /// <param name="encoding">Encoding type</param>
    /// <param name="enableTcpListener">Use Thread TCP Listener</param>
    public NRoadClient(IPEndPoint endPoint, Encoding encoding, bool enableTcpListener)
    {
        // Create new client object
        _client = new Client(endPoint, encoding, enableTcpListener);
        
        // Set NRoad Event
        NRoadEventProperties();
    }

    /// <summary>
    /// Initialize a new NRoad Client Object
    /// </summary>
    /// <param name="address">Address from destination</param>
    /// <param name="port">Port from destination</param>
    /// <param name="encoding"></param>
    /// <exception cref="FormatException">Invalid IPv4 Format</exception>
    /// <exception cref="ArgumentException">Invalid Port Range</exception>
    /// <param name="enableTcpListener">Use Thread TCP Listener</param>
    public NRoadClient(string address, int port, Encoding encoding, bool enableTcpListener = true)
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
        _client = new Client(endpoint, encoding, enableTcpListener);
        
        // Set NRoad Event
        NRoadEventProperties();
    }

    private void NRoadEventProperties()
    {
        // Connection Events
        _client.OnConnect += ClientOnConnect;
        _client.OnDisconnect += ClientOnDisconnect;
    }
    
    private void ClientOnConnect(object sender) => OnConnect?.Invoke(sender);

    private void ClientOnDisconnect(object sender) => OnDisconnect?.Invoke(sender);


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
        if (_client.NetClient.Connected)
            throw new Exception("Client is already connected");
        
        // Start connecting NRoad client 
        return _client.Connect(); 
    }

    /// <summary>
    /// Disconnect the NRoad Client from destination 
    /// </summary>
    /// <exception cref="NullReferenceException"></exception>
    public void Disconnect()
    {
        // Is client successful initialized
        if (_client == null)
            throw new NullReferenceException("Client is not initialized");
        
        _client.Disconnect();
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