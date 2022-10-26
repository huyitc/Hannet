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
    public class TDepartmentController : ControllerBase
    {
        #region Intialize
        private readonly IMapper _mapper;
        private ILogger<TDepartmentController> _logger;
        private readonly ITDepartmentService _departmentService;

        public TDepartmentController(IMapper mapper, ILogger<TDepartmentController> logger, ITDepartmentService departmentService)
        {
            _mapper = mapper;
            _logger = logger;
            _departmentService = departmentService;
        }
        #endregion Intialize

        #region Properties
        /// <summary>
        /// Get danh sách phòng ban
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        [Authorize(Roles = "ViewDepartment")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TDepartment/getall", "GET");
                var model = await _departmentService.GetAll();
                var mapping = _mapper.Map<IEnumerable<TDepartment>, IEnumerable<TDepartmentViewModel>>(model.OrderByDescending(x => x.DepName));
                return Ok(mapping);
            }
            catch (Exception dex)
            {
                return BadRequest(dex);
            }
        }

        /// <summary>
        /// Get căn phòng ban phân trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet("GetAllByPaging")]
        [Authorize(Roles = "ViewDepartment")]
        public async Task<IActionResult> GetAllByPaging(int page = 0, int pageSize = 100, string? keyword = null)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TDepartment/getallbypaging", "GET");
                var model = await _departmentService.GetAll(keyword);
                int totalRow = 0;
                var data = model.OrderByDescending(x => x.DepId).Skip(page * pageSize).Take(pageSize);
                var mapping = _mapper.Map<IEnumerable<TDepartment>, IEnumerable<TDepartmentViewModel>>(data);

                totalRow = model.Count();

                var paging = new PaginationSet<TDepartmentViewModel>()
                {
                    Items = mapping,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                return Ok(paging);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get thông tin phòng ban theo id
        /// </summary>
        /// <param name="id">Id căn hộ</param>
        /// <returns></returns>
        [HttpGet("getbyid/{id}")]
        [Authorize(Roles = "ViewDepartment")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TDepartment/getbyid", "GET");
                var model = await _departmentService.GetById(id);
                var mapping = _mapper.Map<TDepartment, TDepartmentViewModel>(model);
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Thêm mới phòng ban
        /// </summary>
        /// <param name="TDepartmentViewModel"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = "CreateDepartment")]
        public async Task<IActionResult> Create(TDepartmentViewModel department)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TDepartment/create", "POST");
                var de = _mapper.Map<TDepartmentViewModel, TDepartment>(department);
                try
                {
                    await _departmentService.Add(de);
                    return CreatedAtAction(nameof(Create), new { id = de.DepId }, de);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        ///<summary></summary>
        ///Chỉnh sửa phòng ban
        ///<returns></returns>

        [HttpPut("Update")]
        [Authorize(Roles = "EditDepartment")]
        public async Task<IActionResult> Update(TDepartmentViewModel department)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TDepartment/update", "PUT");
                var mapping = _mapper.Map<TDepartmentViewModel, TDepartment>(department);
                try
                {
                    await _departmentService.Update(mapping);
                    return CreatedAtAction(nameof(Update), new { id = mapping.DepId }, mapping);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        ///<summary></summary>
        ///Xóa phòng ban
        ///<returns></returns>
        [HttpDelete("delete")]
        [Authorize(Roles = "DeleteDepartment")]
        public async Task Delete(int id)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TDepartment/delete", "DELETE");
                await _departmentService.Delete(id);
            }
            catch (Exception ex)
            {
                BadRequest(ex);
            }
        }

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="checkedList">list id nhóm cần xóa</param>
        /// <returns></returns>
        [HttpDelete("deletemulti")]
        [Authorize(Roles = "DeleteDepartment")]
        public async Task<IActionResult> DeleteMulti(string checkedList)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TDepartment/deletemulti", "DELETE");
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
                    List<int> result = new List<int>();
                    var listItem = JsonConvert.DeserializeObject<List<int>>(checkedList);
                    foreach (var item in listItem)
                    {
                        try
                        {
                            await _departmentService.Delete(item);
                            countSuccess++;
                        }
                        catch (Exception)
                        {
                            countError++;
                        }
                    }
                    result.Add(countSuccess);
                    result.Add(countError);

                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        #endregion Properties
    }
}
