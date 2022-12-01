using System;
using System.Collections.Generic;

namespace Hannet.Model.Models
{
    public partial class TVisiblecontrol
    {
        public int ViId { get; set; }
        public int? PriId { get; set; }
        public int? MnuId { get; set; }

        public virtual TMenulist Mnu { get; set; }
        public virtual TPrivile Pri { get; set; }
    }
}
