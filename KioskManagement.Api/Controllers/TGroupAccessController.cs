using AutoMapper;
using KioskManagement.Model.Models;
using KioskManagement.Model.ViewModels;
using KioskManagement.Service;
using KioskManagement.WebApi.Infrastructure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace KioskManagement.WebApi.Controllers
{
    [Route("api/kioskmanagement/[controller]")]
    [ApiController]
    [Authorize]
    public class TGroupAccessController : ControllerBase
    {
        #region Intialize
        private readonly ITGroupAccessService _iTGroupAccessService;
        private readonly ITEmployeeService _tEmployeeService;
        private readonly IMapper _mapper;
        private ILogger<TGroupAccessController> _logger;

        public TGroupAccessController(ILogger<TGroupAccessController> logger, ITGroupAccessService tGroupAccessService,
            ITEmployeeService tEmployeeService, IMapper mapper)
        {
            _iTGroupAccessService = tGroupAccessService;
            _mapper = mapper;
            _logger = logger;
            _tEmployeeService = tEmployeeService;
        }
        #endregion

        #region properties
        /// <summary>
        /// Xem danh sách nhóm thiết bị
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        [Authorize(Roles = "ViewTGroupAccess")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TGroupAccess/getall", "GET");
                var result = await _iTGroupAccessService.GetAll();
                var responseData = _mapper.Map<IEnumerable<TGroupAccess>, IEnumerable<TGroupAccessViewModel>>(result);
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get thông tin nhóm thiết bị theo id
        /// </summary>
        /// <param name="id">Id nhóm thiết bị</param>
        /// <returns></returns>
        [HttpGet("getbyid/{id}")]
        [Authorize(Roles = "ViewTGroupAccess")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TGroupAccess/getbyid/{id}", "GET");
                var result = await _iTGroupAccessService.GetById(id);
                var responseData = _mapper.Map<TGroupAccess, TGroupAccessViewModel>(result);
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Xem danh sách phân trang
        /// </summary>
        /// <param name="page">Trang thứ</param>
        /// <param name="pageSize">Số bản ghi hiển thị trong 1 trang</param>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <returns></returns>
        [HttpGet("getlistpaging")]
        [Authorize(Roles = "ViewTGroupAccess")]
        public async Task<IActionResult> GetListPaging(int page = 0, int pageSize = 10, string? keyword = null)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TGroupAccess/getlistpaging", "GET");
                var model = await _iTGroupAccessService.GetAll(keyword);
                int totalRow = 0;
                totalRow = model.Count();
                var result = model.OrderByDescending(x => x.GaId).Skip(page * pageSize).Take(pageSize);
                var mapping = _mapper.Map<IEnumerable<TGroupAccess>, IEnumerable<TGroupAccessViewModel>>(result);
                var responseData = new PaginationSet<TGroupAccessViewModel>()
                {
                    Items = mapping,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                return Ok(responseData);
            }
            catch (Exception dex)
            {
                return BadRequest(dex);
            }
        }

        /// <summary>
        /// Thêm mới nhóm thiết bị
        /// </summary>
        /// <param name="groupAccessViewModel"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = "CreateTGroupAccess")]
        public async Task<IActionResult> Create(TGroupAccessViewModel groupAccessViewModel)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TGroupAccess/create", "POST");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var tGroupAccess = _mapper.Map<TGroupAccessViewModel, TGroupAccess>(groupAccessViewModel);
                var result = await _iTGroupAccessService.Add(tGroupAccess);
                var responseData = _mapper.Map<TGroupAccess, TGroupAccessViewModel>(result);
                return CreatedAtAction(nameof(Create), responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Chỉnh sửa nhóm thiết bị
        /// </summary>
        /// <param name="groupAccessViewModel"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [Authorize(Roles = "UpdateTGroupAccess")]
        public async Task<IActionResult> Update(TGroupAccessViewModel groupAccessViewModel)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TGroupAccess/update", "PUT");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var tGroupAccess = _mapper.Map<TGroupAccessViewModel, TGroupAccess>(groupAccessViewModel);
                var result = await _iTGroupAccessService.Update(tGroupAccess);
                var empList = await _tEmployeeService.GetAllByGaId(result.GaId);
                foreach (var emp in empList)
                {
                    try
                    {
                        emp.EditStatus = true;
                        await _tEmployeeService.Update(emp);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                var responseData = _mapper.Map<TGroupAccess, TGroupAccessViewModel>(result);
                return CreatedAtAction(nameof(Update), responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Xóa 1 bản ghi
        /// </summary>
        /// <param name="id">id nhóm thiết bị</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "DeleteTGroupAccess")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TGroupAccess/delete/{id}", "DELETE");
            try
            {
                var tGroupAccess = await _iTGroupAccessService.Delete(id);
                var responseData = _mapper.Map<TGroupAccess, TGroupAccessViewModel>(tGroupAccess);
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="checkedList">list id nhóm thiết bị cần xóa</param>
        /// <returns></returns>
        [HttpDelete("deletemulti")]
        [Authorize(Roles = "DeleteTGroupAccess")]
        public async Task<IActionResult> DeleteMulti(string checkedList)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TGroupAccess/deletemulti", "DELETE");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                try
                {
                    int countSuccess = 0;
                    int countError = 0;
                    List<string> result = new List<string>();
                    var listItem = JsonConvert.DeserializeObject<List<int>>(checkedList);
                    foreach (var item in listItem)
                    {
                        try
                        {
                            await _iTGroupAccessService.Delete(item);
                            countSuccess++;
                        }
                        catch (Exception)
                        {
                            countError++;
                        }
                    }
                    result.Add("Xóa thành công: " + countSuccess);
                    result.Add("Lỗi" + countError);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        #endregion
    }
}
