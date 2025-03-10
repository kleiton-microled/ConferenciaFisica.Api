using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenciaFisica.Contracts.Commands
{
    public class CadastroAvariaConferenciaCommand
    {
        public int Id { get; set; }
        public int IdConferencia { get; set; }
        public int QuantidadeAvariada { get; set; }
        public decimal PesoAvariado { get; set; }
        public int IdEmbalagem { get; set; }
        public string Conteiner { get; set; }
        public string Observacao { get; set; }
        public List<TiposAvariasCommand> TiposAvarias { get; set; }

        public static CadastroAvariaConferenciaCommand New(int id, int idConferencia, int quantidadeAvariada, decimal pesoAvariado, int idEmbalagem,
            string conteiner, string observacao, List<TiposAvariasCommand> tiposAvarias)
        {
            var command = new CadastroAvariaConferenciaCommand();
            command.Id = id;
            command.IdConferencia = idConferencia;
            command.QuantidadeAvariada = quantidadeAvariada;
            command.PesoAvariado = pesoAvariado;
            command.IdEmbalagem = idEmbalagem;
            command.Conteiner = conteiner;
            command.Observacao = observacao;
            command.TiposAvarias = tiposAvarias;
            return command;
        }
    }
}
