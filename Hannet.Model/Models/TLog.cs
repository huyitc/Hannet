using System;
using System.Collections.Generic;

namespace Hannet.Model.Models
{
    public partial class TLog
    {
        public int LogId { get; set; }
        public int? TacId { get; set; }
        public int? DevId { get; set; }
        public int? EmId { get; set; }
        public int? CaId { get; set; }
        public DateTime? LogDate { get; set; }
        public string LogTime { get; set; }
    }
}
