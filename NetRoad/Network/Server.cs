/*
 * ===============================================
 * N-ROAD OPEN SOURCE NETWORK LIBRARY PROJECT
 * AUTHOR: https://github.com/timfvb
 * ===============================================
 */

using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetRoad.Network;

public class Server
{
    private readonly TcpListener _netListener;
    private readonly Encoding _encoding;
    
    private bool _isRunning;
    
    public delegate void DataEventHandler(object sender, string data);
    
    public event DataEventHandler? DataReceived;
    
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
        
        // Start forward
        Forward();
        
        _isRunning = true;
        
        Thread.Sleep(-1);
    }

    private void Forward()
    {
        while (true)
        {
            // Accept incoming client
            var client = _netListener.AcceptTcpClient();
            
            // Create new thread
            var receiverThread = new Thread(Receiver);
            
            // Start receiver thread with client parameter
            receiverThread.Start(client);
        }
    }
    
    private void Receiver(object? clientObj)
    {
        if (clientObj == null)
            throw new ArgumentException("Client is null", nameof(clientObj));
        
        var client = (TcpClient) clientObj;
        
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
        catch (Exception e)
        {
            // Throw with exception details
            throw new Exception(e.Message);
        }
        finally
        {
            // Close the currently network reader
            reader.Close();
        }
    }
}