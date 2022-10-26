using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class AHoliday
    {
        public int HolId { get; set; }
        public string HolName { get; set; }
        public string HolFromDate { get; set; }
        public string HolToDate { get; set; }
        public bool? HolStatus { get; set; }
    }
}
