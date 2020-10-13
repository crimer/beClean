using beClean.Views.Base;
using Xamarin.Forms.Xaml;

namespace beClean.Views.History
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : BasePage
    {
        public HistoryPage()
        {
            InitializeComponent();
            BindingContext = new HistoryVM();
        }
    }
}