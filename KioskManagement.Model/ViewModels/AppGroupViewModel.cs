using KioskManagement.Model.Abtracts;
using System.Collections.Generic;

namespace KioskManagement.Model.ViewModels
{
    public class AppGroupViewModel:Auditable
    {
        public string GroupCode { get; set; }

        public string Name { get; set; }

        public virtual List<AppRoleViewModel> Roles { get; set; }
    }
}