using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMQTT_4_8_net
{
    public class TestMQTT
    {
        MqttClient mqttClient = null;
        bool isReconnect = true;

        public async Task CreateMQTTClientAsync()
        {
            if (mqttClient == null)
            {
                var factory = new MqttFactory();
                mqttClient = factory.CreateMqttClient() as MqttClient;

                mqttClient.ApplicationMessageReceived += MqttClient_ApplicationMessageReceived;
                mqttClient.Connected += MqttClient_Connected;
                mqttClient.Disconnected += MqttClient_DisconnectedAsync;
            }

            try
            {
                ////Create TCP based options using the builder.
                //var options1 = new MqttClientOptionsBuilder()
                //    .WithClientId("client001")
                //    .WithTcpServer("192.168.88.3")
                //    .WithCredentials("bud", "%spencer%")
                //    .WithTls()
                //    .WithCleanSession()
                //    .Build();

                //// Use TCP connection.
                //var options2 = new MqttClientOptionsBuilder()
                //    .WithTcpServer("192.168.88.3", 8222) // Port is optional
                //    .Build();

                //// Use secure TCP connection.
                //var options3 = new MqttClientOptionsBuilder()
                //    .WithTcpServer("192.168.88.3")
                //    .WithTls()
                //    .Build();

                //Create TCP based options using the builder.
                var options = new MqttClientOptionsBuilder()
                    .WithClientId("claa_001")
                    .WithTcpServer("10.204.138.7", 1883)
                    .WithCredentials("user_001", "pass_001")
                    //.WithTls()//服务器端没有启用加密协议，这里用tls的会提示协议异常
                    .WithCleanSession()
                    .Build();

                //// For .NET Framwork & netstandard apps:
                //MqttTcpChannel.CustomCertificateValidationCallback = (x509Certificate, x509Chain, sslPolicyErrors, mqttClientTcpOptions) =>
                //{
                //    if (mqttClientTcpOptions.Server == "server_with_revoked_cert")
                //    {
                //        return true;
                //    }

                //    return false;
                //};

                //2.4.0版本的
                //var options0 = new MqttClientTcpOptions
                //{
                //    Server = "127.0.0.1",
                //    ClientId = Guid.NewGuid().ToString().Substring(0, 5),
                //    UserName = "u001",
                //    Password = "p001",
                //    CleanSession = true
                //};

                await mqttClient.ConnectAsync(options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"连接到MQTT服务器失败！" + Environment.NewLine + ex.Message + Environment.NewLine);
            }
        }

        private void MqttClient_Connected(object sender, EventArgs e)
        {
            Console.Clear();
            Console.WriteLine("已连接到MQTT服务器！" + Environment.NewLine);
        }

        private void MqttClient_DisconnectedAsync(object sender, EventArgs e)
        {
            Console.Clear();
            Console.WriteLine("已断开MQTT连接！" + Environment.NewLine);
        }

        private void MqttClient_ApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            Console.WriteLine($">> {"### RECEIVED APPLICATION MESSAGE ###"}{Environment.NewLine}");
            Console.WriteLine($">> ClientId = {e.ClientId}{Environment.NewLine}");
            Console.WriteLine($">> Topic = {e.ApplicationMessage.Topic}{Environment.NewLine}");
            Console.WriteLine($">> Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}{Environment.NewLine}");
            Console.WriteLine($">> QoS = {e.ApplicationMessage.QualityOfServiceLevel}{Environment.NewLine}");
            Console.WriteLine($">> Retain = {e.ApplicationMessage.Retain}{Environment.NewLine}");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            TestMQTT test = new TestMQTT();
            test.CreateMQTTClientAsync();

            while(true)
            {
                System.Threading.Thread.Sleep(60 * 1000);
                Console.WriteLine("in while sleep 60 m");
            }

        }
    }
}
