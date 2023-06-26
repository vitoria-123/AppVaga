using AppVaga.Views.Vagas;

namespace AppVaga;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute("cadVagaView", typeof(CadastroVagaView));
	}
}
