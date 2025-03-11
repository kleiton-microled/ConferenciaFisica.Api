using ConferenciaFisica.Domain.Entities.DescargaExportacao;

namespace ConferenciaFisica.Domain.Repositories.DescargaExportacaoReporitory
{
    public interface IDescargaExportacaoRepository
    {
        Task<DescargaExportacao> BuscarRegistroAsync(int registro);
    }
}
