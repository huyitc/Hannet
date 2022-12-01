using System;
using System.Collections.Generic;

namespace Hannet.Model.Models
{
    public partial class TMenulist
    {
        public TMenulist()
        {
            TVisiblecontrols = new HashSet<TVisiblecontrol>();
        }

        public int MnuId { get; set; }
        public string MnuName { get; set; }
        public string MnuDisplay { get; set; }
        public string MnuParent { get; set; }

        public virtual ICollection<TVisiblecontrol> TVisiblecontrols { get; set; }
    }
}
