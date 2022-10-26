using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class ALeaveType
    {
        public int LetId { get; set; }
        public string LetName { get; set; }
        public bool? LetSalary { get; set; }
        public bool? LetStatus { get; set; }
    }
}
