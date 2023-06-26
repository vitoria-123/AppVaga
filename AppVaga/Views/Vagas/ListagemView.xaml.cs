using AppVaga.ViewModels.Vagas;

namespace AppVaga.Views.Vagas;

public partial class ListagemView : ContentPage
{
	ListagemVagaViewModel viewModel;
	public ListagemView()
	{
		InitializeComponent();

		viewModel = new ListagemVagaViewModel();
		BindingContext = viewModel;
		Title = "Vagas - App Vaga Programings";
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

        _ = viewModel.ObterVagas();
    }
}