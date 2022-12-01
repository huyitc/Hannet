using System;
using System.Collections.Generic;

namespace Hannet.Model.Models
{
    public partial class AIoInfo
    {
        public int IoIdA { get; set; }
        /// <summary>
        /// Mã thiết bị
        /// </summary>
        public int? DevId { get; set; }
        public int? CaId { get; set; }
        public TimeSpan? IoTime { get; set; }
        public DateTime? IoDate { get; set; }
        public int? EmId { get; set; }

        public virtual TDevice Dev { get; set; }
    }
}
