using System;
using System.Collections.Generic;

namespace Hannet.Model.Models
{
    public partial class AResultTimeAtt2
    {
        public int RtaId2 { get; set; }
        public int? RdaId2 { get; set; }
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
        public int? ResultCheck { get; set; }
        public int? TacId { get; set; }

        public virtual AResultDateAtt2 RdaId2Navigation { get; set; }
    }
}
