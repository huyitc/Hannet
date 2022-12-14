using AutoMapper;
using Hannet.Model.ViewModels;
using Hannet.Common.Ultilities;
using Hannet.Model.MappingModels;
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
    public class TEmployeeController : ControllerBase
    {
        #region Intialize
        private readonly ITEmployeeService _TEmployeeService;
        private readonly IMapper _mapper;
        private ILogger<TEmployeeController> _logger;

        public TEmployeeController(ITEmployeeService tEmployeeService, IMapper mapper, ILogger<TEmployeeController> logger)
        {
            _TEmployeeService = tEmployeeService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion Intialize

        #region Properties
        /// <summary>
        /// Danh sách employee phân trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet("getAllByPaging")]
        [Authorize(Roles = "ViewTEmployee")]
        public async Task<IActionResult> GetAllByPaging(int page = 0, int pageSize = 10, string? keyword = null)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TEmployee/getAllByPaging", "GET");
                var result = await _TEmployeeService.GetAllByMapping(keyword);
                int totalRow = 0;
                totalRow = result.Count();
                var paging = result.OrderByDescending(x => x.EmId).Skip(page * pageSize).Take(pageSize);
                var mapping = _mapper.Map<IEnumerable<TEmployeeMapping>>(paging);
                var response = new PaginationSet<TEmployeeMapping>()
                {
                    Items = mapping,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Danh sách employee
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet("getAllByMaping")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllByMaping(string? keyword = null)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TEmployee/getAllByMaping", "GET");
                var result = await _TEmployeeService.GetAllByMapping(keyword);
                var mapping = _mapper.Map<IEnumerable<TEmployeeMapping>>(result);
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("getAllNoParam")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllNoParam()
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TEmployee/getAllNoParam", "GET");
                var result = await _TEmployeeService.GetAllNoParam();
                var mapping = _mapper.Map<IEnumerable<TEmployee>, IEnumerable<TEmployeeMapping>>(result);
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("getAllEmployee")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllEmployee()
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TEmployee/getAllEmployee", "GET");
                var result = await _TEmployeeService.GetAllEmployee();
                var mapping = _mapper.Map<IEnumerable<TEmployee>, IEnumerable<TEmployeeMapping>>(result);
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("getById/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TEmployee/getById/{id}", "GET");
                var result = await _TEmployeeService.GetById(id);
                var mapping = _mapper.Map<TEmployeeViewModel>(result);
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Thêm mới nhân viên
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = "CreateTEmployee")]
        public async Task<IActionResult> Create(TEmployeeViewModel employee)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TEmployee/create", "POST");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                    var bodE = new EmployeeHanet
                    {
                        name = employee.EmName,
                        file = employee.EmImage,
                        aliasID = employee.EmCode,
                        placeID = employee.PlaceId.ToString(),
                        title = employee.Description,
                        type = employee.EmTypeId,
                    };
                var settingsE = new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Populate };
                var resE = await Lib.MethodPostFile("https://partner.hanet.ai/person/register", bodE);
                TEmployee em = _mapper.Map<TEmployee>(employee);
                var result = await _TEmployeeService.Add(em);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        

        /// <summary>
        /// Chỉnh sửa employee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [Authorize(Roles = "UpdateTEmployee")]
        public async Task<IActionResult> Update(TEmployeeViewModel employee)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/employee/update", "PUT");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var bod = new EmployeeHanet
                {
                    aliasID = employee.EmCode,
                    placeID = employee.PlaceId.ToString(),
                    name = employee.EmName,
                    title = employee.Description
                };
                var settings = new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Populate };

                var res = await Lib.MethodPostAsyncHanet("https://partner.hanet.ai/person/updateInfo", bod);
                var result = await _TEmployeeService.Update(_mapper.Map<TEmployeeViewModel, TEmployee>(employee));
                var responseData = _mapper.Map<TEmployee, TEmployeeViewModel>(result);
                return CreatedAtAction(nameof(Update), responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        /// <summary>
        /// Chỉnh sửa employee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPut("updateFaceImage")]
        [Authorize(Roles = "UpdateTEmployee")]
        public async Task<IActionResult> UpdateFaceImage(TEmployeeViewModel employee)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/employee/update", "PUT");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var bode = new UpdateFaceImage
                {
                    file = employee.EmImage,
                    aliasID = employee.EmCode,
                    placeID = employee.PlaceId.ToString(),
                };
                var settingsE = new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Populate };
                var resE = await Lib.MethodUpdateFile("https://partner.hanet.ai/person/updateByFaceImage",bode);

                var result = await _TEmployeeService.Update(_mapper.Map<TEmployeeViewModel, TEmployee>(employee));
                var responseData = _mapper.Map<TEmployee, TEmployeeViewModel>(result);
                return CreatedAtAction(nameof(Update), responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkedList"></param>
        /// <returns></returns>
        [HttpDelete("DeleteMuitiple")]
        [Authorize(Roles = "LockTEmployee")]
        public async Task<IActionResult> DeleteMulti(string checkedList)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TEmployee/deletemulti", "DELETE");
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
                    var listItem = JsonConvert.DeserializeObject<List<DeleteEmployeeHannet>>(checkedList);
                    foreach (var item in listItem)
                    {
                        try
                        {
                            var res = await Lib.MethodPostAsyncHanet("https://partner.hanet.ai/person/remove", new { aliasID = item.aliasID });
                            await _TEmployeeService.Delete(item.emId);
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

        [HttpGet("getEmployeeType")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTypeEmployee()
        {
            try
            {
                var result = await _TEmployeeService.GetTypeEmployee();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("getAllEmployeeByDepartment")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllEmployeeByDepartment(string strDepId)
        {
            try
            {
                List<int> depIds = JsonConvert.DeserializeObject<List<int>>(strDepId);
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/TEmployee/getAllEmployeeByDepartment", "GET");
                var result = await _TEmployeeService.GetEmployeesByDepartment(depIds);
                var mapping = _mapper.Map<IEnumerable<EmployeeDepMapping>>(result);
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion
    }
}
