using Core.Domain.Entities;
using Core.DomainServices.IRepositories;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }



        public IEnumerable<Student> GetStudents()
        {
            return _context.Students;
        }

        public async Task AddStudent(Student newStudent)
        {
            _context.Students.Add(newStudent);
            await _context.SaveChangesAsync();
        }

        public Student GetStudentById(string Id)
        {
            return _context.Students.First(s => s.StudentID == Id);
        }
    }
}
