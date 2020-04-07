namespace TransactionMobile.IntegrationTests.Pages
{
    using Common;

    public class MobileTopupFailedPage : BasePage
    {
        protected override PlatformQuery Trait =>
            new PlatformQuery
            {
                Android = x => x.Marked("MOBILE TOPUP FAILURE"),
                iOS = x => x.Marked("MOBILE TOPUP FAILURE")
            };
    }
}