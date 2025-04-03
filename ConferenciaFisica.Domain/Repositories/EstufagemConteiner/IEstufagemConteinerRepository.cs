using ConferenciaFisica.Contracts.DTOs.EstufagemConteiner;

namespace ConferenciaFisica.Domain.Repositories.EstufagemConteiner
{
    public interface IEstufagemConteinerRepository
    {
        Task<PlanejamentoDTO> BuscarPlanejamento(int planejamento);
        Task<IEnumerable<ItensEstufadosDTO>> BuscarItensEstufados(int patio);
        Task<IEnumerable<EtiquetaDTO>> BuscarEtiquetas(int planejamento);
    }
}
