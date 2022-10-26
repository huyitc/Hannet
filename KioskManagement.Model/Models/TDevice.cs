using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class TDevice
    {
        public TDevice()
        {
            AIoInfos = new HashSet<AIoInfo>();
            TGroupAccessDetails = new HashSet<TGroupAccessDetail>();
        }

        public int DevId { get; set; }
        public int? DevTypeId { get; set; }
        public int? ZonId { get; set; }
        public string DevName { get; set; }
        public string DevIp { get; set; }
        public int? DevPort { get; set; }
        public string DevCode { get; set; }
        public string DevSerialnumber { get; set; }
        public string DevPartnumber { get; set; }
        public string DevMacaddress { get; set; }
        public bool? DevTimeCheck { get; set; }
        public bool? DevStatus { get; set; }
        /// <summary>
        /// trạng thái vào - ra 0: KHÔNG PHÂN BIỆT, 1: VÀO , 2: RA 
        /// </summary>
        public int? DevLaneCheck { get; set; }

        public virtual TDeviceType DevType { get; set; }
        public virtual AZone Zon { get; set; }
        public virtual ICollection<AIoInfo> AIoInfos { get; set; }
        public virtual ICollection<TGroupAccessDetail> TGroupAccessDetails { get; set; }
    }
}
