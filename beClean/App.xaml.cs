using beClean.Services;
using beClean.Services.DataServices;
using beClean.Services.DataServices.Notifications;
using beClean.Views.Master;
using Plugin.BluetoothClassic.Abstractions;
using Xamarin.Forms;

namespace beClean
{
    public partial class App : Application
    {
        public App()
        {
            // Fix ios crash
            Current.MainPage = new ContentPage();
        }

        protected override void OnStart()
        {
            InitializeComponent();
            DataServices.Init();
            NavigationService.Init();
            InitServices();
            MainPage = new AppMaster();

        }
        private void InitServices()
        {
            DataServices.BClassic.BltAdapter = DependencyService.Resolve<IBluetoothAdapter>();
            DataServices.Notifications = DependencyService.Get<INotificationService>();
        }
        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
