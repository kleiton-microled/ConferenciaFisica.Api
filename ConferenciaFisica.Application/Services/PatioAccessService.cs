using ConferenciaFisica.Application.Interfaces;
using System.Security.Claims;

namespace ConferenciaFisica.Application.Services
{
    public class PatioAccessService : IPatioAccessService
    {
        public async Task<List<string>> GetPatiosPermitidos(ClaimsPrincipal user)
        {
            var permissionToPatioIdMap = new Dictionary<string, string>
            {
                { "PATIO_CLIA", "3" },
                { "PATIO_IPA", "7" } // Adicione outros conforme necessário
            };

            var permissions = user?.Claims
                .Where(c => c.Type == "permission")
                .Select(c => c.Value)
                .ToList();

            if (permissions == null || !permissions.Any())
                return new List<string>();

            // 🔓 Se for Admin, retorna todos os pátios
            if (permissions.Contains("Admin"))
                return permissionToPatioIdMap.Values.Distinct().ToList();

            var patios = new List<string>();

            foreach (var permission in permissions)
            {
                if (permissionToPatioIdMap.TryGetValue(permission, out string patioId))
                {
                    patios.Add(patioId);
                }
            }

            return patios.Distinct().ToList(); // Elimina duplicados, se houver
        }


    }

}
