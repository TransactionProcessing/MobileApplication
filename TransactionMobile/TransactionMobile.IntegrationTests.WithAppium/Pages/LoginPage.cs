﻿using System;
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
        private readonly String TestModeButton;
        private readonly String ErrorLabel;

        public LoginPage()
        {
            this.EmailEntry = "EmailEntry";
            this.PasswordEntry = "PasswordEntry";
            this.LoginButton = "LoginButton";
            this.TestModeButton = "TestModeButton";
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
        }

        public async Task ClickTestModeButton()
        {
            this.HideKeyboard();
            IWebElement element = await this.WaitForElementByAccessibilityId(this.TestModeButton);
            element.Click();
        }
    }

    public class TestModePage : BasePage
    {
        private readonly String PinEntry;
        private readonly String TestContractDataEntry;
        private readonly String TestMerchantDataEntry;

        private readonly String SetTestModeButton;
        public TestModePage()
        {
            this.PinEntry = "PinEntry";
            this.TestMerchantDataEntry = "TestMerchantDataEntry";
            this.TestContractDataEntry = "TestContractDataEntry";
            this.SetTestModeButton = "SetTestModeButton";
        }

        public async Task EnterPin(String pinNumber)
        {
            this.HideKeyboard();
            IWebElement element = await this.WaitForElementByAccessibilityId(this.PinEntry);
            element.ShouldNotBeNull(this.PinEntry);
            element.SendKeys(pinNumber);
        }

        public async Task EnterTestMerchantData(String testMerchantData)
        {
            this.HideKeyboard();
            IWebElement element = await this.WaitForElementByAccessibilityId(this.TestMerchantDataEntry);
            element.ShouldNotBeNull(this.TestMerchantDataEntry);
            element.SendKeys(testMerchantData);
        }

        public async Task EnterTestContractData(String testContractData)
        {
            this.HideKeyboard();
            IWebElement element = await this.WaitForElementByAccessibilityId(this.TestContractDataEntry);
            element.ShouldNotBeNull(this.TestContractDataEntry);
            element.SendKeys(testContractData);
        }

        public async Task ClickSetTestModeButton()
        {
            this.HideKeyboard();
            IWebElement element = await this.WaitForElementByAccessibilityId(this.SetTestModeButton);
            element.ShouldNotBeNull(this.SetTestModeButton);
            element.Click();
        }

        protected override String Trait => "TestModeLabel";
    }
}
