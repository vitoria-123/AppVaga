using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppVaga.Models;
using AppVaga.Services.Vagas;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AppVaga.ViewModels.Vagas
{
    public class ListagemVagaViewModel : BaseViewModel
    {
        private VagaService vService;

        public ObservableCollection<Vaga> Vagas { get; set; }

        public ListagemVagaViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            vService = new VagaService(token);
            Vagas = new ObservableCollection<Vaga>();

            _ = ObterVagas();

            NovaVaga = new Command(async () => { await ExibirCadastroVaga(); });
            RemoverVagaCommand =
                new Command<Vaga>(async (Vaga v) => { await RemoverVaga(v); });
        }

        public ICommand NovaVaga { get; }
        public ICommand RemoverVagaCommand { get; }

        public async Task ObterVagas()
        {
            try
            {
                Vagas = await vService.GetVagasAsync();
                OnPropertyChanged(nameof(Vagas));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task ExibirCadastroVaga()
        {
            try
            {
                await Shell.Current.GoToAsync("cadVagaView");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }

        private Vaga vagaSelecionada;
        public Vaga VagaSelecionada
        {
            get { return vagaSelecionada; }
            set
            {
                if (value != null)
                {
                    vagaSelecionada = value;
                    Shell.Current
                        .GoToAsync($"cadVagaView?vId={vagaSelecionada.Id}");
                }
            }
        }
        public async Task RemoverVaga(Vaga v)
        {
            try
            {
                if (await Application.Current.MainPage
                    .DisplayAlert("Confirmação", $"Confirma a remoção da vaga {v.NumeroVaga} da Seção {v.SecaoVaga}?", "Sim", "Não"))
                {
                    await vService.DeleteVagaAsync(v.Id);
                    await Application.Current.MainPage.DisplayAlert("Mesagem", 
                        "Vaga Removida do Sistema com Sucesso", "Ok");

                    _ = ObterVagas();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }
    }
}
