using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Application.UseCases.Imagens.Interfaces
{
    public interface ICarregarImagensByRegistroUseCase
    {
        Task<ServiceResult<IEnumerable<Imagem>>> Handle(int registroId);
    }
}
