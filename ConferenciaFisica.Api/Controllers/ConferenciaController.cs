using ConferenciaFisica.Application.Commands;
using ConferenciaFisica.Application.Inputs;
using ConferenciaFisica.Application.UseCases.Agendamento;
using ConferenciaFisica.Application.UseCases.Agendamento.Interfaces;
using ConferenciaFisica.Application.UseCases.Conferencia;
using ConferenciaFisica.Application.UseCases.Conferencia.Interfaces;
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
        private readonly IIniciarConferenciaUseCase _iniciarConferenciaUseCase;
        private readonly IAtualizarConferenciaUseCase _atualizarConferenciaUseCase;
        private readonly ICadastrosAdicionaisUseCase _cadastrosAdicionaisUseCase;



        public ConferenciaController(IBuscarConferenciaUseCase buscarConferenciaUseCase,
                                     ICarregarLotesAgendamentoUseCase carregarLotesAgendamentoUseCase,
                                     ICarregarCntrAgendamentoUseCase carregarCntrAgendamentoUseCase,
                                     IIniciarConferenciaUseCase iniciarConferenciaUseCase,
                                     IAtualizarConferenciaUseCase atualizarConferenciaUseCase,
                                     ICadastrosAdicionaisUseCase cadastrosAdicionaisUseCase)
        {
            _buscarConferenciaUseCase = buscarConferenciaUseCase;
            _carregarLotesAgendamentoUseCase = carregarLotesAgendamentoUseCase;
            _carregarCntrAgendamentoUseCase = carregarCntrAgendamentoUseCase;
            _iniciarConferenciaUseCase = iniciarConferenciaUseCase;
            _atualizarConferenciaUseCase = atualizarConferenciaUseCase;
            _cadastrosAdicionaisUseCase = cadastrosAdicionaisUseCase;
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarConferencia([FromQuery] string cntr = "", [FromQuery] string lote = "")
        {
            var resultado = await _buscarConferenciaUseCase.ExecuteAsync(cntr, lote);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
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

            if (!result.Status && !string.IsNullOrEmpty(result.Error))
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost("iniciar-conferencia")]
        public async Task<IActionResult> IniciarConferencia([FromBody] ConferenciaFisicaRequest request)
        {
            var result = await _iniciarConferenciaUseCase.ExecuteAsync(request);

            if (!result.Status)
                return NotFound(result.Mensagens);

            return Ok(result);

        }

        [HttpPost("atualizar-conferencia")]
        public async Task<IActionResult> AtualizarConferencia([FromBody] ConferenciaFisicaRequest request)
        {
            var result = await _atualizarConferenciaUseCase.ExecuteAsync(request);

            if (!result.Status)
                return NotFound(result.Mensagens);

            return Ok(result);

        }

        
        [HttpPost("cadastro-adicional")]
        public async Task<IActionResult> CadastrosAdicionaisParaConferencia([FromBody] CadastroAdicionalInput request)
        {
            var result = await _cadastrosAdicionaisUseCase.ExecuteAsync(request);

            if (!result.Status)
                return NotFound(result.Mensagens);

            return Ok(result);

        }

        [HttpGet("carregar-cadastros-adicionais")]
        public async Task<IActionResult> CarregarCadastrosAdicionais([FromQuery] int idConferencia)
        {
            var result = await _cadastrosAdicionaisUseCase.CarregarCadastrosAdicionais(idConferencia);

            if (!result.Status && !string.IsNullOrEmpty(result.Error))
                return NotFound(result);

            return Ok(result);
        }

    }
}
