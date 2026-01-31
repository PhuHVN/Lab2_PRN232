using Microsoft.EntityFrameworkCore.Storage;
using PRN232.LaptopShop.Repository.Entities;
using PRN232.LaptopShop.Repository.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.LaptopShop.Repository
{
    public class UnitOfWork
    {
        private readonly ShopDbContext _context;
        private IDbContextTransaction? _transaction;
        private Dictionary<Type, object> repositories;

        public AccountRepo accountRepo { get; set; }
        public CategoryRepo categoryRepo { get; set; }
        public ProductRepo productRepo { get; set; }
        public UnitOfWork(ShopDbContext context, AccountRepo accountRepo, CategoryRepo categoryRepo,ProductRepo productRepo)
        {
            _context = context;
            repositories = new Dictionary<Type, object>();
            this.accountRepo = accountRepo;
            this.categoryRepo = categoryRepo;
            this.productRepo = productRepo;
        }

        public async Task SaveChangesAsync()
        {
            if (_transaction == null)
                await _context.SaveChangesAsync();
        }

    }
}
