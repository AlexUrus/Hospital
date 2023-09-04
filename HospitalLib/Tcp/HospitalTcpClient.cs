using HospitalLib.Event;
using HospitalLib.Record;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HospitalLib.Tcp
{
    public class HospitalTcpClient : TcpClient
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        MemoryStream _buffer;
        private NetworkStream _stream;
        private TcpClient _tcpClient;
        private string _ipAddress;
        private int _port;
        private int _cancellationTime;
        private CancellationTokenSource cst;
        private Type _classRecordType;
        public string IpAddress => _ipAddress;
        public string ClientId { get; set; }
        public EventHandler<ConnectionChangedEventArgs> ConnectionChanged;
        public EventHandler<ReceivedMessageEventArgs> ReceivedMessage;

        public HospitalTcpClient(TcpClient tcpClient, int cancellationTime)
        {
            _cancellationTime = cancellationTime;
            _tcpClient = tcpClient;
            var point = (IPEndPoint)_tcpClient.Client.RemoteEndPoint;
            _port = point.Port;
            _ipAddress = point.Address.ToString();
            _buffer = new MemoryStream();
            cst = new CancellationTokenSource();
            Random random = new Random();
            ClientId = _ipAddress + random.Next();
        }

        public async Task StartAsync()
        {
            if (_tcpClient.Connected)
            {
                StartReadThread();
                _logger.Warn($"Client connected, {_tcpClient?.Client.Connected}");
                ConnectionChanged?.Invoke(this, new ConnectionChangedEventArgs(IpAddress, true));
            }
        }

        void StartReadThread()
        {
            try
            {
                if (_tcpClient != null)
                {
                    _ = Process();
                }
            }
            catch (Exception e)
            {
                _logger.Warn(e);
            }
        }

        public async Task Process()
        {
            try
            {
                _stream = _tcpClient.GetStream();
                int i;
                string data;
                byte[] bytes = new byte[65000];

                while ((i = await _stream.ReadAsync(bytes, 0, bytes.Length, CreateToken.Token)) != 0)
                {
                    data = Encoding.UTF8.GetString(bytes, 0, i);
                    await OnDataReceived(Encoding.UTF8.GetBytes(data));
                }
            }
            catch (Exception ex)
            {
                _logger.Info($"TCP client disconnected");
            }
            finally
            {
                CloseStream();
                ConnectionChanged?.Invoke(this, new ConnectionChangedEventArgs(IpAddress, false));
                cst.Dispose();
            }
        }

        private void CloseStream()
        {
            _stream.Close();
            _tcpClient.Close();
        }

        private CancellationTokenSource CreateToken
        {
            get
            {
                cst.Dispose();
                cst = new CancellationTokenSource(_cancellationTime);
                cst.Token.Register(() =>
                {
                    _logger.Warn($"Disconnect client IP:{IpAddress} because of timeout. Delta: {TimeSpan.FromMilliseconds(_cancellationTime).TotalSeconds} c.");
                    CloseStream();
                });
                return cst;
            }
        }

        private async Task OnDataReceived(byte[] bytes)
        {
            try
            {
                _buffer.Write(bytes, 0, bytes.Length);
                byte[] buffer = _buffer.GetBuffer();
                string bufferString = Encoding.UTF8.GetString(buffer);
                Array.Clear(buffer, 0, buffer.Length);

                _logger.Info($"Server received message from MPOS Device: {bufferString.Replace("\0", string.Empty)}");
                await OnMessageReceived(bufferString);
            }
            catch (Exception ex)
            {
                _logger.Debug($"TCP: {ex.Message}");
                _logger.Error(ex.StackTrace);
            }
            finally
            {
                _buffer.SetLength(0);
            }
        }

        private async Task OnMessageReceived(string jsonMessage)
        {
            try
            {
                JObject jObject = JObject.Parse(jsonMessage);
                if (jObject == null) return;

                if (jObject.ContainsKey("Type"))
                {
                    int typeId = (int)jObject.GetValue("Type");
                    if (Enum.IsDefined(typeof(TypeRecord), typeId))
                    {
                        TypeRecord type = (TypeRecord)typeId;
                        SetRecordType(type);
                        object record = new object();
                        if (_classRecordType != null) record = JsonConvert.DeserializeObject(jsonMessage, _classRecordType);
                        await Task.Run(() => ReceivedMessage?.Invoke(this, new ReceivedMessageEventArgs(record, type, ClientId)));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.StackTrace);
            }
        }
        public async Task SendAsync(IRecord record)
        {
            try
            {
                var stream = _tcpClient.GetStream();

                var jsonRecord = JsonConvert.SerializeObject(record);

                var bytes = Encoding.UTF8.GetBytes(jsonRecord);

                string bytesString = BitConverter.ToString(bytes).Replace("-", "");

                _logger.Info($"Server send message on MPOS Device: Type - {record.Type}, JSON: {jsonRecord}");

                await stream.WriteAsync(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.StackTrace);
            }
        }


        private void SetRecordType(TypeRecord typeRecord)
        {
            switch (typeRecord)
            {
                case TypeRecord.ClientLogin:
                    {
                        _classRecordType = typeof(PatientRecord);
                        break;
                    }
            }
        }
    }
}
