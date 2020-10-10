using beClean.Views.Base;
using Xamarin.Forms.Xaml;

namespace beClean.Views.DevicesPage.DeviceBC
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeviceBC : BasePage
    {
        public DeviceBC()
        {
            InitializeComponent();
            BindingContext = new DeviceBCVM();
        }
    }
}