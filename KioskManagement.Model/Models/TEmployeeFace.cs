using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class TEmployeeFace
    {
        public int FaceId { get; set; }
        public int? EmId { get; set; }
        public string FaceData { get; set; }
        public string DevTypeCode { get; set; }

        public virtual TEmployee Em { get; set; }
    }
}
