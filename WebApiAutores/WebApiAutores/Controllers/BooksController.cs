using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Dtos;
using WebApiAutores.Dtos.Books;
using WebApiAutores.Entities;
using WebApiAutores.Services;

namespace WebApiAutores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BooksController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<IReadOnlyList<BookDto>>>> Get()
        {
            var booksDb = await _context.Books.Include(b => b.Autor).ToListAsync();

            var booksDto = _mapper.Map<List<BookDto>>(booksDb);

            return new ResponseDto<IReadOnlyList<BookDto>>
            {
                Status = true,
                Data = booksDto
            };
        }

        [HttpGet("{id:Guid}")] // api/books/76237-2gdcv-hdj
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<BookDto>>> Get(Guid id)
        {
            var bookDb = await _context.Books
                .Include(b => b.Autor)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (bookDb is null)
            {
                return NotFound(new ResponseDto<BookDto>
                {
                    Status = false,
                    Message = $"El libro con el Id {id} no fue encontrado"
                });
            }

            var bookDto = _mapper.Map<BookDto>(bookDb);

            return Ok(new ResponseDto<BookDto>
            {
                Status = true,
                Data = bookDto
            });
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<BookDto>>> Post(BookCreateDto dto)
        {
            var autorExiste = await _context.Autores.AnyAsync(x => x.Id == dto.AutorId);

            if (!autorExiste)
            {
                return NotFound(new ResponseDto<BookDto>
                {
                    Status = false,
                    Message = $"No existe el autor: {dto.AutorId}",
                });
            }

            var book = _mapper.Map<Book>(dto);

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var bookDto = _mapper.Map<BookDto>(book);

            return StatusCode(StatusCodes.Status201Created, new ResponseDto<BookDto>
            {
                Status = true,
                Message = "Libro creado correctamente",
                Data = bookDto,
            });
        }

       

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseDto<BookDto>>> Put(BookUpdateDto dto, Guid id)
        {
            var bookDb = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (bookDb is null)
            {
                return NotFound(new ResponseDto<BookDto>
                {
                    Status = false,
                    Message = $"No existe el libro: {id}",
                });
            }

            var autorExiste = await _context.Autores.AnyAsync(x => x.Id == dto.AutorId);

            if (!autorExiste)
            {
                return NotFound(new ResponseDto<BookDto>
                {
                    Status = false,
                    Message = $"No existe el autor: {dto.AutorId}",
                });
            }

            _mapper.Map<BookUpdateDto, Book>(dto, bookDb);

            _context.Update(bookDb);
            await _context.SaveChangesAsync();

            var bookDto = _mapper.Map<BookDto>(bookDb);

            return Ok(new ResponseDto<BookDto>
            {
                Status = true,
                Message = "Libro editado correctamente",
                Data = bookDto
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseDto<BookDto>>> Delete(Guid id)
        {
            var bookExist = await _context.Books.AnyAsync(x => x.Id == id);

            if (!bookExist)
            {
                return NotFound(new ResponseDto<BookDto>
                {
                    Status = false,
                    Message = $"No existe el libro: {id}",
                });
            }

            _context.Remove(new Book { Id = id });
            await _context.SaveChangesAsync();

            return Ok(new ResponseDto<BookDto>
            {
                Status = true,
                Message = $"Libro {id} borrado correctamente",
            });
        }
    }
}
