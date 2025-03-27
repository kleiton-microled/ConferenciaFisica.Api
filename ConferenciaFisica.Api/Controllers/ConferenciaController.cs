using ConferenciaFisica.Application.Commands;
using ConferenciaFisica.Application.Inputs;
using ConferenciaFisica.Application.UseCases.Agendamento.Interfaces;
using ConferenciaFisica.Application.UseCases.Conferencia;
using ConferenciaFisica.Application.UseCases.Conferencia.Interfaces;
using ConferenciaFisica.Application.UseCases.Documentos.Interfaces;
using ConferenciaFisica.Application.UseCases.Lacres.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConferenciaFisica.Api.Controllers
{
    [ApiController]
    [Route("api/conferencia")]
    public class ConferenciaController : ControllerBase
    {
        private readonly IBuscarConferenciaUseCase _buscarConferenciaUseCase;
        private readonly ICarregarLotesAgendamentoUseCase _carregarLotesAgendamentoUseCase;
        private readonly ICarregarCntrAgendamentoUseCase _carregarCntrAgendamentoUseCase;
        private readonly IIniciarConferenciaUseCase _iniciarConferenciaUseCase;
        private readonly IAtualizarConferenciaUseCase _atualizarConferenciaUseCase;
        private readonly ICadastrosAdicionaisUseCase _cadastrosAdicionaisUseCase;
        private readonly ITiposLacresUseCase _tiposLacresUseCase;
        private readonly ILacresUseCase _lacresUseCase;
        private readonly IDocumentoConferenciaUseCase _documentosUseCase;
        public ConferenciaController(IBuscarConferenciaUseCase buscarConferenciaUseCase,
                                     ICarregarLotesAgendamentoUseCase carregarLotesAgendamentoUseCase,
                                     ICarregarCntrAgendamentoUseCase carregarCntrAgendamentoUseCase,
                                     IIniciarConferenciaUseCase iniciarConferenciaUseCase,
                                     IAtualizarConferenciaUseCase atualizarConferenciaUseCase,
                                     ICadastrosAdicionaisUseCase cadastrosAdicionaisUseCase,
                                     ITiposLacresUseCase tiposLacresUseCase,
                                     ILacresUseCase lacresUseCase,
                                     IDocumentoConferenciaUseCase documentosUseCase)
        {
            _buscarConferenciaUseCase = buscarConferenciaUseCase;
            _carregarLotesAgendamentoUseCase = carregarLotesAgendamentoUseCase;
            _carregarCntrAgendamentoUseCase = carregarCntrAgendamentoUseCase;
            _iniciarConferenciaUseCase = iniciarConferenciaUseCase;
            _atualizarConferenciaUseCase = atualizarConferenciaUseCase;
            _cadastrosAdicionaisUseCase = cadastrosAdicionaisUseCase;
            _tiposLacresUseCase = tiposLacresUseCase;
            _lacresUseCase = lacresUseCase;
            _documentosUseCase = documentosUseCase;
        }

        [HttpGet("teste-auth")]
        [Authorize]
        public IActionResult Teste()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            return Ok(new { userId, roles });
        }


        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarConferencia([FromQuery] string cntr = "", [FromQuery] string lote = "")
        {
            var resultado = await _buscarConferenciaUseCase.ExecuteAsync(cntr, lote);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpGet("buscar-por-id")]
        public async Task<IActionResult> BuscarConferenciaPorId([FromQuery] int id)
        {
            var resultado = await _buscarConferenciaUseCase.BuscarPorId(id);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [Authorize]
        [HttpGet("lotes")]
        public async Task<IActionResult> CarregarLotesAgendamento([FromQuery] string filtro = "")
        {
            var result = await _carregarLotesAgendamentoUseCase.ExecuteAsync(filtro);

            if (!result.Status && !string.IsNullOrEmpty(result.Error))
                return NotFound(result.Mensagens);

            return Ok(result);
        }

        [Authorize]
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
            var result = await _cadastrosAdicionaisUseCase.GetAllAsync(idConferencia);

            if (!result.Status && !string.IsNullOrEmpty(result.Error))
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("excluir-cadastro-adicional")]
        public async Task<IActionResult> ExcluirCadastroAdicional([FromQuery] int id)
        {
            var result = await _cadastrosAdicionaisUseCase.DeleteAsync(id);

            if (!result.Status && !string.IsNullOrEmpty(result.Error))
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("tipos-lacres")]
        public async Task<IActionResult> TiposLAcres()
        {
            var result = await _tiposLacresUseCase.GetAllAsync();

            if (!result.Status && !string.IsNullOrEmpty(result.Error))
                return NotFound(result);

            return Ok(result);
        }

        //Lacres Conferencia
        [HttpGet("lacres-conferencia")]
        public async Task<IActionResult> GetAllLacresConferencia([FromQuery] int idConferencia)
        {
            var result = await _lacresUseCase.GetAllAsync(idConferencia);

            if (!result.Status && !string.IsNullOrEmpty(result.Error))
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost("cadastro-lacres-conferencia")]
        public async Task<IActionResult> CadastrosLacresConferencia([FromBody] LacreConferenciaInput request)
        {
            var result = await _lacresUseCase.ExecuteAsync(request);

            if (!result.Status)
                return NotFound(result.Mensagens);

            return Ok(result);

        }

        [HttpPost("atualizar-lacre-conferencia")]
        public async Task<IActionResult> AtualizarLacreConferencia([FromBody] LacreConferenciaInput request)
        {
            var result = await _lacresUseCase.UpdateAsync(request);

            if (!result.Status)
                return NotFound(result.Mensagens);

            return Ok(result);

        }

        [HttpDelete("excluir-lacre-conferencia")]
        public async Task<IActionResult> ExcluirLacreConferencia([FromQuery] int id)
        {
            var result = await _lacresUseCase.DeleteAsync(id);

            if (!result.Status && !string.IsNullOrEmpty(result.Error))
                return NotFound(result);

            return Ok(result);
        }

        //Documentos Conferencia
        [HttpGet("documentos-conferencia")]
        public async Task<IActionResult> GetAllDocumentosConferencia([FromQuery] int idConferencia)
        {
            var result = await _documentosUseCase.GetAllAsync(idConferencia);

            if (!result.Status && !string.IsNullOrEmpty(result.Error))
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost("cadastro-documento-conferencia")]
        public async Task<IActionResult> CadastrosDocumentoConferencia([FromBody] DocumentoConferenciaInput request)
        {
            var result = await _documentosUseCase.ExecuteAsync(request);

            if (!result.Status)
                return NotFound(result.Mensagens);

            return Ok(result);

        }

        [HttpPost("atualizar-documento-conferencia")]
        public async Task<IActionResult> AtualizarDocumentoConferencia([FromBody] DocumentoConferenciaInput request)
        {
            var result = await _documentosUseCase.UpdateAsync(request);

            if (!result.Status)
                return NotFound(result.Mensagens);

            return Ok(result);

        }

        [HttpDelete("excluir-documento-conferencia")]
        public async Task<IActionResult> ExcluirDocumentoConferencia([FromQuery] int id)
        {
            var result = await _documentosUseCase.DeleteAsync(id);

            if (!result.Status && !string.IsNullOrEmpty(result.Error))
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("finalizar-conferencia")]
        public async Task<IActionResult> FinalizarConferencia([FromQuery] int idConferencia)
        {
            var result = await _atualizarConferenciaUseCase.FinalizarConferencia(idConferencia);

            if (!result.Status && !string.IsNullOrEmpty(result.Error))
                return NotFound(result);

            return Ok(result);
        }

    }
}
