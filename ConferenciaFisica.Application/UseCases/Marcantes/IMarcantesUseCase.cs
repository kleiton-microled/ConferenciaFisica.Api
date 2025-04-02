using ConferenciaFisica.Application.ViewModels;

namespace ConferenciaFisica.Application.UseCases.Marcantes
{
    public interface IMarcantesUseCase
    {
        Task<IEnumerable<MarcantesViewModel>> BuscarMarcantes(string search);
    }
}
