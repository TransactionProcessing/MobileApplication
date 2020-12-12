namespace TransactionMobile.IntegrationTests.Pages
{
    using Common;

    public class MobileTopupSuccessPage : BasePage
    {
        protected override PlatformQuery Trait =>
            new PlatformQuery
            {
                Android = x => x.Marked("MOBILE TOPUP SUCCESSFUL"),
                iOS = x => x.Marked("MOBILE TOPUP SUCCESSFUL")
            };
    }

    public class VoucherSuccessPage : BasePage
    {
        protected override PlatformQuery Trait =>
            new PlatformQuery
            {
                Android = x => x.Marked("VOUCHER ISSUE SUCCESSFUL"),
                iOS = x => x.Marked("VOUCHER ISSUE SUCCESSFUL")
            };
    }
}