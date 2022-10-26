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
        IEnumerable<AccountEmployeeMapping> GetFromTimeToTimeByID(int Acc_Id);
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
                        join shift in _dbContext.PShifts on acc.ShiftId equals shift.ShiftId
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
                                join r in _dbContext.TRegencies on em.RegId equals r.RegId into emr
                                from r in emr.DefaultIfEmpty()
                                join ga in _dbContext.TGroupAccesses on em.GaId equals ga.GaId into emga
                                from ga in emga.DefaultIfEmpty()
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
                                    EmIdCreated = em.EmIdCreated,
                                    EmName = em.EmName,
                                    EmCode = em.EmCode,
                                    EmAddress = em.EmAddress,
                                    EmGender = em.EmGender == "M" ? "Nam" : "Nữ",
                                    EmPhone = em.EmPhone,
                                    EmBirthdate = em.EmBirthdate,
                                    EmIdentityNumber = em.EmIdentityNumber,
                                    EmEmail = em.EmEmail,
                                    EmImage = em.EmImage,
                                    EmStatus = em.EmStatus,
                                    EmTimeCheck = em.EmTimeCheck,
                                    EditStatus = em.EditStatus,
                                    GaId = ga.GaId,
                                    GaName = ga.GaName,
                                    CreatedDate = em.CreatedDate,
                                    DevIdSynchronized = em.DevIdSynchronized,
                                    SchDevId = em.SchDevId,
                                    FaceExist = em.FaceExist,
                                    Pin = em.Pin,
                                    CheckCard = (from c in _dbContext.TCardNos where em.EmId == c.EmId && c.Using == true && c.CaStatus == true select c).Count() > 0 ? true : false,
                                    CheckFace = (from f in _dbContext.TEmployeeFaces where em.EmId == f.EmId select f).Count() > 0 ? true : false,
                                    CheckFinger = (from fg in _dbContext.TEmployeeFingers where em.EmId == fg.EmId select fg).Count() > 0 ? true : false,
                                }).ToListAsync()).AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.EmName.ToLower().Contains(keyword.ToLower()));
            return query;
        }

        public IEnumerable<AccountEmployeeMapping> GetFromTimeToTimeByID(int Acc_Id)
        {
            var query = from acc in _dbContext.TAccounts
                        join em in _dbContext.TEmployees on acc.EmId equals em.EmId
                        join shift in _dbContext.PShifts on acc.ShiftId equals shift.ShiftId
                        where acc.AccId == Acc_Id
                        select new AccountEmployeeMapping
                        {
                            FromTime = shift.FromTime,
                            ToTime = acc.Shift.ToTime
                        };
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
