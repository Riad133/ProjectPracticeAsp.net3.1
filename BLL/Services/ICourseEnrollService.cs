using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Request;
using BLL.Response;
using DLL.Model;
using DLL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Utility.Exceptions;

namespace BLL.Services
{
    public interface ICourseEnrollService
    {
        Task<CourseStudent> AddCourseEnrollAsync(CourseEnrollRequest request);
        Task<List<CourseStudent>> GetAllEnrollCourseAsync();
        Task<StudentCourseEnrollResponse> GetAStudentReport(string rollno);
        Task<List<StudentCourseEnrollResponse>> GetAllStudentReport();
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
          
            var courseStudentEnroll =
             await   _unitOfWork.CourseStudentEnrollRepository.GetAAsync(x =>
                    x.CourseId == courseId && x.StudentId == studentId);
            if (courseStudentEnroll != null)
            {
                throw new MyAppException("Student Already enroll in this course");
            }
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

        public async Task<StudentCourseEnrollResponse> GetAStudentReport(string rollno)
        {
            var student = await _unitOfWork.StudentRepository.GetAAsync(x => x.RollNo == rollno);
            if (student == null)
            {
                throw  new MyAppException("No data Found");
            }

            var courses = await _unitOfWork.CourseStudentEnrollRepository
                .QueryAll(x => x.StudentId == student.StudentId)
                .Include(x => x.Course).Select(x => x.Course).ToListAsync();
           var studentCourseEnroll = new StudentCourseEnrollResponse
           {
               Student =  student,
               Courses = courses
           };
           return studentCourseEnroll;
        }

        public async Task<List<StudentCourseEnrollResponse>> GetAllStudentReport()
        {
            var studentRolls = await _unitOfWork.StudentRepository.QueryAll().Select(x => x.RollNo).ToListAsync();
            var studentCourseEnrollResponseList = new List<StudentCourseEnrollResponse>();
            if (studentRolls == null)
            {
                throw  new MyAppException("No data Found");
            }
            foreach (var student in studentRolls)
            {
                var studentReport = await GetAStudentReport(student);
                studentCourseEnrollResponseList.Add(studentReport);
            }

            return studentCourseEnrollResponseList;
        }
    }
}