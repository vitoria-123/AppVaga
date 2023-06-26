using AppVaga.ViewModels.Vagas;

namespace AppVaga.Views.Vagas;

public partial class CadastroVagaView : ContentPage
{
	private CadastroVagaViewModel cadViewModel;
	public CadastroVagaView()
	{
		InitializeComponent();
		cadViewModel = new CadastroVagaViewModel();
		BindingContext = cadViewModel;
		Title = "Cadastro de nova Vaga";
	}
}