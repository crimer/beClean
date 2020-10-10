using beClean.Views.Base;
using Xamarin.Forms.Xaml;

namespace beClean.Views.DevicesPage.DeviceBLE
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeviceBLE : BasePage
    {
        public DeviceBLE()
        {
            InitializeComponent();
            BindingContext = new DeviceBLEVM();
        }
    }
}