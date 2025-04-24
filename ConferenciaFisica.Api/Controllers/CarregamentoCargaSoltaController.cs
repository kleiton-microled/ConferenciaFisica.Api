using ConferenciaFisica.Api.Transport.CargaSolta;
using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.UseCases.CarregamentoCargaSolta.Interface;
using ConferenciaFisica.Contracts.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ConferenciaFisica.Api.Controllers
{
    [ApiController]
    [Route("api/carregamento-carga-solta")]
    public class CarregamentoCargaSoltaController : ControllerBase
    {
        private readonly ICarregamentoCargaSoltaUseCase _carregamentoCargaSoltaUseCase;

        public CarregamentoCargaSoltaController(ICarregamentoCargaSoltaUseCase carregamentoCargaSoltaUseCase)
        {
            _carregamentoCargaSoltaUseCase = carregamentoCargaSoltaUseCase;
        }

        [HttpGet("veiculos")]
        public async Task<IActionResult> GetVeiculos(int patio)
        {
            var resultado = await _carregamentoCargaSoltaUseCase.GetVeiculos(patio);

            return resultado.Status ? Ok(resultado) : BadRequest(resultado);
        }

        [HttpGet("ordens")]
        public async Task<IActionResult> GetOrdens(int? patio, string? veiculo, string? local, int? quantidade, DateTime? inicio, string tipo = "I")
        {
            var resultado = await _carregamentoCargaSoltaUseCase.GetOrdens(patio, veiculo, local, quantidade, inicio);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> GetBuscarMarcante(int marcante, int patio)
        {
            var resultado = await _carregamentoCargaSoltaUseCase.BuscarMacantes(marcante, patio);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpGet("salvar")]
        public async Task<IActionResult> PostSalvarMarcante(int marcante, int? patio, string local, string placa)
        {
            var resultado = await _carregamentoCargaSoltaUseCase.SalvarMacantes(marcante, patio, local, placa);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpPost("iniciar")]
        public async Task<IActionResult> PostIniciar(string veiculo)
        {
            var resultado = await _carregamentoCargaSoltaUseCase.Iniciar(veiculo);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpPost("finalizar")]
        public async Task<IActionResult> PostFinalizar([FromBody] FinalizarCargaSolta input)
        {
            var resultado = await _carregamentoCargaSoltaUseCase.Finalizar(input.Placa, input.Usuario, input.Inico);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }
    }
}
