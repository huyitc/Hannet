using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class AResultTimeAtt
    {
        public int RtaId { get; set; }
        public int? RdaId { get; set; }
        public int? EmId { get; set; }
        public DateTime? Date { get; set; }
        public string Day { get; set; }
        public TimeSpan? Time1 { get; set; }
        public string Result1 { get; set; }
        public TimeSpan? Time2 { get; set; }
        public string Result2 { get; set; }
        public TimeSpan? Time3 { get; set; }
        public string Result3 { get; set; }
        public TimeSpan? Time4 { get; set; }
        public string Result4 { get; set; }
        public string ResultAll { get; set; }
        /// <summary>
        /// 0: nghỉ, 1: vào muộn hoặc về sớm, 2: vào muộn và về sớm
        /// </summary>
        public int? ResultCheck { get; set; }
        public int? TacId { get; set; }

        public virtual AResultDateAtt Rda { get; set; }
    }
}
