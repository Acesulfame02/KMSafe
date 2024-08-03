using Safe.Model;
using Safe.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Safe.ViewModel
{
    public class NewReportViewModel : INotifyPropertyChanged
    {
        private ReportModel _report;
        private readonly FirebaseReportService _firebaseReportService;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ReportModel Report
        {
            get => _report;
            set
            {
                _report = value;
                OnPropertyChanged(nameof(Report));
            }
        }

        public ObservableCollection<string> TypeOptions { get; } = new ObservableCollection<string>
        {
            "Health",
            "Safety"
        };

        public ObservableCollection<string> SeverityOptions { get; } = new ObservableCollection<string>
        {
            "Serious",
            "Emergency",
            "General"
        };

        public ObservableCollection<string> MessageOptions { get; } = new ObservableCollection<string>
        {
            "Urgent",
            "Normal",
            "Complaint"
        };

        public ICommand SubmitReportCommand { get; }
        public ICommand CancelCommand { get; }

        public NewReportViewModel()
        {
            _firebaseReportService = new FirebaseReportService();
            Report = new ReportModel();
            SubmitReportCommand = new Command(async () => await SubmitReportAsync());
            CancelCommand = new Command(async () => await CancelAsync());
        }

        private async Task SubmitReportAsync()
        {
            var userId = Preferences.Get("UserEmail", string.Empty);

            if (string.IsNullOrEmpty(userId))
            {
                // Navigate to login page if user email is null or empty
                await Shell.Current.GoToAsync("//LoginPage");
                return;
            }

            Report.UserId = userId;

            try
            {
                await _firebaseReportService.AddReportAsync(Report);
                await Application.Current.MainPage.DisplayAlert("Success", "Report submitted successfully.", "OK");
                await Shell.Current.GoToAsync("//ReportPage");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task CancelAsync()
        {
            // Navigate back to the report page or previous page
            await Shell.Current.GoToAsync("//ReportPage");
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
