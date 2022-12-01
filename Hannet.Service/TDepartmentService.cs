using Hannet.Common.Exceptions;
using Hannet.Data.Repositories;
using Hannet.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Service
{
    public interface ITDepartmentService
    {
        Task<IQueryable<TDepartment>> GetAll();
        Task<IQueryable<TDepartment>> GetAll(string keyword);
        Task<TDepartment> GetById(int id);
        Task<TDepartment> Add(TDepartment department);
        Task<TDepartment> Update(TDepartment department);
        Task<TDepartment> Delete(int id);
    }
    public class TDepartmentService: ITDepartmentService
    {
        private readonly ITDepartmentRepository _departmentRepository;
        public TDepartmentService(ITDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<TDepartment> Add(TDepartment department)
        {
            if (await _departmentRepository.CheckContainsAsync(x => x.DepName == department.DepName))
            {
                throw new NameDuplicatedException("Tên phòng ban đã tồn tại!");
            }
            return await _departmentRepository.AddASync(department);
        }

        public async Task<TDepartment> Delete(int id)
        {
            return await _departmentRepository.DeleteAsync(id);
        }

        public async Task<IQueryable<TDepartment>> GetAll()
        {
            return await _departmentRepository.GetAllAsync(x => x.DepStatus == true);
        }

        public async Task<IQueryable<TDepartment>> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return await _departmentRepository.GetAllAsync(x => x.DepName.ToLower().Contains(keyword.ToLower()) || x.DepDescription.ToLower().Contains(keyword.ToLower()));
            }
            return await _departmentRepository.GetAllAsync();
        }

        public async Task<TDepartment> GetById(int id)
        {
            return await _departmentRepository.GetByIdAsync(id);
        }

        public async Task<TDepartment> Update(TDepartment department)
        {
            if (await _departmentRepository.CheckContainsAsync(x => x.DepName == department.DepName && x.DepId != department.DepId))
            {
                throw new NameDuplicatedException("Tên phòng ban đã tồn tại!");
            }
            return await _departmentRepository.UpdateASync(department);
        }

    }

}
