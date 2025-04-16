using ConferenciaFisica.Api.Transport.PreRegistro;
using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.UseCases.PreRegistro;
using ConferenciaFisica.Application.ViewModels;
using ConferenciaFisica.Domain.Entities.PreRegistro;
using Microsoft.AspNetCore.Mvc;

namespace ConferenciaFisica.Api.Controllers
{
    [ApiController]
    [Route("api/pre-registro")]
    public class PreRegistroController : ControllerBase
    {
        private readonly IPreRegistroUseCase _preRegistroUseCase;

        public PreRegistroController(IPreRegistroUseCase preRegistroUseCase)
        {
            _preRegistroUseCase = preRegistroUseCase;
        }

        [HttpPost("dados-agendamento")]
        public async Task<IActionResult> GetDadosAgendamento(PreRegistroInput input)
        {
            var resultado = await _preRegistroUseCase.GetDadosAgendamento(input);

            return resultado.Status ? Ok(resultado) : BadRequest(resultado);
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> PostRegistrar(SaidaCaminhaoViewModel input)
        {
           var resultado = await _preRegistroUseCase.Cadastrar(input);

            return resultado.Status ? Ok(resultado) : BadRequest(resultado);
        }

        //[HttpPost]
        //public async Task<IActionResult> PostEntradaSemAgendamento(string placaCavalo, string placaCarreta, string Ticketent, string finalidadeId, int patioId)
        //{
        //    return Ok();
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetVerificaSaidaPlaca(string placa)
        //{
        //    return Ok();
        //}
    }
}
