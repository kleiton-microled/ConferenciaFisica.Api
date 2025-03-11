using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.UseCases.DescargaExportacao.Interfaces;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Domain.Repositories.DescargaExportacaoReporitory;
using System.ComponentModel;

namespace ConferenciaFisica.Application.UseCases.DescargaExportacao
{
    public class DescargaExportacaoUseCase : IDescargaExportacaoUseCase
    {
        private readonly IDescargaExportacaoRepository _repository;
        public DescargaExportacaoUseCase(IDescargaExportacaoRepository repository)
        {
            _repository = repository;
        }
        public async Task<ServiceResult<Domain.Entities.DescargaExportacao.DescargaExportacao>> BuscarPorRegistro(int registro)
        {
            

            Domain.Entities.DescargaExportacao.DescargaExportacao descarga = null;

            var _descarga = await _repository.BuscarRegistroAsync(registro);

            if (_descarga == null)
            {
                return ServiceResult<Domain.Entities.DescargaExportacao.DescargaExportacao>.Failure("Registro não encontrado.");
            }

            return ServiceResult<Domain.Entities.DescargaExportacao.DescargaExportacao>.Success(_descarga, "Registro localizada com sucesso.");
        }
    }
}
