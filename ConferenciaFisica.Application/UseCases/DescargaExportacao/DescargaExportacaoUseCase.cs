using AutoMapper;
using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.UseCases.DescargaExportacao.Interfaces;
using ConferenciaFisica.Application.ViewModels;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Domain.Repositories.DescargaExportacaoReporitory;
using System.ComponentModel;

namespace ConferenciaFisica.Application.UseCases.DescargaExportacao
{
    public class DescargaExportacaoUseCase : IDescargaExportacaoUseCase
    {
        private readonly IDescargaExportacaoRepository _repository;
        private readonly IMapper _mapper;
        public DescargaExportacaoUseCase(IDescargaExportacaoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ServiceResult<DescargaExportacaoViewModel>> BuscarPorRegistro(int registro)
        {


            DescargaExportacaoViewModel descarga = null;

            var _descarga = await _repository.BuscarRegistroAsync(registro);

            if (_descarga == null)
            {
                return ServiceResult<DescargaExportacaoViewModel>.Failure("Registro não encontrado.");
            }

            return ServiceResult<DescargaExportacaoViewModel>.Success(_mapper.Map<DescargaExportacaoViewModel>(_descarga), "Registro localizada com sucesso.");
        }
    }
}
