using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetRoad.Protocol.Server;

public class NetUdpServer
{
    private readonly UdpClient NetClient;
    private IPEndPoint _endPoint;
    private readonly Encoding _encoding;

    public delegate void ConnectionEventHandler<in TEventArgs>(object sender, TEventArgs e);
    public event ConnectionEventHandler<string>? DataReceived;
    
    public NetUdpServer(IPAddress address, int port, Encoding encoding)
    {
        NetClient = new UdpClient();
        _endPoint = new IPEndPoint(address, port);
        _encoding = encoding;
    }

    public void Start()
    {
        // Create new receiver thread
        var receiverThread = new Thread(Receiver);
        
        // Start receiver thread
        receiverThread.Start();
    }
    
    private void Receiver()
    {
        while (true)
        {
            // Receive data
            var output = NetClient.Receive(ref _endPoint); 
            // Decode data
            var data = _encoding.GetString(output);
            
            // Invoke with data parameter
            if (!string.IsNullOrEmpty(data))
                DataReceived?.Invoke(this, data);
        }
    }
}