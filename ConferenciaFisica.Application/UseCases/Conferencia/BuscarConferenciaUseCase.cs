﻿using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Domain.Repositories;
using System.Threading.Tasks;

namespace ConferenciaFisica.Application.UseCases.Conferencia
{
    public class BuscarConferenciaUseCase : IBuscarConferenciaUseCase
    {
        private readonly IConferenciaRepository _conferenciaRepository;

        public BuscarConferenciaUseCase(IConferenciaRepository conferenciaRepository)
        {
            _conferenciaRepository = conferenciaRepository;
        }

        public async Task<ServiceResult<Domain.Entities.Conferencia>> ExecuteAsync(string idConteiner, string idLote)
        {
            if (string.IsNullOrEmpty(idConteiner) && string.IsNullOrEmpty(idLote))
            {
                return ServiceResult<Domain.Entities.Conferencia>.Failure("Informe um Contêiner ou um Lote!");
            }

            Domain.Entities.Conferencia conferencia = null;

            if (!string.IsNullOrEmpty(idConteiner))
            {
                conferencia = await _conferenciaRepository.BuscarPorConteinerAsync(idConteiner);
            }
            else if (!string.IsNullOrEmpty(idLote))
            {
                conferencia = await _conferenciaRepository.BuscarPorLoteAsync(idLote);
            }

            if (conferencia == null)
            {
                return ServiceResult<Domain.Entities.Conferencia>.Failure("Conferência não encontrada.");
            }

            return ServiceResult<Domain.Entities.Conferencia>.Success(conferencia, "Conferência localizada com sucesso.");
        }
    }
}
