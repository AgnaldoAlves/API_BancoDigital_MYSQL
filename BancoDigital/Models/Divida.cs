using System.ComponentModel.DataAnnotations;

namespace BancoDigital.Models
{
    public class Divida
    {
        public int Id { get; set; }

        [Required]
        public int ContaId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "O valor da dívida deve ser um valor positivo.")]
        public decimal Valor { get; set; }
    }
}
