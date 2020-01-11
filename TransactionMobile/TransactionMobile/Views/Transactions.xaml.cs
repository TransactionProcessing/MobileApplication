using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TransactionMobile.Views
{
    using Pages;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransactionsPage : ContentPage, ITransactionsPage, IPage
    {
        public TransactionsPage()
        {
            InitializeComponent();
        }

        public void Init()
        {
            
        }
    }
}