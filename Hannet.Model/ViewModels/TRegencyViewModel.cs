using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Model.ViewModels
{
    public class TRegencyViewModel
    {
        public int RegId { get; set; }
        public string RegName { get; set; }
        public string RegDescription { get; set; }
        public bool? RegStatus { get; set; }
    }
}
