namespace TransactionMobile.Common
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using Database;
    using EstateManagement.Client;
    using Pages;
    using Plugin.Toast;
    using Presenters;
    using SecurityService.Client;
    using Services;
    using StructureMap;
    using ViewModels;
    using Views;
    using Views.Admin;
    using Views.MobileTopup;
    using Views.Voucher;

    [ExcludeFromCodeCoverage]
    public class Bootstrapper
    {
        #region Methods

        /// <summary>
        /// Runs this instance.
        /// </summary>
        /// <returns></returns>
        public static IContainer Run()
        {
            Registry registry = new Registry();
            registry.IncludeRegistry<ClientsRegistry>();
            registry.IncludeRegistry<PresenterRegistry>();
            registry.IncludeRegistry<ViewRegistry>();
            registry.IncludeRegistry<ViewModelRegistry>();
            IContainer container = new Container(registry);
            
            return container;
        }

        #endregion
    }

    public class ClientsRegistry : Registry
    {
        public ClientsRegistry()
        {
            this.For<ISecurityServiceClient>().Use<SecurityServiceClient>().Singleton();
            this.For<ITransactionProcessorACLClient>().Use<TransactionProcessorACLClient>().Singleton();
            this.For<IEstateClient>().Use<EstateClient>().Singleton();
            this.For<IConfigurationServiceClient>().Use<ConfigurationServiceClient>().Singleton();
            HttpClientHandler httpClientHandler = new HttpClientHandler
                                                  {
                                                      ServerCertificateCustomValidationCallback = (message,
                                                                                                   certificate2,
                                                                                                   arg3,
                                                                                                   arg4) =>
                                                                                                  {
                                                                                                      return true;
                                                                                                  }
                                                  };
            this.For<HttpClient>().Add(new HttpClient(httpClientHandler));
            this.For<Func<String, String>>().Add(new Func<String, String>(configSetting =>
                                                                          {
                                                                              if (configSetting == "ConfigServiceUrl")
                                                                              {
                                                                                  return "https://5r8nmm.deta.dev";
                                                                              }

                                                                              if (App.Configuration != null)
                                                                              {
                                                                                  IConfiguration config = App.Configuration;

                                                                                  if (configSetting == "TransactionProcessorACL")
                                                                                  {
                                                                                      return config.TransactionProcessorACL;
                                                                                  }

                                                                                  if (configSetting == "EstateManagementApi")
                                                                                  {
                                                                                      return config.EstateManagement;
                                                                                  }

                                                                                  if (configSetting == "SecurityService")
                                                                                  {
                                                                                      return config.SecurityService;
                                                                                  }

                                                                                  return string.Empty;
                                                                              }

                                                                              return string.Empty;
                                                                          }));
        }
    }

    public class PresenterRegistry : Registry
    {
        public PresenterRegistry()
        {
            this.For<ILoginPresenter>().Add<LoginPresenter>().Transient();
            this.For<ITransactionsPresenter>().Add<TransactionsPresenter>().Transient();
            this.For<ISupportPresenter>().Add<SupportPresenter>().Transient();
        }
    }
    public class ViewRegistry : Registry
    {
        public ViewRegistry()
        {
            // General
            this.For<IMainPage>().Use<MainPage>().Transient();
            this.For<ILoginPage>().Use<LoginPage>().Transient();
            this.For<ITransactionsPage>().Use<TransactionsPage>().Transient();

            // Mobile Topup
            this.For<IMobileTopupSelectOperatorPage>().Use<MobileTopupSelectOperatorPage>().Transient();
            this.For<IMobileTopupSelectProductPage>().Use<MobileTopupSelectProductPage>().Transient();
            this.For<IMobileTopupPerformTopupPage>().Use<MobileTopupPerformTopupPage>().Transient();
            this.For<IMobileTopupPaymentSuccessPage>().Use<MobileTopupPaymentSuccessPage>().Transient();
            this.For<IMobileTopupPaymentFailedPage>().Use<MobileTopupPaymentFailedPage>().Transient();

            // Voucher
            this.For<IVoucherSelectOperatorPage>().Use<VoucherSelectOperatorPage>().Transient();
            this.For<IVoucherSelectProductPage>().Use<VoucherSelectProductPage>().Transient();
            this.For<IVoucherPerformVoucherIssuePage>().Use<VoucherPerformVoucherIssuePage>().Transient();
            this.For<IVoucherSuccessPage>().Use<VoucherSuccessPage>().Transient();
            this.For<IVoucherFailedPage>().Use<VoucherFailedPage>().Transient();

            // Support
            this.For<ISupportPage>().Use<SupportPage>().Transient();
            
            // Admin
            this.For<IAdminPage>().Use<AdminPage>().Transient();
        }
    }

    public class ViewModelRegistry : Registry
    {
        public ViewModelRegistry()
        {
            // General
            this.For<LoginViewModel>().Transient();
            this.For<MainPageViewModel>().Transient();

            // Mobile Topup
            this.For<MobileTopupSelectOperatorViewModel>().Transient();
            this.For<MobileTopupSelectProductViewModel>().Transient();
            this.For<MobileTopupPerformTopupViewModel>().Transient();

            // Voucher
            this.For<VoucherSelectOperatorViewModel>().Transient();
            this.For<VoucherSelectProductViewModel>().Transient();
            this.For<VoucherPerformVoucherIssueViewModel>().Transient();
        }
    }
}