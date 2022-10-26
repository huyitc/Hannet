using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class ALeaveDetail
    {
        public int LeadId { get; set; }
        public int LeaId { get; set; }
        public int? EmId { get; set; }
        public DateTime? Date { get; set; }
        /// <summary>
        /// 0: vào ngày ko làm việc- không lương, 1: nửa ca, 2: cả ngày
        /// </summary>
        public int? LeaCheckAtt { get; set; }
    }
}
