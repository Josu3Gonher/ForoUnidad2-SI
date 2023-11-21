using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Dtos.Comentarios
{
    public class RespuestaComentarioCreateDto
    {
        [Display(Name = "Respuesta al Comentario")]
        [Required(ErrorMessage = "La {0} es requerida")]
        [StringLength(500, ErrorMessage = "El {0} requiere {1} caracteres")]
        public string RespuestaAlComentario { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaRespuesta { get; set; } = DateTime.Now;

        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El {0} es requerido")]
        public string UserIdRespuesta { get; set; }
    }
}

