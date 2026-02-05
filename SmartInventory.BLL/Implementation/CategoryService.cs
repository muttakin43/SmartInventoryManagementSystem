using SmartInventory.BLL.Inteface;
using SmartInventory.BLL.Mapping;
using SmartInventory.BLL.Model;
using SmartInventory.Contract.Request;
using SmartInventory.DAL.Interface;
using SmartInventory.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.BLL.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryUnitOfWork _categoryUnitofWork;

        public CategoryService(ICategoryUnitOfWork categoryUnitofWork)
        {
            _categoryUnitofWork = categoryUnitofWork;
        }
        public async Task<Result<int>> AddAsync(CreateCategoryRequest category)
        {
            if (category is null)
            {
                return Result<int>.FailureResult("Category cannot be null");

            }
            var isExist = await _categoryUnitofWork.CategoryRepository
       .IsExistAsync(x => x.CategoryName == category.CategoryName);

            if (isExist)
            {
                return Result<int>.FailureResult("Category already exists");
            }

            try
            {
                var newcategory=category.MapToCategory();
                await _categoryUnitofWork.CategoryRepository.AddAsync(newcategory);
                var saved = await _categoryUnitofWork.SaveChangesAsync();
                if (!saved)
                {
                    return Result<int>.FailureResult("Failed to save category");
                }
                return Result<int>.SuccessResult(newcategory.id);
            }
            catch (Exception)
            {
                return Result<int>.FailureResult("An error occurred while adding the category");
            }

        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
          var Category=await _categoryUnitofWork.CategoryRepository.GetByIdAsync(id);

            if (Category is null)
            {
                return Result<bool>.FailureResult("Category not found");
            }

            await _categoryUnitofWork.CategoryRepository.DeleteAsync(Category);
            var saved = await _categoryUnitofWork.SaveChangesAsync();
            if (!saved)
            {
                return Result<bool>.FailureResult("Failed to delete category");
            }
            return Result<bool>.SuccessResult(true);

        }

        public async Task<Result<IList<Category>>> GetallAsync()
        {
           var Category= await _categoryUnitofWork.CategoryRepository.GetAsync(
               x => x,
        null,
        x => x.OrderByDescending(x => x.id),
        null,
        true);

            return Result<IList<Category>>.SuccessResult(Category);
        }

        public async Task<Result<Category>> GetByIdAsync(int id)
        {
            var category = await _categoryUnitofWork
                .CategoryRepository
                .GetByIdAsync(id);

            if (category == null)
            {
                return Result<Category>.FailureResult($"Category with id {id} not found");
            }

            return Result<Category>.SuccessResult(category);
        }

        public async Task<Result<int>> UpdateAsync(UpdateCategoryRequest model)
        {
            if (model is null)
            {
                return Result<int>.FailureResult("Category cannot be null");
            }
            var Category = await _categoryUnitofWork.CategoryRepository.GetByIdAsync(model.id);
          if (Category is null)
            {
                return Result<int>.FailureResult($"Category with id {model.id} was not found.");
            }

          var isExist = await _categoryUnitofWork.CategoryRepository.IsExistAsync(
              x => x.CategoryName == model.CategoryName && x.id != model.id);

            if (isExist)
            {
                return Result<int>.FailureResult("A Category with the same name already exists.");

            }

            Category.CategoryName = model.CategoryName;
            Category.Description = model.Description;

            await _categoryUnitofWork.CategoryRepository.UpdateAsync(Category);


            var saved = await _categoryUnitofWork.SaveChangesAsync();

            if (!saved)
            {
                return Result<int>.FailureResult("Failed to update category");
            }
            return Result<int>.SuccessResult(model.id);


        }
    }
}
