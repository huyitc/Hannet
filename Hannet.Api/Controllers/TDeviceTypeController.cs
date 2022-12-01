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
    public class TDeviceTypeController : ControllerBase
    {
        #region Intialize

        private readonly ITDeviceTypeService _iTDeviceTypeService;
        private readonly IMapper _mapper;
        private ILogger<TDeviceTypeController> _logger;

        public TDeviceTypeController(ILogger<TDeviceTypeController> logger, ITDeviceTypeService tDeviceTypeService, IMapper mapper)
        {
            _iTDeviceTypeService = tDeviceTypeService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region properties

        /// <summary>
        /// Xem danh sách loại thiết bị
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        //[Authorize(Roles = "ViewTDeviceType")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TDeviceType/getall", "GET");
                var result = await _iTDeviceTypeService.GetAll();
                var responseData = _mapper.Map<IEnumerable<TDeviceType>, IEnumerable<TDeviceTypeViewModel>>(result);
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get thông tin loại thiết bị theo id
        /// </summary>
        /// <param name="id">Id loại thiết bị</param>
        /// <returns></returns>
        [HttpGet("getbyid/{id}")]
        [Authorize(Roles = "ViewTDeviceType")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TDeviceType/getbyid/{id}", "GET");
                var result = await _iTDeviceTypeService.GetDetail(id);
                var responseData = _mapper.Map<TDeviceType, TDeviceTypeViewModel>(result);
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Xem danh sách loại thiết bị phân trang
        /// </summary>
        /// <param name="page">Trang thứ</param>
        /// <param name="pageSize">Số bản ghi hiển thị trong 1 trang</param>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <returns></returns>
        [HttpGet("getlistpaging")]
        [Authorize(Roles = "ViewTDeviceType")]
        public async Task<IActionResult> GetListPaging(int page = 0, int pageSize = 10, string? keyword = null)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TDeviceType/getlistpaging", "GET");
                var model = await _iTDeviceTypeService.GetAll(keyword);
                int totalRow = 0;
                totalRow = model.Count();
                var result = model.OrderByDescending(x => x.DevTypeId).Skip(page * pageSize).Take(pageSize);
                var mapping = _mapper.Map<IEnumerable<TDeviceType>, IEnumerable<TDeviceTypeViewModel>>(result);
                var responseData = new PaginationSet<TDeviceTypeViewModel>()
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
        /// Thêm mới loại thiết bị
        /// </summary>
        /// <param name="deviceTypeViewModel"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = "CreateTDeviceType")]
        public async Task<IActionResult> Create(TDeviceTypeViewModel deviceTypeViewModel)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TDeviceType/create", "POST");
            if (ModelState.IsValid)
            {
                var de = _mapper.Map<TDeviceTypeViewModel, TDeviceType>(deviceTypeViewModel);
                try
                {
                    await _iTDeviceTypeService.Add(de);
                    return CreatedAtAction(nameof(Create), new { id = de.DevTypeId }, de);
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
        /// Chỉnh sửa  loại thiết bị
        /// </summary>
        /// <param name="deviceTypeViewModel"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [Authorize(Roles = "UpdateTDeviceType")]
        public async Task<IActionResult> Update(TDeviceTypeViewModel deviceTypeViewModel)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TDeviceType/update", "PUT");
            if (ModelState.IsValid)
            {
                var mapping = _mapper.Map<TDeviceTypeViewModel, TDeviceType>(deviceTypeViewModel);
                try
                {
                    await _iTDeviceTypeService.Update(mapping);
                    return CreatedAtAction(nameof(Update), new { id = mapping.DevTypeId }, mapping);
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
        /// <param name="id">id loại thiết bị</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "DeleteTDeviceType")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TDeviceType/delete/{id}", "DELETE");
            try
            {
                var tDeviceType = await _iTDeviceTypeService.Delete(id);
                var responseData = _mapper.Map<TDeviceType, TDeviceTypeViewModel>(tDeviceType);
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
        /// <param name="checkedList">list id loại thiết bị cần xóa</param>
        /// <returns></returns>
        [HttpDelete("deletemulti")]
        [Authorize(Roles = "DeleteTDeviceType")]
        public async Task<IActionResult> DeleteMulti(string checkedList)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TDeviceType/deletemulti", "DELETE");
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
                            await _iTDeviceTypeService.Delete(item);
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
