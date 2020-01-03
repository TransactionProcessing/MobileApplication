﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace TransactionMobile.IntegrationTests.Pages
{
    using Common;

    public class LoginPage : BasePage
    {
        protected override PlatformQuery Trait => new PlatformQuery
                                                  {
                                                      Android = x => x.Marked("menu_add_task"),
                                                      iOS = x => x.Marked("Tasky")
                                                  };

        private readonly Query EmailAddressEntry;
        private readonly Query PasswordEntry;
        private readonly Query LoginButton;

        public LoginPage()
        {
            this.EmailAddressEntry = x => x.Marked("EmailEntry");
            this.PasswordEntry= x => x.Marked("PasswordEntry");
            this.LoginButton = x => x.Marked("LoginButton");
        }

        public void EnterEmailAddress(String emailAddress)
        {
            app.WaitForElement(this.EmailAddressEntry);
            app.EnterText(this.EmailAddressEntry, emailAddress);
        }

        public void EnterPassword(String password)
        {
            app.WaitForElement(this.PasswordEntry);
            app.EnterText(this.PasswordEntry,password);
        }

        public void ClickLoginButton()
        {
            app.WaitForElement(this.LoginButton);
            app.Tap(this.LoginButton);
        }
    }
}