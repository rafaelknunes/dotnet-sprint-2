using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hal.DTOs
{
    public class CommentDTO
    {
        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]

        public User? User { get; set; }

        public int? EvaluationId { get; set; }
        [ForeignKey("EvaluationId")]

        public Evaluation? Evaluation { get; set; }

        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]

        public Category? Category { get; set; }

        [Required]
        [Column("CommentText")]
        public string CommentText { get; set; }

        [Required]
        [Column("CommentDate")]
        public DateTime CommentDate { get; set; }
    }
}
