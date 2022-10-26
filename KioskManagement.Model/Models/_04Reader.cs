using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class _04Reader
    {
        public string ControllerName { get; set; }
        public int? Reader { get; set; }
        public string TypeReader { get; set; }
        public int? Delay { get; set; }
        public int? MacRelay { get; set; }
        public int? GateId { get; set; }
        public int? Orderby { get; set; }
    }
}
