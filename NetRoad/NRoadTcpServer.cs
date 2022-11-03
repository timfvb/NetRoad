/*
 * ===============================================
 * N-ROAD OPEN SOURCE NETWORK LIBRARY PROJECT
 * AUTHOR: https://github.com/timfvb
 * ===============================================
 */

using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using NetRoad.Protocol.Server;

namespace NetRoad;

public class NRoadTcpServer
{
    private readonly Server _server;
    
    public delegate void ConnectionEventHandler<in TEventArgs>(object sender, TEventArgs e);
 
    /// <summary>
    /// Raised when the tcp-listener successful started
    /// </summary>
    public event ConnectionEventHandler<EndPoint>? Started;
    /// <summary>
    /// Raised when a new successful connection between NRoad client and server has been established
    /// </summary>
    public event ConnectionEventHandler<IPAddress>? Connected;
    /// <summary>
    /// Raised when an existing connection between NRoad client and server has been disconnected
    /// </summary>
    public event ConnectionEventHandler<IPAddress>? Disconnected;
    /// <summary>
    /// Triggered when a valid data packet is received from the NRoad server
    /// </summary>
    public event ConnectionEventHandler<string>? DataReceived;
    
    /// <summary>
    /// Initialize new NRoadServer Object
    /// </summary>
    /// <param name="port">Port format (0-65535)</param>
    /// <param name="encoding"></param>
    public NRoadTcpServer(int port, Encoding encoding)
    {
        // Check validation of the port format
        if (port > Math.Pow(2, 16) - 1)
            throw new ArgumentException("Port range format is wrong. Port Range: 0-65535");
        
        // Check encoding parameter 
        if (encoding == null)
            throw new ArgumentNullException(nameof(encoding));
        
        _server = new Server(IPAddress.Any, port, encoding);
        
        // Set NRoad Events
        NRoadEventProperties();
    }

    /// <summary>
    /// Initialize new NRoadServer Object
    /// </summary>
    /// <param name="address">Enter a valid IPv4 address</param>
    /// <param name="port">Port format (0-65535)</param>
    /// <param name="encoding"></param>
    public NRoadTcpServer(IPAddress address, int port, Encoding encoding)
    {
        // Check validation of the port format
        if (port > Math.Pow(2, 16) - 1)
            throw new ArgumentException("Port range format is wrong. Port Range: 0-65535");

        _server = new Server(address, port, encoding);
        
        // Set NRoad Events
        NRoadEventProperties();
    }
    
    /// <summary>
    /// Initialize new NRoadServer Object
    /// </summary>
    /// <param name="address">Enter a valid IPv4 address</param>
    /// <param name="port">Port format (0-65535)</param>
    /// <param name="encoding"></param>
    public NRoadTcpServer(string address, int port, Encoding encoding)
    {
        // Check validation of the port format
        if (port > Math.Pow(2, 16) - 1)
            throw new ArgumentException("Port range format is wrong. Port Range: 0-65535");

        _server = new Server(IPAddress.Parse(address), port, encoding);
        
        // Set NRoad Events
        NRoadEventProperties();
    }

    private void NRoadEventProperties()
    {
        _server.Started += ServerOnStarted;
        _server.Connected += ServerOnConnected;
        _server.Disconnected += ServerOnDisconnected;
        _server.DataReceived += ServerOnDataReceived;
    }
    
    private void ServerOnStarted(object sender, EndPoint e) => Started?.Invoke(sender, e);
    
    private void ServerOnConnected(object sender, IPAddress e) => Connected?.Invoke(sender, e);
    
    private void ServerOnDisconnected(object sender, IPAddress e) => Disconnected?.Invoke(sender, e);
    
    private void ServerOnDataReceived(object sender, string data) => DataReceived?.Invoke(sender, data);

    /// <summary>
    /// Start current server instance
    /// </summary>
    /// <param name="backlog">Backlog (Default: 2147483647)</param>
    public void Start(int backlog = int.MaxValue)
    {
        // Start server instance
        _server.Start(backlog);
    }

    /// <summary>
    /// Get all connected clients
    /// </summary>
    /// <returns>Client Arraylist</returns>
    public TcpClient[] GetConnectedClients()
    {
        return _server.ConnectedClients.ToArray();
    }
    
    /// <summary>
    /// Send data to client
    /// </summary>
    /// <param name="client">Destination client</param>
    /// <param name="content">Content to be transferred</param>
    /// <exception cref="Exception">Content is null or empty</exception>
    public void SendToClient(TcpClient client, string content)
    {
        if (content.Length == 0 | string.IsNullOrWhiteSpace(content))
            throw new Exception("Content is empty");
        
        _server.SendToClient(client, content);
    }

    /// <summary>
    /// Send data to multiple clients
    /// </summary>
    /// <param name="clients">Destination clients</param>
    /// <param name="content">Content to be transferred</param>
    /// <exception cref="Exception">Content is null or empty</exception>
    public void SendToClients(IEnumerable<TcpClient> clients, string content)
    {
        if (content.Length == 0 | string.IsNullOrWhiteSpace(content))
            throw new Exception("Content is empty");
        
        _server.SendToClients(clients, content);
    }
    
    /// <summary>
    /// Send data to all connected clients
    /// </summary>
    /// <param name="content">Content to be transferred</param>
    /// <exception cref="Exception">Content is null or empty</exception>
    public void SendToConnectedClients(string content)
    {
        if (content.Length == 0 | string.IsNullOrWhiteSpace(content))
            throw new Exception("Content is empty");
        
        _server.SendToConnectedClients(content);
    }
}