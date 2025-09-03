using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.UseCases.Imagens.Interfaces;
using ConferenciaFisica.Application.ViewModels;
using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Entities.DescargaExportacao;
using ConferenciaFisica.Domain.Repositories;
using Microsoft.Extensions.Configuration;

namespace ConferenciaFisica.Application.UseCases.Imagens
{
    public class ProcessoUseCase : ITipoFotoUseCase
    {
        private IImagemRepository _imagemRepository;
        private readonly IConfiguration _configuration;

        public ProcessoUseCase(IImagemRepository imagemRepository, IConfiguration configuration)
        {
            _imagemRepository = imagemRepository;
            _configuration = configuration;
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

                // Obtém o caminho base das imagens
                var basePath = _configuration["Images:Path"];
                if (string.IsNullOrEmpty(basePath))
                    throw new InvalidOperationException("Caminho das imagens não configurado.");

                var processos = new List<ProcessoViewModel>();

                foreach (var x in tipoProcesso)
                {
                    var processo = new ProcessoViewModel()
                    {
                        TalieId = x.IdTalie,
                        Descricao = x.Descricao,
                        Observacao = x.Observacao,
                        Type = x.IdTipoProcesso,
                        TypeDescription = x.DescricaoTipoProcesso,
                        ImagemPath = x.ImagemPath,
                        Id = x.Id
                    };

                    // Converte a imagem para base64 se o arquivo existir
                    if (!string.IsNullOrEmpty(x.ImagemPath))
                    {
                        var fullImagePath = Path.Combine(basePath, x.ImagemPath.Replace("/", Path.DirectorySeparatorChar.ToString()));
                        
                        if (File.Exists(fullImagePath))
                        {
                            try
                            {
                                var imageBytes = await File.ReadAllBytesAsync(fullImagePath);
                                var base64String = Convert.ToBase64String(imageBytes);
                                
                                // Detecta o tipo de arquivo pela extensão
                                var extension = Path.GetExtension(fullImagePath).ToLower();
                                var mimeType = extension switch
                                {
                                    ".png" => "image/png",
                                    ".jpg" or ".jpeg" => "image/jpeg",
                                    ".gif" => "image/gif",
                                    ".bmp" => "image/bmp",
                                    _ => "image/png" // default para PNG
                                };
                                
                                processo.ImagemBase64 = $"data:{mimeType};base64,{base64String}";
                            }
                            catch (Exception ex)
                            {
                                // Log do erro mas continua sem a imagem
                                Console.WriteLine($"Erro ao converter imagem {fullImagePath} para base64: {ex.Message}");
                                processo.ImagemBase64 = null;
                            }
                        }
                    }

                    processos.Add(processo);
                }

                return ServiceResult<IEnumerable<ProcessoViewModel>>.Success(processos);
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
            // Obtém o caminho base do appsettings
            var basePath = _configuration["Images:Path"];
            if (string.IsNullOrEmpty(basePath))
                throw new InvalidOperationException("Caminho das imagens não configurado.");

            // Define a pasta completa do processo (usando NomeProcesso)
            string pastaFotos = Path.Combine(basePath, processoViewModel.NomeProcesso, processoViewModel.TalieId.ToString());

            // Cria o diretório se não existir
            if (!Directory.Exists(pastaFotos))
            {
                Directory.CreateDirectory(pastaFotos);
            }

            // Nome do arquivo
            string nomeArquivo = $"talie_{processoViewModel.TalieId}_{processoViewModel.Type}_{Guid.NewGuid()}.png";

            // Caminho completo
            string caminhoCompleto = Path.Combine(pastaFotos, nomeArquivo);

            // Decodifica a imagem base64 e salva
            byte[] bytesArquivo = Convert.FromBase64String(processoViewModel.ImagemBase64.Split(',').LastOrDefault());
            await File.WriteAllBytesAsync(caminhoCompleto, bytesArquivo);

            // Retorna o caminho relativo usado para leitura posterior
            return $"{processoViewModel.NomeProcesso}/{processoViewModel.TalieId}/{nomeArquivo}";
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

        public async Task<ServiceResult<IEnumerable<ProcessoViewModel>>> GetImagemByContainer(string container)
        {
            var result = new ServiceResult<IEnumerable<ProcessoViewModel>>();

            try
            {
                var tipoProcesso = await _imagemRepository.ListProcessoByContainer(container);

                // Obtém o caminho base das imagens
                var basePath = _configuration["Images:Path"];
                if (string.IsNullOrEmpty(basePath))
                    throw new InvalidOperationException("Caminho das imagens não configurado.");

                var processos = new List<ProcessoViewModel>();

                foreach (var x in tipoProcesso)
                {
                    var processo = new ProcessoViewModel()
                    {
                        TalieId = x.IdTalie,
                        Descricao = x.Descricao,
                        Observacao = x.Observacao,
                        Type = x.IdTipoProcesso,
                        TypeDescription = x.DescricaoTipoProcesso,
                        ImagemPath = x.ImagemPath,
                        Id = x.Id,
                        ContainerId = x.IdContainer
                    };

                    // Converte a imagem para base64 se o arquivo existir
                    if (!string.IsNullOrEmpty(x.ImagemPath))
                    {
                        var fullImagePath = Path.Combine(basePath, x.ImagemPath.Replace("/", Path.DirectorySeparatorChar.ToString()));
                        
                        if (File.Exists(fullImagePath))
                        {
                            try
                            {
                                var imageBytes = await File.ReadAllBytesAsync(fullImagePath);
                                var base64String = Convert.ToBase64String(imageBytes);
                                
                                // Detecta o tipo de arquivo pela extensão
                                var extension = Path.GetExtension(fullImagePath).ToLower();
                                var mimeType = extension switch
                                {
                                    ".png" => "image/png",
                                    ".jpg" or ".jpeg" => "image/jpeg",
                                    ".gif" => "image/gif",
                                    ".bmp" => "image/bmp",
                                    _ => "image/png" // default para PNG
                                };
                                
                                processo.ImagemBase64 = $"data:{mimeType};base64,{base64String}";
                            }
                            catch (Exception ex)
                            {
                                // Log do erro mas continua sem a imagem
                                Console.WriteLine($"Erro ao converter imagem {fullImagePath} para base64: {ex.Message}");
                                processo.ImagemBase64 = null;
                            }
                        }
                    }

                    processos.Add(processo);
                }

                return ServiceResult<IEnumerable<ProcessoViewModel>>.Success(processos);
            }
            catch (Exception exception)
            {
                result.Error = exception.Message;
            }

            return result;
        }
    }
}
