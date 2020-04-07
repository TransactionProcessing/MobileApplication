using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.IntegrationTests.Features
{
    using Common;
    using NUnit.Framework;
    using Xamarin.UITest;

    [TestFixture(Platform.Android, Category = "Android")]
    [TestFixture(Platform.iOS, Category = "iOS")]
    public partial class LoginFeature : BaseTestFixture
    {
        public LoginFeature(Platform platform)
            : base(platform)
        {
        }
    }

    [TestFixture(Platform.Android, Category = "Android")]
    [TestFixture(Platform.iOS, Category = "iOS")]
    public partial class SafaricomTopupFeature : BaseTestFixture
    {
        public SafaricomTopupFeature(Platform platform)
            : base(platform)
        {
        }
    }
}
