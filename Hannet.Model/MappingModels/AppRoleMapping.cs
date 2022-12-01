using Hannet.Model.Models;
using System.Collections.Generic;

namespace Hannet.Model.MappingModels
{
    public class AppRoleMapping
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ParentId { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }
        public string ActiveLink { get; set; }
        public int? Order_By { get; set; }
        public List<AppRoleMapping> Childrens { get; set; }
    }
}