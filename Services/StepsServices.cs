using Microsoft.EntityFrameworkCore;
using plato_backend.Context;
using plato_backend.Model;

namespace plato_backend.Services
{
    public class StepsServices
    {
        private readonly DataContext _dataContext;

        public StepsServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<StepsModel>> GetStepsAsync()
        {
            return await _dataContext.Steps.ToListAsync();
        }

        public async Task<bool> AddStepsAsync(StepsModel steps)
        {
            await _dataContext.Steps.AddAsync(steps);

            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> EditStepsAsync(StepsModel steps)
        {
            var stepsToEdit = await GetStepsByIdAsync(steps.Id);

            if (stepsToEdit == null) return false;
            
            stepsToEdit.BlogId = steps.BlogId;
            stepsToEdit.Title = steps.Title;
            stepsToEdit.Steps = steps.Steps;

            _dataContext.Steps.Update(stepsToEdit);
            
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<StepsModel> GetStepsByIdAsync(int id)
        {
            return (await _dataContext.Steps.FindAsync(id))!;
        }

        public async Task<List<StepsModel>> GetStepsByBlogIdAsync(int blogId)
        {
            return await _dataContext.Steps.Where(steps => steps.BlogId == blogId).ToListAsync();
        }
    }
}