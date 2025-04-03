using ConferenciaFisica.Application.UseCases.Conferentes;
using ConferenciaFisica.Application.UseCases.Embalagens.Interfaces;
using ConferenciaFisica.Application.UseCases.Marcantes;
using Microsoft.AspNetCore.Mvc;

namespace ConferenciaFisica.Api.Controllers
{
    [ApiController]
    [Route("api/coletor")]
    public class ColetorController : ControllerBase
    {
        private readonly IMarcantesUseCase _marcantesUse;
        private readonly IConferentesUseCase _conferenteUseCase;

        public ColetorController(IMarcantesUseCase marcantesUseCase, IConferentesUseCase conferenteUseCase)
        {
            _marcantesUse = marcantesUseCase;
            _conferenteUseCase = conferenteUseCase;
        }

        [HttpGet("marcantes")]
        public async Task<IActionResult> GetMarcantes([FromQuery] string termo)
        {
            var result = await _marcantesUse.BuscarMarcantes(termo);

            return Ok(result);
        }

        [HttpGet("conferentes")]
        public async Task<IActionResult> GetConferentes()
        {
            var result = await _conferenteUseCase.ListarConferentes();

            return Ok(result);
        }

    }
}
