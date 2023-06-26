using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppVaga.Models.Enuns;

namespace AppVaga.Models
{
    public class Vaga
    {
        public int Id { get; set; }
        public int CoordenadaVaga { get; set; }
        public string SecaoVaga { get; set; }
        public int AndarVaga { get; set; }
        public int NumeroVaga { get; set; }
        public PreferencialVagaEnum PreferencialVaga { get; set; }
        public DisponibilidadeVagaEnum DisponibilidadeVaga { get; set; }
    }
}
