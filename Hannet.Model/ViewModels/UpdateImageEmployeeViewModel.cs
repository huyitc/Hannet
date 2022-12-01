using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Model.ViewModels
{
    public class UpdateImageEmployeeViewModel
    {
        public int EmId { get; set; }
        public string EmCode { get; set; }
        public string EmName { get; set; }
        public byte[] EmImage { get; set; }
        public int? PlaceId { get; set; }
    }
}
