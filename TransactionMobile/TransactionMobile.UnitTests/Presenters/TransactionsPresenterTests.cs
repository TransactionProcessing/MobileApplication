namespace TransactionMobile.UnitTests.Presenters
{
    using Common;
    using Database;
    using Moq;
    using NUnit.Framework;
    using Pages;
    using Services;
    using Shouldly;
    using TransactionMobile.Presenters;
    using TransactionMobile.Services;
    using TransactionMobile.ViewModels;
    using ViewModels;

    [TestFixture]
    public class TransactionsPresenterTests
    {
        [Test]
        public void TransactionsPresenter_CanBeCreated_IsCreated()
        {
            Mock<ITransactionsPage> transactionsPage = new Mock<ITransactionsPage>();
            Mock<IMobileTopupSelectOperatorPage> mobileTopupSelectOperatorPage = new Mock<IMobileTopupSelectOperatorPage>();
            MobileTopupSelectOperatorViewModel mobileTopupSelectOperatorViewModel = new MobileTopupSelectOperatorViewModel();
            Mock<IMobileTopupSelectProductPage> mobileTopupSelectProductPage = new Mock<IMobileTopupSelectProductPage>();
            MobileTopupSelectProductViewModel mobileTopupSelectProductViewModel = new MobileTopupSelectProductViewModel();
            Mock<IMobileTopupPerformTopupPage> mobileTopupPerformTopupPage = new Mock<IMobileTopupPerformTopupPage>();
            MobileTopupPerformTopupViewModel mobileTopupPerformTopupViewModel = new MobileTopupPerformTopupViewModel();
            Mock<IMobileTopupPaymentSuccessPage> mobileTopupPaymentSuccessPage = new Mock<IMobileTopupPaymentSuccessPage>();
            Mock<IMobileTopupPaymentFailedPage> mobileTopupPaymentFailedPage = new Mock<IMobileTopupPaymentFailedPage>();
            Mock<IAdminPage> adminPage = new Mock<IAdminPage>();
            Mock<IDevice> device = new Mock<IDevice>();
            Mock<ITransactionProcessorACLClient> transactionProcessorACLClient = new Mock<ITransactionProcessorACLClient>();
            Mock<ILoggingDatabaseContext> loggingDatabase = new Mock<ILoggingDatabaseContext>();

            TransactionsPresenter transactionsPresenter = new TransactionsPresenter(transactionsPage.Object,
                                                                                    mobileTopupSelectOperatorPage.Object,
                                                                                    mobileTopupSelectOperatorViewModel,
                                                                                    mobileTopupPerformTopupPage.Object,
                                                                                    mobileTopupPerformTopupViewModel,
                                                                                    mobileTopupSelectProductPage.Object,
                                                                                    mobileTopupSelectProductViewModel,
                                                                                    mobileTopupPaymentSuccessPage.Object,
                                                                                    mobileTopupPaymentFailedPage.Object,
                                                                                    adminPage.Object,
                                                                                    device.Object,
                                                                                    transactionProcessorACLClient.Object,
                                                                                    loggingDatabase.Object);

            transactionsPresenter.ShouldNotBeNull();
        }
    }
}