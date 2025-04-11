using ConferenciaFisica.Contracts.DTOs.EstufagemConteiner;

namespace ConferenciaFisica.Domain.Repositories.EstufagemConteiner
{
    public interface IEstufagemConteinerRepository
    {
        Task<PlanejamentoDTO> BuscarPlanejamento(int planejamento);
        Task<IEnumerable<ItensEstufadosDTO>> BuscarItensEstufados(int patio);
        Task<IEnumerable<EtiquetaDTO>> BuscarEtiquetas(int planejamento);
        Task<SaldoCargaMarcanteDto> BuscarSaldoCargaMarcante(int planejamento, string codigoMarcante);
        Task<bool> IniciarEstufagem(TalieInsertDTO talie);
        Task<bool> Estufar(SaldoCargaMarcanteDto request);
        Task<bool> Finalizar(TalieInsertDTO talie);
    }
}
