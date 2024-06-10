using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Univ.Core.Entities;
using Univ.Data;
using Univ.Service.Dtos;
using Univ.Service.Services.Interfaces;

namespace Univ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly AppDbContext _context;

        public GroupsController(IGroupService groupService,AppDbContext context)
        {
            _groupService = groupService;
            _context = context;
        }

        [HttpPost("")]
        public ActionResult Create(GroupCreateDto createDto)
        {
            if (_context.Groups.Any(x => x.No == createDto.No && !x.IsDeleted))
                return StatusCode(409);

            var entity = new Group
            {
                Limit = createDto.Limit,
                No = createDto.No,
            };

            _context.Groups.Add(entity);
            _context.SaveChanges();


            return StatusCode(201, new { Id = entity.Id });
        }
        [HttpGet("")]
        public ActionResult<List<GroupGetDto>> GetAll()
        {
            return Ok(_groupService.GetAll());
        }


        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody] GroupUpdateDto updateDto)
        {
            var group = _context.Groups.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (group == null) return NotFound();

            if (group.No != updateDto.No && _context.Groups.Any(x => x.No == updateDto.No && !x.IsDeleted))
                return Conflict();

            group.No = updateDto.No;
            group.Limit = updateDto.Limit;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var group = _context.Groups.FirstOrDefault(x => x.Id == id && !x.IsDeleted);

            if (group == null) return NotFound();

            group.IsDeleted = true;
            _context.SaveChanges();

            return NoContent();
        }
    }
}
