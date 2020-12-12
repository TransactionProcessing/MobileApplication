// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:3.1.0.0
//      SpecFlow Generator Version:3.1.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace TransactionMobile.IntegrationTests.Features
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("VoucherIssue")]
    [NUnit.Framework.CategoryAttribute("login")]
    [NUnit.Framework.CategoryAttribute("voucherissue")]
    [NUnit.Framework.CategoryAttribute("base")]
    [NUnit.Framework.CategoryAttribute("shared")]
    public partial class VoucherIssueFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "login",
                "voucherissue",
                "base",
                "shared"};
        
#line 1 "VoucherIssue.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "VoucherIssue", null, ProgrammingLanguage.CSharp, new string[] {
                        "login",
                        "voucherissue",
                        "base",
                        "shared"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<NUnit.Framework.TestContext>(NUnit.Framework.TestContext.CurrentContext);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 4
#line hidden
            TechTalk.SpecFlow.Table table45 = new TechTalk.SpecFlow.Table(new string[] {
                        "RoleName"});
            table45.AddRow(new string[] {
                        "Merchant"});
#line 6
 testRunner.Given("the following security roles exist", ((string)(null)), table45, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table46 = new TechTalk.SpecFlow.Table(new string[] {
                        "ResourceName",
                        "DisplayName",
                        "Secret",
                        "Scopes",
                        "UserClaims"});
            table46.AddRow(new string[] {
                        "estateManagement",
                        "Estate Managememt REST",
                        "Secret1",
                        "estateManagement",
                        "MerchantId, EstateId, role"});
            table46.AddRow(new string[] {
                        "transactionProcessor",
                        "Transaction Processor REST",
                        "Secret1",
                        "transactionProcessor",
                        ""});
            table46.AddRow(new string[] {
                        "transactionProcessorACL",
                        "Transaction Processor ACL REST",
                        "Secret1",
                        "transactionProcessorACL",
                        "MerchantId, EstateId, role"});
            table46.AddRow(new string[] {
                        "voucherManagement",
                        "Voucher Management REST",
                        "Secret1",
                        "voucherManagement",
                        ""});
#line 10
 testRunner.Given("the following api resources exist", ((string)(null)), table46, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table47 = new TechTalk.SpecFlow.Table(new string[] {
                        "ClientId",
                        "ClientName",
                        "Secret",
                        "AllowedScopes",
                        "AllowedGrantTypes"});
            table47.AddRow(new string[] {
                        "serviceClient",
                        "Service Client",
                        "Secret1",
                        "estateManagement,transactionProcessor,transactionProcessorACL,voucherManagement",
                        "client_credentials"});
            table47.AddRow(new string[] {
                        "merchantClient",
                        "Merchant Client",
                        "Secret1",
                        "transactionProcessorACL,estateManagement",
                        "password"});
#line 17
 testRunner.Given("the following clients exist", ((string)(null)), table47, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table48 = new TechTalk.SpecFlow.Table(new string[] {
                        "ClientId"});
            table48.AddRow(new string[] {
                        "serviceClient"});
#line 22
 testRunner.Given("I have a token to access the estate management and transaction processor acl reso" +
                    "urces", ((string)(null)), table48, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table49 = new TechTalk.SpecFlow.Table(new string[] {
                        "EstateName"});
            table49.AddRow(new string[] {
                        "Test Estate 1"});
#line 26
 testRunner.Given("I have created the following estates", ((string)(null)), table49, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table50 = new TechTalk.SpecFlow.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "RequireCustomMerchantNumber",
                        "RequireCustomTerminalNumber"});
            table50.AddRow(new string[] {
                        "Test Estate 1",
                        "Voucher",
                        "True",
                        "True"});
#line 30
 testRunner.Given("I have created the following operators", ((string)(null)), table50, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table51 = new TechTalk.SpecFlow.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "ContractDescription"});
            table51.AddRow(new string[] {
                        "Test Estate 1",
                        "Voucher",
                        "Healthcare Centre 1"});
#line 34
 testRunner.Given("I create a contract with the following values", ((string)(null)), table51, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table52 = new TechTalk.SpecFlow.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "ContractDescription",
                        "ProductName",
                        "DisplayText",
                        "Value"});
            table52.AddRow(new string[] {
                        "Test Estate 1",
                        "Voucher",
                        "Healthcare Centre 1",
                        "10 KES Topup",
                        "10 KES",
                        "10.00"});
#line 38
 testRunner.When("I create the following Products", ((string)(null)), table52, "When ");
#line hidden
            TechTalk.SpecFlow.Table table53 = new TechTalk.SpecFlow.Table(new string[] {
                        "MerchantName",
                        "AddressLine1",
                        "Town",
                        "Region",
                        "Country",
                        "ContactName",
                        "EmailAddress",
                        "EstateName"});
            table53.AddRow(new string[] {
                        "Test Merchant 1",
                        "Address Line 1",
                        "TestTown",
                        "Test Region",
                        "United Kingdom",
                        "Test Contact 1",
                        "testcontact1@merchant1.co.uk",
                        "Test Estate 1"});
#line 42
 testRunner.Given("I create the following merchants", ((string)(null)), table53, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table54 = new TechTalk.SpecFlow.Table(new string[] {
                        "EmailAddress",
                        "Password",
                        "GivenName",
                        "FamilyName",
                        "EstateName",
                        "MerchantName"});
            table54.AddRow(new string[] {
                        "merchantuser@testmerchant1.co.uk",
                        "123456",
                        "TestMerchant",
                        "User1",
                        "Test Estate 1",
                        "Test Merchant 1"});
#line 46
 testRunner.Given("I have created the following security users", ((string)(null)), table54, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table55 = new TechTalk.SpecFlow.Table(new string[] {
                        "OperatorName",
                        "MerchantName",
                        "MerchantNumber",
                        "TerminalNumber",
                        "EstateName"});
            table55.AddRow(new string[] {
                        "Voucher",
                        "Test Merchant 1",
                        "00000001",
                        "10000001",
                        "Test Estate 1"});
#line 50
 testRunner.Given("I have assigned the following  operator to the merchants", ((string)(null)), table55, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table56 = new TechTalk.SpecFlow.Table(new string[] {
                        "Reference",
                        "Amount",
                        "DateTime",
                        "MerchantName",
                        "EstateName"});
            table56.AddRow(new string[] {
                        "Deposit1",
                        "100.00",
                        "Today",
                        "Test Merchant 1",
                        "Test Estate 1"});
            table56.AddRow(new string[] {
                        "Deposit2",
                        "100.00",
                        "Yesterday",
                        "Test Merchant 1",
                        "Test Estate 1"});
#line 54
 testRunner.Given("I make the following manual merchant deposits", ((string)(null)), table56, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table57 = new TechTalk.SpecFlow.Table(new string[] {
                        "Balance",
                        "AvailableBalance",
                        "MerchantName",
                        "EstateName"});
            table57.AddRow(new string[] {
                        "200.00",
                        "200.00",
                        "Test Merchant 1",
                        "Test Estate 1"});
#line 59
 testRunner.Then("the merchant balances are as follows", ((string)(null)), table57, "Then ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Successful Voucher Issue")]
        [NUnit.Framework.CategoryAttribute("PRTest")]
        public virtual void SuccessfulVoucherIssue()
        {
            string[] tagsOfScenario = new string[] {
                    "PRTest"};
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Successful Voucher Issue", null, new string[] {
                        "PRTest"});
#line 64
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 4
this.FeatureBackground();
#line hidden
#line 65
 testRunner.Given("I am on the Login Screen", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 67
 testRunner.When("I enter \'merchantuser@testmerchant1.co.uk\' as the Email Address", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 68
 testRunner.And("I enter \'123456\' as the Password", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 69
 testRunner.And("I tap on Login", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 71
 testRunner.Then("the Merchant Home Page is displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 73
 testRunner.And("the available balance is shown as 200.00", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 75
 testRunner.Given("I tap on the Transactions button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 76
 testRunner.Then("the Transactions Page is displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 78
 testRunner.Given("I tap on the Voucher button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 79
 testRunner.Then("the Voucher Select Operator Page is displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 81
 testRunner.Given("I tap on the HealthcareCentre1 button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 82
 testRunner.Then("the Voucher Select Product Page is displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 84
 testRunner.Given("I tap on the 10 KES button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 85
 testRunner.Then("the Voucher Issue Page is displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
                TechTalk.SpecFlow.Table table58 = new TechTalk.SpecFlow.Table(new string[] {
                            "RecipientMobile",
                            "RecipientEmail",
                            "CustomerEmailAddress"});
                table58.AddRow(new string[] {
                            "123456789",
                            "",
                            ""});
#line 87
 testRunner.When("I enter the following recipient details", ((string)(null)), table58, "When ");
#line hidden
#line 91
 testRunner.And("I tap on Issue Voucher", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 93
 testRunner.Then("The Voucher Issue Successful Screen will be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
