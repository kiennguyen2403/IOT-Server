
using IotMobileController.Models;
using IotMobileController.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IotMobileController.ViewModels
{
    public class MainPageViewModel: BaseViewModel
    {
        readonly HttpService _service;
        public ObservableCollection<PieData> PieChart { get; } = new();
        public ObservableCollection<float> LineChart { get; } = new();
        public ObservableCollection<float> BarChart { get; } = new();

        public Command GetDataCommand { get; }


        public MainPageViewModel(HttpService service)
        {
            _service = service;
            GetDataCommand = new Command(async () => await GetDataAsync());
            GetDataCommand.CanExecute(true);
            GetDataCommand.Execute(this);
            GetDataCommand.CanExecute(false);
        }

        private async Task GetDataAsync()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                PieChart?.Clear();
                LineChart?.Clear();
                BarChart?.Clear();
                
                var data = await _service.GetDataAsync();
                List<CommandRecord> commands = new();
                foreach (var item in data) {
                    commands.Add(new() { 
                        Id = item[0], 
                        Time = item[1], 
                        CommandName = item[2], 
                        DeviceId = item[3] });
                }

                var result = DataProcesssing.PieDataProcessing(commands);
                PieChart.Add(new PieData() { 
                    Name = "Bedroom", 
                    Value = result[0] 
                });

                PieChart.Add(new PieData()
                {
                    Name = "Living Room",
                    Value = result[1]
                });

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error:" + ex);
                await Shell.Current.DisplayAlert("Error", "Unables to get data.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
