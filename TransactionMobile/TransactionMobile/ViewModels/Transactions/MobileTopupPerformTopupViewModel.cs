namespace TransactionMobile.ViewModels.Transactions
{
    using System;
    using Models;
    using Xamarin.Forms;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Xamarin.Forms.BindableObject" />
    public class MobileTopupPerformTopupViewModel : BindableObject
    {
        #region Fields

        /// <summary>
        /// Gets or sets the contract product model.
        /// </summary>
        /// <value>
        /// The contract product model.
        /// </value>
        public ContractProductModel ContractProductModel { get; set; }

        /// <summary>
        /// The customer email address
        /// </summary>
        private String customerEmailAddress;

        /// <summary>
        /// The customer mobile number
        /// </summary>
        private String customerMobileNumber;

        /// <summary>
        /// The operator name
        /// </summary>
        private String operatorName;

        /// <summary>
        /// The topup amount
        /// </summary>
        private Decimal topupAmount;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the customer mobile number.
        /// </summary>
        /// <value>
        /// The customer mobile number.
        /// </value>
        public String CustomerMobileNumber
        {
            get
            {
                return this.customerMobileNumber;
            }
            set
            {
                this.customerMobileNumber = value;
                this.OnPropertyChanged(nameof(this.CustomerMobileNumber));
            }
        }

        public String CustomerEmailAddress
        {
            get
            {
                return this.customerEmailAddress;
            }
            set
            {
                this.customerEmailAddress = value;
                this.OnPropertyChanged(nameof(this.CustomerEmailAddress));
            }
        }

        /// <summary>
        /// Gets or sets the name of the operator.
        /// </summary>
        /// <value>
        /// The name of the operator.
        /// </value>
        public String OperatorName
        {
            set
            {
                this.operatorName = value;
            }
            get
            {
                return this.operatorName;
            }
        }

        /// <summary>
        /// Gets or sets the topup amount.
        /// </summary>
        /// <value>
        /// The topup amount.
        /// </value>
        public Decimal TopupAmount
        {
            get
            {
                return this.topupAmount;
            }
            set
            {
                this.topupAmount = value;
                this.OnPropertyChanged(nameof(this.TopupAmount));
            }
        }

        #endregion
    }
}