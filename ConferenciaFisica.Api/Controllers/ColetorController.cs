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

        public ColetorController(IMarcantesUseCase marcantesUseCase)
        {
            _marcantesUse = marcantesUseCase;
        }

        [HttpGet("marcantes")]
        public async Task<IActionResult> GetMarcantes([FromQuery] string termo)
        {
            var result = await _marcantesUse.BuscarMarcantes(termo);

            return Ok(result);
        }

    }
}
