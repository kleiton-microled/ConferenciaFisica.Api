using ConferenciaFisica.Application.UseCases.MovimentacaoContainer.Interfaces;
using ConferenciaFisica.Domain.Repositories;

namespace ConferenciaFisica.Application.UseCases.MovimentacaoContainer
{
    public class MovimentacaoContainerUseCase : IMovimentacaoContainerUseCase
    {
        private readonly IMovimentacaoContainerRepository _movimentacaoContainerRepository;

        public MovimentacaoContainerUseCase(IMovimentacaoContainerRepository movimentacaoContainerRepository)
        {
            _movimentacaoContainerRepository = movimentacaoContainerRepository;
        }
    }
}
