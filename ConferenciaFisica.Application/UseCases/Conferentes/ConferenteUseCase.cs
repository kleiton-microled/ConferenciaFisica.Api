using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Contracts.DTOs.EstufagemConteiner;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Domain.Repositories.EstufagemConteiner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenciaFisica.Application.UseCases.Conferentes
{
    public class ConferenteUseCase : IConferentesUseCase
    {
        private readonly IColetorRepository _repository;

        public ConferenteUseCase(IColetorRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<IEnumerable<ConferenteDTO>>> ListarConferentes()
        {
            var data = await _repository.ListarConferentes();
            if (data == null)
            {
                return ServiceResult<IEnumerable<ConferenteDTO>>.Success(data, "planejamento não encontrada.");
            }

            return ServiceResult<IEnumerable<ConferenteDTO>>.Success(data, "Planejamento localizado com sucesso.");
        }
    }
}
