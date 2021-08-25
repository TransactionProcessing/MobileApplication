namespace TransactionMobile.IntegrationTests.WithAppium.Drivers
{
    using Common;
    using IntegrationTestClients;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Mqtt;
    using System.Reactive.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
            else
            {
                // Send MQTT message
                var client = await MqttClient.CreateAsync("localhost", 1887);

                MqttApplicationMessage message = new MqttApplicationMessage(methodName, Encoding.Default.GetBytes(value));

                await client.PublishAsync(message, MqttQualityOfService.ExactlyOnce);
            }
        }

        private static Dictionary<string, object> CreateBackdoorArgs(string methodName, string value)
        {
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