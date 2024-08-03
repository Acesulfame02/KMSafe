using Safe.Pages;
namespace Safe.ViewModel
{
    internal class LandViewModel
    {
        private INavigation _navigation;
        public Command LoginBtn { get; }
        public Command RegisterBtn { get; }
        public LandViewModel(INavigation navigation)
        {
            _navigation = navigation;
            LoginBtn = new Command(LoginBtnTappedAsync);
            RegisterBtn = new Command(RegisterBtnTappedAsync);
        }

        public async void LoginBtnTappedAsync(object obj)
        {
            await _navigation.PushAsync(new LoginPage());
        }

        public async void RegisterBtnTappedAsync(object obj)
        {
            await _navigation.PushAsync(new RegisterPage());
        }
    }
}
