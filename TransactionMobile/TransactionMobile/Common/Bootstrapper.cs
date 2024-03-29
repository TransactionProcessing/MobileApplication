﻿namespace TransactionMobile.Common
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using Clients;
    using Database;
    using EstateManagement.Client;
    using EstateReporting.Client;
    using IntegrationTestClients;
    using Pages;
    using Pages.Admin;
    using Pages.Reporting;
    using Pages.Support;
    using Pages.Transactions;
    using Plugin.Toast;
    using Presenters;
    using SecurityService.Client;
    using Unity;
    using Unity.Lifetime;
    using ViewModels;
    using ViewModels.Reporting;
    using ViewModels.Transactions;
    using Views;
    using Views.Admin;
    using Views.Reporting;
    using Views.Support;
    using Views.Transactions;

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
            IUnityContainer container = new UnityContainer();

            Bootstrapper.RegisterClients(container);
            Bootstrapper.RegisterPresenters(container);
            Bootstrapper.RegisterViews(container);
            Bootstrapper.RegisterViewModels(container);

            return container;
        }

        #endregion

        private static void RegisterClients(IUnityContainer container)
        {
            if (App.IsIntegrationTestMode)
            {
                container.RegisterSingleton<IConfigurationServiceClient, TestConfigurationServiceClient>();
                container.RegisterSingleton<ISecurityServiceClient, TestSecurityServiceClient>();
                container.RegisterSingleton<ITransactionProcessorACLClient, TestTransactionProcessorACLClient>();
                container.RegisterSingleton<IEstateClient, TestEstateClient>();
                container.RegisterSingleton<IEstateReportingClient, TestEstateReportingClient>();
            }
            else
            {
                container.RegisterSingleton<IConfigurationServiceClient, ConfigurationServiceClient>();
                container.RegisterSingleton<ISecurityServiceClient, SecurityServiceClient>();
                container.RegisterSingleton<ITransactionProcessorACLClient, TransactionProcessorACLClient>();
                container.RegisterSingleton<IEstateClient, EstateClient>();
                container.RegisterSingleton<IEstateReportingClient, EstateReportingClient>();

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
                HttpClient httpClient = new HttpClient(httpClientHandler);
                container.RegisterInstance(httpClient);
                container.RegisterInstance<Func<String, String>>(
                new Func<String, String>(configSetting =>
                {
                    if (configSetting == "ConfigServiceUrl")
                    {
                        return "https://5r8nmm.deta.dev";
                    }

                    if (App.Configuration != null)
                    {
                        IConfiguration config = App.Configuration;

                        if (configSetting == "SecurityService")
                        {
                            return config.SecurityService;
                        }

                        if (configSetting == "TransactionProcessorACL")
                        {
                            return config.TransactionProcessorACL;
                        }

                        if (configSetting == "EstateManagementApi")
                        {
                            return config.EstateManagement;
                        }

                        if (configSetting == "EstateReportingApi")
                        {
                            return config.EstateReporting;
                        }

                        return string.Empty;
                    }

                    return string.Empty;
                }));
            }
        }

        private static void RegisterPresenters(IUnityContainer container)
        {
            container.RegisterType<ILoginPresenter, LoginPresenter>(new TransientLifetimeManager());
            container.RegisterType<ISupportPresenter, SupportPresenter>(new TransientLifetimeManager());
            container.RegisterType<ITransactionsPresenter, TransactionsPresenter>(new TransientLifetimeManager());
            container.RegisterType<IReportingPresenter, ReportingPresenter>(new TransientLifetimeManager());
        }

        private static void RegisterViews(IUnityContainer container)
        {
            // General
            container.RegisterType<IMainPage, MainPage>(new TransientLifetimeManager());
            container.RegisterType<ILoginPage, LoginPage>(new TransientLifetimeManager());
            container.RegisterType<ITestModePage, TestModePage>(new TransientLifetimeManager());
            container.RegisterType<ITransactionsPage, TransactionsPage>(new TransientLifetimeManager());
            container.RegisterType<IReportingPage, ReportingPage>(new TransientLifetimeManager());

            // Mobile Topup
            container.RegisterType<IMobileTopupSelectOperatorPage, MobileTopupSelectOperatorPage>(new TransientLifetimeManager());
            container.RegisterType<IMobileTopupSelectProductPage, MobileTopupSelectProductPage>(new TransientLifetimeManager());
            container.RegisterType<IMobileTopupPerformTopupPage, MobileTopupPerformTopupPage>(new TransientLifetimeManager());
            container.RegisterType<IMobileTopupPaymentSuccessPage, MobileTopupPaymentSuccessPage>(new TransientLifetimeManager());
            container.RegisterType<IMobileTopupPaymentFailedPage, MobileTopupPaymentFailedPage>(new TransientLifetimeManager());

            // Voucher
            container.RegisterType<IVoucherSelectOperatorPage, VoucherSelectOperatorPage>(new TransientLifetimeManager());
            container.RegisterType<IVoucherSelectProductPage, VoucherSelectProductPage>(new TransientLifetimeManager());
            container.RegisterType<IVoucherPerformVoucherIssuePage, VoucherPerformVoucherIssuePage>(new TransientLifetimeManager());
            container.RegisterType<IVoucherSuccessPage, VoucherSuccessPage>(new TransientLifetimeManager());
            container.RegisterType<IVoucherFailedPage, VoucherFailedPage>(new TransientLifetimeManager());

            // Reporting
            container.RegisterType<IMySettlementsListPage, MySettlementListPage>(new TransientLifetimeManager());
            container.RegisterType<IMySettlementsAnalysisPage, MySettlementAnalysisPage>(new TransientLifetimeManager());

            // Support
            container.RegisterType<ISupportPage, SupportPage>(new TransientLifetimeManager());

            // Admin
            container.RegisterType<IAdminPage, AdminPage>(new TransientLifetimeManager());
        }
        private static void RegisterViewModels(IUnityContainer container)
        {
            // General
            container.RegisterType<LoginViewModel>(new TransientLifetimeManager());
            container.RegisterType<MainPageViewModel>(new TransientLifetimeManager());
            container.RegisterType<TestModePageViewModel>(new TransientLifetimeManager());

            // Mobile Topup
            container.RegisterType<MobileTopupSelectOperatorViewModel>(new TransientLifetimeManager());
            container.RegisterType<MobileTopupSelectProductViewModel>(new TransientLifetimeManager());
            container.RegisterType<MobileTopupPerformTopupViewModel>(new TransientLifetimeManager());

            // Voucher
            container.RegisterType<VoucherSelectOperatorViewModel>(new TransientLifetimeManager());
            container.RegisterType<VoucherSelectProductViewModel>(new TransientLifetimeManager());
            container.RegisterType<VoucherPerformVoucherIssueViewModel>(new TransientLifetimeManager());

            // Settlement
            container.RegisterType<MySettlementListViewModel>(new TransientLifetimeManager());
            container.RegisterType<MySettlementAnalysisViewModel>(new TransientLifetimeManager());
        }
    }
}