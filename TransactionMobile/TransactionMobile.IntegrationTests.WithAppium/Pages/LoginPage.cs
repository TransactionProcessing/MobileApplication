using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionMobile.IntegrationTests.WithAppium.Pages
{
    using System.Threading.Tasks;
    using Drivers;
    using OpenQA.Selenium;
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
            IWebElement element = await this.WaitForElementByAccessibilityId(this.EmailEntry);
            
            element.SendKeys(emailAddress);
        }

        public async Task EnterPassword(String password)
        {
            IWebElement element = await this.WaitForElementByAccessibilityId(this.PasswordEntry);
            element.SendKeys(password);
        }

        public async Task ClickLoginButton()
        {
            this.HideKeyboard();
            IWebElement element = await this.WaitForElementByAccessibilityId(this.LoginButton);
            element.Click();
            
            //try
            //{
            //    IWebElement element1 = await this.WaitForElementByAccessibilityId("DebugLabel");
            //    Console.WriteLine(element1.Text);
            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine(await this.GetPageSource());
            //}
        }
    }
}
