using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Contracts.DTOs.EstufagemConteiner;

namespace ConferenciaFisica.Application.UseCases.estufagemConteiner.Interfaces
{
    public interface IPlanejamentoUseCase
    {
        Task<ServiceResult<PlanejamentoDTO>> BuscarPlanejamento(int planejamento);
        Task<ServiceResult<SaldoCargaMarcanteDto>> BuscarSaldoCargaMarcante(int planejamento, string codigoMarcante);
        Task<ServiceResult<bool>> IniciarEstufagem(TalieInsertDTO talie); //Recebe uma Talie para iniciar o processo

    }
}
