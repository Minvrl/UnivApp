using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Univ.Core.Entities;
using Univ.Data;
using Univ.Service.Dtos;
using Univ.Service.Services.Interfaces;

namespace Univ.Service.Services.Implementations
{
    public class GroupService:IGroupService
    {
        private readonly AppDbContext _context;

        public GroupService(AppDbContext context)
        {
            _context = context;
        }

        public int Create(GroupCreateDto dto)
        {
            if (_context.Groups.Any(x => x.No == dto.No))
                throw new Exception();

            Group entity = new Group
            {
                No = dto.No
            };

            _context.Groups.Add(entity);
            _context.SaveChanges();
            return entity.Id;
        }

        public List<GroupGetDto> GetAll()
        {
            return _context.Groups.Select(x => new GroupGetDto
            {
                Id = x.Id,
                No = x.No
            }).ToList();
        }
    }
}
