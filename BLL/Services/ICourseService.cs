using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Request;
using DLL.Model;
using DLL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Utility.Exceptions;

namespace BLL.Services
{
    public interface ICourseService
    {
        Task<Course> AddCourseAsync(CourseInsertRequest request);
        Task<List<Course>> GetAllCourseAsync();
        Task<Course> GetACourseAsync(string code);
        Task<Course> UpdateAsync(string code, CourseInsertRequest request);
        Task<bool> DeleteAsync(string code);
                                                  
       
        Task<bool> IsCodeExists(string code);
        Task<bool> IsNameExists(string name);
        
    }
    public  class  CourseService : ICourseService{
        private readonly IUnitOfWork _unitOfWork;

        public CourseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Course> AddCourseAsync(CourseInsertRequest request)
        {
           var course = new Course
           {
               Code = request.Code,
               Name = request.Name
           };
           await _unitOfWork.CourseRepository.createAsync(course);
           if (await _unitOfWork.ApplicationSaveChangesAsync())
           {
               return course;
           }
           throw new MyAppException("Something went wrong");
        }

        public Task<List<Course>> GetAllCourseAsync()
        {
            var courses =_unitOfWork.CourseRepository.GetListAsync();
            if (courses == null)
            {
                throw new MyAppException("No data found");
            }

            return courses;
        }

        public async Task<Course> GetACourseAsync(string code)
        {
            var course =await _unitOfWork.CourseRepository.QueryAll().Where(x => x.Code== code).Include(x => x.CourseStudents).FirstOrDefaultAsync();
            if (course == null)
            {
                throw new MyAppException("No data found");
            }

            return course;
        }

        public async Task<Course> UpdateAsync(string code, CourseInsertRequest request)
        {
            var acourse = await _unitOfWork.CourseRepository.GetAAsync(x => x.Code== code);
            if (acourse == null)
            {
                throw  new MyAppException("No data found");
            }
           
            acourse.Name = request.Name;
            acourse.Code = request.Code;
            
            _unitOfWork.CourseRepository.UpdateAsync(acourse);
            if (!await _unitOfWork.ApplicationSaveChangesAsync())
            {
                throw  new MyAppException("Some thing is wrong");
            }

            return acourse;
        }

        public Task<bool> DeleteAsync(string code)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> IsNameExists(string name)
        {
            var department  = await _unitOfWork.CourseRepository.GetAAsync(x => x.Name == name);
            if (department != null)
            {
                return true;
            }

            return true;
        }
        public async Task<bool> IsCodeExists(string code)
        {
            var department  = await _unitOfWork.CourseRepository.GetAAsync(x => x.Code == code);
            if (department != null)
            {
                return true;
            }
            return true;
        }
    }
}