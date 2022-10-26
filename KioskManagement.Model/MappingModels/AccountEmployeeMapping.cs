using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskManagement.Model.MappingModels
{
    public class AccountEmployeeMapping
    {
        public int AccId { get; set; }
        public string AccUsername { get; set; }
        public bool? AccStatus { get; set; }
        public string Shift { get; set; }
        public TimeSpan? FromTime { get; set; }
        public TimeSpan? ToTime { get; set; }
        public string IpLogin { get; set; }
        public string Area { get; set; }
        //Employees
        public int EmId { get; set; }
        public string EmName { get; set; }
    }
}
