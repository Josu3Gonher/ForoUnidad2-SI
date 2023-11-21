using AutoMapper;
using System.Globalization;
using WebApiAutores.Dtos.Autores;
using WebApiAutores.Dtos.Books;
using WebApiAutores.Dtos.Comentarios;
using WebApiAutores.Dtos.Reviews;
using WebApiAutores.Entities;

namespace WebApiAutores.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            MapsForBooks();
            MapsForAutores();
            MapsForReviews();
            MapsForComentarios();
            MapsForRespuestaComentarios();
        }

        private void MapsForRespuestaComentarios()
        {
            CreateMap<RespuestaComentario, RespuestaComentarioDto>();
            CreateMap<RespuestaComentarioCreateDto, RespuestaComentario>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserIdRespuesta))
            .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => src.FechaRespuesta));
        }

        private void MapsForComentarios()
        {
            CreateMap<Comentario, ComentarioDto>();
            CreateMap<ComentarioCreateDto, Comentario>();
        }

        private void MapsForReviews()
        {
            CreateMap<Review, ReviewDto>();
            CreateMap<ReviewCreateDto, Review>();
        }
        private void MapsForAutores()
        {
            CreateMap<Autor, AutorDto>();
            CreateMap<Autor, AutorGetByIdDto>();
            CreateMap<AutorCreateDto, Autor>();
        }

        private void MapsForBooks()
        {
            //CreateMap<BookDto, Book>().ReverseMap();
            CreateMap<Book, BookDto>()
                .ForPath(dest => dest.AutorNombre, opt => opt.MapFrom(src => src.Autor.Name));

            CreateMap<BookCreateDto, Book>();
        }
    }
}
