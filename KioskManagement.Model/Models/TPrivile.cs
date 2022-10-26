using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class TPrivile
    {
        public TPrivile()
        {
            TAccounts = new HashSet<TAccount>();
            TVisiblecontrols = new HashSet<TVisiblecontrol>();
        }

        public int PriId { get; set; }
        public string PriDescription { get; set; }
        public bool? PriStatus { get; set; }

        public virtual ICollection<TAccount> TAccounts { get; set; }
        public virtual ICollection<TVisiblecontrol> TVisiblecontrols { get; set; }
    }
}
