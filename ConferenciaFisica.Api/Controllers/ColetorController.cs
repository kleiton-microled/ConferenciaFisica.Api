using ConferenciaFisica.Application.UseCases.Conferentes;
using ConferenciaFisica.Application.UseCases.Embalagens.Interfaces;
using ConferenciaFisica.Application.UseCases.Equipes;
using ConferenciaFisica.Application.UseCases.Marcantes;
using ConferenciaFisica.Application.UseCases.Patios;
using Microsoft.AspNetCore.Mvc;

namespace ConferenciaFisica.Api.Controllers
{
    [ApiController]
    [Route("api/coletor")]
    public class ColetorController : ControllerBase
    {
        private readonly IMarcantesUseCase _marcantesUse;
        private readonly IConferentesUseCase _conferenteUseCase;
        private readonly IEquipesUseCase _equipesUseCase;
        private readonly IPatiosUseCase _patiosUseCase;
        //private readonly IVeiculoUseCase _veiculoUseCase;

        public ColetorController(IMarcantesUseCase marcantesUseCase, 
                                 IConferentesUseCase conferenteUseCase, 
                                 IEquipesUseCase equipesUseCase, 
                                 IPatiosUseCase patiosUseCase)
        {
            _marcantesUse = marcantesUseCase;
            _conferenteUseCase = conferenteUseCase;
            _equipesUseCase = equipesUseCase;
            _patiosUseCase = patiosUseCase;
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

        [HttpGet("equipes")]
        public async Task<IActionResult> GetEquipes()
        {
            var result = await _equipesUseCase.ListarEquipes();

            return Ok(result);
        }

        [HttpGet("patios")]
        public async Task<IActionResult> ListarPatios()
        {
            var result = await _patiosUseCase.ListarPatios();

            return Ok(result);
        }

        [HttpGet("veiculos")]
        public async Task<IActionResult> ListarPatios(DateTime? dataChegada)
        {
            var result = await _patiosUseCase.ListarPatios();

            return Ok(result);
        }


    }
}
