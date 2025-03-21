using ConferenciaFisica.Application.Common.Models;

namespace ConferenciaFisica.Application.UseCases.Utils.Interfaces
{
    public interface IUtilUseCase<TResult, TInput>
    {
        Task<ServiceResult<IEnumerable<TResult>>> GetAll();

        Task<ServiceResult<bool>> Delete(int id);
        Task<ServiceResult<TResult>> Get(int id);

        Task<ServiceResult<TResult>> Create(TInput input);
        Task<ServiceResult<TResult>> Update(int id, TInput input);

    }
}
