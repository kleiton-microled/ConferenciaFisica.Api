using ConferenciaFisica.Application.UseCases.Embalagens.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConferenciaFisica.Api.Controllers
{
    [ApiController]
    [Route("api/embalagens")]
    public class EmbalagensController : ControllerBase
    {
        private readonly IEmbalagensUseCase _embalagensUse;

        public EmbalagensController(IEmbalagensUseCase embalagensUseCase)
        {
            _embalagensUse = embalagensUseCase;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> Get()
        {
            var resultado = await _embalagensUse.GetAllAsync();

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

    }
}
