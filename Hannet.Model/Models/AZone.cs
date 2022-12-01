using System;
using System.Collections.Generic;

namespace Hannet.Model.Models
{
    public partial class AZone
    {
        public AZone()
        {
            TDevices = new HashSet<TDevice>();
            TEmployees = new HashSet<TEmployee>();
        }

        public int ZonId { get; set; }
        public string ZonName { get; set; }
        public string ZonDescription { get; set; }
        public bool? ZonStatus { get; set; }
        public int? PlaceId { get; set; }

        public virtual ICollection<TDevice> TDevices { get; set; }
        public virtual ICollection<TEmployee> TEmployees { get; set; }
    }
}
