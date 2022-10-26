using RestSharp;

namespace KioskManagement.WebApi.Services
{
    public class Worker : BackgroundService
    {
        protected readonly ILogger<Worker> _logger;
        IConfiguration _config;

        public Worker(ILogger<Worker> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    try
            //    {
            //        _logger.LogInformation("Get log: " + DateTime.Now.ToString());
            //        string url = _config.GetConnectionString("UrlApiDevice");
            //        url += "api/kioskmanagement/idemiadevice/synclog";
            //        RestClient client = new RestClient();
            //        RestRequest request = new RestRequest(url, Method.POST);
            //        request.AddHeader("Content-Type", "Application/json");
            //        IRestResponse response = client.Execute<IRestResponse>(request);
            //        int time = Int32.Parse(_config.GetConnectionString("TimeGetLog"));
            //        await Task.Delay(time, stoppingToken);
            //    }
            //    catch (Exception ex)
            //    {
            //        _logger.LogError(ex.Message);
            //    }
            //}
        }

    }
}
