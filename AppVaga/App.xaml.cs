using AppVaga.Views.Vagas;
namespace AppVaga;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}
