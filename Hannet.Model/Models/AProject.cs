using System;
using System.Collections.Generic;

namespace Hannet.Model.Models
{
    public partial class AProject
    {
        public AProject()
        {
            ATimeAdds = new HashSet<ATimeAdd>();
        }

        public int ProId { get; set; }
        public string ProName { get; set; }
        public bool? ProStatus { get; set; }

        public virtual ICollection<ATimeAdd> ATimeAdds { get; set; }
    }
}
