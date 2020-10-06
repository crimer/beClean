using beClean.Services;
using beClean.Views.Master;
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
            //DependencyService.Register<MockDataStore>();
            MainPage = new AppMaster();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
