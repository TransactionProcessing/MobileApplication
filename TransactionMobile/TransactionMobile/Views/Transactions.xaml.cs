using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TransactionMobile.Views
{
    using Common;
    using Pages;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransactionsPage : ContentPage, ITransactionsPage, IPage
    {
        private readonly IDevice Device;

        public TransactionsPage(IDevice device)
        {
            this.Device = device;
            InitializeComponent();
            this.Device.AddDebugInformation("In TransactionsPage ctor");
        }

        public void Init()
        {
            this.Device.AddDebugInformation("In TransactionsPage init");
        }
    }
}