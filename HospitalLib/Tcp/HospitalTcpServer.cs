using HospitalLib.Event;
using HospitalLib.Record;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLib.Tcp
{
    public class HospitalTcpServer
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private TcpListener _tcpListener;
        private int _port;
        public EventHandler<ReceivedMessageEventArgs> MessageReceived;
        public EventHandler<ConnectionChangedEventArgs> ClientConnectionChanged;

        private List<HospitalTcpClient> _clients = new List<HospitalTcpClient>();
        private readonly object _clientLock = new object();
        private int _clientDisconnectTime = 60000;

        static readonly HospitalTcpServer instance = new HospitalTcpServer();
        public static HospitalTcpServer Instance => instance;

        public async Task RunAsync(int port, int clientDisconnectTime)
        {
            _port = port;
            _tcpListener = new TcpListener(IPAddress.Any, _port);
            _clientDisconnectTime = clientDisconnectTime;
            _tcpListener.Start();
            _logger.Info($"TCP: Started TcpListener on port: {_port}");

            await AcceptTcpClient();
        }

        private async Task AcceptTcpClient()
        {
            while (true)
            {
                HospitalTcpClient client = new HospitalTcpClient(await _tcpListener.AcceptTcpClientAsync(), _clientDisconnectTime);
                Add(client);
            }
        }

        private void Add(HospitalTcpClient client)
        {
            lock (_clientLock)
            {
                _clients.Add(client);
                _logger.Info($"TCP: tcpClient accepted {client.IpAddress}");
            }

            try
            {
                client.ConnectionChanged += OnClientConnectionChanged;
                client.ReceivedMessage += OnMessageReceived;
                client.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TCP: {ex.Message}");
            }
        }

        private void Remove(HospitalTcpClient client)
        {
            try
            {
                lock (_clientLock)
                {
                    if (!client.Connected && _clients.Contains(client))
                    {
                        _clients.Remove(client);
                        client.Dispose();
                    }

                    _logger.Info($"TCP Connection Closed and Removed from List IP: {client.IpAddress}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TCP: {ex.Message}");
            }
        }

        private void OnMessageReceived(object sender, ReceivedMessageEventArgs e)
        {
            MessageReceived.Invoke(sender, e);
        }

        private void OnClientConnectionChanged(object sender, ConnectionChangedEventArgs e)
        {
            if (!e.IsConnected)
            {
                Remove((HospitalTcpClient)sender);
            }
        }
    }
}
