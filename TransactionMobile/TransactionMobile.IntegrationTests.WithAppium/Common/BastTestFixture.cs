using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionMobile.IntegrationTests.WithAppium.Common
{
    using Drivers;
    using NUnit.Framework;

    public enum MobileTestPlatform
    {
        //Android,

        iOS,
        Android
    }

    public abstract class BaseTestFixture
    {
        protected BaseTestFixture(MobileTestPlatform mobileTestPlatform)
        {
            AppiumDriver.MobileTestPlatform = mobileTestPlatform;
        }
    }
}
namespace TransactionMobile.IntegrationTests.WithAppium.Features
{
    using Common;
    using NUnit.Framework;

    [TestFixture(MobileTestPlatform.Android, Category = "Android")]
    [TestFixture(MobileTestPlatform.iOS, Category = "iOS")]
    public partial class LoginFeature : BaseTestFixture
    {
        public LoginFeature(MobileTestPlatform mobileTestPlatform)
            : base(mobileTestPlatform)
        {
        }
    }

    [TestFixture(MobileTestPlatform.Android, Category = "Android")]
    [TestFixture(MobileTestPlatform.iOS, Category = "iOS")]
    public partial class SafaricomTopupFeature : BaseTestFixture
    {
        public SafaricomTopupFeature(MobileTestPlatform platform)
            : base(platform)
        {
        }
    }

    [TestFixture(MobileTestPlatform.Android, Category = "Android")]
    [TestFixture(MobileTestPlatform.iOS, Category = "iOS")]
    public partial class AdminFeature : BaseTestFixture
    {
        public AdminFeature(MobileTestPlatform platform)
            : base(platform)
        {
        }
    }

    [TestFixture(MobileTestPlatform.Android, Category = "Android")]
    [TestFixture(MobileTestPlatform.iOS, Category = "iOS")] 
    public partial class VoucherIssueFeature : BaseTestFixture
    {
        public VoucherIssueFeature(MobileTestPlatform platform)
            : base(platform)
        {
        }
    }
}
