using Safe.Model;
using Safe.Services;
using Safe.ViewModel;

namespace Safe.Pages;

public partial class LoginPage : ContentPage
{
    private AuthViewModel _viewModel;
    public LoginPage()
	{
		InitializeComponent();
        _viewModel = new AuthViewModel(Navigation, new KMSafeFireAuth());
    }

	private async void OnLogin(object sender, EventArgs e)
	{
		var model = new AuthFireModel
		{
			Username = Username.Text,
			Password = Password.Text
		};

        await _viewModel.LoginBtnTappedAsync(model);
    }

	private async void OnRegister(object sender, EventArgs e)
	{
        await Shell.Current.GoToAsync("//RegisterPage");
    }
}