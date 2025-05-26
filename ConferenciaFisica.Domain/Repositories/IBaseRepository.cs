namespace ConferenciaFisica.Domain.Repositories
{
    public interface IBaseRepository<TModel>
    {
        Task<IEnumerable<TModel>?> GetAll();

        Task<bool> Delete(int id);
        Task<TModel?> Get(int id);

        Task<TModel?> Update(TModel input);
        Task<TModel?> Create(TModel input);
    }
}
