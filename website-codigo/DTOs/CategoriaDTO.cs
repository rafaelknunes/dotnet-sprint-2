using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hal.DTOs
{
    public class CategoriaDTO
    {
        [Required]
        public string CategoryName { get; set; }

        [Required]
        public string SubCategoryName { get; set; }
    }
}
