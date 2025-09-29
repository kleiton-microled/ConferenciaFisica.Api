using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.UseCases.Conferencia.Interfaces;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Application.Interfaces;
using System.ComponentModel;

namespace ConferenciaFisica.Application.UseCases.Conferencia
{
    public class BuscarConferenciaUseCase : IBuscarConferenciaUseCase
    {
        private readonly IConferenciaRepository _conferenciaRepository;
        private readonly ISchemaService _schemaService;

        public BuscarConferenciaUseCase(IConferenciaRepository conferenciaRepository, ISchemaService schemaService)
        {
            _conferenciaRepository = conferenciaRepository;
            _schemaService = schemaService;
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

        public async Task<ServiceResult<string>> BusrcarCpfConferente(string usuario)
        {
            var data = await _conferenciaRepository.BuscarCpfConferente(usuario);
            if (string.IsNullOrEmpty(data))
            {
                return ServiceResult<string>.Success(data, "Conferente não encontrada.");
            }

            return ServiceResult<string>.Success(data, "Conferente localizado com sucesso.");
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
                // Verifica se o schema é REDEX para chamar o método apropriado
                var currentSchema = _schemaService.GetCurrentSchema();
                if (currentSchema == "REDEX")
                {
                    conferencia = await _conferenciaRepository.BuscarPorConteinerRedexAsync(idConteiner);
                }
                else
                {
                    conferencia = await _conferenciaRepository.BuscarPorConteinerAsync(idConteiner);
                }
               
            }
            else if (!string.IsNullOrEmpty(idLote))
            {
                conferencia = await _conferenciaRepository.BuscarPorLoteAsync(idLote);
            }

            if (conferencia == null)
            {
                return ServiceResult<Domain.Entities.Conferencia>.Success(conferencia,"Conferência não encontrada.");
            }

            return ServiceResult<Domain.Entities.Conferencia>.Success(conferencia, "Conferência localizada com sucesso.");
        }
    }
}
