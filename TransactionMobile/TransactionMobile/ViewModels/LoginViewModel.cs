namespace TransactionMobile.ViewModels
{
    using System;
    using Xamarin.Forms;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Xamarin.Forms.BindableObject" />
    public class LoginViewModel : BindableObject
    {
        #region Fields

        /// <summary>
        /// The email address
        /// </summary>
        private String emailAddress;

        /// <summary>
        /// The password
        /// </summary>
        private String password;

        /// <summary>
        /// The label
        /// </summary>
        private String label;

        #endregion

        public LoginViewModel()
        {
            this.Label = "Debug";
        }

        #region Properties

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        public String EmailAddress
        {
            get
            {
                return this.emailAddress;
            }
            set
            {
                this.emailAddress = value;
                this.OnPropertyChanged(nameof(this.EmailAddress));
            }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public String Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
                this.OnPropertyChanged(nameof(this.Password));
            }
        }

        public String Label
        {
            get
            {
                return this.label;
            }
            set
            {
                this.label = value;
                this.OnPropertyChanged(nameof(this.Label));
            }
        }

        #endregion
    }
}