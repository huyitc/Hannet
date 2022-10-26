using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class TCardNo
    {
        public int CaId { get; set; }
        public int? EmId { get; set; }
        public string CaNo { get; set; }
        public string CaNumber { get; set; }
        public int? EmIdCreated { get; set; }
        public int? GeId { get; set; }
        /// <summary>
        /// 0: A, 1: P
        /// </summary>
        public int? CaTypeCheck { get; set; }
        public bool? CaStatus { get; set; }
        /// <summary>
        /// thẻ vẫn của người hiện tại thì using = true, thu hồi thẻ và cấp mới cho người khác thì người cũ = false, người mới = true
        /// </summary>
        public bool? Using { get; set; }
        public bool? CaDamaged { get; set; }
        public bool? CaLost { get; set; }
        public bool? Destroyed { get; set; }
        public DateTime? DestroyedDate { get; set; }
        public DateTime? ApplyDate { get; set; }
        public DateTime? ExpriedDate { get; set; }
        public DateTime? DateEdit { get; set; }
        public string SynAccessDevice { get; set; }

        public virtual TEmployee Em { get; set; }
    }
}
