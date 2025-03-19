using ConferenciaFisica.Application.Commands;
using ConferenciaFisica.Application.Inputs;
using ConferenciaFisica.Application.UseCases.DescargaExportacao.Interfaces;
using ConferenciaFisica.Application.UseCases.Documentos.Interfaces;
using ConferenciaFisica.Application.UseCases.Imagens;
using ConferenciaFisica.Application.UseCases.Imagens.Interfaces;
using ConferenciaFisica.Application.ViewModels;
using ConferenciaFisica.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ConferenciaFisica.Api.Controllers
{
    [ApiController]
    [Route("api/descarga-exportacao")]
    public class DescargaExportacaoController : ControllerBase
    {
        private readonly IDescargaExportacaoUseCase _descargaExportacaoUseCase;
        private readonly IImagensUseCaseUseCase _imagensUseCaseUseCase;

        public DescargaExportacaoController(IDescargaExportacaoUseCase descargaExportacaoUseCase, IImagensUseCaseUseCase imagensUseCaseUseCase)
        {
            _descargaExportacaoUseCase = descargaExportacaoUseCase;
            _imagensUseCaseUseCase = imagensUseCaseUseCase;
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
        public async Task<IActionResult> CreateTipoProcesso([FromBody]TipoProcessoViewModel input)
        {
            var result = await _imagensUseCaseUseCase.CreateTipoProcesso(input);

            if (!result.Status)
                return BadRequest(result.Mensagens);

            return Ok(result);
        }

        [HttpDelete("tipos-processos")]
        public async Task<IActionResult> DeleteTipoProcesso(int id)
        {
            var result = await _imagensUseCaseUseCase.DeleteTipoProcesso(id);

            if (!result.Status)
                return BadRequest(result.Mensagens);

            return Ok(result);
        }

        [HttpGet("tipos-processos")]
        public async Task<IActionResult> GetImageTypes()
        {
            var result = await _imagensUseCaseUseCase.ListTipoProcesso();

            if (!result.Status)
                return NotFound(result.Mensagens);

            return Ok(result);
        }

        [HttpPost("processo")]
        public async Task<IActionResult> AnexarProcesso([FromBody] ProcessoViewModel input)
        {
            var result = await _imagensUseCaseUseCase.InsertProcesso(input);

            if (!result.Status)
                return NotFound(result.Mensagens);

            return Ok(result);

        }

        [HttpDelete("processo")]
        public async Task<IActionResult> DeletarProcesso(IImagensUseCaseUseCase carregarTiposImagemUseCase)
        {
            var result = await carregarTiposImagemUseCase.ListTipoProcesso();

            if (!result.Status)
                return NotFound(result.Mensagens);

            return Ok(result);

        }

        [HttpGet("processo")]
        public async Task<IActionResult> ListarProcessos(IImagensUseCaseUseCase carregarTiposImagemUseCase)
        {
            var result = await carregarTiposImagemUseCase.ListTipoProcesso();

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
        public async Task<IActionResult> ExcluirTalieItem(int registroId, [FromQuery] int talieItemId)
        {
            var result = await _descargaExportacaoUseCase.ExcluirTalieItem(registroId, talieItemId);

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
    }

}
