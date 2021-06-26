using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AkatsukiDokkanInterrogator.Models
{
    public class UserAccount
    {

        public string AdId { get; set; }
        public string Country { get; set; }

        public string Currency { get; set; }

        public string Device { get; set; }

        public string DeviceModel { get; set; }

        public string OsVersion { get; set; }

        public string Platform { get; set; }

        public string UniqueId { get; set; }
    }

}
