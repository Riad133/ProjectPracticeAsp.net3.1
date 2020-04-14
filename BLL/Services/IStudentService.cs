using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Request;
using DLL.DbContext;
using DLL.Model;
using DLL.Repository;
using DLL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Utility.Exceptions;

namespace BLL.Services
{
    public interface IStudentService
    {
        Task<bool> IsEmailExists(string email);
        Task<Student> AddStudentAsync(StudentInsertRequest astudent);
        Task<List<Student>> GetAllStudentAsync();
        Task<Student> GetAStudentAsync(string rollNo);
        Task<bool> IsRollNoExists(string rollNo);
         Task<Student> UpdateStudentAsync(string rollNo,StudentUpdateRequest aStudent);
    }

    public class StudentService:IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
       


        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
          
        }
        public async Task<bool> IsEmailExists(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            var student= await _unitOfWork.StudentRepository.GetAAsync(x => x.Email==email);
            if (student == null)
            {
                return false;
            }

            return true;
        }

        public async Task<Student> AddStudentAsync(StudentInsertRequest astudent)
        {
            var student = new Student
            {
                Name = astudent.Name,
                Email = astudent.Email,
                RollNo = astudent.RollNo,
                DepartmentId = astudent.DepartmentId
                
            };
             await _unitOfWork.StudentRepository.createAsync(student);
             if (!await _unitOfWork.ApplicationSaveChangesAsync())
             {
                 throw new MyAppException("Something is wrong");
             }

             return student;
        }

        public async Task<List<Student>> GetAllStudentAsync()
        {
            return await _unitOfWork.StudentRepository.GetListAsync();
        }

        public async  Task<Student> GetAStudentAsync(string rollNo)
        {
             var student =await _unitOfWork.StudentRepository.GetAAsync(x => x.RollNo==rollNo);
             if (student == null)
             {
                 throw  new MyAppException("data not found");
             }
             return student;
        }

        public async Task<bool> IsRollNoExists(string rollNo)
        {
            var student = await _unitOfWork.StudentRepository.GetAAsync(x => x.RollNo==rollNo);
            if (student == null)
            {
                return false;
                
            }

            return true;
        }

        public async Task<Student> UpdateStudentAsync(string rollNo, StudentUpdateRequest request)
        {
            var astudent = await _unitOfWork.StudentRepository.GetAAsync(x => x.RollNo== rollNo);
            if (astudent == null)
            {
                throw new Exception(); 
            }
            Student student = new Student
            {
                Name = request.Name,
                Email = request.Email,
                RollNo = request.RollNo,
                DepartmentId = request.DepartmentId
                
            };
            
            _unitOfWork.StudentRepository.UpdateAsync(student);
              if (!await _unitOfWork.ApplicationSaveChangesAsync())
              {
                  throw  new MyAppException("Some thing is wrong");
              }

              return student;
        }
    }
}