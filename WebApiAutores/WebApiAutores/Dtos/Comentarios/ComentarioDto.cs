namespace WebApiAutores.Dtos.Comentarios
{
    public class ComentarioDto
    {
        public int Id { get; set; }
        public string DescripcionComentario { get; set; }
        public DateTime Fecha { get; set; }
        public string UserId { get; set; }
        public int ReviewId { get; set; }
    }
}
