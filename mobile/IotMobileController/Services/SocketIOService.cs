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

        public void SocketConnect()
        {
            _socketIOClient.On("connect", async response => {
                await response.CallbackAsync();
            });

            _socketIOClient.On("message", async response =>
            {
                await response.CallbackAsync();
            });

            _socketIOClient.On("disconnect", async response =>
            {
                await response?.CallbackAsync();
            });

        }


        public async Task SocketSendMessage(string message)
        {
            try
            {
                await _socketIOClient.EmitAsync("message", response => { },message);
            } catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }
    }
}
