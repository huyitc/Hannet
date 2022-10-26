using Microsoft.EntityFrameworkCore;
using KioskManagement.Data.Infrastructure;
using KioskManagement.Model.MappingModels;
using KioskManagement.Model.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;

namespace KioskManagement.Data.Repositories
{
    public interface IApplicationRoleRepository : IRepository<AppRole>
    {
        Task<IQueryable<AppRole>> GetListRoleByGroupId(int groupId);

        Task<IQueryable<AppRoleMapping>> GetTreeRoles();
        Task<IQueryable<AppMenuMappingNew>> GetTreeMenuByUserId(string userId);
        Task<IQueryable<AppUser>> GetListUserByGroupId(int groupId);
    }

    public class ApplicationRoleRepository : RepositoryBase<AppRole>, IApplicationRoleRepository
    {
        private AIOAcessContolDbContext DbContext;

        public ApplicationRoleRepository(AIOAcessContolDbContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<IQueryable<AppRole>> GetListRoleByGroupId(int groupId)
        {
            var query = await (from g in DbContext.AppRoles
                               join ug in DbContext.AppRoleGroups on g.Id equals ug.RoleId
                               where ug.GroupId == groupId
                               select g).ToListAsync();
            return query.AsQueryable();
        }

        public async Task<IQueryable<AppRoleMapping>> GetTreeRoles()
        {
            var query = await (from r in DbContext.AppRoles
                               select new AppRoleMapping
                               {
                                   Id = r.Id,
                                   Description = r.Description,
                                   Name = r.Name,
                                   ParentId = r.ParentId,
                                   Childrens = (from r1 in DbContext.AppRoles
                                                where r1.ParentId == r.Id
                                                select new AppRoleMapping
                                                {
                                                    Id = r1.Id,
                                                    ParentId = r1.ParentId,
                                                    Description = r1.Description,
                                                    Name = r1.Name,
                                                    Childrens =
                                                (from r2 in DbContext.AppRoles where r2.ParentId == r1.Id select new AppRoleMapping { ParentId = r2.ParentId, Name = r2.Name, Id = r2.Id, Description = r2.Description }).ToList()
                                                }).ToList()
                               }).ToListAsync();
            return query.Where(x => x.ParentId == null).AsQueryable();
        }

        public async Task<IQueryable<AppMenuMappingNew>> GetTreeMenuByUserId(string userId)
        {
            
            var parameters = new SqlParameter[]
              {
                    new SqlParameter("@userId",SqlDbType.NVarChar){Value = userId}
              };
            var listChildren = DbContext.GetMenuList.FromSqlRaw("dbo.GetMenuList @userId", parameters).ToList();
            var listParents = listChildren.Select(x => x.ParentId).ToList().Distinct();

            List< AppMenuMappingNew > listChildrenNew = new List< AppMenuMappingNew >();
            foreach (var child in listChildren)
            {
                listChildrenNew.Add( new AppMenuMappingNew() { Id = child.Id,MenuName = child.Description,ParentId = child.ParentId,Icon = child.Icon,Link = child.Link,ActiveLink = child.ActiveLink });
            }

            var parents = (await (from ar in DbContext.AppRoles
                                
                                orderby ar.Order_By

                                select new AppMenuMappingNew
                                {
                                    MenuName = ar.Description,
                                    ParentId = ar.ParentId,
                                    Id = ar.Id,
                                    Icon = ar.Icon,
                                    Link = ar.Link,
                                    ActiveLink = ar.ActiveLink,
                                    Childrens = new List<AppMenuMappingNew>()
                                   
                                }).ToListAsync()).AsQueryable();
            parents = parents.Where(x => listParents.Contains(x.Id));
            List<AppMenuMappingNew> menus = new List<AppMenuMappingNew>();
            foreach (var parent in parents)
            {
                    var children = listChildrenNew.Where(x => x.ParentId == parent.Id).ToList();
                    parent.Childrens = children;
                    menus.Add(parent);
                
            }

            return menus.AsQueryable();
        }

        public async Task<IQueryable<AppUser>> GetListUserByGroupId(int groupId)
        {
            var listUsers = (await (from aug in DbContext.AppUserGroups
                                    join au in DbContext.AppUsers on aug.UserId equals au.Id
                                    where aug.GroupId == groupId
                                    select new AppUser
                                    {
                                        Id = au.Id,
                                        UserName = au.UserName,
                                        Email = au.Email,
                                        PhoneNumber = au.PhoneNumber,
                                        NormalizedUserName = au.NormalizedUserName,
                                        Image = au.Image,
                                        FullName = au.FullName,
                                        IsDeleted = au.IsDeleted,
                                        CreatedBy = au.CreatedBy,
                                        UpdatedBy = au.UpdatedBy

                                    }).ToListAsync());
            return listUsers.AsQueryable();
        }
    }
}