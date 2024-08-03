using Safe.Services;
using Safe.ViewModel;

namespace Safe.Pages;

public partial class ReportPage : ContentPage
{
    public ReportPage()
	{
		InitializeComponent();

        BindingContext = new ReportViewModel();
    }
}