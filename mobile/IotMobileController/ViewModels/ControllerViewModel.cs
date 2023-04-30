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
        private readonly WebSocketService _service;


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

        public ControllerViewModel(WebSocketService service) {
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
                var response = await _service.SocketConnect();
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
                await _service.SocketSendMessage("bedroom1");
            } catch (Exception ex) {
                Debug.WriteLine("Error:" + ex);
                await Shell.Current.DisplayAlert("Error", "Unables to send data.", "OK");
            }
        }


        private async void SetLivingRoomAsync()
        {
            try
            {
                await _service.SocketSendMessage("bedroom1");
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
                await _service.SocketSendMessage("bedroom1");
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
                await _service.SocketSendMessage("bedroom1");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error:" + ex);
                await Shell.Current.DisplayAlert("Error", "Unables to send data.", "OK");
            }

        }

        

    }
}
