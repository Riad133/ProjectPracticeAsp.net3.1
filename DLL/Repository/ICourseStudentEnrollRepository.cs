using DLL.DbContext;
using DLL.Model;
using DLL.UnitOfWork;

namespace DLL.Repository
{
    public interface ICourseStudentEnrollRepository  : IRepositoryBase<CourseStudent>
    {
    
    }
    public class CourseStudentEnrollRepository:RepositoryBase<CourseStudent>, ICourseStudentEnrollRepository
    {
        public CourseStudentEnrollRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}