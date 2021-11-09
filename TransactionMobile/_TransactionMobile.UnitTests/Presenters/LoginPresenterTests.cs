using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.UnitTests.Presenters
{
    using Clients;
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
            Mock<ITestModePage> testModePage = new Mock<ITestModePage>();
            Mock<ISupportPage> supportPage = new Mock<ISupportPage>();
            LoginViewModel loginViewModel = new LoginViewModel();
            MainPageViewModel mainPageViewModel = new MainPageViewModel();
            TestModePageViewModel testModePageViewModel = new TestModePageViewModel();
            Mock<IDevice> device = new Mock<IDevice>();
            Mock<ITransactionProcessorACLClient> transactionProcessorACLClient = new Mock<ITransactionProcessorACLClient>();
            Mock<IEstateClient> estateClient = new Mock<IEstateClient>();
            Mock<IDatabaseContext> loggingDatabase = new Mock<IDatabaseContext>();
            LoginPresenter loginPresenter =
                new LoginPresenter(loginPage.Object, mainPage.Object,
                                   testModePage.Object,
                                   supportPage.Object,
                                   loginViewModel, 
                                   mainPageViewModel,
                                   testModePageViewModel,
                                   device.Object, 
                                   transactionProcessorACLClient.Object,
                                   estateClient.Object,
                                   loggingDatabase.Object);

            loginPresenter.ShouldNotBeNull();
        }
    }
}
