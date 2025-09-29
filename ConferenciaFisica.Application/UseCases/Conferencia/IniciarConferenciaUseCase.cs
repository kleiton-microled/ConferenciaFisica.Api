using ConferenciaFisica.Application.Commands;
using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Domain.Repositories;

namespace ConferenciaFisica.Application.UseCases.Conferencia
{
    public class IniciarConferenciaUseCase : IIniciarConferenciaUseCase
    {
        private readonly IConferenciaRepository _conferenciaRepository;
        public IniciarConferenciaUseCase(IConferenciaRepository repository)
        {
            _conferenciaRepository = repository;
        }
        public async Task<ServiceResult<bool>> ExecuteAsync(ConferenciaFisicaRequest request)
        {
            var _serviceResult = new ServiceResult<bool>();

            var command = ConferenciaFisicaCommand.New(null,
                                                       request.Tipo,
                                                       request.Conteiner, 
                                                       request.Bl, 
                                                       request.Inicio, 
                                                       request.Termino, 
                                                       request.CpfConferente, 
                                                       request.NomeConferente,
                                                       request.TelefoneConferente,
                                                       request.CpfCliente, 
                                                       request.NomeCliente, 
                                                       request.QtdeDivergente, 
                                                       request.DivergenciaQualificacao, 
                                                       request.ObservacaoDivergencias, 
                                                       request.RetiradaAmostra, 
                                                       request.Embalagem,
                                                       request.ConferenciaRemota, 
                                                       request.Operacao,
                                                       request.QtdeVolumesDivergentes, 
                                                       request.QtdeRepresentantes, 
                                                       request.QuantidadeAjudantes, 
                                                       request.QuantidadeOperadores,
                                                       request.Movimentacao, 
                                                       request.Desuniticacao, 
                                                       request.PorcentagemDesunitizacao,
                                                       request.QuantidadeDocumentos, 
                                                       request.AutonumAgendaPosicao);

            var ret = await _conferenciaRepository.IniciarConferencia(command);

            if (ret)
            {
                _serviceResult.Result = true;
            }
            else
            {
                _serviceResult.Result = false;
            }

            return _serviceResult;
        }
    }
}
