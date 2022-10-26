using Microsoft.EntityFrameworkCore;
using KioskManagement.Data.Infrastructure;
using KioskManagement.Model.MappingModels;
using KioskManagement.Model.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KioskManagement.Data.Repositories
{
    public interface IAppMenuRepository : IRepository<AppMenu>
    {
        Task< IQueryable<AppMenuMapping>> GetTreeMenuByUserId(string userId);
        Task<IQueryable<AppMenuMappingNew>> GetTreeMenuByUserIdNew(string userId);
        Task<IQueryable<AppMenuMapping>> GetTreeMenu();
        Task<IQueryable<AppMenuMapping>> GetMenuUser(string userId);
        Task<IQueryable<AppMenuMappingNew>> GetMenuUserNew(string userId);
    }

    internal class AppMenuRepository : RepositoryBase<AppMenu>, IAppMenuRepository
    {
        private readonly AIOAcessContolDbContext _context;
        public AppMenuRepository(AIOAcessContolDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<IQueryable<AppMenuMapping>> GetMenuUser(string id)
        {
            var query = await ( from menu in _context.AppMenus
                        join mnu in _context.AppMenuUsers on menu.Id equals mnu.MenuId
                        join user in _context.AppUsers on mnu.UserId equals user.Id
                        where user.Id == id
                        select new AppMenuMapping
                        {
                            Id = menu.Id,
                            MenuName = menu.MenuName,
                            ParentId = menu.ParentId,
                            Link=menu.Link,
                            Icon=menu.Icon,
                            ActiveLink=menu.ActiveLink,
                        }).ToListAsync();
            return query.AsQueryable();
        }

        public async Task< IQueryable<AppMenuMapping>> GetTreeMenu()
        {
            var query = await (from menu in _context.AppMenus
                        where menu.ParentId == null
                        select new AppMenuMapping
                        {
                            ParentId = menu.ParentId, 
                            Id = menu.Id,
                            MenuName = menu.MenuName,
                            ActiveLink = menu.ActiveLink,
                            Icon = menu.Icon,
                            Link=menu.Link,
                            Childrens= (from mu in _context.AppMenus where mu.ParentId == menu.Id select new AppMenuMapping { Id=mu.Id, MenuName= mu.MenuName, ParentId=mu.ParentId, ActiveLink=mu.ActiveLink}).ToList()
                        }).ToListAsync();
            return query.AsQueryable();
        }

        public async Task< IQueryable<AppMenuMapping>> GetTreeMenuByUserId(string userId)
        {
            var query =( await (from menu in _context.AppMenus
                        join mur in _context.AppMenuUsers on menu.Id equals mur.MenuId into appUserMenu
                        from mur in appUserMenu.DefaultIfEmpty()
                        select new AppMenuMapping
                        {
                            MenuName= menu.MenuName,
                            ParentId= menu.ParentId,
                            Id= menu.Id,
                            Icon= menu.Icon,
                            Link= menu.Link,
                            ActiveLink= menu.ActiveLink,
                            Childrens=
                            (from mu in _context.AppMenus 
                             join mur1 in _context.AppMenuUsers on mu.Id equals mur1.MenuId
                             where mu.ParentId== menu.Id && mur1.UserId==userId select 
                             new AppMenuMapping { Id=mu.Id, MenuName=mu.MenuName, ParentId= mu.ParentId, Link=mu.Link, Icon=mu.Icon, ActiveLink=mu.ActiveLink}).ToList()
                        }).ToListAsync()).AsQueryable();
            query = query.Where(x=>x.ParentId==null);

            //var menuUsers = from menu in _context.AppMenus
            //                join userMenu in _context.AppMenuUsers on menu.Id equals userMenu.MenuId
            //                join user in _context.Users on userMenu.UserId equals user.Id
            //                where user.Id == userId
            //                select menu;
            List<AppMenuMapping> menus= new List<AppMenuMapping>();
            foreach(var menu in query.ToList())
            {
                if (menu.Childrens.Count>0)
                {
                    menus.Add(menu);
                }
            }
            return menus.AsQueryable();
        }

        public async Task<IQueryable<AppMenuMappingNew>> GetTreeMenuByUserIdNew(string userId)
        {
            var query = (await (from ar in _context.AppRoles
                                join arg in _context.AppRoleGroups on ar.Id equals arg.RoleId into approleGroup
                                from arg in approleGroup.DefaultIfEmpty()

                                join ag in _context.AppGroups on arg.GroupId equals ag.Id into appGroup
                                from ag in appGroup.DefaultIfEmpty()

                                join aug in _context.AppUserGroups on ag.Id equals aug.GroupId into appUserGroup
                                from aug in appUserGroup.DefaultIfEmpty()

                                join au in _context.AppUsers on aug.UserId equals au.Id into appUser
                                from au in appUser.DefaultIfEmpty() where au.Id == userId orderby ar.CreatedDate

                                select new AppMenuMappingNew
                                {
                                    MenuName = ar.Description,
                                    ParentId = ar.ParentId,
                                    Id = ar.Id,
                                    Icon = ar.Icon,
                                    Link = ar.Link,
                                    ActiveLink = ar.ActiveLink,
                                    Childrens =
                                    (from ar1 in _context.AppRoles
                                     join arg1 in _context.AppRoleGroups on ar1.Id equals arg1.RoleId into approleGroup1
                                     from arg1 in approleGroup1.DefaultIfEmpty()

                                     join ag1 in _context.AppGroups on arg1.GroupId equals ag1.Id into appGroup1
                                     from ag1 in appGroup1.DefaultIfEmpty()

                                     join aug1 in _context.AppUserGroups on ag1.Id equals aug1.GroupId into appUserGroup1
                                     from aug1 in appUserGroup1.DefaultIfEmpty()

                                     join au1 in _context.AppUsers on aug1.UserId equals au1.Id into appUser1
                                     from au1 in appUser1.DefaultIfEmpty()
                                     where ar1.ParentId == ar.Id && au1.Id == userId
                                     select new AppMenuMappingNew { Id = ar1.Id, MenuName = ar1.Description, ParentId = ar1.ParentId, Link = ar1.Link, Icon = ar1.Icon, ActiveLink = ar1.ActiveLink }).ToList()
                                }).ToListAsync()).AsQueryable();
            query = query.Where(x => x.ParentId == null);

            //var menuUsers = from menu in _context.AppMenus
            //                join userMenu in _context.AppMenuUsers on menu.Id equals userMenu.MenuId
            //                join user in _context.Users on userMenu.UserId equals user.Id
            //                where user.Id == userId
            //                select menu;
            List<AppMenuMappingNew> menus = new List<AppMenuMappingNew>();
            foreach (var menu in query.ToList())
            {
                if (menu.Childrens.Count > 0)
                {
                    menus.Add(menu);
                }
            }
            return menus.AsQueryable();
        }

        public async Task<IQueryable<AppMenuMappingNew>> GetMenuUserNew(string userId)
        {
            var query = (await (from ar in _context.AppRoles
                                join arg in _context.AppRoleGroups on ar.Id equals arg.RoleId into approleGroup
                                from arg in approleGroup.DefaultIfEmpty()

                                join ag in _context.AppGroups on arg.GroupId equals ag.Id into appGroup
                                from ag in appGroup.DefaultIfEmpty()

                                join aug in _context.AppUserGroups on ag.Id equals aug.GroupId into appUserGroup
                                from aug in appUserGroup.DefaultIfEmpty()

                                join au in _context.AppUsers on aug.UserId equals au.Id into appUser
                                from au in appUser.DefaultIfEmpty()
                                where au.Id == userId
                                orderby ar.CreatedDate

                                select new AppMenuMappingNew
                                {
                                    MenuName = ar.Description,
                                    ParentId = ar.ParentId,
                                    Id = ar.Id,
                                    Icon = ar.Icon,
                                    Link = ar.Link,
                                    ActiveLink = ar.ActiveLink,
                                    Childrens =
                                    (from ar1 in _context.AppRoles
                                     join arg1 in _context.AppRoleGroups on ar1.Id equals arg1.RoleId into approleGroup1
                                     from arg1 in approleGroup1.DefaultIfEmpty()

                                     join ag1 in _context.AppGroups on arg1.GroupId equals ag1.Id into appGroup1
                                     from ag1 in appGroup1.DefaultIfEmpty()

                                     join aug1 in _context.AppUserGroups on ag1.Id equals aug1.GroupId into appUserGroup1
                                     from aug1 in appUserGroup1.DefaultIfEmpty()

                                     join au1 in _context.AppUsers on aug1.UserId equals au1.Id into appUser1
                                     from au1 in appUser1.DefaultIfEmpty()
                                     where ar1.ParentId == ar.Id && au1.Id == userId
                                     select new AppMenuMappingNew { Id = ar1.Id, MenuName = ar1.Description, ParentId = ar1.ParentId, Link = ar1.Link, Icon = ar1.Icon, ActiveLink = ar1.ActiveLink }).ToList()
                                }).ToListAsync()).AsQueryable();

            
            return query;
        }
    }
}