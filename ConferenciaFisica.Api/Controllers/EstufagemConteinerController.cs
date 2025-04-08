using ConferenciaFisica.Application.UseCases.estufagemConteiner.Interfaces;
using ConferenciaFisica.Application.ViewModels;
using ConferenciaFisica.Contracts.DTOs.EstufagemConteiner;
using Microsoft.AspNetCore.Mvc;

namespace ConferenciaFisica.Api.Controllers
{
    [ApiController]
    [Route("api/estufagem-conteiner")]
    public class EstufagemConteinerController : ControllerBase
    {
        private readonly IPlanejamentoUseCase _planejamentoUseCase;
        private readonly IItensEstufadosUseCase _itensEstufadosUseCase;
        private readonly IEtiquetasUseCase _etiquetasUseCase;

        public EstufagemConteinerController(IPlanejamentoUseCase planjamentoUseCase,
                                            IItensEstufadosUseCase itensEstufadosUseCase,
                                            IEtiquetasUseCase etiquetasUseCase)
        {
            _planejamentoUseCase = planjamentoUseCase;
            _itensEstufadosUseCase = itensEstufadosUseCase;
            _etiquetasUseCase = etiquetasUseCase;
        }

        [HttpGet("planejamento")]
        public async Task<IActionResult> CarregarPlanejamento([FromQuery] int planejamento)
        {
            var resultado = await _planejamentoUseCase.BuscarPlanejamento(planejamento);

            if (!resultado.Status && !string.IsNullOrEmpty(resultado.Error))
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpGet("itens-estufados")]
        public async Task<IActionResult> CarregarItensEstufados([FromQuery] int patio)
        {
            var resultado = await _itensEstufadosUseCase.BuscarItensEstufados(patio);

            if (!resultado.Status && !string.IsNullOrEmpty(resultado.Error))
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpGet("saldo-carga-marcante")]
        public async Task<IActionResult> BuscarSaldoCargaMarcante([FromQuery] int planejamento, string codigoMarcante)
        {
            var resultado = await _planejamentoUseCase.BuscarSaldoCargaMarcante(planejamento, codigoMarcante);

            if (!resultado.Status && !string.IsNullOrEmpty(resultado.Error))
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpGet("etiquetas")]
        public async Task<IActionResult> CarregarEtiquetas([FromQuery] int planejamento)
        {
            var resultado = await _etiquetasUseCase.BuscarEtiquetas(planejamento);

            if (!resultado.Status && !string.IsNullOrEmpty(resultado.Error))
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpPost("iniciar-estufagem")]
        public async Task<IActionResult> GravarTalie([FromBody] TalieInsertDTO request)
        {
            var result = await _planejamentoUseCase.IniciarEstufagem(request);

            if (!result.Status)
                return NotFound(result.Mensagens);

            return Ok(result);

        }


    }
}
