using System.Net;
using System.Text;
using NetRoad.Protocol.Client;

namespace NetRoad;

public class NRoadUdpClient
{
    private readonly NetUdpClient _client;
    
    public NRoadUdpClient(IPEndPoint endPoint, Encoding encoding, bool enableUdpListener = true)
    {
        // Create new client object
        _client = new NetUdpClient(endPoint, encoding, enableUdpListener);
    }

    /// <summary>
    /// Initialize a new NRoad Client Object
    /// </summary>
    /// <param name="address">Address from destination</param>
    /// <param name="port">Port from destination</param>
    /// <param name="encoding">Encoding Type</param>
    /// <exception cref="FormatException">Invalid IPv4 Format</exception>
    /// <exception cref="ArgumentException">Invalid Port Range</exception>
    /// <param name="enableUdpListener">Use Thread UDP Listener</param>
    public NRoadUdpClient(string address, int port, Encoding encoding, bool enableUdpListener = true)
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
        _client = new NetUdpClient(endpoint, encoding, enableUdpListener);
    }
}