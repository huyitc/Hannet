using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskManagement.Model.MappingModels
{
    public class TGroupAcessDetailMapping
    {
        public int GaId { get; set; }

        public List<TDeviceItemMapping> DeviceItemMappings { get; set; }
    }

    public class TDeviceItemMapping
    {
        public int DevId { set; get; }

        public bool Selected { get; set; }
    }
}
