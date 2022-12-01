using System;
using System.Collections.Generic;

namespace Hannet.Model.Models
{
    public partial class AOfftime
    {
        public int OffId { get; set; }
        public int? EmId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public bool? Accept { get; set; }
    }
}
