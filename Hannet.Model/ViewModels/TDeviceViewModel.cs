using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Model.ViewModels
{
    public class TDeviceViewModel
    {
        public int DevId { get; set; }
        public int? DevTypeId { get; set; }
        public int? ZonId { get; set; }
        public string DevName { get; set; }
        public string DevIp { get; set; }
        public int? DevPort { get; set; }
        public string DevCode { get; set; }
        public string DevSerialnumber { get; set; }
        public string DevPartnumber { get; set; }
        public string DevMacaddress { get; set; }
        public bool? DevTimeCheck { get; set; }
        public bool? DevStatus { get; set; }
        /// <summary>
        /// trạng thái vào - ra 0: KHÔNG PHÂN BIỆT, 1: VÀO , 2: RA 
        /// </summary>
        public int? DevLaneCheck { get; set; }
        public string DeviceId { get; set; }
    }
    public class DeviceHannet
    {
       public string deviceID { get; set; }
       public string deviceName { get; set; }
    }
}
