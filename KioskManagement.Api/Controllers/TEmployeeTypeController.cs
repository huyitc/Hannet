using AutoMapper;
using KioskManagement.Model.Models;
using KioskManagement.Model.ViewModels;
using KioskManagement.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KioskManagement.WebApi.Controllers
{
    [Route("api/kioskmanagement/[controller]")]
    [ApiController]
    [Authorize]
    public class TEmployeeTypeController : ControllerBase
    {
        #region Intialize

        private readonly ITEmployeeTypeService _iTEmployeeTypeService;
        private readonly IMapper _mapper;
        private ILogger<TEmployeeTypeController> _logger;

        public TEmployeeTypeController(ITEmployeeTypeService iTEmployeeTypeService, IMapper mapper, ILogger<TEmployeeTypeController> logger)
        {
            _iTEmployeeTypeService = iTEmployeeTypeService;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion Intialize

        /// <summary>
        /// Xem danh sách loại nhân viên
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        //[Authorize(Roles = "	ViewTEmployeeType")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TEmployeeType/getall", "GET");
            try
            {
                var model = await _iTEmployeeTypeService.GetAll();
                var mapping = _mapper.Map<IEnumerable<TEmployeeType>, IEnumerable<TEmployeeTypeViewModel>>(model.OrderByDescending(x => x.EmTypeId));
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get thông tin loại nhân viên theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getbyid/{id}")]
        //[Authorize(Roles = "	ViewTEmployeeType")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TEmployeeType/getbyid/{id}", "GET");
            try
            {
                var model = await _iTEmployeeTypeService.GetById(id);
                var mapping = _mapper.Map<TEmployeeType, TEmployeeTypeViewModel>(model);
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
