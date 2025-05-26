using ConferenciaFisica.Application.UseCases.SaidaCaminhao.Interfaces;
using ConferenciaFisica.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ConferenciaFisica.Api.Controllers
{
    [ApiController]
    [Route("api/saida-caminhao")]
    public class SaidaCaminhaoController : ControllerBase
    {
        private ISaidaDeCaminhaoUseCase _saidaDeCaminhaoUseCase;

        public SaidaCaminhaoController(ISaidaDeCaminhaoUseCase saidaDeCaminhaoUseCase)
        {
            _saidaDeCaminhaoUseCase = saidaDeCaminhaoUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> GetDadosCaminhao(string protocolo, string ano, string placa, string placaCarreta,int patio)
        {
            var resultado = await _saidaDeCaminhaoUseCase.GetDadosCaminhao(protocolo, ano, placa, placaCarreta, patio);

            if (!resultado.Status)
                return BadRequest(resultado);

            return resultado.Status ? Ok(resultado) : BadRequest(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarSaida(SaidaCaminhaoViewModel input)
        {
            var resultado = await _saidaDeCaminhaoUseCase.RegistrarSaida(input);

            return resultado.Status ? Ok(resultado) : BadRequest(resultado);
        }
    }
}
