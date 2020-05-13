using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Request;
using DLL.Model;
using DLL.UnitOfWork;
using Utility.Exceptions;

namespace BLL.Services
{
    public interface ICourseEnrollService
    {
        Task<CourseStudent> AddCourseEnrollAsync(CourseEnrollRequest request);
        Task<List<CourseStudent>> GetAllEnrollCourseAsync();
    }
    public class CourseEnrollService: ICourseEnrollService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseEnrollService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }

        public async Task<CourseStudent> AddCourseEnrollAsync(CourseEnrollRequest request)
        {
            Guid id = Guid.NewGuid();
            var studentId =  _unitOfWork.StudentRepository.GetAAsync(x => x.RollNo == request.studentRollNo).Result
                .StudentId;
            var courseId = _unitOfWork.CourseRepository.GetAAsync(x => x.Code == request.CourseCode).Result.CourseId;
            var courseStudent = new CourseStudent
            {
                CourseStudentId= id.ToString(),
                StudentId = studentId,
                CourseId =  courseId
            };
          await  _unitOfWork.CourseStudentEnrollRepository.createAsync(courseStudent);
          if (await _unitOfWork.ApplicationSaveChangesAsync())
          {
              return courseStudent;
          }
          throw new MyAppException("Something went wrong");
        }

        public async Task<List<CourseStudent>> GetAllEnrollCourseAsync()
        {
            var courseStudents = await _unitOfWork.CourseStudentEnrollRepository.GetListAsync();
            if (courseStudents == null)
            {
                throw  new MyAppException("No data Found");
            }

            return courseStudents;
        }
    }
}