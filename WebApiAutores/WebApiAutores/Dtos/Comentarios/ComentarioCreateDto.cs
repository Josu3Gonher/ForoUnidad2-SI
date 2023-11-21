using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Dtos.Comentarios
{
    public class ComentarioCreateDto
    {
        [Required(ErrorMessage = "El comentario es requerido")]
        [StringLength(500, ErrorMessage = "El {0} requiere {1} caracteres")]
        public string DescripcionComentario { get; set; }

        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El {0} es requerido")]
        public string UserId { get; set; }

        [Display(Name = "Review")]
        [Required(ErrorMessage = "El id de la {0} es requerido")]
        public int ReviewId { get; set; }
    }
}
