using CommunityToolkit.Maui.Views;
using Safe.Model;
using Safe.Services;
using System.Threading.Tasks;

namespace Safe.Pages;

public partial class FullReportPage : Popup
{
    private readonly FirebaseReportService _firebaseReportService;
    public ReportModel Report { get; set; }

    public FullReportPage(string reportId)
    {
        InitializeComponent();
        _firebaseReportService = new FirebaseReportService();
        LoadReport(reportId);
    }

    private async void LoadReport(string reportId)
    {
        Report = await _firebaseReportService.GetReportByIdAsync(reportId);
        BindingContext = Report;
    }

    private void OnCancelButtonClicked(object sender, EventArgs e)
    {
        Close();
    }
}
