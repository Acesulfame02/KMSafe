
namespace Safe.Pages
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        private async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            Preferences.Remove("IsLoggedIn");
            Preferences.Remove("UserEmail");
            await Shell.Current.GoToAsync("//LandPage");
        }
    }
}
