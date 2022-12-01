using System;
using System.Collections.Generic;

namespace Hannet.Model.Models
{
    public partial class ASchedule
    {
        public int ScheId { get; set; }
        public string ScheDay { get; set; }
        public int? ScheCode { get; set; }
        public TimeSpan? ScheTime1 { get; set; }
        public TimeSpan? ScheTime2 { get; set; }
        public TimeSpan? ScheMiddleTime { get; set; }
        public TimeSpan? ScheTime3 { get; set; }
        public TimeSpan? ScheTime4 { get; set; }
        /// <summary>
        /// Khoảng cách giao ca
        /// </summary>
        public int? ScheTimeDistance { get; set; }
    }
}
