using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class TDeviceType
    {
        public TDeviceType()
        {
            TDevices = new HashSet<TDevice>();
        }

        public int DevTypeId { get; set; }
        public string DevTypeName { get; set; }
        public string DevTypeCode { get; set; }

        public virtual ICollection<TDevice> TDevices { get; set; }
    }
}
