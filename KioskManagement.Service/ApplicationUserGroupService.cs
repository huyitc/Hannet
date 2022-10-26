using KioskManagement.Data.Repositories;
using System.Threading.Tasks;

namespace KioskManagement.Service
{
    public interface IApplicationUserGroupService
    {
        Task DeleteMultiple(int groupId);
    }

    public class ApplicationUserGroupService : IApplicationUserGroupService
    {
        private IApplicationUserGroupRepository _appUserGroupRepository;

        public ApplicationUserGroupService(IApplicationUserGroupRepository applicationUserGroupRepository)
        {
            _appUserGroupRepository = applicationUserGroupRepository;
        }

        public async Task DeleteMultiple(int groupId)
        {
            await _appUserGroupRepository.DeleteMulti(x => x.GroupId == groupId);
        }
    }
}