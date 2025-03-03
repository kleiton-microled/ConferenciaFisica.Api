using ConferenciaFisica.Application.Commands;
using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Domain.Repositories;

namespace ConferenciaFisica.Application.UseCases.Conferencia
{
    public class AtualizarConferenciaUseCase : IAtualizarConferenciaUseCase
    {
        private readonly IConferenciaRepository _conferenciaRepository;
        public AtualizarConferenciaUseCase(IConferenciaRepository repository)
        {
            _conferenciaRepository = repository;
        }
        public async Task<ServiceResult<bool>> ExecuteAsync(ConferenciaFisicaRequest request)
        {
            var _serviceResult = new ServiceResult<bool>();

            var command = ConferenciaFisicaCommand.New(request.Id,
                                                       request.Conteiner, 
                                                       request.Bl, 
                                                       request.Inicio, 
                                                       request.Termino, 
                                                       request.CpfConferente, 
                                                       request.NomeConferente,
                                                       request.CpfCliente, 
                                                       request.NomeCliente, 
                                                       request.QtdeDivergente, 
                                                       request.DivergenciaQualificacao, 
                                                       request.ObservacaoDivergencia, 
                                                       request.RetiradaAmostra, 
                                                       request.ConferenciaRemota, 
                                                       request.Operacao,
                                                       request.QtdeVolumesDivergentes, 
                                                       request.QtdeRepresentantes, 
                                                       request.QuantidadeAjudantes, 
                                                       request.QuantidadeOperadores,
                                                       request.Movimentacao, 
                                                       request.Desuniticacao, 
                                                       request.QuantidadeDocumentos);

            var ret = await _conferenciaRepository.AtualizarConferencia(command);

            if (ret)
            {
                _serviceResult.Result = true;
                _serviceResult.Mensagens.Add("Conferência atualizada com sucesso!");
            }
            else
            {
                _serviceResult.Result = false;
            }

            return _serviceResult;
        }
    }
}
