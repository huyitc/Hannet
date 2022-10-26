using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class ADateAttendance
    {
        public int DatId { get; set; }
        public int? DevId { get; set; }
        public DateTime? DatValue { get; set; }
    }
}
