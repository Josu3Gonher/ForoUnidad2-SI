using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Dtos.Reviews
{
    public class ReviewCreateDto
    {
        [Display(Name = "Review")]
        [Required(ErrorMessage = "El {0} es requerido")]
        [StringLength(500, ErrorMessage = "El {0} requiere {1} caracteres")]
        public string DescripcionReview { get; set; }

        [Display(Name = "Calificacion del Libro")]
        [Required(ErrorMessage = "La {0} es requerida")]
        [Range(1, 5, ErrorMessage = "La {0} debe estar en el rango de 1 a 5")]
        public int Calificacion { get; set; }

        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El {0} es requerido")]
        public string UserId { get; set; }

        [Display(Name = "Libro")]
        [Required(ErrorMessage = "El {0} es requerido")]
        public Guid BookId { get; set; }
    }
}
