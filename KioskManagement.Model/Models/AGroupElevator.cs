using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class AGroupElevator
    {
        public int GeId { get; set; }
        public string GeName { get; set; }
        public string GeNumberFloor { get; set; }
        /// <summary>
        /// TRẠNG THÁI HOẠT ĐỘNG CỦA NHÓM THANG MÁY
        /// </summary>
        public bool? GeStatus { get; set; }
    }
}
