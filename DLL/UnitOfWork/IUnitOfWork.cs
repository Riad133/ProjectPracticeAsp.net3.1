using System;
using System.Threading.Tasks;
using DLL.DbContext;
using DLL.Repository;

namespace DLL.UnitOfWork
{
    public interface IUnitOfWork
    {
        //all repository need to added here open

        #region All My Repoistory 

        IDepartmentRepository DepartmentRepository { get; }
        IStudentRepository StudentRepository { get; }

            #endregion
        //all repository need to added here end 

        Task<bool> ApplicationSaveChangesAsync();
        void Dispose();
    }

    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private IDepartmentRepository _departmentRepository;
        private IStudentRepository _studentRepository;

        public IDepartmentRepository DepartmentRepository =>
            _departmentRepository ??= new DepartmentRepository(_context);

        public IStudentRepository StudentRepository => 
            _studentRepository ??= new StudentRepository(_context);
        
        private readonly ApplicationDbContext _context;
        private bool disposed = false;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> ApplicationSaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            this.disposed = true;
        }
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}