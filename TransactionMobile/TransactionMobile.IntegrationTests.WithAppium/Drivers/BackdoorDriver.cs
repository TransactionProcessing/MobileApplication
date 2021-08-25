namespace TransactionMobile.IntegrationTests.WithAppium.Drivers
{
    using Common;
    using IntegrationTestClients;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reactive.Linq;
    using System.Threading.Tasks;

    public class BackdoorDriver
    {
        private Merchant Merchant = null;

        private List<Contract> Contracts = new List<Contract>();
        public BackdoorDriver()
        {
        }

        public async Task SetIntegrationModeOn()
        {
            if (AppiumDriver.MobileTestPlatform == MobileTestPlatform.Android)
            {
                await this.ExecuteBackdoor("SetIntegrationTestModeOn", "");
            }
        }

        public async Task UpdateTestMerchant(Merchant merchant)
        {
            if (AppiumDriver.MobileTestPlatform == MobileTestPlatform.Android)
            {
                String merchantData = JsonConvert.SerializeObject(merchant);
                await this.ExecuteBackdoor("UpdateTestMerchant", merchantData);
            }
            else if(AppiumDriver.MobileTestPlatform == MobileTestPlatform.iOS)
            {
                this.Merchant = merchant;
            }
        }

        public async Task UpdateTestContract(Contract contract)
        {
            if (AppiumDriver.MobileTestPlatform == MobileTestPlatform.Android)
            {
                String contractData = JsonConvert.SerializeObject(contract);
                await this.ExecuteBackdoor("UpdateTestContract", contractData);
            }
            else if (AppiumDriver.MobileTestPlatform == MobileTestPlatform.iOS)
            {
                this.Contracts.Add(contract);
            }
        }

        private async Task ExecuteBackdoor(string methodName, string value)
        {           
            Dictionary<String, Object> args = BackdoorDriver.CreateBackdoorArgs(methodName, value);
            AppiumDriver.AndroidDriver.ExecuteScript("mobile: backdoor", args);
        }

        public void PushTestDataFile()
        {
            if (AppiumDriver.MobileTestPlatform == MobileTestPlatform.iOS)
            {
                var fileData = new
                               {
                    this.Merchant,
                    this.Contracts
                               };
                AppiumDriver.iOSDriver.PushFile("@com.companyname.TransactionMobile:documents/testdata.txt", JsonConvert.SerializeObject(fileData));
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