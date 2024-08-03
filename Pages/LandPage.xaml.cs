using Safe.ViewModel;
namespace Safe.Pages
{
    public partial class LandPage : ContentPage
    {
        public LandPage()
        {
            InitializeComponent();
            BindingContext = new LandViewModel(Navigation);
        }
    }

}
