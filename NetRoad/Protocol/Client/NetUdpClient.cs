using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetRoad.Protocol.Client;

public class NetUdpClient
{
    public readonly UdpClient NetClient;
    private IPEndPoint _endPoint;
    private readonly Encoding _encoding;
    
    private readonly bool _enableUdpListener;
    
    public delegate void ConnectionEventHandler<in TEventArgs>(object sender, TEventArgs e);
    
    public event ConnectionEventHandler<string>? DataReceived;
    
    public NetUdpClient(IPEndPoint endPoint, Encoding encoding, bool enableUdpListener)
    {
        NetClient = new UdpClient();
        _endPoint = endPoint;
        _encoding = encoding;
        _enableUdpListener = enableUdpListener;
    }
    
    public void Connect()
    {
        // Connect to destination
        NetClient.Connect(_endPoint);
        
        // Start a new receiver
        if (_enableUdpListener)
            StartReceiver();
    }
    
    public void Send(string content)
    {
        // Encode string
        var encoded = _encoding.GetBytes(content);

        // Send bytes to destination
        NetClient.Send(encoded, encoded.Length);
    }
    
    private void StartReceiver()
    {
        // Create new receiver thread
        var receiverThread = new Thread(Receiver);
        
        // Start receiver thread
        receiverThread.Start();
    }
    
    private void Receiver()
    {
        while (_enableUdpListener)
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