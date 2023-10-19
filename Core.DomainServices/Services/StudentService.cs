using Core.Domain.Entities;
using Core.DomainServices.IRepositories;
using Core.DomainServices.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task AddStudent(Student student)
        {
            try
            {
                await _studentRepository.AddStudent(student);
            }
            catch
            {
                throw new Exception("Student could not be registered");
            }
        }

        public Student GetStudentById(string id)
        {
            try
            {
                return _studentRepository.GetStudentById(id);
            }
            catch
            {
                throw new Exception("Student could not be found");
            }
        }
    }
}
