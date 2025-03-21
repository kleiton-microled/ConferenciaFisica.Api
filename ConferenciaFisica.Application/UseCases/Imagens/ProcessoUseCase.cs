using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.UseCases.Imagens.Interfaces;
using ConferenciaFisica.Application.ViewModels;
using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Repositories;

namespace ConferenciaFisica.Application.UseCases.Imagens
{
    public class ProcessoUseCase : ITipoFotoUseCase
    {
        private IImagemRepository _imagemRepository;

        public ProcessoUseCase(IImagemRepository imagemRepository)
        {
            _imagemRepository = imagemRepository;
        }

        public async Task<ServiceResult<bool>> CreateTipoFoto(TipoFotoViewModel input)
        {
            var result = new ServiceResult<bool>();

            try
            {
                var modelCreate = new TipoFotoModel()
                {
                    Codigo = input.Codigo,
                    Descricao = input.Descricao
                };

                var tipoProcesso = await _imagemRepository.CreateTipoFoto(modelCreate);

                return ServiceResult<bool>.Success(tipoProcesso);
            }
            catch (Exception exception)
            {
                result.Error = exception.Message;
            }

            return result;

        }

        public async Task<ServiceResult<bool>> DeleteTipoFoto(int id)
        {
            var result = new ServiceResult<bool>();

            try
            {
                var tipoProcesso = await _imagemRepository.DeleteTipoFoto(id);

                return ServiceResult<bool>.Success(tipoProcesso);
            }
            catch (Exception exception)
            {
                result.Error = exception.Message;
            }

            return result;
        }


        public async Task<ServiceResult<IEnumerable<ProcessoViewModel>>> GetImagemByTalieId(int talieId)
        {
            var result = new ServiceResult<IEnumerable<ProcessoViewModel>> ();

            try
            {
                var tipoProcesso = await _imagemRepository.ListProcessoByTalieId(talieId);

                return ServiceResult<IEnumerable<ProcessoViewModel>>.Success(tipoProcesso.Select(x => new ProcessoViewModel()
                {
                    TalieId = x.IdTalie,
                    Descricao = x.Descricao,
                    Observacao = x.Observacao,
                    Type = x.IdTipoProcesso,
                    TypeDescription = x.DescricaoTipoProcesso,
                    ImagemPath = x.ImagemPath,
                    Id = x.Id

                }));
            }
            catch (Exception exception)
            {
                result.Error = exception.Message;
            }

            return result;
        }

        public async Task<ServiceResult<bool>> InsertProcesso(ProcessoViewModel input)
        {
            var result = new ServiceResult<bool>();
            try
            {
                var pathArquivo = await SalvarArquivo(input);
                var command = new ProcessoCommand()
                {
                    IdTipoFoto = input.IdTipoFoto,
                    IdProcesso = input.IdTipoProcesso,
                    IdContainer = input.ContainerId,
                    ImagemPath = pathArquivo,
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

        public async Task<string> SalvarArquivo(ProcessoViewModel processoViewModel)
        {
            string pastaFotos = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "fotos-processos", processoViewModel.TalieId.ToString());

            if (!Directory.Exists(pastaFotos))
            {
                Directory.CreateDirectory(pastaFotos);
            }

            string nomeArquivo = $"talie_{processoViewModel.TalieId}_{processoViewModel.Type}_{Guid.NewGuid().ToString()}.png";
            string caminhoCompleto = Path.Combine(pastaFotos, nomeArquivo);

            byte[] bytesArquivo = Convert.FromBase64String(processoViewModel.ImagemBase64.Split(',').LastOrDefault());

            await File.WriteAllBytesAsync(caminhoCompleto, bytesArquivo);

            return $"fotos-processos/{processoViewModel.TalieId.ToString()}/{nomeArquivo}";
        }

        public async Task<ServiceResult<IEnumerable<TipoFotoModel>>> GetAllTipoFoto()
        {
            var result = new ServiceResult<IEnumerable<TipoFotoModel>>();

            try
            {
                var tiposImagens = await _imagemRepository.GetImagesTypes();

                return ServiceResult<IEnumerable<TipoFotoModel>>.Success(tiposImagens);
            }
            catch (Exception exception)
            {
                result.Error = exception.Message;
            }

            return result;
        }

        public async Task<ServiceResult<bool>> UpdateTipoFoto(UpdateTipoFotoViewModel input)
        {
            var result = new ServiceResult<bool>();

            try
            {
                var command = new ProcessoCommand()
                {
                    Id = input.Id,
                    Descricao = input.Descricao,
                    Observacao = input.Observacao,
                };

                var resultUpdate = await _imagemRepository.UpdateProcesso(command);

                return resultUpdate ? ServiceResult<bool>.Success(resultUpdate) : ServiceResult<bool>.Failure("Falha ao atualizars");
            }
            catch (Exception exception)
            {
                result.Error = exception.Message;
            }

            return result;
        }

        //public async Task<ServiceResult<bool>> UpdateTipoFoto(UpdateTipoFotoViewModel input)
        //{
        //    var result = new ServiceResult<bool>();

        //    try
        //    {
        //        var command = new ProcessoCommand()
        //        {
        //            Id = input.Id,
        //            Descricao = input.Descricao,
        //            Observacao = input.Observacao,
        //        };

        //        var resultUpdate = await _imagemRepository.UpdateProcesso(command);

        //        return resultUpdate? ServiceResult<bool>.Success(resultUpdate) : ServiceResult<bool>.Failure("Falha ao atualizars");
        //    }
        //    catch (Exception exception)
        //    {
        //        result.Error = exception.Message;
        //    }

        //    return result;
        //}

        public async Task<ServiceResult<bool>> DeleteProcesso(int id)
        {
            var result = new ServiceResult<bool>();

            try
            {
                var resultDelete = await _imagemRepository.DeleteProcesso(id);

                return resultDelete ? ServiceResult<bool>.Success(resultDelete) : ServiceResult<bool>.Failure("Falha ao excluir processo");
            }
            catch (Exception exception)
            {
                result.Error = exception.Message;
            }

            return result;
        }

        public async Task<ServiceResult<bool>> UpdateTiposFoto(UpdateTiposFotoViewModel input)
        {
            try
            {
                var command = new TipoFotoModel
                {
                    Id = input.Id,
                    Descricao = input.Descricao,
                    Codigo = input.Codigo,
                };

                var resultUpdate = await _imagemRepository.UpdateTipoFoto(command);

                return resultUpdate ? ServiceResult<bool>.Success(resultUpdate) : ServiceResult<bool>.Failure("Falha ao atualizars");
            }
            catch (Exception exception)
            {
                return ServiceResult<bool>.Failure($"Falha ao atualizars, {exception.Message}");
            }
        }
    }
}
