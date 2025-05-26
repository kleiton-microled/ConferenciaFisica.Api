using System.Security.Claims;

namespace ConferenciaFisica.Application.Interfaces
{
    public interface IPatioAccessService
    {
        Task<List<int>> GetPatiosPermitidos(ClaimsPrincipal user);
    }
}
