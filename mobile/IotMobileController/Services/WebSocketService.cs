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
        private readonly ClientWebSocket _WebSocketclient;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly CancellationTokenSource cancellationTokenSource;

        public WebSocketService()
        {
            cancellationTokenSource = new();
            _WebSocketclient = new ClientWebSocket();
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
                await _WebSocketclient.ConnectAsync(new Uri(url), CancellationToken.None);
                await Task.Factory.StartNew(async () =>
                {
                    while (true)
                    {
                        await SocketReadMessage();
                    }
                }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            } catch (Exception ex)
            {
                Debug.WriteLine("Error:"+ex);
            }
            return null;
        }

        public async Task SocketSendMessage(string message)
        {
            try
                {
                if (_WebSocketclient.State == WebSocketState.Open)
                    {
                    var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
                    await _WebSocketclient.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
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
                result = await _WebSocketclient.ReceiveAsync(message, CancellationToken.None);
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
            if (_WebSocketclient != null && _WebSocketclient.State == WebSocketState.Open)
            {
                try
                {
                    await _WebSocketclient.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing connection", CancellationToken.None);
                } catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }
    }
}
