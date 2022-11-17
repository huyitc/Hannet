using AutoMapper;
using KioskManagement.Common.Ultilities;
using KioskManagement.Model.MappingModels;
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
    public class TDeviceController : ControllerBase
    {
        private readonly ITDeviceService _tDeviceService;
        private readonly IMapper _mapper;
        private ILogger<TDeviceController> _logger;
        public TDeviceController(ITDeviceService tDeviceService, IMapper mapper, ILogger<TDeviceController> logger)
        {
            _tDeviceService = tDeviceService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Thêm mới thiết bị
        /// </summary>
        /// <param name="tDeviceViewModel"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = "CreateTDevice")]
        public async Task<IActionResult> Create(TDeviceViewModel tDeviceViewModel)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TDevice/create", "POST");
                var de = _mapper.Map<TDeviceViewModel, TDevice>(tDeviceViewModel);
                try
                {
                    await _tDeviceService.Add(de);
                    return CreatedAtAction(nameof(Create), new { id = de.DevId }, de);
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
        /// Chỉnh sửa thiết bị
        /// </summary>
        /// <param name="tDeviceViewModel"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [Authorize(Roles = "UpdateTDevice")]
        public async Task<IActionResult> Update(TDeviceViewModel tDeviceViewModel)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/Device/update", "PUT");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var bod = new DeviceHannet
                {
                    deviceID = tDeviceViewModel.DeviceId,
                    deviceName = tDeviceViewModel.DevName,
                };
                var settings = new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Populate };

                var res = await Lib.MethodPostAsyncHanet("https://partner.hanet.ai/device/updateDevice", bod);

                var result = await _tDeviceService.Update(_mapper.Map<TDeviceViewModel, TDevice>(tDeviceViewModel));
                var responseData = _mapper.Map<TDevice, TDeviceViewModel>(result);
                return CreatedAtAction(nameof(Update), responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Xem danh sách thiết bị
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        [Authorize(Roles = "ViewTDevice")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TDevice/getall", "GET");
                var result = await _tDeviceService.GetAll();
                var responseData = _mapper.Map<IEnumerable<TDevice>, IEnumerable<TDeviceViewModel>>(result);
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Xem danh sách thiết bị
        /// </summary>
        /// <returns></returns>
        [HttpGet("getdevicemorpho")]
        [Authorize(Roles = "ViewTDevice")]
        public async Task<IActionResult> Getdevicemorpho()
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TDevice/getall", "GET");
                var result = await _tDeviceService.GetAll();
                var responseData = _mapper.Map<IEnumerable<TDevice>, IEnumerable<TDeviceViewModel>>(result);
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Xem danh sách thiết bị phân trang
        /// </summary>
        /// <param name="page">Trang thứ</param>
        /// <param name="pageSize">Số bản ghi hiển thị trong 1 trang</param>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <returns></returns>
        [HttpGet("getlistpaging")]
        [Authorize(Roles = "ViewTDevice")]
        public async Task<IActionResult> GetListPaging(int page = 0, int pageSize = 10, string? keyword = null)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TDevice/getlistpaging", "GET");
                var model = await _tDeviceService.GetListPaging(keyword);
                int totalRow = 0;
                totalRow = model.Count();
                var result = model.OrderByDescending(x => x.DevId).Skip(page * pageSize).Take(pageSize);
                var responseData = new PaginationSet<TDeviceMapping>()
                {
                    Items = result,
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
        /// Get thông tin thiết bị theo id
        /// </summary>
        /// <param name="id">id thiết bị</param>
        /// <returns></returns>
        [HttpGet("getbyid/{id}")]
        [Authorize(Roles = "ViewTDevice")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TDevice/getbyid/{id}", "GET");
                var result = await _tDeviceService.GetById(id);
                var responseData = _mapper.Map<TDevice, TDeviceViewModel>(result);
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
        /// <param name="id">id thiết bị</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "DeleteTDevice")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TDevice/delete/{id}", "DELETE");
            try
            {
                var aZone = await _tDeviceService.Delete(id);
                var responseData = _mapper.Map<TDevice, TDeviceViewModel>(aZone);
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
        /// <param name="checkedList">list thiết bị cần xóa</param>
        /// <returns></returns>
        [HttpDelete("deletemulti")]
        [Authorize(Roles = "DeleteTDevice")]
        public async Task<IActionResult> DeleteMulti(string checkedList)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TDevice/deletemulti", "DELETE");
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
                            await _tDeviceService.Delete(item);
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
