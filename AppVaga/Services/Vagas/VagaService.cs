using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppVaga.Models;

namespace AppVaga.Services.Vagas
{
    public class VagaService : Request
    {
        private readonly Request _request;
        private const string apiUrlBase = "http://apivaga.somee.com/APIVaga/Vagas";

        private string _token;
        public VagaService (string token)   
        {
            _request = new Request ();
            _token = token;
        }

        public async Task<ObservableCollection<Vaga>> GetVagasAsync()
        {
            string urlComplementar = string.Format("{0}", "/GetAll");
            ObservableCollection<Models.Vaga> listaUsuarios = await
            _request.GetAsync<ObservableCollection<Models.Vaga>>(apiUrlBase + urlComplementar, _token);
            return listaUsuarios;
        }
        public async Task<Vaga> GetVagaAsync(int usuarioId)
        {
            string urlComplementar = string.Format("/{0}", usuarioId);
            var usuario = await _request.GetAsync<Models.Vaga>(apiUrlBase + urlComplementar, _token);
            return usuario;
        }
        public async Task<int> PostVagaAsync(Vaga v)
        {
            return await _request.PostReturnIntTokenAsync(apiUrlBase, v, _token);
        }
        public async Task<int> PutVagaAsync(Vaga v)
        {
            var result = await _request.PutAsync(apiUrlBase, v, _token);
            return result;
        }
        public async Task<int> DeleteVagaAsync(int vagaId)
        {
            string urlComplementar = string.Format("/{0}", vagaId);
            var result = await _request.DeleteAsync(apiUrlBase + urlComplementar, _token);
            return result;
        }
    }
}
