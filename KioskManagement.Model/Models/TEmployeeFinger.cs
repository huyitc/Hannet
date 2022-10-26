using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class TEmployeeFinger
    {
        public int FinId { get; set; }
        public int EmId { get; set; }
        public byte[] FinValue { get; set; }
        public byte[] FinFormatValue { get; set; }
        public int? FinPosition { get; set; }
        public string Hand { get; set; }
        public string FinUrlValue { get; set; }
        public string FinDeviceType { get; set; }

        public virtual TEmployee Em { get; set; }
    }
}
