using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class TEmployeeQrcode
    {
        public int EmId { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string QrCode { get; set; }
        public string QrCodeUrl { get; set; }
    }
}
