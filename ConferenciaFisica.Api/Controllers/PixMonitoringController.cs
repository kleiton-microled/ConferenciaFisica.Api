using ConferenciaFisica.Application.UseCases.PixMonitoring.Interfaces;
using ConferenciaFisica.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ConferenciaFisica.Contracts.Common;

namespace ConferenciaFisica.Api.Controllers
{
    [ApiController]
    [Route("api/pix-monitoring")]
    public class PixMonitoringController : ControllerBase
    {
        private readonly IPixMonitoringUseCase _pixMonitoringUseCase;

        public PixMonitoringController(IPixMonitoringUseCase pixMonitoringUseCase)
        {
            _pixMonitoringUseCase = pixMonitoringUseCase;
        }

        [HttpGet("list-all-pix")]
        //[Authorize]
        public async Task<IActionResult> ListAllPix([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 25)
        {
            var pagination = new PaginationInput
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var resultado = await _pixMonitoringUseCase.ListAllPixWithPaginationAsync(pagination);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpGet("list-all-pix-paginated")]
        //[Authorize]
        public async Task<IActionResult> ListAllPixPaginated([FromQuery] PaginationInput pagination)
        {
            var resultado = await _pixMonitoringUseCase.ListAllPixWithPaginationAsync(pagination);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpGet("list-all-pix-unlimited")]
        //[Authorize]
        public async Task<IActionResult> ListAllPixUnlimited()
        {
            var resultado = await _pixMonitoringUseCase.ListAllPixAsync();

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpGet("get-total-pix-ativos")]
        //[Authorize]
        public async Task<IActionResult> GetTotalPixAtivos()
        {
            var resultado = await _pixMonitoringUseCase.GetTotalPixAtivosAsync();

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpGet("get-total-pix-pagos")]
        //[Authorize]
        public async Task<IActionResult> GetTotalPixPagos()
        {
            var resultado = await _pixMonitoringUseCase.GetTotalPixPagosAsync();

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpGet("get-total-pix-cancelados")]
        //[Authorize]
        public async Task<IActionResult> GetTotalPixCancelados()
        {
            var resultado = await _pixMonitoringUseCase.GetTotalPixCanceladosAsync();

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }
    }
} 