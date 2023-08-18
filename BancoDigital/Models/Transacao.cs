using System.ComponentModel.DataAnnotations;

namespace BancoDigital.Models
{

    public class Transacao
    {
        public int Id { get; set; }

        [Required]
        public int ContaId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "O valor do saldo deve ser um valor positivo.")]
        public decimal Valor { get; set; }

        public DateTime Data { get; set; }

    }
}
