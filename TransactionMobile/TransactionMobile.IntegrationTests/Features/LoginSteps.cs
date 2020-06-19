using System;
using TechTalk.SpecFlow;

namespace TransactionMobile.IntegrationTests
{
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using Common;
    using Features;
    using NUnit.Framework;
    using Pages;
    using Shouldly;
    using Xamarin.UITest;

    [Binding]
    [Scope(Tag = "login")]
    public class LoginSteps
    {
        LoginPage loginPage = new LoginPage();
        MainPage mainPage = new MainPage();

        [Given(@"I am on the Login Screen")]
        public async Task GivenIAmOnTheLoginScreen()
        {
            await this.loginPage.AssertOnPage();
        }
        
        [When(@"I enter '(.*)' as the Email Address")]
        public void WhenIEnterAsTheEmailAddress(String emailAddress)
        {
            this.loginPage.EnterEmailAddress(emailAddress);
        }
        
        [When(@"I enter '(.*)' as the Password")]
        public void WhenIEnterAsThePassword(String password)
        {
            this.loginPage.EnterPassword(password);
        }
        
        [When(@"I tap on Login")]
        public void WhenITapOnLogin()
        {
            this.loginPage.ClickLoginButton();
        }
        
        [Then(@"the Merchant Home Page is displayed")]
        public async Task ThenTheMerchantHomePageIsDisplayed()
        {
            await this.mainPage.AssertOnPage(TimeSpan.FromSeconds(60));
        }

        [Then(@"the available balance is shown as (.*)")]
        public async Task ThenTheAvailableBalanceIsShownAs(Decimal expectedAvailableBalance)
        {
            try
            {
                Decimal availableBalance = await this.mainPage.GetAvailableBalanceValue(TimeSpan.FromSeconds(120)).ConfigureAwait(false);

                availableBalance.ShouldBe(expectedAvailableBalance);
            }
            catch(Exception e)
            {
                String name = $"BalanceError{DateTime.UtcNow:yyyyMMddhhmmssfff}";
                FileInfo fi = AppManager.App.Screenshot(name);
                
                throw new Exception(fi.FullName, e);
            }
            
        }

    }
}
