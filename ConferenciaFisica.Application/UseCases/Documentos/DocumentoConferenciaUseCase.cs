using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.Inputs;
using ConferenciaFisica.Application.UseCases.Documentos.Interfaces;
using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Repositories;

namespace ConferenciaFisica.Application.UseCases.Documentos
{
    public class DocumentoConferenciaUseCase : IDocumentoConferenciaUseCase
    {
        private readonly IConferenciaRepository _conferenciaRepository;

        public DocumentoConferenciaUseCase(IConferenciaRepository conferenciaRepository)
        {
            _conferenciaRepository = conferenciaRepository;
        }
        public async Task<ServiceResult<IEnumerable<DocumentosConferencia>>> GetAllAsync(int idConferencia)
        {
            var data = await _conferenciaRepository.CarregarDocumentosConferencia(idConferencia);

            return ServiceResult<IEnumerable<DocumentosConferencia>>.Success(data, "Documentos carregados com sucesso.");
        }

        public async Task<ServiceResult<bool>> ExecuteAsync(DocumentoConferenciaInput input)
        {
            var _serviceResult = new ServiceResult<bool>();

            var command = new DocumentoConferenciaCommand()
            {
                IdConferencia = input.IdConferencia,
                Numero = input.Numero,
                Tipo = input.Tipo
            };

            var ret = await _conferenciaRepository.CadastroDocumentosConferencia(command);

            if (ret)
            {
                _serviceResult.Result = true;
                _serviceResult.Mensagens.Add("Documento cadastrado com sucesso!");
            }
            else
            {
                _serviceResult.Result = false;
            }

            return _serviceResult;
        }

        public async Task<ServiceResult<bool>> UpdateAsync(DocumentoConferenciaInput input)
        {
            var _serviceResult = new ServiceResult<bool>();

            var command = new DocumentoConferenciaCommand()
            {
                Id = input.Id,
                IdConferencia = input.IdConferencia,
                Numero = input.Numero,
                Tipo = input.Tipo
            };

            var ret = await _conferenciaRepository.AtualizarDocumentosConferencia(command);

            if (ret)
            {
                _serviceResult.Result = true;
                _serviceResult.Mensagens.Add("Documento atualizado com sucesso!");
            }
            else
            {
                _serviceResult.Result = false;
            }

            return _serviceResult;
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            var _serviceResult = new ServiceResult<bool>();

            var result = await _conferenciaRepository.ExcluirDocumentosConferencia(id);
            if (result)
            {
                _serviceResult.Result = result;
                _serviceResult.Mensagens.Add("Documento excluido com sucesso!");
            }
            else
            {
                _serviceResult.Mensagens.Add("Falha ao tentar excluir o registro!");
            }

            return _serviceResult;
        }
    }
}
