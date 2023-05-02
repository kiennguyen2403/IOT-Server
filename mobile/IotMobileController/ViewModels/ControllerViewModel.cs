using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using IotMobileController.Services;


namespace IotMobileController.ViewModels
{
    public class ControllerViewModel: BaseViewModel
    {
        private readonly SocketIOService _service;



        bool isTimerSet;
        bool isLivingRoomOn;
        bool isBedRoomOn;


        public bool IsTimerSet 
        {
            get
            {
                return isTimerSet;
            } 
            set
            {
                if (isTimerSet != value)
                {
                    isTimerSet = value;
                    OnPropertyChanged(nameof(IsTimerSet));
                }
            }
        }
        public bool IsLivingRoomOn 
        {
            get
            {
                return isLivingRoomOn;
            }
            set
            {
                if (isLivingRoomOn != value)
                {
                    isLivingRoomOn = value;
                    OnPropertyChanged(nameof(IsLivingRoomOn));
                }
            }
        
        }

        public bool IsBedRoomOn 
        {
            get
            {
                return isBedRoomOn;
            }
            set
            {
                if (isBedRoomOn != value)
                {
                    isBedRoomOn = value;
                    OnPropertyChanged(nameof(IsBedRoomOn));
                }
            } 
        }

        public Command GetDataCommand { get; }
        public Command SetBedRoomDataCommand { get; }
        public Command SetLivingRoomDataCommand { get; }
        public Command SetIsTimerAvailableCommand { get; }
        public Command SetTimerCommand { get; }

        public ControllerViewModel(SocketIOService service) {
            isBedRoomOn= false;
            isLivingRoomOn= false;
            isTimerSet= false;

            _service = service;

            SetBedRoomDataCommand = new Command(SetBedRoomAsync);
            SetLivingRoomDataCommand = new Command(SetLivingRoomAsync);
            SetIsTimerAvailableCommand = new Command(SetIsTimerAvailable);
            SetTimerCommand = new Command(SetTimer);

            GetDataCommand = new Command(async() => await GetDataAsync() );

            GetDataCommand.CanExecute(true);
            GetDataCommand.Execute(this);
            GetDataCommand.CanExecute(false);
        }

        private async Task GetDataAsync()
        {
            try
            {
                IsBusy = true;
                await _service.SocketConnect();
                
                if (_service.Commands.GetProperty("bed").ToString() == "off")
                {
                    IsBedRoomOn = false;
                }
                else
                {
                    IsBedRoomOn = true;
                }

                if (_service.Commands.GetProperty("living").ToString() == "off")
                {
                    IsLivingRoomOn = false;
                }
                else
                {
                    IsLivingRoomOn = true;
                }

            } catch (Exception ex)
            {
                Debug.WriteLine("Error:" + ex);
                await Shell.Current.DisplayAlert("Error", "Unables to get data.", "OK");
            } finally
            {
                IsBusy = false;
            }

        }


        private async void SetBedRoomAsync()
        {
            
            try
            {
                if (!IsBedRoomOn)
                {
                    await _service.SocketSendMessage(1,"off");
                } else
                {
                    await _service.SocketSendMessage(1,"on");
                }
            } catch (Exception ex) {
                Debug.WriteLine("Error:" + ex);
                await Shell.Current.DisplayAlert("Error", "Unables to send data.", "OK");
            }
        }


        private async void SetLivingRoomAsync()
        {
            try
            {
                if (!IsLivingRoomOn)
                {
                    await _service.SocketSendMessage(2, "off");
                } else
                {
                    await _service.SocketSendMessage(2, "on");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error:" + ex);
                await Shell.Current.DisplayAlert("Error", "Unables to send data.", "OK");
            }

        }

        private async void SetIsTimerAvailable()
        {
            try
            {
                if (!IsTimerSet)
                {
                    await _service.SocketSendMessage(0, "timer", true);
                }
                else
                {
                    await _service.SocketSendMessage(0, "timer", true);
                }
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error:" + ex);
                await Shell.Current.DisplayAlert("Error", "Unables to send data.", "OK");
            }
        }

        private async void SetTimer()
        {
            try
            {
                await _service.SocketSendMessage(2,"");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error:" + ex);
                await Shell.Current.DisplayAlert("Error", "Unables to send data.", "OK");
            }

        }

        

    }
}
