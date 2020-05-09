using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Request;
using DLL.Model;
using DLL.Repository;
using DLL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Utility.Exceptions;

namespace BLL.Services
{
    public interface IDepartmentService
    {
        
        Task<Department> AddDepartmentAsync(DepartmentInsertRequest request);
        Task<List<Department>> GetAllDepartmentAsync();
        Task<Department> GetADepartmentAsync(string code);
        Task<Department> UpdateAsync(string code, DepartmentInsertRequest request);
        Task<bool> DeleteAsync(string code);
                                                  
        Task<bool> IsCodeExists(string code);
        Task<bool> IsNameExists(string name);
        Task<bool> IsDepartmentIdExists(int id);  
        
    }

    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
       

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
          
        }
        public async  Task<Department> AddDepartmentAsync(DepartmentInsertRequest request)
        {
           Department department = new Department
           {
               Name = request.Name,
               Code = request.Code
           };
           await _unitOfWork.DepartmentRepository.createAsync(department);
           if (await _unitOfWork.ApplicationSaveChangesAsync())
           {
               return department;
           }
           throw new MyAppException("Something went wrong");

           
        }

        public async Task<List<Department>> GetAllDepartmentAsync()
        {
            var departments =  _unitOfWork.DepartmentRepository.QueryAll().Include(x => x.Students).ToList();
            if (departments == null)
            {
                throw new MyAppException("Department not found");
            }

            return departments;
        }

        public async Task<Department> GetADepartmentAsync(string code)
        {
            var department =  _unitOfWork.DepartmentRepository.QueryAll().Where(x => x.Code==code).Include(x => x.Students).FirstOrDefault();
            if (department == null)
            {
                throw new MyAppException("Department not found");
            }

            return department; 
        }

        public async Task<Department> UpdateAsync(string code, DepartmentInsertRequest request)
        {
            var getADepartmentData = await _unitOfWork.DepartmentRepository.GetAAsync(x => x.Code == code);
            if (getADepartmentData == null)
            {
                throw new MyAppException("Department not found");
            }
            if (!string.IsNullOrWhiteSpace(request.Code) && !string.IsNullOrWhiteSpace(request.Name))
            {
                getADepartmentData.Code = request.Code;
                getADepartmentData.Name = request.Name;
                _unitOfWork.DepartmentRepository.UpdateAsync(getADepartmentData);
            }


            if (await _unitOfWork.ApplicationSaveChangesAsync())
            {
                return getADepartmentData;
            }

            throw new MyAppException("something went wrong");
            
        }

        public Task<bool> DeleteAsync(string code)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> IsNameExists(string name)
        {
            var department  = await _unitOfWork.DepartmentRepository.GetAAsync(x => x.Name == name);
            if (department != null)
            {
                return true;
            }

            return true;
        }
        public async Task<bool> IsCodeExists(string code)
        {
            var department  = await _unitOfWork.DepartmentRepository.GetAAsync(x => x.Code == code);
            if (department != null)
            {
                return true;
            }
            return true;
        }
        public async Task<bool> IsDepartmentIdExists(int id)
        {
            var department  = await _unitOfWork.DepartmentRepository.GetAAsync(x => x.DepartmentId == id);
            if (department != null)
            {
                return true;
            }
            return true;
        }
    }
}