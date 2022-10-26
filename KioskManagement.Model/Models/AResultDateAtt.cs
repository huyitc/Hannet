using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class AResultDateAtt
    {
        public AResultDateAtt()
        {
            AResultTimeAtts = new HashSet<AResultTimeAtt>();
        }

        public int RdaId { get; set; }
        public DateTime? RdaValue { get; set; }

        public virtual ICollection<AResultTimeAtt> AResultTimeAtts { get; set; }
    }
}
