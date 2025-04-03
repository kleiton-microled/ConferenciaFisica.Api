using ConferenciaFisica.Application.UseCases.MovimentacaoContainer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConferenciaFisica.Api.Controllers
{
    [ApiController]
    [Route("api/movimentacao-container")]
    public class MovimentacaoContainerController : ControllerBase
    {
        private readonly IMovimentacaoContainerUseCase _movimentacaoContainerUseCase;

        public MovimentacaoContainerController(IMovimentacaoContainerUseCase movimentacaoContainerUseCase)
        {
            _movimentacaoContainerUseCase = movimentacaoContainerUseCase;
        }
    }
}
