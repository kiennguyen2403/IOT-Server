using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SocketIOClient;
namespace IotMobileController.Services
{
    
    public class SocketIOService
    {
        private readonly SocketIO _socketIOClient;
        private JsonSerializerOptions _serializerOptions;
        
        public SocketIOService()
        {
            _socketIOClient = new SocketIO("http://100.88.95.100:5000/");
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }


        public JsonElement Commands { 
            get; 
            private set; 
        }

        public async Task SocketConnect()
        {
    
            _socketIOClient.On("connect", async response => {
                Commands = response.GetValue();
                await response.CallbackAsync();
            });

            _socketIOClient.On("message", async response =>
            {
                Commands = response.GetValue();
                await response.CallbackAsync();
            });

            _socketIOClient.On("disconnect", async response =>
            {
                Debug.WriteLine("Response:" + response);
                await response?.CallbackAsync();
            });
            await _socketIOClient.ConnectAsync();
        }


        public async Task SocketSendMessage(int value, string message, bool timer = false)
        {
            try
            {
                if (!timer)
                {
                    Dictionary<string, dynamic> sendingMessage = new()
                {
                    { "data", message },
                    { "led", value }
                };
                    await _socketIOClient.EmitAsync("message",
                        response =>
                        {
                            Debug.WriteLine(message);
                            var result = response.GetValue();
                        }, sendingMessage);
                } else
                {
                    Dictionary<string, dynamic> sendingMessage = new()
                {
                    { "data", message },
                    { "time", value }
                };
                    await _socketIOClient.EmitAsync("message",
                        response =>
                        {
                            Debug.WriteLine(message);
                            var result = response.GetValue();
                        }, sendingMessage);
                }
            } catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }
    }
}
