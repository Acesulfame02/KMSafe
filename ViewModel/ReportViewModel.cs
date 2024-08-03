using CommunityToolkit.Maui.Views;
using Safe.Model;
using Safe.Pages;
using Safe.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Input;

namespace Safe.ViewModel
{
    public class ReportViewModel : INotifyPropertyChanged
    {
        private readonly FirebaseReportService _firebaseReportService;
        private readonly KMSafeFireAuth _authService;
        private bool _isLoading;
        private bool _hasReports;
        private string _userId;
        private ObservableCollection<ReportModel> _reports;
        private string _selectedFilterOption;

        public ObservableCollection<ReportModel> Reports
        {
            get => _reports;
            set
            {
                _reports = value;
                RaisePropertyChanged(nameof(Reports));
            }
        }

        public ObservableCollection<string> FilterOptions { get; } = new ObservableCollection<string>
        {
            "Last 24 hours",
            "Last 7 days",
            "Last 30 days"
        };

        public string SelectedFilterOption
        {
            get => _selectedFilterOption;
            set
            {
                _selectedFilterOption = value;
                RaisePropertyChanged(nameof(SelectedFilterOption));
                LoadReportsAsync(); // Reload reports when the filter option changes
            }
        }

        public ICommand ViewReportCommand { get; }
        public ICommand StartNewReportCommand { get; }
        public event PropertyChangedEventHandler? PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                RaisePropertyChanged(nameof(IsLoading));
            }
        }

        public bool HasReports
        {
            get => _hasReports;
            set
            {
                _hasReports = value;
                RaisePropertyChanged(nameof(HasReports));
            }
        }

        public ReportViewModel()
        {
            _firebaseReportService = new FirebaseReportService();
            _authService = new KMSafeFireAuth();
            Reports = new ObservableCollection<ReportModel>();
            ViewReportCommand = new Command<ReportModel>(async (report) => await ViewReportAsync(report));
            StartNewReportCommand = new Command(async () => await StartNewReportAsync());

            SelectedFilterOption = FilterOptions[0]; // Default to "Last 24 hours"
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            try
            {
                IsLoading = true;

                // Check if the user is logged in
                if (!Preferences.Get("IsLoggedIn", false))
                {
                    await Shell.Current.GoToAsync("//LoginPage");
                    return;
                }

                // Retrieve the user email from preferences
                _userId = Preferences.Get("UserEmail", string.Empty);
                Debug.WriteLine($"User ID: {_userId}");

                if (string.IsNullOrEmpty(_userId))
                {
                    // Navigate to login page if user email is null or empty
                    await Shell.Current.GoToAsync("//LoginPage");
                    return;
                }

                await _firebaseReportService.EnsureDatabaseExistsAsync();
                await LoadReportsAsync();
                SubscribeToRealTimeUpdates();
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it or display an alert)
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                Debug.WriteLine($"Error in InitializeAsync: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadReportsAsync()
        {
            try
            {
                Debug.WriteLine($"Loading reports for user ID: {_userId}");
                var reports = await _firebaseReportService.GetReportsAsync(_userId);
                HasReports = reports.Any();
                Debug.WriteLine($"Reports loaded: {reports.Count}");

                if (HasReports)
                {
                    var filteredReports = FilterReports(reports);
                    Reports = new ObservableCollection<ReportModel>(
                        filteredReports.OrderByDescending(r => $"{r.Date} {r.Time}"));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in LoadReportsAsync: {ex.Message}");
            }
        }

        private IEnumerable<ReportModel> FilterReports(IEnumerable<ReportModel> reports)
        {
            var now = DateTime.Now;
            var filteredReports = reports;

            try
            {
                switch (SelectedFilterOption)
                {
                    case "Last 24 hours":
                        filteredReports = reports.Where(r =>
                            DateTime.ParseExact($"{r.Date} {r.Time}", "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture) >= now.AddDays(-1));
                        break;
                    case "Last 7 days":
                        filteredReports = reports.Where(r =>
                            DateTime.ParseExact($"{r.Date} {r.Time}", "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture) >= now.AddDays(-7));
                        break;
                    case "Last 30 days":
                        filteredReports = reports.Where(r =>
                            DateTime.ParseExact($"{r.Date} {r.Time}", "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture) >= now.AddDays(-30));
                        break;
                }

                // If no reports found for the selected filter, apply the default filter
                if (!filteredReports.Any() && SelectedFilterOption == "Last 24 hours")
                {
                    filteredReports = reports.Where(r =>
                        DateTime.ParseExact($"{r.Date} {r.Time}", "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture) >= now.AddDays(-7));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in FilterReports: {ex.Message}");
            }

            return filteredReports;
        }

        private void SubscribeToRealTimeUpdates()
        {
            _firebaseReportService.SubscribeToReports(_userId, report =>
            {
                if (!Reports.Any(r => r.Date == report.Date && r.Time == report.Time))
                {
                    Application.Current.Dispatcher.Dispatch(() =>
                    {
                        Reports.Add(report);
                        HasReports = Reports.Any();
                    });
                }
            });
        }

        private async Task ViewReportAsync(ReportModel report)
        {
            var reportId = $"{report.Date}_{report.Time}"; // Create report ID from date and time
            var popup = new FullReportPage(reportId);
            Application.Current.MainPage.ShowPopup(popup);
        }

        private async Task StartNewReportAsync()
        {
            // Navigate to new report page
            await Shell.Current.GoToAsync("//NewReportPage");
        }
    }
}
