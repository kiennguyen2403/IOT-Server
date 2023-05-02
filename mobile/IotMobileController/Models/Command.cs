using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IotMobileController.Models
{
    public class CommandRecord
    {
        public int Id { get; set; }
        public String Time { get; set; }
        public string CommandName { get; set; }
        public int DeviceId { get; set; }
    }
}
