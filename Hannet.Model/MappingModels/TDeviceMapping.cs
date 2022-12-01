using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Model.MappingModels
{
    public class TDeviceMapping
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
        public int? DevLaneCheck { get; set; }
        public string ZonName { get; set; }
        public string DevTypeName { get; set; }
    }
}
