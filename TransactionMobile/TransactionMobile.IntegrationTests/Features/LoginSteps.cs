using System;
using TechTalk.SpecFlow;

namespace TransactionMobile.IntegrationTests
{
    using System.Threading.Tasks;
    using Features;
    using NUnit.Framework;
    using Pages;
    using Xamarin.UITest;

    [Binding]
    [Scope(Tag = "login")]
    public class LoginSteps
    {
        LoginPage loginPage = new LoginPage();
        HomePage homePage = new HomePage();

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
            await this.homePage.AssertOnPage(TimeSpan.FromSeconds(15));
        }
    }
}
