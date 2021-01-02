using beClean.Services.DataServices;
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
            InitServices();
            MainPage = new AppMaster();

        }
        private void InitServices()
        {
            DataServices.BClassic.BltAdapter = DependencyService.Resolve<IBluetoothAdapter>();
        }
        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
