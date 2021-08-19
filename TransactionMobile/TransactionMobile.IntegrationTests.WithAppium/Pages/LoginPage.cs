using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionMobile.IntegrationTests.WithAppium.Pages
{
    using System.Threading.Tasks;
    using OpenQA.Selenium.Appium.Android.UiAutomator;
    using OpenQA.Selenium.Appium.Enums;
    using OpenQA.Selenium.Appium.Interfaces;
    using Shouldly;

    public class LoginPage : BasePage
    {
        protected override String Trait => "LoginLabel";

        private readonly String EmailEntry;
        private readonly String PasswordEntry;
        private readonly String LoginButton;
        private readonly String ErrorLabel;

        public LoginPage()
        {
            this.EmailEntry = "EmailEntry";
            this.PasswordEntry = "PasswordEntry";
            this.LoginButton = "LoginButton";
            this.ErrorLabel = "ErrorLabel";
        }

        public async Task EnterEmailAddress(String emailAddress)
        {
            var element = await app.WaitForElementByAccessibilityId(this.EmailEntry);
            
            element.SendKeys(emailAddress);
        }

        public async Task EnterPassword(String password)
        {
            var element = await app.WaitForElementByAccessibilityId(this.PasswordEntry);
            element.SendKeys(password);
        }

        public async Task ClickLoginButton()
        {
            app.HideKeyboard();
            var element = await app.WaitForElementByAccessibilityId(this.LoginButton);
            element.Click();
        }
    }
}
