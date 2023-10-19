using Core.Domain.Entities;

namespace Core.DomainServices.IServices
{
    public interface IStudentService
    {
        Student GetStudentById(string id);

        Task AddStudent(Student student);
    }
}