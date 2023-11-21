namespace WebApiAutores.Dtos.Reviews
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string DescripcionReview { get; set; }
        public int Calificacion { get; set; }
        public DateTime Fecha { get; set; }

        public string UserId { get; set; }
        public Guid BookId { get; set; }
    }
}

