using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiAutores.Entities
{
    [Table("comentarios", Schema = "transacctional")]
    public class Comentario
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("descripcion")]
        [StringLength(500)]
        [Required]
        public string DescripcionComentario { get; set; }

        [Column("user_id")]
        [Required]
        public string UserId { get; set; }

        [Column("review_id")]
        [Required]
        public int ReviewId { get; set; }

        [Column("fecha")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [ForeignKey(nameof(UserId))]
        public IdentityUser Usuario { get; set; }

        [ForeignKey(nameof(ReviewId))]
        public Review Review { get; set; }

        [Column("respuestas")]
        public List<RespuestaComentario> Respuestas { get; set; }
    }
}


