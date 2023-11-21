using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Dtos;
using WebApiAutores.Dtos.Reviews;
using WebApiAutores.Entities;

namespace WebApiAutores.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ReviewsController(ApplicationDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto<IReadOnlyList<ReviewDto>>>> GetReviews()
        {
            try
            {
                var reviewsDb = await _context.Reviews.Include(r => r.Comentarios).ToListAsync();
                var reviewsDto = _mapper.Map<List<ReviewDto>>(reviewsDb);

                return new ResponseDto<IReadOnlyList<ReviewDto>>
                {
                    Status = true,
                    Data = reviewsDto
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<IReadOnlyList<ReviewDto>>
                {
                    Status = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<ReviewDto>>> CreateReview(ReviewCreateDto dto)
        {
            if (ContieneMalasPalabras(dto.DescripcionReview))
            {
                return BadRequest(new ResponseDto<ReviewDto>
                {
                    Status = false,
                    Message = "Contiene palabras inapropiadas, procura no utilizarlas"
                });
            }

            var review = _mapper.Map<Review>(dto);

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            var reviewDto = _mapper.Map<ReviewDto>(review);

            return StatusCode(StatusCodes.Status201Created, new ResponseDto<ReviewDto>
            {
                Status = true,
                Message = "Review Creada con exito",
                Data = reviewDto
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ResponseDto<ReviewDto>>> UpdateReview(int id, ReviewUpdateDto dto)
        {
            if (ContieneMalasPalabras(dto.DescripcionReview))
            {
                return BadRequest(new ResponseDto<ReviewDto>
                {
                    Status = false,
                    Message = "Contiene palabras inapropiadas, procura no utilizarlas"
                });
            }

            var reviewDb = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == id);

            if (reviewDb is null)
            {
                return NotFound(new ResponseDto<ReviewDto>
                {
                    Status = false,
                    Message = $"No existe la review: {id}"
                });
            }


            _mapper.Map<ReviewUpdateDto, Review>(dto, reviewDb);

            _context.Update(reviewDb);
            await _context.SaveChangesAsync();

            var reviewDto = _mapper.Map<ReviewDto>(reviewDb);

            return Ok(new ResponseDto<ReviewDto>
            {
                Status = true,
                Message = "Review editado",
                Data = reviewDto,
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




