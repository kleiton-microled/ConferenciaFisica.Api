using ConferenciaFisica.Application.UseCases.Documentos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConferenciaFisica.Api.Controllers
{
    [ApiController]
    [Route("api/tipos-documentos")]
    public class TiposDocumentosController : ControllerBase
    {
        private readonly ITiposDocumentosUseCase _tiposDocumentosUseCase;

        public TiposDocumentosController(ITiposDocumentosUseCase tiposDocumentosUseCase)
        {
            _tiposDocumentosUseCase = tiposDocumentosUseCase;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> Get()
        {
            var resultado = await _tiposDocumentosUseCase.GetAllAsync();

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

    }
}
