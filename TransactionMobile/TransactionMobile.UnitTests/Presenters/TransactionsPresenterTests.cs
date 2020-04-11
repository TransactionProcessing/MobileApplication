namespace TransactionMobile.UnitTests.Presenters
{
    using Common;
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
            Mock<IMobileTopupPerformTopupPage> mobileTopupPerformTopupPage = new Mock<IMobileTopupPerformTopupPage>();
            MobileTopupPerformTopupViewModel mobileTopupPerformTopupViewModel = new MobileTopupPerformTopupViewModel();
            Mock<IMobileTopupPaymentSuccessPage> mobileTopupPaymentSuccessPage = new Mock<IMobileTopupPaymentSuccessPage>();
            Mock<IMobileTopupPaymentFailedPage> mobileTopupPaymentFailedPage = new Mock<IMobileTopupPaymentFailedPage>();
            Mock<IDevice> device = new Mock<IDevice>();
            Mock<ITransactionProcessorACLClient> transactionProcessorACLClient = new Mock<ITransactionProcessorACLClient>();

            TransactionsPresenter transactionsPresenter = new TransactionsPresenter(transactionsPage.Object,
                                                                                    mobileTopupSelectOperatorPage.Object,
                                                                                    mobileTopupSelectOperatorViewModel,
                                                                                    mobileTopupPerformTopupPage.Object,
                                                                                    mobileTopupPerformTopupViewModel,
                                                                                    mobileTopupPaymentSuccessPage.Object,
                                                                                    mobileTopupPaymentFailedPage.Object,
                                                                                    device.Object,
                                                                                    transactionProcessorACLClient.Object);

            transactionsPresenter.ShouldNotBeNull();
        }
    }
}