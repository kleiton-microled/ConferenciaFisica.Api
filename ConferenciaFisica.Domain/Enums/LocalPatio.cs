using System.ComponentModel.DataAnnotations;

namespace ConferenciaFisica.Domain.Enums
{
    public enum LocalPatio
    {
        [Display(Name = "Pátio")]
        Patio = 1,

        [Display(Name = "Estacionamento")]
        Estacionamento
    }
}
