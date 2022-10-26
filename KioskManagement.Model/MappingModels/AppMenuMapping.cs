using KioskManagement.Model.Models;
using System.Collections.Generic;

namespace KioskManagement.Model.MappingModels
{
    public class AppMenuMapping
    {
        public int Id { get; set; }

        public string MenuName { get; set; }

        public int? ParentId { get; set; }

        public string Icon { get; set; }

        public string Link { get; set; }

        public string ActiveLink { get; set; }

        public List<AppMenuMapping> Childrens { get; set; }
    }

    public class AppMenuMappingNew
    {
        public string Id { get; set; }

        public string MenuName { get; set; }

        public string? ParentId { get; set; }

        public string Icon { get; set; }

        public string Link { get; set; }

        public string ActiveLink { get; set; }

        public List<AppMenuMappingNew> Childrens { get; set; }
    }

    public class AppMenuMappingSQL
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public string? ParentId { get; set; }

        public string Icon { get; set; }

        public string Link { get; set; }

        public string ActiveLink { get; set; }

        public int Order_By { get; set; }
    }
}