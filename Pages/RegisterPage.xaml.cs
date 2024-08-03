using Safe.Model;
using Safe.Services;
using Safe.ViewModel;

namespace Safe.Pages;

public partial class RegisterPage : ContentPage
{
    private AuthViewModel _viewModel;
    public RegisterPage()
	{
		InitializeComponent();
        _viewModel = new AuthViewModel(Navigation, new KMSafeFireAuth());
    }

    private async void OnRegister(object sender, EventArgs e)
    {
        var model = new AuthFireModel
        {
            Username = Email.Text,
            Password = Password.Text
        };

        await _viewModel.RegisterUserTappedAsync(model);
    }

    private async void OnLogin(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
}