using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.UseCases.Conferencia.Interfaces;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Repositories;
using System.ComponentModel;

namespace ConferenciaFisica.Application.UseCases.Conferencia
{
    public class BuscarConferenciaUseCase : IBuscarConferenciaUseCase
    {
        private readonly IConferenciaRepository _conferenciaRepository;

        public BuscarConferenciaUseCase(IConferenciaRepository conferenciaRepository)
        {
            _conferenciaRepository = conferenciaRepository;
        }

        public async Task<ServiceResult<Domain.Entities.Conferencia>> BuscarPorId(int id)
        {
            var data =  await _conferenciaRepository.BuscarPorPorId(id);
            if (data == null)
            {
                return ServiceResult<Domain.Entities.Conferencia>.Success(data, "Conferência não encontrada.");
            }

            return ServiceResult<Domain.Entities.Conferencia>.Success(data, "Conferência localizada com sucesso.");

        }

        public async Task<ServiceResult<Domain.Entities.Conferencia>> ExecuteAsync(string idConteiner, string idLote)
        {
            if (string.IsNullOrEmpty(idConteiner) && string.IsNullOrEmpty(idLote))
            {
                return ServiceResult<Domain.Entities.Conferencia>.Failure("Informe um Contêiner ou um Lote!");
            }

            Domain.Entities.Conferencia conferencia = null;

            if (!string.IsNullOrEmpty(idConteiner) && idConteiner != "0")
            {
                conferencia = await _conferenciaRepository.BuscarPorConteinerAsync(idConteiner);
               
            }
            else if (!string.IsNullOrEmpty(idLote))
            {
                conferencia = await _conferenciaRepository.BuscarPorLoteAsync(idLote);
                //if (conferencia is null)
                //{
                //    conferencia = await _conferenciaRepository.BuscarPorReservaAsync(idLote);
                //}
            }

            if (conferencia == null)
            {
                return ServiceResult<Domain.Entities.Conferencia>.Success(conferencia,"Conferência não encontrada.");
            }

            return ServiceResult<Domain.Entities.Conferencia>.Success(conferencia, "Conferência localizada com sucesso.");
        }
    }
}
