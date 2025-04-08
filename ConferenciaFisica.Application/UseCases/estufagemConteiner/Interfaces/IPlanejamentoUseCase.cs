using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Contracts.DTOs.EstufagemConteiner;

namespace ConferenciaFisica.Application.UseCases.estufagemConteiner.Interfaces
{
    public interface IPlanejamentoUseCase
    {
        Task<ServiceResult<PlanejamentoDTO>> BuscarPlanejamento(int planejamento);
        Task<ServiceResult<SaldoCargaMarcanteDto>> BuscarSaldoCargaMarcante(int planejamento, string codigoMarcante);
        Task<ServiceResult<bool>> IniciarEstufagem(TalieInsertDTO talie); 
        Task<ServiceResult<bool>> Estufar(TalieInsertDTO request);
        Task<ServiceResult<bool>> Finalizar(TalieInsertDTO talie); 

    }
}
