using Firebase.Auth.Providers;
using Firebase.Auth;

namespace Safe.Services
{
    internal class KMSafeFireAuth
    {
        private string webApiKey = "AIzaSyDL7Os7A0tBIEhaT3CfCSnj1O7zoh7YLSE";
        private string authDomain = "km.safe.app";

        public async Task<FirebaseAuthClient> AuthTappedAsync()
        {
            try
            {
                var config = new FirebaseAuthConfig
                {
                    ApiKey = webApiKey,
                    AuthDomain = authDomain,
                    Providers = new FirebaseAuthProvider[]
                    {
                        new GoogleProvider().AddScopes("email"),
                        new EmailProvider()
                    }
                };
                return await Task.Run(() => new FirebaseAuthClient(config));
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
                throw;
            }
        }
    }
}
