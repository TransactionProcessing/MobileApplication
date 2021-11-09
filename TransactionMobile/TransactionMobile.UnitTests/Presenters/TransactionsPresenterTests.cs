namespace TransactionMobile.UnitTests.Presenters
{
    using Clients;
    using Common;
    using Database;
    using Moq;
    using NUnit.Framework;
    using Pages;
    using Services;
    using Shouldly;
    using TransactionMobile.Pages.Admin;
    using TransactionMobile.Pages.Transactions;
    using TransactionMobile.Presenters;
    using TransactionMobile.ViewModels;
    using TransactionMobile.ViewModels.Transactions;
    using ViewModels;
    using Xunit;

    public class TransactionsPresenterTests
    {
        [Fact]
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
            Mock<IVoucherSelectOperatorPage> voucherSelectOperatorPage = new Mock<IVoucherSelectOperatorPage>();
            VoucherSelectOperatorViewModel voucherSelectOperatorViewModel = new VoucherSelectOperatorViewModel();
            Mock<IVoucherSelectProductPage> voucherSelectProductPage = new Mock<IVoucherSelectProductPage>();
            VoucherSelectProductViewModel voucherSelectProductViewModel = new VoucherSelectProductViewModel();
            Mock<IVoucherPerformVoucherIssuePage> voucherPerformVoucherIssuePage = new Mock<IVoucherPerformVoucherIssuePage>();
            VoucherPerformVoucherIssueViewModel voucherPerformVoucherIssueViewModel = new VoucherPerformVoucherIssueViewModel();
            Mock<IVoucherSuccessPage> voucherSuccessPage = new Mock<IVoucherSuccessPage>();
            Mock<IVoucherFailedPage> voucherFailedPage = new Mock<IVoucherFailedPage>();
            Mock<IAdminPage> adminPage = new Mock<IAdminPage>();
            Mock<IDevice> device = new Mock<IDevice>();
            Mock<ITransactionProcessorACLClient> transactionProcessorACLClient = new Mock<ITransactionProcessorACLClient>();
            Mock<IDatabaseContext> database = new Mock<IDatabaseContext>();

            TransactionsPresenter transactionsPresenter = new TransactionsPresenter(transactionsPage.Object,
                                                                                    mobileTopupSelectOperatorPage.Object,
                                                                                    mobileTopupSelectOperatorViewModel,
                                                                                    mobileTopupPerformTopupPage.Object,
                                                                                    mobileTopupPerformTopupViewModel,
                                                                                    mobileTopupSelectProductPage.Object,
                                                                                    mobileTopupSelectProductViewModel,
                                                                                    mobileTopupPaymentSuccessPage.Object,
                                                                                    mobileTopupPaymentFailedPage.Object,
                                                                                    voucherSelectOperatorPage.Object,
                                                                                    voucherSelectOperatorViewModel,
                                                                                    voucherSelectProductPage.Object,
                                                                                    voucherSelectProductViewModel,
                                                                                    voucherPerformVoucherIssuePage.Object,
                                                                                    voucherPerformVoucherIssueViewModel,
                                                                                    voucherSuccessPage.Object,
                                                                                    voucherFailedPage.Object,
                                                                                    adminPage.Object,
                                                                                    device.Object,
                                                                                    transactionProcessorACLClient.Object,
                                                                                    database.Object);

            transactionsPresenter.ShouldNotBeNull();
        }
    }
}