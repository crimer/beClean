using beClean.Views.Base;
using Xamarin.Forms.Xaml;

namespace beClean.Views.DevicesPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DevicesPage : BasePage
    {
        public DevicesPage()
        {
            InitializeComponent();
            BindingContext = new DevicesPageVM();
            
        }
    }
}