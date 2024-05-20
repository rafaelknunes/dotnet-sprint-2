using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hal.Models
{
    [Table("TB_CATEGORY")]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Assegure que o Id é gerado automaticamente
        public int Id { get; set; }

        [Required]
        [Column("CategoryName")]
        public string CategoryName { get; set; }

        [Required]
        [Column("SubCategoryName")]
        public string SubCategoryName { get; set; }
    }
}
