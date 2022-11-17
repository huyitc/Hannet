using KioskManagement.Data.Infrastructure;
using KioskManagement.Model.MappingModels;
using KioskManagement.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskManagement.Data.Repositories
{
    public interface ITEmployeeRepository : IRepository<TEmployee>
    {
        Task<IQueryable<TEmployeeMapping>> GetAllByMapping(string keyword);
        Task<IEnumerable<TEmployeeTypeMapping>> GetTypeEmployee();
        public IEnumerable<AccountEmployeeMapping> GetAllAccIdByEmId(int Em_Id);
        Task<IQueryable<EmployeeDepMapping>> GetEmployeesByDepartment(List<int> depIds);
    }
    public class TEmployeeRepository : RepositoryBase<TEmployee>, ITEmployeeRepository
    {
        private AIOAcessContolDbContext _dbContext;
        public TEmployeeRepository(AIOAcessContolDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<AccountEmployeeMapping> GetAllAccIdByEmId(int Em_Id)
        {
            var query = from acc in _dbContext.TAccounts
                        join em in _dbContext.TEmployees on acc.EmId equals em.EmId
                        where em.EmId == Em_Id
                        select new AccountEmployeeMapping
                        {
                            AccId = acc.AccId,
                        };
            return query;
        }

        public async Task<IQueryable<TEmployeeMapping>> GetAllByMapping(string keyword)
        {
            var query = (await (from em in _dbContext.TEmployees
                                join emt in _dbContext.TEmployeeTypes on em.EmTypeId equals emt.EmTypeId
                                join zon in _dbContext.AZones on em.ZonId equals zon.ZonId into zonem
                                from zon in zonem.DefaultIfEmpty()
                                join r in _dbContext.TRegencies on em.RegId equals r.RegId into emr
                                from r in emr.DefaultIfEmpty()
                                join dep in _dbContext.TDepartments on em.DepId equals dep.DepId into emdep
                                from dep in emdep.DefaultIfEmpty()
                                select new TEmployeeMapping
                                {
                                    EmId = em.EmId,
                                    RegId = r.RegId,
                                    EmTypeId = em.EmTypeId,
                                    EmType = emt.EmType,
                                    RegName = r.RegName,
                                    DepId = em.DepId,
                                    DepName = dep.DepName,
                                    ZonId = zon.ZonId,
                                    ZonName = zon.ZonName,
                                    EmName = em.EmName,
                                    EmCode = em.EmCode,
                                    Description = em.Description,
                                    EmGender = em.EmGender == "M" ? "Nam" : "Nữ",
                                    EmPhone = em.EmPhone,
                                    EmBirthdate = em.EmBirthdate,
                                    EmIdentityNumber = em.EmIdentityNumber,
                                    EmEmail = em.EmEmail,
                                    EmImage = em.EmImage,
                                    EmStatus = em.EmStatus,
                                    FaceExist = em.FaceExist,
                                    CheckFace = (from f in _dbContext.TEmployeeFaces where em.EmId == f.EmId select f).Count() > 0 ? true : false,
                                }).ToListAsync()).AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.EmName.ToLower().Contains(keyword.ToLower()));
            return query;
        }

        public async Task<IEnumerable<TEmployeeTypeMapping>> GetTypeEmployee()
        {
            var query = await (from emt in _dbContext.TEmployeeTypes
                               select new TEmployeeTypeMapping
                               {
                                   EmTypeId = emt.EmTypeId,
                                   EmType = emt.EmType,
                                   EmCheck = emt.EmCheck
                               }).ToListAsync();
            return query;
        }

        // Lấy danh sách nhân viên theo nhiều phòng ban
        public async Task<IQueryable<EmployeeDepMapping>> GetEmployeesByDepartment(List<int> depIds)
        {
            var query = (await (from em in _dbContext.TEmployees
                                where depIds.Contains((int)em.DepId) && em.EmStatus == true
                                select new EmployeeDepMapping
                                {
                                    EmId = em.EmId,
                                    EmName = em.EmName
                                }).ToListAsync()).AsQueryable();
            return query;
        }
    }
}
