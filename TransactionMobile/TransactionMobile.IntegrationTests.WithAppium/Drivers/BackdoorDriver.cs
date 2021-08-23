namespace TransactionMobile.IntegrationTests.WithAppium.Drivers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mqtt;
    using System.Reactive.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Common;
    using IntegrationTestClients;
    using Newtonsoft.Json;

    public class BackdoorDriver
    {
        public BackdoorDriver()
        {
        }

        public async Task SetIntegrationModeOn()
        {
            await this.ExecuteBackdoor("SetIntegrationTestModeOn", "");
        }

        public async Task UpdateTestMerchant(Merchant merchant)
        {
            String merchantData = JsonConvert.SerializeObject(merchant);
            await this.ExecuteBackdoor("UpdateTestMerchant", merchantData);
        }

        public async Task UpdateTestContract(Contract contract)
        {
            String contractData = JsonConvert.SerializeObject(contract);
            await this.ExecuteBackdoor("UpdateTestContract", contractData);
        }

        private async Task ExecuteBackdoor(string methodName, string value)
        {           
            if (AppiumDriver.MobileTestPlatform == MobileTestPlatform.Android)
            {
                Dictionary<String, Object> args = BackdoorDriver.CreateBackdoorArgs(methodName, value);
                AppiumDriver.AndroidDriver.ExecuteScript("mobile: backdoor", args);
            }
            else if (AppiumDriver.MobileTestPlatform == MobileTestPlatform.iOS)
            {
                var client = await MqttClient.CreateAsync("127.0.0.1", 1883);
                var result = await client.ConnectAsync();
                MqttApplicationMessage m = new MqttApplicationMessage($"IOSBackdoor/{methodName}", Encoding.Default.GetBytes(value));
                await client.PublishAsync(m, MqttQualityOfService.ExactlyOnce);

                var x = client.MessageStream;
                //Rx Subscription to receive all the messages for the subscribed topics

                client
                    .MessageStream
                    .Where(msg => msg.Topic == "IOSBackdoor/responses")
                    .Subscribe(msg => Console.WriteLine(Encoding.Default.GetString(msg.Payload)));

            }
        }

        private static Dictionary<string, object> CreateBackdoorArgs(string methodName, string value)
        {
            if (AppiumDriver.MobileTestPlatform == MobileTestPlatform.iOS)
            {
                methodName = $"{methodName}:";
            }

            return new Dictionary<string, object>
                   {
                       {"target", "application"},
                       {
                           "methods", new List<Dictionary<string, object>>
                                      {
                                          new Dictionary<string, object>
                                          {
                                              {"name", methodName},
                                              {
                                                  "args", new List<Dictionary<string, object>>
                                                          {
                                                              new Dictionary<string, object>
                                                              {
                                                                  {"value", value},
                                                                  {"type", "String"}
                                                              }
                                                          }
                                              }
                                          }
                                      }
                       }
                   };
        }
    }
}