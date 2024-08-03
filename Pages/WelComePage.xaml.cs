using Safe.ViewModel;

namespace Safe.Pages;

public partial class WelComePage : ContentPage
{
	public WelComePage()
	{
		InitializeComponent();
    }

    private async void EmergencyContactClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//EmergencyContactsPage");
    }
}