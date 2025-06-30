using System.Security.Claims;

namespace ConferenciaFisica.Application.Interfaces
{
    public interface IPatioAccessService
    {
        Task<List<string>> GetPatiosPermitidos(ClaimsPrincipal user);
    }
}
