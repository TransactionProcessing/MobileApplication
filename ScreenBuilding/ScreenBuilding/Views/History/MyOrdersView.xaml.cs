﻿using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace ScreenBuilding.Views.History
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyOrdersView : ContentView
    {
        public MyOrdersView()
        {
            InitializeComponent();
        }
    }
}