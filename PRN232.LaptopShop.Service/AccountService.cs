using Microsoft.Extensions.Configuration;
using PRN232.LaptopShop.Repository;
using PRN232.LaptopShop.Repository.Entity;
using PRN232.LaptopShop.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.LaptopShop.Service
{
    public class AccountService
    {
        private readonly UnitOfWork _unitOfWork;
        public AccountService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public async Task<AccountResponse> CreateAccount(AccountRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.PasswordHash) || string.IsNullOrEmpty(request.Email))
            {
                throw new ArgumentException("Username, PasswordHash, and Email are required.");
            }
            var existingAccount = await _unitOfWork.accountRepo.FindAsync(a => a.Username == request.Username || a.Email == request.Email);
            var passwordHasing = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);
            var account = new Account
            {
                Username = request.Username,
                PasswordHash = passwordHasing,
                Email = request.Email,
                Role = request.Role
            };
            await _unitOfWork.accountRepo.InsertAsync(account);
            await _unitOfWork.SaveChangesAsync();
            return new AccountResponse
            {
                Username = account.Username,
                Email = account.Email,
                Role = account.Role,
                CreatedAt = account.CreatedAt
            };
        }

        public async Task<AccountResponse> GetAccountById(int accountId)
        {
            var account = await _unitOfWork.accountRepo.FindAsync(x => x.AccountId == accountId);
            if (account == null)
            {
                throw new KeyNotFoundException("Account not found.");
            }
            return new AccountResponse
            {
                AccountId = account.AccountId,
                Username = account.Username,
                Email = account.Email,
                Role = account.Role,
                CreatedAt = account.CreatedAt
            };
        }
        public async Task<AccountResponse> UpdateAccount(int accountId, AccountRequest request)
        {
            var account = await _unitOfWork.accountRepo.GetByIdAsync(accountId);
            if (account == null)
            {
                throw new KeyNotFoundException("Account not found.");
            }
            var isUpdated = false;
            if (!string.IsNullOrEmpty(request.Username) && request.Username != account.Username)
            {
                account.Username = request.Username;
                isUpdated = true;
            }
            if (!string.IsNullOrEmpty(request.Email) && request.Email != account.Email)
            {
                account.Email = request.Email;
                isUpdated = true;
            }
            if (!string.IsNullOrEmpty(request.Role) && request.Role != account.Role)
            {
                account.Role = request.Role;
                isUpdated = true;
            }
            if (isUpdated)
            {
                await _unitOfWork.accountRepo.UpdateAsync(account);
                await _unitOfWork.SaveChangesAsync();
            }
            return new AccountResponse
            {
                AccountId = account.AccountId,
                Username = account.Username,
                Email = account.Email,
                Role = account.Role,
                CreatedAt = account.CreatedAt
            };
        }
        public async Task<BasePaginatedList<AccountResponse>> GetAllAccount(int pageIndex, int pageSize)
        {
            var query = _unitOfWork.accountRepo.Entity;
            var rs = await _unitOfWork.accountRepo.GetPagging(query, pageIndex, pageSize);
            var accMap = rs.Items.Select(account => new AccountResponse
            {
                AccountId = account.AccountId,
                Username = account.Username,
                Email = account.Email,
                Role = account.Role,
                CreatedAt = account.CreatedAt
            }).ToList();
            return new BasePaginatedList<AccountResponse>
                (
                accMap,
                rs.TotalItems,
                rs.PageIndex,
                rs.PageSize
                );
        }

        public async Task DeleteAccount(int accountId)
        {
            var account = await _unitOfWork.accountRepo.GetByIdAsync(accountId);
            if (account == null)
            {
                throw new KeyNotFoundException("Account not found.");
            }
            await _unitOfWork.accountRepo.DeleteAsync(account);
            await _unitOfWork.SaveChangesAsync();

        }
    }
}
