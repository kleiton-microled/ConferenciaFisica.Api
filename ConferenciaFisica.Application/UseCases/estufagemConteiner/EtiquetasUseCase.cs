using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.UseCases.estufagemConteiner.Interfaces;
using ConferenciaFisica.Contracts.DTOs.EstufagemConteiner;
using ConferenciaFisica.Domain.Repositories.EstufagemConteiner;
using System.IO;

namespace ConferenciaFisica.Application.UseCases.estufagemConteiner
{
    public class EtiquetasUseCase : IEtiquetasUseCase
    {
        private readonly IEstufagemConteinerRepository _estufagemConteinerRepository;

        public EtiquetasUseCase(IEstufagemConteinerRepository estufagemConteinerRepository)
        {
            _estufagemConteinerRepository = estufagemConteinerRepository;
        }

        public async Task<ServiceResult<IEnumerable<EtiquetaDTO>>> BuscarEtiquetas(int planejamento)
        {
            var data = await _estufagemConteinerRepository.BuscarEtiquetas(planejamento);
            if (data == null)
            {
                return ServiceResult<IEnumerable<EtiquetaDTO>>.Success(data, "Nenhum registro encontrado.");
            }

            return ServiceResult<IEnumerable<EtiquetaDTO>>.Success(data, "Registros localizados com sucesso.");
        }
    }
}
