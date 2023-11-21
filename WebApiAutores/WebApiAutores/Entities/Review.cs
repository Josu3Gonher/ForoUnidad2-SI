using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebApiAutores.Entities
{
    [Table("reviews", Schema = "transacctional")]
    public class Review
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("descripcion")]
        [StringLength(500)]
        [Required]
        public string DescripcionReview { get; set; }

        [Column("calificacion")]
        [Required]
        public int Calificacion { get; set; }

        [Column("user_id")]
        [Required]
        public string UserId { get; set; }

        [Column("book_id")]
        [Required]
        public Guid BookId { get; set; }

        [Column("fecha")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [ForeignKey(nameof(UserId))]
        public IdentityUser Usuario { get; set; }

        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; }
        public ICollection<Comentario> Comentarios { get; set; }// Relacion de uno a muchos
    }
}
