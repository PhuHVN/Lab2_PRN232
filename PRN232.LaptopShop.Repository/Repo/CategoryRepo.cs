using PRN232.LaptopShop.Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.LaptopShop.Repository.Repo
{
    public class CategoryRepo : RepositoryBase<Category>
    {
        public CategoryRepo(ShopDbContext context) : base(context)
        {
        }
    }
}
