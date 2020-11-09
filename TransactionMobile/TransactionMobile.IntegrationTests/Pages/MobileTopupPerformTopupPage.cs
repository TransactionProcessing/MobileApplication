namespace TransactionMobile.IntegrationTests.Pages
{
    using System;
    using Common;
    using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

    public class MobileTopupPerformTopupPage : BasePage
    {
        #region Fields

        private readonly Query CustomerMobileNumberEntry;
        private readonly Query TopupAmountEntry;
        private readonly Query PerformTopupButton;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileTopupPerformTopupPage"/> class.
        /// </summary>
        public MobileTopupPerformTopupPage()
        {
            this.CustomerMobileNumberEntry = x => x.Marked("CustomerMobileNumberEntry");
            this.TopupAmountEntry = x => x.Marked("TopupAmountEntry");
            this.PerformTopupButton = x => x.Marked("PerformTopupButton");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the trait.
        /// </summary>
        /// <value>
        /// The trait.
        /// </value>
        protected override PlatformQuery Trait =>
            new PlatformQuery
            {
                Android = x => x.Marked("Topup Details"),
                iOS = x => x.Marked("Topup Details")
            };

        #endregion

        public void EnterCustomerMobileNumber(String customerMobileNumber)
        {
            app.WaitForElement(this.CustomerMobileNumberEntry);
            app.EnterText(this.CustomerMobileNumberEntry, customerMobileNumber);
        }

        public void EnterTopupAmount(Decimal topupAmount)
        {
            app.WaitForElement(this.TopupAmountEntry);
            app.EnterText(this.TopupAmountEntry, topupAmount.ToString());
        }

        public void ClickPerformTopupButton()
        {
            app.DismissKeyboard();
            app.WaitForElement(this.PerformTopupButton);
            app.Tap(this.PerformTopupButton);
        }

        public void AssertTopupValidationErrorDisplayed()
        {
            String errorMessage = "Please enter a mobile number and Topup Amount to continue";
            app.WaitForElement(errorMessage, timeout:TimeSpan.FromMinutes(2));
            
            // Dismiss the error
            app.Tap("OK");
        }
    }
}