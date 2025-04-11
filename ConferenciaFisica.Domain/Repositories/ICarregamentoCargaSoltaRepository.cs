using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Domain.Repositories
{
    public interface ICarregamentoCargaSoltaRepository
    {
        Task<string[]?> GetVeiculosByPatio(int patio);
        Task<IEnumerable<CarregamentoOrdemModel>> GetOrdens(string placa);
        Task<IEnumerable<ItemCarregadoModel>> GetItensCarregados(string placa, string tipo = "I");
        Task<MarcantePatioModel> GetMarcanteByIdEPatio(int marcante, int? patio);
        Task<int?> GetAutonumCs(int aUTONUMCS, string placa);
        Task<QuantidadeCarregamentoModel?> GetQuantidadeCarregamento(int aUTONUMCS, string placa);
        Task UpdateMarcanteAndCargaSolta(int marcante, int? patio, string local, string placa, MarcantePatioModel marcanteByQuery, int? itensCarregados);
    }
}
