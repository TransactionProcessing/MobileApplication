using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionMobile.ViewModels
{
    using Xamarin.Forms;

    public class MainPageViewModel : BindableObject
    {
        public MainPageViewModel()
        {
            this.availableBalance = "0 KES";
        }

        private String availableBalance;
        public String AvailableBalance
        {
            get
            {
                return this.availableBalance;
            }
            set
            {
                this.availableBalance = value;
                this.OnPropertyChanged(nameof(this.AvailableBalance));
            }
        }
    }

    public class TestModePageViewModel : BindableObject
    {
        public TestModePageViewModel()
        {
        }

        private string pinNumber;

        private string testContractData;
        private string testMerchantData;
        private string testSettlementData;
        public String PinNumber
        {
            get
            {
                return this.pinNumber;
            }
            set
            {
                this.pinNumber = value;
                this.OnPropertyChanged(nameof(this.PinNumber));
            }
        }

        public String TestContractData
        {
            get
            {
                return this.testContractData;
            }
            set
            {
                this.testContractData = value;
                this.OnPropertyChanged(nameof(this.TestContractData));
            }
        }

        public String TestMerchantData
        {
            get
            {
                return this.testMerchantData;
            }
            set
            {
                this.testMerchantData = value;
                this.OnPropertyChanged(nameof(this.TestMerchantData));
            }
        }

        public String TestSettlementData
        {
            get
            {
                return this.testSettlementData;
            }
            set
            {
                this.testSettlementData = value;
                this.OnPropertyChanged(nameof(this.TestSettlementData));
            }
        }
    }
}
