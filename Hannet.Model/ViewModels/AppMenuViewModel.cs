using Hannet.Model.Abtracts;

namespace Hannet.Model.ViewModels
{
    public class AppMenuViewModel : Auditable
    {
        public string MenuName { get; set; }

        public int? ParentId { get; set; }

        public string Icon { get; set; }

        public string Link { get; set; }

        public string ActiveLink { get; set; }
    }
}