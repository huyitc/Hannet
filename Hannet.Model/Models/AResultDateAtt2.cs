using System;
using System.Collections.Generic;

namespace Hannet.Model.Models
{
    public partial class AResultDateAtt2
    {
        public AResultDateAtt2()
        {
            AResultTimeAtt2s = new HashSet<AResultTimeAtt2>();
        }

        public int RdaId2 { get; set; }
        public DateTime? RdaValue2 { get; set; }

        public virtual ICollection<AResultTimeAtt2> AResultTimeAtt2s { get; set; }
    }
}
