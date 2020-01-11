using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Unity;

namespace TransactionMobile
{
    using Presenters;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            this.BindingContext = this;
        }
    }

    public class MyTab : TabBar
    {
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == "CurrentItem")
            {
                var i = this.CurrentItem.Title;
                int index = this.Items.IndexOf(this.CurrentItem);
                if (index == 0)
                {
                    //handle the stuff
                    ITransactionsPresenter transactionsPresenter = App.Container.Resolve<ITransactionsPresenter>();
                    transactionsPresenter.Start();
                }

            }
        }
    }
}