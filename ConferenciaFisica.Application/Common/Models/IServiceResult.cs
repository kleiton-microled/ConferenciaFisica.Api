namespace ConferenciaFisica.Application.Common.Models
{
    public interface IServiceResult<T>
    {
        bool Status { get; set; }
        IList<string> Mensagens { get; set; }
    }
}
