//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Storage;
//using Microsoft.Extensions.Logging;
//using LMS.Domain.Interfaces;
//using LMS.Infrastructure.Data;

//namespace LMS.Infrastructure.Repositories
//{
//    public class UnitOfWork : IUnitOfWork
//    {
//        private readonly LMSDbContext _context;
//        private readonly ILogger<UnitOfWork> _logger;
//        private readonly ILeaveApprovalRepository _leaveApprovalRepository;
//        private IDbContextTransaction _transaction;
//        private bool _disposed = false;

//        public UnitOfWork(LMSDbContext context, ILogger<UnitOfWork> logger, ILeaveApprovalRepository leaveApprovalRepository)
//        {
//            _context = context;
//            _logger = logger;
//            _leaveApprovalRepository = leaveApprovalRepository;
//        }

//        public ILeaveApprovalRepository LeaveApprovalRepository => _leaveApprovalRepository;

//        public async Task<int> SaveChangesAsync()
//        {
//            try
//            {
//                var result = await _context.SaveChangesAsync();
//                _logger.LogInformation($"SaveChangesAsync completed. {result} entities saved.");
//                return result;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error occurred while saving changes");
//                throw;
//            }
//        }

//        public async Task BeginTransactionAsync()
//        {
//            try
//            {
//                if (_transaction != null)
//                {
//                    _logger.LogWarning("Transaction already exists. Disposing previous transaction.");
//                    await _transaction.DisposeAsync();
//                }

//                _transaction = await _context.Database.BeginTransactionAsync();
//                _logger.LogInformation("Database transaction started");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error starting database transaction");
//                throw;
//            }
//        }

//        public async Task CommitTransactionAsync()
//        {
//            try
//            {
//                if (_transaction == null)
//                {
//                    _logger.LogWarning("No active transaction to commit");
//                    return;
//                }

//                await _context.SaveChangesAsync();
//                await _transaction.CommitAsync();
//                _logger.LogInformation("Database transaction committed successfully");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error committing database transaction");
//                await RollbackTransactionAsync();
//                throw;
//            }
//            finally
//            {
//                if (_transaction != null)
//                {
//                    await _transaction.DisposeAsync();
//                    _transaction = null;
//                }
//            }
//        }

//        public async Task RollbackTransactionAsync()
//        {
//            try
//            {
//                if (_transaction != null)
//                {
//                    await _transaction.RollbackAsync();
//                    _logger.LogInformation("Database transaction rolled back");
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error rolling back database transaction");
//                throw;
//            }
//            finally
//            {
//                if (_transaction != null)
//                {
//                    await _transaction.DisposeAsync();
//                    _transaction = null;
//                }
//            }
//        }

//        public void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }

//        protected virtual void Dispose(bool disposing)
//        {
//            if (!_disposed && disposing)
//            {
//                try
//                {
//                    if (_transaction != null)
//                    {
//                        _transaction.Dispose();
//                        _transaction = null;
//                    }

//                    _context?.Dispose();
//                    _logger.LogInformation("UnitOfWork disposed successfully");
//                }
//                catch (Exception ex)
//                {
//                    _logger.LogError(ex, "Error disposing UnitOfWork");
//                }
//                finally
//                {
//                    _disposed = true;
//                }
//            }
//        }
//    }
//}