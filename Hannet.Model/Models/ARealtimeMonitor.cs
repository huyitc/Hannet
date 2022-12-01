﻿using System;
using System.Collections.Generic;

namespace Hannet.Model.Models
{
    public partial class ARealtimeMonitor
    {
        public int RtId { get; set; }
        public int? TacId { get; set; }
        public int? DevId { get; set; }
        public int? CaId { get; set; }
        public int? EmId { get; set; }
        public DateTime? TatDate { get; set; }
        public TimeSpan? TatTime { get; set; }
        /// <summary>
        /// trạng thái vào - ra 0: KHÔNG PHÂN BIỆT, 1: VÀO , 2: RA 
        /// </summary>
        public bool? IoStatus { get; set; }
        public string Zone { get; set; }
    }
}
