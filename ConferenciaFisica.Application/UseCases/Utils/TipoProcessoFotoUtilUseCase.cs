using AutoMapper;
using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.UseCases.Utils.Interfaces;
using ConferenciaFisica.Application.ViewModels;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Repositories;

namespace ConferenciaFisica.Application.UseCases.Utils
{
    public class TipoProcessoFotoUtilUseCase : ITipoProcessoFotoUtilUseCase
    {
        private readonly ITiposProcessoFotoRepository _tiposProcessoFotoRepository;
        private readonly IMapper _mapper;

        public TipoProcessoFotoUtilUseCase(ITiposProcessoFotoRepository tiposProcessoFotoRepository, IMapper mapper)
        {
            _tiposProcessoFotoRepository = tiposProcessoFotoRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResult<TipoProcessoFotoModel>> Create(TipoProcessoFotoViewModel input)
        {
            try
            {
                var result = await _tiposProcessoFotoRepository.Create(_mapper.Map<TipoProcessoFotoModel>(input));

                return result is not null ? ServiceResult<TipoProcessoFotoModel>.Success(result) : ServiceResult<TipoProcessoFotoModel>.Failure("Falha ao criar");
            }
            catch (Exception exception)
            {
                return ServiceResult<TipoProcessoFotoModel>.Failure($"Falha ao criar, {exception.Message}");
            }
        }

        public async Task<ServiceResult<bool>> Delete(int id)
        {
            try
            {
                var result = await _tiposProcessoFotoRepository.Delete(id);

                return result ? ServiceResult<bool>.Success(result) : ServiceResult<bool>.Success(result, "Registro nao encontrado");
            }
            catch (Exception exception)
            {
                return ServiceResult<bool>.Failure($"Falha ao deletar, {exception.Message}");
            }
        }

        public async Task<ServiceResult<TipoProcessoFotoModel>> Get(int id)
        {
            try
            {
                var result = await _tiposProcessoFotoRepository.Get(id);

                return result is not null ? ServiceResult<TipoProcessoFotoModel>.Success(result)
                                            : ServiceResult<TipoProcessoFotoModel>.Success(result, "Registro nao encontrado");
            }
            catch (Exception exception)
            {
                return ServiceResult<TipoProcessoFotoModel>.Failure($"Falha ao buscar registo, {exception.Message}");
            }
        }

        public async Task<ServiceResult<IEnumerable<TipoProcessoFotoModel>>> GetAll()
        {
            try
            {
                var result = await _tiposProcessoFotoRepository.GetAll();

                return result is not null ? ServiceResult<IEnumerable<TipoProcessoFotoModel>>.Success(result)
                                            : ServiceResult<IEnumerable<TipoProcessoFotoModel>>.Success(result, "Registros nao encontrados");
            }
            catch (Exception exception)
            {
                return ServiceResult<IEnumerable<TipoProcessoFotoModel>>.Failure($"Falha ao buscar registos, {exception.Message}");
            }
        }

        public async Task<ServiceResult<IEnumerable<EnumValueDTO>>> GetByProcessoNome(string processoName)
        {
            try
            {
                var result = await _tiposProcessoFotoRepository.GetByProcessoNome(processoName);

                if (result is not null)
                {
                    var d = result.Select( x => new EnumValueDTO()
                    {
                        Id = x.TipoFotoID,
                        Codigo = x.TipoProcessoID,
                        Descricao = x.Descricao,

                    });
                }
                return result is not null ? ServiceResult<IEnumerable<EnumValueDTO>>.Success(result.Select(x => new EnumValueDTO()
                                                                                                {
                                                                                                    Id = x.TipoFotoID,
                                                                                                    Codigo = x.TipoProcessoID,
                                                                                                    Descricao = x.Descricao,

                                                                                                }))
                                            : ServiceResult<IEnumerable<EnumValueDTO>>.Success(null, "Registros nao encontrados");
            }
            catch (Exception exception)
            {
                return ServiceResult<IEnumerable<EnumValueDTO>>.Failure($"Falha ao buscar registos, {exception.Message}");
            }
        }

        public async Task<ServiceResult<TipoProcessoFotoModel>> Update(int id, TipoProcessoFotoViewModel input)
        {
            try
            {
                var mappedModel = _mapper.Map<TipoProcessoFotoModel>(input);
                mappedModel.Id = id;

                var result = await _tiposProcessoFotoRepository.Update(mappedModel);

                return result is not null ? ServiceResult<TipoProcessoFotoModel>.Success(result) : ServiceResult<TipoProcessoFotoModel>.Failure("Nenhum registro foi afetado");
            }
            catch (Exception exception)
            {
                return ServiceResult<TipoProcessoFotoModel>.Failure($"Falha ao atualizar, {exception.Message}");
            }
        }
    }
}
