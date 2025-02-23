using ConferenciaFisica.Application.UseCases.Agendamento;
using ConferenciaFisica.Application.UseCases.Agendamento.Interfaces;
using ConferenciaFisica.Application.UseCases.Conferencia;
using Microsoft.AspNetCore.Mvc;

namespace ConferenciaFisica.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConferenciaController : ControllerBase
    {
        private readonly IBuscarConferenciaUseCase _buscarConferenciaUseCase;
        private readonly ICarregarLotesAgendamentoUseCase _carregarLotesAgendamentoUseCase;
        private readonly ICarregarCntrAgendamentoUseCase _carregarCntrAgendamentoUseCase;



        public ConferenciaController(IBuscarConferenciaUseCase buscarConferenciaUseCase,
                                     ICarregarLotesAgendamentoUseCase carregarLotesAgendamentoUseCase,
                                     ICarregarCntrAgendamentoUseCase carregarCntrAgendamentoUseCase)
        {
            _buscarConferenciaUseCase = buscarConferenciaUseCase;
            _carregarLotesAgendamentoUseCase = carregarLotesAgendamentoUseCase;
            _carregarCntrAgendamentoUseCase = carregarCntrAgendamentoUseCase;
        }

        [HttpGet("buscar/{idConteiner}")]
        public async Task<IActionResult> BuscarConferencia(string idConteiner, string lote)
        {
            var result = await _buscarConferenciaUseCase.ExecuteAsync(idConteiner, lote);

            if (!result.Status)
                return NotFound(result);

            return Ok(result.Result);
        }

        [HttpGet("lotes")]
        public async Task<IActionResult> CarregarLotesAgendamento([FromQuery] string filtro = "")
        {
            var result = await _carregarLotesAgendamentoUseCase.ExecuteAsync(filtro);

            if (!result.Status)
                return NotFound(result.Mensagens);

            return Ok(result);
        }

        [HttpGet("conteineres")]
        public async Task<IActionResult> CarregarCntrAgendamento([FromQuery] string filtro = "")
        {
            var result = await _carregarCntrAgendamentoUseCase.ExecuteAsync(filtro);

            if (!result.Status)
                return NotFound(result.Mensagens);

            return Ok(result);
        }
    }
}
