
using ConferenciaFisica.Domain.Enums;

namespace ConferenciaFisica.Domain.Repositories
{
    public interface IPreRegistroRepository
    {
        Task<bool> AtualizarDataChegada(int id);
        Task<bool> Cadastrar(string? protocolo, string? placaCarreta1, string? placaCarreta2, string? ticket, LocalPatio localPatio, DateTime now1, DateTime? now2, bool v, string? finalidadeId, int? patioDestinoId);
    }
}
