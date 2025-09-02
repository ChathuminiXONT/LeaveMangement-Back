//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LMS.Domain.Interfaces
//{
//    public interface IUnitOfWork : IDisposable
//    {
//        ILeaveApprovalRepository LeaveApprovalRepository { get; }
//        Task<int> SaveChangesAsync();
//        Task BeginTransactionAsync();
//        Task CommitTransactionAsync();
//        Task RollbackTransactionAsync();
//    }
//}
