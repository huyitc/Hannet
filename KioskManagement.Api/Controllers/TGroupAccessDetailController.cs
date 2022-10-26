using AutoMapper;
using KioskManagement.Model.MappingModels;
using KioskManagement.Model.Models;
using KioskManagement.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KioskManagement.WebApi.Controllers
{
    [Route("api/kioskmanagement/[controller]")]
    [ApiController]
    [Authorize]
    public class TGroupAccessDetailController : ControllerBase
    {
        private readonly ITGroupAccessDetailService _iTGroupAccessDetailService;
        private readonly IMapper _mapper;
        private readonly ITEmployeeService _tEmployeeService;
        private ILogger<TGroupAccessDetailController> _logger;
        public TGroupAccessDetailController(ITGroupAccessDetailService iTGroupAccessDetailService,
            IMapper mapper, ILogger<TGroupAccessDetailController> logger, ITEmployeeService tEmployeeService)
        {
            _iTGroupAccessDetailService = iTGroupAccessDetailService;
            _mapper = mapper;
            _logger = logger;
            _tEmployeeService = tEmployeeService;
        }

        /// <summary>
        /// Xem danh sách phân nhóm thiết bị theo gaid và gadStatus = true
        /// </summary>
        /// <returns></returns>
        [HttpGet("getbygaid/{gaId}")]
        [Authorize(Roles = "ViewTGroupAccessDetail")]
        public async Task<IActionResult> GetByGaId(int gaId)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TGroupAccessDetail/getbygaid/{gaId}", "GET");
                var responseData = await _iTGroupAccessDetailService.GetDevIdsByGaIdAndHaveStatus(gaId);
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get tree AZone-TDevice
        /// </summary>
        /// <returns></returns>
        [HttpGet("gettreeazonetdevice")]
        [Authorize(Roles = "ViewTGroupAccessDetail")]
        public async Task<IActionResult> GetTreeAZoneTDevice()
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TGroupAccessDetail/gettreeazonetdevice", "GET");
                var responseData = await _iTGroupAccessDetailService.GetTreeAZoneTDevice();
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Lưu phân quyền từng tầng
        /// </summary>
        /// <param name="groupAcessDetailMapping"></param>
        /// <returns></returns>
        [HttpPut("settgrouaccessdetail")]
        [Authorize(Roles = "SetTGrouAccessDetail")]
        public async Task<IActionResult> SetTGAD(TGroupAcessDetailMapping groupAcessDetailMapping)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TGroupAccessDetail/settgrouaccessdetail", "POST");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                int gaId = groupAcessDetailMapping.GaId;
                // Cập nhật Employee thuộc phân nhóm EditStatus => true
                var tEmployees = await _iTGroupAccessDetailService.GetTEmployeeByGaIdAndHaveStatus(gaId);
                foreach (var itemTEmployee in tEmployees)
                {
                    itemTEmployee.EditStatus = true;
                    await _tEmployeeService.Update(itemTEmployee);
                }

                foreach (var devItem in groupAcessDetailMapping.DeviceItemMappings)
                {
                    var tgadDb = await _iTGroupAccessDetailService.GetByGaIdAndDevId(gaId, devItem.DevId);
                    // Những thiết bị đc selected
                    if (devItem.Selected)
                    {
                        // Check chưa có => Thêm
                        if (tgadDb == null)
                        {
                            TGroupAccessDetail tGroupAccessDetail = new TGroupAccessDetail();
                            tGroupAccessDetail.GaId = gaId;
                            tGroupAccessDetail.DevId = devItem.DevId;
                            tGroupAccessDetail.GadStatus = true;
                            await _iTGroupAccessDetailService.Add(tGroupAccessDetail);

                        }
                        else // Đã có => update status = true
                        {
                            tgadDb.GadStatus = true;
                            await _iTGroupAccessDetailService.Update(tgadDb);
                        }
                    }
                    // Những thiết bị k đc selected
                    else
                    {
                        if (tgadDb != null)
                        {
                            tgadDb.GadStatus = false;
                            await _iTGroupAccessDetailService.Update(tgadDb);
                        }
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
