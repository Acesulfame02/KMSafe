using Safe.Model;
using Safe.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Safe.ViewModel
{
    public class EmergencyContactsViewModel : BindableObject
    {
        private readonly FirebaseEmergencyContactService _firebaseService;
        private EmergencyContactModel _selectedHostelContact;
        private EmergencyContactModel _selectedClinicContact;

        public ObservableCollection<EmergencyContactModel> HostelContacts { get; }
        public ObservableCollection<EmergencyContactModel> ClinicContacts { get; }

        public EmergencyContactModel SelectedHostelContact
        {
            get => _selectedHostelContact;
            set
            {
                _selectedHostelContact = value;
                OnPropertyChanged();

                if (_selectedHostelContact != null)
                {
                    MakePhoneCall(_selectedHostelContact.Phone);
                }
            }
        }

        public EmergencyContactModel SelectedClinicContact
        {
            get => _selectedClinicContact;
            set
            {
                _selectedClinicContact = value;
                OnPropertyChanged();

                if (_selectedClinicContact != null)
                {
                    MakePhoneCall(_selectedClinicContact.Phone);
                }
            }
        }

        public EmergencyContactsViewModel()
        {
            _firebaseService = new FirebaseEmergencyContactService();
            HostelContacts = new ObservableCollection<EmergencyContactModel>();
            ClinicContacts = new ObservableCollection<EmergencyContactModel>();
            Task.Run(InitializeAsync);
        }

        private async Task InitializeAsync()
        {
            try
            {
                var hostels = await _firebaseService.GetHostelContactsAsync();
                var clinics = await _firebaseService.GetClinicContactsAsync();

                Dispatcher.Dispatch(() =>
                {
                    foreach (var hostel in hostels)
                    {
                        HostelContacts.Add(hostel);
                    }

                    foreach (var clinic in clinics)
                    {
                        ClinicContacts.Add(clinic);
                    }
                });
            }
            catch (Exception ex)
            {
                Dispatcher.Dispatch(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                    Debug.WriteLine($"Firebase error: {ex.Message}");
                });
            }
        }

        private void MakePhoneCall(string phoneNumber)
        {
            try
            {
                PhoneDialer.Open(phoneNumber);
            }
            catch (ArgumentNullException)
            {
                Dispatcher.Dispatch(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Phone number is invalid.", "OK");
                });
            }
            catch (FeatureNotSupportedException)
            {
                Dispatcher.Dispatch(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Phone Dialer is not supported on this device.", "OK");
                });
            }
            catch (Exception)
            {
                Dispatcher.Dispatch(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "An unexpected error occurred.", "OK");
                });
            }
        }
    }
}
