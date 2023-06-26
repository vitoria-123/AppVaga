using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppVaga.Services.Vagas;
using AppVaga.Models;
using AppVaga.Models.Enuns;
using System.Windows.Input;

namespace AppVaga.ViewModels.Vagas
{
    [QueryProperty("VagaSelecionadoId", "vId")]

    public class CadastroVagaViewModel : BaseViewModel
    {
        private VagaService vService;

        public ICommand SalvarCommand { get; }

        public ICommand CancelarCommand { get; set; }

        public CadastroVagaViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            vService = new VagaService(token);
            _ = ObterDisponibilidade();
            _ = ObterPreferencia();

            SalvarCommand = new Command(async () => { await SalvarVaga(); });
            CancelarCommand = new Command(async => CancelarCadastro());
        }

        private async void CancelarCadastro()
        {
            await Shell.Current.GoToAsync("..");
        }

        private int id;
        private int coordenadaVaga;
        private string secaoVaga;
        private int andarVaga;
        private int numeroVaga;
        private TipoDisponibilidade tipoDisponibilidadeSelecionado;
        private TipoPreferencial tipoPreferencialSelecionado;

        public int Id 
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        public int CoordenadaVaga
        {
            get => coordenadaVaga;
            set
            {
                coordenadaVaga = value;
                OnPropertyChanged();
            }
        }

        public string SecaoVaga
        {
            get => secaoVaga;
            set
            {
                secaoVaga = value;
                OnPropertyChanged();
            }
        }

        public int AndarVaga
        {
            get => andarVaga;
            set
            {
                andarVaga = value;
                OnPropertyChanged();
            }
        }

        public int NumeroVaga
        {
            get => numeroVaga;
            set
            {
                numeroVaga = value;
                OnPropertyChanged();
            }
        }

        public TipoDisponibilidade TipoDisponibilidadeSelecionado
        {
            get { return tipoDisponibilidadeSelecionado; }
            set
            {
                if (value != null)
                {
                    tipoDisponibilidadeSelecionado = value;
                    OnPropertyChanged();
                }
            }
        }

        public TipoPreferencial TipoPreferencialSelecionado
        {
            get { return tipoPreferencialSelecionado; }
            set
            {
                if (value != null)
                {
                    tipoPreferencialSelecionado = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<TipoDisponibilidade> listaTiposDisponibilidade;

        public ObservableCollection<TipoDisponibilidade> ListaTiposDisponibilidade
        {
            get { return listaTiposDisponibilidade; }
            set
            {
                if (value != null)
                {
                    listaTiposDisponibilidade = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<TipoPreferencial> listaTiposPreferencial;

        public ObservableCollection<TipoPreferencial> ListaTiposPreferencial
        {
            get { return listaTiposPreferencial; }
            set
            {
                if (value != null)
                {
                    listaTiposPreferencial = value;
                    OnPropertyChanged();
                }
            }
        }

        public async Task ObterDisponibilidade()
        {
            try
            {
                ListaTiposDisponibilidade = new ObservableCollection<TipoDisponibilidade>();
                listaTiposDisponibilidade.Add(new TipoDisponibilidade() { Id = 1, DescricaoDisp = "Disponivel" });
                listaTiposDisponibilidade.Add(new TipoDisponibilidade() { Id = 2, DescricaoDisp = "Ocupada" });
                listaTiposDisponibilidade.Add(new TipoDisponibilidade() { Id = 3, DescricaoDisp = "Disponivel em Breve" });
                OnPropertyChanged(nameof(ListaTiposDisponibilidade));
            }
            catch (Exception ex) 
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task ObterPreferencia()
        {
            try
            {
                ListaTiposPreferencial = new ObservableCollection<TipoPreferencial>();
                listaTiposPreferencial.Add(new TipoPreferencial() { Id = 0, DescricaoPref = "Negativo" });
                listaTiposPreferencial.Add(new TipoPreferencial() { Id = 1, DescricaoPref = "Idoso" });
                listaTiposPreferencial.Add(new TipoPreferencial() { Id = 2, DescricaoPref = "PCD" });
                listaTiposPreferencial.Add(new TipoPreferencial() { Id = 3, DescricaoPref = "Gestante" });
                OnPropertyChanged(nameof(ListaTiposPreferencial));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task SalvarVaga()
        {
            try
            {
                Vaga model = new Vaga()
                {
                    CoordenadaVaga = this.coordenadaVaga,
                    SecaoVaga = this.secaoVaga,
                    AndarVaga = this.andarVaga,
                    NumeroVaga = this.numeroVaga,
                    Id = this.id,
                    PreferencialVaga = (PreferencialVagaEnum)tipoPreferencialSelecionado.Id,
                    DisponibilidadeVaga = (DisponibilidadeVagaEnum)tipoDisponibilidadeSelecionado.Id
                };
                if (model.Id == 0)
                    await vService.PostVagaAsync(model);
                else
                    await vService.PutVagaAsync(model);

                await Application.Current.MainPage
                    .DisplayAlert("Mensagem", "Dados salvos com sucesso!", "Ok");
                await Shell.Current.GoToAsync(".."); //Remove a página atual da pilha de páginas
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }

        private string vagaSelecionadoId;

        public string VagaSelecionadoId
        {
            set
            {
                if (value != null)
                {
                    vagaSelecionadoId = Uri.UnescapeDataString(value);
                    CarregarVaga();
                }
            }
        }

        public async void CarregarVaga()
        {
            try
            {
                Vaga v = await vService.GetVagaAsync(int.Parse(vagaSelecionadoId));

                this.CoordenadaVaga = v.CoordenadaVaga;
                this.SecaoVaga = v.SecaoVaga;
                this.AndarVaga = v.AndarVaga;
                this.NumeroVaga = v.NumeroVaga;
                this.Id = v.Id;

                TipoDisponibilidadeSelecionado = this.ListaTiposDisponibilidade
                    .FirstOrDefault(dClasse => dClasse.Id == (int)v.DisponibilidadeVaga);

                TipoPreferencialSelecionado = this.ListaTiposPreferencial
                        .FirstOrDefault(pClasse => pClasse.Id == (int)v.PreferencialVaga);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }

    }
}
