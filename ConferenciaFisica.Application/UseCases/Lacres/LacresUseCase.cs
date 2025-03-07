using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.Inputs;
using ConferenciaFisica.Application.UseCases.Lacres.Interfaces;
using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Repositories;

namespace ConferenciaFisica.Application.UseCases.Lacres
{
    public class LacresUseCase : ILacresUseCase
    {
        private readonly IConferenciaRepository _conferenciaRepository;

        public LacresUseCase(IConferenciaRepository conferenciaRepository)
        {
            _conferenciaRepository = conferenciaRepository;
        }

        public async Task<ServiceResult<bool>> ExecuteAsync(LacreConferenciaInput request)
        {
            var _serviceResult = new ServiceResult<bool>();

            var command = new LacreConferenciaCommand()
            {
                IdConferencia = request.IdConferencia,
                Numero = request.Numero,
                Tipo = request.Tipo,
                LacreFechamento = request.LacreFechamento
            };

            var ret = await _conferenciaRepository.CadastroLacreConferencia(command);

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

        public async Task<ServiceResult<IEnumerable<Lacre>>> GetAllAsync(int idConferencia)
        {
            var lacres = await _conferenciaRepository.CarregarLacresConferencia(idConferencia);

            return ServiceResult<IEnumerable<Lacre>>.Success(lacres, "Lacres carregados com sucesso.");
        }

        public async Task<ServiceResult<bool>> UpdateAsync(LacreConferenciaInput request)
        {
            var _serviceResult = new ServiceResult<bool>();

            var command = new LacreConferenciaCommand()
            {
                Id = request.Id,
                IdConferencia = request.IdConferencia,
                Numero = request.Numero,
                Tipo = request.Tipo,
                LacreFechamento = request.LacreFechamento
            };

            var ret = await _conferenciaRepository.AtualizarLacreConferencia(command);

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

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            var _serviceResult = new ServiceResult<bool>();

            var result = await _conferenciaRepository.ExcluirLacreConferencia(id);
            if (result)
            {
                _serviceResult.Result = result;
                _serviceResult.Mensagens.Add("Cadastro excluido com sucesso!");
            }
            else
            {
                _serviceResult.Mensagens.Add("Falha ao tentar excluir o registro!");
            }

            return _serviceResult;
        }
    }
    
}
