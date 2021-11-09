namespace TransactionMobile.UnitTests.ViewModels
{
    using System;
    using System.ComponentModel;
    using NUnit.Framework;
    using Shouldly;
    using TransactionMobile.ViewModels;
    using TransactionMobile.ViewModels.Transactions;
    using Xunit;

    public class MobileTopupPerformTopupViewModelTests
    {
        [Fact]
        public void MobileTopupPerformTopupViewModel_TopupAmount_PropertyChangedFired()
        {
            MobileTopupPerformTopupViewModel mobileTopupPerformTopupViewModel = new MobileTopupPerformTopupViewModel();
            mobileTopupPerformTopupViewModel.PropertyChanged += this.MobileTopupPerformTopupViewModel_PropertyChangedTopupAmount;
            mobileTopupPerformTopupViewModel.TopupAmount = TestData.TopupAmount;
            mobileTopupPerformTopupViewModel.TopupAmount.ShouldBe(TestData.TopupAmount);
        }

        [Fact]
        public void MobileTopupPerformTopupViewModel_CustomerMobileNumber_PropertyChangedFired()
        {
            MobileTopupPerformTopupViewModel mobileTopupPerformTopupViewModel = new MobileTopupPerformTopupViewModel();
            mobileTopupPerformTopupViewModel.PropertyChanged += this.MobileTopupPerformTopupViewModel_PropertyChangedCustomerMobileNumber;
            mobileTopupPerformTopupViewModel.CustomerMobileNumber = TestData.CustomerMobileNumber;
            mobileTopupPerformTopupViewModel.CustomerMobileNumber.ShouldBe(TestData.CustomerMobileNumber);
        }

        [Fact]
        public void MobileTopupPerformTopupViewModel_OperatorName_OperatorNameSet()
        {
            MobileTopupPerformTopupViewModel mobileTopupPerformTopupViewModel = new MobileTopupPerformTopupViewModel();
            mobileTopupPerformTopupViewModel.OperatorName = TestData.OperatorName;
            mobileTopupPerformTopupViewModel.OperatorName.ShouldBe(TestData.OperatorName);
        }

        private void MobileTopupPerformTopupViewModel_PropertyChangedCustomerMobileNumber(Object sender,
                                                                                          PropertyChangedEventArgs e)
        {
            MobileTopupPerformTopupViewModel viewModel = (MobileTopupPerformTopupViewModel)sender;
            e.PropertyName.ShouldBe(nameof(viewModel.CustomerMobileNumber));
        }

        private void MobileTopupPerformTopupViewModel_PropertyChangedTopupAmount(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            MobileTopupPerformTopupViewModel viewModel = (MobileTopupPerformTopupViewModel)sender;
            e.PropertyName.ShouldBe(nameof(viewModel.TopupAmount));
        }
    }
}