using admission_task.Dtos;

namespace admission_task.Repos.Interfaces
{
    public interface IModelRepository
    {
        Task<string?> CreateModelAsync(CreatedModelDto model);
        Task<string?> UpdateModelAsync(CreatedModelDto model);
        Task<string?> DeleteModelAsync(string id);
        Task<List<ModelDto>> GetModelsAsync();
        Task<ModelDto> GetModelAsync(string id);



    }
}
