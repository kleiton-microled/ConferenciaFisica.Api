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
                                                       request.ObservacaoDivergencia, 
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
                                                       request.QuantidadeDocumentos,
                                                       request.AutonumAgendaPosicao);

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

        public async Task<ServiceResult<bool>> FinalizarConferencia(int idConferencia)
        {
            var _serviceResult = new ServiceResult<bool>();

            var ret = await _conferenciaRepository.FinalizarConferencia(idConferencia);

            if (ret)
            {
                _serviceResult.Result = true;
                _serviceResult.Mensagens.Add("Conferência finalizada com sucesso!");
            }
            else
            {
                _serviceResult.Result = false;
            }

            return _serviceResult;
        }
    }
}
