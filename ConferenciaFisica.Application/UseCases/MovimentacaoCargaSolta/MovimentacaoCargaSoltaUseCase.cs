using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.ViewModels;
using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Repositories;
using Microsoft.Win32;

namespace ConferenciaFisica.Application.UseCases.MovimentacaoCargaSolta
{
    public class MovimentacaoCargaSoltaUseCase : IMovimentacaoCargaSoltaUseCase
    {
        private readonly IMovimentacaoCargaSoltaRepository _repository;

        public MovimentacaoCargaSoltaUseCase(IMovimentacaoCargaSoltaRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<MovimentacaoCargaDTO>> BuscarCargaParaMovimentar(int idMarcante)
        {
            DescargaExportacaoViewModel descarga = null;

            var carga = await _repository.BuscarCargaParaMovimentar(idMarcante);

            if (carga == null)
            {
                return ServiceResult<MovimentacaoCargaDTO>.Failure("Registro não encontrado.");
            }

            return ServiceResult<MovimentacaoCargaDTO>.Success(carga, "Registro localizada com sucesso.");

        }

        public async Task<ServiceResult<bool>> Movimentar(MovimentacaoCargaDTO carga)
        {
            var _serviceResult = new ServiceResult<bool>();

            var ret = await _repository.Movimentar(carga);
            if (ret)
            {
                _serviceResult.Result = true;
            }
            else
            {
                _serviceResult.Result = false;
            }

            return _serviceResult;
        }
    }
}
