using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.Inputs;
using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Repositories;

namespace ConferenciaFisica.Application.UseCases.Conferencia
{
    public class CadastrosAdicionaisUseCase : ICadastrosAdicionaisUseCase
    {
        private readonly IConferenciaRepository _conferenciaRepository;
        public CadastrosAdicionaisUseCase(IConferenciaRepository repository)
        {
            _conferenciaRepository = repository;
        }

        public async Task<ServiceResult<bool>> ExecuteAsync(CadastroAdicionalInput request)
        {
            var _serviceResult = new ServiceResult<bool>();

            var command = new CadastroAdicionalCommand()
            {
                IdConferencia = request.IdConferencia,
                Nome = request.Nome,
                Cpf = request.Cpf,
                Qualificacao = request.Qualificacao,
                Tipo = request.Tipo,
            };

            var ret = await _conferenciaRepository.CadastroAdicional(command);

            if (ret)
            {
                _serviceResult.Result = true;
                _serviceResult.Mensagens.Add("Cadastro realizado com sucesso!");
            }
            else
            {
                _serviceResult.Result = false;
            }

            return _serviceResult;
        }

        public async Task<ServiceResult<IEnumerable<CadastrosAdicionaisDTO>>> CarregarCadastrosAdicionais(int idConferencia)
        {
            var _serviceResult = new ServiceResult<IEnumerable<CadastrosAdicionaisDTO>>();

            _serviceResult.Result = await _conferenciaRepository.CarregarCadastrosAdicionais(idConferencia);
            return _serviceResult;
        }
    }
}
