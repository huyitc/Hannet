using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KioskManagement.Model.Models
{
    public partial class TGroupAccess
    {
        public TGroupAccess()
        {
            TGroupAccessDetails = new HashSet<TGroupAccessDetail>();
        }

        [Key]
        public int GaId { get; set; }
        public string GaName { get; set; }
        public bool? GaStatus { get; set; }

        public virtual ICollection<TGroupAccessDetail> TGroupAccessDetails { get; set; }
    }
}
