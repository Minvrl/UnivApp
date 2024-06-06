using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Univ.Data;
using Univ.Service.Dtos;
using Univ.Service.Services.Implementations;
using Univ.Service.Services.Interfaces;

namespace Univ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost("")]
        public ActionResult Create([FromForm] StudentCreateDto createDto)
        {
            return StatusCode(201, new { Id = _studentService.Create(createDto) });
        }

        [HttpGet("")]
        public ActionResult<List<GroupGetDto>> GetAll()
        {
            return Ok(_studentService.GetAll());
        }
    }
}
