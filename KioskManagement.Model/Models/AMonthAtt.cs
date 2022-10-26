using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class AMonthAtt
    {
        public AMonthAtt()
        {
            AResultMonthAtts = new HashSet<AResultMonthAtt>();
        }

        public int MaId { get; set; }
        public int? MaMonth { get; set; }
        public int? MaYear { get; set; }

        public virtual ICollection<AResultMonthAtt> AResultMonthAtts { get; set; }
    }
}
