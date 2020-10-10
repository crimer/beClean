using beClean.DAL.DataServices;
using beClean.Services;
using beClean.Views.Master;
using Plugin.BluetoothClassic.Abstractions;
using Xamarin.Forms;

namespace beClean
{
    public partial class App : Application
    {
        //public static IBluetoothManagedConnection BltConnection { get; internal set; }
        public static IBluetoothAdapter BltAdapter { get; internal set; }
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
            InitBluetooth();
            MainPage = new AppMaster();

        }
        private void InitBluetooth()
        {
            App.BltAdapter = DependencyService.Resolve<IBluetoothAdapter>();
        }
        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
