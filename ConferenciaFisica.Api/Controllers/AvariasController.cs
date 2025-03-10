using ConferenciaFisica.Application.Commands;
using ConferenciaFisica.Application.Inputs;
using ConferenciaFisica.Application.UseCases.Avarias.Interface;
using ConferenciaFisica.Application.UseCases.Documentos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConferenciaFisica.Api.Controllers
{
    [ApiController]
    [Route("api/avarias")]
    public class AvariasController : ControllerBase
    {
        private readonly ITiposAvariasUseCase _tiposAvariasUseCase;
        private readonly IAvariasConferenciaUseCase _avariasConferenciaUseCase;

        public AvariasController(ITiposAvariasUseCase tiposAvariasUseCase, IAvariasConferenciaUseCase avariasConferenciaUseCase)
        {
            _tiposAvariasUseCase = tiposAvariasUseCase;
            _avariasConferenciaUseCase = avariasConferenciaUseCase;
        }

        [HttpPost("cadastrar-avaria")]
        public async Task<IActionResult> CadastrarAvaria([FromBody] AvariaConferenciaInput input)
        {
            var result = await _avariasConferenciaUseCase.CadastrarAvariaConferencia(input);

            if (!result.Status)
                return NotFound(result.Mensagens);

            return Ok(result);

        }

        [HttpGet("tipos-listar")]
        public async Task<IActionResult> Get()
        {
            var resultado = await _tiposAvariasUseCase.GetAllAsync();

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpGet("avarias-conferencia")]
        public async Task<IActionResult> BuscarAvariasConferencia([FromQuery] int idConferencia)
        {
            var resultado = await _avariasConferenciaUseCase.BuscarAvariasConferencia(idConferencia);

            if (!resultado.Status && !string.IsNullOrEmpty(resultado.Error))
                return BadRequest(resultado);

            return Ok(resultado);
        }

    }
}
