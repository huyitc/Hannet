using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Newtonsoft.Json;

namespace Hannet.WebApi.Services
{
    public class MqttService
    {
        IMqttClient _mqttClient;
        IMqttClientOptions options;
        public MqttService()
        {
             options = new MqttClientOptionsBuilder()
                               .WithClientId(Guid.NewGuid().ToString())
                               .WithTcpServer("760462689e7f488db33ca33865aeacf1.s1.eu.hivemq.cloud", 8883)
                               .WithCredentials("phongnguyen", "Astec@2022")
                               .WithTls(new MqttClientOptionsBuilderTlsParameters()
                               {
                                   UseTls = true, // Is set by default to true, I guess...
                                    SslProtocol = System.Security.Authentication.SslProtocols.Tls12, // TLS downgrade
                                    AllowUntrustedCertificates = true, // Not sure if this is really needed...
                                    IgnoreCertificateChainErrors = true, // Not sure if this is really needed...
                                    IgnoreCertificateRevocationErrors = true, // Not sure if this is really needed...
                                    CertificateValidationHandler = (w) => true // Not sure if this is really needed...
                                })
                               .Build();

            _mqttClient = new MqttFactory().CreateMqttClient();
        }
        public async Task PublishDataAsync(String str)
        {
            await _mqttClient.ConnectAsync(options);
            string json = JsonConvert.SerializeObject(new { message = "Heyo :)", sent = DateTime.Now.ToString() });
            await _mqttClient.PublishAsync("dev.to/topic/json", json);
            await _mqttClient.DisconnectAsync();
        }
    }
}
