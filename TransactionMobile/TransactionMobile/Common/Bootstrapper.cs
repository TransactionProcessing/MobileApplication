namespace TransactionMobile.Common
{
    using Pages;
    using Presenters;
    using Unity;
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
            unityContainer.RegisterType<IConfiguration, DevelopmentConfiguration>();

            // Presenter registrations
            unityContainer.RegisterType<ILoginPresenter, LoginPresenter>(new TransientLifetimeManager());

            // View registrations
            unityContainer.RegisterType<ILoginPage, LoginPage>(new TransientLifetimeManager());

            // View model registrations
            unityContainer.RegisterType<LoginViewModel>(new TransientLifetimeManager());

            return unityContainer;
        }

        #endregion
    }
}