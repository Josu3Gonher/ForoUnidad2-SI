namespace WebApiAutores.Dtos.Comentarios
{
    public class RespuestaComentarioDto
    {
        public int Id { get; set; }
        public string RespuestaAlComentario { get; set; }
        public DateTime Fecha { get; set; }
        public string UserId { get; set; }
        public int ComentarioId { get; set; }
    }
}
