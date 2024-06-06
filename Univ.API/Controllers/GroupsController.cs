using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Univ.Service.Dtos;
using Univ.Service.Services.Interfaces;

namespace Univ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost("")]
        public ActionResult Create(GroupCreateDto createDto)
        {
            try
            {
                return StatusCode(201, new { id = _groupService.Create(createDto) });
            }
            catch (Exception e)
            {
                return StatusCode(500, "Bilinmedik bir xeta bas verdi");
            }
        }

        [HttpGet("")]
        public ActionResult<List<GroupGetDto>> GetAll()
        {
            return Ok(_groupService.GetAll());
        }
    }
}
