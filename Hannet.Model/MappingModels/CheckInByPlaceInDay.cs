using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Model.MappingModels
{
    public class CheckInByPlaceInDay
    {
        public string personName { get; set; }
        public string date { get; set; }
        public long checkinTime { get; set; }
        public string aliasID { get; set; }
        public string placeID { get; set; }
        public string personID { get; set; }
        public string avatar { get; set; }
        public string place { get; set; }
        public string title { get; set; }
        public int type { get; set; }
        public string deviceID { get; set; }
        public string deviceName { get; set; }
    }

    public class CheckInByPlaceReponse
    {
        public int statusCode { get; set; }
        public int returnCode   { get; set; }
        public string returnMessage { get; set; }
        public List<CheckInByPlaceInDay> data { get; set; }
    }
}
