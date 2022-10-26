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
    public class TRegencyController : ControllerBase
    {
        #region Intialize
        private readonly ITRegencyService _tRegencyService;
        private readonly IMapper _mapper;
        private ILogger<TRegencyController> _logger;

        public TRegencyController(ILogger<TRegencyController> logger, ITRegencyService tRegencyService, IMapper mapper)
        {
            _tRegencyService = tRegencyService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region properties
        /// <summary>
        /// Xem danh sách chức vụ
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        [Authorize(Roles = "ViewTRegency")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TRegency/getall", "GET");
                var result = await _tRegencyService.GetAll();
                var responseData = _mapper.Map<IEnumerable<TRegency>, IEnumerable<TRegencyViewModel>>(result);
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get thông tin chức vụ theo id
        /// </summary>
        /// <param name="id">Id </param>
        /// <returns></returns>
        [HttpGet("getbyid/{id}")]
        [Authorize(Roles = "ViewTRegency")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TRegency/getbyid/{id}", "GET");
                var result = await _tRegencyService.GetById(id);
                var responseData = _mapper.Map<TRegency, TRegencyViewModel>(result);
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Xem danh sách chức vụ phân trang
        /// </summary>
        /// <param name="page">Trang thứ</param>
        /// <param name="pageSize">Số bản ghi hiển thị trong 1 trang</param>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <returns></returns>
        [HttpGet("getlistpaging")]
        [Authorize(Roles = "ViewTRegency")]
        public async Task<IActionResult> GetListPaging(int page = 0, int pageSize = 10, string? keyword = null)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TRegency/getlistpaging", "GET");
                var model = await _tRegencyService.GetAll(keyword);
                int totalRow = 0;
                totalRow = model.Count();
                var result = model.OrderByDescending(x => x.RegId).Skip(page * pageSize).Take(pageSize);
                var mapping = _mapper.Map<IEnumerable<TRegency>, IEnumerable<TRegencyViewModel>>(result);
                var responseData = new PaginationSet<TRegencyViewModel>()
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
        /// Thêm mới  chức vụ
        /// </summary>
        /// <param name="regencyViewModel"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = "CreateTRegency")]
        public async Task<IActionResult> Create(TRegencyViewModel regencyViewModel)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TDepartment/create", "POST");
                var de = _mapper.Map<TRegencyViewModel, TRegency>(regencyViewModel);
                try
                {
                    await _tRegencyService.Add(de);
                    return CreatedAtAction(nameof(Create), new { id = de.RegId }, de);
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

        /// <summary>
        /// Chỉnh sửa chức vụ
        /// </summary>
        /// <param name="regencyViewModel"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [Authorize(Roles = "UpdateTRegency")]
        public async Task<IActionResult> Update(TRegencyViewModel regencyViewModel)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TDepartment/update", "PUT");
                var mapping = _mapper.Map<TRegencyViewModel, TRegency>(regencyViewModel);
                try
                {
                    await _tRegencyService.Update(mapping);
                    return CreatedAtAction(nameof(Update), new { id = mapping.RegId }, mapping);
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

        /// <summary>
        /// Xóa 1 bản ghi
        /// </summary>
        /// <param name="id">id chức vụ</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "DeleteTRegency")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TRegency/delete/{id}", "DELETE");
            try
            {
                var tRegency = await _tRegencyService.Delete(id);
                var responseData = _mapper.Map<TRegency, TRegencyViewModel>(tRegency);
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
        /// <param name="checkedList">list id chức vụ cần xóa</param>
        /// <returns></returns>
        [HttpDelete("deletemulti")]
        [Authorize(Roles = "DeleteTRegency")]
        public async Task<IActionResult> DeleteMulti(string checkedList)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TRegency/deletemulti", "DELETE");
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
                            await _tRegencyService.Delete(item);
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

        #endregion
    }
}
