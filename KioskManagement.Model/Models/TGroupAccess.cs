using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class TGroupAccess
    {
        public TGroupAccess()
        {
            TEmployees = new HashSet<TEmployee>();
            TGroupAccessDetails = new HashSet<TGroupAccessDetail>();
        }

        public int GaId { get; set; }
        public string GaName { get; set; }
        public bool? GaStatus { get; set; }

        public virtual ICollection<TEmployee> TEmployees { get; set; }
        public virtual ICollection<TGroupAccessDetail> TGroupAccessDetails { get; set; }
    }
}
