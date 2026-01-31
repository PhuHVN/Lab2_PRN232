using PRN232.LaptopShop.Repository;
using PRN232.LaptopShop.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.LaptopShop.Repository.Repo
{
    public class ProductRepo : RepositoryBase<Product>
    {
        public ProductRepo(ShopDbContext context) : base(context)
        {
        }
    
    }
}
