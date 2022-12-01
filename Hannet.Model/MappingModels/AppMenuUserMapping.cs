using Hannet.Model.Models;
using System.Collections.Generic;

namespace Hannet.Model.MappingModels
{
    public class AppMenuUserMapping
    {
        public string UserId { get; set; }

        public List<AppMenu> AppMenus { get; set; }
    }
}