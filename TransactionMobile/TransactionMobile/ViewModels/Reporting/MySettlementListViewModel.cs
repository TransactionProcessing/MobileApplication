using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionMobile.ViewModels.Reporting
{
    using System.ComponentModel;
    using Xamarin.Forms;

    public class MySettlementListViewModel
    {
        public List<SettlementListItem> SettlementListItems { get; set; }
    }

    public class SettlementListItem : INotifyPropertyChanged
    {
        private String automationId;
        private DateTime settlementDate;

        private Guid settlementId;

        private Decimal value;

        private Int32 numberOfFeesSettled;

        private Boolean isComplete;

        public String AutomationId
        {
            get
            {
                return $"{this.automationId}";
            }
            set
            {
                this.automationId = value;
                OnPropertyChanged("AutomationId");
            }
        }

        public Boolean IsComplete
        {
            get
            {
                return this.isComplete;
            }
            set
            {
                this.isComplete = value;
                OnPropertyChanged("IsComplete");
            }
        }

        public DateTime SettlementDate
        {
            get
            {
                return this.settlementDate;
            }
            set
            {
                this.settlementDate = value;
                OnPropertyChanged("SettlementDate");
            }
        }

        public Guid SettlementId
        {
            get
            {
                return this.settlementId;
            }
            set
            {
                this.settlementId = value;
                OnPropertyChanged("SettlementId");
            }
        }

        public Decimal Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
                OnPropertyChanged("Value");
            }
        }

        public Int32 NumberOfFeesSettled
        {
            get
            {
                return this.numberOfFeesSettled;
            }
            set
            {
                this.numberOfFeesSettled = value;
                OnPropertyChanged("NumberOfFeesSettled");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }

    public class MySettlementAnalysisViewModel
    {
        public List<SettlementFeeListItem> SettlementFeeListItems { get; set; }
    }

    public class SettlementFeeListItem : INotifyPropertyChanged
    {
        private Guid settlementId;

        private DateTime settlementDate;
        private Decimal calculatedValue;

        private String feeDescription;

        private Boolean isSettled;

        private Guid transactionId;

        private String operatorIdentifier;

        public Guid SettlementId
        {
            get
            {
                return this.settlementId;
            }
            set
            {
                this.settlementId = value;
                OnPropertyChanged("SettlementId");
            }
        }

        public DateTime SettlementDate
        {
            get
            {
                return this.settlementDate;
            }
            set
            {
                this.settlementDate = value;
                OnPropertyChanged("SettlementDate");
            }
        }

        public String OperatorIdentifier
        {
            get
            {
                return this.operatorIdentifier;
            }
            set
            {
                this.operatorIdentifier = value;
                OnPropertyChanged("OperatorIdentifier");
            }
        }

        public Guid TransactionId
        {
            get
            {
                return this.transactionId;
            }
            set
            {
                this.transactionId = value;
                OnPropertyChanged("TransactionId");
            }
        }

        public Decimal CalculatedValue
        {
            get
            {
                return this.calculatedValue;
            }
            set
            {
                this.calculatedValue = value;
                OnPropertyChanged("CalculatedValue");
            }
        }

        public String FeeDescription
        {
            get
            {
                return this.feeDescription;
            }
            set
            {
                this.feeDescription = value;
                OnPropertyChanged("FeeDescription");
            }
        }

        public Boolean IsSettled
        {
            get
            {
                return this.isSettled;
            }
            set
            {
                this.isSettled = value;
                OnPropertyChanged("IsSettled");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }

}
