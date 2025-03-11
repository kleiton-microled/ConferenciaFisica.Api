using ConferenciaFisica.Application.UseCases.DescargaExportacao.Interfaces;
using ConferenciaFisica.Application.UseCases.Documentos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConferenciaFisica.Api.Controllers
{
    [ApiController]
    [Route("api/descarga-exportacao")]
    public class DescargaExportacaoController : ControllerBase
    {
        private readonly IDescargaExportacaoUseCase _descargaExportacaoUseCase;

        public DescargaExportacaoController(IDescargaExportacaoUseCase descargaExportacaoUseCase)
        {
            _descargaExportacaoUseCase = descargaExportacaoUseCase;
        }

        [HttpGet("buscar-registro")]
        public async Task<IActionResult> Get(int registro)
        {
            var resultado = await _descargaExportacaoUseCase.BuscarPorRegistro(registro);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

    }
}
