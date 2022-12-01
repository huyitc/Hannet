using AutoMapper;
using Hannet.Model.Models;
using Hannet.Model.ViewModels;
using Hannet.Service;
using Hannet.WebApi.Infrastructure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Hannet.WebApi.Controllers
{
    [Route("hannet/api/[controller]")]
    [ApiController]
    [Authorize]
    public class AScheduleDeviceDetailController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ILogger<AScheduleDeviceDetailController> _logger;
        private readonly IAScheduleDeviceDetailService _iAScheduleDeviceDetailService;

        public AScheduleDeviceDetailController(IMapper mapper, ILogger<AScheduleDeviceDetailController> logger, IAScheduleDeviceDetailService iAScheduleDeviceDetailService)
        {
            _mapper = mapper;
            _logger = logger;
            _iAScheduleDeviceDetailService = iAScheduleDeviceDetailService;
        }

        /// <summary>
        /// Thêm mới lịch truy cập
        /// </summary>
        /// <param name="aScheduleDeviceDetailViewModel"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = "CreateAScheduleDeviceDetail")]
        public async Task<IActionResult> Create(AScheduleDeviceDetailViewModel aScheduleDeviceDetailViewModel)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/AScheduleDeviceDetail/create", "POST");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _iAScheduleDeviceDetailService.Add(_mapper.Map<AScheduleDeviceDetailViewModel, AScheduleDeviceDetail>(aScheduleDeviceDetailViewModel));
                var responseData = _mapper.Map<AScheduleDeviceDetail, AScheduleDeviceDetailViewModel>(result);
                return CreatedAtAction(nameof(Create), responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Chỉnh sửa lịch truy cập
        /// </summary>
        /// <param name="aScheduleDeviceDetailViewModel"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [Authorize(Roles = "UpdateAScheduleDeviceDetail")]
        public async Task<IActionResult> Update(AScheduleDeviceDetailViewModel aScheduleDeviceDetailViewModel)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/AScheduleDeviceDetail/update", "PUT");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _iAScheduleDeviceDetailService.Update(_mapper.Map<AScheduleDeviceDetailViewModel, AScheduleDeviceDetail>(aScheduleDeviceDetailViewModel));
                var responseData = _mapper.Map<AScheduleDeviceDetail, AScheduleDeviceDetailViewModel>(result);
                return CreatedAtAction(nameof(Update), responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Xem danh sách lịch truy cập
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        [Authorize(Roles = "ViewAScheduleDeviceDetail")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/AScheduleDeviceDetail/getall", "GET");
                var result = await _iAScheduleDeviceDetailService.GetAll();
                var responseData = _mapper.Map<IEnumerable<AScheduleDeviceDetail>, IEnumerable<AScheduleDeviceDetailViewModel>>(result);
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Xem danh sách lịch truy cập phân trang
        /// </summary>
        /// <param name="page">Trang thứ</param>
        /// <param name="pageSize">Số bản ghi hiển thị trong 1 trang</param>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <returns></returns>
        [HttpGet("getlistpaging")]
        [Authorize(Roles = "ViewAScheduleDeviceDetail")]
        public async Task<IActionResult> GetListPaging(int page = 0, int pageSize = 10, string? keyword = null)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/AScheduleDeviceDetail/getlistpaging", "GET");
                var model = await _iAScheduleDeviceDetailService.GetAll(keyword);
                int totalRow = 0;
                totalRow = model.Count();
                var result = model.Skip(page * pageSize).Take(pageSize);
                var mapping = _mapper.Map<IEnumerable<AScheduleDeviceDetail>, IEnumerable<AScheduleDeviceDetailViewModel>>(result);
                var responseData = new PaginationSet<AScheduleDeviceDetailViewModel>()
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
        /// Get thông tin lịch truy cập theo id
        /// </summary>
        /// <param name="id">id lịch truy cập</param>
        /// <returns></returns>
        [HttpGet("getbyid/{id}")]
        [Authorize(Roles = "ViewAScheduleDeviceDetail")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/AScheduleDeviceDetail/getbyid/{id}", "GET");
                var result = await _iAScheduleDeviceDetailService.GetById(id);
                var responseData = _mapper.Map<AScheduleDeviceDetail, AScheduleDeviceDetailViewModel>(result);
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Xóa 1 bản ghi
        /// </summary>
        /// <param name="id">id lịch truy cập</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "DeleteAScheduleDeviceDetail")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/AScheduleDeviceDetail/delete/{id}", "DELETE");
            try
            {
                var aZone = await _iAScheduleDeviceDetailService.Delete(id);
                var responseData = _mapper.Map<AScheduleDeviceDetail, AScheduleDeviceDetailViewModel>(aZone);
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
        /// <param name="checkedList">list id lịch truy cập cần xóa</param>
        /// <returns></returns>
        [HttpDelete("deletemulti")]
        [Authorize(Roles = "DeleteAScheduleDeviceDetail")]
        public async Task<IActionResult> DeleteMulti(string checkedList)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/AScheduleDeviceDetail/deletemulti", "DELETE");
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
                            await _iAScheduleDeviceDetailService.Delete(item);
                            countSuccess++;
                        }
                        catch (Exception)
                        {
                            countError++;
                        }
                    }
                    result.Add("Xóa thành công: " + countSuccess);
                    result.Add("Lỗi: " + countError);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
