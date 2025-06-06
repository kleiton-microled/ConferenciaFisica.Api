﻿using ConferenciaFisica.Application.Inputs;
using ConferenciaFisica.Application.UseCases.DescargaExportacao.Interfaces;
using ConferenciaFisica.Application.UseCases.Imagens.Interfaces;
using ConferenciaFisica.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace ConferenciaFisica.Api.Controllers
{
    [ApiController]
    [Route("api/descarga-exportacao")]
    public class DescargaExportacaoController : ControllerBase
    {
        private readonly IDescargaExportacaoUseCase _descargaExportacaoUseCase;
        private readonly ITipoFotoUseCase _processoUseCase;

        private readonly IConfiguration _configuration;

        public DescargaExportacaoController(IDescargaExportacaoUseCase descargaExportacaoUseCase, 
            ITipoFotoUseCase imagensUseCaseUseCase,
            IConfiguration configuration)
        {
            _descargaExportacaoUseCase = descargaExportacaoUseCase;
            _processoUseCase = imagensUseCaseUseCase;
            _configuration = configuration;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var resultado = await _descargaExportacaoUseCase.BuscarPorRegistro(id);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpPost("gravar-talie")]
        public async Task<IActionResult> GravarTalie([FromBody] DescargaExportacaoViewModel request)
        {
            var result = await _descargaExportacaoUseCase.GravarOuAtualizarTalie(request);

            if (!result.Status)
                return NotFound(result.Mensagens);

            return Ok(result);

        }

        [HttpPost("cadastrar-avaria")]
        public async Task<IActionResult> CadastrarAvaria([FromBody] AvariaInput input)
        {
            var result = await _descargaExportacaoUseCase.CadastrarAvaria(input);

            if (!result.Status)
                return NotFound(result.Mensagens);

            return Ok(result);

        }

        [HttpPost("salvar-talie-item")]
        public async Task<IActionResult> SalvarTalieItem([FromBody] TalieItemViewModel request, int registro)
        {
            var result = await _descargaExportacaoUseCase.SalvarTalieItem(request, registro);

            if (!result.Status)
                return NotFound(result.Mensagens);

            return Ok(result);

        }

        [HttpPost("tipos-processos")]
        public async Task<IActionResult> CreateTipoProcesso([FromBody] TipoFotoViewModel input)
        {
            var result = await _processoUseCase.CreateTipoFoto(input);

            if (!result.Status)
                return BadRequest(result.Mensagens);

            return Ok(result);
        }

        [HttpDelete("tipos-processos")]
        public async Task<IActionResult> DeleteTipoProcesso(int id)
        {
            var result = await _processoUseCase.DeleteTipoFoto(id);

            if (!result.Status)
                return BadRequest(result.Mensagens);

            return Ok(result);
        }

        [HttpGet("tipos-processos")]
        public async Task<IActionResult> GetImageTypes()
        {
            var result = await _processoUseCase.GetAllTipoFoto();

            if (!result.Status)
                return NotFound(result.Mensagens);

            return Ok(result);
        }

        [HttpGet("locais")]
        public async Task<IActionResult> GetLocais([FromQuery] string termo)
        {
            var result = await _descargaExportacaoUseCase.BuscarYard(termo);

            return Ok(result);
        }

        [HttpPost("processo")]
        public async Task<IActionResult> AnexarProcesso([FromBody] ProcessoViewModel input)
        {
            var result = await _processoUseCase.InsertProcesso(input);

            if (!result.Status)
                return NotFound(result.Mensagens);

            return Ok(result);

        }

        [HttpDelete("processo/{id}")]
        public async Task<IActionResult> DeletarProcesso(int id)
        {
            var result = await _processoUseCase.DeleteProcesso(id);

            if (!result.Status)
                return BadRequest(result.Mensagens);

            return Ok(result);

        }

        [HttpPut("processo")]
        public async Task<IActionResult> AtualizarProcesso([FromBody] UpdateTipoFotoViewModel input)
        {
            var result = await _processoUseCase.UpdateTipoFoto(input);

            if (!result.Status)
                return BadRequest(result);

            return Ok(result);

        }


        [HttpGet("processo/{talieId}")]
        public async Task<IActionResult> ListarProcessos(int talieId)
        {
            var result = await _processoUseCase.GetImagemByTalieId(talieId);

            if (!result.Status)
                return NotFound(result.Mensagens);

            return Ok(result);

        }

        [HttpGet("processo-container/{container}")]
        public async Task<IActionResult> ListarProcessosByContainer(string container)
        {
            var result = await _processoUseCase.GetImagemByContainer(container);

            if (!result.Status)
                return NotFound(result.Mensagens);

            return Ok(result);

        }

        [HttpPost("gravar-observacao")]
        public async Task<IActionResult> GravarObservacao(string observacao, int talieId)
        {
            var result = await _descargaExportacaoUseCase.GravarObservacao(observacao, talieId);

            if (!result.Status)
                return NotFound(result.Mensagens);

            return Ok(result);

        }

        [HttpDelete("excluir-talie-item/{registroId}")]
        public async Task<IActionResult> ExcluirTalieItem(int registroId, [FromQuery] int talieItemId, string notaFiscal)
        {
            var result = await _descargaExportacaoUseCase.ExcluirTalieItem(registroId, talieItemId, notaFiscal);

            if (!result.Status && !string.IsNullOrEmpty(result.Error))
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("carregar-armazens")]
        public async Task<IActionResult> CarregarArmazens([FromQuery] int patio)
        {
            var resultado = await _descargaExportacaoUseCase.CarregarArmazens(patio);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }


        [HttpPost("gravar-marcante")]
        public async Task<IActionResult> GravarMarcante([FromBody] MarcanteInput input)
        {
            var result = await _descargaExportacaoUseCase.GravarMarcante(input);

            if (!result.Status)
                return NotFound(result.Mensagens);

            return Ok(result);

        }

        [HttpGet("carregar-marcantes-talie-item")]
        public async Task<IActionResult> CarregarMarcantesTalieItem([FromQuery] int talieItemId)
        {
            var resultado = await _descargaExportacaoUseCase.CarregarMarcantes(talieItemId);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpDelete("excluir-marcante-talie-item")]
        public async Task<IActionResult> ExcluirTalieItem([FromQuery] int id)
        {
            var result = await _descargaExportacaoUseCase.ExcluirMarcanteTalieItem(id);

            if (!result.Status && !string.IsNullOrEmpty(result.Error))
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("finalizar-processo")]
        public async Task<IActionResult> FinalizarProcesso([FromQuery] int id, bool crossdock, string? user, string? container = null)
        {
            var result = await _descargaExportacaoUseCase.FinalizarProcesso(id, crossdock, user, container);
            if (!result.Status && !string.IsNullOrEmpty(result.Error))
                return NotFound(result);

            return Ok(result);
        }

        #region IMAGES
        [HttpGet("processo/{talieId}/zip")]
        public async Task<IActionResult> BaixarImagensZip(int talieId)
        {
            var result = await _processoUseCase.GetImagemByTalieId(talieId);

            if (!result.Status || result.Result == null || !result.Result.Any())
                return NotFound("Nenhuma imagem encontrada para esse processo.");

            var imagens = result.Result;

            // Obtém o caminho do appsettings
            var basePath = _configuration["Images:Path"];
            if (string.IsNullOrEmpty(basePath))
                return StatusCode(500, "Caminho das imagens não configurado.");

            using var memoryStream = new MemoryStream();
            using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var imagem in imagens)
                {
                    var fullImagePath = Path.Combine(basePath, imagem.ImagemPath.Replace("/", Path.DirectorySeparatorChar.ToString()));

                    if (!System.IO.File.Exists(fullImagePath))
                        continue;

                    var fileName = Path.GetFileName(fullImagePath);

                    var zipEntry = zip.CreateEntry(fileName, CompressionLevel.Fastest);
                    using var originalFileStream = System.IO.File.OpenRead(fullImagePath);
                    using var entryStream = zipEntry.Open();
                    await originalFileStream.CopyToAsync(entryStream);
                }
            }

            memoryStream.Position = 0;
            var zipFileName = $"processo_{talieId}_imagens.zip";

            return File(memoryStream.ToArray(), "application/zip", zipFileName);
        }


        #endregion
    }

}
