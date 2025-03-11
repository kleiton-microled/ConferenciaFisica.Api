using ConferenciaFisica.Application.Common.Models;

namespace ConferenciaFisica.Application.UseCases.DescargaExportacao.Interfaces
{
    public interface IDescargaExportacaoUseCase
    {
        Task<ServiceResult<Domain.Entities.DescargaExportacao.DescargaExportacao>> BuscarPorRegistro(int registro);
    }
}
