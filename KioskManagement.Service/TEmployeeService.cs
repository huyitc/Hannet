using KioskManagement.Common;
using KioskManagement.Common.Exceptions;
using KioskManagement.Data;
using KioskManagement.Data.Repositories;
using KioskManagement.Model.MappingModels;
using KioskManagement.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskManagement.Service
{
    public interface ITEmployeeService
    {
        Task<IQueryable<TEmployee>> GetAllEmployee();
        Task<IQueryable<TEmployee>> GetAllNoParam();
        Task<TEmployee> GetById(int EmId);
        Task<IQueryable<TEmployeeMapping>> GetAllByMapping(string keyword);
        Task<TEmployee> Add(TEmployee employee);
        Task<TEmployee> Update(TEmployee employee);
        Task<TEmployee> Delete(int id);
        Task<IEnumerable<TEmployeeTypeMapping>> GetTypeEmployee();
        Task<IQueryable<TEmployee>> GetEnableTimeCheck();
        Task<IQueryable<EmployeeDepMapping>> GetEmployeesByDepartment(List<int> depIds);
    }
    public class TEmployeeService : ITEmployeeService
    {
        private readonly ITEmployeeRepository _tEmployeeRepository;
        private AIOAcessContolDbContext _dbContext;

        public TEmployeeService(ITEmployeeRepository tEmployeeRepository, AIOAcessContolDbContext dbContext)
        {
            _tEmployeeRepository = tEmployeeRepository;
            _dbContext = dbContext;
        }

        public async Task<TEmployee> GetById(int EmId)
        {
            return await _tEmployeeRepository.GetByIdAsync(EmId);
        }

        public async Task<IQueryable<TEmployeeMapping>> GetAllByMapping(string keyword)
        {
            return await _tEmployeeRepository.GetAllByMapping(keyword);
        }

        public async Task<IQueryable<TEmployee>> GetAllEmployee()
        {
            return await _tEmployeeRepository.GetAllAsync(x => x.EmTypeId == 1 && x.EmStatus == true);
        }

        public async Task<TEmployee> Add(TEmployee employee)
        {
            if (await _tEmployeeRepository.CheckContainsAsync(x => x.EmStatus == true && x.EmCode == employee.EmCode))
                throw new NameDuplicatedException("Mã nhân viên đã tồn tại!");
            return await _tEmployeeRepository.AddASync(employee);
        }

        public async Task<TEmployee> Update(TEmployee employee)
        {
            if (await _tEmployeeRepository.CheckContainsAsync(x => x.EmStatus == true && x.EmCode == employee.EmCode && x.EmId != employee.EmId))
                throw new NameDuplicatedException("Mã nhân viên đã tồn tại!");
            return await _tEmployeeRepository.UpdateASync(employee);
        }

        public async Task<TEmployee> Delete(int id)
        {
            return await _tEmployeeRepository.DeleteAsync(id);
        }

        public async Task<IQueryable<TEmployee>> GetAllNoParam()
        {
            return await _tEmployeeRepository.GetAllAsync(x => x.EmStatus == true);
        }

        public async Task<IEnumerable<TEmployeeTypeMapping>> GetTypeEmployee()
        {
            return await _tEmployeeRepository.GetTypeEmployee();
        }

        public async Task<IQueryable<TEmployee>> GetEnableTimeCheck()
        {
            return await _tEmployeeRepository.GetAllAsync(x => x.EmTypeId == AstecConstant.emTypeIDEmployee && x.EmStatus == true);
        }

        public async Task<IQueryable<EmployeeDepMapping>> GetEmployeesByDepartment(List<int> depIds)
        {
            return await _tEmployeeRepository.GetEmployeesByDepartment(depIds);
        }
    }
}
