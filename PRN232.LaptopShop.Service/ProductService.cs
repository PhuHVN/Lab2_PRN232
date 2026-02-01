using Microsoft.EntityFrameworkCore;
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
    public class ProductService
    {
        private readonly UnitOfWork _unitOfWork;

        public ProductService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<BasePaginatedList<ProductResponse>> GetAllProducts(int pageIndex, int pageSize)
        {
            var products = _unitOfWork.productRepo.Entity.Include(x => x.Category);
            var rs = await _unitOfWork.productRepo.GetPagging(products, pageIndex, pageSize);
            var result = rs.Items.Select(x => new ProductResponse
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                Price = x.Price,
                StockQuantity = x.StockQuantity,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.CategoryName,
                CreatedBy = x.CreatedBy,
                CreatedAt = x.CreatedAt
            }).ToList();
            return new BasePaginatedList<ProductResponse>(
                result,
                rs.TotalItems,
                rs.PageIndex,
                rs.PageSize
                );
        }
        public async Task<Product?> GetProductById(int productId)
        {
            var product = await _unitOfWork.productRepo.GetByIdAsync(productId);
            return product;
        }
        public async Task<Product> AddProduct(int categoryId,ProductRequest productRequest)
        {
            var category = await _unitOfWork.categoryRepo.GetByIdAsync(categoryId);
            if (category == null)
            {
                throw new ArgumentException("Category not found");
            }
            //validate product
            if (string.IsNullOrEmpty(productRequest.ProductName))
            {
                throw new ArgumentException("Product name is required");
            }
            if (productRequest.Price < 0)
            {
                throw new ArgumentException("Product price must be greater than or equal to 0");
            }
            if (productRequest.StockQuantity < 0)
            {
                throw new ArgumentException("Stock quantity must be greater than or equal to 0");
            }
            var newProduct = new Product
            {
                ProductName = productRequest.ProductName,
                Price = productRequest.Price,
                StockQuantity = productRequest.StockQuantity,
                CategoryId = categoryId,
                CreatedBy = productRequest.CreatedBy,
                CreatedAt = DateTime.UtcNow

            };
            await _unitOfWork.productRepo.InsertAsync(newProduct);
            await _unitOfWork.SaveChangesAsync();
            return newProduct;
        }
        public async Task UpdateProduct(int id, ProductRequest product)
        {
            var existingProduct = await _unitOfWork.productRepo.GetByIdAsync(id);
            if (existingProduct == null)
            {
                throw new ArgumentException("Product not found");
            }
            var isUpdate = false;
            if (!string.IsNullOrEmpty(product.ProductName) && existingProduct.ProductName != product.ProductName)
            {
                existingProduct.ProductName = product.ProductName;
                isUpdate = true;
            }
            if (product.Price >= 0 && existingProduct.Price != product.Price)
            {
                existingProduct.Price = product.Price;
                isUpdate = true;
            }
            if (product.StockQuantity >= 0 && existingProduct.StockQuantity != product.StockQuantity)
            {
                existingProduct.StockQuantity = product.StockQuantity;
                isUpdate = true;
            }
            if (!isUpdate)
            {
                throw new ArgumentException("No changes detected to update");
            }

            await _unitOfWork.productRepo.UpdateAsync(existingProduct);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteProduct(int productId)
        {
            await _unitOfWork.productRepo.DeleteAsync(productId);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
