using KioskManagement.Model.Models;
using System.Collections.Generic;

namespace KioskManagement.Model.MappingModels
{
    public class AppMenuUserMapping
    {
        public string UserId { get; set; }

        public List<AppMenu> AppMenus { get; set; }
    }
}