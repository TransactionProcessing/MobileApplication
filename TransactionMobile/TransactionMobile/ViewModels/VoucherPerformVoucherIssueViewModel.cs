using System;
using System.Collections.Generic;
using System.Text;
using TransactionMobile.Models;
using Xamarin.Forms;

namespace TransactionMobile.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Xamarin.Forms.BindableObject" />
    public class VoucherPerformVoucherIssueViewModel : BindableObject
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
        /// The recipient email address
        /// </summary>
        private String recipientEmailAddress;

        /// <summary>
        /// The customer mobile number
        /// </summary>
        private String recipientMobileNumber;

        /// <summary>
        /// The operator name
        /// </summary>
        private String operatorName;

        /// <summary>
        /// The topup amount
        /// </summary>
        private Decimal voucherAmount;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the recipient mobile number.
        /// </summary>
        /// <value>
        /// The recipient mobile number.
        /// </value>
        public String RecipientMobileNumber
        {
            get
            {
                return this.recipientMobileNumber;
            }
            set
            {
                this.recipientMobileNumber = value;
                this.OnPropertyChanged(nameof(this.RecipientMobileNumber));
            }
        }

        /// <summary>
        /// Gets or sets the recipient email address.
        /// </summary>
        /// <value>
        /// The recipient email address.
        /// </value>
        public String RecipientEmailAddress
        {
            get
            {
                return this.recipientEmailAddress;
            }
            set
            {
                this.recipientEmailAddress = value;
                this.OnPropertyChanged(nameof(this.RecipientEmailAddress));
            }
        }

        /// <summary>
        /// Gets or sets the customer email address.
        /// </summary>
        /// <value>
        /// The customer email address.
        /// </value>
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
        public Decimal VoucherAmount
        {
            get
            {
                return this.voucherAmount;
            }
            set
            {
                this.voucherAmount = value;
                this.OnPropertyChanged(nameof(this.VoucherAmount));
            }
        }

        #endregion
    }
}
