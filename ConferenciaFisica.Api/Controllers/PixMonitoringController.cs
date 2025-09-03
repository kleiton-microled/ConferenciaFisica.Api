using ConferenciaFisica.Application.UseCases.PixMonitoring.Interfaces;
using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Contracts.DTOs;
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

        [HttpGet("list-pix-with-filter")]
        //[Authorize]
        public async Task<IActionResult> ListPixWithFilter([FromQuery] PixFiltroInput filtro)
        {
            // Validações básicas
            if (filtro.PageNumber <= 0) filtro.PageNumber = 1;
            if (filtro.PageSize <= 0) filtro.PageSize = 25;
            if (filtro.PageSize > 100) filtro.PageSize = 100; // Limitar o tamanho máximo da página

            // Validar status se fornecido
            if (!string.IsNullOrEmpty(filtro.Status))
            {
                var statusValidos = new[] { PixStatusFilter.Ativo, PixStatusFilter.Pago, PixStatusFilter.Cancelado };
                if (!statusValidos.Contains(filtro.Status.ToLower()))
                {
                    return BadRequest(new ServiceResult<object>
                    {
                        Status = false,
                        Mensagens = new List<string> { $"Status inválido. Valores válidos: {string.Join(", ", statusValidos)}" }
                    });
                }
            }

            // Validar datas se fornecidas
            if (filtro.DataCriacaoInicial.HasValue && filtro.DataCriacaoFinal.HasValue)
            {
                if (filtro.DataCriacaoInicial > filtro.DataCriacaoFinal)
                {
                    return BadRequest(new ServiceResult<object>
                    {
                        Status = false,
                        Mensagens = new List<string> { "Data inicial não pode ser maior que a data final." }
                    });
                }
            }

            var resultado = await _pixMonitoringUseCase.ListPixWithFilterAsync(filtro);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpGet("get-total-pix-ativos-with-filter")]
        //[Authorize]
        public async Task<IActionResult> GetTotalPixAtivosWithFilter([FromQuery] PixFiltroInput filtro)
        {
            // Validar datas se fornecidas
            if (filtro.DataCriacaoInicial.HasValue && filtro.DataCriacaoFinal.HasValue)
            {
                if (filtro.DataCriacaoInicial > filtro.DataCriacaoFinal)
                {
                    return BadRequest(new ServiceResult<object>
                    {
                        Status = false,
                        Mensagens = new List<string> { "Data inicial não pode ser maior que a data final." }
                    });
                }
            }

            var resultado = await _pixMonitoringUseCase.GetTotalPixAtivosComFiltroAsync(filtro);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpGet("get-total-pix-pagos-with-filter")]
        //[Authorize]
        public async Task<IActionResult> GetTotalPixPagosWithFilter([FromQuery] PixFiltroInput filtro)
        {
            // Validar datas se fornecidas
            if (filtro.DataCriacaoInicial.HasValue && filtro.DataCriacaoFinal.HasValue)
            {
                if (filtro.DataCriacaoInicial > filtro.DataCriacaoFinal)
                {
                    return BadRequest(new ServiceResult<object>
                    {
                        Status = false,
                        Mensagens = new List<string> { "Data inicial não pode ser maior que a data final." }
                    });
                }
            }

            var resultado = await _pixMonitoringUseCase.GetTotalPixPagosComFiltroAsync(filtro);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpGet("get-total-pix-cancelados-with-filter")]
        //[Authorize]
        public async Task<IActionResult> GetTotalPixCanceladosWithFilter([FromQuery] PixFiltroInput filtro)
        {
            // Validar datas se fornecidas
            if (filtro.DataCriacaoInicial.HasValue && filtro.DataCriacaoFinal.HasValue)
            {
                if (filtro.DataCriacaoInicial > filtro.DataCriacaoFinal)
                {
                    return BadRequest(new ServiceResult<object>
                    {
                        Status = false,
                        Mensagens = new List<string> { "Data inicial não pode ser maior que a data final." }
                    });
                }
            }

            var resultado = await _pixMonitoringUseCase.GetTotalPixCanceladosComFiltroAsync(filtro);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }
    }
} 