using Microsoft.EntityFrameworkCore;
using plato_backend.Context;
using plato_backend.Model;

namespace plato_backend.Services
{
    public class IngredientsServices
    {
        private readonly DataContext _dataContext;

        public IngredientsServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<IngredientsModel>> GetIngredientsAsync()
        {
            return await _dataContext.Ingredients.ToListAsync();
        }

        public async Task<bool> AddIngredientsAsync(IngredientsModel ingredients)
        {
            await _dataContext.Ingredients.AddAsync(ingredients);

            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> EditIngredientsAsync(IngredientsModel ingredients)
        {
            var ingredientsToEdit = await GetIngredientsByIdAsync(ingredients.Id);

            if (ingredientsToEdit == null) return false;
            
            ingredientsToEdit.BlogId = ingredients.BlogId;
            ingredientsToEdit.Title = ingredients.Title;
            ingredientsToEdit.Ingredients = ingredients.Ingredients;

            _dataContext.Ingredients.Update(ingredientsToEdit);
            
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<IngredientsModel> GetIngredientsByIdAsync(int id)
        {
            return (await _dataContext.Ingredients.FindAsync(id))!;
        }

        public async Task<List<IngredientsModel>> GetIngredientsByBlogIdAsync(int blogId)
        {
            return await _dataContext.Ingredients.Where(ingredients => ingredients.BlogId == blogId).ToListAsync();
        }
    }
}