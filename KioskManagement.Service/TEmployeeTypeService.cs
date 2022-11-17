using KioskManagement.Data.Repositories;
using KioskManagement.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskManagement.Service
{
    public interface ITEmployeeTypeService
    {
        Task<IEnumerable<TEmployeeType>> GetAll();

        Task<TEmployeeType> GetById(int Id);
    }
    public class TEmployeeTypeService : ITEmployeeTypeService
    {
        private readonly ITEmployeeTypeRepository _iTEmployeeTypeRepository;

        public TEmployeeTypeService(ITEmployeeTypeRepository iTEmployeeTypeRepository)
        {
            _iTEmployeeTypeRepository = iTEmployeeTypeRepository;
        }
        public async Task<IEnumerable<TEmployeeType>> GetAll()
        {
            return await _iTEmployeeTypeRepository.GetAllAsync();
        }

        public async Task<TEmployeeType> GetById(int Id)
        {
            return await _iTEmployeeTypeRepository.GetByIdAsync(Id);
        }
    }
}
