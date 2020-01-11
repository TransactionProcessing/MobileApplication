using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.UnitTests.Presenters
{
    using Common;
    using Moq;
    using NUnit.Framework;
    using Pages;
    using SecurityService.Client;
    using Shouldly;
    using TransactionMobile.Presenters;
    using ViewModels;

    [TestFixture]
    public class LoginPresenterTests
    {
        [Test]
        public void LoginPresenter_CanBeCreated_IsCreated()
        {
            Mock<ILoginPage> loginPage = new Mock<ILoginPage>();
            LoginViewModel loginViewModel = new LoginViewModel();
            Mock<IDevice> device = new Mock<IDevice>();
            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();

            LoginPresenter loginPresenter = new LoginPresenter(loginPage.Object,loginViewModel, device.Object, securityServiceClient.Object);

            loginPresenter.ShouldNotBeNull();
        }
    }
}
