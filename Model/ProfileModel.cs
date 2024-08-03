using System.ComponentModel;

namespace Safe.Model
{
    public class ProfileModel : INotifyPropertyChanged
    {
        private string _username = string.Empty;
        private string _campusAddress = string.Empty;
        private string _healthId = string.Empty;
        private string _phoneNumber = string.Empty;
        private bool _proofOfPayment;
        private string _email = string.Empty;
        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Username
        {
            get=> _username;
            set
            {
                _username = value;
                RaisePropertyChanged(nameof(Username));
            }
        }

        public string CampusAddress
        {
            get => _campusAddress;
            set
            {
                _campusAddress = value;
                RaisePropertyChanged(nameof(CampusAddress));
            }
        }

        public string HealthId
        {
            get => _healthId;
            set
            {
                _healthId = value;
                RaisePropertyChanged(nameof(HealthId));
            }
        }

        public bool ProofOfPayment
        {
            get => _proofOfPayment;
            set
            {
                _proofOfPayment = value;
                RaisePropertyChanged(nameof(ProofOfPayment));
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                RaisePropertyChanged(nameof(PhoneNumber));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged(nameof(Email));
            }
        }
    }
}
