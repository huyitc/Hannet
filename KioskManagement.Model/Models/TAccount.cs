using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class TAccount
    {
        public int AccId { get; set; }
        public int? PriId { get; set; }
        public int? EmId { get; set; }
        public string AccUsername { get; set; }
        public string AccPassword { get; set; }
        public bool? AccStatus { get; set; }
        public string IpLogin { get; set; }
        public int? ShiftId { get; set; }

        public virtual TEmployee Em { get; set; }
        public virtual TPrivile Pri { get; set; }
    }
}
