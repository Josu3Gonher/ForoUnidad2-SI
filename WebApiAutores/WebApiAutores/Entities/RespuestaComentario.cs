using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Entities
{
    [Table("respuestas_comentarios", Schema = "transacctional")]
    public class RespuestaComentario
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("respuesta_comentario")]
        [StringLength(500)]
        [Required]
        public string RespuestaAlComentario { get; set; }

        [Column("user_id")]
        [Required]
        public string UserId { get; set; }

        [Column("comentario_id")]
        [Required]
        public int ComentarioId { get; set; }

        [Column("fecha")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [ForeignKey(nameof(UserId))]
        public IdentityUser Usuario { get; set; }

        [ForeignKey(nameof(ComentarioId))]
        public Comentario Comentario { get; set; }
    }

}