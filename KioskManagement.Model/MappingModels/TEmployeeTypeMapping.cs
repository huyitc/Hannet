using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskManagement.Model.MappingModels
{
    public class TEmployeeTypeMapping
    {
        public int EmTypeId { get; set; }
        public string EmType { get; set; }
        public int? EmCheck { get; set; }
    }
}
