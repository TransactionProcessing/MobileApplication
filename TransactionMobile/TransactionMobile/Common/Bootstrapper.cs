namespace TransactionMobile.Common
{
    using System;
    using System.Net.Http;
    using Pages;
    using Plugin.Toast;
    using Presenters;
    using SecurityService.Client;
    using Unity;
    using Unity.Injection;
    using Unity.Lifetime;
    using ViewModels;
    using Views;

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

            // Common Registrations
            unityContainer.RegisterType<ISecurityServiceClient, SecurityServiceClient>(new SingletonLifetimeManager());
            HttpClient httpClient = new HttpClient();
            unityContainer.RegisterInstance(httpClient, new SingletonLifetimeManager());
            unityContainer.RegisterType<Func<String, String>>(new InjectionFactory(c => new Func<String, String>(configSetting =>
                                                                                                                 {
                                                                                                                     if (App.Configuration != null)
                                                                                                                     {

                                                                                                                         IConfiguration config = App.Configuration;

                                                                                                                         //if (configSetting == "ManagementAPI")
                                                                                                                         //{
                                                                                                                         //    return config.TransactionProcessorACL;
                                                                                                                         //}

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

            // View registrations
            unityContainer.RegisterType<ILoginPage, LoginPage>(new TransientLifetimeManager());
            unityContainer.RegisterType<ITransactionsPage, TransactionsPage>(new TransientLifetimeManager());

            // View model registrations
            unityContainer.RegisterType<LoginViewModel>(new TransientLifetimeManager());

            return unityContainer;
        }

        #endregion
    }
}