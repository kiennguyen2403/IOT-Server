using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IotMobileController.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (isBusy == value)
                    return;
                isBusy = value;
                OnPropertyChanged(nameof(IsBusy));

            }
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
             PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
