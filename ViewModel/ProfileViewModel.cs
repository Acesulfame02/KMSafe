using Safe.Model;
using Safe.Services;
using System.ComponentModel;
using Safe.Pages;
using CommunityToolkit.Maui.Views;
using System.Windows.Input;

namespace Safe.ViewModel
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private ProfileModel _profile;
        private readonly ProfileFireService _profileFireService;
        private Popup _currentPopup;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ProfileModel Profile
        {
            get => _profile;
            set
            {
                _profile = value;
                OnPropertyChanged(nameof(Profile));
            }
        }

        public ICommand EditProfileCommand { get; }
        public ICommand SubmitUpdateCommand { get; }

        public ProfileViewModel()
        {
            _profileFireService = new ProfileFireService();
            Profile = new ProfileModel();
            EditProfileCommand = new Command(OpenEditProfilePopup);
            SubmitUpdateCommand = new Command(async () => await SubmitUpdateAsync());
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            var userId = Preferences.Get("UserEmail", string.Empty);
            if (!string.IsNullOrEmpty(userId))
            {
                Profile = await _profileFireService.GetUserProfileAsync(userId);
            }
        }

        private async Task SubmitUpdateAsync()
        {
            var userId = Preferences.Get("UserEmail", string.Empty);
            if (!string.IsNullOrEmpty(userId))
            {
                await _profileFireService.UpdateUserProfileAsync(userId, Profile);
                await Application.Current.MainPage.DisplayAlert("Success", "Profile updated successfully.", "OK");

                // Close the popup after updating
                _currentPopup?.Close();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "User not logged in.", "OK");
            }
        }

        private void OpenEditProfilePopup()
        {
            var popup = new EditProfile(this);
            _currentPopup = popup; // Store reference to the current popup
            Application.Current.MainPage.ShowPopup(popup);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
