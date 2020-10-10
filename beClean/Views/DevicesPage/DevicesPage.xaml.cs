using beClean.Views.Base;
using Plugin.BluetoothClassic.Abstractions;
using Xamarin.Forms.Xaml;

namespace beClean.Views.DevicesPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DevicesPage : BaseTabbedPage
    {
        public DevicesPage()
        {
            InitializeComponent();
            BindingContext = new DevicesPageVM();
            
        }
    }
}