using Hannet.Model.Abtracts;
using System.Collections.Generic;

namespace Hannet.Model.ViewModels
{
    public class AppGroupViewModel:Auditable
    {
        public string GroupCode { get; set; }

        public string Name { get; set; }

        public virtual List<AppRoleViewModel> Roles { get; set; }
    }
}