using ConferenciaFisica.Application.UseCases.Imagens.Interfaces;
using ConferenciaFisica.Application.UseCases.Utils;
using ConferenciaFisica.Application.UseCases.Utils.Interfaces;
using ConferenciaFisica.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ConferenciaFisica.Api.Controllers
{
    [ApiController]
    [Route("api/utils")]
    public class UtilController : ControllerBase
    {
        private readonly ITipoFotoUseCase _tipoFotoUseCase;
        private readonly ITiposProcessosUseCase _tipoProcessosUseCase;
        private readonly ITipoProcessoFotoUtilUseCase _tipoProcessosFotoUseCase;

        public UtilController(ITipoFotoUseCase processoUseCase, ITiposProcessosUseCase tipoProcessosUseCase, ITipoProcessoFotoUtilUseCase tipoProcessosFotoUseCase)
        {
            _tipoFotoUseCase = processoUseCase;
            _tipoProcessosUseCase = tipoProcessosUseCase;
            _tipoProcessosFotoUseCase = tipoProcessosFotoUseCase;
        }

        #region TipoFoto
        [HttpGet("/tipos-foto")]
        public async Task<IActionResult> GetTipoFoto()
        {
            var resultado = await _tipoFotoUseCase.GetAllTipoFoto();

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpPost("/tipos-foto")]
        public async Task<IActionResult> CreateTipoFoto([FromBody] TipoFotoViewModel input)
        {
            var resultado = await _tipoFotoUseCase.CreateTipoFoto(input);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpPut("/tipos-foto")]
        public async Task<IActionResult> UpdateTipoFoto([FromBody] UpdateTiposFotoViewModel input)
        {
            var resultado = await _tipoFotoUseCase.UpdateTiposFoto(input);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpDelete("/tipos-foto")]
        public async Task<IActionResult> DeleteTipoFoto(int id)
        {
            var resultado = await _tipoFotoUseCase.DeleteTipoFoto(id);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }
        #endregion

        #region TipoProcesso

        [HttpGet("/tipos-processo")]
        public async Task<IActionResult> GetTipoProcesso()
        {
            var resultado = await _tipoProcessosUseCase.GetAll();

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpPost("/tipos-processo")]
        public async Task<IActionResult> CreateTipoProcesso([FromBody] TipoProcessoViewModel input)
        {
            var resultado = await _tipoProcessosUseCase.Create(input);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpPut("/tipos-processo/{id}")]
        public async Task<IActionResult> UpdateTipoProcesso([FromBody] TipoProcessoViewModel input, int id)
        {
            var resultado = await _tipoProcessosUseCase.Update(id, input);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpDelete("/tipos-processo/{id}")]
        public async Task<IActionResult> DeleteTipoProcesso(int id)
        {
            var resultado = await _tipoProcessosUseCase.Delete(id);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpGet("/tipos-processo/{id}")]
        public async Task<IActionResult> GetTipoProcesso(int id)
        {
            var resultado = await _tipoProcessosUseCase.Get(id);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        #endregion

        #region TipoProcessoFoto

        [HttpGet("/tipos-processo-foto")]
        public async Task<IActionResult> GetTipoProcessoFoto()
        {
            var resultado = await _tipoProcessosFotoUseCase.GetAll();

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpPost("/tipos-processo-foto")]
        public async Task<IActionResult> CreateTipoProcessoFoto([FromBody] TipoProcessoFotoViewModel input)
        {
            var resultado = await _tipoProcessosFotoUseCase.Create(input);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpPut("/tipos-processo-foto/{id}")]
        public async Task<IActionResult> UpdateTipoProcessoFoto([FromBody] TipoProcessoFotoViewModel input, int id)
        {
            var resultado = await _tipoProcessosFotoUseCase.Update(id, input);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpDelete("/tipos-processo-foto/{id}")]
        public async Task<IActionResult> DeleteTipoProcessoFoto(int id)
        {
            var resultado = await _tipoProcessosFotoUseCase.Delete(id);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpGet("/tipos-processo-foto/{id}")]
        public async Task<IActionResult> GetTipoProcessoFoto(int id)
        {
            var resultado = await _tipoProcessosFotoUseCase.Get(id);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpGet("/tipos-processo-foto/processo/{processoName}")]
        public async Task<IActionResult> GetTipoProcessoFotoByProcessoId(string processoName)
        {
            var resultado = await _tipoProcessosFotoUseCase.GetByProcessoNome(processoName);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }
        #endregion
    }
}
