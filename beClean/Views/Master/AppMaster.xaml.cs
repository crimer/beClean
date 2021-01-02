using beClean.Services.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace beClean.Views.Master
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppMaster : MasterDetailPage
    {
        public List<MasterPageItem> MenuItems { get; set; }
        public AppMaster()
        {
            InitializeComponent();

            MenuItems = new List<MasterPageItem>(new[]
            {
                new MasterPageItem { Id = 1, Title = "Обзор", Type = typeof(OverviewPage.OverviewPage), IconSource="resource://beClean.Resources.Svg.master.overview.svg" },
                new MasterPageItem { Id = 2, Title = "Комнаты", Type = typeof(RoomsPage.RoomsPage), IconSource="resource://beClean.Resources.Svg.master.rooms.svg" },
                new MasterPageItem { Id = 3, Title = "Устройства", Type = typeof(DevicesPage.DevicesPage), IconSource="resource://beClean.Resources.Svg.master.devices.svg" },
            });

            PageCollection.ItemsSource = MenuItems;
            // Первая страница что отобразится
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(OverviewPage.OverviewPage)));
        }

        private void PageCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (MasterPageItem)e.CurrentSelection[0];
            Type page = item.Type;

            Detail = new NavigationPage((Page)Activator.CreateInstance(page));
            IsPresented = false;
        }
    }
}