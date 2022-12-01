using System;
using System.Collections.Generic;

namespace Hannet.Model.Models
{
    public partial class ATimeAttendance
    {
        public int TatId { get; set; }
        public int? TacId { get; set; }
        public int? DatId { get; set; }
        public int? DevId { get; set; }
        public int? CaId { get; set; }
        public int? EmId { get; set; }
        public DateTime? TatDate { get; set; }
        public TimeSpan? TatTime { get; set; }
    }
}
