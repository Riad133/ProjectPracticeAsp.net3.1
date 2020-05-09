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
                                                  
        Task<bool> IsCourseExists(string code);
        
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
            var course =_unitOfWork.CourseRepository.QueryAll().Include(x => x.CourseStudents).FirstOrDefault();
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
            var course = new Course
            {
                Name = request.Name,
                Code = request.Code

            };
            
            _unitOfWork.CourseRepository.UpdateAsync(course);
            if (!await _unitOfWork.ApplicationSaveChangesAsync())
            {
                throw  new MyAppException("Some thing is wrong");
            }

            return course;
        }

        public Task<bool> DeleteAsync(string code)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsCourseExists(string code)
        {
            throw new System.NotImplementedException();
        }
    }
}