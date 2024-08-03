using Safe.ViewModel;
namespace Safe.Pages;

public partial class NewReportPage : ContentPage
{
	public NewReportPage()
	{
		InitializeComponent();
		BindingContext = new NewReportViewModel();
	}
}