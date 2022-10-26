using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskManagement.Model.ViewModels
{
    public class TDepartmentViewModel
    {
        public int DepId { get; set; }
        public string DepName { get; set; }
        public string DepDescription { get; set; }
        public bool? DepStatus { get; set; }
    }
}
