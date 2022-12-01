using System;
using System.Collections.Generic;

namespace Hannet.Model.Models
{
    public partial class TTimeAttendancyCheck
    {
        public int TacId { get; set; }
        public string TacType { get; set; }
        public int? TacCode { get; set; }
        public bool? TacCheck { get; set; }
    }
}
