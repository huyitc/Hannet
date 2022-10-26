using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskManagement.Model.ViewModels
{
    public class AZoneViewModel
    {
        public int ZonId { get; set; }
        public string ZonName { get; set; }
        public string ZonDescription { get; set; }
        public bool? ZonStatus { get; set; }
    }
}
