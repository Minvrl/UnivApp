using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Univ.Service.Dtos;

namespace Univ.Service.Services.Interfaces
{
    public interface IGroupService
    {

        int Create(GroupCreateDto createDto);
        List<GroupGetDto> GetAll();
    }
}
