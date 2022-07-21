/*
 * ===============================================
 * N-ROAD OPEN SOURCE NETWORK LIBRARY PROJECT
 * AUTHOR: https://github.com/timfvb
 * ===============================================
 */

using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace NetRoad.Protocol.Client;

public class NetTcpClient
{
    public readonly TcpClient NetClient;
    private readonly IPEndPoint _endPoint;
    private readonly Encoding _encoding;
    
    private readonly bool _enableTcpListener;
    
    public delegate void ConnectionEventHandler(object sender);
    public delegate void ConnectionEventHandler<in TEventArgs>(object sender, TEventArgs e);
    
    public event ConnectionEventHandler? Connected;
    public event ConnectionEventHandler? Disconnected;
    public event ConnectionEventHandler<string>? DataReceived;

    public NetTcpClient(IPEndPoint endPoint, Encoding encoding, bool enableTcpListener)
    {
        NetClient = new TcpClient();
        _endPoint = endPoint;
        _encoding = encoding;
        _enableTcpListener = enableTcpListener;
    }

    public bool Connect()
    {
        try
        {
            // Try to connect to destination
            NetClient.Connect(_endPoint);
            
            // Start a new receiver
            if (_enableTcpListener)
                StartReceiver();
            
            // Invoke Connection Success Event
            Connected?.Invoke(this);
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
    
    public void Disconnect()
    {
        // Check if client still connected
        if (!NetClient.Connected)
            throw new Exception("Client is not connected");
        
        // Close current network connection
        NetClient.Close();

        // Invoke Disconnection Event
        Disconnected?.Invoke(this);
    }

    public bool Send(string content, int timeout)
    {
        // Check if client is connected
        if (!NetClient.Connected)
            Disconnected?.Invoke(this);

        // Set send timeout
        NetClient.SendTimeout = timeout; 
        
        try
        {
            // Create a network writer
            var writer = new StreamWriter(NetClient.GetStream(), _encoding);

            // Send bytes to the destination
            writer.WriteLine(content);
            writer.Flush();

            // Return current connection status
            return true;
        }
        catch
        {
            // Invoke Disconnection Event
            Disconnected?.Invoke(this);
            
            // Return current connection status
            return false;
        }
    }
    
    public bool Send(byte[] content, int timeout)
    {
        // Check if client is connected
        if (!NetClient.Connected)
            Disconnected?.Invoke(this);

        // Set send timeout
        NetClient.SendTimeout = timeout;
        
        // Decode byte array to string
        var decoded = _encoding.GetString(content);

        try
        {
            // Create a network writer
            var writer = new StreamWriter(NetClient.GetStream(), _encoding);

            // Send bytes to the destination
            writer.WriteLine(decoded);
            writer.Flush();

            // Return current connection status
            return true;
        }
        catch
        {
            // Invoke Disconnection Event
            Disconnected?.Invoke(this);
            
            // Return current connection status
            return false;
        }
    }
    
    public bool SendObjectAsJson<T>(T obj, int timeout)
    {
        // Check if client is connected
        if (!NetClient.Connected)
            Disconnected?.Invoke(this);

        // Set timeout
        NetClient.SendTimeout = timeout;
        
        // Serialize object
        var json = JsonConvert.SerializeObject(obj);

        try
        {
            // Create a network writer
            var writer = new StreamWriter(NetClient.GetStream(), _encoding);

            // Send bytes to the destination
            writer.WriteLine(json);
            writer.Flush();
            
            // Return current connection status
            return true;
        }
        catch
        {
            // Invoke Disconnection Event
            Disconnected?.Invoke(this);

            // Return current connection status
            return false;
        }
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
        // Create Network Stream Reader
        var reader = new StreamReader(NetClient.GetStream(), _encoding);

        try
        {
            while (NetClient.Connected)
            {
                // Waiting for available data
                while (NetClient.Connected && !reader.EndOfStream)
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
        catch
        {
            // ignored
        }
        finally
        {
            // Close the currently network reader
            reader.Close();
        }
    }
}