namespace TransactionMobile.UnitTests.ViewModels
{
    using NUnit.Framework;
    using Shouldly;
    using TransactionMobile.ViewModels;

    public class LoginViewModelsTests
    {
        [Test]
        public void LoginViewModel_SetEmailAddress_PropertyChangedFired()
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            loginViewModel.PropertyChanged += this.LoginViewModel_PropertyChangedEmailAddress;
            loginViewModel.EmailAddress = TestData.EmailAddress;
            loginViewModel.EmailAddress.ShouldBe(TestData.EmailAddress);
        }

        [Test]
        public void LoginViewModel_SetPassword_PropertyChangedFired()
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            loginViewModel.PropertyChanged += this.LoginViewModel_PropertyChangedPassword;
            loginViewModel.Password = TestData.Password;
            loginViewModel.Password.ShouldBe(TestData.Password);
        }

        private void LoginViewModel_PropertyChangedEmailAddress(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            LoginViewModel viewModel = (LoginViewModel)sender;

            e.PropertyName.ShouldBe(nameof(viewModel.EmailAddress));
        }

        private void LoginViewModel_PropertyChangedPassword(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            LoginViewModel viewModel = (LoginViewModel)sender;

            e.PropertyName.ShouldBe(nameof(viewModel.Password));
        }
    }
}