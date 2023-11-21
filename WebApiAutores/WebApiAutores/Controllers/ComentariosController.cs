using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Dtos;
using WebApiAutores.Dtos.Comentarios;
using WebApiAutores.Dtos.Reviews;
using WebApiAutores.Entities;

namespace WebApiAutores.Controllers
{
    [Route("api/comentarios")]
    [ApiController]
    public class ComentariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ComentariosController(ApplicationDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto<IReadOnlyList<ComentarioDto>>>> GetComentarios()
        {
            try
            {
                var comentariosDb = await _context.Comentarios.Include(c => c.Usuario).Include(c => c.Review).ToListAsync();
                var comentariosDto = _mapper.Map<List<ComentarioDto>>(comentariosDb);

                return new ResponseDto<IReadOnlyList<ComentarioDto>>
                {
                    Status = true,
                    Data = comentariosDto
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<IReadOnlyList<ComentarioDto>>
                {
                    Status = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }


        [HttpPost]
        public async Task<ActionResult<ResponseDto<ReviewDto>>> CreateComentario(ComentarioCreateDto dto)
        {
            if (ContieneMalasPalabras(dto.DescripcionComentario))
            {
                return BadRequest(new ResponseDto<ReviewDto>
                {
                    Status = false,
                    Message = "Contiene palabras inapropiadas, procura no utilizarlas"
                });
            }

            var comentario = _mapper.Map<Comentario>(dto);

            _context.Comentarios.Add(comentario);
            await _context.SaveChangesAsync();

            var comentarioDto = _mapper.Map<ComentarioDto>(comentario);

            return StatusCode(StatusCodes.Status201Created, new ResponseDto<ComentarioDto>
            {
                Status = true,
                Message = "Comentario creado con exito",
                Data = comentarioDto
            });
        }

        [HttpPost("{id:int}/respuestas")] // api/comentarios/5/respuestas
        public async Task<ActionResult<ResponseDto<RespuestaComentarioDto>>> CreateRespuestaComentario(int id, RespuestaComentarioCreateDto dto)
        {
            try
            {
                if (ContieneMalasPalabras(dto.RespuestaAlComentario))
                {
                    return BadRequest(new ResponseDto<ReviewDto>
                    {
                        Status = false,
                        Message = "Contiene palabras inapropiadas, procura no utilizarlas"
                    });
                }

                var comentario = await _context.Comentarios.FirstOrDefaultAsync(c => c.Id == id);

                if (comentario is null)
                {
                    return NotFound(new ResponseDto<RespuestaComentarioDto>
                    {
                        Status = false,
                        Message = $"No existe el comentario con ID: {id}"
                    });
                }

                var respuestaComentario = _mapper.Map<RespuestaComentario>(dto);
                respuestaComentario.ComentarioId = id;

                _context.RespuestaComentarios.Add(respuestaComentario);
                await _context.SaveChangesAsync();

                var respuestaComentarioDto = _mapper.Map<RespuestaComentarioDto>(respuestaComentario);

                return StatusCode(StatusCodes.Status201Created, new ResponseDto<RespuestaComentarioDto>
                {
                    Status = true,
                    Message = "Respuesta al comentario creada con éxito",
                    Data = respuestaComentarioDto
                });
            }
            catch (Exception ex)
            {
                // Registra la excepción interna
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                return new ResponseDto<RespuestaComentarioDto>
                {
                    Status = false,
                    Message = "Error al guardar los cambios. Consulta la excepción interna para obtener más detalles.",
                    Data = null
                };
            }
        }


        [HttpPut("{id:int}")] //api/comentarios/5
        public async Task<ActionResult<ResponseDto<ComentarioDto>>> UpdateComentario(int id, ComentarioUpdateDto dto)
        {
            if (ContieneMalasPalabras(dto.DescripcionComentario))
            {
                return BadRequest(new ResponseDto<ReviewDto>
                {
                    Status = false,
                    Message = "Contiene palabras inapropiadas, procura no utilizarlas"
                });
            }

            var comentarioDb = await _context.Comentarios.FirstOrDefaultAsync(r => r.Id == id);

            if (comentarioDb is null)
            {
                return NotFound(new ResponseDto<ComentarioDto>
                {
                    Status = false,
                    Message = $"No existe el comentario: {id}"
                });
            }


            _mapper.Map<ComentarioUpdateDto, Comentario>(dto, comentarioDb);

            _context.Update(comentarioDb);
            await _context.SaveChangesAsync();

            var comentarioDto = _mapper.Map<ComentarioDto>(comentarioDb);

            return Ok(new ResponseDto<ComentarioDto>
            {
                Status = true,
                Message = "Comentario editado",
                Data = comentarioDto
            });
        }

        //Verificacion de malas palabras

        private bool ContieneMalasPalabras(string texto)
        {
            foreach (var malaPalabra in MalasPalabras)
            {
                if (texto.Contains(malaPalabra, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private static readonly List<string> MalasPalabras = new List<string>
        {
            "puta",
            "pendejo",
            "cerote",
            "cabeza de pija",
            "malparido",
            "te pisan",
            "culero",
            "estupido",
            "no servis",
        };

    }
    

}
