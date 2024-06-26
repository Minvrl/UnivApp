﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Univ.Core.Entities;
using Univ.Data;
using Univ.Service.Dtos;
using Univ.Service.Exceptions;
using Univ.Service.Services.Interfaces;

namespace Univ.Service.Services.Implementations
{
    public class StudentService:IStudentService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public StudentService(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public int Create(StudentCreateDto createDto)
        {
            Group group = _context.Groups.Include(x => x.Students).FirstOrDefault(x => x.Id == createDto.GroupId && !x.IsDeleted);

            if (group == null)
                throw new RestException(StatusCodes.Status404NotFound, "GroupId", "Group not found by given GroupId");

            if (group.Limit <= group.Students.Count)
                throw new RestException(StatusCodes.Status400BadRequest, "Group limit reached");

            if (_context.Students.Any(x => x.Email.ToUpper() == createDto.Email.ToUpper() && !x.IsDeleted))
                throw new RestException(StatusCodes.Status400BadRequest, "Email", "Student already exists by given Email");



            Student entity = new Student
            {
                Fullname = createDto.FullName,
                Email = createDto.Email,
                BirthDate = createDto.BirthDate,
                GroupId = createDto.GroupId,
            };

            _context.Students.Add(entity);
            _context.SaveChanges();

            return entity.Id;
        }

        public List<StudentGetDto> GetAll()
        {
            return _context.Students.Where(x=> !x.IsDeleted).Select(x => new StudentGetDto
            {
                Id = x.Id,
                Fullname = x.Fullname,
                GroupId = x.GroupId,
            }).ToList();
        }
    }
}
