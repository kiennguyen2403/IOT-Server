using IotMobileController.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IotMobileController.Services
{
    static class DataProcesssing
    {
        static public List<float> PieDataProcessing(List<CommandRecord> data)
        {
            List<float> totaltime = new() { 0, 0};
            int currenttimebedroom = 0;
            int currenttimelivingroom = 0;

            foreach (CommandRecord element in data)
            {
                if (element.DeviceId == 2 && element.CommandName == "on")
                {
                    currenttimelivingroom = (int)(DateTime.Parse(element.Time) - DateTime.MinValue).TotalMilliseconds - currenttimelivingroom;
                    totaltime[0] += currenttimelivingroom;
                }
                else if (element.DeviceId == 1 && element.CommandName == "on")
                {
                    currenttimebedroom = (int)(DateTime.Parse(element.Time) - DateTime.MinValue).TotalMilliseconds - currenttimebedroom;
                    totaltime[1] += currenttimebedroom;
                }
                else if (element.DeviceId == 2 && element.CommandName == "off")
                {
                    currenttimelivingroom = 0;
                }
                else if (element.DeviceId == 1 && element.CommandName == "off")
                {
                    currenttimebedroom = 0;
                }
            }

            return totaltime;
        }

    }
}
