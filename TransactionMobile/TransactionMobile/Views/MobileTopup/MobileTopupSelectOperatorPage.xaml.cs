namespace TransactionMobile.Views.MobileTopup
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Database;
    using Models;
    using Pages;
    using Syncfusion.XForms.Buttons;
    using ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Xamarin.Forms.ContentPage" />
    /// <seealso cref="TransactionMobile.Pages.IMobileTopupSelectOperatorPage" />
    /// <seealso cref="TransactionMobile.Pages.IPage" />
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [ExcludeFromCodeCoverage]
    public partial class MobileTopupSelectOperatorPage : ContentPage, IMobileTopupSelectOperatorPage, IPage
    {
        private readonly ILoggingDatabaseContext LoggingDatabase;

        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileTopupSelectOperatorPage" /> class.
        /// </summary>
        /// <param name="analysisLogger">The analysis logger.</param>
        public MobileTopupSelectOperatorPage(ILoggingDatabaseContext loggingDatabase)
        {
            this.LoggingDatabase = loggingDatabase;
            this.LoggingDatabase.InsertLogMessage(LoggingDatabaseContext.CreateDebugLogMessage($"In {this.GetType().Name} ctor"));
            this.InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when [operator selected].
        /// </summary>
        public event EventHandler<SelectedItemChangedEventArgs> OperatorSelected;

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the specified view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public void Init(MobileTopupSelectOperatorViewModel viewModel)
        {
            this.LoggingDatabase.InsertLogMessage(LoggingDatabaseContext.CreateDebugLogMessage($"In {this.GetType().Name} Init"));
            this.LoadOperators(viewModel);
        }

        /// <summary>
        /// Executes the specified e.
        /// </summary>
        /// <param name="e">The <see cref="SelectedItemChangedEventArgs" /> instance containing the event data.</param>
        private void Execute(SelectedItemChangedEventArgs e)
        {
            this.OperatorSelected(this, e);
        }

        /// <summary>
        /// Loads the operators.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        private void LoadOperators(MobileTopupSelectOperatorViewModel viewModel)
        {
            RowDefinitionCollection rowDefinitionCollection = new RowDefinitionCollection();
            for (Int32 i = 0; i < viewModel.Operators.Count; i++)
            {
                rowDefinitionCollection.Add(new RowDefinition
                                            {
                                                Height = 80
                                            });
            }

            this.OperatorsGrid.RowDefinitions = rowDefinitionCollection;

            Int32 rowCount = 0;
            foreach (ContractProductModel modelOperator in viewModel.Operators)
            {
                SfButton button = new SfButton
                                  {
                                      Text = modelOperator.OperatorName,
                                      HorizontalOptions = LayoutOptions.FillAndExpand,
                                      //HeightRequest = 100,
                                      //WidthRequest = 400,
                                      AutomationId = modelOperator.OperatorName,
                                      Command = new Command<SelectedItemChangedEventArgs>(this.Execute),
                                      CommandParameter = new SelectedItemChangedEventArgs(modelOperator, rowCount)
                                  };
                button.SetDynamicResource(VisualElement.StyleProperty, "SfButtonStyleMobileTopup");

                this.OperatorsGrid.Children.Add(button, 0, rowCount);
                rowCount++;
            }
        }

        #endregion
    }
}