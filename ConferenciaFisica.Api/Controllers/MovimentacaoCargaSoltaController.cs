using ConferenciaFisica.Application.UseCases.MovimentacaoCargaSolta;
using ConferenciaFisica.Application.ViewModels;
using ConferenciaFisica.Contracts.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ConferenciaFisica.Api.Controllers
{
    [ApiController]
    [Route("api/movimentacao-carga-solta")]
    public class MovimentacaoCargaSoltaController : ControllerBase
    {
        private readonly IMovimentacaoCargaSoltaUseCase _useCase;

        public MovimentacaoCargaSoltaController(IMovimentacaoCargaSoltaUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet]
        public async Task<IActionResult> BuscarCargaParaMovimentacao([FromQuery] int idMarcante)
        {
            var resultado = await _useCase.BuscarCargaParaMovimentar(idMarcante);
            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpPost("movimentar")]
        public async Task<IActionResult> Movimentar([FromBody] MovimentacaoCargaDTO request)
        {
            var result = await _useCase.Movimentar(request);

            if (!result.Status)
                return NotFound(result.Mensagens);

            return Ok(result);

        }
    }
}
