using Firebase.Database;
using Firebase.Database.Query;
using Safe.Model;
using System.Diagnostics;

namespace Safe.Services
{
    public class FirebaseReportService
    {
        private readonly FirebaseClient _firebaseClient;
        private readonly string _basePath = "Reports";

        public FirebaseReportService()
        {
            _firebaseClient = new FirebaseClient("https://kmsafe-67573-default-rtdb.firebaseio.com/");
        }

        public async Task<List<ReportModel>> GetReportsAsync(string userId)
        {
            var reports = new List<ReportModel>();
            try
            {
                Debug.WriteLine($"Getting reports for user ID: {userId}");
                var firebaseReports = await _firebaseClient
                    .Child(_basePath)
                    .OrderBy("UserId")
                    .EqualTo(userId)
                    .OnceAsync<ReportModel>();

                reports = firebaseReports.Select(fr => fr.Object).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting reports: {ex.Message}");
                // Handle exceptions
            }
            return reports;
        }

        public async Task AddReportAsync(ReportModel report)
        {
            try
            {
                await _firebaseClient
                    .Child(_basePath)
                    .PostAsync(report);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Debug.WriteLine($"Error getting reports: {ex.Message}");
            }
        }

        public async Task EnsureDatabaseExistsAsync()
        {
            try
            {
                var firebaseReports = await _firebaseClient
                    .Child(_basePath)
                    .OnceAsync<ReportModel>();

                if (!firebaseReports.Any())
                {
                    var initialReport = new ReportModel
                    {
                        Date = DateTime.Today.ToString("dd-MM-yyyy"),
                        Title = "Initial Report",
                        Description = "This is an initial report to create the database.",
                        Type = "Initial",
                        Severity = "None",
                        Message = "This is a placeholder report.",
                        Time = DateTime.Now.ToString("HH:mm:ss"),
                        UserId = "initialUser"
                    };

                    await AddReportAsync(initialReport);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Debug.WriteLine($"Error getting reports: {ex.Message}");
            }
        }

        public void SubscribeToReports(string userId, Action<ReportModel> onReportAdded)
        {
            _firebaseClient
                .Child(_basePath)
                .OrderBy("UserId")
                .EqualTo(userId)
                .AsObservable<ReportModel>()
                .Subscribe(d =>
                {
                    if (d.EventType == Firebase.Database.Streaming.FirebaseEventType.InsertOrUpdate && d.Object.UserId == userId)
                    {
                        onReportAdded(d.Object);
                    }
                });
        }

        public async Task<ReportModel> GetReportByIdAsync(string reportId)
        {
            var reportDateTime = reportId.Split('_');
            var reportDate = reportDateTime[0];
            var reportTime = reportDateTime[1];

            var firebaseReports = await _firebaseClient
                .Child(_basePath)
                .OnceAsync<ReportModel>();

            var report = firebaseReports.FirstOrDefault(r => r.Object.Date == reportDate && r.Object.Time == reportTime)?.Object;
            return report;
        }
    }
}
