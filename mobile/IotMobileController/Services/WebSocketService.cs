using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text.Json;
using System.Diagnostics;
using System.Threading;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;

namespace IotMobileController.Services
{
 
    public class WebSocketService
    {
        private readonly ClientWebSocket _webSocketClient;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly CancellationTokenSource cancellationTokenSource;

        public WebSocketService()
        {
            cancellationTokenSource = new();
            _webSocketClient = new ClientWebSocket();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<dynamic> SocketConnect()
        {
            try
            {
                var url = "ws://100.88.95.100:5000";
                await _webSocketClient.ConnectAsync(new Uri(url), CancellationToken.None);
                while (_webSocketClient.State == WebSocketState.Open)
                {
                    var buffer = new ArraySegment<byte>(new byte[1024]);
                    var result = await _webSocketClient.ReceiveAsync(buffer, cancellationTokenSource.Token);
                    var message = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                    dynamic dynJson = JsonConvert.DeserializeObject<dynamic>(message);
                    return dynJson;
                }
            } catch (Exception ex)
            {
                Debug.WriteLine("Error:"+ex);
            }
            return null;
        }

        /*public async Task<dynamic> SocketConnect()
        {
            try
            {
                var url = "ws://100.88.95.100:5000";
                await _webSocketClient.ConnectAsync(new Uri(url), CancellationToken.None);
                await Task.Factory.StartNew(async () =>
                {
                    while (true)
                    {
                        await SocketReadMessage();
                    }
                }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error:" + ex);
            }
            return null;
        }
*/
        public async Task SocketSendMessage(string message)
        {
            try
                {
                if (_webSocketClient.State == WebSocketState.Open)
                    {
                    var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
                    await _webSocketClient.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            catch (Exception ex)
                {
                Debug.WriteLine("Error:"+ex);
            }
        }


        public async Task<JObject> SocketReadMessage()
        {
            WebSocketReceiveResult result;
            var message = new ArraySegment<byte>(new byte[4096]);
            do
            {
                result = await _webSocketClient.ReceiveAsync(message, CancellationToken.None);
                if (result.MessageType != WebSocketMessageType.Text)
                    break;
                var messageBytes = message.Skip(message.Offset).Take(result.Count).ToArray();
                string receivedMessage = Encoding.UTF8.GetString(messageBytes);
                JObject jsonObject = JObject.Parse(receivedMessage);
                return jsonObject;
                
            }
            while (!result.EndOfMessage);
            return null;
        }

        public async Task SocketDisconnect()
        {
            if (_webSocketClient != null && _webSocketClient.State == WebSocketState.Open)
            {
                try
                {
                    await _webSocketClient.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing connection", CancellationToken.None);
                } catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }
    }
}
