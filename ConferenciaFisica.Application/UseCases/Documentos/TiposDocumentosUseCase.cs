using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.UseCases.Documentos.Interfaces;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenciaFisica.Application.UseCases.Documentos
{
    public class TiposDocumentosUseCase : ITiposDocumentosUseCase
    {
        private readonly ITiposDocumentosRepository _repository;

        public TiposDocumentosUseCase(ITiposDocumentosRepository repository)
        {
            _repository = repository;
        }
        public async Task<ServiceResult<IEnumerable<TipoDocumentos>>> GetAllAsync()
        {
            var documents = await _repository.GetAll();

            return ServiceResult<IEnumerable<TipoDocumentos>>.Success(documents, "Documentos carregados com sucesso.");
        }
    }
}
