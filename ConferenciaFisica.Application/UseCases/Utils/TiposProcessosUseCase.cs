using AutoMapper;
using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.UseCases.Utils.Interfaces;
using ConferenciaFisica.Application.ViewModels;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConferenciaFisica.Application.UseCases.Utils
{
    public class TiposProcessosUseCase : ITiposProcessosUseCase
    {
        private readonly ITiposProcessoRepository _tipoProcessoRepository;
        private readonly IMapper _mapper;

        public TiposProcessosUseCase(ITiposProcessoRepository tipoProcessoRepository, IMapper mapper)
        {
            _tipoProcessoRepository = tipoProcessoRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResult<TipoProcessoModel>> Create(TipoProcessoViewModel input)
        {
            try
            {
                var result = await _tipoProcessoRepository.Create(_mapper.Map<TipoProcessoModel>(input));

                return result is not null ? ServiceResult<TipoProcessoModel>.Success(result) : ServiceResult<TipoProcessoModel>.Failure("Falha ao criar");
            }
            catch (Exception exception)
            {
                return ServiceResult<TipoProcessoModel>.Failure($"Falha ao criar, {exception.Message}");
            }
        }

        public async Task<ServiceResult<bool>> Delete(int id)
        {
            try
            {
                var result = await _tipoProcessoRepository.Delete(id);

                return result ? ServiceResult<bool>.Success(result) : ServiceResult<bool>.Success(result, "Registro nao encontrado");
            }
            catch (Exception exception)
            {
                return ServiceResult<bool>.Failure($"Falha ao deletar, {exception.Message}");
            }
        }

        public async Task<ServiceResult<TipoProcessoModel>> Get(int id)
        {
            try
            {
                var result = await _tipoProcessoRepository.Get(id);

                return result is not null ? ServiceResult<TipoProcessoModel>.Success(result) : ServiceResult<TipoProcessoModel>.Success(result, "Registro nao encontrado");
            }
            catch (Exception exception)
            {
                return ServiceResult<TipoProcessoModel>.Failure($"Falha ao buscar registo, {exception.Message}");
            }
        }

        public async Task<ServiceResult<IEnumerable<TipoProcessoModel>>> GetAll()
        {
            try
            {
                var result = await _tipoProcessoRepository.GetAll();

                return result is not null ? ServiceResult<IEnumerable<TipoProcessoModel>>.Success(result) : ServiceResult<IEnumerable<TipoProcessoModel>>.Success(result, "Registros nao encontrados");
            }
            catch (Exception exception)
            {
                return ServiceResult<IEnumerable<TipoProcessoModel>>.Failure($"Falha ao buscar registos, {exception.Message}");
            }
        }

        public async Task<ServiceResult<TipoProcessoModel>> Update(int id, TipoProcessoViewModel input)
        {
            try
            {
                var mappedModel = _mapper.Map<TipoProcessoModel>(input);
                mappedModel.Id = id;

                var result = await _tipoProcessoRepository.Update(mappedModel);

                return result is not null ? ServiceResult<TipoProcessoModel>.Success(result) : ServiceResult<TipoProcessoModel>.Failure("Nenhum registro foi afetado");
            }
            catch (Exception exception)
            {
                return ServiceResult<TipoProcessoModel>.Failure($"Falha ao atualizar, {exception.Message}");
            }
        }
    }
}
