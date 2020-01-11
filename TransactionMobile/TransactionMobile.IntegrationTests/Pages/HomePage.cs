namespace TransactionMobile.IntegrationTests.Pages
{
    using System;
    using Common;
    using Xamarin.UITest.Queries;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="TransactionMobile.IntegrationTests.Common.BasePage" />
    public class HomePage : BasePage
    {
        #region Fields

        /// <summary>
        /// The home page label
        /// </summary>
        private readonly Func<AppQuery, AppQuery> HomePageLabel;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HomePage"/> class.
        /// </summary>
        public HomePage()
        {
            this.HomePageLabel = x => x.Marked("HomePageLabel");
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
                Android = x => x.Marked("This is my home!"),
                iOS = x => x.Marked("This is my home!")
            };

        #endregion
    }
}