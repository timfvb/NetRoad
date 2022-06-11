/*
 * ===============================================
 * N-ROAD OPEN SOURCE NETWORK LIBRARY PROJECT
 * AUTHOR: https://github.com/timfvb
 * ===============================================
 */

using System.Net;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using NetRoad.Network;

namespace NetRoad;

public class NRoadServer
{
    private readonly Server _server;
    
    public delegate void ConnectionEventHandler<in TEventArgs>(object sender, TEventArgs e);
    
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
    public NRoadServer(int port, Encoding encoding)
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
    public NRoadServer(IPAddress address, int port, Encoding encoding)
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
    public NRoadServer(string address, int port, Encoding encoding)
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
        _server.Connected += ServerOnConnected;
        _server.Disconnected += ServerOnDisconnected;
        _server.DataReceived += ServerOnDataReceived;
    }
    
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
}