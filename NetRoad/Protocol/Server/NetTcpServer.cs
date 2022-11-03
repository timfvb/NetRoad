/*
 * ===============================================
 * N-ROAD OPEN SOURCE NETWORK LIBRARY PROJECT
 * AUTHOR: https://github.com/timfvb
 * ===============================================
 */

using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetRoad.Protocol.Server;

public class Server
{
    private readonly TcpListener _netListener;
    private readonly Encoding _encoding;
    
    private bool _isRunning;

    public readonly List<TcpClient> ConnectedClients = new ();
    
    public delegate void ConnectionEventHandler<in TEventArgs>(object sender, TEventArgs e);
    
    public event ConnectionEventHandler<EndPoint>? Started;
    public event ConnectionEventHandler<IPAddress>? Connected;
    public event ConnectionEventHandler<IPAddress>? Disconnected;
    public event ConnectionEventHandler<string>? DataReceived;
    
    public Server(IPAddress address, int port, Encoding encoding)
    {
        _netListener = new TcpListener(address, port);
        _encoding = encoding;
    }

    public void Start(int backlog)
    {
        // Check if server already running
        if (_isRunning)
            throw new Exception("NRoadServer already started");
        
        // Start new listener
        _netListener.Start(backlog);
        
        // Start forward thread
        var forwardThread = new Thread(Forward);
        forwardThread.Start();
        
        // Set the server running status true
        _isRunning = true;
        
        // Invoke with endpoint parameter
        Started?.Invoke(this, _netListener.LocalEndpoint);
    }
    
    public void SendToClient(TcpClient client, string content)
    {
        // Create stream writer
        var networkWriter = new StreamWriter(client.GetStream(), _encoding);
        
        // Write on stream
        networkWriter.WriteLine(content);
        
        // Flush
        networkWriter.Flush();
    }
    
    public void SendToClients(IEnumerable<TcpClient> clients, string content)
    {
        foreach (var client in clients)
        {
            // Create stream writer
            var networkWriter = new StreamWriter(client.GetStream(), _encoding);
        
            // Write on stream
            networkWriter.WriteLine(content);
        
            // Flush
            networkWriter.Flush();
        }
    }
    
    public void SendToConnectedClients(string content)
    {
        foreach (var networkWriter in ConnectedClients.Select(client 
                     => new StreamWriter(client.GetStream(), _encoding)))
        {
            // Write on stream
            networkWriter.WriteLine(content);
        
            // Flush
            networkWriter.Flush();
        }
    }
    
    private void Forward()
    {
        while (_netListener.Server.IsBound)
        {
            // Accept incoming client
            var client = _netListener.AcceptTcpClient();
            
            // Get ipv4 address
            var clientEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
            
            // Add client to connected clients
            ConnectedClients.Add(client);
            
            // Create new thread
            var receiverThread = new Thread(Receiver);
            
            // Start receiver thread with client parameter
            receiverThread.Start(client);

            // Invoke with address parameter
            Connected?.Invoke(this, clientEndPoint!.Address);
        }
    }
    
    private void Receiver(object? clientObj)
    {
        // Check if client object is invalid (null)
        if (clientObj == null)
            throw new ArgumentException("Client is null", nameof(clientObj));
        
        var client = (TcpClient) clientObj;
        var clientEndPoint = (client.Client.RemoteEndPoint as IPEndPoint)!;

        // Create Network Stream Reader
        var reader = new StreamReader(client.GetStream(), _encoding);

        try
        {
            while (client.Connected)
            {
                // Waiting for available data
                while (client.Connected && !reader.EndOfStream)
                {
                    // Read available data
                    var data = reader.ReadLine();

                    // Invoke if available data not null or empty
                    if (!string.IsNullOrEmpty(data))
                        DataReceived?.Invoke(this, data);
                }

                // CPU safety
                Thread.Sleep(1);
            }
        }
        catch (InvalidOperationException)
        {
            // ignore
        }
        catch (Exception e)
        {
            // Throw with exception details
            throw new Exception(e.Message);
        }
        finally
        {
            // Close the current network reader
            reader.Close();
            
            // Remove client from the connected client list
            ConnectedClients.Remove(client);

            // Invoke disconnect with address parameter
            Disconnected?.Invoke(this, clientEndPoint.Address);
        }
    }
}