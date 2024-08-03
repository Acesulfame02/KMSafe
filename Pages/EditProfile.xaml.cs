using CommunityToolkit.Maui.Views;
using Safe.ViewModel;

namespace Safe.Pages
{
    public partial class EditProfile : Popup
    {
        public EditProfile(ProfileViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
