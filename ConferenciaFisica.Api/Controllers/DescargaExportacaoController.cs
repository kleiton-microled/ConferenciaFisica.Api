using ConferenciaFisica.Application.Commands;
using ConferenciaFisica.Application.Inputs;
using ConferenciaFisica.Application.UseCases.DescargaExportacao.Interfaces;
using ConferenciaFisica.Application.UseCases.Documentos.Interfaces;
using ConferenciaFisica.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ConferenciaFisica.Api.Controllers
{
    [ApiController]
    [Route("api/descarga-exportacao")]
    public class DescargaExportacaoController : ControllerBase
    {
        private readonly IDescargaExportacaoUseCase _descargaExportacaoUseCase;

        public DescargaExportacaoController(IDescargaExportacaoUseCase descargaExportacaoUseCase)
        {
            _descargaExportacaoUseCase = descargaExportacaoUseCase;
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

        [HttpPost("gravar-observacao")]
        public async Task<IActionResult> GravarObservacao(string observacao, int talieId)
        {
            var result = await _descargaExportacaoUseCase.GravarObservacao(observacao, talieId);

            if (!result.Status)
                return NotFound(result.Mensagens);

            return Ok(result);

        }

        [HttpDelete("excluir-talie-item/{registroId}")]
        public async Task<IActionResult> ExcluirTalieItem(int registroId, [FromQuery] int talieId)
        {
            var result = await _descargaExportacaoUseCase.ExcluirTalieItem(registroId, talieId);

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
