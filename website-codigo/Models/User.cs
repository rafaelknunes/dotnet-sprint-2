using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hal.Models
{
    [Table("TB_USER")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Assegure que o Id é gerado automaticamente
        public int Id { get; set; }

        [Required]
        [Column("UserName")]
        public string UserName { get; set; }

        [Required]
        [Column("PasswordHash")]
        public string PasswordHash { get; set; }

        [Required]
        [EmailAddress]
        [Column("UserEmail")]
        public string UserEmail { get; set; }

        [Required]
        [Column("DateRegister")]
        public DateTime DateRegister { get; set; }

        [Required]
        [Column("IsActive")]
        public bool IsActive { get; set; } = true;

        public ICollection<Comment>? Comments { get; set; }

    }
}
