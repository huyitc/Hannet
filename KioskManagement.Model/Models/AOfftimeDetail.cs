using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class AOfftimeDetail
    {
        public int OffdId { get; set; }
        public int OffId { get; set; }
        public int? EmId { get; set; }
        public DateTime? Date { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
    }
}
