using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskManagement.Model.MappingModels
{
    public class AZoneDeviceMapping
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<AZoneDeviceMapping> Childrens { get; set; }
    }
}
