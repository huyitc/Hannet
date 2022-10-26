using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class ALeave
    {
        public int LeaId { get; set; }
        public int? LetId { get; set; }
        public int? EmId { get; set; }
        public DateTime? LeaFrom { get; set; }
        /// <summary>
        /// 1: ca sáng,2: ca chiều, 3: cả ngày
        /// </summary>
        public int? FromCheck { get; set; }
        public DateTime? LeaTo { get; set; }
        public int? ToCheck { get; set; }
        public bool? LeaStatus { get; set; }
    }
}
