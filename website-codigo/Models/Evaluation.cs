using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hal.Models
{
    [Table("TB_EVALUATION")]
    public class Evaluation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Assegure que o Id é gerado automaticamente
        public int Id { get; set; }

        [Required]
        [Column("Sentiment")]
        public string Sentiment { get; set; }
    }
}
