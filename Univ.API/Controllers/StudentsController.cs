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

        public StudentsController(IStudentService studentService, AppDbContext context)
        {
            _studentService = studentService;
            _context = context;
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

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] StudentUpdateDto updateDto)
        {
            var student = _context.Students.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (student == null) return NotFound();

            if (student.Email != updateDto.Email && _context.Students.Any(x => x.Email == updateDto.Email && !x.IsDeleted))
                return Conflict();
            if (student.GroupId != updateDto.GroupId && _context.Groups.Any(x => x.Id == updateDto.GroupId && x.IsDeleted))
                return Conflict();

            student.Fullname = updateDto.FullName;
            student.BirthDate = updateDto.BirthDate;
            student.Email = updateDto.Email;
            student.GroupId = updateDto.GroupId;

            _context.SaveChanges();
            return NoContent();

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var student = _context.Students.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (student == null) return NotFound();
            student.IsDeleted = true;
            _context.SaveChanges();
            return NoContent();
        }


    }
}
