using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class _02Controler
    {
        public int Id { get; set; }
        public int? GateId { get; set; }
        public string ControlerIp { get; set; }
        public string ControlerName { get; set; }
        public string MacRelay { get; set; }
    }
}
