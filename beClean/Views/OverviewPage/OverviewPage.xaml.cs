
using beClean.Views.Base;
using Xamarin.Forms.Xaml;

namespace beClean.Views.OverviewPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OverviewPage : BasePage
    {
        public OverviewPage()
        {
            InitializeComponent();
            BindingContext = new OverviewPageVM();
        }
    }
}