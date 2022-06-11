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
    
    public delegate void ConnectionEventHandler<TEventArgs>(object sender, TEventArgs e);
    
    /// <summary>
    /// Triggered when a valid data packet is received from the NRoad client
    /// </summary>
    public event ConnectionEventHandler<string>? DataReceived;
    
    public NRoadServer(int port, Encoding encoding)
    {
        _server = new Server(IPAddress.Any, port, encoding);
        _server.DataReceived += ServerOnDataReceived;
    }

    public NRoadServer(IPAddress address, int port, Encoding encoding)
    {
        _server = new Server(address, port, encoding);
        _server.DataReceived += ServerOnDataReceived;
    }
    
    public NRoadServer(string address, int port, Encoding encoding)
    {
        _server = new Server(IPAddress.Parse(address), port, encoding);
        _server.DataReceived += ServerOnDataReceived;
    }

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