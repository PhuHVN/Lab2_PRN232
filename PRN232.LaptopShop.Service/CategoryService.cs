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
    public class CategoryService
    {
        private readonly UnitOfWork unitOfWork;
        public CategoryService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<BasePaginatedList<Category>> GetAllCategories(int pageIndex, int pageSize)
        {
            var categories = unitOfWork.categoryRepo.Entity;
            var rs = await unitOfWork.categoryRepo.GetPagging(categories, pageIndex, pageSize);
            return rs;
        }
        public async Task<Category?> GetCategoryById(int id)
        {
            return await unitOfWork.categoryRepo.GetByIdAsync(id);
        }
        public async Task<Category> AddCategory(CategoryRequest category)
        {
            var newCategory = new Category
            {
                CategoryName = category.CategoryName,
                Description = category.Description,
                CreatedAt = DateTime.Now
            };
            await unitOfWork.categoryRepo.InsertAsync(newCategory);
            await unitOfWork.SaveChangesAsync();
            return newCategory;
        }
        public async Task<Category?> UpdateCategory(int id,CategoryRequest category)
        {
            var existingCategory = await unitOfWork.categoryRepo.GetByIdAsync(id);
            if (existingCategory == null)
            {
                return null;
            }
            existingCategory.CategoryName = category.CategoryName;
            existingCategory.Description = category.Description;
            await unitOfWork.categoryRepo.UpdateAsync(existingCategory);
            await unitOfWork.SaveChangesAsync();
            return existingCategory;
        }
        public async Task<bool> DeleteCategory(int id)
        {
            var existingCategory = await unitOfWork.categoryRepo.GetByIdAsync(id);
            if (existingCategory == null)
            {
                return false;
            }
            await unitOfWork.categoryRepo.DeleteAsync(existingCategory);
            await unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
