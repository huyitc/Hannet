using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class AMonthAtt2
    {
        public AMonthAtt2()
        {
            AResultMonthAtt2s = new HashSet<AResultMonthAtt2>();
        }

        public int MaId2 { get; set; }
        public int? MaMonth2 { get; set; }
        public int? MaYear2 { get; set; }

        public virtual ICollection<AResultMonthAtt2> AResultMonthAtt2s { get; set; }
    }
}
