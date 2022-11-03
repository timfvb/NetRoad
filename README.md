<p align="center"><img width="256" src="nroad.png"/></p>
<h1 align="center">NetRoad</h1>
<p align="center">A lightweight network library written in C#</p>
<br>

## About
NetRoad helps developers to easily use sockets in their project.<br />
With minimal code you can connect to a TCP or UDP server or host one yourself.<br />
The library is lightweight (16.3kb), easy to use and very developer friendly.

## Features
 - TCP Support
   - Client
   - Server
 - UDP Support
   - Client
   - Server (Soon accessible)
 - Data
   - Integrated listener in client (NRoadTcpClient)
   - Automatic encode data
   - Send Object as JSON
 - Events
   - Connected (TCP)
   - Disconnected (TCP)
   - DataReceived (TCP/UDP)

## TCP Client Example
```csharp
// Connection credentials
string address = "127.0.0.1";
int port = 2299;
Encoding encoding = Encoding.UTF8;

// Create NRoad Client
var client = new NRoadTcpClient(address, port, encoding);

// Connect to server
client.Connect();

// Send data to server
int dataTimeout = 500;
client.Send("This is NetRoad!", 500);

// Disconnect client
client.Disconnect();
```

## TCP Server Example
```csharp
// Server credentials
string address = "127.0.0.1";
int port = 2299;
Encoding encoding = Encoding.UTF8;

// Create NRoad Server
var server = new NRoadTcpServer(address, port, encoding);

// Register Data Receive Event
server.DataReceived += (sender, s) 
    => Console.WriteLine("Data:\t" + s);
        
// Start listening
server.Start();
```

## Installation

.NET CLI
```
dotnet add package NetRoad
```

<br />
NuGet Gallery: https://www.nuget.org/packages/NetRoad

## Dependencies
| **Dependency** | **Description** | **Version** | **License**
| -------------- | --------------- | ----------- | -----------
 | [Newtonsoft.Json@^13.0.1](https://www.nuget.org/packages/Newtonsoft.Json) | Json.NET is a popular high-performance JSON framework for .NET| 13.0.1 | MIT

## License
MIT License: https://choosealicense.com/licenses/mit

## Contact
Discord: TimFvb#9999
