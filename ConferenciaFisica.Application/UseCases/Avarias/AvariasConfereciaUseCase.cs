using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.Inputs;
using ConferenciaFisica.Application.UseCases.Avarias.Interface;
using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Repositories;

namespace ConferenciaFisica.Application.UseCases.Avarias
{
    public class AvariasConfereciaUseCase : IAvariasConferenciaUseCase
    {
        private readonly IAvariasRepository _repository;

        public AvariasConfereciaUseCase(IAvariasRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<AvariasConferencia>> BuscarAvariasConferencia(int idConferencia)
        {
            var data = await _repository.BuscarAvariasConferencia(idConferencia);

            return ServiceResult<AvariasConferencia>.Success(data, "Avarias carregados com sucesso.");
        }

        public async Task<ServiceResult<bool>> CadastrarAvariaConferencia(AvariaConferenciaInput input)
        {
            var _serviceResult = new ServiceResult<bool>();
            var avarias = new List<TiposAvariasCommand>();

            foreach (var item in input.TiposAvarias)
            {
                avarias.Add(TiposAvariasCommand.New(item.Id, item.Codigo, item.Descricao));
            }

            var command = CadastroAvariaConferenciaCommand.New(input.Id ?? 0, input.IdConferencia, input.QuantidadeAvariada, input.PesoAvariado, input.IdEmbalagem,
                input.Conteiner, input.Observacao, avarias);

            if (command.Id > 0)
            {
                var ret = await _repository.AtualizarAvariaConferencia(command);
                if (ret > 0)
                {
                    _serviceResult.Result = true;
                }
                else
                {
                    _serviceResult.Result = false;
                }
            }
            else
            {
                var ret = await _repository.CadastrarAvariaConferencia(command);

                if (ret > 0)
                {
                    _serviceResult.Result = true;
                }
                else
                {
                    _serviceResult.Result = false;
                }
            }


            return _serviceResult;
        }
    }
}
