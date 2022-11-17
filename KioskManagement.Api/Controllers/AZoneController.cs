using AutoMapper;
using KioskManagement.Common.Ultilities;
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
    public class AZoneController : ControllerBase
    {
        private readonly IAZoneService _aZoneService;
        private readonly IMapper _mapper;
        private ILogger<AZoneController> _logger;
        public AZoneController(IAZoneService aZoneService, IMapper mapper, ILogger<AZoneController> logger)
        {
            _aZoneService = aZoneService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Thêm mới khu vực
        /// </summary>
        /// <param name="aZoneViewModel"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = "CreateAZone")]
        public async Task<IActionResult> Create(AZoneViewModel aZoneViewModel)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/AZone/create", "POST");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var bod = new AZoneHanet
                {
                    name = aZoneViewModel.ZonName,
                    address = aZoneViewModel.ZonDescription
                };
                var settings = new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Populate };

                var res = await Lib.MethodPostAsyncHanet("https://partner.hanet.ai/place/addPlace", bod);
                Place? resultInfo = JsonConvert.DeserializeObject<Place>(res.Content);
                aZoneViewModel.PlaceId = resultInfo!.data.id;
                var result = await _aZoneService.Add(_mapper.Map<AZoneViewModel, AZone>(aZoneViewModel));
                var responseData = _mapper.Map<AZone, AZoneViewModel>(result);
                return CreatedAtAction(nameof(Create), responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Chỉnh sửa khu vực
        /// </summary>
        /// <param name="aZoneViewModel"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [Authorize(Roles = "UpdateAZone")]
        public async Task<IActionResult> Update(AZoneViewModel aZoneViewModel)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/AZone/update", "PUT");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var bod = new AZoneHanet
                {
                    placeID = aZoneViewModel.PlaceId,
                    name = aZoneViewModel.ZonName,
                    address = aZoneViewModel.ZonDescription
                };
                var settings = new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Populate };

                var res = await Lib.MethodPostAsyncHanet("https://partner.hanet.ai/place/updatePlace", bod);
                var result = await _aZoneService.Update(_mapper.Map<AZoneViewModel, AZone>(aZoneViewModel));
                var responseData = _mapper.Map<AZone, AZoneViewModel>(result);
                return CreatedAtAction(nameof(Update), responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Xem danh sách khu vực
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        [Authorize(Roles = "ViewAZone")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/AZone/getall", "GET");
                var result = await _aZoneService.GetAll();
                var responseData = _mapper.Map<IEnumerable<AZone>, IEnumerable<AZoneViewModel>>(result);
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Xem danh sách khu vực phân trang
        /// </summary>
        /// <param name="page">Trang thứ</param>
        /// <param name="pageSize">Số bản ghi hiển thị trong 1 trang</param>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <returns></returns>
        [HttpGet("getlistpaging")]
        [Authorize(Roles = "ViewAZone")]
        public async Task<IActionResult> GetListPaging(int page = 0, int pageSize = 10, string? keyword = null)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/AZone/getlistpaging", "GET");
                var model = await _aZoneService.GetAll(keyword);
                int totalRow = 0;
                totalRow = model.Count();
                var result = model.OrderByDescending(x => x.ZonId).Skip(page * pageSize).Take(pageSize);
                var mapping = _mapper.Map<IEnumerable<AZone>, IEnumerable<AZoneViewModel>>(result);
                var responseData = new PaginationSet<AZoneViewModel>()
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
        /// Get thông tin khu vực theo id
        /// </summary>
        /// <param name="id">id khu vực</param>
        /// <returns></returns>
        [HttpGet("getbyid/{id}")]
        [Authorize(Roles = "ViewAZone")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/AZone/getbyid/{id}", "GET");
                var result = await _aZoneService.GetById(id);
                var responseData = _mapper.Map<AZone, AZoneViewModel>(result);
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
        /// <param name="id">id khu vực</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "DeleteAZone")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/AZone/delete/{id}", "DELETE");
            try
            {
                var res = await Lib.MethodPostAsyncHanet("https://partner.hanet.ai/place/removePlace", new { placeId = id });
                var aZone = await _aZoneService.Delete(id);
                var responseData = _mapper.Map<AZone, AZoneViewModel>(aZone);
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
        /// <param name="checkedList">list id khu vực cần xóa</param>
        /// <returns></returns>
        [HttpDelete("deletemulti")]
        [Authorize(Roles = "DeleteAZone")]
        public async Task<IActionResult> DeleteMulti(string checkedList)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/AZone/deletemulti", "DELETE");
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
                    var listItem = JsonConvert.DeserializeObject<List<DeleteZone>>(checkedList);
                    foreach (var item in listItem)
                    {
                        try
                        {
                            var res = await Lib.MethodPostAsyncHanet("https://partner.hanet.ai/place/removePlace", new { placeID = item.placeId });
                            await _aZoneService.Delete(item.id);
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
