namespace TransactionMobile.Common
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Net.Http;
    using Database;
    using EstateManagement.Client;
    using Pages;
    using Plugin.Toast;
    using Presenters;
    using SecurityService.Client;
    using Services;
    using Unity;
    using Unity.Injection;
    using Unity.Lifetime;
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
        public static IUnityContainer Run()
        {
            UnityContainer unityContainer = new UnityContainer();
            String dbConnectionString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "TransactionProcessing.db");
            // Common Registrations
            //unityContainer.RegisterType<ILoggingDatabaseContext, LoggingDatabaseContext>(new SingletonLifetimeManager());
            //unityContainer.RegisterFactory<Func<String>>((c) => { return dbConnectionString; });

            unityContainer.RegisterType<ISecurityServiceClient, SecurityServiceClient>(new SingletonLifetimeManager());
            unityContainer.RegisterType<ITransactionProcessorACLClient, TransactionProcessorACLClient>(new SingletonLifetimeManager());
            unityContainer.RegisterType<IEstateClient, EstateClient>(new SingletonLifetimeManager());
            unityContainer.RegisterType<IConfigurationServiceClient, ConfigurationServiceClient>(new SingletonLifetimeManager());
            HttpClient httpClient = new HttpClient();
            unityContainer.RegisterInstance(httpClient, new SingletonLifetimeManager());
            unityContainer.RegisterType<Func<String, String>>(new InjectionFactory(c => new Func<String, String>(configSetting =>
                                                                                                                 {
                                                                                                                     if (configSetting == "ConfigServiceUrl")
                                                                                                                     {
                                                                                                                         return "https://5r8nmm.deta.dev";
                                                                                                                         //return "http://192.168.1.67:1337";
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
                                                                                                                 })));

            // Presenter registrations
            unityContainer.RegisterType<ILoginPresenter, LoginPresenter>(new TransientLifetimeManager());
            unityContainer.RegisterType<ITransactionsPresenter, TransactionsPresenter>(new TransientLifetimeManager());
            unityContainer.RegisterType<ISupportPresenter, SupportPresenter>(new TransientLifetimeManager());

            // View registrations
            unityContainer.RegisterType<IMainPage, MainPage>(new TransientLifetimeManager());
            unityContainer.RegisterType<ILoginPage, LoginPage>(new TransientLifetimeManager());
            unityContainer.RegisterType<ITransactionsPage, TransactionsPage>(new TransientLifetimeManager());
            // Mobile Topup
            unityContainer.RegisterType<IMobileTopupSelectOperatorPage, MobileTopupSelectOperatorPage>(new TransientLifetimeManager());
            unityContainer.RegisterType<IMobileTopupSelectProductPage, MobileTopupSelectProductPage>(new TransientLifetimeManager());
            unityContainer.RegisterType<IMobileTopupPerformTopupPage, MobileTopupPerformTopupPage>(new TransientLifetimeManager());
            unityContainer.RegisterType<IMobileTopupPaymentSuccessPage, MobileTopupPaymentSuccessPage>(new TransientLifetimeManager());
            unityContainer.RegisterType<IMobileTopupPaymentFailedPage, MobileTopupPaymentFailedPage>(new TransientLifetimeManager());

            // Voucher
            unityContainer.RegisterType<IVoucherSelectOperatorPage, VoucherSelectOperatorPage>(new TransientLifetimeManager());
            unityContainer.RegisterType<IVoucherSelectProductPage, VoucherSelectProductPage>(new TransientLifetimeManager());
            unityContainer.RegisterType<IVoucherPerformVoucherIssuePage, VoucherPerformVoucherIssuePage>(new TransientLifetimeManager());
            unityContainer.RegisterType<IVoucherSuccessPage, VoucherSuccessPage>(new TransientLifetimeManager());
            unityContainer.RegisterType<IVoucherFailedPage, VoucherFailedPage>(new TransientLifetimeManager());

            // Support
            unityContainer.RegisterType<ISupportPage, SupportPage>(new TransientLifetimeManager());
            
            // Admin
            unityContainer.RegisterType<IAdminPage, AdminPage>(new TransientLifetimeManager());

            // View model registrations
            unityContainer.RegisterType<LoginViewModel>(new TransientLifetimeManager());
            unityContainer.RegisterType<MainPageViewModel>(new TransientLifetimeManager());

            // Mobile Topup
            unityContainer.RegisterType<MobileTopupSelectOperatorViewModel>(new TransientLifetimeManager());
            unityContainer.RegisterType<MobileTopupSelectProductViewModel>(new TransientLifetimeManager());
            unityContainer.RegisterType<MobileTopupPerformTopupViewModel>(new TransientLifetimeManager());

            // Voucher
            unityContainer.RegisterType<VoucherSelectOperatorViewModel>(new TransientLifetimeManager());
            unityContainer.RegisterType<VoucherSelectProductViewModel>(new TransientLifetimeManager());
            unityContainer.RegisterType<VoucherPerformVoucherIssueViewModel>(new TransientLifetimeManager());

            return unityContainer;
        }

        #endregion
    }
}