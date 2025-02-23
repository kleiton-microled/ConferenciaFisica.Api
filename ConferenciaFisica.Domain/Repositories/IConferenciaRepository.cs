using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Domain.Repositories
{
    public interface IConferenciaRepository
    {
        Task<Conferencia> BuscarPorConteinerAsync(string idConteiner);
        Task<Conferencia> BuscarPorLoteAsync(string idLote);
    }
}
