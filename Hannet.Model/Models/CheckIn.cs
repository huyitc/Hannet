using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Model.Models
{
    public class CheckIn
    {
        public int CheckInId { get; set; }
        public string PersonName { get; set; }
        public string Date { get; set; }
        public long CheckinTime { get; set; }
        public string AliasID { get; set; }
        public string PlaceID { get; set; }
        public string PersonID { get; set; }
        public string Avatar { get; set; }
        public string Place { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public string DeviceID { get; set; }
        public string DeviceName { get; set; }
    }
}
