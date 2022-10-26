using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class PShift
    {
        public PShift()
        {
            TAccounts = new HashSet<TAccount>();
        }

        public int ShiftId { get; set; }
        public string Shift { get; set; }
        public TimeSpan? FromTime { get; set; }
        public TimeSpan? ToTime { get; set; }
        public string Area { get; set; }

        public virtual ICollection<TAccount> TAccounts { get; set; }
    }
}
