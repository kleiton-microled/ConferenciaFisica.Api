using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.UseCases.Imagens.Interfaces;
using ConferenciaFisica.Application.ViewModels;
using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Repositories;

namespace ConferenciaFisica.Application.UseCases.Imagens
{
    public class ImagensUseCase : IImagensUseCaseUseCase
    {
        private IImagemRepository _imagemRepository;

        public ImagensUseCase(IImagemRepository imagemRepository)
        {
            _imagemRepository = imagemRepository;
        }

        public async Task<ServiceResult<bool>> CreateTipoProcesso(TipoProcessoViewModel input)
        {
            var result = new ServiceResult<bool>();

            try
            {
                var modelCreate = new TipoProcesso()
                {
                    Codigo = input.Codigo,
                    Descricao = input.Descricao
                };

                var tipoProcesso = await _imagemRepository.CreateTipoProcesso(modelCreate);

                return ServiceResult<bool>.Success(tipoProcesso);
            }
            catch (Exception exception)
            {
                result.Error = exception.Message;
            }

            return result;

        }

        public async Task<ServiceResult<bool>> DeleteTipoProcesso(int id)
        {
            var result = new ServiceResult<bool>();

            try
            {
                var tipoProcesso = await _imagemRepository.DeleteTipoProcesso(id);

                return ServiceResult<bool>.Success(tipoProcesso);
            }
            catch (Exception exception)
            {
                result.Error = exception.Message;
            }

            return result;
        }

        public Task<ServiceResult<IEnumerable<TipoProcesso>>> GetImagemByRegistroId(int registroId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<bool>> InsertProcesso(ProcessoViewModel input)
        {
            var result = new ServiceResult<bool>();
            //return result;
            try
            {
                var command = new ProcessoCommand()
                {
                    IdTipoProcesso = input.Type,
                    ImagemPath = "",
                    Descricao = input.Descricao,
                    Observacao = input.Observacao,
                    IdTalie = input.TalieId
                };
                var tiposImagens = await _imagemRepository.CreateProcesso(command);

                return ServiceResult<bool>.Success(tiposImagens);
            }
            catch (Exception exception)
            {
                result.Error = exception.Message;
            }

            return result;
        }

        public async Task<ServiceResult<IEnumerable<TipoProcesso>>> ListTipoProcesso()
        {
            var result = new ServiceResult<IEnumerable<TipoProcesso>>();

            try
            {
                var tiposImagens = await _imagemRepository.GetImagesTypes();

                return ServiceResult<IEnumerable<TipoProcesso>>.Success(tiposImagens);
            }
            catch (Exception exception)
            {
                result.Error = exception.Message;
            }

            return result;
        }

        public Task<ServiceResult<IEnumerable<TipoProcesso>>> UpdateImagemByRegistroId(int registroId)
        {
            throw new NotImplementedException();
        }
    }
}
