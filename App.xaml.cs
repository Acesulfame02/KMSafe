namespace Safe
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            base.OnStart();

            // Check if the user is logged in
            bool isLoggedIn = Preferences.Get("IsLoggedIn", false);

            if (isLoggedIn)
            {
                // Navigate to Welcome Page
                Shell.Current.GoToAsync("//WelcomePage");
            }
            else
            {
                // Navigate to Login Page
                Shell.Current.GoToAsync("//LoginPage");
            }
        }
    }
}
