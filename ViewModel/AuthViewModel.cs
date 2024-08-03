using Safe.Model;
using Safe.Services;

namespace Safe.ViewModel
{
    internal class AuthViewModel
    {
        private KMSafeFireAuth _auth;
        private INavigation _navigation;

        public AuthViewModel(INavigation navigation, KMSafeFireAuth auth)
        {
            _navigation = navigation;
            _auth = auth;
        }

        public async Task LoginBtnTappedAsync(AuthFireModel model)
        {
            try
            {
                var client = await _auth.AuthTappedAsync();
                var userCredential = await client.SignInWithEmailAndPasswordAsync(model.Username, model.Password);
                var user = userCredential.User;
                // Store login state and email in preferences
                Preferences.Set("IsLoggedIn", true);
                Preferences.Set("UserEmail", user.Info.Email);

                await Shell.Current.GoToAsync("//WelcomePage");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
                throw;
            }
        }

        public async Task RegisterUserTappedAsync(AuthFireModel model)
        {
            try
            {
                var client = await _auth.AuthTappedAsync();
                var userCredential = await client.CreateUserWithEmailAndPasswordAsync(model.Username, model.Password);
                var user = userCredential.User;
                // Store login state and email in preferences
                Preferences.Set("IsLoggedIn", true);
                Preferences.Set("UserEmail", user.Info.Email);

                await Shell.Current.GoToAsync("//WelcomePage");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
                throw;
            }
        }
    }
}
