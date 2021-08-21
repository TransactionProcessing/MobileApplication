namespace TransactionMobile.IntegrationTests.WithAppium.Drivers
{
    using System;
    using System.Collections.Generic;
    using IntegrationTestClients;
    using Newtonsoft.Json;

    public class BackdoorDriver
    {
        private readonly AppiumDriver AppiumDriver;

        public BackdoorDriver(AppiumDriver appiumDriver)
        {
            this.AppiumDriver = appiumDriver;
        }

        public void SetIntegrationModeOn()
        {
            this.ExecuteBackdoor("SetIntegrationTestModeOn", "");
        }

        public void UpdateTestMerchant(Merchant merchant)
        {
            String merchantData = JsonConvert.SerializeObject(merchant);
            this.ExecuteBackdoor("UpdateTestMerchant", merchantData);
        }

        public void UpdateTestContract(Contract contract)
        {
            String contractData = JsonConvert.SerializeObject(contract);
            this.ExecuteBackdoor("UpdateTestContract", contractData);
        }

        private void ExecuteBackdoor(string methodName, string value)
        {
            Dictionary<String, Object> args = BackdoorDriver.CreateBackdoorArgs(methodName, value);
            
            AppiumDriver.Driver.ExecuteScript("mobile: backdoor", args);
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