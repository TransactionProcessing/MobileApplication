namespace TransactionMobile.Presenters
{
    using System.Threading.Tasks;
    using Syncfusion.XForms.TabView;

    /// <summary>
    /// 
    /// </summary>
    public interface IPresenter
    {
        #region Methods

        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <returns></returns>
        Task Start();

        #endregion
    }
}