using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.UnitTests.Presenters
{
    using Common;
    using Database;
    using EstateManagement.Client;
    using Moq;
    using NUnit.Framework;
    using Pages;
    using SecurityService.Client;
    using Services;
    using Shouldly;
    using TransactionMobile.Presenters;
    using TransactionMobile.Services;
    using TransactionMobile.ViewModels;
    using ViewModels;

    [TestFixture]
    public class LoginPresenterTests
    {
        [Test]
        public void LoginPresenter_CanBeCreated_IsCreated()
        {
            Mock<ILoginPage> loginPage = new Mock<ILoginPage>();
            Mock<IMainPage> mainPage = new Mock<IMainPage>();
            LoginViewModel loginViewModel = new LoginViewModel();
            MainPageViewModel mainPageViewModel = new MainPageViewModel();
            Mock<IDevice> device = new Mock<IDevice>();
            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();
            Mock<ITransactionProcessorACLClient> transactionProcessorACLClient = new Mock<ITransactionProcessorACLClient>();
            Mock<IEstateClient> estateClient = new Mock<IEstateClient>();
            Mock<IDatabaseContext> loggingDatabase = new Mock<IDatabaseContext>();
            LoginPresenter loginPresenter =
                new LoginPresenter(loginPage.Object, mainPage.Object, 
                                   loginViewModel, mainPageViewModel,
                                   device.Object, securityServiceClient.Object, 
                                   transactionProcessorACLClient.Object,
                                   estateClient.Object,
                                   loggingDatabase.Object);

            loginPresenter.ShouldNotBeNull();
        }
    }
}
